// --------------------------------------------------------------------
// <copyright file="OperatorsController.cs" company="Ellucian">
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
using WebAdminUI.Areas.ElectronicDegree.Models.Operators;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicDegree.Controllers
{
    /// <summary>
    /// Operators Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class OperatorsController : WebAdminUI.Controllers.BaseController
    {
        #region Views

        /// <summary>
        /// Adds the operators.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Add()
        {
            try
            {
                List<InstitutionOptions> institionOptions = new List<InstitutionOptions>();
                AddOperatorsViewModel addOperatorsViewModel = new AddOperatorsViewModel();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Operators/Institutions");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    institionOptions = JsonConvert.DeserializeObject<List<InstitutionOptions>>(content);
                    addOperatorsViewModel.Institutions = institionOptions;
                    return View(addOperatorsViewModel);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController - Operators", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Edits the specified operator identifier.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Edit()
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    List<InstitutionSignerList> institutionSignerLists = new List<InstitutionSignerList>();
                    List<InstitutionOptions> institutionOptions = new List<InstitutionOptions>();
                    List<OperatorsList> operatorsLists = new List<OperatorsList>();
                    List<OperatorsList> permissionsLists = new List<OperatorsList>();
                    AddOperatorsViewModel addOperatorsViewModel = new AddOperatorsViewModel();
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage respInstitutions = await client.GetAsync(client.BaseAddress + "/api/Operators/Institutions");
                    HttpResponseMessage respOperators = await client.GetAsync(client.BaseAddress + "/api/Operators/Edit/" + Account.OperatorId);
                    HttpResponseMessage respPermissions = await client.GetAsync(client.BaseAddress + "/api/Permissions/" + Account.OperatorId);
                    if (respInstitutions.StatusCode == HttpStatusCode.OK && respOperators.StatusCode == HttpStatusCode.OK && respPermissions.StatusCode == HttpStatusCode.OK)
                    {
                        string institutionContent = await respInstitutions.Content.ReadAsStringAsync();
                        string operatorContent = await respOperators.Content.ReadAsStringAsync();
                        string permissionContent = await respPermissions.Content.ReadAsStringAsync();
                        institutionOptions = JsonConvert.DeserializeObject<List<InstitutionOptions>>(institutionContent);
                        operatorsLists = JsonConvert.DeserializeObject<List<OperatorsList>>(operatorContent);
                        permissionsLists = JsonConvert.DeserializeObject<List<OperatorsList>>(permissionContent);
                        if (operatorsLists.Count > 0)
                        {
                            foreach (OperatorsList operators in operatorsLists)
                            {
                                List<InstitutionSignerList> institutionSigners = new List<InstitutionSignerList>();
                                HttpResponseMessage respInstSigners = await client.GetAsync(client.BaseAddress + "/api/Operators/InstitutionSigners/" + operators.ElectronicDegreeInstitutionId);
                                if (respInstSigners.StatusCode == HttpStatusCode.OK)
                                {
                                    string content = await respInstSigners.Content.ReadAsStringAsync();
                                    institutionSigners = JsonConvert.DeserializeObject<List<InstitutionSignerList>>(content);
                                    institutionSignerLists.Add(new InstitutionSignerList
                                    {
                                        EdInstitutionId = operators.ElectronicDegreeInstitutionId,
                                        InstitutionSigners = institutionSigners
                                    });
                                }
                                else
                                {
                                    LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", respInstSigners.StatusCode.ToString());
                                    LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", respInstSigners.ReasonPhrase);
                                }
                            }

                            addOperatorsViewModel.InstitutionSignerLists = institutionSignerLists;
                        }
                        if (permissionsLists.Count > 0)
                        {
                            OperatorsList permissions = new OperatorsList();
                            addOperatorsViewModel.PermissionList = new List<PermissionsList>();
                            foreach (OperatorsList permissionList in permissionsLists)
                            {
                                HttpResponseMessage respPermissionsList = await client.GetAsync(client.BaseAddress + "/api/Permissions/List/" + permissionList.GrantedOperatorsId);
                                if (respPermissionsList.StatusCode == HttpStatusCode.OK)
                                {
                                    string content = await respPermissionsList.Content.ReadAsStringAsync();
                                    permissions = JsonConvert.DeserializeObject<OperatorsList>(content);
                                    addOperatorsViewModel.PermissionList.Add(new PermissionsList()
                                    {
                                        Institutions = permissions.Institutions,
                                        Name = permissions.Name,
                                        GrantedOperatorId = permissions.OperatorId,
                                        PeopleCodeId = permissions.PeopleCodeId
                                    });
                                }
                                else
                                {
                                    LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", respPermissionsList.StatusCode.ToString());
                                    LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", respPermissionsList.ReasonPhrase);
                                }
                            }
                        }
                        addOperatorsViewModel.OperatorId = Account.OperatorId;
                        addOperatorsViewModel.Name = Account.OperatorName;
                        addOperatorsViewModel.NumberId = Account.PeopleOrgCodeId;
                        addOperatorsViewModel.Institutions = institutionOptions;
                        return View(addOperatorsViewModel);
                    }
                    LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", respInstitutions.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", respInstitutions.ReasonPhrase);

                    return RedirectToAction("Index", "HomeED");
                }

                return RedirectToAction("Error", "HomeED");
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController - Edit", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Operatorses this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            try
            {
                List<OperatorsList> operators = new List<OperatorsList>();
                List<OperatorsViewModel> operatorsViewModel = new List<OperatorsViewModel>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Operators");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    operators = JsonConvert.DeserializeObject<List<OperatorsList>>(content);
                    operatorsViewModel = operators.ToViewModel();
                    return View(operatorsViewModel);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.ReasonPhrase);
                    return RedirectToAction("Index", "HomeED");
                }
                else
                {
                    return RedirectToAction("Error", "HomeED");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController - Operators", ex.Message);
                throw;
            }
        }

        #endregion Views

        #region Json

        /// <summary>
        /// Creates the operators.
        /// </summary>
        /// <param name="operators"></param>
        /// <returns></returns>
        public async Task<ActionResult> CreateOperators(OperatorsList operators)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    string json = JsonConvert.SerializeObject(operators);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Operators/Create", httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController - CreateOperators", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Creates the permissions.
        /// </summary>
        /// <param name="operators">The operators.</param>
        /// <returns></returns>
        public async Task<ActionResult> CreatePermissions(OperatorsList operators)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    string json = JsonConvert.SerializeObject(operators);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Permissions/Create", httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController - CreatePermissions", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes the specified operator identifier on Operators and Permissions tables.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(string operatorId)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage response = await client.DeleteAsync(client.BaseAddress + "/api/Operators/Delete/" + operatorId);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", response.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", response.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", response.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", response.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController - Delete", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes the institution assigned to the Operator.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteInstitution(int institutionId, string operatorId)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage response = await client.DeleteAsync(client.BaseAddress + "/api/Operators/Institutions/Delete/" + institutionId + "/" + operatorId);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", response.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", response.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", response.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", response.ReasonPhrase);
                        return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController - DeleteInstitution", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes the permission assigned to the Operator.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="grantedOperatorId">The granted operator identifier.</param>
        public async Task<ActionResult> DeletePermission(string operatorId, string grantedOperatorId)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage response = await client.DeleteAsync(client.BaseAddress + "/api/Permissions/Delete/" + operatorId + "/" + grantedOperatorId);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", response.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", response.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", response.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", response.ReasonPhrase);
                        return Json(new { id = -1, message = CustomMessages.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController - DeleteInstitution", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Institutions the signers.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> InstitutionSigners(int institutionId)
        {
            try
            {
                List<InstitutionSignerList> signersList = new List<InstitutionSignerList>();
                AddOperatorsViewModel singerInstitutionViewModel = new AddOperatorsViewModel();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Operators/InstitutionSigners/" + institutionId);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    signersList = JsonConvert.DeserializeObject<List<InstitutionSignerList>>(content);
                    singerInstitutionViewModel = signersList.ToViewModel();
                    return Json(new { singerInstitutionViewModel, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController - InstitutionSigners", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Operators the specified operator identifier.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> Operator(string operatorId)
        {
            try
            {
                List<OperatorsList> operatorsList = new List<OperatorsList>();
                AddOperatorsViewModel operatorViewModel = new AddOperatorsViewModel();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Operator/" + operatorId);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    operatorsList = JsonConvert.DeserializeObject<List<OperatorsList>>(content);
                    return Json(new { operatorsList, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "OperatorsController - Operator", ex.Message);
                throw;
            }
        }

        #endregion Json

        /// <summary>
        /// Sets the specified operator identifier.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="operatorName">Name of the operator.</param>
        /// <param name="peopleCodeId">The people code identifier.</param>
        /// <returns></returns>
        public EmptyResult Set(string operatorId, string operatorName, string peopleCodeId)
        {
            Account.OperatorId = operatorId;
            Account.OperatorName = operatorName;
            Account.PeopleOrgCodeId = peopleCodeId;
            return null;
        }
    }
}