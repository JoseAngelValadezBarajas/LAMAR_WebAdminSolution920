// --------------------------------------------------------------------
// <copyright file="AuthorizationTypeController.cs" company="Ellucian">
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
    /// Handles all the CRUD operations for the Authorization types
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController"/>
    [LoggingActionFilterAttribute]
    public class AuthorizationTypeController : ApiController
    {
        private readonly IAuthorizationTypeServices services;

        public AuthorizationTypeController() => services = new AuthorizationTypeServices();

        [Route("api/AuthorizationType/CatalogMapping")]
        [HttpGet]
        public IHttpActionResult GetCatalogMapping()
        {
            CodeAuthorizationTypeMapping[] catalog = services.GetCurrentCatalog();
            return Ok(catalog);
        }

        [Route("api/AuthorizationType/RvoeCatalog")]
        [HttpGet]
        public IHttpActionResult GetRvoeCatalog()
        {
            CodeRvoe[] catalog = services.GetRvoeCatalog();
            return Ok(catalog);
        }

        [Route("api/AuthorizationType/Mapping/Save")]
        [HttpPost]
        public IHttpActionResult SaveAuthorizationTypeMapping(CodeAuthorizationTypeMapping mapping)
        {
            services.SaveAuthorizationTypeMapping(mapping);
            return Ok();
        }
    }
}