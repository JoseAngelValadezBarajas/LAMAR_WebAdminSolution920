// --------------------------------------------------------------------
// <copyright file="CashReceiptsController.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Business;
using PowerCampus.BusinessInterfaces;
using PowerCampus.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers
{
    /// <summary>
    /// CashReceiptsController Class
    /// </summary>
    [LoggingActionFilterAttribute]
    public class CashReceiptsController : ApiController
    {
        private readonly ICashReceiptServices _cashReceiptServices;
        private readonly IReceiverServices _receiverServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="CashReceiptsController" /> class.
        /// </summary>
        public CashReceiptsController()
        {
            _cashReceiptServices = new CashReceiptServices();
            _receiverServices = new ReceiverServices();
        }

        /// <summary>
        /// Get all the details of a Cash Receipt
        /// </summary>
        /// <param name="id">Receipt number</param>
        /// <returns>The view of Application</returns>
        [Route("api/CashReceipts/Application/{id}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult Application(int id)
        {
            ChargeCreditApplication cashReceiptDetail = _cashReceiptServices.GetCashReceiptApplication(id);
            if (cashReceiptDetail?.ReceiptNumber > 0)
                return Ok(cashReceiptDetail);
            return NotFound();
        }

        /// <summary>
        /// Get the cancelled global payment type.
        /// </summary>
        /// <param name="invoiceHeaderId">The cancelled global invoice header identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/CashReceipts/CancelledGlobalPaymentType/{invoiceHeaderId}")]
        public IHttpActionResult CancelledGlobalPaymentType(int invoiceHeaderId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<FiscalRecordCatalog> lstPaymentType = _cashReceiptServices.GetCancelledGlobalPaymentType(invoiceHeaderId);
                if (lstPaymentType != null)
                    return Ok(lstPaymentType);
                return NotFound();
            }
            else
                return null;
        }

        /// <summary>
        /// Deletes the receipt payment mapping.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [Route("api/CashReceipts/receiptpaymentmappings")]
        [HttpDelete]
        public IHttpActionResult DeleteReceiptPaymentMapping(int Id)
        {
            int result = _cashReceiptServices.DeleteReceiptPaymentMapping(Id);
            return Ok(result);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                CashReceipt cashReceipt = _cashReceiptServices.GetCashReceipt(id);
                if (cashReceipt?.receiptNumber > 0)
                    return Ok(cashReceipt);
                return NotFound();
            }
            else
                return Unauthorized();
        }

        /// <summary>
        /// Gets the mapping product service.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("api/CashReceipts/GetChargeCreditMapping")]
        [HttpGet]
        public IHttpActionResult GetChargeCreditMapping()
        {
            List<ReceiptPaymentMethodMapping> lstChargeCreditMapping = _cashReceiptServices.GetChargeCreditMapping();
            return Ok(lstChargeCreditMapping);
        }

        /// <summary>
        /// Gets the global cash receipts.
        /// </summary>
        /// <param name="globalInvoiceFiltersForCreation">The global invoice filters for creation.</param>
        /// <returns></returns>
        [Route("api/CashReceipts/GetGlobalCashReceipts")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult GetGlobalCashReceipts(GlobalInvoiceFiltersForCreation globalInvoiceFiltersForCreation)
        {
            if (User.Identity.IsAuthenticated)
            {
                CashReceipt globalFiscalRecord = _cashReceiptServices.GetGlobalCashReceipts(globalInvoiceFiltersForCreation);
                if (globalFiscalRecord != null)
                    return Ok(globalFiscalRecord);
                return NotFound();
            }
            else
                return null;
        }

        /// <summary>
        /// Gets the global fiscal record.
        /// </summary>
        /// <returns></returns>
        [Route("api/CashReceipts/GetGlobalFiscalRecord")]
        [Authorize]
        [HttpGet]
        public CashReceipt GetGlobalFiscalRecord()
        {
            if (User.Identity.IsAuthenticated)
            {
                CashReceipt globalFiscalRecord = _cashReceiptServices.GetGlobalFiscalRecord();

                return globalFiscalRecord;
            }
            else
                return null;
        }

        /// <summary>
        /// Gets the global cash receipts of the previously canceled global invoice with reason 04.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier of the canceled invoice with reason 04.</param>
        /// <returns></returns>
        [Route("api/CashReceipts/GetGlobalInvoiceDetail/{invoiceHeaderId}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetGlobalInvoiceDetail(int invoiceHeaderId)
        {
            if (User.Identity.IsAuthenticated)
            {
                CashReceipt globalFiscalRecord = _cashReceiptServices.GetGlobalInvoiceDetail(invoiceHeaderId);
                if (globalFiscalRecord != null)
                    return Ok(globalFiscalRecord);
                return NotFound();
            }
            else
                return null;
        }

        /// <summary>
        /// Gets the type of the global payment.
        /// </summary>
        /// <param name="globalInvoiceFiltersForCreation">The global invoice filters for creation.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IHttpActionResult GetGlobalPaymentType(GlobalInvoiceFiltersForCreation globalInvoiceFiltersForCreation)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<FiscalRecordCatalog> lstPaymentType = _cashReceiptServices.GetGlobalPaymentType(globalInvoiceFiltersForCreation);
                if (lstPaymentType != null)
                    return Ok(lstPaymentType);
                return NotFound();
            }
            else
                return null;
        }

        /// <summary>
        /// Gets the receipt details.
        /// </summary>
        /// <param name="receiptNumber">The receipt number.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/CashReceipts/GetReceiptDetails/{receiptNumber}")]
        public IHttpActionResult GetReceiptDetails(int receiptNumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<ChargeCredit> chargeCredits = _cashReceiptServices.GetChargeCreditsByReceipt(receiptNumber);
                if (chargeCredits != null)
                    return Ok(chargeCredits);
                return NotFound();
            }
            else
                return null;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <param name="receiptPaymentMethodMapping"></param>
        /// <returns></returns>
        [Authorize]
        [Route("api/CashReceipts/receiptpaymentmappings")]
        [HttpPost]
        public IHttpActionResult SaveReceiptPaymentMapping([FromBody] ReceiptPaymentMethodMapping receiptPaymentMethodMapping)
        {
            ClaimsPrincipal claim = User as ClaimsPrincipal;
            string userName = claim.FindFirst("userName").Value;
            int result = _cashReceiptServices.SaveReceiptPaymentMapping(receiptPaymentMethodMapping, userName);
            return Ok(result);
        }
    }
}