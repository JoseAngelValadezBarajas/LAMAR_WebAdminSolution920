// --------------------------------------------------------------------
// <copyright file="TaxProfilesController.cs" company="Ellucian">
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
    /// TaxProfileRatesController
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class TaxProfilesController : ApiController
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IHttpActionResult Get()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();
            ITaxProfileServices services = new TaxProfileServices();
            List<TaxProfile> taxProfiles = services.GetTaxProfileList();
            return Ok(taxProfiles);
        }

        /// <summary>
        /// Taxes the mapping.
        /// </summary>
        /// <param name="fiscalRecordTaxMapping">The fiscal record tax mapping.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IHttpActionResult TaxMapping([FromBody] FiscalRecordTaxMapping fiscalRecordTaxMapping)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();
            ClaimsPrincipal claim = User as ClaimsPrincipal;
            string userName = claim.FindFirst("userName").Value;
            ITaxProfileServices services = new TaxProfileServices();
            int taxMapping = services.SaveTaxMapping(fiscalRecordTaxMapping, userName);
            return Ok(taxMapping);
        }

        /// <summary>
        /// Gets the validity details.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IHttpActionResult ValidityDetails(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();
            ITaxProfileServices services = new TaxProfileServices();
            List<TaxProfileDetail> validityDetails = services.GetTaxProfileDetails(id);
            return Ok(validityDetails);
        }
    }
}