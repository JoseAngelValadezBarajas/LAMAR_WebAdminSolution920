// --------------------------------------------------------------------
// <copyright file="MenuController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicCertificate.Controllers
{
    /// <summary>
    /// MenuController
    /// </summary>
    /// <seealso cref="WebAdminUI.Controllers.BaseController" />
    public class MenuController : WebAdminUI.Controllers.BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(string guid)
        {
            Account = ViewBag.Account as Account;

            if (Account == null)
            {
                if (string.IsNullOrEmpty(guid))
                    return RedirectToRoute("ErrorUnauthorized");

                Account = await PowerCampusAccess.IsAuthorizedElectronicCertificate(guid);
            }

            if (Account == null || Account.status == enumAccess.Unauthorized)
                return RedirectToRoute("ErrorUnauthorized");

            if (Account.status == enumAccess.Authorized)
            {
                ViewBag.Account = Account;
                return View();
            }

            return RedirectToRoute("ErrorUnauthorized");
        }
    }
}