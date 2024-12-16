// --------------------------------------------------------------------
// <copyright file="InstitutionMapper.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Newtonsoft.Json;
using PowerCampus.Entities;
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using WebAdminUI.Areas.ElectronicDegree.Models.Institution;
using WebAdminUI.Helpers;

namespace WebAdminUI.Areas.ElectronicDegree.Mappers
{
    /// <summary>
    /// Institution Mapper
    /// </summary>
    internal static class InstitutionMapper
    {
        internal static async System.Threading.Tasks.Task<AddInstitutionViewModel> ToViewModelAsync(this InstitutionModel institutionModel, Account Account, int institutionId = 0)
        {
            AddInstitutionViewModel institutionViewModel = null;
            List<DropDownListModel> options = new List<DropDownListModel>();
            HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Institution/GetPowerCampusInstitutions");
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                string content = await resp.Content.ReadAsStringAsync();
                options = JsonConvert.DeserializeObject<List<DropDownListModel>>(content);

                institutionViewModel = new AddInstitutionViewModel
                {
                    Equivalents = options,
                };

                institutionViewModel.Code = institutionModel.Code;
                institutionViewModel.EquivalentId = institutionModel.Equivalent;
                institutionViewModel.Folio = institutionModel.Folio;
                institutionViewModel.Id = institutionModel.Id;
                if (institutionModel.Majors != null)
                    institutionViewModel.MajorsNumber = (int)institutionModel.Majors;
                if (institutionModel.Id == institutionId && institutionModel.MajorList != null)
                    institutionViewModel.MajorsViewModelList = institutionModel.MajorList.ToViewModel();
                institutionViewModel.Name = institutionModel.Name;
            }
            return institutionViewModel;
        }

        internal static async System.Threading.Tasks.Task<AddInstitutionViewModel> ToViewModelAsync(this InstitutionDAModel institutionDAModel, Account Account)
        {
            AddInstitutionViewModel institutionViewModel = null;
            List<DropDownListModel> options = new List<DropDownListModel>();
            HttpClient client = PowerCampusHttpClient.GetClient(Account: Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Institution/GetPowerCampusInstitutions");
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                string content = await resp.Content.ReadAsStringAsync();
                options = JsonConvert.DeserializeObject<List<DropDownListModel>>(content);

                institutionViewModel = new AddInstitutionViewModel
                {
                    Code = institutionDAModel.Code,
                    Equivalents = options,
                    EquivalentId = institutionDAModel.EquivalentId != null ? institutionDAModel.EquivalentId.ToString() : string.Empty,
                    Id = institutionDAModel.Id,
                    Folio = institutionDAModel.Folio,
                    FolioFormat = institutionDAModel.FolioFormat,
                    Name = institutionDAModel.Name
                };
            }

            return institutionViewModel;
        }
    }
}