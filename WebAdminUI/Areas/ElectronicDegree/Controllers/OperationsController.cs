// --------------------------------------------------------------------
// <copyright file="OperationsController.cs" company="Ellucian">
//     Copyright 2020 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using PowerCampus.Entities.ElectronicDegree;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Areas.ElectronicDegree.Mappers;
using WebAdminUI.Areas.ElectronicDegree.Models;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicDegree.Controllers
{
    /// <summary>
    /// Operations Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class OperationsController : WebAdminUI.Controllers.BaseController
    {
        /// <summary>
        /// Generateds the table.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> _GeneratedTable(string folio, string student, string degreeType, string major, bool isSentAvailable)
        {
            try
            {
                ElectronicDegreeModelList electronicDegreeModelList = new ElectronicDegreeModelList();
                List<ElectronicDegreeModel> electronicDegreeList = new List<ElectronicDegreeModel>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                folio = !folio.Equals("param-folio") ? folio : "";
                student = !student.Equals("param-student") ? student : "";
                degreeType = !degreeType.Equals("param-degreeType") ? degreeType : "";
                major = !major.Equals("param-major") ? major : "";
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/ElectronicDegreeInformation?folio=" + folio +
                    "&student=" + student + "&degreeType=" + degreeType + "&major=" + major);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    electronicDegreeList = JsonConvert.DeserializeObject<List<ElectronicDegreeInfo>>(content).ToViewModel();
                    electronicDegreeModelList.ElectronicDegreesModel = electronicDegreeList;
                    electronicDegreeModelList.IsSentAvailable = isSentAvailable;
                    return PartialView("_GeneratedTable", electronicDegreeModelList);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree 8", "Addvantit", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree 9", "Addvantit", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                    return RedirectToRoute("ErrorException");
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree 10", "Addvantit", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Views the electronic degree information.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> _ViewElectronicDegreeInfo(int id)
        {
            try
            {
                ElectronicDegreeInfo generateViewModel = new ElectronicDegreeInfo();
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/GetElectronicDegreeInformation/" + id);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    generateViewModel = JsonConvert.DeserializeObject<ElectronicDegreeInfo>(content);
                    return PartialView("_ViewElectronicDegreeInfo", generateViewModel);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                    return RedirectToRoute("ErrorException");
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Cancels the ed.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Cancel(SendDocument document)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    string json = JsonConvert.SerializeObject(document);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/ElectronicDegreeServices/Cancel", httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", resp.ReasonPhrase);
                        return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { id = -1, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes the specified electronic degree identifier.
        /// </summary>
        /// <param name="electronicDegreeId">The electronic degree identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(int electronicDegreeId)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage response = await client.DeleteAsync(client.BaseAddress + "/api/Delete/" + electronicDegreeId);
                if (Account.status == enumAccess.Authorized)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", response.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", response.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", response.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", response.ReasonPhrase);
                        return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { id = -1, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Downloads the original string.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> DownloadOriginalString(int id)
        {
            try
            {
                ElectronicDegreeInfo generateViewModel = new ElectronicDegreeInfo();
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/GetElectronicDegreeInformation/" + id);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    generateViewModel = JsonConvert.DeserializeObject<ElectronicDegreeInfo>(content);
                    byte[] toBytes = Encoding.UTF8.GetBytes(generateViewModel.OriginalString);
                    DateTime now = DateTime.Now;
                    string dateNow = now.ToString("yyyyMMdd");
                    string timeNow = now.ToString("HHmm");
                    //return File(toBytes, "text/csv", $"{generateViewModel.Folio}_{dateNow}_{timeNow}.txt");
                    //MOD ADDV 11/12/2024
                    return File(toBytes, "text/csv", $"{generateViewModel.Folio}.txt");
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                    return RedirectToRoute("ErrorException");
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Downloads the XML.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> DownloadXml(int id)
        {
            try
            {
                ElectronicDegreeInfo generateViewModel = new ElectronicDegreeInfo();
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/GetElectronicDegreeInformation/" + id);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    generateViewModel = JsonConvert.DeserializeObject<ElectronicDegreeInfo>(content);
                    byte[] toBytes = Encoding.UTF8.GetBytes(generateViewModel.RequestXML);
                    DateTime now = DateTime.Now;
                    string dateNow = now.ToString("yyyyMMdd");
                    string timeNow = now.ToString("HHmm");
                    //return File(toBytes, "application/xml", $"{generateViewModel.Folio}_{dateNow}_{timeNow}.xml");
                    //MOD ADDV 11/12/2024
                    return File(toBytes, "application/xml", $"{generateViewModel.Folio}.xml");
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                    return RedirectToRoute("ErrorException");
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", ex.Message);
                throw;
            }
        }


        /// Downloads the XMLSTAMP AADDV
        //public async Task<ActionResult> DownloadXmlStamp(int id)
        //{
        //    try
        //    {
        //        ElectronicDegreeInfo generateViewModel = new ElectronicDegreeInfo();
        //        HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
        //        HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/GetElectronicDegreeInformation/" + id);
        //        if (resp.StatusCode == HttpStatusCode.OK)
        //        {
        //            string content = await resp.Content.ReadAsStringAsync();
        //            generateViewModel = JsonConvert.DeserializeObject<ElectronicDegreeInfo>(content);
        //            byte[] toBytes = Encoding.UTF8.GetBytes(generateViewModel.RequestXML_Stamp);
        //            DateTime now = DateTime.Now;
        //            string dateNow = now.ToString("yyyyMMdd");
        //            string timeNow = now.ToString("HHmm");
        //            return File(toBytes, "application/xml", $"{generateViewModel.Folio}_{dateNow}_{timeNow}.xml");
        //        }
        //        else if (resp.StatusCode == HttpStatusCode.Unauthorized)
        //        {
        //            LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", resp.StatusCode.ToString());
        //            LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", resp.ReasonPhrase);
        //            return RedirectToRoute("ErrorUnauthorized");
        //        }
        //        else
        //            return RedirectToRoute("ErrorException");
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", ex.Message);
        //        throw;
        //    }
        //}

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            try
            {
                ElectronicDegreeModelList electronicDegreeModelList = new ElectronicDegreeModelList();
                List<ElectronicDegreeModel> electronicDegreeList = new List<ElectronicDegreeModel>();
                List<CodeCancelationCatalog> codeCancelationCatalog = new List<CodeCancelationCatalog>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/ElectronicDegreeInformation?folio=" + "*" +
                    "&student=" + "*" + "&degreeType=" + "*" + "&major=" + "*");
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "api/CancelationCatalog");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    string contentCatalog = await response.Content.ReadAsStringAsync();
                    codeCancelationCatalog = JsonConvert.DeserializeObject<List<CodeCancelationCatalog>>(contentCatalog);
                    electronicDegreeList = JsonConvert.DeserializeObject<List<ElectronicDegreeInfo>>(content).ToViewModel();
                    electronicDegreeModelList.ElectronicDegreesModel = electronicDegreeList;
                    electronicDegreeModelList.ElectronicDegreeParameters = new ElectronicDegreeParameters();
                    electronicDegreeModelList.IsSentAvailable = true;
                    electronicDegreeModelList.CodeCancelationCatalog = codeCancelationCatalog;
                    ElectronicDegreeParameters parameters = new ElectronicDegreeParameters();
                    parameters.EducationType = new List<ListOption>()
                    {
                        new ListOption(){ Value = Resources.ElectronicDegree.Major.lblBachilleratoTecnico, Description = Resources.ElectronicDegree.Major.lblBachilleratoTecnico },
                        new ListOption(){ Value = Resources.ElectronicDegree.Major.lblDoctorado, Description = Resources.ElectronicDegree.Major.lblDoctorado },
                        new ListOption(){ Value = Resources.ElectronicDegree.Major.lblEspecialidad, Description = Resources.ElectronicDegree.Major.lblEspecialidad },
                        new ListOption(){ Value = Resources.ElectronicDegree.Major.lblLicenciatura, Description = Resources.ElectronicDegree.Major.lblLicenciatura },
                        new ListOption(){ Value = Resources.ElectronicDegree.Major.lblMaestria, Description = Resources.ElectronicDegree.Major.lblMaestria },
                        new ListOption(){ Value = Resources.ElectronicDegree.Major.lblProfesionalAsociado, Description = Resources.ElectronicDegree.Major.lblProfesionalAsociado },
                        new ListOption(){ Value = Resources.ElectronicDegree.Major.lblProfesionalTecnico, Description = Resources.ElectronicDegree.Major.lblProfesionalTecnico },
                        new ListOption(){ Value = Resources.ElectronicDegree.Major.lblProfesor, Description = Resources.ElectronicDegree.Major.lblProfesor },
                        new ListOption(){ Value = Resources.ElectronicDegree.Major.lblTecnico, Description = Resources.ElectronicDegree.Major.lblTecnico },
                        new ListOption(){ Value = Resources.ElectronicDegree.Major.lblTecnicoProfesional, Description = Resources.ElectronicDegree.Major.lblTecnicoProfesional },
                        new ListOption(){ Value = Resources.ElectronicDegree.Major.lblTecnicoSuperior, Description = Resources.ElectronicDegree.Major.lblTecnicoSuperior },
                        new ListOption(){ Value = Resources.ElectronicDegree.Major.lblTecnicoSuperiorUniv, Description = Resources.ElectronicDegree.Major.lblTecnicoSuperiorUniv },
                        new ListOption(){ Value = Resources.ElectronicDegree.Major.lblTecnologico, Description = Resources.ElectronicDegree.Major.lblTecnologico }
                    };

                    List<MajorList> majorListCatalog = null;
                    resp = await client.GetAsync(client.BaseAddress + "/api/Majors");
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        string majorsContent = await resp.Content.ReadAsStringAsync();
                        majorListCatalog = JsonConvert.DeserializeObject<List<MajorList>>(majorsContent);
                        List<ListOption> majors = new List<ListOption>();
                        foreach (MajorList major in majorListCatalog)
                        {
                            majors.Add(new ListOption
                            {
                                Description = major.Name,
                                Value = major.Code
                            });
                        }
                        parameters.Major = majors;

                        electronicDegreeModelList.ElectronicDegreeParameters = parameters;

                        return View(electronicDegreeModelList);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree 4", "Addvantit", resp.StatusCode.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree 5", "Addvantit", resp.ReasonPhrase);
                        return RedirectToRoute("ErrorUnauthorized");
                    }
                    else
                        return RedirectToRoute("ErrorException");
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree 6", "Addvantit", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree 7", "Addvantit", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                    return RedirectToRoute("ErrorException");
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Stamps the send.
        /// </summary>
        /// <param name="electronicDegreeId">The electronic degree identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> StampSend(int electronicDegreeId)
        {
            try
            {
                string json = JsonConvert.SerializeObject(new
                {
                    electronicDegreeId,
                    operatorName = Account.UserName
                });

                LoggerHelper.LogInformation(new LogDetail
                {
                    Layer = typeof(OperationsController).FullName,
                    Message = $"Send request for {electronicDegreeId}"

                });

                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/ElectronicDegreeServices/Send", httpContent);

                LoggerHelper.LogWebError("ElectronicDegree -StampSend", "Addvantit ", client.BaseAddress + "/api/ElectronicDegreeServices/Send");
                LoggerHelper.LogWebError("ElectronicDegree -StampSend", "Addvantit ", "Estatus-Resp" + resp.StatusCode.ToString());

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = await resp.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    string content = readStream.ReadToEnd();
                    LoggerHelper.LogInformation(new LogDetail
                    {
                        Layer = typeof(OperationsController).FullName,
                        Message = $"Send response for {electronicDegreeId}: \n {content}"
                    });

                    SepServicesResponse sepResponse = JsonConvert.DeserializeObject<SepServicesResponse>(content);

                    return Json(new { id = 1, sepResponse.status, message = CustomMessages.Success });
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree -StampSend", "Addvantit", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree -StampSend", "Addvantit", resp.ReasonPhrase);
                    return Json(new { id = 0, message = CustomMessages.SessionExpired });
                }
                else
                {
                    return Json(new { id = -1, message = CustomMessages.Error });
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree -StampSend", "Addvantit", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="electronicDegreeId">The electronic degree identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UpdateStatus(int electronicDegreeId)
        {
            try
            {
                string json = JsonConvert.SerializeObject(new
                {
                    electronicDegreeId,
                    operatorName = Account.UserName
                });

                LoggerHelper.LogInformation(new LogDetail
                {
                    Layer = typeof(OperationsController).FullName,
                    Message = $"Update request for {electronicDegreeId}"
                });

                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "api/ElectronicDegreeServices/Update", httpContent);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = await resp.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    string content = readStream.ReadToEnd();
                    LoggerHelper.LogInformation(new LogDetail
                    {
                        Layer = typeof(OperationsController).FullName,
                        Message = $"Update response for {electronicDegreeId}: \n {content}"
                    });

                    SepServicesResponse sepResponse = JsonConvert.DeserializeObject<SepServicesResponse>(content);

                    return Json(new { id = 1, sepResponse.status, message = CustomMessages.Success });
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", resp.ReasonPhrase);
                    return Json(new { id = 0, message = CustomMessages.SessionExpired });
                }
                else
                {
                    return Json(new { id = -1, message = CustomMessages.Error });
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "OperationsController", ex.Message);
                throw;
            }
        }
    }
}