// --------------------------------------------------------------------
// <copyright file="OrganizationController.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;
using WebAdminUI.Models.Organization;
using WebAdminUI.Models.SearchResult;
using WebAdminUI.OrganizationMappers;
using WebAdminUI.SearchMappers;

namespace WebAdminUI.Controllers
{
    /// <summary>
    /// Organization Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordSetup)]
    public class OrganizationController : BaseController
    {
        /// <summary>
        /// Gets the charges.
        /// </summary>
        /// <param name="YearTermSession">The yts.</param>
        /// <returns></returns>
        public async Task<PartialViewResult> Charges(string YearTermSession)
        {
            OrganizationViewModel organizationViewModel;
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            PeopleOrgCharges peopleCharges = new PeopleOrgCharges
            {
                CodeId = "O" + Account.SearchPeopleOrgCodeId,
                YTS = YearTermSession
            };

            string json = JsonConvert.SerializeObject(peopleCharges);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/organization/Charges/list", httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Organization organizationModel = await resp.Content.ReadAsAsync<Organization>();
                organizationViewModel = organizationModel.ToViewModelWithCharges(Account);
                return PartialView("YTSCharges", organizationViewModel);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - OrganizationController", resp.ReasonPhrase);
                return PartialView("Error", "Home");
            }
        }

        /// <summary>
        /// Lists the specified model organization.
        /// </summary>
        /// <param name="modelOrganization">The model organization.</param>
        /// <returns></returns>
        public async Task<ActionResult> List(string modelOrganization)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);

            Organization organization = JsonConvert.DeserializeObject<Organization>(modelOrganization);
            SearchResultViewModel searchResultViewModel;
            List<SearchResultViewModel> organizationresultLst = new List<SearchResultViewModel>();

            string json = JsonConvert.SerializeObject(organization);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/search/organization/", httpContent);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                List<SearchResult> LstSearchOrganization = await resp.Content.ReadAsAsync<List<SearchResult>>();

                if (!string.IsNullOrEmpty(LstSearchOrganization[0].PeopleOrgCodeId))
                {
                    foreach (var searchPeople in LstSearchOrganization)
                    {
                        searchResultViewModel = searchPeople.ToViewModelAdvanced();
                        organizationresultLst.Add(searchResultViewModel);
                    }

                    return PartialView("ListView", organizationresultLst);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - OrganizationController", resp.ReasonPhrase);
                return PartialView("ListView", organizationresultLst);
            }
        }

        /// <summary>
        /// Menus the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public EmptyResult Menu(string id)
        {
            if (Account != null)
            {
                Account.SearchPeopleOrgCodeId = id;
                Account.PeopleOrgCodeId = "O" + id;
            }
            return null;
        }

        /// <summary>
        /// Saves the taxpayer identifier.
        /// </summary>
        /// <param name="organizationViewModel">The organization view model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> SaveTaxpayerId(string organizationViewModel)
        {
            OrganizationViewModel orgViewModel = JsonConvert.DeserializeObject<OrganizationViewModel>(organizationViewModel);
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            string json = JsonConvert.SerializeObject(orgViewModel.ToDataEntity());
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "api/organization/Save/Taxpayerid", httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                int organizationId = await resp.Content.ReadAsAsync<int>();
                return organizationId;
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - OrganizationController", resp.ReasonPhrase);
                return 0;
            }
        }

        /// <summary>
        /// Taxpayers the identifier.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> TaxpayerId()
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Organization/" + "O" + Account.SearchPeopleOrgCodeId);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Organization organization = await resp.Content.ReadAsAsync<Organization>();
                OrganizationViewModel organizationViewModel = new OrganizationViewModel();
                organizationViewModel = organization.ToViewModel(Account);
                return View("Organization", organizationViewModel);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - OrganizationController", resp.ReasonPhrase);
                return RedirectToRoute("ErrorExpired");
            }
        }

        /// <summary>
        /// Years the term session.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> YearTermSession()
        {
            OrganizationViewModel organizationViewModel;

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/organization/yts/?OrganizationId=" + "O" + Account.SearchPeopleOrgCodeId);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Organization organizationModel = await resp.Content.ReadAsAsync<Organization>();
                organizationViewModel = organizationModel.ToViewModelYearTermSession();
                return Json(organizationViewModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - OrganizationController", resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }
    }
}