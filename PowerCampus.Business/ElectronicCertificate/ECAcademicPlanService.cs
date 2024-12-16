// --------------------------------------------------------------------
// <copyright file="ECAcademicPlanService.cs" company="Ellucian">
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
    /// ECAcademicPlan Service
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.ElectronicCertificate.IECAcademicPlanService" />
    public class ECAcademicPlanService : IECAcademicPlanService
    {
        /// <summary>
        /// The academic plan da
        /// </summary>
        private readonly ECAcademicPlanDA _academicPlanDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="ECAcademicPlanService"/> class.
        /// </summary>
        public ECAcademicPlanService() => _academicPlanDA = new ECAcademicPlanDA();

        /// <summary>
        /// Creates the ec academic plan course.
        /// </summary>
        /// <param name="pdcCourse">The PDC course.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public bool CreateECAcademicPlanCourse(PdcCourse pdcCourse, string operatorId) => _academicPlanDA.CreateECAcademicPlanCourse(pdcCourse, operatorId);

        /// <summary>
        /// Gets the ec campus.
        /// </summary>
        /// <returns></returns>
        public List<CodeTable> GetECCampusFromInstitutionCampus() => _academicPlanDA.GetECCampusFromInstitutionCampus();

        /// <summary>
        /// Gets the ec deg req by campus year term.
        /// </summary>
        /// <param name="campusCodeId">The campus code identifier.</param>
        /// <param name="matricYear">The matric year.</param>
        /// <param name="matricTerm">The matric term.</param>
        /// <returns></returns>
        public List<PdcRvoe> GetECDegReqByCampusYearTerm(string campusCodeId, string matricYear, string matricTerm) =>
            _academicPlanDA.GetECDegReqByCampusYearTerm(campusCodeId, matricYear, matricTerm);

        /// <summary>
        /// Gets the ec deg req courses by rvoe.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public PdcRvoe GetECDegReqCoursesByRvoe(int id) => _academicPlanDA.GetECDegReqCoursesByRvoe(id);

        /// <summary>
        /// Gets the ec matriculation term by campus year.
        /// </summary>
        /// <param name="campusCodeId">The campus code identifier.</param>
        /// <param name="matricYear">The matric year.</param>
        /// <returns></returns>
        public List<CodeTable> GetECMatriculationTermByCampusYear(string campusCodeId, string matricYear) => _academicPlanDA.GetECMatriculationTermByCampusYear(campusCodeId, matricYear);

        /// <summary>
        /// Gets the ec matriculation year by campus.
        /// </summary>
        /// <param name="campusCodeId">The campus code identifier.</param>
        /// <returns></returns>
        public List<CodeTable> GetECMatriculationYearByCampus(string campusCodeId) => _academicPlanDA.GetECMatriculationYearByCampus(campusCodeId);

        /// <summary>
        /// Updates the ec academic plan course.
        /// </summary>
        /// <param name="pdcCourse">The PDC course.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public bool UpdateECAcademicPlanCourse(PdcCourse pdcCourse, string operatorId) => _academicPlanDA.UpdateECAcademicPlanCourse(pdcCourse, operatorId);
    }
}