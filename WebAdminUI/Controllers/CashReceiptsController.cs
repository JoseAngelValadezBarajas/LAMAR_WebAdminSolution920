// --------------------------------------------------------------------
// <copyright file="CashReceiptsController.cs" company="Ellucian">
//     Copyright 2018 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using PowerCampus.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.CashReceiptsMappers;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;
using WebAdminUI.Models.CashReceipts;
using WebAdminUI.Models.FiscalRecords;

namespace WebAdminUI.Controllers
{
    /// <summary>
    /// Controller for all the Cash Receipt actions
    /// Inherit from BaseController <seealso cref="BaseController"/>
    /// </summary>
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
    public class CashReceiptsController : BaseController
    {
        // GET: Details
        /// <summary>
        /// Get all the details of a Cash Receipt
        /// </summary>
        /// <param name="charges"></param>
        /// <returns></returns>
        [HttpGet]
        [ChildActionOnly]
        public ActionResult _ChargesList(IEnumerable<ChargeCreditApplicationDetailsViewModel> charges) => PartialView(charges);

        // GET: Details
        /// <summary>
        /// Get all the details of a Cash Receipt
        /// </summary>
        /// <returns>The view of Application</returns>
        [HttpGet]
        public async Task<ActionResult> Application()
        {
            try
            {
                if (string.IsNullOrEmpty(Account.ReceiptNumber))
                    return RedirectToAction("Unauthorized", "Home");
                var client = PowerCampusHttpClient.GetClient(Account);
                var resp = await client.GetAsync($"{client.BaseAddress}/api/CashReceipts/Application/{Account.ReceiptNumber}");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    var modelResult = await resp.Content.ReadAsAsync<ChargeCreditApplication>();
                    var model = modelResult.ToViewModel();
                    return View(model);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("FiscalRecords", "CashReceiptsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorExpired");
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "CashReceiptsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CashReceiptsController", ex.Message);
                throw;
            }
        }

        // GET: CashReceipts
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Gets the cash receipt details.
        /// </summary>
        /// <param name="receiptNumber">The receipt number.</param>
        /// <returns></returns>
        [HttpGet]
        [PermissionRequired(PermissionName = PermissionsConstants.GlobalFiscalRecord)]
        public async Task<ActionResult> ReceiptDetails(int receiptNumber)
        {
            List<ChargeCredit> chargeCredits = new List<ChargeCredit>();

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}/api/CashReceipts/GetReceiptDetails/{receiptNumber}");
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                chargeCredits = await resp.Content.ReadAsAsync<List<ChargeCredit>>();
            }

            return Json(chargeCredits.ToViewModel(Account), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Receipts the number.
        /// </summary>
        /// <param name="receiptNumber">The receipt number.</param>
        /// <returns></returns>
        public EmptyResult ReceiptNumber(string receiptNumber)
        {
            Account.ReceiptNumber = receiptNumber;
            return null;
        }
    }
}