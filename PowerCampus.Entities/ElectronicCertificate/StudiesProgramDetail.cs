// --------------------------------------------------------------------
// <copyright file="StudiesProgramDetail.cs" company="Ellucian">
//     Copyright 2020-2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System;

namespace PowerCampus.Entities.ElectronicCertificate
{
    /// <summary>
    /// StudiesProgramDetail
    /// </summary>
    public class StudiesProgramDetail
    {
        /// <summary>
        /// Gets or sets the name of the campus.
        /// </summary>
        /// <value>
        /// The name of the campus.
        /// </value>
        public string CampusName { get; set; }

        /// <summary>
        /// Gets or sets the campus sep identifier.
        /// </summary>
        /// <value>
        /// The campus sep identifier.
        /// </value>
        public string CampusSepId { get; set; }

        /// <summary>
        /// Gets or sets the federal entity catalog mapping.
        /// </summary>
        /// <value>
        /// The federal entity catalog mapping.
        /// </value>
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

        /// <summary>
        /// Gets or sets the studies program major.
        /// </summary>
        /// <value>
        /// The studies program major.
        /// </value>
        public StudiesProgramMajor StudiesProgramMajor { get; set; }

        /// <summary>
        /// Gets or sets the studies program responsible.
        /// </summary>
        /// <value>
        /// The studies program responsible.
        /// </value>
        public StudiesProgramResponsible StudiesProgramResponsible { get; set; }

        /// <summary>
        /// Gets or sets the studies program rvoe.
        /// </summary>
        /// <value>
        /// The studies program rvoe.
        /// </value>
        public StudiesProgramRvoe StudiesProgramRvoe { get; set; }
    }

    /// <summary>
    /// StudiesProgramMajor
    /// </summary>
    public class StudiesProgramMajor
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
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the maximum grade.
        /// </summary>
        /// <value>
        /// The maximum grade.
        /// </value>
        public byte? MaximumGrade { get; set; }

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
        public decimal? MinimumPassingGrade { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the period type long desc.
        /// </summary>
        /// <value>
        /// The period type long desc.
        /// </value>
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
        public string PlanCode { get; set; }

        /// <summary>
        /// Gets or sets the study level.
        /// </summary>
        /// <value>
        /// The study level.
        /// </value>
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
    /// StudiesProgramResponsible
    /// </summary>
    public class StudiesProgramResponsible
    {
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
        /// Gets or sets the job title.
        /// </summary>
        /// <value>
        /// The job title.
        /// </value>
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
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the second surname.
        /// </summary>
        /// <value>
        /// The second surname.
        /// </value>
        public string SecondSurname { get; set; }
    }

    /// <summary>
    /// StudiesProgramRvoe
    /// </summary>
    public class StudiesProgramRvoe
    {
        /// <summary>
        /// Gets or sets the issuing date.
        /// </summary>
        /// <value>
        /// The issuing date.
        /// </value>
        public DateTime? IssuingDate { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        public string Number { get; set; }
    }
}