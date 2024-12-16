// --------------------------------------------------------------------
// <copyright file="ISearchServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces
{
    /// <summary>
    /// ISearch Service
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Gets the advanced search.
        /// </summary>
        /// <param name="people">The people.</param>
        /// <returns></returns>
        List<SearchResult> GetAdvancedSearch(People people);

        /// <summary>
        /// Gets the advanced search.
        /// </summary>
        /// <param name="organization">The organization.</param>
        /// <returns></returns>
        List<SearchResult> GetAdvancedSearch(Organization organization);

        /// <summary>
        /// Searches the result.
        /// </summary>
        /// <param name="Keyword">The keyword.</param>
        /// <returns></returns>
        List<SearchResult> SearchResult(string Keyword);
    }
}