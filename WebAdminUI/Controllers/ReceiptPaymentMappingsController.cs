// --------------------------------------------------------------------
// <copyright file="ReceiptPaymentMappingsController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.CashReceiptsMappers;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;
using WebAdminUI.Models.ReceiptPaymentMapping;

namespace WebAdminUI.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordSetup)]
    public class ReceiptPaymentMappingsController : BaseController
    {
        // POST: ChargeCredits/Create
        /// <summary>
        /// Creates the specified collection.
        /// </summary>
        /// <param name="receiptPaymentMethodMappingViewModel">The receipt payment method mapping view model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Create(ReceiptPaymentMethodMappingViewModel receiptPaymentMethodMappingViewModel)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                string json = JsonConvert.SerializeObject(receiptPaymentMethodMappingViewModel.ToViewModel());
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/CashReceipts/receiptpaymentmappings", httpContent);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    int taxpayerId = await resp.Content.ReadAsAsync<int>();
                    return taxpayerId;
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.ReasonPhrase);
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.RequestMessage.ToString());
                    return 0;
                }
            }
            return 0;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.DeleteAsync(client.BaseAddress + "/api/CashReceipts/receiptpaymentmappings?id=" + id);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    int deleted = await resp.Content.ReadAsAsync<int>();
                    return deleted;
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.ReasonPhrase);
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.RequestMessage.ToString());
                    return 0;
                }
            }

            return 0;
        }

        /// <summary>
        /// Gets the charge credits.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetChargeCredits(string id)
        {
            List<FiscalRecordCatalog> LstChargeCreditCatalog = new List<FiscalRecordCatalog>();
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/catalogs?name=" + Catalog.ChargeCredit + "&code=" + id);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                LstChargeCreditCatalog = await resp.Content.ReadAsAsync<List<FiscalRecordCatalog>>(); ;
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.RequestMessage.ToString());
            }
            return Json(LstChargeCreditCatalog, JsonRequestBehavior.AllowGet);
        }

        // GET: ChargeCredits
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> List()
        {
            List<ReceiptPaymentMethodMappingViewModel> LstChargeCreditMappingViewModel = new List<ReceiptPaymentMethodMappingViewModel>();
            List<ReceiptPaymentMethodMapping> LstChargeCreditMappingModel;
            List<FiscalRecordCatalog> LstRecieptCharge;
            List<FiscalRecordCatalog> LstPaymentType;

            #region Get ChargeCreditMappings

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/CashReceipts/GetChargeCreditMapping");
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                LstChargeCreditMappingModel = await resp.Content.ReadAsAsync<List<ReceiptPaymentMethodMapping>>();
                if (LstChargeCreditMappingModel != null)
                {
                    foreach (var model in LstChargeCreditMappingModel)
                    {
                        LstChargeCreditMappingViewModel.Add(model.ToViewModel());
                    }
                }
                resp = await client.GetAsync(client.BaseAddress + "/api/catalogs?name=" + Catalog.ReceiptCharge);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    LstRecieptCharge = await resp.Content.ReadAsAsync<List<FiscalRecordCatalog>>();
                    resp = await client.GetAsync(client.BaseAddress + "/api/catalogs?name=" + Catalog.PaymentType);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        LstPaymentType = new List<FiscalRecordCatalog>();

                        FiscalRecordCatalog item = new FiscalRecordCatalog();
                        item.Description = WebAdminUI.Views.ReceiptPaymentMappings.App_LocalResources.ReceiptPayment.lblSelect;
                        LstPaymentType.Add(item);
                        List<FiscalRecordCatalog> paymmentList = await resp.Content.ReadAsAsync<List<FiscalRecordCatalog>>();
                        foreach (var payment in paymmentList)
                        {
                            LstPaymentType.Add(payment);
                        }
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.ReasonPhrase);
                        LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.StatusCode.ToString());
                        return RedirectToRoute("ErrorExpired");
                    }
                    else
                    {
                        LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.ReasonPhrase);
                        LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.StatusCode.ToString());
                        return RedirectToRoute("ErrorException");
                    }
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.ReasonPhrase);
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.StatusCode.ToString());
                    return RedirectToRoute("ErrorExpired");
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.ReasonPhrase);
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.StatusCode.ToString());
                    return RedirectToRoute("ErrorException");
                }
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.StatusCode.ToString());
                return RedirectToRoute("ErrorExpired");
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiptPaymentMappingsController", resp.StatusCode.ToString());
                return RedirectToRoute("ErrorException");
            }
            ReceiptPaymentMethodMappingViewModel receiptPaymentMethodMappingViewModel = new ReceiptPaymentMethodMappingViewModel();
            receiptPaymentMethodMappingViewModel.ReceiptPaymentMethodList = LstChargeCreditMappingViewModel;
            receiptPaymentMethodMappingViewModel.PaymentTypeList = LstPaymentType;
            receiptPaymentMethodMappingViewModel.ReceiptCodeList = LstRecieptCharge;
            return View(receiptPaymentMethodMappingViewModel);

            #endregion Get ChargeCreditMappings
        }
    }
}