// --------------------------------------------------------------------
// <copyright file="AcademicPlansController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using PowerCampus.Entities.ElectronicCertificate;
using Resources;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Areas.ElectronicCertificate.Mappers;
using WebAdminUI.Areas.ElectronicCertificate.Models;
using WebAdminUI.Areas.ElectronicCertificate.Models.Shared;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicCertificate.Controllers
{
    /// <summary>
    /// Setup Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicCertificate)]
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicCertificateSetup)]
    public class AcademicPlansController : WebAdminUI.Controllers.BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcAcademicPlanCampus}");
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        AcademicPlanViewModel academicPlanViewModel = new AcademicPlanViewModel();
                        string content = await resp.Content.ReadAsStringAsync();
                        academicPlanViewModel.CampusList = JsonConvert.DeserializeObject<List<CodeTable>>(content).ToViewModel();
                        return View(academicPlanViewModel);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "InstitutionCampusesController", resp.StatusCode.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "InstitutionCampusesController", resp.ReasonPhrase);
                        return RedirectToRoute("ErrorUnauthorized");
                    }
                    else
                    {
                        return RedirectToRoute("ErrorException");
                    }
                }
                else
                {
                    return Json(new { id = -1, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "AcademicPlansController", ex.Message);
                throw;
            }
        }

        #region JSON

        /// <summary>
        /// Courseses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Courses(int id)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcAcademicPlanCourses}{id}");
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await resp.Content.ReadAsStringAsync();
                        ProgramViewModel program = JsonConvert.DeserializeObject<PdcRvoe>(content).ToViewModel();
                        return View(program);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "InstitutionCampusesController", resp.StatusCode.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "InstitutionCampusesController", resp.ReasonPhrase);
                        return RedirectToRoute("ErrorUnauthorized");
                    }
                    else
                    {
                        return RedirectToRoute("ErrorException");
                    }
                }
                else
                {
                    return Json(new { id = -1, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "AcademicPlansController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Matrics the terms.
        /// </summary>
        /// <param name="campusCodeId">The campus code identifier.</param>
        /// <param name="matricYear">The matric year.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> MatricTerms(string campusCodeId, string matricYear)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcAcademicPlanMatricTerm}{campusCodeId}/{matricYear}");
                    List<DropdownListViewModel> matricTerms = null;
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await resp.Content.ReadAsStringAsync();
                        matricTerms = JsonConvert.DeserializeObject<List<CodeTable>>(content).ToViewModel();
                        return Json(new { matricTerms, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    LoggerHelper.LogWebError("ElectronicCertificate", "AcademicPlansController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "AcademicPlansController", resp.ReasonPhrase);
                    return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { id = -1, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "AcademicPlansController - MatricTerms", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Matrics the years.
        /// </summary>
        /// <param name="campusCodeId">The campus code identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> MatricYears(string campusCodeId)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcAcademicPlanMatricYear}{campusCodeId}");

                    List<DropdownListViewModel> matricYears = null;
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await resp.Content.ReadAsStringAsync();
                        matricYears = JsonConvert.DeserializeObject<List<CodeTable>>(content).ToViewModel();
                        return Json(new { matricYears, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "InstitutionCampusesController", resp.StatusCode.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "InstitutionCampusesController", resp.ReasonPhrase);
                        return RedirectToRoute("ErrorUnauthorized");
                    }
                    else
                    {
                        return RedirectToRoute("ErrorException");
                    }
                }
                else
                {
                    return Json(new { id = -1, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "AcademicPlansController - MatricYears", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Matrics the years.
        /// </summary>
        /// <param name="campusCodeId">The campus code identifier.</param>
        /// <param name="matricYear">The matric year.</param>
        /// <param name="matricTerm">The matric term.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Programs(string campusCodeId, string matricYear, string matricTerm)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcAcademicPlanPrograms}{campusCodeId}/{matricYear}/{matricTerm}");
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await resp.Content.ReadAsStringAsync();
                        List<ProgramViewModel> programs = JsonConvert.DeserializeObject<List<PdcRvoe>>(content).ToViewModel();
                        return Json(new { programs, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "InstitutionCampusesController", resp.StatusCode.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "InstitutionCampusesController", resp.ReasonPhrase);
                        return RedirectToRoute("ErrorUnauthorized");
                    }
                    else
                    {
                        return RedirectToRoute("ErrorException");
                    }
                }
                else
                {
                    return Json(new { id = -1, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "AcademicPlansController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Saves the specified institution campuses.
        /// </summary>
        /// <param name="pdcCourses">The PDC courses.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Save(List<PdcCourse> pdcCourses)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    PdcCourses pdc = new PdcCourses()
                    {
                        PdcCourse = pdcCourses,
                        OperatorId = Account.UserName
                    };
                    string json = JsonConvert.SerializeObject(pdc);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + ApiRoute.EcSaveAcademicPlanCourses, httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "AcademicPlansController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "AcademicPlansController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "AcademicPlansController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "AcademicPlansController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicDegree", "AcademicPlansController - Create", ex.Message);
                throw;
            }
        }

        #endregion JSON
    }
}