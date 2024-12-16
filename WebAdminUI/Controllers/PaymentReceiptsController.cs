// --------------------------------------------------------------------
// <copyright file="PaymentReceiptsController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
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
using WebAdminUI.Views.FiscalRecords.App_LocalResources;

namespace WebAdminUI.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// PaymentReceipts Controller
    /// </summary>
    /// <seealso cref="T:WebAdminUI.Controllers.BaseController" />
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
    public class PaymentReceiptsController : BaseController
    {
        /// <summary>
        /// Creates a Payment Receipt from an existing fiscal record
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);

            HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}/api/fiscalrecords/{Account.InvoiceHeaderId}");
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                FiscalRecord fiscalRecord = await resp.Content.ReadAsAsync<FiscalRecord>();
                if (fiscalRecord.PaymentMethod.StartsWith(Constants.PPDPaymentMethod))
                {
                    resp = await client.GetAsync($"{client.BaseAddress}/api/fiscalrecords/PaymentComplement/{Account.InvoiceHeaderId}/{Account.ReceiptNumber}");
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        PaymentComplement paymentComplement = await resp.Content.ReadAsAsync<PaymentComplement>();
                        resp = await client.GetAsync($"{client.BaseAddress}/api/CashReceipts/GetChargeCreditMapping");
                        if (resp.StatusCode == HttpStatusCode.OK)
                        {
                            List<ReceiptPaymentMethodMapping> paymentsMapping = await resp.Content.ReadAsAsync<List<ReceiptPaymentMethodMapping>>();
                            resp = await client.GetAsync($"{client.BaseAddress}/api/Catalogs/CFDIUsage");
                            if (resp.StatusCode == HttpStatusCode.OK)
                            {
                                List<CFDIUsageCatalog> cfdiUsageList = await resp.Content.ReadAsAsync<List<CFDIUsageCatalog>>();
                                Models.FiscalRecords.PaymentReceiptViewModel paymentReceipt = fiscalRecord.ToViewModelPaymentReceipt(paymentComplement, paymentsMapping, cfdiUsageList, Account);
                                ViewBag.Account = Account;

                                return await Task.FromResult(View(paymentReceipt));
                            }
                        }
                    }
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - PaymentReceiptsController", "The fiscal record is not a PPD");
                    return await Task.FromResult(RedirectToRoute("ErrorException"));
                }
            }

            LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - PaymentReceiptsController", resp.RequestMessage.ToString());
            LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - PaymentReceiptsController", resp.ReasonPhrase);
            return await Task.FromResult(RedirectToRoute("ErrorException"));
        }

        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="createPaymentReceipt">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(CreatePaymentReceipt createPaymentReceipt)
        {
            if (ModelState.IsValid)
            {
                if (createPaymentReceipt.Comments != null && createPaymentReceipt.Comments.Contains("|"))
                    return Json(new { id = -1, message = CreateResource.InvalidCharacterMessage },
                                JsonRequestBehavior.AllowGet);

                createPaymentReceipt.CFDIRelatedId = Account.InvoiceHeaderId;
                createPaymentReceipt.ReceiptNumber = int.Parse(Account.ReceiptNumber);

                HttpClient client = PowerCampusHttpClient.GetClient(Account);

                string json = JsonConvert.SerializeObject(createPaymentReceipt);
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/FiscalRecords/CreatePaymentReceipt", httpContent);

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    int invoiceHeaderId = await resp.Content.ReadAsAsync<int>();
                    if (invoiceHeaderId == 0)
                        return Json(new { id = invoiceHeaderId, message = CustomMessages.Error },
                                    JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { id = invoiceHeaderId, message = CustomMessages.Success },
                                    JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", nameof(PaymentReceiptsController), resp.ReasonPhrase);
                    LoggerHelper.LogWebError("FiscalRecords", nameof(PaymentReceiptsController), resp.RequestMessage.ToString());
                    return Json(new { id = 0, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new
            {
                id = 0,
                message =
                string.Join("<br/>",
                            ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).Distinct().ToList())
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Sets the invoiceHeaderId in session and redirects the user to the right action
        /// </summary>
        /// <param name="invoiceHeaderId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(int invoiceHeaderId)
        {
            Account account = Account;
            account.InvoiceHeaderId = invoiceHeaderId;
            Account = account;
            return await Task.FromResult(RedirectToAction("Create"));
        }
    }
}