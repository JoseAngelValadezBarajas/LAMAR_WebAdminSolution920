// --------------------------------------------------------------------
// <copyright file="IssuersController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
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
    /// Issuers API Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class IssuersController : ApiController
    {
        /// <summary>
        /// Issuer Services
        /// </summary>
        private readonly IIssuerServices _IssuerServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="IssuersController"/> class.
        /// </summary>
        public IssuersController()
        {
            _IssuerServices = new IssuerServices();
        }

        /// <summary>
        /// Creates the issuer set up.
        /// </summary>
        /// <param name="issuerDefault">The issuer default.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/Issuers/CreateIssuerSetUp")]
        public IHttpActionResult CreateIssuerSetUp([FromBody] IssuerDefault issuerDefault)
        {
            ClaimsPrincipal claim = User as ClaimsPrincipal;
            string userName = claim.FindFirst("userName").Value;
            int InvoiceOrganizationId = _IssuerServices.CreateIssuerSetUp(issuerDefault, userName);
            return Ok(InvoiceOrganizationId);
        }

        /// <summary>
        /// Deletes the specified unique identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route("api/Issuers/Edit/DeleteIssuerAddressId")]
        public IHttpActionResult DeleteIssuingAddressId(int id)
        {
            int result = _IssuerServices.DeleteIssuingAddress(id);
            return Ok(result);
        }

        /// <summary>
        /// Edits the issuing address.
        /// </summary>
        /// <param name="issuer">The issuer.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/Issuers/AddressId")]
        public IHttpActionResult EditIssuingAddress([FromBody] InvoiceExpedition issuer)
        {
            ClaimsPrincipal claim = User as ClaimsPrincipal;
            string userName = claim.FindFirst("userName").Value;
            int InvoiceOrganizationId = _IssuerServices.SaveIssuingAddress(issuer, userName);
            return Ok(InvoiceOrganizationId);
        }

        /// <summary>
        /// Gets all issuers.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetAllIssuers(string id = "")
        {
            List<Issuer> lstIssuerModel = _IssuerServices.GetAllIssuers(id);
            return Ok(lstIssuerModel);
        }

        /// <summary>
        /// Gets all issuers.
        /// </summary>
        /// <param name="issuer">The issuer.</param>
        /// <returns></returns>
        [Authorize]
        [Route("api/Issuers/GetIssuer")]
        [HttpPost]
        public IHttpActionResult GetIssuer([FromBody] Issuer issuer)
        {
            List<InvoiceOrganization> lstIssuerModel = _IssuerServices.GetIssuers(issuer.IssTaxpayerId);
            return Ok(lstIssuerModel);
        }

        /// <summary>
        /// Gets all issuers.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [Route("api/IssuersList/")]
        [HttpGet]
        public IHttpActionResult GetIssuers(string id = "")
        {
            List<InvoiceOrganization> lstIssuerModel = _IssuerServices.GetIssuers(id);
            return Ok(lstIssuerModel);
        }

        /// <summary>
        /// Gets the issuer serial number.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="expId"></param>
        /// <returns></returns>
        [Authorize]
        [Route("api/Issuers/GetSerialNumber")]
        [HttpGet]
        public IHttpActionResult GetSerialNumber(int id, int? expId)
        {
            Issuer lstIssuerModel = _IssuerServices.GetIssuerSerialNumber(id, expId);
            return Ok(lstIssuerModel);
        }

        /// <summary>
        /// Gets the tax regimen.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [Route("api/Issuers/GetTaxRegimen")]
        [HttpGet]
        public IHttpActionResult GetTaxRegimen(int id)
        {
            Issuer lstIssuerModel = _IssuerServices.GetIssuer(id);
            return Ok(lstIssuerModel);
        }

        /// <summary>
        /// Gets all issuers.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [Route("api/IssuersList/{id}")]
        [HttpGet]
        public IHttpActionResult IssuersList(int id)
        {
            InvoiceOrganization invoiceOrganization = _IssuerServices.GetIssuerById(id);
            return Ok(invoiceOrganization);
        }

        /// <summary>
        /// Saves the tax payer.
        /// </summary>
        /// <param name="id">The receiver model.</param>
        /// <returns></returns>
        [Authorize]
        [Route("api/Issuers/Address/{id}")]
        [HttpGet]
        public IHttpActionResult IssuingAddress(int id)
        {
            int? invoiceExpeditionId = null;
            List<InvoiceExpedition> InvoiceExpeditions = _IssuerServices.GetIssuingAddress(id, invoiceExpeditionId);
            return Ok(InvoiceExpeditions);
        }

        /// <summary>
        /// Get The Document setups for a issuer
        /// </summary>
        /// <param name="id"> issuer invoice org id</param>
        /// <returns></returns>
        [Authorize]
        [Route("api/Issuers/Receipt/{id}")]
        [HttpGet]
        public IHttpActionResult IssuingReceipt(int id)
        {
            int? invoiceReceiptId = null;
            List<InvoiceReceipt> InvoiceReceipts = _IssuerServices.GetIssuingReceipt(id, invoiceReceiptId);
            return Ok(InvoiceReceipts);
        }

        /// <summary>
        /// Saves issuer Address.
        /// </summary>
        /// <param name="issuer">The receiver model.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/Issuers/Address")]
        public IHttpActionResult SaveIssuingAddress([FromBody] InvoiceExpedition issuer)
        {
            ClaimsPrincipal claim = User as ClaimsPrincipal;
            string userName = claim.FindFirst("userName").Value;
            int InvoiceOrganizationId = _IssuerServices.SaveIssuingAddress(issuer, userName);
            return Ok(InvoiceOrganizationId);
        }

        /// <summary>
        /// Update doucument setup
        /// </summary>
        /// <param name="issuer">The issuer.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/Issuers/Receipt")]
        public IHttpActionResult SaveIssuingReceipt([FromBody] InvoiceReceipt issuer)
        {
            ClaimsPrincipal claim = User as ClaimsPrincipal;
            string userName = claim.FindFirst("userName").Value;
            int InvoiceExpeditionId = _IssuerServices.SaveIssuingReceipt(issuer, userName);
            return Ok(InvoiceExpeditionId);
        }

        /// <summary>
        /// Saves the tax payer.
        /// </summary>
        /// <param name="issuer">The receiver model.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IHttpActionResult SaveTaxPayer([FromBody] InvoiceOrganization issuer)
        {
            ClaimsPrincipal claim = User as ClaimsPrincipal;
            string userName = claim.FindFirst("userName").Value;
            int invoiceOrganizationId = _IssuerServices.SaveTaxPayers(issuer, userName);
            return Ok(invoiceOrganizationId);
        }

        /// <summary>
        /// Selects the issuer set up.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/Issuers/SelectIssuerSetUp")]
        public IHttpActionResult SelectIssuerSetUp()
        {
            ClaimsPrincipal claim = User as ClaimsPrincipal;
            string userName = claim.FindFirst("userName").Value;
            IssuerDefault issuerDefault = _IssuerServices.SelectIssuerSetUp(userName);
            return Ok(issuerDefault);
        }
    }
}