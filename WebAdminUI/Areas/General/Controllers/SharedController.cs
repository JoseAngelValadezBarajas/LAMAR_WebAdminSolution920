// --------------------------------------------------------------------
// <copyright file="SharedController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Areas.General.Models;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.General.Controllers
{
    /// <summary>
    /// SharedController
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class SharedController : Controller
    {
        /// <summary>
        /// Abouts this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<PartialViewResult> About()
        {
            AboutViewModel about = new AboutViewModel();
            Account account = (Account)Session["Account"];
            if (account?.status == enumAccess.Authorized)
            {
                HttpClient client = PowerCampusHttpClient.GetClient(account);
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "/api/about");
                if (response.StatusCode == HttpStatusCode.OK)
                    about.Version = await response.Content.ReadAsAsync<string>();
                Assembly assembly = Assembly.GetExecutingAssembly();
                object[] objects = assembly.GetCustomAttributes(false);
                foreach (object o in objects)
                {
                    if (o.GetType() == typeof(System.Reflection.AssemblyFileVersionAttribute))
                    {
                        AssemblyFileVersionAttribute versionAttr = (AssemblyFileVersionAttribute)o;
                        about.UIVersion = versionAttr.Version;
                    }
                }
                const int c_PeHeaderOffset = 60;
                const int c_LinkerTimestampOffset = 8;
                byte[] buffer = new byte[2048];
                using (FileStream stream = new FileStream(assembly.Location, FileMode.Open, FileAccess.Read))
                    stream.Read(buffer, 0, 2048);
                int offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
                int secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime linkTimeUtc = epoch.AddSeconds(secondsSince1970);
                TimeZoneInfo target = null;
                TimeZoneInfo tz = target ?? TimeZoneInfo.Local;
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);
                about.BuildDate = localTime.ToString(account.reportFormats.DateFormat, CultureInfo.InvariantCulture);
                about.Copyright = localTime.Year.ToString();
            }
            return PartialView("_About", about);
        }
    }
}