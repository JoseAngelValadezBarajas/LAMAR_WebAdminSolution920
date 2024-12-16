// --------------------------------------------------------------------
// <copyright file="LoggingActionFilterAttribute.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebAdminServices.Filters
{
    /// <inheritdoc />
    /// <summary>
    /// LoggingFilterAttribute
    /// </summary>
    /// <seealso cref="T:System.Web.Http.Filters" />
    public sealed class LoggingActionFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Performs logging after the execution of the action
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }

        /// <summary>
        /// Performs logging prior the execution of the action
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }
    }
}