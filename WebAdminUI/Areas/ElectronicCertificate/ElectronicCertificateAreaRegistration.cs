// --------------------------------------------------------------------
// <copyright file="ElectronicCertificateAreaRegistration.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Web.Mvc;

namespace WebAdminUI.Areas.ElectronicCertificate
{
    /// <summary>
    /// ElectronicCertificateAreaRegistration
    /// </summary>
    /// <seealso cref="System.Web.Mvc.AreaRegistration" />
    public class ElectronicCertificateAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets the name of the area to register.
        /// </summary>
        public override string AreaName => "ElectronicCertificate";

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              "MenuElectronicCertificate",
              "ElectronicCertificate/Menu",
              new { controller = "Menu", action = "Index" }
            );

            context.MapRoute(
                "ElectronicCertificate_default",
                "ElectronicCertificate/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}