// --------------------------------------------------------------------
// <copyright file="FiscalRecordsController.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.CashReceiptsMappers;
using WebAdminUI.Config;
using WebAdminUI.Filter;
using WebAdminUI.FiscalRecordsMappers;
using WebAdminUI.Helpers;
using WebAdminUI.Mappers;
using WebAdminUI.Models.FiscalRecords;
using WebAdminUI.Models.Issuers;
using WebAdminUI.Models.PaymentReceipt;
using WebAdminUI.Models.Receivers;
using WebAdminUI.Models.Resources;
using WebAdminUI.Views.FiscalRecords.App_LocalResources;

namespace WebAdminUI.Controllers
{
    /// <inheritdoc/>
    /// <summary>
    /// FiscalRecords Controller
    /// </summary>
    /// <seealso cref="T:System.Web.Mvc.Controller"/>
    public class FiscalRecordsController : BaseController
    {
        // GET: Details
        /// <summary>
        /// Get all the fiscal records related
        /// </summary>
        /// <param name="related"></param>
        /// <returns></returns>
        [HttpGet]
        [ChildActionOnly]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public ActionResult _RelatedList(IEnumerable<FiscalRecordRelatedViewModel> related) => PartialView(related);

        /// <summary>
        /// Get View to cancel the global fiscal record with reason 04.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.GlobalFiscalRecord)]
        public async Task<ActionResult> CancelGlobal(int id)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp;

            resp = await client.GetAsync($"{client.BaseAddress}/api/fiscalrecords/{id}");

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                FiscalRecord fiscalRecord = await resp.Content.ReadAsAsync<FiscalRecord>();

                if (fiscalRecord.CancelReasonKey == "04")
                {
                    FiscalRecordViewModel fiscalRecordViewModel = fiscalRecord.ToViewModelForEdit(Account, true);
                    return View(fiscalRecordViewModel);
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords",
                                         nameof(FiscalRecordsController),
                                         resp.RequestMessage.ToString());
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> Create(int? id)
        {
            bool isSubstitution = id.HasValue && id > 0;

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp;
            if (isSubstitution)
                resp = await client.GetAsync($"{client.BaseAddress}/api/fiscalrecords/forsubstitution/{id.Value}");
            else
                resp = await client.GetAsync($"{client.BaseAddress}/api/cashreceipts/{Account.ReceiptNumber}");

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                CashReceipt CRModel = await resp.Content.ReadAsAsync<CashReceipt>();
                if (CRModel.chargesToInvoiceList?.Count > 0)
                {
                    ViewBag.ReceiverPanelModel = new ReceiverViewModel()
                    {
                        InvoiceOrganization = new InvoiceOrganizationViewModel()
                        {
                            ListTaxRegimenCatalogs = new List<TaxRegimenCatalog>()
                        },
                        ListStates = await GetStates()
            
                    };
                    return View(CRModel.ToViewModel(Account));
                }
                return RedirectToAction(nameof(Index), "Home");
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorExpired");
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Creates the specified fiscal record model.
        /// </summary>
        /// <param name="fiscalRecordModel">The fiscal record model.</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> Create(CreateFiscalRecord fiscalRecordModel)
        {
            if (fiscalRecordModel.Comments == null)
                fiscalRecordModel.Comments = string.Empty;

            if (fiscalRecordModel.PaymentCondition == null)
                fiscalRecordModel.PaymentCondition = string.Empty;

            if (ModelState.IsValid)
            {
                if (fiscalRecordModel.Detail.Count <= 0)
                {
                    return Json(new { id = -1, message = CreateResource.ReceiptsToInvoiceEmpty },
                                JsonRequestBehavior.AllowGet);
                }

                if (fiscalRecordModel.Comments.Contains("|"))
                    return Json(new { id = -1, message = CreateResource.InvalidCharacterMessage },
                                JsonRequestBehavior.AllowGet);
                if (fiscalRecordModel.PaymentCondition.Contains("|"))
                    return Json(new { id = -1, message = CreateResource.InvalidCharacterMessage },
                                JsonRequestBehavior.AllowGet);

                string total = fiscalRecordModel.Total.Replace("$", string.Empty);
                string subtotal = fiscalRecordModel.Subtotal.Replace("$", string.Empty);
                string totalTax = fiscalRecordModel.TotalTransferTaxes.Replace("$", string.Empty);
                if (Convert.ToDecimal(subtotal, CultureInfo.InvariantCulture) == 0)
                    return Json(new { id = -1, message = CreateResource.InvalidSubTotalAmount },
                                JsonRequestBehavior.AllowGet);
                if (Convert.ToDecimal(total, CultureInfo.InvariantCulture) == 0)
                    return Json(new { id = -1, message = CreateResource.InvalidTotalAmount },
                                JsonRequestBehavior.AllowGet);

                foreach (FiscalRecordDetail item in fiscalRecordModel.Detail)
                {
                    if (string.IsNullOrEmpty(item.ProductServiceKey))
                        return Json(new { id = -1, message = CreateResource.ProductServiceCodeRequired },
                                    JsonRequestBehavior.AllowGet);
                    if (string.IsNullOrEmpty(item.Description))
                        return Json(new { id = -1, message = CreateResource.ChargeCreditDescRequired },
                                    JsonRequestBehavior.AllowGet);
                    if (item.Description.Contains("|") || item.UnitDescription.Contains("|"))
                        return Json(new { id = -1, message = CreateResource.InvalidCharacterMessage },
                                    JsonRequestBehavior.AllowGet);
                }
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                string json = JsonConvert.SerializeObject(fiscalRecordModel);
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage resp = await client.PostAsync($"{client.BaseAddress}/api/FiscalRecords/CreateFiscalRecord",
                                                                  httpContent);
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
                    LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                    LoggerHelper.LogWebError("FiscalRecords",
                                             nameof(FiscalRecordsController),
                                             resp.RequestMessage.ToString());
                    return Json(new { id = 0, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new
            {
                id = 0,
                message =
                string.Join("<br/>",
                            ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).Distinct().ToList())
            },
                        JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Creates the global.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.GlobalFiscalRecord)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> CreateGlobal(int? id)
        {
            bool isSubstitution = id.HasValue && id > 0;

            CashReceiptViewModel modelCR = null;

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp;
            if (isSubstitution)
                resp = await client.GetAsync($"{client.BaseAddress}/api/fiscalrecords/GetForGlobalSubstitution/{id.Value}");
            else
                resp = await client.GetAsync($"{client.BaseAddress}/api/cashreceipts/GetGlobalFiscalRecord");

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                CashReceipt CRModel = await resp.Content.ReadAsAsync<CashReceipt>();
                if (isSubstitution)
                    modelCR = CRModel.ToViewModelForGlobalWithCharges(Account);
                modelCR = CRModel.ToViewModelForGlobal(modelCR);
                return View(modelCR);
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.StatusCode.ToString());
                return RedirectToRoute("ErrorExpired");
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.StatusCode.ToString());
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Deletes the invoce fiscal record.
        /// </summary>
        /// <param name="fiscalRecordRequest">The invoice header identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> DeleteInvoceFiscalRecord(FiscalRecordRequest fiscalRecordRequest)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            string json = JsonConvert.SerializeObject(fiscalRecordRequest);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await client.PostAsync($"{client.BaseAddress}/api/FiscalRecords/DeleteFiscalRecords",
                                                              httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                int invoiceHeaderId = await resp.Content.ReadAsAsync<int>();
                if (invoiceHeaderId <= 0)
                    return Json(new { id = invoiceHeaderId, message = CustomMessages.Error },
                                JsonRequestBehavior.AllowGet);
                else
                    return Json(new { id = invoiceHeaderId, message = CustomMessages.Success },
                                JsonRequestBehavior.AllowGet);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords",
                                         nameof(FiscalRecordsController),
                                         resp.RequestMessage.ToString());
                return Json(new { id = 0, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Edit.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> Edit(int id)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp;

            resp = await client.GetAsync($"{client.BaseAddress}/api/fiscalrecords/{id}");

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                FiscalRecord fiscalRecord = await resp.Content.ReadAsAsync<FiscalRecord>();
                FiscalRecordViewModel fiscalRecordViewModel = fiscalRecord.ToViewModelForEdit(Account);

                if (fiscalRecord.FiscalRecordType == Constants.FiscalRecordTypePago)
                {
                    HttpResponseMessage documentsRelatedResponse = await client.GetAsync($"{client.BaseAddress}/api/fiscalrecords/PaymentReceiptsRelated/{fiscalRecord.InvoiceHeaderId}");
                    List<PaymentReceipt>[] documentsRelated = await documentsRelatedResponse.Content.ReadAsAsync<List<PaymentReceipt>[]>();
                    // 0 - Fiscal Record by id
                    // 1 - Parent
                    // 2 - Childs
                    // 3 - Siblings
                    if (fiscalRecordViewModel.InvoiceDetails.Count == 0)
                    {
                        ViewBag.HasDetails = false;
                        fiscalRecordViewModel.InvoiceDetails = new List<ChargeCreditViewModel>();
                        ChargeCreditViewModel detail = new ChargeCreditViewModel
                        {
                            ChargeCreditCode = Constants.PaymentReceiptChargeCreditCode,
                            ChargeCreditDesc = Constants.PaymentReceiptChargeCreditCDesc,
                            Quantity = 1,
                            TotalUnit = decimal.Zero.ToString(Account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                            UnitAmount = decimal.Zero.ToString(Account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                            UnityKey = Constants.PaymentReceiptUnityKey,
                        };
                        fiscalRecordViewModel.InvoiceDetails.Add(detail);
                    }
                    else
                    {
                        ViewBag.HasDetails = true;
                        PaymentComplement paymentComplement = null;
                        if (fiscalRecord.FiscalRecordDetailList.Count > 0)
                        {
                            decimal baseAmount = fiscalRecord.FiscalRecordDetailList[0].UnitAmount;
                            decimal taxesAmount = fiscalRecord.FiscalRecordDetailList[0].TotalTaxes;

                            fiscalRecordViewModel.PaymentReceiptTotals = new PaymentReceiptTotalsViewModel()
                            {
                                TotalPaymentsAmount = (baseAmount + taxesAmount).ToString(Account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture)
                            };

                            resp = await client.GetAsync($"{client.BaseAddress}/api/fiscalrecords/PaymentComplement/{fiscalRecord.InvoiceHeaderId}/{fiscalRecord.FiscalRecordDetailList[0].ReceiptNumber}");
                            if (resp.StatusCode == HttpStatusCode.OK)
                                paymentComplement = await resp.Content.ReadAsAsync<PaymentComplement>();
                            if (paymentComplement != null && !string.IsNullOrEmpty(paymentComplement.FactorType))
                            {
                                string baseAmountFormatted = fiscalRecord.FiscalRecordDetailList[0].UnitAmount.ToString(Account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
                                string taxesAmountFormatted = fiscalRecord.FiscalRecordDetailList[0].TotalTaxes.ToString(Account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);

                                if (paymentComplement.FactorType.Equals(FactorType.Exento.ToString()))
                                {
                                    fiscalRecordViewModel.PaymentReceiptTotals.TotalTransferredBaseExempt = baseAmountFormatted;
                                }
                                else if (paymentComplement.TransferRate == decimal.Zero)
                                {
                                    fiscalRecordViewModel.PaymentReceiptTotals.TotalTransferredBase0 = baseAmountFormatted;
                                    fiscalRecordViewModel.PaymentReceiptTotals.TotalTransferredTaxes0 = taxesAmountFormatted;
                                }
                                else if (paymentComplement.TransferRate == Constants.IVA8)
                                {
                                    fiscalRecordViewModel.PaymentReceiptTotals.TotalTransferredBase8 = baseAmountFormatted;
                                    fiscalRecordViewModel.PaymentReceiptTotals.TotalTransferredTaxes8 = taxesAmountFormatted;
                                }
                                else if (paymentComplement.TransferRate == Constants.IVA16)
                                {
                                    fiscalRecordViewModel.PaymentReceiptTotals.TotalTransferredBase16 = baseAmountFormatted;
                                    fiscalRecordViewModel.PaymentReceiptTotals.TotalTransferredTaxes16 = taxesAmountFormatted;
                                }
                            }
                        }
                    }

                    PaymentReceipt paymentReceipt = documentsRelated[0].Single();

                    FiscalRecordCatalog paymentTypeForComplement = fiscalRecord.PaymentTypeList
                                                                               .SingleOrDefault(f => f.Code == paymentReceipt.PaymentTypeComplement);
                    paymentReceipt.PaymentTypeComplement = paymentTypeForComplement?.Description;

                    PaymentReceipt ppd = documentsRelated[1].Single();
                    if (paymentReceipt.IssuerTaxpayerIdSourceAccount.HasValue)
                    {
                        HttpResponseMessage respReceiver = await client.GetAsync($"{client.BaseAddress}/api/receivers/GetTaxPayerbyId/?id={paymentReceipt.IssuerTaxpayerIdSourceAccount.Value}&foreignId=");
                        if (respReceiver.StatusCode == HttpStatusCode.OK)
                        {
                            Receiver receiver = await respReceiver.Content.ReadAsAsync<Receiver>();
                            paymentReceipt.IssuerTaxpayerIdSourceAccountName = receiver.TaxPayerId;
                        }
                    }
                    if (paymentReceipt.IssuerTaxpayerIdBeneficiaryAccount.HasValue)
                    {
                        HttpResponseMessage respReceiver = await client.GetAsync($"{client.BaseAddress}/api/receivers/GetTaxPayerbyId/?id={paymentReceipt.IssuerTaxpayerIdBeneficiaryAccount.Value}&foreignId=");
                        if (respReceiver.StatusCode == HttpStatusCode.OK)
                        {
                            Receiver receiver = await respReceiver.Content.ReadAsAsync<Receiver>();
                            paymentReceipt.IssuerTaxpayerIdBeneficiaryAccountName = receiver.TaxPayerId;
                        }
                    }

                    resp = await client.GetAsync($"{client.BaseAddress}/api/fiscalrecords/{ppd.InvoiceHeaderId}");
                    FiscalRecord parentPPD = await resp.Content.ReadAsAsync<FiscalRecord>();

                    ViewBag.itself = paymentReceipt;
                    ViewBag.parentPPD = parentPPD.ToViewModelForEdit(Account);
                    ViewBag.Account = Account;

                    return View("EditPaymentReceipt", fiscalRecordViewModel);
                }

                return View(fiscalRecordViewModel);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords",
                                         nameof(FiscalRecordsController),
                                         resp.RequestMessage.ToString());
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Get Format Date.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<string> GetFormatDate()
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}/api/Settings/Value?area=SYSADMIN&section=DATE_FORMAT&label=REPORT_FORMAT");
            string result = await resp.Content.ReadAsAsync<string>();
            //resp.EnsureSuccessStatusCode();
            return result;
        }

        /// <summary>
        /// Gets the global cash receipts.
        /// </summary>
        /// <param name="globalInvoiceFiltersForCreation">The global invoice filters for creation.</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<PartialViewResult> GetGlobalCashReceipts(GlobalInvoiceFiltersForCreation globalInvoiceFiltersForCreation)
        {
            CashReceiptViewModel modelCR = new CashReceiptViewModel();

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            string json = JsonConvert.SerializeObject(globalInvoiceFiltersForCreation);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await client.PostAsync($"{client.BaseAddress}/api/CashReceipts/GetGlobalCashReceipts", httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                CashReceipt CRModel = await resp.Content.ReadAsAsync<CashReceipt>();
                modelCR = CRModel.ToViewModelForGlobalWithCharges(Account);
            }

            return PartialView("GlobalDetailsView", modelCR);
        }

        /// <summary>
        /// Gets the global cash receipts totals.
        /// </summary>
        /// <param name="globalInvoiceFiltersForCreation">The global invoice filters for creation.</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> GetGlobalCashReceiptsTotals(GlobalInvoiceFiltersForCreation globalInvoiceFiltersForCreation)
        {
            CashReceiptViewModel modelCR = new CashReceiptViewModel();

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            string json = JsonConvert.SerializeObject(globalInvoiceFiltersForCreation);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await client.PostAsync($"{client.BaseAddress}/api/CashReceipts/GetGlobalCashReceipts", httpContent);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                CashReceipt CRModel = await resp.Content.ReadAsAsync<CashReceipt>();

                return Json(new
                {
                    Subtotal = CRModel.SubTotal.ToString(Account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                    TotalTaxes = CRModel.TotalTT.ToString(Account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                    Total = CRModel.Total.ToString(Account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture)
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "FiscalRecordsController", resp.RequestMessage.ToString());
                LoggerHelper.LogWebError("FiscalRecords", "FiscalRecordsController", resp.ReasonPhrase);
                return Json(new { id = 0, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets the available global invoice filters.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> GetGlobalInvoiceFilters(string startDate, string endDate)
        {
            if (Account.status == enumAccess.Authorized)
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}/api/FiscalRecords/GetGlobalInvoiceFilters/?startDate={startDate}&endDate={endDate}");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    GlobalInvoiceFilters globalInvoiceFilters = await resp.Content.ReadAsAsync<GlobalInvoiceFilters>();
                    return Json(globalInvoiceFilters, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "FiscalRecordsController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("FiscalRecords", "FiscalRecordsController", resp.ReasonPhrase);
                    return Json(new { id = 0, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets the global cash receipts.
        /// </summary>
        /// <param name="globalInvoiceFiltersForCreation">The global invoice filters for creation.</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> GetGlobalPaymentType(GlobalInvoiceFiltersForCreation globalInvoiceFiltersForCreation)
        {
            List<FiscalRecordCatalog> LstPaymentType;
            List<FiscalRecordCatalog> LstPaymentTypeCatalog = new List<FiscalRecordCatalog>();

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            string json = JsonConvert.SerializeObject(globalInvoiceFiltersForCreation);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await client.PostAsync($"{client.BaseAddress}/api/CashReceipts/GetGlobalPaymentType", httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                LstPaymentType = await resp.Content.ReadAsAsync<List<FiscalRecordCatalog>>();
                HttpResponseMessage respPaymentType = await client.GetAsync($"{client.BaseAddress}/api/catalogs?name={Catalog.PaymentType}");
                if (respPaymentType.StatusCode == HttpStatusCode.OK)
                {
                    LstPaymentTypeCatalog = await respPaymentType.Content.ReadAsAsync<List<FiscalRecordCatalog>>();
                    LstPaymentTypeCatalog = (from elements in LstPaymentTypeCatalog
                                             where elements.Code != "99"
                                             select elements).ToList();
                }
                if (LstPaymentType.Count > 0)
                {
                    foreach (FiscalRecordCatalog row in LstPaymentType)
                    {
                        LstPaymentTypeCatalog.Where(x => x.Code == row.Code).First().Description = row.Description;
                        LstPaymentTypeCatalog.Where(x => x.Code == row.Code).First().Status = "1";
                    }
                }
            }

            return Json(LstPaymentTypeCatalog, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get the invoice filters options
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInvoiceFilters()
        {
            FiscalRecordCatalog allStatusOption = new FiscalRecordCatalog
            {
                Id = (int)enumFiscalRecordStatus.Null,
                Description = ViewAllResource.lblAll
            };

            List<FiscalRecordCatalog> statusList = new List<FiscalRecordCatalog>()
            {
                new FiscalRecordCatalog
                {
                    Id = (int)enumFiscalRecordStatus.Active,
                    Description = FiscalRecordModelResource.Active
                },
                new FiscalRecordCatalog
                {
                    Id = (int)enumFiscalRecordStatus.RequestedCancellation,
                    Description = FiscalRecordModelResource.RequestedCancellation
                },
                new FiscalRecordCatalog
                {
                    Id = (int)enumFiscalRecordStatus.Canceled,
                    Description = FiscalRecordModelResource.Canceled
                },
                new FiscalRecordCatalog
                {
                    Id = (int)enumFiscalRecordStatus.RequestedFiscalRecord,
                    Description = FiscalRecordModelResource.RequestedFiscalRecord
                },
                new FiscalRecordCatalog
                {
                    Id = (int)enumFiscalRecordStatus.ProviderIsCanceling,
                    Description = FiscalRecordModelResource.ProviderIsCanceling
                },
                new FiscalRecordCatalog
                {
                    Id = (int)enumFiscalRecordStatus.ProviderIsCreating,
                    Description = FiscalRecordModelResource.ProviderIsCreating
                },
                new FiscalRecordCatalog
                {
                    Id = (int)enumFiscalRecordStatus.ProviderCannotCreate,
                    Description = FiscalRecordModelResource.ProviderCannotCreate
                },
                new FiscalRecordCatalog
                {
                    Id = (int)enumFiscalRecordStatus.ProviderCannotCancel,
                    Description = FiscalRecordModelResource.ProviderCannotCancel
                }
            };

            List<FiscalRecordCatalog> recordTypeList = new List<FiscalRecordCatalog>()
            {
                new FiscalRecordCatalog
                {
                    Code = ViewAllResource.lblAll,
                    Description = ViewAllResource.lblAll
                },
                new FiscalRecordCatalog
                {
                    Code = Constants.FiscalRecordTypeIngresoDesc,
                    Description = Constants.FiscalRecordTypeIngresoDesc
                },
                new FiscalRecordCatalog
                {
                    Code = Constants.FiscalRecordTypeEgresoDesc,
                    Description = Constants.FiscalRecordTypeEgresoDesc
                },
                new FiscalRecordCatalog
                {
                    Code = Constants.FiscalRecordTypePagoDesc,
                    Description = Constants.FiscalRecordTypePagoDesc
                }
            };

            return Json(new
            {
                StatusList = statusList,
                RecordTypeList = recordTypeList,
                AllStatusOption = allStatusOption
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(string guid)

        {
            if (string.IsNullOrEmpty(guid))
            {
                return RedirectToAction(nameof(Index), "Home");
            }
            Account = await PowerCampusAccess.IsAuthorized(guid);
            if (Account.status == enumAccess.Authorized)
            {
                if (Account.Action == EnumFiscalRecordAction.CashReceipts)
                    return RedirectToAction("Application", "CashReceipts");
                else if (Account.Action == EnumFiscalRecordAction.Menu)
                    return RedirectToAction(nameof(Menu), "FiscalRecords");
                else if (Account.Action == EnumFiscalRecordAction.Settings)
                    return RedirectToAction(nameof(Settings), "FiscalRecords");
                else if (Account.Action == EnumFiscalRecordAction.ViewAll)
                {
                    if (string.IsNullOrEmpty(Account.PeopleOrgCodeId))
                        return RedirectToAction(nameof(ViewAll), "FiscalRecords");
                    else
                        return RedirectToAction(nameof(ViewByPeopleOrgId), "FiscalRecords");
                }
                else
                    return RedirectToAction(nameof(Index), "Home");
            }
            else
            {
                return RedirectToAction("Unauthorized", "Error");
            }
        }

        /// <summary>
        /// Menus this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public ActionResult Menu() => View();

        /// <summary>
        /// Gets the partial view to create new global invoice after cancelation 04 of global invoice.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> NewGlobalInvoicePartialView(int invoiceHeaderId)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}/api/cashreceipts/GetGlobalFiscalRecord");
            HttpResponseMessage respCharges = await client.GetAsync($"{client.BaseAddress}/api/CashReceipts/GetGlobalInvoiceDetail/{invoiceHeaderId}");
            HttpResponseMessage respHeader = await client.GetAsync($"{client.BaseAddress}/api/FiscalRecords/InvoiceHeader/{invoiceHeaderId}");

            if (resp.StatusCode == HttpStatusCode.OK && respCharges.StatusCode == HttpStatusCode.OK && respHeader.StatusCode == HttpStatusCode.OK)
            {
                CashReceipt CRModel = await resp.Content.ReadAsAsync<CashReceipt>();
                FiscalRecord fiscalRecord = await respHeader.Content.ReadAsAsync<FiscalRecord>();
                CashReceiptViewModel cashReceiptViewModel = CRModel.ToViewModelForGlobal(null, fiscalRecord, Account);

                CashReceipt cashReceiptWithCharges = await respCharges.Content.ReadAsAsync<CashReceipt>();
                CashReceiptViewModel cashReceiptWithChargesViewModel = cashReceiptWithCharges.ToViewModelForGlobalWithCharges(Account);
                cashReceiptViewModel.SubTotal = cashReceiptWithChargesViewModel.SubTotal;
                cashReceiptViewModel.TotalTT = cashReceiptWithChargesViewModel.TotalTT;
                cashReceiptViewModel.Total = cashReceiptWithChargesViewModel.Total;
                cashReceiptViewModel.IsCancellation04 = true;

                cashReceiptViewModel.chargesToInvoiceList = cashReceiptWithChargesViewModel.chargesToInvoiceList;

                HttpResponseMessage respPayment = await client.GetAsync($"{client.BaseAddress}/api/CashReceipts/CancelledGlobalPaymentType/{invoiceHeaderId}");
                if (respPayment.StatusCode == HttpStatusCode.OK)
                {
                    List<FiscalRecordCatalog> LstPaymentType = await respPayment.Content.ReadAsAsync<List<FiscalRecordCatalog>>();
                    if (LstPaymentType.Count > 0)
                    {
                        foreach (FiscalRecordCatalog row in LstPaymentType)
                        {
                            cashReceiptViewModel.PaymentTypeList.Where(x => x.Code == row.Code).First().Description = row.Description;
                            cashReceiptViewModel.PaymentTypeList.Where(x => x.Code == row.Code).First().Status = "1";
                        }
                    }
                }

                return PartialView("_NewGlobalInvoiceCreation", cashReceiptViewModel);
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase + respCharges.ReasonPhrase);
                return RedirectToRoute("ErrorExpired");
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase + respCharges.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Gets the partial view to create new invoice after cancelation 04 of global invoice.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> NewInvoicePartialView(int receiptNumber)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp;
            resp = await client.GetAsync($"{client.BaseAddress}/api/cashreceipts/{receiptNumber}");

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                CashReceipt CRModel = await resp.Content.ReadAsAsync<CashReceipt>();
                if (CRModel.chargesToInvoiceList?.Count > 0)
                    return PartialView("_NewInvoiceCreation", CRModel.ToViewModel(Account));
                else
                    return PartialView("Error", "Home");
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorExpired");
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Gets the NewInvoiceTables partial view.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.GlobalFiscalRecord)]
        public async Task<PartialViewResult> NewInvoiceTables(int invoiceHeaderId)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);

            HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}/api/FiscalRecords/GlobalInvoiceDetails/{invoiceHeaderId}");

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                GlobalInvoiceCancellationDetails globalInvoiceCancellationDetails = await resp.Content.ReadAsAsync<GlobalInvoiceCancellationDetails>();
                GlobalInvoiceDetailViewModel globalInvoiceDetailViewModel = globalInvoiceCancellationDetails.ToViewModel(Account);

                return PartialView("_NewInvoiceTables", globalInvoiceDetailViewModel);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                return PartialView("Error", "Home");
            }
        }

        /// <summary>
        /// Get the payment receipts associated to a PPD
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> PaymentReceiptsRelated(int id)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp2 = await client.GetAsync($"{client.BaseAddress}/api/FiscalRecords/Related/{id}");

            if (resp2.StatusCode == HttpStatusCode.OK && resp2.StatusCode == HttpStatusCode.OK)
            {
                List<FiscalRecord>[] modelResult = await resp2.Content.ReadAsAsync<List<FiscalRecord>[]>();
                if (modelResult != null)
                {
                    ViewBag.InvoiceHeaderId = id;
                    ViewBag.ShowSupplementChildren = true;
                    List<FiscalRecord>[] newModelResult = new List<FiscalRecord>[2];
                    newModelResult[0] = modelResult[0];

                    List<FiscalRecord> secondList = new List<FiscalRecord>();
                    if (modelResult[1] != null)
                    {
                        foreach (FiscalRecord fiscalRecordItem in modelResult[1])
                        {
                            secondList.Add(fiscalRecordItem);
                        }
                    }
                    if (modelResult[2] != null)
                    {
                        foreach (FiscalRecord fiscalRecordItem in modelResult[2])
                        {
                            secondList.Add(fiscalRecordItem);
                        }
                    }

                    newModelResult[1] = secondList;

                    FiscalRecordOriginViewModel model = newModelResult.ToViewModel();
                    return View(nameof(Related), model);
                }
                return RedirectToRoute("ErrorException");
            }
            else if (resp2.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp2.ReasonPhrase);
                return RedirectToRoute("ErrorExpired");
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp2.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// PDFs the download.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="idc">The idc.</param>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<FileResult> PDFDownload(int id, int? idc)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp;

            resp = await client.GetAsync($"{client.BaseAddress}/api/fiscalrecords/{id}/certificateFiles/{idc}");

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                //resp.EnsureSuccessStatusCode();

                FiscalRecordCertificate fiscalRecordCertificate = await resp.Content
                                                                            .ReadAsAsync<FiscalRecordCertificate>();
                byte[] pdfByte = fiscalRecordCertificate.PdfFile;
                return File(pdfByte, "application/pdf", "PdfFile.pdf");
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords",
                                         nameof(FiscalRecordsController),
                                         resp.RequestMessage.ToString());
                return null;
            }
        }

        /// <summary>
        /// Processes the global.
        /// </summary>
        /// <param name="fiscalRecordModel">The fiscal record model.</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> ProcessGlobal(CreateFiscalRecord fiscalRecordModel)
        {
            if (fiscalRecordModel.PeopleOrgCodeId is null)
                fiscalRecordModel.PeopleOrgCodeId = string.Empty;

            if (fiscalRecordModel.Comments is null)
                fiscalRecordModel.Comments = string.Empty;

            if (fiscalRecordModel.PaymentCondition is null)
                fiscalRecordModel.PaymentCondition = string.Empty;

            string emailRegularExpression = PowerCampusSystemFormat.GetMailValidation();
            string email = fiscalRecordModel.ReceiverEmail;
            if (!Regex.IsMatch(email.Trim(), emailRegularExpression))
            {
                return Json(new { id = -1, message = string.Empty },
                                        JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                if (fiscalRecordModel.Detail.Count > 0)
                {
                    #region Validations

                    string total, subtotal, totalTax;

                    if (fiscalRecordModel.Comments.Contains("|"))
                        return Json(new { id = -2, message = CreateResource.InvalidCharacterMessage },
                                    JsonRequestBehavior.AllowGet);

                    if (!string.IsNullOrEmpty(fiscalRecordModel.Total))
                    {
                        total = fiscalRecordModel.Total.Replace("$", string.Empty);
                        if (Convert.ToDecimal(total, CultureInfo.InvariantCulture) == 0)
                            return Json(new { id = -2, message = CreateResource.InvalidTotalAmount },
                                        JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { id = -2, message = CreateResource.InvalidTotalAmount },
                                    JsonRequestBehavior.AllowGet);

                    if (!string.IsNullOrEmpty(fiscalRecordModel.Subtotal))
                    {
                        subtotal = fiscalRecordModel.Subtotal.Replace("$", string.Empty);
                        if (Convert.ToDecimal(subtotal, CultureInfo.InvariantCulture) == 0)
                            return Json(new { id = -2, message = CreateResource.InvalidSubTotalAmount },
                                        JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { id = -2, message = CreateResource.InvalidSubTotalAmount },
                                    JsonRequestBehavior.AllowGet);

                    if (!string.IsNullOrEmpty(fiscalRecordModel.TotalTransferTaxes))
                        totalTax = fiscalRecordModel.TotalTransferTaxes.Replace("$", string.Empty);

                    #endregion Validations

                    #region Details of Fiscal Record

                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    string json = JsonConvert.SerializeObject(fiscalRecordModel);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync($"{client.BaseAddress}/api/FiscalRecords/CreateGlobalFiscalRecord", httpContent);
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
                        LoggerHelper.LogWebError("FiscalRecords",
                                                 nameof(FiscalRecordsController),
                                                 resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                    }

                    #endregion Details of Fiscal Record
                }
                return Json(new { id = -2, message = CreateResource.ReceiptsToInvoiceEmpty },
                            JsonRequestBehavior.AllowGet);
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
        /// Get the fiscal records associated to a fiscal record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> Related(int id)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}/api/FiscalRecords/Related/{id}");

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.InvoiceHeaderId = id;
                List<FiscalRecord>[] modelResult = await resp.Content.ReadAsAsync<List<FiscalRecord>[]>();
                FiscalRecord itself = modelResult[0].Single();

                if (modelResult != null)
                {
                    List<FiscalRecord>[] newModelResult = new List<FiscalRecord>[2];
                    newModelResult[0] = modelResult[0];

                    List<FiscalRecord> secondList = new List<FiscalRecord>();
                    if (modelResult[1] != null)
                    {
                        foreach (FiscalRecord fiscalRecordItem in modelResult[1])
                        {
                            secondList.Add(fiscalRecordItem);
                        }
                    }
                    if (modelResult[2] != null)
                    {
                        foreach (FiscalRecord fiscalRecordItem in modelResult[2])
                        {
                            secondList.Add(fiscalRecordItem);
                        }
                    }

                    newModelResult[1] = secondList;

                    FiscalRecordOriginViewModel model = newModelResult.ToViewModel();
                    return View(model);
                }
                return RedirectToRoute("ErrorException");
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorExpired");
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Reprocess.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> Reprocess(FiscalRecordRequest fiscalRecordRequest)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                string json = JsonConvert.SerializeObject(fiscalRecordRequest);
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage resp = await client.PostAsync($"{client.BaseAddress}/api/FiscalRecordRequests",
                                                                  httpContent);

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    int invoiceRequestId = await resp.Content.ReadAsAsync<int>();
                    if (invoiceRequestId > 0)
                    {
                        return Json(new { id = invoiceRequestId, message = CustomMessages.Success });
                    }
                    else
                    {
                        LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                        LoggerHelper.LogWebError("FiscalRecords",
                                                 nameof(FiscalRecordsController),
                                                 resp.RequestMessage.ToString());
                        return Json(new { id = 0, message = CustomMessages.Error });
                    }
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                    LoggerHelper.LogWebError("FiscalRecords",
                                             nameof(FiscalRecordsController),
                                             resp.RequestMessage.ToString());
                    return Json(new { id = 0, message = CustomMessages.Error });
                }
            }

            return Json(new
            {
                id = 0,
                message =
            string.Join("<br/>",
                        ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).Distinct().ToList())
            });
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="idc">The idc.</param>
        /// <param name="emailTo">The email to.</param>
        /// <param name="folio">The folio.</param>
        /// <param name="serie">The serie.</param>
        /// <param name="uuid">The UUID.</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<int> SendEmail(int id, int? idc, string emailTo, string folio, string serie, string uuid)
        {
            try
            {
                EmailSettings emailSetting = ConfigurationManager.GetSection("emailSettings") as EmailSettings;
                SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                SmtpClient smtpClient = new SmtpClient();
                string emailRegularExpression = PowerCampusSystemFormat.GetMailValidation();
                if (string.IsNullOrEmpty(smtpClient.Host) || string.IsNullOrEmpty(smtpClient.Port.ToString()) || string.IsNullOrEmpty(section.From))
                    return -2;
                string[] emails = emailTo.Split(',');
                foreach (string email in emails)
                {
                    if (!Regex.IsMatch(email.Trim(), emailRegularExpression))
                        return -1;
                }

                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}/api/fiscalrecords/{id}/certificateFiles/{idc}");

                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                    LoggerHelper.LogWebError("FiscalRecords",
                                             nameof(FiscalRecordsController),
                                             resp.RequestMessage.ToString());
                    HttpStatusCodeResult result = new HttpStatusCodeResult(resp.StatusCode);
                    return 0;
                }

                FiscalRecordCertificate fiscalRecordCertificate = await resp.Content.ReadAsAsync<FiscalRecordCertificate>();
                byte[] xmlByte = Encoding.UTF8.GetBytes(fiscalRecordCertificate.XmlFile);
                byte[] pdfByte = null;
                if (idc != null)
                    pdfByte = fiscalRecordCertificate.PdfFile;

                if (!smtpClient.UseDefaultCredentials)
                {
                    if (string.IsNullOrEmpty(section.Network.UserName) || string.IsNullOrEmpty(section.Network.Password))
                        return -2;
                }

                MailAddress fromAddress = new MailAddress(section.From);
                byte[] subjectBytes = Encoding.UTF8.GetBytes(emailSetting.FiscalRecordsSetting.EmailSubject);
                string subject = Encoding.UTF8.GetString(subjectBytes);
                MailMessage message = new MailMessage(
                    fromAddress.ToString(),
                    emailTo,
                    $"{subject} {uuid}",
                    emailSetting.FiscalRecordsSetting.EmailBody
                );
                message.IsBodyHtml = true;

                Attachment attXml = new Attachment(new MemoryStream(xmlByte), $"{uuid}.xml");
                message.Attachments.Add(attXml);
                if (pdfByte != null)
                {
                    Attachment attPdf = new Attachment(new MemoryStream(pdfByte), $"{uuid}.pdf");
                    message.Attachments.Add(attPdf);
                }

                await smtpClient.SendMailAsync(message);
                return 1;
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), e.Message);
                return -2;
            }
        }

        /// <summary>
        /// Settingses this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public ActionResult Settings() => View();

        /// <summary>
        /// View All.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> ViewAll()
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);

            string todaysDate = $"{DateTime.Now.Day:d2}/{DateTime.Now.Month:d2}/{DateTime.Now.Year}";
            InvoiceFilters invoiceFilters = new InvoiceFilters
            {
                StartDate = todaysDate,
                EndDate = todaysDate,
                Status = enumFiscalRecordStatus.Null
            };
            string json = JsonConvert.SerializeObject(invoiceFilters);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await client.PostAsync($"{client.BaseAddress}/api/fiscalrecords/GetFilteredFiscalRecords", httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                return View(GetFiscalRecordViewModelList(await resp.Content.ReadAsAsync<List<FiscalRecord>>(), Account));
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.StatusCode.ToString());
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorExpired");
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// View By People Org Id.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> ViewByPeopleOrgId()
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);

            InvoiceFilters invoiceFilters = new InvoiceFilters
            {
                PeopleOrgCodeId = Account.PeopleOrgCodeId,
                Status = enumFiscalRecordStatus.Null
            };
            string json = JsonConvert.SerializeObject(invoiceFilters);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await client.PostAsync($"{client.BaseAddress}/api/fiscalrecords/GetFilteredFiscalRecords", httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                return View(GetFiscalRecordViewModelList(await resp.Content.ReadAsAsync<List<FiscalRecord>>(), Account));
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.StatusCode.ToString());
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorExpired");
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// View Filtered Fiscal Records.
        /// </summary>
        /// <param name="invoiceFilters">The invoice filters.</param>
        /// <returns></returns>
        [HttpPost]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> ViewFilteredFiscalRecords(InvoiceFilters invoiceFilters)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);

            string json = JsonConvert.SerializeObject(invoiceFilters);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await client.PostAsync($"{client.BaseAddress}/api/fiscalrecords/GetFilteredFiscalRecords", httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                return Json(GetFiscalRecordViewModelList(await resp.Content.ReadAsAsync<List<FiscalRecord>>(), Account),
                            JsonRequestBehavior.AllowGet);
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.StatusCode.ToString());
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorExpired");
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// XMLs the download.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="idc">The idc.</param>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<FileResult> XMLDownload(int id, int? idc)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp;

            resp = await client.GetAsync($"{client.BaseAddress}/api/fiscalrecords/{id}/certificateFiles/{idc}");

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                //resp.EnsureSuccessStatusCode();

                FiscalRecordCertificate fiscalRecordCertificate = await resp.Content
                                                                            .ReadAsAsync<FiscalRecordCertificate>();
                byte[] toBytes = Encoding.UTF8.GetBytes(fiscalRecordCertificate.XmlFile);
                return File(toBytes, "application/xml", "XMLFile.xml");
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(FiscalRecordsController), resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords",
                                         nameof(FiscalRecordsController),
                                         resp.RequestMessage.ToString());
                return null;
            }
        }

        /// <summary>
        /// Gets the fiscal record view model list.
        /// </summary>
        /// <param name="fiscalRecordList">The fiscal record list.</param>
        /// <param name="account">The user account.</param>
        /// <returns></returns>
        private List<FiscalRecordViewModel> GetFiscalRecordViewModelList(List<FiscalRecord> fiscalRecordList,
                                                                         Account account)
        {
            List<FiscalRecordViewModel> fiscalRecordViewModelList = new List<FiscalRecordViewModel>();
            List<FiscalRecord> tmpFiscalRecordList = fiscalRecordList.OrderByDescending(m => m.ExpeditionDateTime)
                                                                     .ToList();
            foreach (FiscalRecord fiscalRecord in tmpFiscalRecordList)
            {
                FiscalRecordViewModel fiscalRecordViewModel = fiscalRecord.ToViewModel(account);
                fiscalRecordViewModelList.Add(fiscalRecordViewModel);
            }
            return fiscalRecordViewModelList;
        }
        public async Task<List<FiscalRecordCatalog>> GetStates()
        {
            IEnumerable<FiscalRecordCatalog> IEtypeStatesCatalog = await
                       GetListOfClassOfTypeT<FiscalRecordCatalog>(Account, "api/catalogs?name=States", null);
            List<FiscalRecordCatalog> StatesCatalogs = null;

            StatesCatalogs = IEtypeStatesCatalog == null ? null : IEtypeStatesCatalog.ToList<FiscalRecordCatalog>();

            return StatesCatalogs;
        }
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