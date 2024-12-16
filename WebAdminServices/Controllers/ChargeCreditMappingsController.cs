// --------------------------------------------------------------------
// <copyright file="ChargeCreditsController.cs" company="Ellucian">
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
    /// ChargeCreditController
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class ChargeCreditMappingsController : ApiController
    {
        private readonly IChargeCreditMappingServices _chargeCreditMappingServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChargeCreditMappingsController"/> class.
        /// </summary>
        public ChargeCreditMappingsController()
        {
            _chargeCreditMappingServices = new ChargeCreditMappingServices();
        }

        /// <summary>
        /// Deletes the charge credit mapping.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        public IHttpActionResult DeleteChargeCreditMapping(int id)
        {
            int chargeProductServiceMappingId = _chargeCreditMappingServices.DeleteChargeCreditMapping(id);
            return Ok(chargeProductServiceMappingId);
        }

        /// <summary>
        /// Gets the mapping product service.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetChargeCreditMapping()
        {
            List<ChargeCreditMapping> lstChargeCreditMapping = _chargeCreditMappingServices.GetChargeCreditMapping();
            return Ok(lstChargeCreditMapping);
        }

        /// <summary>
        /// Gets the mapping product tax service.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/ChargeCreditMapping/GetChargeCreditWithTaxes/{chargeCreditCode}")]
        public IHttpActionResult GetChargeCreditWithTaxes(string chargeCreditCode)
        {
            List<ChargeCreditWithTaxes> lstChargeCreditTaxes = _chargeCreditMappingServices.GetChargeCreditsWithTaxes(chargeCreditCode);
            return Ok(lstChargeCreditTaxes);
        }

        /// <summary>
        /// Gets the special iva tax.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/ChargeCreditMapping/GetSpecialIvaTax")]
        public IHttpActionResult GetSpecialIvaTax()
        {
            List<ChargeCreditTaxes> lstChargeCreditMapping = _chargeCreditMappingServices.GetSpecialIvaTax();
            return Ok(lstChargeCreditMapping);
        }

        /// <summary>
        /// Saves the charge credit mapping.
        /// </summary>
        /// <param name="chargeCreditMappings">The charge credit mappings.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IHttpActionResult SaveChargeCreditMappings([FromBody] List<ChargeCreditMapping> chargeCreditMappings)
        {
            ClaimsPrincipal claim = User as ClaimsPrincipal;
            string userName = claim.FindFirst("userName").Value;
            bool result = _chargeCreditMappingServices.SaveChargeCreditMappings(chargeCreditMappings, userName);
            return Ok(result);
        }
    }
}