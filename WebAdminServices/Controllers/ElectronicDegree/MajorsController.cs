// --------------------------------------------------------------------
// <copyright file="MajorsController.cs" company="Ellucian">
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
    /// Majors Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class MajorsController : ApiController
    {
        private readonly IMajorServices _majorServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="MajorsController"/> class.
        /// </summary>
        public MajorsController() => _majorServices = new MajorServices();

        /// <summary>
        /// Creates the major.
        /// </summary>
        /// <param name="majorList">The major list.</param>
        /// <returns></returns>
        [Route("api/Majors/Create")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult Create(MajorList majorList)
        {
            _majorServices.CreateMajor(majorList);
            return Ok();
        }

        /// <summary>
        /// Deletes the specified major identifier.
        /// </summary>
        /// <param name="majorId">The major identifier.</param>
        /// <returns></returns>
        [Route("api/Majors/Delete/{majorId}")]
        [Authorize]
        [HttpDelete]
        public IHttpActionResult Delete(int majorId)
        {
            _majorServices.DeleteMajor(majorId);
            return Ok();
        }

        /// <summary>
        /// Gets the edit major.
        /// </summary>
        /// <param name="majorId">The major identifier.</param>
        /// <returns></returns>
        [Route("api/Majors/{majorId}")]
        [HttpGet]
        public IHttpActionResult Major(int majorId)
        {
            MajorList majorList = _majorServices.GetMajor(majorId);
            if (majorList == null)
                return NotFound();
            return Ok(majorList);
        }

        /// <summary>
        /// Gets the majors.
        /// </summary>
        /// <returns></returns>
        [Route("api/Majors")]
        [HttpGet]
        public IHttpActionResult Majors()
        {
            List<MajorList> major = _majorServices.GetMajors();
            if (major == null)
                return NotFound();
            return Ok(major);
        }

        /// <summary>
        /// Updates the major.
        /// </summary>
        /// <param name="majorList">The major list.</param>
        /// <returns></returns>
        [Route("api/Majors/Update")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult Update(MajorList majorList)
        {
            _majorServices.UpdateMajor(majorList);
            return Ok();
        }

        /// <summary>
        /// Validates if the code already exists.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        [Route("api/Majors/ValidateCode/{code}")]
        [HttpGet]
        public IHttpActionResult ValidateCode(string code)
        {
            bool Exists = _majorServices.ValidateCode(code);
            return Ok(Exists);
        }
    }
}