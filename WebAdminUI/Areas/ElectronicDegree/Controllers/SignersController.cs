// --------------------------------------------------------------------
// <copyright file="SignersController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using PowerCampus.Entities.ElectronicDegree;
using Resources;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Areas.ElectronicDegree.Mappers;
using WebAdminUI.Areas.ElectronicDegree.Models.Signers;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicDegree.Controllers
{
    /// <summary>
    /// Signers Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicDegree)]
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicDegreeSetup)]
    public class SignersController : WebAdminUI.Controllers.BaseController
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
                AddSignersViewModel signersCatalogViewModel = new AddSignersViewModel();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage respLab = await client.GetAsync(client.BaseAddress + "/api/Signers/LaborCatalog");
                HttpResponseMessage respAbb = await client.GetAsync(client.BaseAddress + "/api/Signers/TitleAbbreviation");
                if (respLab.StatusCode == HttpStatusCode.OK && respAbb.StatusCode == HttpStatusCode.OK)
                {
                    SignersModel labCatalogModel = await respLab.Content.ReadAsAsync<SignersModel>();
                    SignersModel abbCatalogModel = await respAbb.Content.ReadAsAsync<SignersModel>();
                    signersCatalogViewModel.LaborPosition = labCatalogModel;
                    signersCatalogViewModel.ProfessionalTitleAbbreviation = abbCatalogModel;
                }
                return View(signersCatalogViewModel);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "SignersController - Add", ex.Message);
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
                AddSignersViewModel signersEditViewModel = new AddSignersViewModel();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage respLab = await client.GetAsync(client.BaseAddress + "/api/Signers/LaborCatalog");
                HttpResponseMessage respAbb = await client.GetAsync(client.BaseAddress + "/api/Signers/TitleAbbreviation");
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Signers/Edit/?signerId=" + Account.SignerId);
                if (respLab.StatusCode == HttpStatusCode.OK && respAbb.StatusCode == HttpStatusCode.OK)
                {
                    SignersModel labCatalogModel = await respLab.Content.ReadAsAsync<SignersModel>();
                    SignersModel abbCatalogModel = await respAbb.Content.ReadAsAsync<SignersModel>();
                    SignerList signerList = await resp.Content.ReadAsAsync<SignerList>();
                    signersEditViewModel.LaborPosition = labCatalogModel;
                    signersEditViewModel.ProfessionalTitleAbbreviation = abbCatalogModel;
                    signersEditViewModel.SignerId = signerList.SignerId;
                    signersEditViewModel.Name = signerList.Name;
                    signersEditViewModel.FirstSurname = signerList.FirstSurname;
                    signersEditViewModel.SecondSurname = signerList.SecondSurname;
                    signersEditViewModel.Curp = signerList.Curp;
                    signersEditViewModel.AbbreviationId = signerList.AbreviationTitleId;
                    signersEditViewModel.SignerPositionId = signerList.SignerPositionId;
                    signersEditViewModel.IsActive = signerList.IsActive;
                    signersEditViewModel.Thumbprint = signerList.Thumbprint;
                }
                return View("Edit", signersEditViewModel);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "SignersController - Edit", ex.Message);
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
                List<SignersIndexList> signerLists = new List<SignersIndexList>();
                List<SignerList> signerModel = new List<SignerList>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Index");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    signerModel = JsonConvert.DeserializeObject<List<SignerList>>(content);
                    signerLists = signerModel.ToViewModel();
                    return View(signerLists);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "SignersController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "SignersController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "SignersController - Index", ex.Message);
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
                SignerList signerList = JsonConvert.DeserializeObject<SignerList>(model);
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    string json = JsonConvert.SerializeObject(signerList);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Signers/Create", httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "SignersController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "SignersController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "SignersController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "SignersController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicDegree", "SignersController - Create", ex.Message);
                throw;
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
                SignerList signerList = JsonConvert.DeserializeObject<SignerList>(model);
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    string json = JsonConvert.SerializeObject(signerList);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Signers/Update", httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "SignersController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "SignersController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "SignersController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "SignersController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicDegree", "SignersController - Update", ex.Message);
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
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Signers/ValidateCurp/" + curp);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    bool exists = JsonConvert.DeserializeObject<bool>(content);
                    return Json(new { exists, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                LoggerHelper.LogWebError("ElectronicDegree", "SignersController", resp.RequestMessage.ToString());
                LoggerHelper.LogWebError("ElectronicDegree", "SignersController", resp.ReasonPhrase);
                return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "SignersController - ValidateCurp", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the thumprint.
        /// </summary>
        /// <param name="thumprint">The thumprint.</param>
        /// <returns></returns>
        public async Task<ActionResult> ValidateThumprint(string thumprint)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Signers/ValidateThumprint/" + thumprint);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    bool IsValid = JsonConvert.DeserializeObject<bool>(content);
                    return Json(new { IsValid, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                LoggerHelper.LogWebError("ElectronicDegree", "SignersController", resp.RequestMessage.ToString());
                LoggerHelper.LogWebError("ElectronicDegree", "SignersController", resp.ReasonPhrase);
                return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "SignersController - ValidateThumprint", ex.Message);
                throw;
            }
        }

        #endregion JSON

        /// <summary>
        /// Gets the edit signer.
        /// </summary>
        /// <param name="signerId">The signer identifier.</param>
        /// <returns></returns>
        public EmptyResult Set(int signerId)
        {
            if (Account != null)
                Account.SignerId = signerId;

            return null;
        }
    }
}