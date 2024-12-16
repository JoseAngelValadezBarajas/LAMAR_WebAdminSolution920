// --------------------------------------------------------------------
// <copyright file="ElectronicDegreeAreaRegistration.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Web.Mvc;

namespace WebAdminUI.Areas.ElectronicDegree
{
    /// <summary></summary>
    /// <seealso cref="System.Web.Mvc.AreaRegistration" />
    public class ElectronicDegreeAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets the name of the area to register.
        /// </summary>
        public override string AreaName => "ElectronicDegree";

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              "MenuElectronicDegree",
              "ElectronicDegree/Menu",
              new { controller = "Menu", action = "Index" }
            );

            context.MapRoute(
                "ElectronicDegree_default",
                "ElectronicDegree/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}