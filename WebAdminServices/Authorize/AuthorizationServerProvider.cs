// --------------------------------------------------------------------
// <copyright file="AuthorizationServerProvider.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Microsoft.Owin.Security.OAuth;
using PowerCampus.Entities;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using WebAdminServices.Controllers;

namespace WebAdminServices
{
    /// <inheritdoc />
    /// <summary>
    /// AuthorizationServerProvider
    /// </summary>
    /// <seealso cref="T:Microsoft.Owin.Security.OAuth.OAuthAuthorizationServerProvider" />
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// Called when a request to the Token endpoint arrives with a "grant_type" of "password". This occurs when the user has provided name and password
        /// credentials directly into the client application's user interface, and the client application is using those to acquire an "AccessToken" and
        /// optional "refresh_token". If the web application supports the
        /// resource owner credentials grant type it must validate the context.Username and context.Password as appropriate. To issue an
        /// access token the context.Validated must be called with a new ticket containing the claims about the resource owner which should be associated
        /// with the access token. The application should take appropriate measures to ensure that the endpoint isn’t abused by malicious callers.
        /// The default behavior is to reject this grant type.
        /// See also http://tools.ietf.org/html/rfc6749#section-4.3.2
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>
        /// Task to enable asynchronous execution
        /// </returns>
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            AccountsController accountController = new AccountsController();
            IHttpActionResult actionResult = accountController.Get(Guid.Parse(context.UserName));
            OkNegotiatedContentResult<Account> contentResult = actionResult as OkNegotiatedContentResult<Account>;
            if (contentResult != null)
            {
                if (contentResult.Content.GUID == Guid.Parse(context.UserName))
                {
                    ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim("guid", contentResult.Content.GUID.ToString()));
                    identity.AddClaim(new Claim("peopleOrgCodeId", contentResult.Content.PeopleOrgCodeId));
                    identity.AddClaim(new Claim("receiptNumber", contentResult.Content.ReceiptNumber));
                    identity.AddClaim(new Claim("userName", contentResult.Content.UserName));
                    identity.AddClaim(new Claim("createDateTime", contentResult.Content.CreateDateTime.ToString()));
                    identity.AddClaim(new Claim("action", contentResult.Content.Action.ToString()));
                    context.Validated(identity);
                    return base.GrantResourceOwnerCredentials(context);
                }
                context.SetError(HttpStatusCode.NotFound.ToString());
                context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);

                return base.GrantResourceOwnerCredentials(context);
            }
            context.SetError(HttpStatusCode.NotFound.ToString());
            context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);

            return base.GrantResourceOwnerCredentials(context);
        }

        /// <summary>
        /// Called to validate that the origin of the request is a registered "client_id", and that the correct credentials for that client are
        /// present on the request. If the web application accepts Basic authentication credentials,
        /// context.TryGetBasicCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request header. If the web
        /// application accepts "client_id" and "client_secret" as form encoded POST parameters,
        /// context.TryGetFormCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request body.
        /// If context.Validated is not called the request will not proceed further.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>
        /// Task to enable asynchronous execution
        /// </returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); //Client has been validated.
            return base.ValidateClientAuthentication(context);
        }
    }
}