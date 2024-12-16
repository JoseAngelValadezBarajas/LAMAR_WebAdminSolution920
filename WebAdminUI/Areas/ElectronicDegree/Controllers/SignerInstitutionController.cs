// --------------------------------------------------------------------
// <copyright file="SignerInstitutionController.cs" company="Ellucian">
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
using WebAdminUI.Areas.ElectronicDegree.Models.SignerInstitution;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicDegree.Controllers
{
    /// <summary>
    /// Signer-Institution Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class SignerInstitutionController : WebAdminUI.Controllers.BaseController
    {
        /// <summary>
        /// Creates the signer institution.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<ActionResult> Create(string model)
        {
            try
            {
                InstitutionList signerInstitution = JsonConvert.DeserializeObject<InstitutionList>(model);
                if (Account.status == enumAccess.Authorized)
                {
                    HttpClient client = PowerCampusHttpClient.GetClient(Account);
                    string json = JsonConvert.SerializeObject(signerInstitution);
                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/SignerInstitution/Create", httpContent);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { id = 1, message = CustomMessages.Success }, JsonRequestBehavior.AllowGet);
                    }
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "SignerInstitutionController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "SignerInstitutionController", resp.ReasonPhrase);
                        return Json(new { id = 0, message = CustomMessages.SessionExpired }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerHelper.LogWebError("ElectronicDegree", "SignerInstitutionController", resp.RequestMessage.ToString());
                        LoggerHelper.LogWebError("ElectronicDegree", "SignerInstitutionController", resp.ReasonPhrase);
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
                LoggerHelper.LogWebError("ElectronicDegree", "SignerInstitutionController - Create", ex.Message);
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
                List<SignerList> signerLists = new List<SignerList>();
                List<InstitutionList> signerInstitutionLists = new List<InstitutionList>();
                List<SignerInstitutionViewModel> signerInstitutionViewModels = new List<SignerInstitutionViewModel>();
                List<InstitutionSignerList> institutionSignerList = new List<InstitutionSignerList>();
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                HttpResponseMessage respInstitutions = await client.GetAsync(client.BaseAddress + "/api/Institutions");
                HttpResponseMessage respSigners = await client.GetAsync(client.BaseAddress + "/api/Signers");
                HttpResponseMessage respInstitutionSigners = await client.GetAsync(client.BaseAddress + "/api/InstitutionSigners");
                if (respSigners.StatusCode == HttpStatusCode.OK && respInstitutions.StatusCode == HttpStatusCode.OK)
                {
                    string signers = await respSigners.Content.ReadAsStringAsync();
                    string institutions = await respInstitutions.Content.ReadAsStringAsync();
                    string institutionSigner = await respInstitutionSigners.Content.ReadAsStringAsync();
                    signerInstitutionLists = JsonConvert.DeserializeObject<List<InstitutionList>>(institutions);
                    signerLists = JsonConvert.DeserializeObject<List<SignerList>>(signers);
                    institutionSignerList = JsonConvert.DeserializeObject<List<InstitutionSignerList>>(institutionSigner);
                    signerInstitutionViewModels = signerInstitutionLists.ToViewModel(signerLists, institutionSignerList);
                    return View(signerInstitutionViewModels);
                }
                else if (respInstitutions.StatusCode == HttpStatusCode.Unauthorized || respSigners.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("ElectronicDegree", "SignerInstitutionController", respSigners.StatusCode.ToString());
                    LoggerHelper.LogWebError("ElectronicDegree", "SignerInstitutionController", respSigners.ReasonPhrase);
                    return RedirectToRoute("ErrorUnauthorized");
                }
                else
                {
                    return RedirectToRoute("ErrorException");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "SignerInstitutionController - Index", ex.Message);
                throw;
            }
        }
    }
}