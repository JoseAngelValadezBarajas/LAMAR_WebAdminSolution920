// --------------------------------------------------------------------
// <copyright file="PermissionRequiredAttribute.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using PowerCampus.Entities;
using System;
using System.Linq;
using System.Web.Mvc;

namespace WebAdminUI.Filter
{
    /// <summary>
    /// Filter attribute to validate permissions
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class PermissionRequiredAttribute : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// Gets or sets the name of the permission.
        /// </summary>
        /// <value>
        /// The name of the permission.
        /// </value>
        public string PermissionName { get; set; }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Account account = filterContext.RequestContext.HttpContext.Session["Account"] as Account;
            if (account == null)
            {
                LoggerHelper.LogInformation(new LogDetail
                {
                    Layer = typeof(PermissionRequiredAttribute).FullName,
                    Message = "Token has expired"
                });
                filterContext.Result = RedirectToRoute("ErrorExpired");
                return;
            }

            Permissions permission = account.permissions?.FirstOrDefault(p => p.Description == PermissionName && p.Value);
            if (permission == null)
            {
                LoggerHelper.LogInformation(new LogDetail
                {
                    Layer = typeof(PermissionRequiredAttribute).FullName,
                    Message = $"User {account.UserName} requires permission '{PermissionName}'"
                });
                filterContext.Result = RedirectToRoute("ErrorUnauthorized");
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        private RedirectToRouteResult RedirectToRoute(string routeName) => new RedirectToRouteResult(routeName, null);
    }
}