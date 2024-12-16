// --------------------------------------------------------------------
// <copyright file="ReceiversController.cs" company="Ellucian">
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
using WebAdminUI.FiscalRecordsMappers;
using WebAdminUI.Helpers;
using WebAdminUI.Models.Issuers;
using WebAdminUI.Models.Receivers;
using WebAdminUI.ReceiversMappers;

namespace WebAdminUI.Controllers
{
    /// <summary>
    /// ReceiversController Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
    [PermissionRequired(PermissionName = PermissionsConstants.ReceiverFiscalInformation)]
    public class ReceiversController : BaseController
    {
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        // GET: Receivers/Create
        public async Task<ActionResult> Create()
        {
            ReceiverViewModel receiverViewModel = new ReceiverViewModel();
            receiverViewModel.InvoiceOrganization = new InvoiceOrganizationViewModel();

            receiverViewModel.InvoiceOrganization.ListTaxRegimenCatalogs = await GetTaxRegimenCatalogs(13);
            receiverViewModel.ListStates = await GetStates();
            return View(receiverViewModel);
        }

        // POST: Receivers/Create
        /// <summary>
        /// Creates the specified receiver model.
        /// </summary>
        /// <param name="receiverViewModel">The receiver view model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Create(ReceiverViewModel receiverViewModel)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                string json = JsonConvert.SerializeObject(receiverViewModel.ToDataEntity());
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/receivers", httpContent);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    int taxpayerId = await resp.Content.ReadAsAsync<int>();
                    return taxpayerId;
                }
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.RequestMessage.ToString());

                return 0;
            }
            return 0;
        }

        /// <summary>
        /// Deletes the taxpayer identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.DeleteAsync(client.BaseAddress + "/api/Receivers/DeleteTaxpayer/" + id);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                bool result = await resp.Content.ReadAsAsync<bool>();
                return Json(result);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiversController - DeleteTaxpayer", resp.ReasonPhrase);
                return Json(false);
            }
        }

        /// <summary>
        /// Detailses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        // GET: Receivers/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="foreignId">The foreign identifier.</param>
        /// <returns></returns>
        // GET: Receivers/Edit/5
        public async Task<ActionResult> Edit(int id, int? foreignId)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/receivers/GetTaxPayerbyId/?id=" + id + "&foreignId=" + foreignId);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                var Model = await resp.Content.ReadAsAsync<Receiver>();
                ReceiverViewModel receiverViewModel = Model.ToViewModel();
                receiverViewModel.InvoiceOrganization = new InvoiceOrganizationViewModel();
                receiverViewModel.InvoiceOrganization.ListTaxRegimenCatalogs = await GetTaxRegimenCatalogs(receiverViewModel.TaxPayerId.Length);
                receiverViewModel.ListStates = await GetStates();

                return View(receiverViewModel);
            }
            LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.ReasonPhrase);
            LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.RequestMessage.ToString());

            return RedirectToRoute("ErrorException");
        }

        /// <summary>
        /// Gets all tax payer.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Get(string id)
        {
            List<Receiver> LstReceiverModel;
            List<ReceiverViewModel> LstReceiverViewModel = new List<ReceiverViewModel>();

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            Dictionary<string, string> values = new Dictionary<string, string>
                    {
                        {"TaxPayerId", id},
                    };
            FormUrlEncodedContent body = new FormUrlEncodedContent(values);
            HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/receivers/gettaxpayer", body);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                LstReceiverModel = await resp.Content.ReadAsAsync<List<Receiver>>();
                if (LstReceiverModel != null)
                {
                    foreach (Receiver model in LstReceiverModel)
                        LstReceiverViewModel.Add(model.ToViewModel());
                }
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.RequestMessage.ToString());
            }
            return Json(LstReceiverViewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the cfdi.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetCFDI(int length)
        {
            List<CFDIUsageCatalog> LstCFDI = new List<CFDIUsageCatalog>();
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/catalogs?length=" + length);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                LstCFDI = await resp.Content.ReadAsAsync<List<CFDIUsageCatalog>>();
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.RequestMessage.ToString());
            }
            return Json(LstCFDI, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the countries.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetCountries(string id)
        {
            List<FiscalRecordCatalog> LstCountryCatalog = new List<FiscalRecordCatalog>();
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/catalogs?name=" + Catalog.Country + "&code=" + id);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                LstCountryCatalog = await resp.Content.ReadAsAsync<List<FiscalRecordCatalog>>();
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.RequestMessage.ToString());
            }
            return Json(LstCountryCatalog, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the tax regimen catalogs.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<List<TaxRegimenCatalog>> GetTaxRegimenCatalogs(int? id)
        {
            IEnumerable<TaxRegimenCatalog> IEtypeTaxRegimenCatalog = await
                       GetListOfClassOfTypeT<TaxRegimenCatalog>(Account, "api/catalogs?name=TaxRegimen", null);
            List<TaxRegimenCatalog> TaxRegimenCatalogs = null;
            if (id == 13)
                TaxRegimenCatalogs = IEtypeTaxRegimenCatalog == null ? null : IEtypeTaxRegimenCatalog.Where(cat => cat.PhysicalPerson).ToList<TaxRegimenCatalog>();

            if (id == 12)
                TaxRegimenCatalogs = IEtypeTaxRegimenCatalog == null ? null : IEtypeTaxRegimenCatalog.Where(cat => cat.MoralPerson).ToList<TaxRegimenCatalog>();

            return TaxRegimenCatalogs;
        }

        public async Task<List<FiscalRecordCatalog>> GetStates()
        {
            IEnumerable<FiscalRecordCatalog> IEtypeStatesCatalog = await
                       GetListOfClassOfTypeT<FiscalRecordCatalog>(Account, "api/catalogs?name=States", null);
            List<FiscalRecordCatalog> StatesCatalogs = null;

            StatesCatalogs = IEtypeStatesCatalog == null ? null : IEtypeStatesCatalog.ToList<FiscalRecordCatalog>();

            return StatesCatalogs;
        }

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> List()
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/receivers/");
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                List<Receiver> Model = await resp.Content.ReadAsAsync<List<Receiver>>();
                List<ReceiverViewModel> lstReceiverViewModel = new List<ReceiverViewModel>();
                foreach (Receiver model in Model)
                    lstReceiverViewModel.Add(model.ToViewModel());
                return View(lstReceiverViewModel);
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.StatusCode.ToString());
                return RedirectToRoute("ErrorExpired");
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Gets the receivers searching by keyword.
        /// </summary>
        /// <param name="keyword">The keyword search.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ReceiversSearch(string keyword)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}/api/receivers?keyword={keyword}");

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                List<Receiver> Model = await resp.Content.ReadAsAsync<List<Receiver>>();
                List<ReceiverViewModel> lstReceiverViewModel = new List<ReceiverViewModel>();
                if (Model != null)
                {
                    foreach (Receiver model in Model)
                        lstReceiverViewModel.Add(model.ToViewModel());
                }
                return PartialView("_ReceiversTable", lstReceiverViewModel);
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.StatusCode.ToString());
                return RedirectToRoute("ErrorExpired");
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="receiverViewModel">The receiver view model.</param>
        /// <returns></returns>
        // POST: Receivers/Edit/5
        [HttpPost]
        public async Task<ActionResult> Update(ReceiverViewModel receiverViewModel)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                string json = JsonConvert.SerializeObject(receiverViewModel.ToDataEntity());
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage resp = await client.PutAsync(client.BaseAddress + "/api/receivers/UpdateTaxPayer", httpContent);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    int taxpayerId = await resp.Content.ReadAsAsync<int>();
                    if (taxpayerId > 0)
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                }
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "ReceiversController", resp.RequestMessage.ToString());
            }
            return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the list of class of type t.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Account">The account.</param>
        /// <param name="WebApiLink">The web API link.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="method">The method.</param>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Error in getting request from WebAPI: " + WebApiLink + id</exception>
        private async Task<IEnumerable<T>> GetListOfClassOfTypeT<T>(Account Account, string WebApiLink, int? id, string method = "GET", string json = "")
        {
            List<T> ListOfClassOfTypeT;
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            try
            {
                HttpResponseMessage resp = null;
                switch (method)
                {
                    case "DELETE":
                        resp = await client.DeleteAsync(client.BaseAddress + WebApiLink + id);
                        break;

                    case "POST":
                        StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                        resp = await client.PostAsync(client.BaseAddress + WebApiLink, httpContent);
                        break;

                    default:
                        resp = await client.GetAsync(client.BaseAddress + "/" + WebApiLink + id);
                        break;
                }

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    ListOfClassOfTypeT = await resp.Content.ReadAsAsync<List<T>>();
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "ReceiversController - GetListOfClassOfTypeT", resp.ReasonPhrase);
                    LoggerHelper.LogWebError("FiscalRecords", "ReceiversController - GetListOfClassOfTypeT", resp.RequestMessage.ToString());
                    throw new System.Exception("Error in getting request from WebAPI: " + WebApiLink + id);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
            return ListOfClassOfTypeT;
        }
    }
}