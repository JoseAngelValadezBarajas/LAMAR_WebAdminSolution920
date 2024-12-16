// --------------------------------------------------------------------
// <copyright file="SearchController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Business;
using PowerCampus.Entities;
using System.Collections.Generic;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers
{
    /// <summary>
    /// Search Controller Class
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class SearchController : ApiController
    {
        private readonly SearchServices _searchServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchController"/> class.
        /// </summary>
        public SearchController()
        {
            _searchServices = new SearchServices();
        }

        /// <summary>
        /// Gets the specified keyword.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IHttpActionResult Get(string keyword)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<SearchResult> searchResult = _searchServices.SearchResult(keyword);

                return Ok(searchResult);
            }
            else
                return Unauthorized();
        }

        /// <summary>
        /// Gets the organization.
        /// </summary>
        /// <param name="organization">The organization.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/search/organization")]
        public IHttpActionResult GetOrganization([FromBody] Organization organization)
        {
            if (organization == null)
                return BadRequest();
            return Ok(_searchServices.GetAdvancedSearch(organization));
        }

        /// <summary>
        /// Gets the people.
        /// </summary>
        /// <param name="people">The people.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/search/people")]
        public IHttpActionResult GetPeople([FromBody] People people)
        {
            if (people == null)
                return BadRequest();
            return Ok(_searchServices.GetAdvancedSearch(people));
        }
    }
}