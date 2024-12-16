// --------------------------------------------------------------------
// <copyright file="GeneralAreaRegistration.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Web.Mvc;

namespace WebAdminUI.Areas.General
{
    /// <summary>
    /// GeneralAreaRegistration
    /// </summary>
    public class GeneralAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// AreaName
        /// </summary>
        public override string AreaName => "General";

        /// <summary>
        /// RegisterArea
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MenuMain",
                "General/Menu/Main",
                new { controller = "Menu", action = "Main" }
            );

            context.MapRoute(
               "MenuAcademicRecords",
               "General/Menu/AcademicRecords",
               new { controller = "Menu", action = "AcademicRecords" }
            );

            context.MapRoute(
              "MenuCashReceipts",
              "General/Menu/CashReceipts",
              new { controller = "Menu", action = "CashReceipts" }
            );

            context.MapRoute(
              "ErrorExpired",
              "Error/Expired",
              new { controller = "Error", action = "Expired" }
            );

            context.MapRoute(
              "ErrorUnauthorized",
              "Error/Unauthorized",
              new { controller = "Error", action = "Unauthorized" }
            );

            context.MapRoute(
              "ErrorException",
              "Error/Exception",
              new { controller = "Error", action = "Exception" }
            );

            context.MapRoute(
                "General_default",
                "General/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}