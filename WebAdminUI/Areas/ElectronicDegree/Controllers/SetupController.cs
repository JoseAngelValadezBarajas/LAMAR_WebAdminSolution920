// --------------------------------------------------------------------
// <copyright file="SetupController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System.Web.Mvc;
using WebAdminUI.Filter;

namespace WebAdminUI.Areas.ElectronicDegree.Controllers
{
    /// <summary>
    /// Setup Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicDegree)]
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicDegreeSetup)]
    public class SetupController : WebAdminUI.Controllers.BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() => View();
    }
}