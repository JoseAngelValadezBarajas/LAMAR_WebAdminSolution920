// --------------------------------------------------------------------
// <copyright file="ReceiversController.cs" company="Ellucian">
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
    /// ReceiversController Class Controller
    /// </summary>
    [LoggingActionFilterAttribute]
    public class ReceiversController : ApiController
    {
        private readonly IReceiverServices _receiverServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FiscalRecordsServices.Controllers.FiscalRecordsController" /> class.
        /// </summary>
        /// <inheritdoc />
        public ReceiversController()
        {
            _receiverServices = new ReceiverServices();
        }

        /// <summary>
        /// Deletes the tax payer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [Route("api/Receivers/DeleteTaxpayer/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteTaxpayer(int id)
        {
            bool result = _receiverServices.DeleteTaxpayer(id);
            return Ok(result);
        }

        /// <summary>
        /// Gets the tax payer.
        /// </summary>
        /// <param name="receiver">The receiver.</param>
        /// <returns></returns>
        [Authorize]
        [Route("api/Receivers/GetTaxPayer")]
        [HttpPost]
        public IHttpActionResult GetTaxPayer([FromBody] Receiver receiver)
        {
            List<Receiver> lstReceiverModel = _receiverServices.GetTaxPayers(receiver.TaxPayerId);
            return Ok(lstReceiverModel);
        }

        /// <summary>
        /// Gets the tax payerby identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="foreignId">The foreign identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetTaxPayerbyId(int id, int? foreignId)
        {
            Receiver receiver = _receiverServices.GetTaxPayerbyId(id, foreignId);
            return Ok(receiver);
        }

        // GET: Receivers
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetTaxPayers(string keyword = null)
        {
            List<Receiver> lstReceiverModel = _receiverServices.GetTaxPayers(string.Empty, keyword);
            return Ok(lstReceiverModel);
        }

        /// <summary>
        /// Saves the tax payer.
        /// </summary>
        /// <param name="Receiver">The receiver model.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IHttpActionResult SaveTaxPayer([FromBody] Receiver Receiver)
        {
            ClaimsPrincipal claim = User as ClaimsPrincipal;
            string userName = claim.FindFirst("userName").Value;
            int taxpayerId = _receiverServices.SaveTaxPayers(Receiver, userName);
            return Ok(taxpayerId);
        }

        /// <summary>
        /// Updates the tax payer.
        /// </summary>
        /// <param name="Receiver">The receiver.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("api/Receivers/UpdateTaxPayer")]
        public IHttpActionResult UpdateTaxPayer([FromBody] Receiver Receiver)
        {
            ClaimsPrincipal claim = User as ClaimsPrincipal;
            string userName = claim.FindFirst("userName").Value;
            int taxpayerId = _receiverServices.UpdateTaxPayer(Receiver, userName);
            return Ok(taxpayerId);
        }
    }
}