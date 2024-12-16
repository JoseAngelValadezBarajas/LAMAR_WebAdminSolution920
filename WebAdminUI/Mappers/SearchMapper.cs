// --------------------------------------------------------------------
// <copyright file="SearchMapper.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System.Collections.Generic;
using WebAdminUI.Models.SearchResult;

namespace WebAdminUI.SearchMappers
{
    /// <summary>
    /// Mapper for search view models
    /// </summary>
    internal static class SearchMapper
    {
        /// <summary>
        /// To the search result model.
        /// </summary>
        /// <param name="searchResult">The search result.</param>
        /// <returns></returns>
        internal static SearchResultViewModel ToViewModel(this SearchResult searchResult)
        {
            SearchResultViewModel searchResultViewModel = new SearchResultViewModel();

            if (string.IsNullOrEmpty(searchResult.PeopleOrgCodeId))
            {
                searchResultViewModel.PeopleOrgCodeId = string.Empty;
                searchResultViewModel.FullName = Views.FiscalRecords.App_LocalResources.MenuResource.NoResultsFound;
            }
            else
            {
                searchResultViewModel.PeopleOrgCodeId = searchResult.PeopleOrgCodeId;
                searchResultViewModel.FullName = searchResult.FullName;
            }
            return searchResultViewModel;
        }

        /// <summary>
        /// To the advanced search result model.
        /// </summary>
        /// <param name="searchResult">The search result.</param>
        /// <returns></returns>
        internal static SearchResultViewModel ToViewModelAdvanced(this SearchResult searchResult)
        {
            SearchResultViewModel searchResultViewModel = new SearchResultViewModel();

            foreach (string recordtype in searchResult.RecordTypeList)
            {
                if (string.IsNullOrEmpty(searchResultViewModel.RecordTypeList))
                    searchResultViewModel.RecordTypeList = recordtype;
                else
                    searchResultViewModel.RecordTypeList = $"{searchResultViewModel.RecordTypeList}, {recordtype}";
            }

            searchResultViewModel.PeopleOrgCodeId = searchResult.PeopleOrgCodeId;
            searchResultViewModel.FullName = searchResult.FullName;
            searchResultViewModel.PrimaryEmail = searchResult.PrimaryEmail;

            return searchResultViewModel;
        }

        /// <summary>
        /// To the type of the record.
        /// </summary>
        /// <param name="recordType">Type of the record.</param>
        /// <returns></returns>
        internal static SearchResultViewModel ToViewModelWithRecordType(this SearchResult recordType)
        {
            SearchResultViewModel searchResultViewModel = new SearchResultViewModel
            {
                RecordTypeListFull = new List<RecordTypeViewModel>()
            };

            foreach (RecordType recordT in recordType.RecordTypeListFull)
            {
                searchResultViewModel.RecordTypeListFull.Add(
                    new RecordTypeViewModel
                    {
                        CodeValue = recordT.CodeValue,
                        Description = recordT.Description,
                        IsOrgType = recordT.IsOrgType,
                        IsPeopleType = recordT.IsPeopleType
                    });
            }

            return searchResultViewModel;
        }
    }
}