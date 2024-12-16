// --------------------------------------------------------------------
// <copyright file="TaxProfilesController.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using Resources;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;
using WebAdminUI.Models.TaxProfiles;
using WebAdminUI.TaxProfilesMappers;

namespace WebAdminUI.Controllers
{
    /// <summary>
    /// TaxProfilesController
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordSetup)]
    public class TaxProfilesController : BaseController
    {
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="profileDetailViewModel">The profile detail view model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Create(List<TaxProfileDetailViewModel> profileDetailViewModel)
        {
            HttpResponseMessage resp = null;
            if (ModelState.IsValid)
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);

                bool sucess = false;
                foreach (var item in profileDetailViewModel)
                {
                    if (item.TaxRate == "43.770000")
                        item.FactorType = "Cuota";
                    else
                        item.FactorType = "Tasa";

                    string json = JsonConvert.SerializeObject(item.ToDataEntity());
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    resp = await client.PostAsync(client.BaseAddress + "/api/taxprofiles/taxmapping", httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        int fiscalRecordTaxMappingId = await resp.Content.ReadAsAsync<int>();
                        if (fiscalRecordTaxMappingId > 0)
                            sucess = true;
                    }
                    else
                    {
                        sucess = false;
                        break;
                    }
                }
                if (sucess)
                {
                    return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }

                LoggerHelper.LogWebError("FiscalRecords", "TaxProfilesController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "TaxProfilesController", resp.RequestMessage.ToString());
                return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    id = 0,
                    message =
                    string.Join("<br/>", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).Distinct().ToList())
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets the tax rate catalog.
        /// </summary>
        /// <param name="tax">The tax.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetTaxRateCatalog(string tax)
        {
            List<FiscalRecordCatalog> taxRateCatalog = new List<FiscalRecordCatalog>();

            if (string.IsNullOrEmpty(tax))
                return Json(taxRateCatalog, JsonRequestBehavior.AllowGet);

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await
                client.GetAsync(client.BaseAddress + "/api/catalogs?name=" + Catalog.TaxRate + "&code=" + tax);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                taxRateCatalog = await resp.Content.ReadAsAsync<List<FiscalRecordCatalog>>();
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "TaxProfilesController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "TaxProfilesController", resp.RequestMessage.ToString());
            }

            return Json(taxRateCatalog, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> List()
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/taxprofiles/");
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                var taxProfileList = await resp.Content.ReadAsAsync<List<TaxProfile>>();
                List<TaxProfileListViewModel> taxProfileListViewModel = new List<TaxProfileListViewModel>();
                foreach (var item in taxProfileList)
                    taxProfileListViewModel.Add(item.ToViewModel(Account));
                return View(taxProfileListViewModel.OrderBy(m => m.TaxProfileName));
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("FiscalRecords", "TaxProfilesController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "TaxProfilesController", resp.StatusCode.ToString());
                return RedirectToRoute("ErrorExpired");
            }
            {
                LoggerHelper.LogWebError("FiscalRecords", "TaxProfilesController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "TaxProfilesController", resp.StatusCode.ToString());
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Gets the validity details.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<PartialViewResult> ValidityDetails(int id)
        {
            var details = new List<TaxProfileDetail>();
            List<TaxProfileDetailViewModel> taxProfileDetailViewModel = new List<TaxProfileDetailViewModel>();
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/taxprofiles/validitydetails/?id=" + id);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                details = await resp.Content.ReadAsAsync<List<TaxProfileDetail>>();
                foreach (var item in details)
                    taxProfileDetailViewModel.Add(item.ToViewModel());

                //Get TaxesCatalog for each detail
                HttpResponseMessage responseCatalogs = await client.GetAsync(client.BaseAddress + "/api/catalogs?name=" + Catalog.Tax);
                var taxCatalogtmp = new List<FiscalRecordCatalog>();
                var taxCatalog = new List<FiscalRecordCatalog>();
                if (responseCatalogs.StatusCode == HttpStatusCode.OK)
                {
                    taxCatalog.Add(new FiscalRecordCatalog { Code = string.Empty, Description = string.Empty });
                    taxCatalogtmp = await responseCatalogs.Content.ReadAsAsync<List<FiscalRecordCatalog>>();
                    foreach (var item in taxCatalogtmp)
                        taxCatalog.Add(item);
                    foreach (var taxProfileDetail in taxProfileDetailViewModel)
                    {
                        taxProfileDetail.TaxesCatalog = new List<FiscalRecordCatalog>();
                        taxProfileDetail.TaxesCatalog = taxCatalog;
                    }
                }

                //Get TaxRateCatalog for each detail
                HttpResponseMessage taxRateCatalogResponse = await client.GetAsync(client.BaseAddress + "/api/catalogs?name=" + Catalog.TaxRate);
                var taxRateCatalog = new List<FiscalRecordCatalog>();
                if (taxRateCatalogResponse.StatusCode == HttpStatusCode.OK)
                {
                    taxRateCatalog = await taxRateCatalogResponse.Content.ReadAsAsync<List<FiscalRecordCatalog>>();
                    foreach (var taxProfileDetail in taxProfileDetailViewModel)
                    {
                        taxProfileDetail.TaxRatesCatalog = new List<FiscalRecordCatalog>();
                        taxProfileDetail.TaxRatesCatalog = taxRateCatalog.FindAll(m => m.Status == taxProfileDetail.TaxDescription);
                    }
                }
            }

            return PartialView("ValidityDetailsView", taxProfileDetailViewModel);
        }
    }
}