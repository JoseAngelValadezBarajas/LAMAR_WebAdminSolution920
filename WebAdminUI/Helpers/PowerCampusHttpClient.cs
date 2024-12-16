// --------------------------------------------------------------------
// <copyright file="PowerCampusHttpClient.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebAdminUI.Helpers
{
    /// <summary>
    /// PowerCampusHttpClient Class
    /// </summary>
    public static class PowerCampusHttpClient
    {
        private static readonly string PowerCampusWebApi = ConfigurationManager.AppSettings["WebAdminServicesBaseAddress"];

        /// <summary>
        /// Gets the authenticated client.
        /// </summary>
        /// <returns></returns>
        public static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(PowerCampusWebApi);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="Account">The account model.</param>
        /// <returns></returns>
        public static HttpClient GetClient(Account Account)
        {
            HttpClient client = new HttpClient(new PowerCampusMessageHandler());
            client.BaseAddress = new Uri(PowerCampusWebApi);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            if (Account != null)
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(Account.TokenType, Account.Token);
            }
            return client;
        }
    }
}