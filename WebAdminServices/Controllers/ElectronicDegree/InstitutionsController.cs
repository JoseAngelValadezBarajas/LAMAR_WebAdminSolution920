// --------------------------------------------------------------------
// <copyright file="InstitutionsController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business.ElectronicDegree;
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers.ElectronicDegree
{
    /// <summary>
    /// Signers Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class InstitutionsController : ApiController
    {
        private readonly IInstitutionService _institutionServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstitutionsController"/> class.
        /// </summary>
        public InstitutionsController() => _institutionServices = new InstitutionService();

        /// <summary>
        /// Creates the institution.
        /// </summary>
        /// <param name="institutionDAModel">The major list.</param>
        /// <returns></returns>
        [Route("api/Institution/Create")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult Create(InstitutionDAModel institutionDAModel)
        {
            _institutionServices.Createinstitution(institutionDAModel);
            return Ok();
        }

        /// <summary>
        /// Deletes the mapping.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("api/Institution/Mapping/Delete/{id}")]
        [HttpDelete]
        [HttpPost]
        public IHttpActionResult DeleteMapping(int id)
        {
            _institutionServices.DeleteMapping(id);
            return Ok();
        }

        /// <summary>
        /// Detailses this instance.
        /// </summary>
        /// <returns></returns>
        [Route("api/Institution/Details")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult Details()
        {
            List<InstitutionModel> institutions = _institutionServices.GetInstitutionsDetail();
            if (institutions == null)
                return NotFound();
            return Ok(institutions);
        }

        /// <summary>
        /// Gets the institutions.
        /// </summary>
        /// <returns></returns>
        [Route("api/Institution/Get")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<InstitutionModel> institutions = _institutionServices.GetInstitutions();
            if (institutions == null)
                return NotFound();
            return Ok(institutions);
        }

        /// <summary>
        /// Gets the specified institution identifier.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        [Route("api/Institution/GetInstitution")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetInstitution(int institutionId)
        {
            if (institutionId > 0)
                return Ok(_institutionServices.GetInstitution(institutionId));

            return NotFound();
        }

        /// <summary>
        /// Gets the institution by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("api/Institution/{id}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetInstitutionById(int id)
        {
            InstitutionModel institution = _institutionServices.GetInstitutionById(id);
            if (institution == null)
                return NotFound();
            return Ok(institution);
        }

        /// <summary>
        /// Gets the majors by institution identifier.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        [Route("api/Institution/GetMajorsByInstitutionId")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetMajorsByInstitutionId(int institutionId)
        {
            List<MajorList> majorList = _institutionServices.GetMajorsByInstitutionId(institutionId);
            if (majorList == null)
                return NotFound();
            return Ok(majorList);
        }

        /// <summary>
        /// Gets the power campus institutions.
        /// </summary>
        /// <returns></returns>
        [Route("api/Institution/GetPowerCampusInstitutions")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetPowerCampusInstitutions()
        {
            List<DropDownListModel> options = _institutionServices.GetPowerCampusInstitutions();
            if (options == null)
                return NotFound();
            return Ok(options);
        }

        /// <summary>
        /// Gets the rvoe options.
        /// </summary>
        /// <returns></returns>
        [Route("api/Institution/GetRvoeOptions")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetRvoeOptions()
        {
            List<DropDownListModel> options = _institutionServices.GetRvoeOptions();
            if (options == null)
                return NotFound();
            return Ok(options);
        }

        /// <summary>
        /// Maps the institution major rvoe.
        /// </summary>
        /// <param name="mapInstitutionMajorRvoe">The map institution major rvoe.</param>
        /// <returns></returns>
        [Route("api/Institution/MapInstitutionMajorRvoe")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult MapInstitutionMajorRvoe(MapInstitutionMajorRvoe mapInstitutionMajorRvoe)
        {
            _institutionServices.MapInstitutionMajorRvoe(mapInstitutionMajorRvoe);
            return Ok();
        }

        /// <summary>
        /// Maps the major.
        /// </summary>
        /// <param name="majorMappingModel">The major mapping model.</param>
        /// <returns></returns>
        [Route("api/Institution/MapMajor")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult MapMajor(MajorMappingModel majorMappingModel)
        {
            _institutionServices.MapMajor(majorMappingModel);
            return Ok();
        }

        /// <summary>
        /// Updates the specified institution da model.
        /// </summary>
        /// <param name="institutionDAModel">The institution da model.</param>
        /// <returns></returns>
        [Route("api/Institution/Update")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult Update(InstitutionDAModel institutionDAModel)
        {
            _institutionServices.Update(institutionDAModel);
            return Ok();
        }

        /// <summary>
        /// Validates the code.
        /// </summary>
        /// <param name="code">The curp.</param>
        /// <returns></returns>
        [Route("api/Institution/ValidateCode")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult ValidateCode(string code)
        {
            bool Exists = _institutionServices.ValidateCode(code);
            return Ok(Exists);
        }

        /// <summary>
        /// Validates the folio.
        /// </summary>
        /// <param name="folio">The folio.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        [Route("api/Institution/ValidateFolio")]
        [HttpGet]
        public IHttpActionResult ValidateFolio(string folio, string code)
        {
            bool Exists = _institutionServices.ValidateFolio(folio, code);
            return Ok(Exists);
        }

        /// <summary>
        /// Validates the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [Route("api/Institution/ValidateName")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult ValidateName(string name)
        {
            bool Exists = _institutionServices.ValidateName(name);
            return Ok(Exists);
        }
    }
}