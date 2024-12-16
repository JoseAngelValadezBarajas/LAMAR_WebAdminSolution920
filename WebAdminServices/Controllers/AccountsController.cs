// --------------------------------------------------------------------
// <copyright file="AccountsController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business;
using PowerCampus.BusinessInterfaces;
using PowerCampus.Entities;
using System;
using System.Net;
using System.Security.Claims;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Accounts Controller
    /// </summary>
    /// <seealso cref="T:System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class AccountsController : ApiController
    {
        private readonly IFiscalRecordTransactionServices _fiscalRecordTransactionServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FiscalRecordsServices.Controllers.FiscalRecordsController" /> class.
        /// </summary>
        /// <inheritdoc />
        public AccountsController() => _fiscalRecordTransactionServices = new FiscalRecordTransactionServices();

        /// <summary>
        /// Deletes the specified unique identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            ClaimsPrincipal claims = User as ClaimsPrincipal;
            if (User.Identity.IsAuthenticated && id == Guid.Parse(claims.FindFirst("guid").Value))
            {
                int rowsAffected = _fiscalRecordTransactionServices.DeleteFiscalRecordTransaction(id);
                if (rowsAffected == 1)
                    return StatusCode(HttpStatusCode.NoContent);
                return StatusCode(HttpStatusCode.NotFound);
            }

            return Unauthorized();
        }

        /// <summary>
        /// Gets the specified unique identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            Account account = _fiscalRecordTransactionServices.GetFiscalRecordTransactionByGuid(id);
            if (account == null)
                return NotFound();
            return Ok(account);
        }

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        [Route("api/Accounts/GetPermissions/{userName}")]
        [HttpGet]
        public IHttpActionResult GetPermissions(string userName)
        {
            Account account = _fiscalRecordTransactionServices.GetFiscalRecordPermissions(userName);
            if (account == null)
                return NotFound();
            return Ok(account);
        }
    }
}