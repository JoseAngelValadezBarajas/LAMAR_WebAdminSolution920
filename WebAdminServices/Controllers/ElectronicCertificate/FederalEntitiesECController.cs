// --------------------------------------------------------------------
// <copyright file="FederalEntitiesController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business.ElectronicDegree;
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers.ElectronicCertificate
{
    /// <summary>
    /// Handles all the CRUD operations for Federal Entities
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class FederalEntitiesECController : ApiController
    {
        /// <summary>
        /// The services
        /// </summary>
        private readonly IFederalEntitiesServices services;

        /// <summary>
        /// Initializes a new instance of the <see cref="FederalEntitiesECController"/> class.
        /// </summary>
        public FederalEntitiesECController() => services = new FederalEntitiesServices();

        /// <summary>
        /// Gets the catalog mapping.
        /// </summary>
        /// <returns></returns>
        [Route("api/EC/FederalEntities/Mapping")]
        [HttpGet]
        public IHttpActionResult GetCatalogMapping()
        {
            CodeFederalEntitityMapping[] catalog = services.GetCurrentCatalog();
            return Ok(catalog);
        }

        /// <summary>
        /// Gets the code catalog.
        /// </summary>
        /// <returns></returns>
        [Route("api/EC/FederalEntities/CodeCatalog")]
        [HttpGet]
        public IHttpActionResult GetCodeCatalog()
        {
            List<PowerCampus.Entities.ElectronicCertificate.CodeTable> catalog = services.GetCodeCatalog();
            return Ok(catalog);
        }

        /// <summary>
        /// Gets the code catalog without foreign.
        /// </summary>
        /// <returns></returns>
        [Route("api/EC/FederalEntities/CodeCatalogWithoutForeign")]
        [HttpGet]
        public IHttpActionResult GetCodeCatalogWithoutForeign()
        {
            List<PowerCampus.Entities.ElectronicCertificate.CodeTable> catalog = services.GetCodeCatalogWithoutForeign();
            return Ok(catalog);
        }

        /// <summary>
        /// Gets the states catalog.
        /// </summary>
        /// <returns></returns>
        [Route("api/EC/FederalEntities/States")]
        [HttpGet]
        public IHttpActionResult GetStatesCatalog()
        {
            CodeState[] catalog = services.GetStatesCatalog();
            return Ok(catalog);
        }

        /// <summary>
        /// Saves the federal entity mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        [Route("api/EC/FederalEntities/Mapping")]
        [HttpPost]
        public IHttpActionResult SaveFederalEntityMapping(CodeFederalEntitityMapping mapping)
        {
            services.SaveFederalEntityMapping(mapping);
            return Ok();
        }
    }
}