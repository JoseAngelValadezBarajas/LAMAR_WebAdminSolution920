// --------------------------------------------------------------------
// <copyright file="MenuController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicDegree.Controllers
{
    /// <summary>
    /// Handles Electronic Degree menu and options
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
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

                Account = await PowerCampusAccess.IsAuthorizedElectronicDegree(guid);
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

        /// <summary>
        /// Menus this instance.
        /// </summary>
        /// <returns></returns>
        [PermissionRequired(PermissionName = PermissionsConstants.ElectronicDegree)]
        public ActionResult Menu()
        {
            ViewBag.Account = Account;
            return View("Index");
        }
    }
}