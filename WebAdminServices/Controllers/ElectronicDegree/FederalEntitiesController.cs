// --------------------------------------------------------------------
// <copyright file="FederalEntitiesController.cs" company="Ellucian">
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
    public class FederalEntitiesController : ApiController
    {
        private readonly IFederalEntitiesServices services;

        public FederalEntitiesController() => services = new FederalEntitiesServices();

        [Route("api/FederalEntities/GetCatalogMapping")]
        [HttpGet]
        public IHttpActionResult GetCatalogMapping()
        {
            CodeFederalEntitityMapping[] catalog = services.GetCurrentCatalog();
            return Ok(catalog);
        }

        [Route("api/FederalEntities/GetStatesCatalog")]
        [HttpGet]
        public IHttpActionResult GetStatesCatalog()
        {
            CodeState[] catalog = services.GetStatesCatalog();
            return Ok(catalog);
        }

        [Route("api/FederalEntities/SaveFederalEntityMapping")]
        [HttpPost]
        public IHttpActionResult SaveFederalEntityMapping(CodeFederalEntitityMapping mapping)
        {
            services.SaveFederalEntityMapping(mapping);
            return Ok();
        }
    }
}