// --------------------------------------------------------------------
// <copyright file="GenerationsController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using PowerCampus.Entities.ElectronicCertificate;
using PowerCampus.Entities.ElectronicDegree;
using Resources;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Areas.ElectronicCertificate.Mappers;
using WebAdminUI.Areas.ElectronicCertificate.Models.Generation;
using WebAdminUI.Areas.ElectronicCertificate.Models.Shared;
using WebAdminUI.Areas.General.Mappers;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicCertificate.Controllers
{
    /// <summary>
    /// GenerationsController
    /// </summary>
    /// <seealso cref="WebAdminUI.Controllers.BaseController" />
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicCertificate)]
    [PermissionRequired(PermissionName = PermissionsConstants.GenerateElectronicCertificate)]
    public class GenerationsController : WebAdminUI.Controllers.BaseController
    {
        /// <summary>
        /// Adds the course.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="rvoeId">The rvoe identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> _AddCourse(string peopleId, int rvoeId)
        {
            try
            {
                AcademicPlanCourse academicPlan = new AcademicPlanCourse();
                CoursesViewModel coursesViewModel = new CoursesViewModel();

                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcCoursesOutsideAcademicPlan}{peopleId}/{rvoeId}");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    academicPlan = JsonConvert.DeserializeObject<AcademicPlanCourse>(content);
                    return PartialView("_AddCourse", academicPlan.ToViewModel());
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "GenerationsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Generateds the table.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> _GeneratedTable(Search search)
        {
            try
            {
                List<ElectronicCertificateViewModel> electronicCertificatesModelList = new List<ElectronicCertificateViewModel>();
                if (Account.status == enumAccess.Authorized)
                {
                    search.Status = CertificateStatus.Generated;
                    string json = JsonConvert.SerializeObject(search);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage resp = null;
                    if (search.Advanced)
                        resp = await client.PostAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesAdvancedSearch}", httpContent);
                    else
                        resp = await client.PostAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesBasicSearch}", httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await resp.Content.ReadAsStringAsync();
                        electronicCertificatesModelList = JsonConvert.DeserializeObject<List<Certificate>>(content).ToViewModel(true);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.StatusCode.ToString());
                        LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.ReasonPhrase);
                        return RedirectToRoute("ErrorUnauthorized");
                    }
                }
                return PartialView("_GeneratedTable", electronicCertificatesModelList);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Views the data.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> _ViewData(int id)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesDetail}{id}");
                ElectronicCertificateInfoViewModel electronicCertificateInfoViewModel = new ElectronicCertificateInfoViewModel();
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    electronicCertificateInfoViewModel = JsonConvert.DeserializeObject<CertificateInfo>(content).ToViewModel();
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                return PartialView(electronicCertificateInfoViewModel);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Generates this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Generate()
        {
            try
            {
                bool canGenerateNew = false;
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage respValidation = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcValidateOperatorCampus}{Account.UserName}");
                if (respValidation.StatusCode == HttpStatusCode.OK)
                {
                    string contentValidation = await respValidation.Content.ReadAsStringAsync();
                    canGenerateNew = JsonConvert.DeserializeObject<bool>(contentValidation);
                }
                if (canGenerateNew)
                {
                    return View("Generate", new GenerationViewModel
                    {
                        StudiesProgramDetailViewModel = new StudiesProgramDetailViewModel
                        {
                            StudiesProgramMajor = new StudiesProgramMajorViewModel(),
                            StudiesProgramResponsible = new StudiesProgramResponsibleViewModel(),
                            StudiesProgramRvoe = new StudiesProgramRvoeViewModel()
                        },
                        CoursesViewModel = new CoursesViewModel(),
                        IssuingCertificateViewModel = new IssuingCertificateViewModel()
                    });
                }
                else
                {
                    return RedirectToRoute("ErrorUnauthorized");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                ElectronicCertificateHeaderList electronicCertificateList = new ElectronicCertificateHeaderList();
                electronicCertificateList.CanGenerateNew = false;

                electronicCertificateList.ElectronicCertificateParameters = new ElectronicCertificateParameters();
                electronicCertificateList.ElectronicCertificateParameters.ShowAllStatus = false;

                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                string content;

                HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcValidateOperatorCampus}{Account.UserName}");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    content = await resp.Content.ReadAsStringAsync();
                    electronicCertificateList.CanGenerateNew = JsonConvert.DeserializeObject<bool>(content);
                }

                resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesCampuses}G");
                List<DropdownListViewModel> campuses = null;
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    content = await resp.Content.ReadAsStringAsync();
                    campuses = JsonConvert.DeserializeObject<List<CodeTable>>(content).ToViewModel();
                }
                electronicCertificateList.ElectronicCertificateParameters.Campus = campuses ?? new List<DropdownListViewModel>();

                resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesCertificationTypes}G");
                List<DropdownListViewModel> certificationTypes = null;
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    content = await resp.Content.ReadAsStringAsync();
                    certificationTypes = JsonConvert.DeserializeObject<List<CodeTable>>(content).ToViewModel();
                }
                electronicCertificateList.ElectronicCertificateParameters.CertificationType = certificationTypes ?? new List<DropdownListViewModel>();

                resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesMajors}G");
                List<DropdownListViewModel> majors = null;
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    content = await resp.Content.ReadAsStringAsync();
                    majors = JsonConvert.DeserializeObject<List<CodeTable>>(content).ToViewModel();
                }
                electronicCertificateList.ElectronicCertificateParameters.Program = majors ?? new List<DropdownListViewModel>();

                return View(electronicCertificateList);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", ex.Message);
                throw;
            }
        }

        #region JSON

        /// <summary>
        /// Courseses the specified people identifier.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="rvoeId">The rvoe identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> Courses(string peopleId, int rvoeId)
        {
            try
            {
                AcademicPlanCourse academicPlan = new AcademicPlanCourse();
                CoursesViewModel coursesViewModel = new CoursesViewModel();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcAcademicPlanCourseByStudent}{peopleId}/{rvoeId}");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    academicPlan = JsonConvert.DeserializeObject<AcademicPlanCourse>(content);
                    return Json(new
                    {
                        coursesViewModel = academicPlan.ToViewModel(),
                    }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController - Courses", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Creates the specified certificate information.
        /// </summary>
        /// <param name="certificateInfo">The certificate information.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(CertificateInfo certificateInfo)
        {
            try
            {
                if (certificateInfo != null)
                {
                    certificateInfo.OperatorId = Account.UserName;
                    string certificateInfoJson = JsonConvert.SerializeObject(certificateInfo);
                    HttpResponseMessage response = await GetResponseAsync(HttpVerbs.Post, certificateInfoJson, ApiRoute.EcCertificates);
                    if (response?.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success });
                    }
                }
                return Json(new { id = -1, message = CustomMessages.Error });
            }
            catch (Exception exception)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Creates the transcript detail certificate.
        /// </summary>
        /// <param name="academicPlanCourseDetails">The academic plan course details.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateTranscriptDetailCertificate(List<AcademicPlanCourseDetail> academicPlanCourseDetails)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    List<AcademicPlanCourseDetail> coursesToInsert = new List<AcademicPlanCourseDetail>();
                    List<AcademicPlanCourseDetail> coursesToUpdate = new List<AcademicPlanCourseDetail>();
                    foreach (AcademicPlanCourseDetail academicPlanCourseDetail in academicPlanCourseDetails)
                    {
                        academicPlanCourseDetail.OperatorId = Account.UserName;
                        if (academicPlanCourseDetail.TranscriptDetailCertificateId != null &&
                            academicPlanCourseDetail.TranscriptDetailCertificateId > 0 &&
                            academicPlanCourseDetail.IncludeInTemporalTable)
                            coursesToUpdate.Add(academicPlanCourseDetail);
                        else
                        {
                            if (academicPlanCourseDetail.IncludeInTemporalTable)
                                coursesToInsert.Add(academicPlanCourseDetail);
                        }
                    }
                    TranscriptDetailCertificate transcriptDetailCertificate = new TranscriptDetailCertificate();
                    transcriptDetailCertificate.CoursesToInsert = coursesToInsert;
                    transcriptDetailCertificate.CoursesToUpdate = coursesToUpdate;

                    string json = JsonConvert.SerializeObject(transcriptDetailCertificate);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + ApiRoute.EcCreateTranscriptDetailCertificate, httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.ReasonPhrase);
                        return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { id = -1, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController - Create", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.DeleteAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesDelete}{id}");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.ReasonPhrase);
                    return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.ReasonPhrase);
                    return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the Electronic Certificate folio
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="institutionCampusId">The institution campus identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> Folio(string peopleId, int institutionCampusId)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcGenerationsFolio}{peopleId}/{institutionCampusId}");

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string contentFolio = await resp.Content.ReadAsStringAsync();
                    string folio = JsonConvert.DeserializeObject<string>(contentFolio);
                    return Json(new { folio, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController - Folio", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the issuing certificate step information.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> IssuingCertificate()
        {
            try
            {
                List<CodeTable> certificationTypes = new List<CodeTable>();
                List<CodeTable> federalEntitiesCatalog = new List<CodeTable>();

                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage respCertificationTypes = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcCertificationTypes}");
                HttpResponseMessage respFederalEntitiesCatalog = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcFederalEntitiesCodeCatalogWithoutForeign}");

                if (respCertificationTypes.StatusCode == HttpStatusCode.OK && respFederalEntitiesCatalog.StatusCode == HttpStatusCode.OK)
                {
                    string contentCertificationTypes = await respCertificationTypes.Content.ReadAsStringAsync();
                    string contentFederalEntitiesCatalog = await respFederalEntitiesCatalog.Content.ReadAsStringAsync();

                    certificationTypes = JsonConvert.DeserializeObject<List<CodeTable>>(contentCertificationTypes);
                    federalEntitiesCatalog = JsonConvert.DeserializeObject<List<CodeTable>>(contentFederalEntitiesCatalog);
                    return Json(new
                    {
                        certificationTypes = certificationTypes.ToViewModel(),
                        federalEntitiesCatalog = federalEntitiesCatalog.ToViewModel()
                    }, JsonRequestBehavior.AllowGet);
                }
                else if (respCertificationTypes.StatusCode == HttpStatusCode.Unauthorized || respFederalEntitiesCatalog.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (respCertificationTypes.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", respCertificationTypes.StatusCode.ToString());
                        LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", respCertificationTypes.ReasonPhrase);
                    }
                    if (respFederalEntitiesCatalog.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", respFederalEntitiesCatalog.StatusCode.ToString());
                        LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", respFederalEntitiesCatalog.ReasonPhrase);
                    }
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController - CertificationTypes", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Peoples the specified people identifier.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> People(string peopleId)
        {
            try
            {
                List<PeopleModel> peopleList;
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.People}{peopleId}");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    peopleList = JsonConvert.DeserializeObject<List<PeopleModel>>(content);
                    return Json(new { peopleList = peopleList.ToViewModel(), message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController - People", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Studieses the program detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> StudiesProgramDetail(int id)
        {
            try
            {
                StudiesProgramDetail studiesProgramDetail = new StudiesProgramDetail();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcStudiesProgramDetail}{id}");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    studiesProgramDetail = JsonConvert.DeserializeObject<StudiesProgramDetail>(content);
                    List<CodeTable> periodTypes = null;
                    HttpResponseMessage respPeriodTypes = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcPeriodTypes}");
                    if (respPeriodTypes.StatusCode == HttpStatusCode.OK)
                    {
                        string contentPeriodTypes = await respPeriodTypes.Content.ReadAsStringAsync();
                        periodTypes = JsonConvert.DeserializeObject<List<CodeTable>>(contentPeriodTypes);
                    }
                    List<CodeTable> codeFederalEntities = null;
                    HttpResponseMessage respFederalEntitiesCodeCatalog = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcFederalEntitiesCodeCatalog}");
                    if (respFederalEntitiesCodeCatalog.StatusCode == HttpStatusCode.OK)
                    {
                        string contentFederalEntitiesCodeCatalog = await respFederalEntitiesCodeCatalog.Content.ReadAsStringAsync();
                        codeFederalEntities = JsonConvert.DeserializeObject<List<CodeTable>>(contentFederalEntitiesCodeCatalog);
                    }
                    return Json(new
                    {
                        studiesProgramDetail = studiesProgramDetail.ToViewModel(),
                        periodTypes = periodTypes.ToViewModel(),
                        codeFederalEntities = codeFederalEntities.ToViewModel()
                    }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController - StudiesPrograms", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Studieses the programs.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> StudiesPrograms(string peopleId)
        {
            try
            {
                List<StudiesProgram> studiesPrograms = new List<StudiesProgram>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcStudiesPrograms}{peopleId}/{Account.UserName}");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    studiesPrograms = JsonConvert.DeserializeObject<List<StudiesProgram>>(content);
                    return Json(studiesPrograms.ToViewModel(), JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController - StudiesPrograms", ex.Message);
                throw;
            }
        }

        #endregion JSON

        #region Private Methods

        /// <summary>
        /// Gets the response asynchronous.
        /// </summary>
        /// <param name="httpVerb">The HTTP verb.</param>
        /// <param name="json">The json.</param>
        /// <param name="apiRoute">The API route will be concatenated as client.BaseAddress + apiRoute</param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> GetResponseAsync(HttpVerbs httpVerb, string json, string apiRoute)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage response = null;
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                switch (httpVerb)
                {
                    case HttpVerbs.Get:
                        break;

                    case HttpVerbs.Post:
                        response = await client.PostAsync(client.BaseAddress + apiRoute, httpContent);
                        break;

                    case HttpVerbs.Put:
                        break;

                    case HttpVerbs.Delete:
                        break;

                    case HttpVerbs.Head:
                        break;

                    case HttpVerbs.Patch:
                        break;

                    case HttpVerbs.Options:
                        break;

                    default:
                        break;
                }

                return response;
            }
            catch (Exception exception)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController - GetResponseAsync", exception.Message);
                throw;
            }
        }

        #endregion Private Methods
    }
}