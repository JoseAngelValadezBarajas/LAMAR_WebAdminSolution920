// --------------------------------------------------------------------
// <copyright file="IECAcademicPlanService.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces.ElectronicCertificate
{
    /// <summary>
    /// IECAcademicPlan Service
    /// </summary>
    public interface IECAcademicPlanService
    {
        /// <summary>
        /// Creates the ec academic plan course.
        /// </summary>
        /// <param name="pdcCourse">The PDC course.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        bool CreateECAcademicPlanCourse(PdcCourse pdcCourse, string operatorId);

        /// <summary>
        /// Gets the ec campus from institution campus.
        /// </summary>
        /// <returns></returns>
        List<CodeTable> GetECCampusFromInstitutionCampus();

        /// <summary>
        /// Gets the ec deg req by campus year term.
        /// </summary>
        /// <param name="campusCodeId">The campus code identifier.</param>
        /// <param name="matricYear">The matric year.</param>
        /// <param name="matricTerm">The matric term.</param>
        /// <returns></returns>
        List<PdcRvoe> GetECDegReqByCampusYearTerm(string campusCodeId, string matricYear, string matricTerm);

        /// <summary>
        /// Gets the ec deg req courses by rvoe.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        PdcRvoe GetECDegReqCoursesByRvoe(int id);

        /// <summary>
        /// Gets the ec matriculation term by campus year.
        /// </summary>
        /// <param name="campusCodeId">The campus code identifier.</param>
        /// <param name="matricYear">The matric year.</param>
        /// <returns></returns>
        List<CodeTable> GetECMatriculationTermByCampusYear(string campusCodeId, string matricYear);

        /// <summary>
        /// Gets the ec matriculation year by campus.
        /// </summary>
        /// <param name="campusCodeId">The campus code identifier.</param>
        /// <returns></returns>
        List<CodeTable> GetECMatriculationYearByCampus(string campusCodeId);

        /// <summary>
        /// Updates the ec academic plan course.
        /// </summary>
        /// <param name="pdcCourse">The PDC course.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        bool UpdateECAcademicPlanCourse(PdcCourse pdcCourse, string operatorId);
    }
}