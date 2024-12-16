// --------------------------------------------------------------------
// <copyright file="ElectronicDegreeInformationController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business.ElectronicDegree;
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using System.Web.Http;

namespace WebAdminServices.Controllers.ElectronicDegree
{
    /// <summary>
    /// Electronic Degree Information Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class ElectronicDegreeInformationController : ApiController
    {
        private readonly IElectronicDegreeInformationServices _electronicDegreeInformationServices;

        public ElectronicDegreeInformationController() => _electronicDegreeInformationServices =
            new ElectronicDegreeInformationServices();

        [Route("api/Delete/{id}")]
        [Authorize]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            _electronicDegreeInformationServices.DeleteElectronicDegreeInfo(id);
            return Ok();
        }

        /// <summary>
        /// Gets the institutions.
        /// </summary>
        /// <returns></returns>
        [Route("api/ElectronicDegreeInformation")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult Get(string folio, string student, string degreeType, string major)
        {
            folio = folio != null ? folio : "";
            student = student != null ? student : "";
            degreeType = degreeType != null ? degreeType : "";
            major = major != null ? major : "";
            List<ElectronicDegreeInfo> electronicDegreeInfos = _electronicDegreeInformationServices.GetElectronicDegreeInformation(folio, student, degreeType, major);
            if (electronicDegreeInfos == null)
                return NotFound();
            return Ok(electronicDegreeInfos);
        }

        /// <summary>
        /// Gets the cancelation catalog.
        /// </summary>
        /// <returns></returns>
        [Route("api/CancelationCatalog")]
        [HttpGet]
        public IHttpActionResult GetCancelationCatalog()
        {
            List<CodeCancelationCatalog> cancelationCatalog = _electronicDegreeInformationServices.GetCancelationCatalog();
            if (cancelationCatalog == null)
                return NotFound();
            return Ok(cancelationCatalog);
        }

        /// <summary>
        /// Gets the electronic degree information.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("api/GetElectronicDegreeInformation/{id}")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetElectronicDegreeInformation(int id)
        {
            ElectronicDegreeInfo electronicDegreeInfo =
                _electronicDegreeInformationServices.GetElectronicDegreeInfo(id);
            if (electronicDegreeInfo == null)
                return NotFound();
            return Ok(electronicDegreeInfo);
        }
    }
}