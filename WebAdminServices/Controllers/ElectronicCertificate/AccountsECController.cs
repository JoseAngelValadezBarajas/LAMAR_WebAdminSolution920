// --------------------------------------------------------------------
// <copyright file="AccountsECController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business.ElectronicCertificate;
using PowerCampus.BusinessInterfaces.ElectronicCertificate;
using PowerCampus.Entities;
using System;
using System.Net;
using System.Security.Claims;
using System.Web.Http;

namespace WebAdminServices.Controllers.ElectronicCertificate
{
    public class AccountsECController : ApiController
    {
        private readonly IECTransactionServices _ecTransactionServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountsECController"/> class.
        /// </summary>
        public AccountsECController() => _ecTransactionServices = new ECTransactionServices();

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [Route("api/EC/AccountsEC/Delete/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            ClaimsPrincipal claims = User as ClaimsPrincipal;
            if (User.Identity.IsAuthenticated && id == Guid.Parse(claims.FindFirst("guid").Value))
            {
                _ecTransactionServices.DeleteTransaction(id);
                return StatusCode(HttpStatusCode.NoContent);
            }

            return Unauthorized();
        }

        /// <summary>
        /// Gets the account by unique identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("api/EC/AccountsEC/GetAccountByGuid/{id}")]
        [HttpGet]
        public IHttpActionResult GetAccountByGuid(Guid id)
        {
            Account account = _ecTransactionServices.GetTransactionByGuid(id);
            if (account == null)
                return NotFound();
            return Ok(account);
        }
    }
}