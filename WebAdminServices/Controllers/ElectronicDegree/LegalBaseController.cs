// --------------------------------------------------------------------
// <copyright file="LegalBaseController.cs" company="Ellucian">
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
    /// Handles all the CRUD operations for the Legal Base
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController"/>
    [LoggingActionFilterAttribute]
    public class LegalBaseController : ApiController
    {
        /// <summary>
        /// The services
        /// </summary>
        private readonly ILegalBaseServices services;

        /// <summary>
        /// Initializes a new instance of the <see cref="LegalBaseController"/> class.
        /// </summary>
        public LegalBaseController() => services = new LegalBaseServices();

        /// <summary>
        /// Gets the legal base catalog.
        /// </summary>
        /// <returns></returns>
        [Route("api/LegalBase/Catalog")]
        [HttpGet]
        public IHttpActionResult GetLegalBaseCatalog()
        {
            CodeLegalBase[] catalog = services.GetLegalBaseCatalog();
            return Ok(catalog);
        }
    }
}