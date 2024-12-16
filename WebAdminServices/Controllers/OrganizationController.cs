// --------------------------------------------------------------------
// <copyright file="OrganizationController.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Business;
using PowerCampus.Entities;
using System.Security.Claims;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers
{
    /// <summary>
    /// OrganizationController API
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class OrganizationController : ApiController
    {
        private readonly OrganizationServices _organizationServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationController"/> class.
        /// </summary>
        public OrganizationController()
        {
            _organizationServices = new OrganizationServices();
        }

        /// <summary>
        /// Chargeses the specified code identifier.
        /// </summary>
        /// <param name="CodeId">The code identifier.</param>
        /// <param name="YearTermSession">The yts.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/organization/Charges/list")]
        public IHttpActionResult Charges(PeopleOrgCharges peopleCharge)
        {
            if (User.Identity.IsAuthenticated)
            {
                Organization organizationResult = _organizationServices.Charges(peopleCharge.CodeId, peopleCharge.YTS);
                return Ok(organizationResult);
            }
            else
                return Unauthorized();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            if (User.Identity.IsAuthenticated)
                return Ok(_organizationServices.GetTaxpayerInfo(id));
            return Unauthorized();
        }

        /// <summary>
        /// Saves the taxpayer identifier.
        /// </summary>
        /// <param name="organization">The organization.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/organization/Save/Taxpayerid")]
        public IHttpActionResult SaveTaxpayerId(Organization organization)
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal claim = User as ClaimsPrincipal;
                string userName = claim.FindFirst("userName").Value;
                organization.FiscalRecordsDefault.InvoicePreferredTaxpayerId = _organizationServices.SaveTaxpayerId(organization, userName);
                return Ok(organization.FiscalRecordsDefault.InvoicePreferredTaxpayerId);
            }
            else
                return Unauthorized();
        }

        /// <summary>
        /// Years the term session.
        /// </summary>
        /// <param name="OrganizationId">The organization identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/organization/yts")]
        public IHttpActionResult YearTermSession(string OrganizationId)
        {
            if (User.Identity.IsAuthenticated)
            {
                Organization organizationResult = _organizationServices.YearTermSession(OrganizationId);
                return Ok(organizationResult);
            }
            else
                return Unauthorized();
        }
    }
}