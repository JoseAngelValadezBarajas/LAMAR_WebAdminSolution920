// --------------------------------------------------------------------
// <copyright file="InstitutionsController.cs" company="Ellucian">
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
using WebAdminUI.Areas.ElectronicDegree.Models.Institution;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicDegree.Controllers
{
    /// <summary>
    /// Institution Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicDegree)]
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicDegreeSetup)]
    public class InstitutionsController : WebAdminUI.Controllers.BaseController
    {
        #region Views

        /// <summary>
        /// Adds the institution.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Add()
        {
            try
            {
                InstitutionModel institutionModel = new InstitutionModel();
                AddInstitutionViewModel addInstitutionViewModel = await institutionModel.ToViewModelAsync(Account);
                return View(addInstitutionViewModel);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController - AddInstitution", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Edits the specified institution identifier.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int institutionId)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    InstitutionDAModel institutionDAModel = new InstitutionDAModel();
                    AddInstitutionViewModel institutionViewModel = new AddInstitutionViewModel();
                    if (institutionId > 0)
                    {
                        HttpResponseMessage resp = await client.GetAsync(client.BaseAddress +
                            "/api/Institution/GetInstitution?institutionId=" + institutionId);
                        string content = await resp.Content.ReadAsStringAsync();
                        institutionDAModel = JsonConvert.DeserializeObject<InstitutionDAModel>(content);
                        AddInstitutionViewModel addInstitutionViewModel = await institutionDAModel.ToViewModelAsync(Account);
                        return View(addInstitutionViewModel);
                    }

                    return RedirectToAction("Error", "HomeED");
                }

                return Json(new { id = -1, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController - EditInstitution", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Edits the mapping.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> EditMapping(int institutionId)
        {
            try
            {
                EditInsMappingViewModel editInsMappingViewModel = new EditInsMappingViewModel();
                List<InstitutionModel> institutionList = new List<InstitutionModel>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Institution/Details");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    institutionList = JsonConvert.DeserializeObject<List<InstitutionModel>>(content);
                    List<AddInstitutionViewModel> institutionViewModelList = new List<AddInstitutionViewModel>();

                    foreach (InstitutionModel institutionModel in institutionList)
                        institutionViewModelList.Add(await institutionModel.ToViewModelAsync(Account, institutionId));

                    resp = await client.GetAsync(client.BaseAddress + "/api/Institution/GetMajorsByInstitutionId?institutionId=" + institutionId);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        List<MajorList> majorList = null;
                        string majors = await resp.Content.ReadAsStringAsync();
                        majorList = JsonConvert.DeserializeObject<List<MajorList>>(majors);

                        resp = await client.GetAsync(client.BaseAddress + "/api/Institution/" + institutionId);
                        if (resp.StatusCode == HttpStatusCode.OK)
                        {
                            InstitutionModel institutionModel = null;
                            string inst = await resp.Content.ReadAsStringAsync();
                            institutionModel = JsonConvert.DeserializeObject<InstitutionModel>(inst);
                            resp = await client.GetAsync(client.BaseAddress + "/api/Institution/GetRvoeOptions");
                            if (resp.StatusCode == HttpStatusCode.OK)
                            {
                                editInsMappingViewModel.InstitutionCode = institutionModel.Code;
                                editInsMappingViewModel.InstitututionName = institutionModel.Name;
                                editInsMappingViewModel.MajorsViewModelList = majorList.ToViewModel();
                                editInsMappingViewModel.Majors = institutionModel.Majors;
                                editInsMappingViewModel.SelectedInstitututionId = institutionId;
                                editInsMappingViewModel.AddInstitutionViewModelList = institutionViewModelList;
                            }

                            if (majorList.Count == 0)
                            {
                                List<MajorList> majorListCatalog = null;
                                resp = await client.GetAsync(client.BaseAddress + "/api/Majors");
                                if (resp.StatusCode == HttpStatusCode.OK)
                                {
                                    string majorsContent = await resp.Content.ReadAsStringAsync();
                                    majorListCatalog = JsonConvert.DeserializeObject<List<MajorList>>(majorsContent);
                                    AddInstitutionViewModel addInstViewModel = new AddInstitutionViewModel
                                    {
                                        Id = institutionId,
                                        MajorsViewModelList = majorListCatalog.ToViewModel()
                                    };
                                    editInsMappingViewModel.InstitutionViewModelList = addInstViewModel;
                                    editInsMappingViewModel.InstitutionViewModelList.Code = editInsMappingViewModel.InstitutionCode;
                                    editInsMappingViewModel.InstitutionViewModelList.Name = editInsMappingViewModel.InstitututionName;
                                    editInsMappingViewModel.InstitutionViewModelList.MajorsNumber = (int)editInsMappingViewModel.Majors;
                                }
                            }
                            return View(editInsMappingViewModel);
                        }
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.StatusCode.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
                        return RedirectToRoute("ErrorUnauthorized");
                    }
                    else
                    {
                        return RedirectToRoute("ErrorException");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", ex.Message);
                throw;
            }

            return null;
        }

        /// <summary>
        /// Institution
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            try
            {
                List<InstitutionViewModel> institutionList = new List<InstitutionViewModel>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Institution/Get");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    institutionList = JsonConvert.DeserializeObject<List<InstitutionViewModel>>(content);
                    return View(institutionList);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Majorses the mapping.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> MajorsMapping()
        {
            try
            {
                List<InstitutionViewModel> institutionList = new List<InstitutionViewModel>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Institution/Get");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    institutionList = JsonConvert.DeserializeObject<List<InstitutionViewModel>>(content);
                    return View(institutionList);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Majorses the mapping list.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> MajorsMappingList(int institutionId)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Institution/GetMajorsByInstitutionId?institutionId=" + institutionId);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    List<MajorList> majorList = null;
                    string content = await resp.Content.ReadAsStringAsync();
                    majorList = JsonConvert.DeserializeObject<List<MajorList>>(content);
                    resp = await client.GetAsync(client.BaseAddress + "/api/Institution/" + institutionId);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        InstitutionModel institutionModel = null;
                        string inst = await resp.Content.ReadAsStringAsync();
                        institutionModel = JsonConvert.DeserializeObject<InstitutionModel>(inst);
                        EditMajorInsMappingViewModel editMajorInsMappingViewModel = new EditMajorInsMappingViewModel();
                        resp = await client.GetAsync(client.BaseAddress + "/api/Institution/GetRvoeOptions");
                        if (resp.StatusCode == HttpStatusCode.OK)
                        {
                            List<DropDownListModel> dropDownLists = null;
                            string options = await resp.Content.ReadAsStringAsync();
                            dropDownLists = JsonConvert.DeserializeObject<List<DropDownListModel>>(options);
                            editMajorInsMappingViewModel.InstitutionCode = institutionModel.Code;
                            editMajorInsMappingViewModel.InstitututionName = institutionModel.Name;
                            editMajorInsMappingViewModel.MajorsViewModelList = majorList.ToViewModel(dropDownLists);
                            editMajorInsMappingViewModel.Majors = institutionModel.Majors;
                            editMajorInsMappingViewModel.Id = institutionId;

                            return View(editMajorInsMappingViewModel);
                        }
                    }
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", ex.Message);
                throw;
            }

            return null;
        }

        #endregion Views

        #region Actions

        /// <summary>
        /// Creates the institution.
        /// </summary>
        /// <param name="institutionDAModel">The institution da model.</param>
        /// <returns></returns>
        public async Task<ActionResult> Create(InstitutionDAModel institutionDAModel)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    if (institutionDAModel.FolioFormat == null)
                        institutionDAModel.FolioFormat = "#ElectronicDegreeInstitution.Folio#";
                    institutionDAModel.Folio = "0";
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    string json = JsonConvert.SerializeObject(institutionDAModel);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Institution/Create", httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController - Create", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes the mapping.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteMapping(int id)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage resp = await client.DeleteAsync(client.BaseAddress + "/api/Institution/Mapping/Delete/" + id);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController - RemoveMapping", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the power campus institutions.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetPowerCampusInstitutions()
        {
            try
            {
                List<DropDownListModel> options = new List<DropDownListModel>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Institution/GetPowerCampusInstitutions");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    options = JsonConvert.DeserializeObject<List<DropDownListModel>>(content);
                    return Json(new { options, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.RequestMessage.ToString());
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
                return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Maps the institution major rvoe.
        /// </summary>
        /// <param name="rvoeList">The rvoe list.</param>
        /// <returns></returns>
        public async Task<ActionResult> MapInstitutionMajorRvoe(List<InstitutionMajorRvoe> rvoeList)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    MapInstitutionMajorRvoe mapInstitutionMajorRvoe = new MapInstitutionMajorRvoe();
                    if (rvoeList != null)
                    {
                        mapInstitutionMajorRvoe.institutionMajorRvoe = rvoeList;
                        mapInstitutionMajorRvoe.UserName = Account.UserName;
                    }
                    string json = JsonConvert.SerializeObject(mapInstitutionMajorRvoe);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Institution/MapInstitutionMajorRvoe", httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController - Update", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Maps the major.
        /// </summary>
        /// <param name="majorMappingModel">The major mapping model.</param>
        /// <returns></returns>
        public async Task<ActionResult> MapMajor(MajorMappingModel majorMappingModel)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    if (majorMappingModel != null)
                        majorMappingModel.UserName = Account.UserName;
                    string json = JsonConvert.SerializeObject(majorMappingModel);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Institution/MapMajor", httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController - Update", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the specified institution da model.
        /// </summary>
        /// <param name="institutionDAModel">The institution da model.</param>
        /// <returns></returns>
        public async Task<ActionResult> Update(InstitutionDAModel institutionDAModel)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    if (institutionDAModel.FolioFormat == null)
                        institutionDAModel.FolioFormat = "#ElectronicDegreeInstitution.Folio#";
                    institutionDAModel.Folio = "0";
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    string json = JsonConvert.SerializeObject(institutionDAModel);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Institution/Update", httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController - Update", ex.Message);
                throw;
            }
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
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Institution/ValidateCode?Code=" + code);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    bool exists = JsonConvert.DeserializeObject<bool>(content);
                    return Json(new { exists, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.RequestMessage.ToString());
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
                return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController - ValidateCode", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the folio.
        /// </summary>
        /// <param name="folio">The folio.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public async Task<ActionResult> ValidateFolio(string folio, string code)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Institution/ValidateFolio?Folio=" + folio + "&Code=" + code);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    bool exists = JsonConvert.DeserializeObject<bool>(content);
                    return Json(new { exists, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.RequestMessage.ToString());
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
                return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController - ValidateFolio", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task<ActionResult> ValidateName(string name)
        {
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Institution/ValidateName?Name=" + name);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    bool exists = JsonConvert.DeserializeObject<bool>(content);
                    return Json(new { exists, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.RequestMessage.ToString());
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController", resp.ReasonPhrase);
                return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionController - ValidateName", ex.Message);
                throw;
            }
        }

        #endregion Actions
    }
}