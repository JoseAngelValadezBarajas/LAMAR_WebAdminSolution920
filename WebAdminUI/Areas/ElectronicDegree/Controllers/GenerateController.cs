// --------------------------------------------------------------------
// <copyright file="GenerateController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using PowerCampus.Entities.ElectronicDegree;
using Resources;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Areas.ElectronicDegree.Mappers;
using WebAdminUI.Areas.ElectronicDegree.Models;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using WebAdminUI.Models.AcademicInfoViewModel;
using Microsoft.Extensions.Logging;

namespace WebAdminUI.Areas.ElectronicDegree.Controllers
{
    /// <summary>
    /// Generate Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicDegree)]
    [PermissionRequired(PermissionName = PermissionsConstants.GenerateElectronicDegrees)]
    public class GenerateController : WebAdminUI.Controllers.BaseController
    {
        /// <summary>
        /// Generates this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Generate() => View("Generate", new GenerateViewModel());

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            try
            {
                ElectronicDegreeModelList electronicDegreeModelList = new ElectronicDegreeModelList();
                List<ElectronicDegreeModel> electronicDegreeList = new List<ElectronicDegreeModel>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/ElectronicDegreeInformation?folio=" + "" +
                    "&student=" + "" + "&degreeType=" + "" + "&major=" + "");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    electronicDegreeList = JsonConvert.DeserializeObject<List<ElectronicDegreeInfo>>(content).ToViewModel();
                    electronicDegreeModelList.ElectronicDegreesModel = electronicDegreeList;
                    electronicDegreeModelList.ElectronicDegreeParameters = new ElectronicDegreeParameters();
                    electronicDegreeModelList.IsSentAvailable = false;

                    ElectronicDegreeParameters parameters = new ElectronicDegreeParameters();
                    parameters.EducationType = new List<ListOption>()
                    {
                        new ListOption(){ Value = "0", Description = Resources.ElectronicDegree.Major.lblBachilleratoTecnico },
                        new ListOption(){ Value = "1", Description = Resources.ElectronicDegree.Major.lblDoctorado },
                        new ListOption(){ Value = "2", Description = Resources.ElectronicDegree.Major.lblEspecialidad },
                        new ListOption(){ Value = "3", Description = Resources.ElectronicDegree.Major.lblLicenciatura },
                        new ListOption(){ Value = "4", Description = Resources.ElectronicDegree.Major.lblMaestria },
                        new ListOption(){ Value = "5", Description = Resources.ElectronicDegree.Major.lblProfesionalAsociado },
                        new ListOption(){ Value = "6", Description = Resources.ElectronicDegree.Major.lblProfesionalTecnico },
                        new ListOption(){ Value = "7", Description = Resources.ElectronicDegree.Major.lblProfesor },
                        new ListOption(){ Value = "8", Description = Resources.ElectronicDegree.Major.lblTecnico },
                        new ListOption(){ Value = "9", Description = Resources.ElectronicDegree.Major.lblTecnicoProfesional },
                        new ListOption(){ Value = "10", Description = Resources.ElectronicDegree.Major.lblTecnicoSuperior },
                        new ListOption(){ Value = "11", Description = Resources.ElectronicDegree.Major.lblTecnicoSuperiorUniv },
                        new ListOption(){ Value = "12", Description = Resources.ElectronicDegree.Major.lblTecnologico }
                    };

                    List<MajorList> majorListCatalog = null;
                    resp = await client.GetAsync(client.BaseAddress + "/api/Majors");
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        string majorsContent = await resp.Content.ReadAsStringAsync();
                        majorListCatalog = JsonConvert.DeserializeObject<List<MajorList>>(majorsContent);
                        List<ListOption> majors = new List<ListOption>();
                        foreach (MajorList major in majorListCatalog)
                        {
                            majors.Add(new ListOption
                            {
                                Description = major.Name,
                                Value = major.Code
                            });
                        }
                        parameters.Major = majors;

                        electronicDegreeModelList.ElectronicDegreeParameters = parameters;
                        return View(electronicDegreeModelList);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree1", "Addvantit", resp.StatusCode.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree2", "Addvantit", resp.ReasonPhrase);
                        return RedirectToRoute("ErrorUnauthorized");
                    }
                    else
                        return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree3", "Addvantit", ex.Message);
                throw;
            }

            return null;
        }

        #region JSON

        /// <summary>
        /// Backgrounds the studies.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="ElectronicDegreeInstMajorId">The electronic degree inst major identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> BackgroundStudies(string peopleId, int ElectronicDegreeInstMajorId)
        {
            try
            {
                List<BackgroundStudies> backgroundStudies = new List<BackgroundStudies>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/BackgroundStudies/" + peopleId);
                HttpResponseMessage respFolio = await client.GetAsync(client.BaseAddress + "/api/InstitutionFolio/" + peopleId + "/" + ElectronicDegreeInstMajorId);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    string contentFolio = await respFolio.Content.ReadAsStringAsync();
                    backgroundStudies = JsonConvert.DeserializeObject<List<BackgroundStudies>>(content);
                    string institutionFolio = JsonConvert.DeserializeObject<string>(contentFolio);
                    return Json(new { backgroundStudies, institutionFolio, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "GenerateController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "GenerateController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "GenerateController - BackgroundStudies", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Creates the specified electronic degree information.
        /// </summary>
        /// <param name="electronicDegreeInfo">The electronic degree information.</param>
        /// <returns></returns>
        public async Task<ActionResult> Create(ElectronicDegreeInfo electronicDegreeInfo)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    string json = JsonConvert.SerializeObject(electronicDegreeInfo);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/ElectronicDegree/Create", httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "GenerateController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "GenerateController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "GenerateController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "GenerateController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicDegree", "GenerateController - InstitutionMajor", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Institutions the major.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> InstitutionMajor(string peopleId, string operatorId)
        {
            try
            {
                List<InstitutionMajor> institutionMajors = new List<InstitutionMajor>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/InstitutionMajor/" + peopleId + "/" + operatorId);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    institutionMajors = JsonConvert.DeserializeObject<List<InstitutionMajor>>(content);
                    return Json(new { institutionMajors, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "GenerateController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "GenerateController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "GenerateController - InstitutionMajor", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Institutions the signer.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> InstitutionSigner(int institutionId)
        {
            try
            {
                List<InstitutionSignerList> institutionSigners = new List<InstitutionSignerList>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/InstitutionSigner/" + institutionId);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    institutionSigners = JsonConvert.DeserializeObject<List<InstitutionSignerList>>(content);
                    return Json(new { institutionSigners, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "GenerateController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "GenerateController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "GenerateController - InstitutionSigner", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Issuings the degree.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="instMajorId">The inst major identifier.</param>
        /// <param name="transcriptDegreeId">The transcript degree identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> IssuingDegree(string peopleId, string instMajorId, string transcriptDegreeId)
        {
            try
            {
                IssuingDegree issuingDegree = new IssuingDegree();
                CodeLegalBase[] legalBaseCatalog = null;
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/IssuingDegree/" + peopleId + "/" + instMajorId + "/" + transcriptDegreeId);
                HttpResponseMessage respLegalBase = await client.GetAsync(client.BaseAddress + "/api/LegalBase/Catalog");
                if (resp.StatusCode == HttpStatusCode.OK && respLegalBase.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    string contentLegalBase = await respLegalBase.Content.ReadAsStringAsync();
                    issuingDegree = JsonConvert.DeserializeObject<IssuingDegree>(content);
                    legalBaseCatalog = JsonConvert.DeserializeObject<CodeLegalBase[]>(contentLegalBase);
                    return Json(new { issuingDegree, legalBaseCatalog, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "GenerateController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "GenerateController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "GenerateController - IssuingDegree", ex.Message);
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
                List<PeopleModel> peopleList = new List<PeopleModel>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/People/" + peopleId);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    peopleList = JsonConvert.DeserializeObject<List<PeopleModel>>(content);
                    return Json(new { peopleList, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "GenerateController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "GenerateController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "GenerateController - People", ex.Message);
                throw;
            }
        }

        public ActionResult GetDatos(string peopleId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PowerCampusDbContext"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT TOP 10 * FROM ACADEMIC", connection);
        
                    JArray jsonArray = new JArray();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JObject obj = new JObject();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                obj[reader.GetName(i)] = reader.GetValue(i).ToString();
                            }
                            jsonArray.Add(obj);
                        }
                    }
        
                    connection.Close();
                    return Ok(jsonArray.ToString());
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        public async Task<ActionResult> AcademicYearsG(string peopleId, string institutionName)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                // Usamos la URL original como estaba en tu código
                var url = client.BaseAddress + "api/people/yts/getgraduatedates/?codestudent=" + peopleId + "&institutionName=" + institutionName;
                HttpResponseMessage resp = await client.GetAsync(url);
                
                if (resp.IsSuccessStatusCode)
                {
                    // Leer el contenido JSON como una lista de AcademicInfoViewModelDegree
                    string content = await resp.Content.ReadAsStringAsync();
                    LoggerHelper.LogWebError("AcademicYearsG", "PowerCampus.DataAccess - PeopleController", content);
                    
                    // Deserializar el contenido JSON en una lista de objetos
                    List<AcademicInfoViewModelDegree> academicInfoList = JsonConvert.DeserializeObject<List<AcademicInfoViewModelDegree>>(content);
                    
                    return Json(academicInfoList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LoggerHelper.LogWebError("AcademicYearsG", "PowerCampus.DataAccess - PeopleController", resp.ReasonPhrase);
                    
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        LoggerHelper.LogWebError("AcademicYearsG", "PowerCampus.DataAccess - PeopleController - Not Found", resp.ReasonPhrase);
                        return RedirectToRoute("ErrorException");
                    }
                    else
                    {
                        LoggerHelper.LogWebError("AcademicYearsG", "PowerCampus.DataAccess - PeopleController - Other Error", resp.ReasonPhrase);
                        return RedirectToRoute("ErrorException");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                LoggerHelper.LogWebError("AcademicYearsG", "PowerCampus.DataAccess - PeopleController", ex.Message);
                return RedirectToRoute("ErrorException");
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("AcademicYearsG", "PowerCampus.DataAccess - PeopleController", ex.Message);
                return RedirectToRoute("ErrorException");
            }
        }



        #endregion JSON
    }
}