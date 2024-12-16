// --------------------------------------------------------------------
// <copyright file="SearchServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.BusinessInterfaces;
using PowerCampus.DataAccess;
using PowerCampus.Entities;
using System.Collections.Generic;

namespace PowerCampus.Business
{
    /// <summary>
    /// SearchServices Class
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.ISearchService" />
    public class SearchServices : ISearchService
    {
        private readonly SearchDA _searchDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchServices"/> class.
        /// </summary>
        public SearchServices()
        {
            _searchDA = new SearchDA();
        }

        /// <summary>
        /// Gets the advanced search.
        /// </summary>
        /// <param name="people">The people.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<SearchResult> GetAdvancedSearch(People people)
        {
            // _logService.Debug("GetAdvancedSearch starts");
            List<SearchResult> lstSearchResult = _searchDA.GetAdvancedSearch(people);
            // _logService.Debug("GetAdvancedSearch ends");
            return lstSearchResult;
        }

        /// <summary>
        /// Gets the advanced search.
        /// </summary>
        /// <param name="organization">The organization.</param>
        /// <returns></returns>
        public List<SearchResult> GetAdvancedSearch(Organization organization)
        {
            // _logService.Debug("GetAdvancedSearch starts");
            List<SearchResult> lstSearchResult = _searchDA.GetAdvancedSearch(organization);
            // _logService.Debug("GetAdvancedSearch ends");
            return lstSearchResult;
        }

        /// <summary>
        /// Searches the result.
        /// </summary>
        /// <param name="Keyword">The keyword.</param>
        /// <returns></returns>
        public List<SearchResult> SearchResult(string Keyword)
        {
            // _logService.Debug("SearchResult starts");
            List<SearchResult> lstSearchResult = _searchDA.SearchResult(Keyword);
            // _logService.Debug("SearchResult ends");
            return lstSearchResult;
        }
    }
}