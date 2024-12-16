// --------------------------------------------------------------------
// <copyright file="CertificateInfo.cs" company="Ellucian">
//     Copyright 2020-2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace PowerCampus.Entities.ElectronicCertificate
{
    /// <summary>
    /// The input model to create an ElectronicCerticate record
    /// </summary>
    public class CertificateInfo
    {
        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        public string Folio { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public string OperatorId { get; set; }

        #region Student

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the curp.
        /// </summary>
        /// <value>
        /// The curp.
        /// </value>
        public string Curp { get; set; }

        /// <summary>
        /// Gets or sets the first surname.
        /// </summary>
        /// <value>
        /// The first surname.
        /// </value>
        public string FirstSurname { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the gender identifier.
        /// </summary>
        /// <value>
        /// The gender identifier.
        /// </value>
        public string GenderId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the people identifier.
        /// </summary>
        /// <value>
        /// The people identifier.
        /// </value>
        public string PeopleId { get; set; }

        /// <summary>
        /// Gets or sets the second surname.
        /// </summary>
        /// <value>
        /// The second surname.
        /// </value>
        public string SecondSurname { get; set; }

        #endregion Student

        #region Institution

        /// <summary>
        /// Gets or sets the name of the campus.
        /// </summary>
        /// <value>
        /// The name of the campus.
        /// </value>
        public string CampusName { get; set; }

        /// <summary>
        /// Gets or sets the campus sep code.
        /// </summary>
        /// <value>
        /// The campus sep code.
        /// </value>
        public string CampusSepCode { get; set; }

        /// <summary>
        /// Gets or sets the federal entity.
        /// </summary>
        /// <value>
        /// The federal entity.
        /// </value>
        public string FederalEntity { get; set; }

        /// <summary>
        /// Gets or sets the federal entity code.
        /// </summary>
        /// <value>
        /// The federal entity code.
        /// </value>
        public string FederalEntityCode { get; set; }

        /// <summary>
        /// Gets or sets the federal entity short desc.
        /// </summary>
        /// <value>
        /// The federal entity short desc.
        /// </value>
        public string FederalEntityShortDesc { get; set; }

        /// <summary>
        /// Gets or sets the name of the institution.
        /// </summary>
        /// <value>
        /// The name of the institution.
        /// </value>
        public string InstitutionName { get; set; }

        /// <summary>
        /// Gets or sets the institution sep identifier.
        /// </summary>
        /// <value>
        /// The institution sep identifier.
        /// </value>
        public string InstitutionSepId { get; set; }

        /// <summary>
        /// Gets or sets the signing institution identifier.
        /// </summary>
        /// <value>
        /// The signing institution identifier.
        /// </value>
        public string SigningInstitutionId { get; set; }

        #endregion Institution

        #region Responsible

        /// <summary>
        /// Gets or sets the responsible curp.
        /// </summary>
        /// <value>
        /// The responsible curp.
        /// </value>
        public string ResponsibleCurp { get; set; }

        /// <summary>
        /// Gets or sets the responsible first surname.
        /// </summary>
        /// <value>
        /// The responsible first surname.
        /// </value>
        public string ResponsibleFirstSurname { get; set; }

        /// <summary>
        /// Gets or sets the responsible job title.
        /// </summary>
        /// <value>
        /// The responsible job title.
        /// </value>
        public string ResponsibleJobTitle { get; set; }

        /// <summary>
        /// Gets or sets the responsible job title identifier.
        /// </summary>
        /// <value>
        /// The responsible job title identifier.
        /// </value>
        public string ResponsibleJobTitleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the responsible.
        /// </summary>
        /// <value>
        /// The name of the responsible.
        /// </value>
        public string ResponsibleName { get; set; }

        /// <summary>
        /// Gets or sets the responsible second surname.
        /// </summary>
        /// <value>
        /// The responsible second surname.
        /// </value>
        public string ResponsibleSecondSurname { get; set; }

        #endregion Responsible

        #region Major

        /// <summary>
        /// Gets or sets the major code.
        /// </summary>
        /// <value>
        /// The major code.
        /// </value>
        public string MajorCode { get; set; }

        /// <summary>
        /// Gets or sets the major identifier.
        /// </summary>
        /// <value>
        /// The major identifier.
        /// </value>
        public int MajorId { get; set; }

        /// <summary>
        /// Gets or sets the name of the major.
        /// </summary>
        /// <value>
        /// The name of the major.
        /// </value>
        public string MajorName { get; set; }

        /// <summary>
        /// Gets or sets the maximum grade.
        /// </summary>
        /// <value>
        /// The maximum grade.
        /// </value>
        public byte MaximumGrade { get; set; }

        /// <summary>
        /// Gets or sets the minimum grade.
        /// </summary>
        /// <value>
        /// The minimum grade.
        /// </value>
        public byte MinimumGrade { get; set; }

        /// <summary>
        /// Gets or sets the minimum passing grade.
        /// </summary>
        /// <value>
        /// The minimum passing grade.
        /// </value>
        public decimal MinimumPassingGrade { get; set; }

        /// <summary>
        /// Gets or sets the type of the period.
        /// </summary>
        /// <value>
        /// The type of the period.
        /// </value>
        public string PeriodType { get; set; }

        /// <summary>
        /// Gets or sets the period type identifier.
        /// </summary>
        /// <value>
        /// The period type identifier.
        /// </value>
        public string PeriodTypeId { get; set; }

        /// <summary>
        /// Gets or sets the period type short desc.
        /// </summary>
        /// <value>
        /// The period type short desc.
        /// </value>
        public string PeriodTypeShortDesc { get; set; }

        /// <summary>
        /// Gets or sets the plan code.
        /// </summary>
        /// <value>
        /// The plan code.
        /// </value>
        public string PlanCode { get; set; }

        /// <summary>
        /// Gets or sets the study level.
        /// </summary>
        /// <value>
        /// The study level.
        /// </value>
        public string StudyLevel { get; set; }

        /// <summary>
        /// Gets or sets the study level identifier.
        /// </summary>
        /// <value>
        /// The study level identifier.
        /// </value>
        public string StudyLevelId { get; set; }

        #endregion Major

        #region Rvoe

        /// <summary>
        /// Gets or sets the expedition date.
        /// </summary>
        /// <value>
        /// The expedition date.
        /// </value>
        public DateTime? ExpeditionDate { get; set; }

        /// <summary>
        /// Gets or sets the rvoe agreement number.
        /// </summary>
        /// <value>
        /// The rvoe agreement number.
        /// </value>
        public string RvoeAgreementNumber { get; set; }

        #endregion Rvoe

        #region Courses

        /// <summary>
        /// Gets or sets the course assigned.
        /// </summary>
        /// <value>
        /// The course assigned.
        /// </value>
        public int CourseAssigned { get; set; }

        /// <summary>
        /// Gets or sets the courses.
        /// </summary>
        /// <value>
        /// The courses.
        /// </value>
        public List<CertificateCourseInfo> Courses { get; set; }

        /// <summary>
        /// Gets or sets the credit earned.
        /// </summary>
        /// <value>
        /// The credit earned.
        /// </value>
        public decimal? CreditEarned { get; set; }

        /// <summary>
        /// Gets or sets the gpa.
        /// </summary>
        /// <value>
        /// The gpa.
        /// </value>
        public decimal GPA { get; set; }

        /// <summary>
        /// Gets or sets the total course.
        /// </summary>
        /// <value>
        /// The total course.
        /// </value>
        public int TotalCourse { get; set; }

        /// <summary>
        /// Gets or sets the total credit.
        /// </summary>
        /// <value>
        /// The total credit.
        /// </value>
        public decimal? TotalCredit { get; set; }

        /// <summary>
        /// Gets or sets the total cycle.
        /// </summary>
        /// <value>
        /// The total cycle.
        /// </value>
        public int TotalCycle { get; set; }

        #endregion Courses

        #region Issuing Certificate

        /// <summary>
        /// Gets or sets the average.
        /// </summary>
        /// <value>
        /// The average.
        /// </value>
        public decimal Average { get; set; }

        /// <summary>
        /// Gets or sets the type of the certification.
        /// </summary>
        /// <value>
        /// The type of the certification.
        /// </value>
        public string CertificationType { get; set; }

        /// <summary>
        /// Gets or sets the certification type identifier.
        /// </summary>
        /// <value>
        /// The certification type identifier.
        /// </value>
        public string CertificationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the issuing date.
        /// </summary>
        /// <value>
        /// The issuing date.
        /// </value>
        public DateTime? IssuingDate { get; set; }

        /// <summary>
        /// Gets or sets the issuing federal entity.
        /// </summary>
        /// <value>
        /// The issuing federal entity.
        /// </value>
        public string IssuingFederalEntity { get; set; }

        /// <summary>
        /// Gets or sets the issuing federal entity code.
        /// </summary>
        /// <value>
        /// The issuing federal entity code.
        /// </value>
        public string IssuingFederalEntityCode { get; set; }

        /// <summary>
        /// Gets or sets the issuing federal short desc.
        /// </summary>
        /// <value>
        /// The issuing federal short desc.
        /// </value>
        public string IssuingFederalShortDesc { get; set; }

        #endregion Issuing Certificate
    }
}