// --------------------------------------------------------------------
// <copyright file="ChargeCreditsController.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Business;
using PowerCampus.Entities;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers
{
    /// <summary>
    /// ChargeCreditsController
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class ChargeCreditsController : ApiController
    {
        private readonly ChargeCreditsServices chargeCredits;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChargeCreditsController"/> class.
        /// </summary>
        public ChargeCreditsController() => chargeCredits = new ChargeCreditsServices();

        /// <summary>
        /// Charges the credits.
        /// </summary>
        /// <param name="chargeCreditNumber">The charge credit number.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/chargecredits/Get")]
        public IHttpActionResult ChargeCredits(int chargeCreditNumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                ChargeCredit chargeCreditResult = chargeCredits.ChargeCredits(chargeCreditNumber);
                return Ok(chargeCreditResult);
            }
            return Unauthorized();
        }
    }
}