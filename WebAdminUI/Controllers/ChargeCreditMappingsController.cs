// --------------------------------------------------------------------
// <copyright file="ChargeCreditMappingsController.cs" company="Ellucian">
//     Copyright 2018 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.ChargeCreditsMappers;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;
using WebAdminUI.Models.ChargeCreditMappings;

namespace WebAdminUI.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
    public class ChargeCreditMappingsController : BaseController
    {
        /// <summary>
        /// Charges the credits table partial view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> _ChargeCreditsTable(string id)
        {
            List<ChargeCreditWithTaxes> chargeCreditWithTaxes = new List<ChargeCreditWithTaxes>();

            if (Account.status == enumAccess.Authorized)
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "api/ChargeCreditMapping/GetChargeCreditWithTaxes/" + id);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    chargeCreditWithTaxes = await resp.Content.ReadAsAsync<List<ChargeCreditWithTaxes>>();
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditMappingsController", resp.ReasonPhrase);
                    LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditMappingsController", resp.RequestMessage.ToString());
                }
            }

            return PartialView(chargeCreditWithTaxes.ToViewModel());
        }

        // POST: ChargeCredits/Create
        /// <summary>
        /// Creates the specified charge credit mappings.
        /// </summary>
        /// <param name="chargeCreditMappings">The charge credit mappings.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(List<ChargeCreditMapping> chargeCreditMappings)
        {
            try
            {
                if (chargeCreditMappings != null && Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    string json = JsonConvert.SerializeObject(chargeCreditMappings);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/ChargeCreditMappings", httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        bool result = await resp.Content.ReadAsAsync<bool>();
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditMappingsController", resp.ReasonPhrase);
                    LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditMappingsController", resp.RequestMessage.ToString());
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditMappingsController", ex.Message, ex);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid && Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage resp = await client.DeleteAsync(client.BaseAddress + "/api/ChargeCreditMappings/" + id);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        int deleted = await resp.Content.ReadAsAsync<int>();
                        return deleted;
                    }

                    LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditMappingsController", resp.ReasonPhrase);
                    LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditMappingsController", resp.RequestMessage.ToString());
                }
            }
            catch
            {
                return 0;
            }
            return 0;
        }

        /// <summary>
        /// Gets the product service.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetProductService(string id)
        {
            List<FiscalRecordCatalog> LstProductServiceCatalog = new List<FiscalRecordCatalog>();

            if (Account.status == enumAccess.Authorized)
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/catalogs?name=" + Catalog.ProductServiceKey + "&code=" + id);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    LstProductServiceCatalog = await resp.Content.ReadAsAsync<List<FiscalRecordCatalog>>(); ;
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditMappingsController", resp.ReasonPhrase);
                    LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditMappingsController", resp.RequestMessage.ToString());
                }
            }

            return Json(LstProductServiceCatalog, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the unity key.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetUnityKey(string id)
        {
            List<FiscalRecordCatalog> LsttUnityCatalog = new List<FiscalRecordCatalog>();

            if (Account.status == enumAccess.Authorized)
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/catalogs?name=" + Catalog.UnityKey + "&code=" + id);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    LsttUnityCatalog = await resp.Content.ReadAsAsync<List<FiscalRecordCatalog>>(); ;
                }

                LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditMappingsController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditMappingsController", resp.RequestMessage.ToString());
            }

            return Json(LsttUnityCatalog, JsonRequestBehavior.AllowGet);
        }

        // GET: ChargeCredits
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            List<ChargeCreditMapping> chargeCreditMappings = null;
            List<ChargeCreditTaxes> chargeCreditSpecialTaxes = null;

            #region Get ChargeCreditMappings

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/ChargeCreditMappings");
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                chargeCreditMappings = await resp.Content.ReadAsAsync<List<ChargeCreditMapping>>();
                resp = await client.GetAsync(client.BaseAddress + "api/ChargeCreditMapping/GetSpecialIvaTax");
                if (resp.StatusCode == HttpStatusCode.OK)
                    chargeCreditSpecialTaxes = await resp.Content.ReadAsAsync<List<ChargeCreditTaxes>>();
            }

            if (resp?.StatusCode != HttpStatusCode.OK)
            {
                LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditMappingsController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditMappingsController", resp.StatusCode.ToString());
                if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    return RedirectToRoute("ErrorExpired");
                else
                    return RedirectToRoute("ErrorException");
            }

            ChargeCreditMappingViewModel chargeCreditMappingViewModel = new ChargeCreditMappingViewModel
            {
                ChargeCreditList = chargeCreditMappings.ToViewModel(),
                ChargeCreditSpecialTaxes = chargeCreditSpecialTaxes.ToViewModel()
            };
            return View(chargeCreditMappingViewModel);

            #endregion Get ChargeCreditMappings
        }
    }
}