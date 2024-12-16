// --------------------------------------------------------------------
// <copyright file="ElectronicCertificateViewModel.cs" company="Ellucian">
//     Copyright 2020-2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Areas.ElectronicCertificate.Models.Shared;
using WebAdminUI.Resources.ElectronicCertificate;

namespace WebAdminUI.Areas.ElectronicCertificate.Models.Generation
{
    /// <summary>
    /// ElectronicCertificateCourseInfoViewModel
    /// </summary>
    public class ElectronicCertificateCourseInfoViewModel
    {
        /// <summary>
        /// Gets or sets the credits.
        /// </summary>
        /// <value>
        /// The credits.
        /// </value>
        public string Credits { get; set; }

        /// <summary>
        /// Gets or sets the cycle.
        /// </summary>
        /// <value>
        /// The cycle.
        /// </value>
        public string Cycle { get; set; }

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        /// <value>
        /// The grade.
        /// </value>
        public string Grade { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the observations.
        /// </summary>
        /// <value>
        /// The observations.
        /// </value>
        public string Observations { get; set; }

        /// <summary>
        /// Gets or sets the sep identifier.
        /// </summary>
        /// <value>
        /// The sep identifier.
        /// </value>
        public string SepId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }
    }

    /// <summary>
    /// Electronic Certificate Header List
    /// </summary>
    public class ElectronicCertificateHeaderList
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance can generate new.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can generate new; otherwise, <c>false</c>.
        /// </value>
        public bool CanGenerateNew { get; set; }

        /// <summary>
        /// Gets or sets the electronic certificate parameters.
        /// </summary>
        /// <value>
        /// The electronic certificate parameters.
        /// </value>
        public ElectronicCertificateParameters ElectronicCertificateParameters { get; set; }
    }

    /// <summary>
    /// ElectronicCertificateDetailViewModel
    /// </summary>
    public class ElectronicCertificateInfoViewModel
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

        #region Student

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        public string BirthDate { get; set; }

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
        /// Gets or sets the major identifier.
        /// </summary>
        /// <value>
        /// The major identifier.
        /// </value>
        public string MajorId { get; set; }

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
        public string MaximumGrade { get; set; }

        /// <summary>
        /// Gets or sets the minimum grade.
        /// </summary>
        /// <value>
        /// The minimum grade.
        /// </value>
        public string MinimumGrade { get; set; }

        /// <summary>
        /// Gets or sets the minimum passing grade.
        /// </summary>
        /// <value>
        /// The minimum passing grade.
        /// </value>
        public string MinimumPassingGrade { get; set; }

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
        public string ExpeditionDate { get; set; }

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
        public string CourseAssigned { get; set; }

        /// <summary>
        /// Gets or sets the courses.
        /// </summary>
        /// <value>
        /// The courses.
        /// </value>
        public List<ElectronicCertificateCourseInfoViewModel> Courses { get; set; }

        /// <summary>
        /// Gets or sets the credit earned.
        /// </summary>
        /// <value>
        /// The credit earned.
        /// </value>
        public string CreditEarned { get; set; }

        /// <summary>
        /// Gets or sets the gpa.
        /// </summary>
        /// <value>
        /// The gpa.
        /// </value>
        public string GPA { get; set; }

        /// <summary>
        /// Gets or sets the total course.
        /// </summary>
        /// <value>
        /// The total course.
        /// </value>
        public string TotalCourse { get; set; }

        /// <summary>
        /// Gets or sets the total credit.
        /// </summary>
        /// <value>
        /// The total credit.
        /// </value>
        public string TotalCredit { get; set; }

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
        /// Gets or sets the course average.
        /// </summary>
        /// <value>
        /// The course average.
        /// </value>
        public string CourseAverage { get; set; }

        /// <summary>
        /// Gets or sets the issuing date.
        /// </summary>
        /// <value>
        /// The issuing date.
        /// </value>
        public string IssuingDate { get; set; }

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

        #endregion Issuing Certificate
    }

    /// <summary>
    /// Electronic Certificate Parameters
    /// </summary>
    public class ElectronicCertificateParameters
    {
        /// <summary>
        /// Gets or sets the campus.
        /// </summary>
        /// <value>
        /// The campus.
        /// </value>
        [Display(Name = "lblCampus", ResourceType = typeof(Parameters))]
        public List<DropdownListViewModel> Campus { get; set; }

        /// <summary>
        /// Gets or sets the type of the certification.
        /// </summary>
        /// <value>
        /// The type of the certification.
        /// </value>
        [Display(Name = "lblCertificationType", ResourceType = typeof(Parameters))]
        public List<DropdownListViewModel> CertificationType { get; set; }

        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        [Display(Name = "lblFolio", ResourceType = typeof(Parameters))]
        public string Folio { get; set; }

        /// <summary>
        /// Gets or sets the issuing date.
        /// </summary>
        /// <value>
        /// The issuing date.
        /// </value>
        [Display(Name = "lblIssuingDate", ResourceType = typeof(Parameters))]
        public string IssuingDate { get; set; }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        /// <value>
        /// The keywords.
        /// </value>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the program.
        /// </summary>
        /// <value>
        /// The program.
        /// </value>
        [Display(Name = "lblProgram", ResourceType = typeof(Parameters))]
        public List<DropdownListViewModel> Program { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show all status].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show all status]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowAllStatus { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [Display(Name = "lblStatus", ResourceType = typeof(Parameters))]
        public List<DropdownListViewModel> Status { get; set; }

        /// <summary>
        /// Gets or sets the student.
        /// </summary>
        /// <value>
        /// The student.
        /// </value>
        [Display(Name = "lblStudent", ResourceType = typeof(Parameters))]
        public string Student { get; set; }
    }

    /// <summary>
    /// ElectronicCertificateViewModel
    /// </summary>
    public class ElectronicCertificateViewModel
    {
        /// <summary>
        /// Gets or sets the major.
        /// </summary>
        /// <value>
        /// The major.
        /// </value>
        [Display(Name = "lblCampus", ResourceType = typeof(GeneratedTable))]
        public string Campus { get; set; }

        /// <summary>
        /// Gets or sets the type of the certification.
        /// </summary>
        /// <value>
        /// The type of the certification.
        /// </value>
        [Display(Name = "lblCertificationType", ResourceType = typeof(GeneratedTable))]
        public string CertificationType { get; set; }

        /// <summary>
        /// Gets or sets the electronic certificate file identifier.
        /// </summary>
        /// <value>
        /// The electronic certificate file identifier.
        /// </value>
        public int ElectronicCertificateFileId { get; set; }

        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        [Display(Name = "lblFolio", ResourceType = typeof(GeneratedTable))]
        public string Folio { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has PDF file.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has PDF file; otherwise, <c>false</c>.
        /// </value>
        public bool HasPdfFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has XML file.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has XML file; otherwise, <c>false</c>.
        /// </value>
        public bool HasXmlFile { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the issuing date.
        /// </summary>
        /// <value>
        /// The issuing date.
        /// </value>
        [Display(Name = "lblIssuingDate", ResourceType = typeof(GeneratedTable))]
        public string IssuingDate { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the payment folio.
        /// </summary>
        /// <value>
        /// The payment folio.
        /// </value>
        public string PaymentFolio { get; set; }

        /// <summary>
        /// Gets the size of the PDF.
        /// </summary>
        /// <value>
        /// The size of the PDF.
        /// </value>
        public string PdfSize { get; set; }

        /// <summary>
        /// Gets or sets the people code identifier.
        /// </summary>
        /// <value>
        /// The people code identifier.
        /// </value>
        public string PeopleCodeId { get; set; }

        /// <summary>
        /// Gets or sets the program.
        /// </summary>
        /// <value>
        /// The program.
        /// </value>
        [Display(Name = "lblProgram", ResourceType = typeof(GeneratedTable))]
        public string Program { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [Display(Name = "lblStatus", ResourceType = typeof(GeneratedTable))]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the name of the student.
        /// </summary>
        /// <value>
        /// The name of the student.
        /// </value>
        [Display(Name = "lblStudent", ResourceType = typeof(GeneratedTable))]
        public string Student { get; set; }

        /// <summary>
        /// Gets the size of the XML.
        /// </summary>
        /// <value>
        /// The size of the XML.
        /// </value>
        public string XmlSize { get; set; }
    }
}