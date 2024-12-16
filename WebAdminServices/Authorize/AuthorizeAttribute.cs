// --------------------------------------------------------------------
// <copyright file="AuthorizeAttribute.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Controllers;

namespace WebAdminServices
{
    /// <inheritdoc />
    /// <summary>
    /// Authorize Attribute Class
    /// </summary>
    /// <seealso cref="T:System.Web.Http.AuthorizeAttribute" />
    public class AuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        /// <summary>
        /// Processes requests that fail authorization.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        /// <inheritdoc />
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
                base.HandleUnauthorizedRequest(actionContext);
            }
            else
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }
    }
}