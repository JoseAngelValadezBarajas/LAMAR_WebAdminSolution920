// --------------------------------------------------------------------
// <copyright file="ResponsiblesController.cs" company="Ellucian">
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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Areas.ElectronicCertificate.Mapper;
using WebAdminUI.Areas.ElectronicCertificate.Models.Responsibles;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicCertificate.Controllers
{
    /// <summary>
    /// Signers Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicCertificate)]
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicCertificateSetup)]
    public class ResponsiblesController : WebAdminUI.Controllers.BaseController
    {
        #region Views

        /// <summary>
        /// Adds the signer.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Add()
        {
            try
            {
                AddResponsiblesViewModel responsiblesCatalogViewModel = new AddResponsiblesViewModel();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage respLab = await client.GetAsync(client.BaseAddress + ApiRoute.EcResponsiblesPostitionCatalog);
                if (respLab.StatusCode == HttpStatusCode.OK)
                {
                    ResponsibleModel labCatalogModel = await respLab.Content.ReadAsAsync<ResponsibleModel>();
                    responsiblesCatalogViewModel.Position = labCatalogModel;
                }
                return View(responsiblesCatalogViewModel);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController - Add", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Edits the signer.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Edit()
        {
            try
            {
                AddResponsiblesViewModel responsiblesEditViewModel = new AddResponsiblesViewModel();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage respLab = await client.GetAsync(client.BaseAddress + ApiRoute.EcResponsiblesPostitionCatalog);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + ApiRoute.EcResponsiblesEdit + Account.ResponsibleId);
                if (resp.StatusCode == HttpStatusCode.OK && respLab.StatusCode == HttpStatusCode.OK)
                {
                    ResponsibleModel positionCatalogModel = await respLab.Content.ReadAsAsync<ResponsibleModel>();
                    ResponsibleList responsibleList = await resp.Content.ReadAsAsync<ResponsibleList>();
                    responsiblesEditViewModel.Position = positionCatalogModel;
                    responsiblesEditViewModel.ResponsibleId = responsibleList.ResponsibleId;
                    responsiblesEditViewModel.Name = responsibleList.Name;
                    responsiblesEditViewModel.FirstSurname = responsibleList.FirstSurname;
                    responsiblesEditViewModel.SecondSurname = responsibleList.SecondSurname;
                    responsiblesEditViewModel.Curp = responsibleList.Curp;
                    responsiblesEditViewModel.ResponsiblePositionId = responsibleList.ResponsiblePositionId;
                    responsiblesEditViewModel.IsActive = responsibleList.IsActive;
                    responsiblesEditViewModel.Thumbprint = responsibleList.Thumbprint;
                }
                return View("Edit", responsiblesEditViewModel);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController - Edit", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            try
            {
                List<ResponsiblesIndexList> responsiblesLists = new List<ResponsiblesIndexList>();
                List<ResponsibleList> responsibleModel = new List<ResponsibleList>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + ApiRoute.EcResponsiblesIndex);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    responsibleModel = JsonConvert.DeserializeObject<List<ResponsibleList>>(content);
                    responsiblesLists = responsibleModel.ToViewModel();
                    return View(responsiblesLists);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController - Index", ex.Message);
                throw;
            }
        }

        #endregion Views

        #region JSON

        /// <summary>
        /// Creates the signer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<ActionResult> Create(string model)
        {
            try
            {
                ResponsibleList responsibleList = JsonConvert.DeserializeObject<ResponsibleList>(model);
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    string json = JsonConvert.SerializeObject(responsibleList);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + ApiRoute.EcResponsiblesCreate, httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController - Create", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get the status of the Thumbprint
        /// </summary>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <param name="responsibleId">The responsible identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> Thumbprint(string thumbprint, int responsibleId)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}{ApiRoute.EcResponsibles}{responsibleId}/Thumbprint/{thumbprint}");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    int status = JsonConvert.DeserializeObject<int>(content);
                    return Json(new { status, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                LoggerHelper.LogWebError(Constants.EcArea, "ResponsiblesController", $"{resp.ReasonPhrase} - {resp.RequestMessage}");
                return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, "ResponsiblesController - Thumbprint", ex.Message);
                return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Updates the signer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<ActionResult> Update(string model)
        {
            try
            {
                ResponsibleList responsibleList = JsonConvert.DeserializeObject<ResponsibleList>(model);
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    string json = JsonConvert.SerializeObject(responsibleList);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + ApiRoute.EcUpdateResposibles, httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController - Update", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the curp.
        /// </summary>
        /// <param name="curp">The curp.</param>
        /// <returns></returns>
        public async Task<ActionResult> ValidateCurp(string curp)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + ApiRoute.EcValidateCurp + curp);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    bool exists = JsonConvert.DeserializeObject<bool>(content);
                    return Json(new { exists, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController", resp.RequestMessage.ToString());
                LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController", resp.ReasonPhrase);
                return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "ResponsiblesController - ValidateCurp", ex.Message);
                throw;
            }
        }

        #endregion JSON

        /// <summary>
        /// Sets the specified responsible identifier.
        /// </summary>
        /// <param name="responsibleId">The responsible identifier.</param>
        /// <returns></returns>
        public EmptyResult Set(int responsibleId)
        {
            if (Account != null)
                Account.ResponsibleId = responsibleId;

            return null;
        }
    }
}