// --------------------------------------------------------------------
// <copyright file="AcademicPlansController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business.ElectronicCertificate;
using PowerCampus.BusinessInterfaces.ElectronicCertificate;
using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers.ElectronicCertificate
{
    /// <summary>
    /// AcademicPlans Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    [Authorize]
    public class AcademicPlansController : ApiController
    {
        private readonly IECAcademicPlanService _academicPlanServices;

        public AcademicPlansController() => _academicPlanServices = new ECAcademicPlanService();

        /// <summary>
        /// Courseses the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [Route("api/EC/AcademicPlans/Courses/{id}")]
        [HttpGet]
        public IHttpActionResult Courses(int id)
        {
            PdcRvoe pdc = _academicPlanServices.GetECDegReqCoursesByRvoe(id);
            if (pdc == null)
                return NotFound();
            return Ok(pdc);
        }

        /// <summary>
        /// Gets the institutions.
        /// </summary>
        /// <returns></returns>
        [Route("api/EC/AcademicPlans")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            List<CodeTable> campuses = _academicPlanServices.GetECCampusFromInstitutionCampus();
            if (campuses == null)
                return NotFound();
            return Ok(campuses);
        }

        /// <summary>
        /// Matrics the terms.
        /// </summary>
        /// <param name="codeValueKey">The code value key.</param>
        /// <param name="matricYear">The matric year.</param>
        /// <returns></returns>
        [Route("api/EC/AcademicPlans/MatricTerms/{campusCodeId}/{matricYear}")]
        [HttpGet]
        public IHttpActionResult MatricTerms(string campusCodeId, string matricYear)
        {
            List<CodeTable> campuses = _academicPlanServices.GetECMatriculationTermByCampusYear(campusCodeId, matricYear);
            if (campuses == null)
                return NotFound();
            return Ok(campuses);
        }

        /// <summary>
        /// Matrics the years.
        /// </summary>
        /// <param name="codeValueKey">The code value key.</param>
        /// <returns></returns>
        [Route("api/EC/AcademicPlans/MatricYears/{campusCodeId}")]
        [HttpGet]
        public IHttpActionResult MatricYears(string campusCodeId)
        {
            List<CodeTable> campuses = _academicPlanServices.GetECMatriculationYearByCampus(campusCodeId);
            if (campuses == null)
                return NotFound();
            return Ok(campuses);
        }

        /// <summary>
        /// Matrics the years.
        /// </summary>
        /// <param name="codeValueKey">The code value key.</param>
        /// <returns></returns>
        [Route("api/EC/AcademicPlans/Programs/{campusCodeId}/{matricYear}/{matricTerm}")]
        [HttpGet]
        public IHttpActionResult Programs(string campusCodeId, string matricYear, string matricTerm)
        {
            List<PdcRvoe> pdcs = _academicPlanServices.GetECDegReqByCampusYearTerm(campusCodeId, matricYear, matricTerm);
            if (pdcs == null)
                return NotFound();
            return Ok(pdcs);
        }

        /// <summary>
        /// Saves the specified PDC courses.
        /// </summary>
        /// <param name="pdcCourses">The PDC courses.</param>
        /// <returns></returns>
        [Route("api/EC/AcademicPlans/Courses/Save")]
        [HttpPost]
        public IHttpActionResult Save(PdcCourses pdcCourses)
        {
            foreach (PdcCourse pdcCourse in pdcCourses.PdcCourse)
            {
                if (pdcCourse.AcademicPlanCourseCatalogId > 0)
                    _academicPlanServices.UpdateECAcademicPlanCourse(pdcCourse, pdcCourses.OperatorId);
                else if (!string.IsNullOrEmpty(pdcCourse.SepId) || pdcCourse.SubjectTypeId > 0)
                    _academicPlanServices.CreateECAcademicPlanCourse(pdcCourse, pdcCourses.OperatorId);
            }
            return Ok();
        }
    }
}