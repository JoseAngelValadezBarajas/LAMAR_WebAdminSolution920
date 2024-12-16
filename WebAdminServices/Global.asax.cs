// --------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System;
using System.Web.Http;

namespace WebAdminServices
{
    /// <inheritdoc />
    /// <summary>
    /// WebApiApplication Class
    /// </summary>
    /// <seealso cref="T:System.Web.HttpApplication" />
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Applications start.
        /// </summary>
        protected void Application_Start()
        {
            Environment.SetEnvironmentVariable("BASEDIR", AppDomain.CurrentDomain.BaseDirectory);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}