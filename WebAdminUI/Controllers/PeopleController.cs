// --------------------------------------------------------------------
// <copyright file="PeopleController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;
using WebAdminUI.Models.People;
using WebAdminUI.Models.SearchResult;
using WebAdminUI.PeopleMappers;
using WebAdminUI.SearchMappers;
using WebAdminUI.Models.AcademicInfoViewModel;

namespace WebAdminUI.Controllers
{
    /// <summary>
    /// PeopleController class
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
    public class PeopleController : BaseController
    {
        /// <summary>
        /// Gets the charges.
        /// </summary>
        /// <param name="YearTermSession">The yts.</param>
        /// <returns></returns>
        public async Task<PartialViewResult> Charges(string YearTermSession)
        {
            PeopleViewModel peopleViewModel = new PeopleViewModel();
            try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                PeopleOrgCharges peopleCharges = new PeopleOrgCharges
                {
                    CodeId = "P" + Account.SearchPeopleOrgCodeId,
                    YTS = YearTermSession
                };

                string json = JsonConvert.SerializeObject(peopleCharges);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/People/Charges/list", httpContent);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    People peopleModel = await resp.Content.ReadAsAsync<People>();
                    peopleViewModel = peopleModel.ToViewModelWithCharges(Account);
                    return PartialView("YTSCharges", peopleViewModel);
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - PeopleController", resp.ReasonPhrase);
                    return PartialView("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - PeopleController", ex.Message + " " + ex.StackTrace);
                return PartialView("YTSCharges", peopleViewModel);
            }
        }

        /// <summary>
        /// Gets the people.
        /// </summary>
        /// <param name="ModelPeople">The modelpeople.</param>
        /// <returns></returns>
        public async Task<PartialViewResult> List(string ModelPeople)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);

            People people = JsonConvert.DeserializeObject<People>(ModelPeople);
            SearchResultViewModel searchResultViewModel;
            List<SearchResultViewModel> peopleresultLst = new List<SearchResultViewModel>();

            string json = JsonConvert.SerializeObject(people);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/search/people/", httpContent);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                List<SearchResult> LstSearchPeople = await resp.Content.ReadAsAsync<List<SearchResult>>();

                if (!string.IsNullOrEmpty(LstSearchPeople[0].PeopleOrgCodeId))
                {
                    foreach (var searchPeople in LstSearchPeople)
                    {
                        searchResultViewModel = searchPeople.ToViewModelAdvanced();
                        peopleresultLst.Add(searchResultViewModel);
                    }

                    return PartialView("ListView", peopleresultLst);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - PeopleController", resp.ReasonPhrase);
                return PartialView("Error", "Home");
            }
        }

        /// <summary>
        /// Menus this instance.
        /// This is only used to assign the People id which is coming from Search
        /// </summary>
        /// <returns></returns>
        public EmptyResult Menu(string id)
        {
            Account.SearchPeopleOrgCodeId = id;
            Account.PeopleOrgCodeId = "P" + id;
            return null;
        }

        /// <summary>
        /// Saves the taxpayer identifier.
        /// </summary>
        /// <param name="peopleModelView">The people.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> SaveTaxpayerId(string peopleModelView)
        {
            PeopleViewModel peopleViewModel = JsonConvert.DeserializeObject<PeopleViewModel>(peopleModelView);
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            string json = JsonConvert.SerializeObject(peopleViewModel.ToDataEntity(Account));
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/people/Save/TaxpayerId", httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                int peopleModel = await resp.Content.ReadAsAsync<int>();
                return peopleModel;
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - PeopleController", resp.ReasonPhrase);
                return 0;
            }
        }

        /// <summary>
        /// Taxpayers the identifier.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> TaxpayerId()
        {
            PeopleViewModel peopleViewModel;

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/people/get/P" + Account.SearchPeopleOrgCodeId);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                People peopleModel = await resp.Content.ReadAsAsync<People>();
                peopleViewModel = peopleModel.ToViewModel(Account);
                return View("People", peopleViewModel);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - PeopleController", resp.ReasonPhrase);
                return RedirectToRoute("ErrorExpired");
            }
        }

        /// <summary>
        /// Years the term session.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> YearTermSession()
        {
            PeopleViewModel peopleViewModel;

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/people/yts/P" + Account.SearchPeopleOrgCodeId);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                People peopleModel = await resp.Content.ReadAsAsync<People>();
                peopleViewModel = peopleModel.ToViewModelYearTermSession();
                return Json(peopleViewModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - PeopleController", resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }


        /// <summary>
        /// Return Year Info Session.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AcademicYears(string id)
        {
             try
            {
                HttpClient client = PowerCampusHttpClient.GetClient(Account);
                var urltest = client.BaseAddress + "api/people/yts/getgraduatedates/" + id;
                LoggerHelper.LogWebError("AcademicYearsG", "PowerCampus.DataAccess - PeopleController", urltest);
                HttpResponseMessage resp2 = await client.GetAsync(client.BaseAddress + "api/people/yts/getgraduatedates/" + id);
                if (resp2.IsSuccessStatusCode)
                {
                    AcademicInfoViewModelDegree AcademicInfoViewModelDegree = new AcademicInfoViewModelDegree();
                    string content2 = await resp2.Content.ReadAsStringAsync();
                    LoggerHelper.LogWebError("AcademicYearsG", "PowerCampus.DataAccess - PeopleController", content2);
                    AcademicInfoViewModelDegree = JsonConvert.DeserializeObject<AcademicInfoViewModelDegree>(content2);
                    return Json(new {AcademicInfoViewModelDegree}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LoggerHelper.LogWebError("AcademicYearsG", "PowerCampus.DataAccess - PeopleController", resp2.ReasonPhrase);
                    if (resp2.StatusCode == HttpStatusCode.NotFound)
                    {
                        LoggerHelper.LogWebError("AcademicYearsG", "PowerCampus.DataAccess - PeopleController - Not Found", resp2.ReasonPhrase);
                        return RedirectToRoute("ErrorException");
                    }
                    else
                    {
                        LoggerHelper.LogWebError("AcademicYearsG", "PowerCampus.DataAccess - PeopleController - Other Error", resp2.ReasonPhrase);
                        return RedirectToRoute("ErrorException");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                LoggerHelper.LogWebError("AcademicYearsG", "PowerCampus.DataAccess - PeopleController", ex.Message);
                return RedirectToRoute("ErrorException");
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("AcademicYearsG", "PowerCampus.DataAccess - PeopleController", ex.Message);
                return RedirectToRoute("ErrorException");
            }
        }


    }
}