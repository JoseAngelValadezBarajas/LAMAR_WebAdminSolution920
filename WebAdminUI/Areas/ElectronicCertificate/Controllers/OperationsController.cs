// --------------------------------------------------------------------
// <copyright file="OperationsController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using PowerCampus.Entities.ElectronicCertificate;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Areas.ElectronicCertificate.Mappers;
using WebAdminUI.Areas.ElectronicCertificate.Models.Generation;
using WebAdminUI.Areas.ElectronicCertificate.Models.Shared;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;
using WebAdminUI.Resources.ElectronicCertificate;

namespace WebAdminUI.Areas.ElectronicCertificate.Controllers
{
    /// <summary>
    /// OperationsController
    /// </summary>
    /// <seealso cref="WebAdminUI.Controllers.BaseController" />
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicCertificate)]
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicCertificateOperations)]
    public class OperationsController : WebAdminUI.Controllers.BaseController
    {
        /// <summary>
        /// Generateds the table.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> _GeneratedTable(Search search)
        {
            try
            {
                List<ElectronicCertificateViewModel> electronicCertificatesModelList = new List<ElectronicCertificateViewModel>();
                if (Account.status == enumAccess.Authorized)
                {
                    string json = JsonConvert.SerializeObject(search);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage resp = null;
                    if (search.Advanced)
                        resp = await client.PostAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesAdvancedSearch}", httpContent);
                    else
                        resp = await client.PostAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesBasicSearch}", httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await resp.Content.ReadAsStringAsync();
                        electronicCertificatesModelList = JsonConvert.DeserializeObject<List<Certificate>>(content).ToViewModel(false);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.StatusCode.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.ReasonPhrase);
                        return RedirectToRoute("ErrorUnauthorized");
                    }
                }
                return PartialView("_GeneratedTable", electronicCertificatesModelList);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Views the data.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> _ViewData(int id)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesDetail}{id}");
                ElectronicCertificateInfoViewModel electronicCertificateInfoViewModel = new ElectronicCertificateInfoViewModel();
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    electronicCertificateInfoViewModel = JsonConvert.DeserializeObject<CertificateInfo>(content).ToViewModel();
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                return PartialView(electronicCertificateInfoViewModel);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Downloads the files.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<FileResult> DownloadFiles(string status, string ids)
        {
            StringContent httpContent = new StringContent(ids, Encoding.UTF8, "application/json");
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.PostAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesFiles}", httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                List<CertificateFile> certificateFiles = await resp.Content.ReadAsAsync<List<CertificateFile>>();
                DateTime now = DateTime.Now;
                string dateNow = now.ToString("yyyyMMdd");
                string timeNow = now.ToString("HHmm");
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (CertificateFile certificateFile in certificateFiles)
                        {
                            if (certificateFile.PdfFile != null)
                                AddToArchive(zipArchive, $"{certificateFile.Folio}_{dateNow}_{timeNow}.pdf", certificateFile.PdfFile);
                            AddToArchive(zipArchive, $"{certificateFile.Folio}_{dateNow}_{timeNow}.xml", Encoding.UTF8.GetBytes(certificateFile.XmlFile));
                        }
                    }
                    return File(memoryStream.ToArray(), "application/zip", $"CE_{status.Replace(" ", string.Empty)}_{dateNow}_{timeNow}.zip");
                }
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.StatusCode.ToString());
                LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.ReasonPhrase);
                return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                ElectronicCertificateHeaderList electronicCertificateList = new ElectronicCertificateHeaderList();
                electronicCertificateList.CanGenerateNew = false;

                electronicCertificateList.ElectronicCertificateParameters = new ElectronicCertificateParameters();
                electronicCertificateList.ElectronicCertificateParameters.ShowAllStatus = true;

                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                string content;

                HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesCampuses}All");
                List<DropdownListViewModel> campuses = null;
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    content = await resp.Content.ReadAsStringAsync();
                    campuses = JsonConvert.DeserializeObject<List<CodeTable>>(content).ToViewModel();
                }
                electronicCertificateList.ElectronicCertificateParameters.Campus = campuses ?? new List<DropdownListViewModel>();

                resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesCertificationTypes}All");
                List<DropdownListViewModel> certificationTypes = null;
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    content = await resp.Content.ReadAsStringAsync();
                    certificationTypes = JsonConvert.DeserializeObject<List<CodeTable>>(content).ToViewModel();
                }
                electronicCertificateList.ElectronicCertificateParameters.CertificationType = certificationTypes ?? new List<DropdownListViewModel>();

                resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesMajors}All");
                List<DropdownListViewModel> majors = null;
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    content = await resp.Content.ReadAsStringAsync();
                    majors = JsonConvert.DeserializeObject<List<CodeTable>>(content).ToViewModel();
                }
                electronicCertificateList.ElectronicCertificateParameters.Program = majors ?? new List<DropdownListViewModel>();

                resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesStatuses}");
                List<DropdownListViewModel> status = new List<DropdownListViewModel>();
                List<string> statusList = null;
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    content = await resp.Content.ReadAsStringAsync();
                    statusList = JsonConvert.DeserializeObject<List<string>>(content);
                }
                if (statusList != null)
                {
                    foreach (string statusCode in statusList)
                    {
                        status.Add(new DropdownListViewModel
                        {
                            CodeValueKey = statusCode,
                            Description = GetStatusDescription(statusCode)
                        });
                    }
                }
                electronicCertificateList.ElectronicCertificateParameters.Status = status;
                return View(electronicCertificateList);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "GenerationsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// PDFs the download.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<FileResult> PDFDownload(int id)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesPdf}{id}");

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                CertificateFile certificateFile = await resp.Content.ReadAsAsync<CertificateFile>();
                DateTime now = DateTime.Now;
                string dateNow = now.ToString("yyyyMMdd");
                string timeNow = now.ToString("HHmm");
                //return File(certificateFile.PdfFile, "application/pdf", $"{certificateFile.Folio}_{dateNow}_{timeNow}.pdf");
                //MOD ADDV 11/12/2024
                return File(certificateFile.PdfFile, "application/pdf", $"{certificateFile.Folio}.pdf");
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.StatusCode.ToString());
                LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.ReasonPhrase);
                return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// XMLs the download.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<FileResult> XMLDownload(int id)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesXml}{id}");
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                CertificateFile certificateFile = await resp.Content.ReadAsAsync<CertificateFile>();
                byte[] toBytes = Encoding.UTF8.GetBytes(certificateFile.XmlFile);
                DateTime now = DateTime.Now;
                string dateNow = now.ToString("yyyyMMdd");
                string timeNow = now.ToString("HHmm");
                //return File(toBytes, "application/xml", $"{certificateFile.Folio}_{dateNow}_{timeNow}.xml");
                //MOD ADDV 11/12/2024
                return File(toBytes, "application/xml", $"{certificateFile.Folio}.xml");
            }
            else if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.StatusCode.ToString());
                LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.ReasonPhrase);
                return null;
            }
            else
            {
                return null;
            }
        }

        #region JSON

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.DeleteAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesDelete}{id}");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.ReasonPhrase);
                    return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.ReasonPhrase);
                    return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Stamps the specified identifier.
        /// </summary>
        /// <param name="certificateOperation">The certificate operation.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Stamp(CertificateOperation certificateOperation)
        {
            try
            {
                if (certificateOperation == null)
                {
                    return Json(new { id = -1, message = CustomMessages.Error });
                }

                if (certificateOperation.CertificateId > 0 && certificateOperation.Status.Equals(CertificateStatus.Stamped) && certificateOperation.GenerateXml)
                {
                    certificateOperation.OperatorId = Account.UserName;
                    string certificateOperationJson = JsonConvert.SerializeObject(certificateOperation);
                    HttpResponseMessage response = await GetResponseAsync(HttpVerbs.Post, certificateOperationJson, ApiRoute.EcCertificatesStamp);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success });
                    }
                }
                return Json(new { id = -1, message = CustomMessages.Error });
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "OperationsController - Stamp", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="certificateOperation">The certificate operation.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UpdateStatus(CertificateOperation certificateOperation)
        {
            try
            {
                certificateOperation.OperatorId = Account.UserName;
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                string json = JsonConvert.SerializeObject(certificateOperation);
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage resp = await client.PostAsync($"{client.BaseAddress}{ApiRoute.EcCertificatesUpdateStatus}", httpContent);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.DenyGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.ReasonPhrase);
                    return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.RequestMessage.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", resp.ReasonPhrase);
                    return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "OperationsController", ex.Message);
                throw;
            }
        }

        #endregion JSON

        #region Private Methods

        private void AddToArchive(ZipArchive ziparchive, string fileName, byte[] attach)
        {
            ZipArchiveEntry zipEntry = ziparchive.CreateEntry(fileName, CompressionLevel.Optimal);
            using (Stream zipStream = zipEntry.Open())
            using (MemoryStream streamIn = new MemoryStream(attach))
                streamIn.CopyTo(zipStream);
        }

        /// <summary>
        /// Gets the response asynchronous.
        /// </summary>
        /// <param name="httpVerb">The HTTP verb.</param>
        /// <param name="json">The json.</param>
        /// <param name="apiRoute">The API route will be concatenated as client.BaseAddress + apiRoute</param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> GetResponseAsync(HttpVerbs httpVerb, string json, string apiRoute)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage response = null;
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                switch (httpVerb)
                {
                    case HttpVerbs.Get:
                        break;

                    case HttpVerbs.Post:
                        response = await client.PostAsync(client.BaseAddress + apiRoute, httpContent);
                        break;

                    case HttpVerbs.Put:
                        break;

                    case HttpVerbs.Delete:
                        break;

                    case HttpVerbs.Head:
                        break;

                    case HttpVerbs.Patch:
                        break;

                    case HttpVerbs.Options:
                        break;

                    default:
                        break;
                }

                return response;
            }
            catch (Exception exception)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "GenerationsController - GetResponseAsync", exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the status description.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        private string GetStatusDescription(string code)
        {
            string description = string.Empty;
            switch (code)
            {
                case CertificateStatus.Generated:
                    description = Parameters.lblGenerated;
                    break;

                case CertificateStatus.Stamped:
                    description = Parameters.lblStamped;
                    break;

                case CertificateStatus.Processed:
                    description = Parameters.lblProcessedCorrectly;
                    break;

                case CertificateStatus.Error:
                    description = Parameters.lblError;
                    break;

                case CertificateStatus.Canceled:
                    description = Parameters.lblCanceled;
                    break;

                default:
                    break;
            }
            return description;
        }

        #endregion Private Methods
    }
}