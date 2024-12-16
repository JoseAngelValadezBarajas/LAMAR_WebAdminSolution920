// --------------------------------------------------------------------
// <copyright file="FederalEntitiesController - Copy.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business.ElectronicDegree;
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.Entities.ElectronicDegree;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers.ElectronicDegree
{
    /// <summary>
    /// Handles all the CRUD operations for Federal Entities
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController"/>
    [LoggingActionFilterAttribute]
    public class BackgroundStudyStypeController : ApiController
    {
        private readonly IBackgroundStudyTypeServices services;

        public BackgroundStudyStypeController() => services = new BackgroundStudyTypeServices();

        [Route("api/BackgroundStudyType/GetCatalogMapping")]
        [HttpGet]
        public IHttpActionResult GetCatalogMapping()
        {
            CodeBackgroundStudyTypeMapping[] catalog = services.GetCurrentCatalog();
            return Ok(catalog);
        }

        [Route("api/BackgroundStudyType/GetScholarshipLevelsCatalog")]
        [HttpGet]
        public IHttpActionResult GetScholarshipLevelsCatalog()
        {
            CodeScholarshipLevel[] catalog = services.GetScholarshipLevelsCatalog();
            return Ok(catalog);
        }

        [Route("api/BackgroundStudyType/SaveScholarshipLevelMapping")]
        [HttpPost]
        public IHttpActionResult SaveScholarshipLevelMapping(CodeBackgroundStudyTypeMapping mapping)
        {
            services.SaveScholarshipLevelMapping(mapping);
            return Ok();
        }
    }
}