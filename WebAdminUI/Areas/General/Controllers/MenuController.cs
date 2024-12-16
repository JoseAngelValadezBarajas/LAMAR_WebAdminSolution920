// --------------------------------------------------------------------
// <copyright file="MenuController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System.Web.Mvc;

namespace WebAdminUI.Areas.General.Controllers
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="WebAdminUI.Controllers.BaseController" />
    public class MenuController : WebAdminUI.Controllers.BaseController
    {
        /// <summary>
        /// Academics the records.
        /// </summary>
        /// <returns></returns>
        public ActionResult AcademicRecords()
        {
            Account = ViewBag.Account as Account;

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
        /// Cashes the receipts.
        /// </summary>
        /// <returns></returns>
        public ActionResult CashReceipts() => View();

        /// <summary>
        /// Mains this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Main() => View();
    }
}