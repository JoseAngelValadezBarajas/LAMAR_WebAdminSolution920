// --------------------------------------------------------------------
// <copyright file="SettingsController.cs" company="Ellucian">
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
    /// <inheritdoc />
    /// <summary>
    /// Settings Controller Class
    /// </summary>
    /// <seealso cref="T:System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class SettingsController : ApiController
    {
        /// <summary>
        /// Gets the specified settings model.
        /// </summary>
        /// <param name="Settings">The settings model.</param>
        /// <returns></returns>
        [Authorize]
        public IHttpActionResult Get([FromUri] Settings Settings)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();
            if (string.IsNullOrEmpty(Settings.Area) || string.IsNullOrEmpty(Settings.Section) || string.IsNullOrEmpty(Settings.Label))
                return BadRequest();
            ISettingsService settingsService = new SettingsService();
            string setting = settingsService.GetSetting(Settings);
            if (string.IsNullOrEmpty(setting))
                return NotFound();
            return Ok(setting);
        }
    }
}