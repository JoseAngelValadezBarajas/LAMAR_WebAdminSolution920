// --------------------------------------------------------------------
// <copyright file="StudiesProgramDetailViewModel.cs" company="Ellucian">
//     Copyright 2020-2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.ElectronicCertificate;

namespace WebAdminUI.Areas.ElectronicCertificate.Models.Generation
{
    /// <summary>
    /// StudiesProgramDetailViewModel
    /// </summary>
    public class StudiesProgramDetailViewModel
    {
        /// <summary>
        /// Gets or sets the name of the campus.
        /// </summary>
        /// <value>
        /// The name of the campus.
        /// </value>
        [Display(Name = "lblCampusName", ResourceType = typeof(Generate))]
        public string CampusName { get; set; }

        /// <summary>
        /// Gets or sets the campus sep identifier.
        /// </summary>
        /// <value>
        /// The campus sep identifier.
        /// </value>
        [Display(Name = "lblCampusSepId", ResourceType = typeof(Generate))]
        public string CampusSepId { get; set; }

        /// <summary>
        /// Gets or sets the federal entity catalog mapping.
        /// </summary>
        /// <value>
        /// The federal entity catalog mapping.
        /// </value>
        [Display(Name = "lblFederalEntityCatalogMapping", ResourceType = typeof(Generate))]
        public string FederalEntityCatalogMapping { get; set; }

        /// <summary>
        /// Gets or sets the federal entity catalog mapping code.
        /// </summary>
        /// <value>
        /// The federal entity catalog mapping code.
        /// </value>
        public int? FederalEntityCatalogMappingCode { get; set; }

        /// <summary>
        /// Gets or sets the federal entity catalog mapping short desc.
        /// </summary>
        /// <value>
        /// The federal entity catalog mapping short desc.
        /// </value>
        public string FederalEntityCatalogMappingShortDesc { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the institution campus identifier.
        /// </summary>
        /// <value>
        /// The institution campus identifier.
        /// </value>
        public int InstitutionCampusId { get; set; }

        /// <summary>
        /// Gets or sets the name of the institution.
        /// </summary>
        /// <value>
        /// The name of the institution.
        /// </value>
        [Display(Name = "lblInstitutionName", ResourceType = typeof(Generate))]
        public string InstitutionName { get; set; }

        /// <summary>
        /// Gets or sets the institution sep identifier.
        /// </summary>
        /// <value>
        /// The institution sep identifier.
        /// </value>
        [Display(Name = "lblInstitutionSepId", ResourceType = typeof(Generate))]
        public string InstitutionSepId { get; set; }

        /// <summary>
        /// Gets or sets the signing institution identifier.
        /// </summary>
        /// <value>
        /// The signing institution identifier.
        /// </value>
        [Display(Name = "lblSigningInstitutionId", ResourceType = typeof(Generate))]
        public string SigningInstitutionId { get; set; }

        /// <summary>
        /// Gets or sets the studies program major.
        /// </summary>
        /// <value>
        /// The studies program major.
        /// </value>
        public StudiesProgramMajorViewModel StudiesProgramMajor { get; set; }

        /// <summary>
        /// Gets or sets the studies program responsible.
        /// </summary>
        /// <value>
        /// The studies program responsible.
        /// </value>
        public StudiesProgramResponsibleViewModel StudiesProgramResponsible { get; set; }

        /// <summary>
        /// Gets or sets the studies program rvoe.
        /// </summary>
        /// <value>
        /// The studies program rvoe.
        /// </value>
        public StudiesProgramRvoeViewModel StudiesProgramRvoe { get; set; }
    }

    /// <summary>
    /// StudiesProgramMajorViewModel
    /// </summary>
    public class StudiesProgramMajorViewModel
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Display(Name = "lblId", ResourceType = typeof(Generate))]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the maximum grade.
        /// </summary>
        /// <value>
        /// The maximum grade.
        /// </value>
        [Display(Name = "lblMaximumGrade", ResourceType = typeof(Generate))]
        public string MaximumGrade { get; set; }

        /// <summary>
        /// Gets or sets the minimum grade.
        /// </summary>
        /// <value>
        /// The minimum grade.
        /// </value>
        [Display(Name = "lblMinimumGrade", ResourceType = typeof(Generate))]
        public string MinimumGrade { get; set; }

        /// <summary>
        /// Gets or sets the minimum passing grade.
        /// </summary>
        /// <value>
        /// The minimum passing grade.
        /// </value>
        [Display(Name = "lblMinimumPassingGrade", ResourceType = typeof(Generate))]
        public string MinimumPassingGrade { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Display(Name = "lblName", ResourceType = typeof(Generate))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the period.
        /// </summary>
        /// <value>
        /// The type of the period.
        /// </value>
        [Display(Name = "lblPeriodType", ResourceType = typeof(Generate))]
        public string PeriodType { get; set; }

        /// <summary>
        /// Gets or sets the period type identifier.
        /// </summary>
        /// <value>
        /// The period type identifier.
        /// </value>
        public int? PeriodTypeId { get; set; }

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
        [Display(Name = "lblPlanCode", ResourceType = typeof(Generate))]
        public string PlanCode { get; set; }

        /// <summary>
        /// Gets or sets the study level.
        /// </summary>
        /// <value>
        /// The study level.
        /// </value>
        [Display(Name = "lblStudyLevel", ResourceType = typeof(Generate))]
        public string StudyLevel { get; set; }

        /// <summary>
        /// Gets or sets the study level short desc.
        /// </summary>
        /// <value>
        /// The study level short desc.
        /// </value>
        public string StudyLevelShortDesc { get; set; }
    }

    /// <summary>
    /// StudiesProgramResponsibleViewModel
    /// </summary>
    public class StudiesProgramResponsibleViewModel
    {
        /// <summary>
        /// Gets or sets the curp.
        /// </summary>
        /// <value>
        /// The curp.
        /// </value>
        [Display(Name = "lblCurp", ResourceType = typeof(Generate))]
        public string Curp { get; set; }

        /// <summary>
        /// Gets or sets the first surname.
        /// </summary>
        /// <value>
        /// The first surname.
        /// </value>
        [Display(Name = "lblFirstSurname", ResourceType = typeof(Generate))]
        public string FirstSurname { get; set; }

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        /// <value>
        /// The job title.
        /// </value>
        [Display(Name = "lblJobTitle", ResourceType = typeof(Generate))]
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the job title short desc.
        /// </summary>
        /// <value>
        /// The job title short desc.
        /// </value>
        public string JobTitleShortDesc { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Display(Name = "lblName", ResourceType = typeof(Generate))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the second surname.
        /// </summary>
        /// <value>
        /// The second surname.
        /// </value>
        [Display(Name = "lblSecondSurname", ResourceType = typeof(Generate))]
        public string SecondSurname { get; set; }
    }

    /// <summary>
    /// StudiesProgramRvoeViewModel
    /// </summary>
    public class StudiesProgramRvoeViewModel
    {
        /// <summary>
        /// Gets or sets the issuing date.
        /// </summary>
        /// <value>
        /// The issuing date.
        /// </value>
        [Display(Name = "lblIssuingDate", ResourceType = typeof(Generate))]
        public string IssuingDate { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        [Display(Name = "lblNumber", ResourceType = typeof(Generate))]
        public string Number { get; set; }
    }
}