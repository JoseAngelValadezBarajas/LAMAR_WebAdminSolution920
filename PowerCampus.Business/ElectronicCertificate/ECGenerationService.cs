// --------------------------------------------------------------------
// <copyright file="ECGenerationService.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.BusinessInterfaces.ElectronicCertificate;
using PowerCampus.DataAccess.ElectronicCertificate;
using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;

namespace PowerCampus.Business.ElectronicCertificate
{
    /// <summary>
    /// ECGenerationService
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.ElectronicCertificate.IECGenerationService" />
    public class ECGenerationService : IECGenerationService
    {
        /// <summary>
        /// The entities da
        /// </summary>
        private readonly ECGenerationDA entitiesDa;

        /// <summary>
        /// Initializes a new instance of the <see cref="ECGenerationService" /> class.
        /// </summary>
        public ECGenerationService() => entitiesDa = new ECGenerationDA();

        /// <summary>
        /// Gets the certification types.
        /// </summary>
        /// <returns></returns>
        public List<CodeTable> GetCertificationTypes() => entitiesDa.GetCertificationTypes();

        /// <summary>
        /// Gets the folio.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="institutionCampusId">The institution campus identifier.</param>
        /// <returns></returns>
        public string GetFolio(string peopleId, int institutionCampusId) => entitiesDa.GetFolio(peopleId, institutionCampusId);

        /// <summary>
        /// Gets the operator campus validation.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public bool GetOperatorCampusValidation(string operatorId) => entitiesDa.GetOperatorCampusValidation(operatorId);

        /// <summary>
        /// Gets the period types.
        /// </summary>
        /// <returns></returns>
        public List<CodeTable> GetPeriodTypes() => entitiesDa.GetPeriodTypes();

        /// <summary>
        /// Gets the studies program detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public StudiesProgramDetail GetStudiesProgramDetail(int id) => entitiesDa.GetStudiesProgramDetail(id);

        /// <summary>
        /// Gets the studies programs.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public List<StudiesProgram> GetStudiesPrograms(string peopleId, string operatorId) => entitiesDa.GetStudiesPrograms(peopleId, operatorId);

        #region Courses List

        /// <summary>
        /// Creates the transcript detail certificate.
        /// </summary>
        /// <param name="academicPlanCourseDetails">The academic plan course details.</param>
        public void CreateTranscriptDetailCertificate(TranscriptDetailCertificate academicPlanCourseDetails)
        {
            entitiesDa.CreateTranscriptDetailCertificate(academicPlanCourseDetails.CoursesToInsert);
            entitiesDa.UpdateTranscriptDetailCertificate(academicPlanCourseDetails.CoursesToUpdate);
        }

        /// <summary>
        /// Gets the academic plan course by student.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="rvoeId">The rvoe identifier.</param>
        /// <returns></returns>
        public AcademicPlanCourse GetAcademicPlanCourseByStudent(string peopleId, int rvoeId) => entitiesDa.GetAcademicPlanCourseByStudent(peopleId, rvoeId);

        /// <summary>
        /// Gets the courses outside academic plan.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="rvoeId">The rvoe identifier.</param>
        /// <returns></returns>
        public AcademicPlanCourse GetCoursesOutsideAcademicPlan(string peopleId, int rvoeId) => entitiesDa.GetCoursesOutsideAcademicPlan(peopleId, rvoeId);

        #endregion Courses List
    }
}