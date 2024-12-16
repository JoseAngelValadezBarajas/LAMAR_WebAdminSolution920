// --------------------------------------------------------------------
// <copyright file="InstitutionCampusesController.cs" company="Ellucian">
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
using WebAdminUI.Areas.ElectronicCertificate.Models.InstitutionCampus;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicCertificate.Controllers
{
    /// <summary>
    /// Institution Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicCertificate)]
    [PermissionRequired(PermissionName = PermissionsConstants.ElectronicCertificateSetup)]
    public class InstitutionCampusesController : WebAdminUI.Controllers.BaseController
    {
        /// <summary>
        /// Gets the power campus institutions.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    List<InstitutionCampusViewModel> institutionCampuses = null;
                    List<ResponsibleNameViewModel> responsibles = null;
                    InstitutionCampusesViewModel institutionCampus = new InstitutionCampusesViewModel();
                    InstitutionCampuses institutionCampusesDto = null;
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + ApiRoute.EcInstitutionCampuses);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await resp.Content.ReadAsStringAsync();
                        institutionCampusesDto = JsonConvert.DeserializeObject<InstitutionCampuses>(content);
                        institutionCampuses = institutionCampusesDto.InstitutionCampus.ToViewModel();
                        HttpResponseMessage respResponsible = await client.GetAsync(client.BaseAddress + ApiRoute.EcResponsible);
                        if (respResponsible.StatusCode == HttpStatusCode.OK)
                        {
                            string contentResponsible = await respResponsible.Content.ReadAsStringAsync();
                            responsibles = JsonConvert.DeserializeObject<List<ResponsibleName>>(contentResponsible).ToViewModel();
                            institutionCampus.InstitutionCampuses = institutionCampuses;
                            institutionCampus.Responsibles = responsibles;
                            institutionCampus.IssuingPlace = institutionCampusesDto.IssuingPlace.ToViewModel();
                        }
                        return View(institutionCampus);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "InstitutionCampusesController", resp.StatusCode.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "InstitutionCampusesController", resp.ReasonPhrase);
                        return RedirectToRoute("ErrorUnauthorized");
                    }
                    else
                    {
                        return RedirectToRoute("ErrorException");
                    }
                }
                else
                {
                    return Json(new { id = -1, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "InstitutionCampusesController", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Saves the specified institution campuses.
        /// </summary>
        /// <param name="institutionCampuses">The institution campuses.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Save(List<InstitutionCampus> institutionCampuses)
        {
            try
            {
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    InstitutionCampuses institution = new InstitutionCampuses()
                    {
                        InstitutionCampus = institutionCampuses,
                        OperatorId = Account.UserName
                    };
                    string json = JsonConvert.SerializeObject(institution);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + ApiRoute.EcSaveInstitutionCampuses, httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "InstitutionCampusesController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "InstitutionCampusesController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicCertificate", "InstitutionCampusesController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicCertificate", "InstitutionController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicDegree", "InstitutionCampusesController - Create", ex.Message);
                throw;
            }
        }
    }
}