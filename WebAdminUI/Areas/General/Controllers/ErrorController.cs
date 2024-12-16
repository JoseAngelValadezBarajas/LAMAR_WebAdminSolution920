// --------------------------------------------------------------------
// <copyright file="ErrorController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Web.Mvc;

namespace WebAdminUI.Areas.General.Controllers
{
    /// <summary>
    /// Handles all errors for WebAdministrator
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller"/>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Exceptions this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Exception() => View();

        /// <summary>
        /// Expireds this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Expired()
        {
            Session.Remove("Account");
            Session.Abandon();
            return View();
        }

        /// <summary>
        /// Unauthorizeds this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Unauthorized() => View();
    }
}