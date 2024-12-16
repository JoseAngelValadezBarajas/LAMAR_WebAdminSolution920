// --------------------------------------------------------------------
// <copyright file="PowerCampusMessageHandler.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using PowerCampus.Entities;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebAdminUI.Helpers
{
    /// <summary>
    /// Custom handler to intercept requests
    /// </summary>
    public class PowerCampusMessageHandler : DelegatingHandler
    {
        /// <summary>
        /// Creates a new instance of PowerCampusMessageHandler
        /// </summary>
        public PowerCampusMessageHandler()
        {
            InnerHandler = new HttpClientHandler();
        }

        /// <summary>Sends an HTTP request to the inner handler to send to the server as an asynchronous operation and logs the URL</summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>Returns <see cref="System.Threading.Tasks.Task" />. The task object representing the asynchronous operation.</returns>
        /// <exception cref="System.ArgumentNullException">The <paramref name="request" /> was null.</exception>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LoggerHelper.LogInformation(new LogDetail
            {
                Layer = typeof(PowerCampusMessageHandler).FullName,
                Message = $"API Call: {request.RequestUri}"
            });
            if (request.Content != null)
            {
                LoggerHelper.LogWebError(Constants.WebAdminUi, "PowerCampusMessageHandler", $"    Body:{request.Content.ReadAsStringAsync().Result.ToString()}");
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}