// --------------------------------------------------------------------
// <copyright file="FiscalRecordRequestsController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Business;
using PowerCampus.BusinessInterfaces;
using PowerCampus.Entities;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers
{
    /// <summary>
    /// FiscalRecordRequestsController
    /// </summary>
    [LoggingActionFilterAttribute]
    public class FiscalRecordRequestsController : ApiController
    {
        private readonly IFiscalRecordRequestServices _fiscalRecordRequestServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FiscalRecordsServices.Controllers.FiscalRecordsController" /> class.
        /// </summary>
        /// <inheritdoc />
        public FiscalRecordRequestsController()
        {
            _fiscalRecordRequestServices = new FiscalRecordRequestServices();
        }

        /// <summary>
        /// Inserts the invoice request.
        /// </summary>
        /// <param name="fiscalRecordRequest">The fiscal record request.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IHttpActionResult Post([FromBody] FiscalRecordRequest fiscalRecordRequest)
        {
            int InvoiceHeaderId = _fiscalRecordRequestServices.InsertInvoiceRequest(fiscalRecordRequest);
            return Ok(InvoiceHeaderId);
        }
    }
}