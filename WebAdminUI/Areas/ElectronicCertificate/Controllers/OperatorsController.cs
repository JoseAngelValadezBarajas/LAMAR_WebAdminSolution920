// --------------------------------------------------------------------
// <copyright file="OperatorsController.cs" company="Ellucian">
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
using WebAdminUI.Areas.ElectronicCertificate.Mappers;
using WebAdminUI.Areas.ElectronicCertificate.Models.Operators;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicCertificate.Controllers
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
                List<InstitutionCampusOperator> institionOptions = new List<InstitutionCampusOperator>();
                AddOperatorsViewModel addOperatorsViewModel = new AddOperatorsViewModel();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + ApiRoute.EcOperatorsInstitutions);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    institionOptions = JsonConvert.DeserializeObject<List<InstitutionCampusOperator>>(content);
                    addOperatorsViewModel.InstitutionOptions = institionOptions;
                    return View(addOperatorsViewModel);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.ReasonPhrase);
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
                    List<InstitutionCampusOperator> institutionOptions = new List<InstitutionCampusOperator>();
                    List<OperatorsList> operatorsLists = new List<OperatorsList>();
                    List<OperatorsList> permissionsLists = new List<OperatorsList>();
                    AddOperatorsViewModel addOperatorsViewModel = new AddOperatorsViewModel();
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage respInstitutions = await client.GetAsync(client.BaseAddress + ApiRoute.EcOperatorsInstitutions);
                    HttpResponseMessage respOperators = await client.GetAsync(client.BaseAddress + ApiRoute.EcOperatorsEdit + Account.OperatorId);
                    HttpResponseMessage respPermissions = await client.GetAsync(client.BaseAddress + ApiRoute.ECPermissionsEdit + Account.OperatorId);
                    if (respInstitutions.StatusCode == HttpStatusCode.OK && respOperators.StatusCode == HttpStatusCode.OK && respPermissions.StatusCode == HttpStatusCode.OK)
                    {
                        string institutionContent = await respInstitutions.Content.ReadAsStringAsync();
                        string operatorContent = await respOperators.Content.ReadAsStringAsync();
                        string permissionContent = await respPermissions.Content.ReadAsStringAsync();
                        institutionOptions = JsonConvert.DeserializeObject<List<InstitutionCampusOperator>>(institutionContent);
                        operatorsLists = JsonConvert.DeserializeObject<List<OperatorsList>>(operatorContent);
                        permissionsLists = JsonConvert.DeserializeObject<List<OperatorsList>>(permissionContent);
                        if (operatorsLists.Count > 0)
                        {
                            OperatorsList operators = new OperatorsList();
                            addOperatorsViewModel.OperatorList = new List<OperatorsList>();
                            foreach (OperatorsList operatorList in operatorsLists)
                            {
                                addOperatorsViewModel.OperatorList.Add(new OperatorsList()
                                {
                                    CampusId = operatorList.CampusId,
                                    OperatorId = operatorList.OperatorId,
                                    PeopleCodeId = operatorList.PeopleCodeId,
                                    Name = operatorList.Name
                                });
                            }
                        }
                        if (permissionsLists.Count > 0)
                        {
                            OperatorsList permissions = new OperatorsList();
                            addOperatorsViewModel.PermissionList = new List<PermissionsList>();
                            foreach (OperatorsList permissionList in permissionsLists)
                            {
                                HttpResponseMessage respPermissionsList = await client.GetAsync(client.BaseAddress + ApiRoute.EcPermissionsList + permissionList.GrantedOperatorsId);
                                if (respPermissionsList.StatusCode == HttpStatusCode.OK)
                                {
                                    string content = await respPermissionsList.Content.ReadAsStringAsync();
                                    permissions = JsonConvert.DeserializeObject<OperatorsList>(content);
                                    addOperatorsViewModel.PermissionList.Add(new PermissionsList()
                                    {
                                        Campuses = permissions.Institutions,
                                        Name = permissions.Name,
                                        GrantedOperatorId = permissions.OperatorId,
                                        PeopleCodeId = permissions.PeopleCodeId
                                    });
                                }
                                else
                                {
                                    LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", respPermissionsList.StatusCode.ToString());
                                    LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", respPermissionsList.ReasonPhrase);
                                }
                            }
                        }
                        addOperatorsViewModel.OperatorId = Account.OperatorId;
                        addOperatorsViewModel.Name = Account.OperatorName;
                        addOperatorsViewModel.NumberId = Account.PeopleOrgCodeId;
                        addOperatorsViewModel.InstitutionOptions = institutionOptions;
                        return View(addOperatorsViewModel);
                    }
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", respInstitutions.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", respInstitutions.ReasonPhrase);

                    return RedirectToRoute("ErrorUnauthorized");
                }

                return RedirectToRoute("ErrorException");
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController - Edit", ex.Message);
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
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + ApiRoute.EcOperatorsOperator);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    operators = JsonConvert.DeserializeObject<List<OperatorsList>>(content);
                    operatorsViewModel = operators.ToViewModel();
                    return View(operatorsViewModel);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController - Operators", ex.Message);
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
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + ApiRoute.EcOperatorsCreate, httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController - CreateOperators", ex.Message);
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
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + ApiRoute.EcOperatorsPermissions, httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController - CreatePermissions", ex.Message);
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
                    HttpResponseMessage response = await client.DeleteAsync(client.BaseAddress + ApiRoute.EcOperatorsDelete + operatorId);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", response.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", response.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", response.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", response.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController - Delete", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes the institution assigned to the Operator.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteInstitution(string institutionId, string operatorId)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage response = await client.DeleteAsync(client.BaseAddress + ApiRoute.EcOperatorsInstitutionsDelete + institutionId + "/" + operatorId);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", response.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", response.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", response.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", response.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController - DeleteInstitution", ex.Message);
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
                    HttpResponseMessage response = await client.DeleteAsync(client.BaseAddress + ApiRoute.EcDeletePermissions + operatorId + "/" + grantedOperatorId);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", response.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", response.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", response.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", response.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController - DeleteInstitution", ex.Message);
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
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + ApiRoute.EcSearchOperator + operatorId);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    operatorsList = JsonConvert.DeserializeObject<List<OperatorsList>>(content);
                    return Json(new { operatorsList, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController - Operator", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Operators the perm.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> OperatorPerm(string operatorId)
        {
            try
            {
                List<OperatorsList> operatorsList = new List<OperatorsList>();
                AddOperatorsViewModel operatorViewModel = new AddOperatorsViewModel();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + ApiRoute.EcSearchOperatorPerm + operatorId);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    string content = await resp.Content.ReadAsStringAsync();
                    operatorsList = JsonConvert.DeserializeObject<List<OperatorsList>>(content);
                    return Json(new { operatorsList, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController", resp.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "OperatorsController - Operator", ex.Message);
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