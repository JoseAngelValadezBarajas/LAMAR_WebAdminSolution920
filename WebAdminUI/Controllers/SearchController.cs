// --------------------------------------------------------------------
// <copyright file="SearchController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
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
using WebAdminUI.Models.SearchResult;
using WebAdminUI.SearchMappers;

namespace WebAdminUI.Controllers
{
    /// <summary>
    /// SearchController
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
    public class SearchController : BaseController
    {
        /// <summary>
        /// Advanceds this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Advanced()
        {
            SearchResult LstSearchResult = new SearchResult();
            SearchResultViewModel LstSearchResultViewModel;

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Catalogs?name=RecordType");
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                //resp.EnsureSuccessStatusCode();
                var LstSearchRecordType = await resp.Content.ReadAsAsync<List<RecordType>>();
                LstSearchResult.RecordTypeListFull = new List<RecordType>();

                foreach (var recordType in LstSearchRecordType)
                {
                    LstSearchResult.RecordTypeListFull.Add(recordType);
                }

                LstSearchResultViewModel = LstSearchResult.ToViewModelWithRecordType();

                return View("Advanced", LstSearchResultViewModel);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - SearchController", resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Gets the organization.
        /// </summary>
        /// <param name="ModelOrganization">The model organization.</param>
        /// <returns></returns>
        public async Task<ActionResult> GetOrganization(string ModelOrganization)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);

            Organization organization = JsonConvert.DeserializeObject<Organization>(ModelOrganization);

            string json = JsonConvert.SerializeObject(organization);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/search/organization/", httpContent);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                List<SearchResult> LstSearchOrganization = await resp.Content.ReadAsAsync<List<SearchResult>>();

                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - SearchController", resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Resultses the specified keyword.
        /// </summary>
        /// <param name="Keyword">The keyword.</param>
        /// <returns></returns>
        public async Task<ActionResult> Results(string Keyword)
        {
            List<SearchResult> LstSearchResult;
            List<SearchResultViewModel> LstSearchResultViewModel = new List<SearchResultViewModel>();

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Search/Get/?keyword=" + Keyword);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                //resp.EnsureSuccessStatusCode();
                LstSearchResult = await resp.Content.ReadAsAsync<List<SearchResult>>();

                foreach (var searchResult in LstSearchResult)
                {
                    LstSearchResultViewModel.Add(searchResult.ToViewModel());
                }

                return Json(LstSearchResultViewModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - SearchController", resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }
    }
}