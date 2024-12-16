// --------------------------------------------------------------------
// <copyright file="GenerationsController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Business.ElectronicCertificate;
using PowerCampus.BusinessInterfaces.ElectronicCertificate;
using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;
using System.Web.Http;

namespace WebAdminServices.Controllers.ElectronicCertificate
{
    /// <summary>
    /// GenerationsController
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class GenerationsController : ApiController
    {
        /// <summary>
        /// The generate services
        /// </summary>
        private readonly IECGenerationService _generateServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateController" /> class.
        /// </summary>
        public GenerationsController() => _generateServices = new ECGenerationService();

        /// <summary>
        /// Certifications the types catalog.
        /// </summary>
        /// <returns></returns>
        [Route("api/EC/CertificationTypes")]
        [HttpGet]
        public IHttpActionResult CertificationTypesCatalog()
        {
            List<CodeTable> catalog = _generateServices.GetCertificationTypes();
            return Ok(catalog);
        }

        /// <summary>
        /// Gets the folio of the Electronic Certificate generation
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="institutionCampusId">The institution campus identifier.</param>
        /// <returns></returns>
        [Route("api/EC/Generations/Folio/{peopleId}/{institutionCampusId}")]
        [HttpGet]
        public IHttpActionResult Folio(string peopleId, int institutionCampusId)
        {
            string folio = _generateServices.GetFolio(peopleId, institutionCampusId);
            return Ok(folio);
        }

        /// <summary>
        /// Periods the types.
        /// </summary>
        /// <returns></returns>
        [Route("api/EC/PeriodTypes")]
        [HttpGet]
        public IHttpActionResult PeriodTypes()
        {
            List<CodeTable> catalog = _generateServices.GetPeriodTypes();
            return Ok(catalog);
        }

        /// <summary>
        /// Studieses the program detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("api/EC/StudiesProgramDetail/{id}")]
        [HttpGet]
        public IHttpActionResult StudiesProgramDetail(int id)
        {
            StudiesProgramDetail studiesProgramDetail = _generateServices.GetStudiesProgramDetail(id);
            return Ok(studiesProgramDetail);
        }

        /// <summary>
        /// Studies the programs.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <returns></returns>
        [Route("api/EC/StudiesPrograms/{peopleId}/{operatorId}")]
        [HttpGet]
        public IHttpActionResult StudiesPrograms(string peopleId, string operatorId)
        {
            List<StudiesProgram> studiesPrograms = _generateServices.GetStudiesPrograms(peopleId, operatorId);
            return Ok(studiesPrograms);
        }

        /// <summary>
        /// Validates the operator campus.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        [Route("api/EC/ValidateOperatorCampus/{operatorId}")]
        [HttpGet]
        public IHttpActionResult ValidateOperatorCampus(string operatorId)
        {
            bool hasCampusRelated = _generateServices.GetOperatorCampusValidation(operatorId);
            return Ok(hasCampusRelated);
        }

        #region Courses List

        /// <summary>
        /// Academics the plan course by student.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="rvoeId">The rvoe identifier.</param>
        /// <returns></returns>
        [Route("api/EC/AcademicPlanCourseByStudent/{peopleId}/{rvoeId}")]
        [HttpGet]
        public IHttpActionResult AcademicPlanCourseByStudent(string peopleId, int rvoeId)
        {
            AcademicPlanCourse academicPlanCourse = _generateServices.GetAcademicPlanCourseByStudent(peopleId, rvoeId);
            return Ok(academicPlanCourse);
        }

        /// <summary>
        /// Academics the plan course by student.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="rvoeId">The rvoe identifier.</param>
        /// <returns></returns>
        [Route("api/EC/CoursesOutsideAcademicPlan/{peopleId}/{rvoeId}")]
        [HttpGet]
        public IHttpActionResult CoursesOutsideAcademicPlan(string peopleId, int rvoeId)
        {
            AcademicPlanCourse academicPlanCourse = _generateServices.GetCoursesOutsideAcademicPlan(peopleId, rvoeId);
            return Ok(academicPlanCourse);
        }

        /// <summary>
        /// Creates the transcript detail certificate.
        /// </summary>
        /// <param name="academicPlanCourseDetails">The academic plan course details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/EC/CreateTranscriptDetailCertificate")]
        public IHttpActionResult CreateTranscriptDetailCertificate(TranscriptDetailCertificate academicPlanCourseDetails)
        {
            _generateServices.CreateTranscriptDetailCertificate(academicPlanCourseDetails);
            return Ok();
        }

        #endregion Courses List
    }
}