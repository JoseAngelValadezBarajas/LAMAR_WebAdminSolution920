// --------------------------------------------------------------------
// <copyright file="AccountsEdController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business.ElectronicDegree;
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.Entities;
using System;
using System.Net;
using System.Security.Claims;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers
{
    /// <summary>
    /// Accounts Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [LoggingActionFilterAttribute]
    public class AccountsEDController : ApiController
    {
        private readonly IElectronicDegreeTransactionServices _electronicDegreeTransactionServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountsEDController"/> class.
        /// </summary>
        public AccountsEDController() => _electronicDegreeTransactionServices = new ElectronicDegreeTransactionServices();

        /// <summary>
        /// Deletes the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            ClaimsPrincipal claims = User as ClaimsPrincipal;
            if (User.Identity.IsAuthenticated && id == Guid.Parse(claims.FindFirst("guid").Value))
            {
                _electronicDegreeTransactionServices.DeleteElectronicDegreeTransaction(id);
                return StatusCode(HttpStatusCode.NoContent);
            }

            return Unauthorized();
        }

        /// <summary>
        /// Gets the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            Account account = _electronicDegreeTransactionServices.GetElectronicDegreeTransactionByGuid(id);
            if (account == null)
                return NotFound();
            return Ok(account);
        }

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        [Route("api/AccountsED/GetPermissions/{userName}")]
        [HttpGet]
        public IHttpActionResult GetPermissions(string userName)
        {
            Account account = _electronicDegreeTransactionServices.GetElectronicDegreePermissions(userName);
            if (account == null)
                return NotFound();
            return Ok(account);
        }
    }
}