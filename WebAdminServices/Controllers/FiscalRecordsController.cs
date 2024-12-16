// --------------------------------------------------------------------
// <copyright file="FiscalRecordsController.cs" company="Ellucian">
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
    /// <inheritdoc />
    /// <summary>
    /// FiscalRecords Controller
    /// </summary>
    /// <seealso cref="T:System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    [Authorize]
    public class FiscalRecordsController : ApiController
    {
        private readonly IFiscalRecordServices _fiscalRecordServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FiscalRecordsServices.Controllers.FiscalRecordsController" /> class.
        /// </summary>
        /// <inheritdoc />
        public FiscalRecordsController()
        {
            _fiscalRecordServices = new FiscalRecordServices();
        }

        /// <summary>
        /// Certificates the files.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <param name="certificateId">The certificate identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/FiscalRecords/{invoiceHeaderId}/certificatefiles/{certificateId}")]
        public IHttpActionResult CertificateFiles(int invoiceHeaderId, int certificateId)
        {
            FiscalRecordCertificate fiscalRecord = _fiscalRecordServices.GetCertificateFiles(invoiceHeaderId, certificateId);
            if (fiscalRecord != null)
                return Ok(fiscalRecord);
            return NotFound();
        }

        /// <summary>
        /// Creates the fiscal record.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <returns></returns>
        [Route("api/FiscalRecords/CreateFiscalRecord")]
        [HttpPost]
        public IHttpActionResult CreateFiscalRecord(CreateFiscalRecord Model)
        {
            int invoiceHeaderId = _fiscalRecordServices.CreateFiscalRecord(Model);
            if (invoiceHeaderId > 0)
                return Ok(invoiceHeaderId);
            return NotFound();
        }

        /// <summary>
        /// Creates the global fiscal record.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <returns></returns>
        [Route("api/FiscalRecords/CreateGlobalFiscalRecord")]
        [HttpPost]
        public IHttpActionResult CreateGlobalFiscalRecord(CreateFiscalRecord Model)
        {
            int invoiceHeaderId = Model.IsCreationForReason04
                ? _fiscalRecordServices.CreateGlobalFiscalRecordForReason04(Model)
                : _fiscalRecordServices.CreateGlobalFiscalRecord(Model);

            if (invoiceHeaderId > 0)
                return Ok(invoiceHeaderId);
            return NotFound();
        }

        /// <summary>
        /// Creates the payment receipt.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/FiscalRecords/CreatePaymentReceipt")]
        public IHttpActionResult CreatePaymentReceipt(CreatePaymentReceipt Model)
        {
            int invoiceHeaderId = _fiscalRecordServices.CreatePaymentReceipt(Model);
            if (invoiceHeaderId > 0)
                return Ok(invoiceHeaderId);
            return NotFound();
        }

        /// <summary>
        /// Deletes the fiscal records.
        /// </summary>
        /// <param name="fiscalRecordRequest">The fiscal record request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/FiscalRecords/DeleteFiscalRecords")]
        public IHttpActionResult DeleteFiscalRecords(FiscalRecordRequest fiscalRecordRequest)
        {
            ClaimsPrincipal claim = User as ClaimsPrincipal;
            string userName = claim.FindFirst("userName").Value;
            int deleteOk = _fiscalRecordServices.DeleteFiscalRecord(fiscalRecordRequest.InvoiceHeaderId, userName);
            if (deleteOk > 0)
                return Ok(deleteOk);
            return NotFound();
        }

        /// <summary>
        /// Gets the fiscal record.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(int Id)
        {
            FiscalRecord fiscalRecord = _fiscalRecordServices.GetFiscalRecord(Id);
            if (fiscalRecord != null)
                return Ok(fiscalRecord);
            return NotFound();
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/FiscalRecords/InvoiceHeader/{id}")]
        public IHttpActionResult GetById(int Id)
        {
            FiscalRecord fiscalRecord = _fiscalRecordServices.GetFiscalRecordById(Id);
            if (fiscalRecord != null)
                return Ok(fiscalRecord);
            return NotFound();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="invoiceFilters">The invoice filters.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetFilteredFiscalRecords(InvoiceFilters invoiceFilters)
        {
            List<FiscalRecord> allfiscalRecord = _fiscalRecordServices.GetAllFiscalRecords(invoiceFilters);
            return Ok(allfiscalRecord);
        }

        /// <summary>
        /// Gets for global substitution.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/FiscalRecords/GetForGlobalSubstitution/{invoiceHeaderId}")]
        public IHttpActionResult GetForGlobalSubstitution(int invoiceHeaderId)
        {
            CashReceipt cashReceipt = _fiscalRecordServices.GetForSubstitution(invoiceHeaderId, true);
            if (cashReceipt != null)
                return Ok(cashReceipt);
            return NotFound();
        }

        /// <summary>
        /// Gets for substitution.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/FiscalRecords/ForSubstitution/{invoiceHeaderId}")]
        public IHttpActionResult GetForSubstitution(int invoiceHeaderId)
        {
            CashReceipt cashReceipt = _fiscalRecordServices.GetForSubstitution(invoiceHeaderId, false);
            if (cashReceipt != null && (cashReceipt.ReceiverPaymentMethodDefault.StartsWith(Constants.PPDPaymentMethod) || cashReceipt.receiptNumber > 0))
                return Ok(cashReceipt);
            return NotFound();
        }

        /// <summary>
        /// Gets the available global invoice filters.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        [Route("api/FiscalRecords/GetGlobalInvoiceFilters")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetGlobalInvoiceFilters(string startDate, string endDate)
        {
            if (User.Identity.IsAuthenticated)
            {
                GlobalInvoiceFilters globalInvoiceFilters = _fiscalRecordServices.GetGlobalInvoiceFilters(startDate, endDate);
                return Ok(globalInvoiceFilters);
            }
            else
                return null;
        }

        /// <summary>
        /// Gets the paymen complement.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <param name="receiptNumber">The receipt number.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/FiscalRecords/PaymentComplement/{invoiceHeaderId}/{receiptNumber}")]
        public IHttpActionResult GetPaymenComplement(int invoiceHeaderId, int receiptNumber)
        {
            PaymentComplement paymentComplement = _fiscalRecordServices.GetPaymentComplement(invoiceHeaderId, receiptNumber);
            if (paymentComplement == null)
                return NotFound();
            return Ok(paymentComplement);
        }

        /// <summary>
        /// Gets the fiscal record.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/FiscalRecords/GlobalInvoiceDetails/{invoiceHeaderId}")]
        public IHttpActionResult GlobalInvoiceDetails(int invoiceHeaderId)
        {
            GlobalInvoiceCancellationDetails globalInvoiceCancellationDetails = _fiscalRecordServices.GetGlobalInvoiceCancellationDetails(invoiceHeaderId);
            if (globalInvoiceCancellationDetails != null)
                return Ok(globalInvoiceCancellationDetails);
            return NotFound();
        }

        /// <summary>
        /// Get the fiscal records associated to a fiscal record when they are related to payment receipt supplement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/FiscalRecords/PaymentReceiptsRelated/{id}")]
        [HttpGet]
        public IHttpActionResult PaymentReceiptsRelated(int id)
        {
            List<PaymentReceipt>[] result = _fiscalRecordServices.GetPaymentReceiptsRelated(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        /// <summary>
        /// Get the fiscal records associated to a fiscal record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/FiscalRecords/Related/{id}")]
        [HttpGet]
        public IHttpActionResult Related(int id)
        {
            List<FiscalRecord>[] result = _fiscalRecordServices.GetFiscalRecordsRelated(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }
    }
}