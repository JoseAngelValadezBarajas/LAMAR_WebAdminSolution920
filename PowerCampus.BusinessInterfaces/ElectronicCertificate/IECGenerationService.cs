// --------------------------------------------------------------------
// <copyright file="IECGenerationService.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces.ElectronicCertificate
{
    /// <summary>
    /// IECGenerationService
    /// </summary>
    public interface IECGenerationService
    {
        /// <summary>
        /// Gets the certification types.
        /// </summary>
        /// <returns></returns>
        List<CodeTable> GetCertificationTypes();

        /// <summary>
        /// Gets the folio.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="institutionCampusId">The institution campus identifier.</param>
        /// <returns></returns>
        string GetFolio(string peopleId, int institutionCampusId);

        /// <summary>
        /// Gets the operator campus validation.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        bool GetOperatorCampusValidation(string operatorId);

        /// <summary>
        /// Gets the period types.
        /// </summary>
        /// <returns></returns>
        List<CodeTable> GetPeriodTypes();

        /// <summary>
        /// Gets the studies program detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        StudiesProgramDetail GetStudiesProgramDetail(int id);

        /// <summary>
        /// Gets the studies programs.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        List<StudiesProgram> GetStudiesPrograms(string peopleId, string operatorId);

        #region Courses List

        /// <summary>
        /// Creates the transcript detail certificate.
        /// </summary>
        /// <param name="academicPlanCourseDetails">The academic plan course details.</param>
        void CreateTranscriptDetailCertificate(TranscriptDetailCertificate academicPlanCourseDetails);

        /// <summary>
        /// Gets the academic plan course by student.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="rvoeId">The rvoe identifier.</param>
        /// <returns></returns>
        AcademicPlanCourse GetAcademicPlanCourseByStudent(string peopleId, int rvoeId);

        /// <summary>
        /// Gets the courses outside academic plan.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="rvoeId">The rvoe identifier.</param>
        /// <returns></returns>
        AcademicPlanCourse GetCoursesOutsideAcademicPlan(string peopleId, int rvoeId);

        #endregion Courses List
    }
}