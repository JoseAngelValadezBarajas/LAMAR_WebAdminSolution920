// --------------------------------------------------------------------
// <copyright file="CreditNotesController.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using PowerCampus.Entities.Enum;
using Resources;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Filter;
using WebAdminUI.FiscalRecordsMappers;
using WebAdminUI.Helpers;
using WebAdminUI.Models.FiscalRecords;
using WebAdminUI.Views.FiscalRecords.App_LocalResources;

namespace WebAdminUI.Controllers
{
    /// <summary>
    /// CreditNoteController class
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
    public class CreditNotesController : BaseController
    {
        /// <summary>
        /// Calculates the tax totals by charge.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CalculateTaxTotalsByCharge(string model)
        {
            FiscalRecordDetail fiscalRecordModel = JsonConvert.DeserializeObject<FiscalRecordDetail>(model);

            if (Account.status == enumAccess.Authorized)
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                string json = JsonConvert.SerializeObject(fiscalRecordModel);
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/CreditNotes/CalculateTaxTotalsByCharge", httpContent);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    FiscalRecord fiscalRecordDetail = await resp.Content.ReadAsAsync<FiscalRecord>();
                    var chargeDetail = new
                    {
                        subTotal = fiscalRecordDetail.SubTotal.ToString(Account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        totalTaxes = fiscalRecordDetail.TotalTransferTaxes.ToString(Account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        total = fiscalRecordDetail.Total.ToString(Account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture)
                    };
                    return Json(chargeDetail);
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "CreditNotesController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("FiscalRecords", "CreditNotesController", resp.ReasonPhrase);
                    return Json(new { id = 0, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Calculatetotalses the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Calculatetotals(string model)
        {
            List<FiscalRecordDetail> fiscalRecordModel = JsonConvert.DeserializeObject<List<FiscalRecordDetail>>(model);

            if (Account.status == enumAccess.Authorized)
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                string json = JsonConvert.SerializeObject(fiscalRecordModel);
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/CreditNotes/CalculateTotals", httpContent);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    FiscalRecord fiscalRecord = await resp.Content.ReadAsAsync<FiscalRecord>();
                    var chargeDetail = new
                    {
                        subTotal = fiscalRecord.SubTotal.ToString(Account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        totalTaxes = fiscalRecord.TotalTransferTaxes.ToString(Account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        total = fiscalRecord.Total.ToString(Account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture)
                    };
                    return Json(chargeDetail);
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "CreditNotesController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("FiscalRecords", "CreditNotesController", resp.ReasonPhrase);
                    return Json(new { id = 0, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Creates the specified is substitution.
        /// </summary>
        /// <param name="isSubstitution">The is substitution.</param>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
        [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
        public async Task<ActionResult> Create(bool? isSubstitution)
        {
            FiscalRecordViewModel fiscalRecordViewModel;
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp;

            if (isSubstitution == null)
                resp = await client.GetAsync(client.BaseAddress + "/api/creditnotes/Get/?Id=" + Account.InvoiceHeaderId + "&peopleOrgcodeId=" + Account.PeopleOrgCodeId);
            else
                resp = await client.GetAsync($"{client.BaseAddress}/api/CreditNotes/ForSubstitution/{Account.InvoiceHeaderId}/{Account.PeopleOrgCodeId}");

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                FiscalRecord fiscalRecordModel = await resp.Content.ReadAsAsync<FiscalRecord>();
                fiscalRecordViewModel = fiscalRecordModel.ToViewModelCreditNote(Account);
                Account.PeopleOrgCodeId = fiscalRecordModel.PeopleOrgCodeId;
                fiscalRecordViewModel.peopleOrgName = Account.PeopleOrgName;
                fiscalRecordViewModel.PeopleOrgCodeIdFormat = Account.PeopleOrgCodeId;
                fiscalRecordViewModel.PeopleOrgCodeId = PowerCampusSystemFormat.FormatPeopleCodeId(Account.PeopleOrgCodeId.Substring(1));
                if (isSubstitution != null)
                    fiscalRecordViewModel.CancelReasonName = CancelReasonName.ErrorRelacion;

                return View("CreditNotes", fiscalRecordViewModel);
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(CreditNotesController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorExpired");
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", nameof(CreditNotesController), resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Creates the credit note.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateCreditNote(string model)
        {
            CreateFiscalRecord fiscalRecordModel = JsonConvert.DeserializeObject<CreateFiscalRecord>(model);

            if (Account.status == enumAccess.Authorized)
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                string json = JsonConvert.SerializeObject(fiscalRecordModel);

                foreach (FiscalRecordDetail item in fiscalRecordModel.Detail)
                {
                    if (item.Description is null)
                        item.Description = string.Empty;

                    if (item.UnitDescription is null)
                        item.UnitDescription = string.Empty;

                    if (string.IsNullOrEmpty(item.Description))
                        return Json(new { id = -1, message = CreateResource.ChargeCreditDescRequired }, JsonRequestBehavior.AllowGet);
                    if (item.Description.Contains("|") ||
                        item.UnitDescription.Contains("|"))
                        return Json(new { id = -1, message = CreateResource.InvalidCharacterMessage }, JsonRequestBehavior.AllowGet);
                }

                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/CreditNote/CreateCreditNote", httpContent);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    int invoiceHeaderId = await resp.Content.ReadAsAsync<int>();
                    if (invoiceHeaderId == 0)
                        return Json(new { id = invoiceHeaderId, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);

                    return Json(new { id = invoiceHeaderId, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "CreditNotesController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("FiscalRecords", "CreditNotesController", resp.ReasonPhrase);
                    return Json(new { id = 0, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Gets the credit code.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetCreditCode()
        {
            FiscalRecordViewModel fiscalRecordViewModel;

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/catalogs?name=Credit");
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                FiscalRecord fiscalRecordModel = await resp.Content.ReadAsAsync<FiscalRecord>();
                fiscalRecordViewModel = fiscalRecordModel.ToViewModelCreditNote(Account);
                return Json(fiscalRecordViewModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "CreditNotesController", resp.ReasonPhrase);
                return RedirectToRoute("ErrorExpired");
            }
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="peopleOrgId">The people org identifier.</param>
        /// <param name="peopleOrgName">Name of the people org.</param>
        /// <returns></returns>
        public EmptyResult GetId(int id, string peopleOrgId, string peopleOrgName)
        {
            if (Account == null)
                return null;

            Account.InvoiceHeaderId = id;
            Account.PeopleOrgCodeId = peopleOrgId;
            Account.PeopleOrgName = peopleOrgName;

            return null;
        }
    }
}