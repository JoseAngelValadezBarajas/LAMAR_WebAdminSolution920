// --------------------------------------------------------------------
// <copyright file="CatalogsController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using PowerCampus.Entities.ElectronicDegree;
using Resources;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Areas.ElectronicDegree.Models.Catalogs;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicDegree.Controllers
{
    /// <summary>
    /// Handles federal entity mappings
    /// </summary>
    /// <seealso cref="WebAdminUI.Controllers.BaseController" />
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicDegree)]
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicDegreeSetup)]
    public class CatalogsController : WebAdminUI.Controllers.BaseController
    {
        /// <summary>
        /// Authorizations the type.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AuthorizationType()
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage respMapping = await client.GetAsync(client.BaseAddress + "api/AuthorizationType/CatalogMapping");
                HttpResponseMessage respRvoe = await client.GetAsync(client.BaseAddress + "api/AuthorizationType/RvoeCatalog");

                if (respMapping.StatusCode == HttpStatusCode.OK && respRvoe.StatusCode == HttpStatusCode.OK)
                {
                    string contentMapping = await respMapping.Content.ReadAsStringAsync();
                    CodeAuthorizationTypeMapping[] authorizationTypeCatalog = JsonConvert.DeserializeObject<CodeAuthorizationTypeMapping[]>(contentMapping);

                    string contentRvoe = await respRvoe.Content.ReadAsStringAsync();
                    CodeRvoe[] rvoeCatalog = JsonConvert.DeserializeObject<CodeRvoe[]>(contentRvoe);

                    decimal mappingProgress = 100.00m *
                        decimal.Round(authorizationTypeCatalog.Count(f => f.MappedRvoes.Length > 0), 2)
                        / decimal.Round(authorizationTypeCatalog.Length, 2);

                    ViewBag.RvoeCatalog = rvoeCatalog;
                    ViewBag.UserName = Account.UserName;
                    ViewBag.MappingProgress = mappingProgress;

                    return View(authorizationTypeCatalog);
                }
                else if (respMapping.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", respMapping.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", respMapping.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Backgrounds the type of the study.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> BackgroundStudyType()
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage respMapping = await client.GetAsync(client.BaseAddress + "api/BackgroundStudyType/GetCatalogMapping");
                HttpResponseMessage respScholarshipLevel = await client.GetAsync(client.BaseAddress + "api/BackgroundStudyType/GetScholarshipLevelsCatalog");

                if (respMapping.StatusCode == HttpStatusCode.OK && respScholarshipLevel.StatusCode == HttpStatusCode.OK)
                {
                    string contentMapping = await respMapping.Content.ReadAsStringAsync();
                    CodeBackgroundStudyTypeMapping[] backgroundStudyTypeCatalog = JsonConvert.DeserializeObject<CodeBackgroundStudyTypeMapping[]>(contentMapping);

                    string contentScholarshipLevel = await respScholarshipLevel.Content.ReadAsStringAsync();
                    CodeScholarshipLevel[] scholarshipLevelCatalog = JsonConvert.DeserializeObject<CodeScholarshipLevel[]>(contentScholarshipLevel);

                    decimal mappingProgress = 100.00m *
                        decimal.Round(backgroundStudyTypeCatalog.Count(f => f.MappedLevels.Length > 0), 2)
                        / decimal.Round(backgroundStudyTypeCatalog.Length, 2);

                    ViewBag.ScholarshipLevelCatalog = scholarshipLevelCatalog;
                    ViewBag.UserName = Account.UserName;
                    ViewBag.MappingProgress = mappingProgress;

                    return View(backgroundStudyTypeCatalog);
                }
                else if (respMapping.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", respMapping.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", respMapping.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> FederalEntity()
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage respMapping = await client.GetAsync(client.BaseAddress + "api/FederalEntities/GetCatalogMapping");
                HttpResponseMessage respStates = await client.GetAsync(client.BaseAddress + "api/FederalEntities/GetStatesCatalog");

                if (respMapping.StatusCode == HttpStatusCode.OK && respStates.StatusCode == HttpStatusCode.OK)
                {
                    string contentMapping = await respMapping.Content.ReadAsStringAsync();
                    CodeFederalEntitityMapping[] federalEntitiesCatalog = JsonConvert.DeserializeObject<CodeFederalEntitityMapping[]>(contentMapping);

                    string contentStates = await respStates.Content.ReadAsStringAsync();
                    CodeState[] statesCatalog = JsonConvert.DeserializeObject<CodeState[]>(contentStates);

                    decimal mappingProgress = 100.00m *
                        decimal.Round(federalEntitiesCatalog.Count(f => f.MappedStates.Length > 0), 2)
                        / decimal.Round(federalEntitiesCatalog.Length, 2);

                    ViewBag.StatesCatalog = statesCatalog;
                    ViewBag.UserName = Account.UserName;
                    ViewBag.MappingProgress = mappingProgress;

                    return View(federalEntitiesCatalog);
                }
                else if (respMapping.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", respMapping.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", respMapping.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Saves the authorization type mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SaveAuthorizationTypeMapping(SaveAuthorizationTypeMapping mapping)
        {
            try
            {
                if (mapping.Rvoes == null)
                    mapping.Rvoes = new string[] { };

                string json = JsonConvert.SerializeObject(new
                {
                    UserName = mapping.UserName,
                    ShortDescription = mapping.AuthorizationType,
                    MappedRvoes = mapping.Rvoes.Select(s => new PowerCampusRvoe
                    {
                        RvoeId = Convert.ToInt32(s)
                    })
                });
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "api/AuthorizationType/Mapping/Save", httpContent);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", resp.ReasonPhrase);
                    return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", resp.ReasonPhrase);
                    return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Saves the background study type mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SaveBackgroundStudyTypeMapping(SaveBackgroundStudyTypeMapping mapping)
        {
            try
            {
                if (mapping.Levels == null)
                    mapping.Levels = new string[] { };

                string json = JsonConvert.SerializeObject(new
                {
                    UserName = mapping.UserName,
                    ShortDescription = mapping.BackgroundStudyType,
                    MappedLevels = mapping.Levels.Select(s => new PowerCampusScholarshipLevel
                    {
                        ScholarshipLevelId = s
                    })
                });
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "api/BackgroundStudyType/SaveScholarshipLevelMapping", httpContent);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", resp.ReasonPhrase);
                    return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", resp.ReasonPhrase);
                    return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Saves the federal entity mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SaveFederalEntityMapping(SaveFederalEntityMapping mapping)
        {
            try
            {
                if (mapping.States == null)
                    mapping.States = new string[] { };

                string json = JsonConvert.SerializeObject(new
                {
                    UserName = mapping.UserName,
                    ShortDescription = mapping.FederalEntity,
                    MappedStates = mapping.States.Select(s => new PowerCampusCodeState
                    {
                        CodeStateId = s
                    })
                });
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "api/FederalEntities/SaveFederalEntityMapping", httpContent);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", resp.ReasonPhrase);
                    return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", resp.ReasonPhrase);
                    return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "CatalogsController", ex.Message);
                throw;
            }
        }
    }
}