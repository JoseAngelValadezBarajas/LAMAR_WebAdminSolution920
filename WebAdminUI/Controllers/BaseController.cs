//--------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
//--------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using SelfService.Helpers;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebAdminUI.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Base Controller with all the shared behaviors for all its child controllers
    /// </summary>
    /// <seealso cref="T:System.Web.Mvc.Controller" />
    public class BaseController : Controller
    {
        /// <summary>
        /// Gets the account information from the session (if exists)
        /// </summary>
        protected Account Account
        {
            get => Session["Account"] as Account;
            set => Session["Account"] = value;
        }

        /// <summary>
        /// Gets or sets the build information.
        /// </summary>
        /// <value>
        /// The build information.
        /// </value>
        protected BuildInformation BuildInfo { get; set; }

        /// <summary>
        /// Begins execution of the specified request context
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="callback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        /// Returns an IAsyncController instance.
        /// </returns>
        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            LoggerHelper.LogInformation(new LogDetail
            {
                Layer = typeof(BaseController).FullName,
                Message = $"Request [{requestContext.RouteData.Values["controller"]}] - [{requestContext.RouteData.Values["action"]}]  will start"
            });

            if (requestContext.HttpContext.Application["BuildInfo"] != null)
            {
                BuildInfo = (BuildInformation)requestContext.HttpContext.Application["BuildInfo"];
                ViewBag.BuildInfo = BuildInfo;
            }
            else
            {
                string json = PowerCampusSerializer.ReadJson(requestContext.HttpContext.Server.MapPath("~/App_Data"), "buildInfo");
                BuildInfo = JsonConvert.DeserializeObject<BuildInformation>(json);
                ViewBag.BuildInfo = BuildInfo;
            }

            return base.BeginExecute(requestContext, callback, state);
        }

        /// <summary>
        /// Performs a set of validations after the execution of the action
        /// </summary>
        /// <param name="filterContext">The filter context</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            LoggerHelper.LogInformation(new LogDetail
            {
                Layer = typeof(BaseController).FullName,
                Message = $"Request [{filterContext.RouteData.Values["controller"]}] - [{filterContext.RouteData.Values["action"]}]  ends"
            });
            base.OnActionExecuted(filterContext);
        }

        /// <inheritdoc />
        /// <summary>
        /// Performs a set of validations prior the execution of the action
        /// </summary>
        /// <param name="filterContext">The filter context</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoggerHelper.LogInformation(new LogDetail
            {
                Layer = typeof(BaseController).FullName,
                Message = $"Request [{filterContext.RouteData.Values["controller"]}] - [{filterContext.RouteData.Values["action"]}]  starts"
            });

            ViewBag.SessionExist = Account != null;
            ViewBag.Account = Account;

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Performs a general logging actions for all the unhandled exceptions
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            LoggerHelper.LogWebError(Constants.WebAdminUi, "OnException",
                $"Exception: {filterContext.GetType()} Message: {filterContext.Exception.Message} StackTrace: {filterContext.Exception.StackTrace}", filterContext.Exception);
            filterContext.ExceptionHandled = true;
            filterContext.Result = RedirectToRoute("ErrorException");
            base.OnException(filterContext);
        }

        /// <summary>
        /// Performs logging after the action result executes.
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            LoggerHelper.LogInformation(new LogDetail
            {
                Layer = typeof(BaseController).FullName,
                Message = $"Request [{filterContext.RouteData.Values["controller"]}] - [{filterContext.RouteData.Values["action"]}]  executed"
            });
            base.OnResultExecuted(filterContext);
        }

        protected ActionResult Ok(object value)
        {
            return new OkObjectResult(value);
        }

        private class OkObjectResult : ActionResult
        {
            private object value;

            public OkObjectResult(object value)
            {
                this.value = value;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                throw new NotImplementedException();
            }
        }
    }
}