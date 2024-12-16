// --------------------------------------------------------------------
// <copyright file="SettingsController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using PowerCampus.Entities;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;

namespace WebAdminUI.Controllers
{
    /// <summary>
    /// SettingsController
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordSetup)]
    public class SettingsController : BaseController
    {
        /// <summary>
        /// Values the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Value(Settings settings)
        {
            string settingValue = string.Empty;
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(
                client.BaseAddress + "/api/settings?area=" + settings.Area + "&section=" + settings.Section + "&label=" + settings.Label);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                settingValue = await resp.Content.ReadAsAsync<string>();
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - SettingsController", resp.ReasonPhrase);
            }
            return Json(settingValue, JsonRequestBehavior.AllowGet);
        }
    }
}