// --------------------------------------------------------------------
// <copyright file="Startup.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Configuration;
using System.Web.Http;
using WebAdminServices;
using WebAdminServices.Authorize;

[assembly: OwinStartup(typeof(Startup))]

namespace WebAdminServices
{
    /// <summary>
    /// Startup Class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configurations of specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            //enable cors origin requests
            app.UseCors(CorsOptions.AllowAll);
            var myProvider = new AuthorizationServerProvider();
            var electronicDegreeProvider = new AuthorizationElectronicDegree();
            var electronicCertificateProvider = new AuthorizationElectronicCertificate();

            int tokenExpirationTime = int.Parse(ConfigurationManager.AppSettings["TokenExpirationTime"]);
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(tokenExpirationTime),
                AuthenticationType = "bearer",
                AuthenticationMode = AuthenticationMode.Active,
                Provider = myProvider
            };

            OAuthAuthorizationServerOptions electronicDegreeOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token/electronicDegree"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(tokenExpirationTime),
                AuthenticationType = "bearer",
                AuthenticationMode = AuthenticationMode.Active,
                Provider = electronicDegreeProvider
            };

            OAuthAuthorizationServerOptions electronicCertificateOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token/electronicCertificate"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(tokenExpirationTime),
                AuthenticationType = "bearer",
                AuthenticationMode = AuthenticationMode.Active,
                Provider = electronicCertificateProvider
            };

            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthAuthorizationServer(electronicDegreeOptions);
            app.UseOAuthAuthorizationServer(electronicCertificateOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }
}