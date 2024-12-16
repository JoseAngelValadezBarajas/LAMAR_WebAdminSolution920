// --------------------------------------------------------------------
// <copyright file="CatalogsController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using PowerCampus.Entities.ElectronicCertificate;
using Resources;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Areas.ElectronicCertificate.Models.Catalogs;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicCertificate.Controllers
{
    /// <summary>
    /// Handles federal entity mappings
    /// </summary>
    /// <seealso cref="WebAdminUI.Controllers.BaseController" />
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicCertificate)]
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicCertificateSetup)]
    public class CatalogsController : WebAdminUI.Controllers.BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> FederalEntity()
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage respMapping = await client.GetAsync(client.BaseAddress + ApiRoute.EcCatalogMapping);
                HttpResponseMessage respStates = await client.GetAsync(client.BaseAddress + ApiRoute.EcStatesCatalog);

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
                    LoggerHelper.LogWebError("ElectronicCertificate", "CatalogsController", respMapping.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "CatalogsController", respMapping.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "CatalogsController", ex.Message);
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
                HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + ApiRoute.EcSaveFederalEntity, httpContent);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicCertificate", "CatalogsController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "CatalogsController", resp.ReasonPhrase);
                    return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LoggerHelper.LogWebError("ElectronicCertificate", "CatalogsController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "CatalogsController", resp.ReasonPhrase);
                    return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "CatalogsController", ex.Message);
                throw;
            }
        }
    }
}