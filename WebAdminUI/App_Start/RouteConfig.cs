// --------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Web.Mvc;
using System.Web.Routing;

namespace WebAdminUI
{
    /// <summary>
    /// RouteConfig Class
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
               name: "Create",
               url: "create",
               defaults: new { controller = "FiscalRecords", action = "Create" }
            );

            routes.MapRoute(
              name: "CreateGlobal",
              url: "createglobal",
              defaults: new { controller = "FiscalRecords", action = "CreateGlobal" }
            );

            routes.MapRoute(
                name: "FiscalRecordsMenu",
                url: "FiscalRecords/Menu",
                defaults: new { controller = "FiscalRecords", action = "Menu" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "FiscalRecords", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "About",
                url: "General/Shared/About",
                defaults: new { controller = "~/General/Shared", action = "About" }
            );

            routes.MapRoute(
              name: "CancelGlobal",
              url: "cancelglobal",
              defaults: new { controller = "FiscalRecords", action = "CancelGlobal" }
            );
        }
    }
}