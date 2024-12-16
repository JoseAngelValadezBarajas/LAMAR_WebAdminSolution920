// --------------------------------------------------------------------
// <copyright file="MajorsController.cs" company="Ellucian">
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
using WebAdminUI.Areas.ElectronicDegree.Models.Majors;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicDegree.Controllers
{
    /// <summary>
    /// Majors Controller
    /// </summary>
    /// <seealso cref="WebAdminUI.Controllers.BaseController" />
    public class MajorsController : WebAdminUI.Controllers.BaseController
    {
        #region Views

        /// <summary>
        /// Adds the major.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Add()
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                CodeLegalBase[] legalBaseCatalog = null;
                HttpResponseMessage respLegalBase = await client.GetAsync(client.BaseAddress + "/api/LegalBase/Catalog");
                if (respLegalBase.StatusCode == HttpStatusCode.OK)
                {
                    string content = await respLegalBase.Content.ReadAsStringAsync();
                    legalBaseCatalog = JsonConvert.DeserializeObject<CodeLegalBase[]>(content);
                }

                AddMajorsViewModel addMajorsViewModel = new AddMajorsViewModel
                {
                    EducationLevel = GetEducationLevelOptions(),
                    LegalBaseCatalog = legalBaseCatalog
                };
                return View(addMajorsViewModel);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EdArea, "MajorsController - Add", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Edits the major.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Edit()
        {
            try
            {
                AddMajorsViewModel editMajorsViewModel = new AddMajorsViewModel();
                List<ListOption> equivalents = new List<ListOption>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Majors/" + Account.MajorId);
                HttpResponseMessage respEquivalents = await client.GetAsync(client.BaseAddress + "/api/Institution/GetPowerCampusInstitutions");
                HttpResponseMessage respLegalBase = await client.GetAsync(client.BaseAddress + "/api/LegalBase/Catalog");

                if (resp.StatusCode == HttpStatusCode.OK && respEquivalents.StatusCode == HttpStatusCode.OK && respLegalBase.StatusCode == HttpStatusCode.OK)
                {
                    MajorList majorList = await resp.Content.ReadAsAsync<MajorList>();
                    string content = await resp.Content.ReadAsStringAsync();
                    string contentEq = await respEquivalents.Content.ReadAsStringAsync();
                    string contentLegalBase = await respLegalBase.Content.ReadAsStringAsync();
                    CodeLegalBase[] legalBaseCatalog = JsonConvert.DeserializeObject<CodeLegalBase[]>(contentLegalBase);

                    equivalents = JsonConvert.DeserializeObject<List<ListOption>>(contentEq);
                    editMajorsViewModel.EducationLevel = GetEducationLevelOptions();
                    ListOption studyLevelId = editMajorsViewModel.EducationLevel.Find(m => m.Description == majorList.StudyLevel);
                    editMajorsViewModel.EducationLevelId = Convert.ToInt32(studyLevelId.Value);
                    editMajorsViewModel.Cve = majorList.Code;
                    editMajorsViewModel.MajorName = majorList.Name;
                    editMajorsViewModel.LegalBaseCatalog = legalBaseCatalog;
                    editMajorsViewModel.LegalBaseId = majorList.LegalBaseId;
                }
                return View("Edit", editMajorsViewModel);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EdArea, "MajorsController - Edit", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Majorses this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            try
            {
                List<MajorList> majorLists = new List<MajorList>();
                List<MajorsViewModel> majorsViewModels = new List<MajorsViewModel>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Majors");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    majorLists = JsonConvert.DeserializeObject<List<MajorList>>(content);
                    majorsViewModels = majorLists.ToViewModel();
                    return View(majorsViewModels);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError(Constants.EdArea, "MajorsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError(Constants.EdArea, "MajorsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EdArea, "MajorsController - Index", ex.Message);
                throw;
            }
        }

        #endregion Views

        #region Json

        /// <summary>
        /// Creates the major.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<ActionResult> Create(string model)
        {
            try
            {
                MajorList majorList = JsonConvert.DeserializeObject<MajorList>(model);
                if (Account.status == enumAccess.Authorized)
                {
                    if (majorList.Code.Length >= 6 && majorList.Code.Length <= 7)
                    {
                        HttpClient client = PowerCampusHttpClient.GetClient(Account);
                        string json = JsonConvert.SerializeObject(majorList);
                        StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Majors/Create", httpContent);
                        if (resp.StatusCode == HttpStatusCode.OK)
                        {
                            return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                        }
                        else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            LoggerHelper.LogWebError(Constants.EdArea, "MajorsController", $"{resp.RequestMessage} - {resp.ReasonPhrase}");
                            return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            LoggerHelper.LogWebError(Constants.EdArea, "MajorsController", $"{resp.RequestMessage} - {resp.ReasonPhrase}");
                            return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        LoggerHelper.LogWebError(Constants.EdArea, "MajorsController - Create", "Major Code Length invalid");
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
                LoggerHelper.LogWebError(Constants.EdArea, "MajorsController - Create", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes the major.
        /// </summary>
        /// <param name="majorId">The major identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(int majorId)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage response = await client.DeleteAsync(client.BaseAddress + "/api/Majors/Delete/" + majorId);
                if (Account.status == enumAccess.Authorized)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError(Constants.EdArea, "MajorsController", $"{response.RequestMessage} - {response.ReasonPhrase}");
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError(Constants.EdArea, "MajorsController", $"{response.RequestMessage} - {response.ReasonPhrase}");
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
                LoggerHelper.LogWebError(Constants.EdArea, "MajorsController - Delete", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the major.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<ActionResult> Update(string model)
        {
            try
            {
                MajorList majorList = JsonConvert.DeserializeObject<MajorList>(model);
                if (Account.status == enumAccess.Authorized)
                {
                    if (majorList.Code.Length >= 6 && majorList.Code.Length <= 7)
                    {
                        HttpClient client = PowerCampusHttpClient.GetClient(Account);
                        string json = JsonConvert.SerializeObject(majorList);
                        StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Majors/Update", httpContent);
                        if (resp.StatusCode == HttpStatusCode.OK)
                        {
                            return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                        }
                        else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            LoggerHelper.LogWebError(Constants.EdArea, "MajorsController", $"{resp.RequestMessage} - {resp.ReasonPhrase}");
                            return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            LoggerHelper.LogWebError(Constants.EdArea, "MajorsController", $"{resp.RequestMessage} - {resp.ReasonPhrase}");
                            return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        LoggerHelper.LogWebError(Constants.EdArea, "MajorsController - Update", "Major Code Length invalid");
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
                LoggerHelper.LogWebError(Constants.EdArea, "MajorsController - Update", ex.Message);
                throw;
            }
        }

        #endregion Json

        /// <summary>
        /// Gets the edit signer.
        /// </summary>
        /// <param name="majorId">The major identifier.</param>
        /// <returns></returns>
        public EmptyResult Set(int majorId)
        {
            if (Account != null)
                Account.MajorId = majorId;

            return null;
        }

        /// <summary>
        /// Validates the code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public async Task<ActionResult> ValidateCode(string code)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Majors/ValidateCode/" + code);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    bool exists = JsonConvert.DeserializeObject<bool>(content);
                    return Json(new { exists, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                LoggerHelper.LogWebError(Constants.EdArea, "MajorsController", $"{resp.RequestMessage} - {resp.ReasonPhrase}");
                return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EdArea, "MajorsController - ValidateCode", ex.Message);
                throw;
            }
        }

        #region Private Methods

        /// <summary>
        /// Gets the education level options.
        /// </summary>
        /// <returns></returns>
        private List<ListOption> GetEducationLevelOptions()
        {
            return new List<ListOption>() {
                        new ListOption(){ Value = "0", Description = Resources.ElectronicDegree.Major.lblBachilleratoTecnico },
                        new ListOption(){ Value = "1", Description = Resources.ElectronicDegree.Major.lblDoctorado },
                        new ListOption(){ Value = "2", Description = Resources.ElectronicDegree.Major.lblEspecialidad },
                        new ListOption(){ Value = "3", Description = Resources.ElectronicDegree.Major.lblLicenciatura },
                        new ListOption(){ Value = "4", Description = Resources.ElectronicDegree.Major.lblMaestria },
                        new ListOption(){ Value = "5", Description = Resources.ElectronicDegree.Major.lblProfesionalAsociado },
                        new ListOption(){ Value = "6", Description = Resources.ElectronicDegree.Major.lblProfesionalTecnico },
                        new ListOption(){ Value = "7", Description = Resources.ElectronicDegree.Major.lblProfesor },
                        new ListOption(){ Value = "8", Description = Resources.ElectronicDegree.Major.lblTecnico },
                        new ListOption(){ Value = "9", Description = Resources.ElectronicDegree.Major.lblTecnicoProfesional },
                        new ListOption(){ Value = "10", Description = Resources.ElectronicDegree.Major.lblTecnicoSuperior },
                        new ListOption(){ Value = "11", Description = Resources.ElectronicDegree.Major.lblTecnicoSuperiorUniv },
                        new ListOption(){ Value = "12", Description = Resources.ElectronicDegree.Major.lblTecnologico }
                    };
        }

        #endregion Private Methods
    }
}