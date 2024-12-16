// --------------------------------------------------------------------
// <copyright file="LoggingExceptionFilterAttribute.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using PowerCampus.Entities;
using System.Web.Http.Filters;

namespace WebAdminServices.Filters
{
    /// <summary>
    /// LoggingExceptionFilterAttribute
    /// </summary>
    /// <seealso cref="System.Web.Http.Filters.ExceptionFilterAttribute" />
    public sealed class LoggingExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="actionExecutedContext">The context for the action.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            LoggerHelper.LogWebError(Constants.WebAdminServices, "LoggingExceptionFilterAttribute",
                $"Route: {actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName}/{actionExecutedContext.ActionContext.ActionDescriptor.ActionName}, " +
                $"Exception: {actionExecutedContext.Exception.GetType()}, Message: {actionExecutedContext.Exception.Message}, StackTrace: {actionExecutedContext.Exception.StackTrace}");
            base.OnException(actionExecutedContext);
        }
    }
}