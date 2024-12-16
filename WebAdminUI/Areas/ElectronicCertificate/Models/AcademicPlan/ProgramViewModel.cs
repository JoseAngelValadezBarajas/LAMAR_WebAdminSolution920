// --------------------------------------------------------------------
// <copyright file="ProgramViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Areas.ElectronicCertificate.Models.Shared;
using WebAdminUI.Resources.ElectronicCertificate;

namespace WebAdminUI.Areas.ElectronicCertificate.Models
{
    /// <summary>
    /// PdcCourseViewModel
    /// </summary>
    public class PdcCourseViewModel
    {
        /// <summary>
        /// Gets or sets the academic plan course catalog identifier.
        /// </summary>
        /// <value>
        /// The academic plan course catalog identifier.
        /// </value>
        public int? AcademicPlanCourseCatalogId { get; set; }

        /// <summary>
        /// Gets or sets the classification.
        /// </summary>
        /// <value>
        /// The classification.
        /// </value>
        public string Classification { get; set; }

        /// <summary>
        /// Gets the classification desc.
        /// </summary>
        /// <value>
        /// The classification desc.
        /// </value>
        [Display(Name = "lblClassification", ResourceType = typeof(PdcCourse))]
        public string ClassificationDesc { get; internal set; }

        /// <summary>
        /// Gets or sets the credits.
        /// </summary>
        /// <value>
        /// The credits.
        /// </value>
        public decimal? Credits { get; set; }

        /// <summary>
        /// Gets or sets the credits string.
        /// </summary>
        /// <value>
        /// The credits string.
        /// </value>
        [Display(Name = "lblCredits", ResourceType = typeof(PdcCourse))]
        public string CreditsString { get; set; }

        /// <summary>
        /// Gets or sets the discipline.
        /// </summary>
        /// <value>
        /// The discipline.
        /// </value>
        public string Discipline { get; set; }

        /// <summary>
        /// Gets or sets the discipline.
        /// </summary>
        /// <value>
        /// The discipline.
        /// </value>
        [Display(Name = "lblDiscipline", ResourceType = typeof(PdcCourse))]
        public string DisciplineDesc { get; set; }

        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        /// <value>
        /// The event identifier.
        /// </value>
        [Display(Name = "lblEventId", ResourceType = typeof(PdcCourse))]
        public string EventId { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>
        /// The name of the event.
        /// </value>
        [Display(Name = "lblEventName", ResourceType = typeof(PdcCourse))]
        public string EventName { get; set; }

        /// <summary>
        /// Gets or sets the type of the event sub.
        /// </summary>
        /// <value>
        /// The type of the event sub.
        /// </value>
        [Display(Name = "lblEventSubType", ResourceType = typeof(PdcCourse))]
        public string EventSubType { get; set; }

        /// <summary>
        /// Gets the event sub type desc.
        /// </summary>
        /// <value>
        /// The event sub type desc.
        /// </value>
        public string EventSubTypeDesc { get; internal set; }

        /// <summary>
        /// Gets or sets the sep identifier.
        /// </summary>
        /// <value>
        /// The sep identifier.
        /// </value>
        [Display(Name = "lblSepId", ResourceType = typeof(PdcCourse))]
        public string SepId { get; set; }

        /// <summary>
        /// Gets or sets the subject type identifier.
        /// </summary>
        /// <value>
        /// The subject type identifier.
        /// </value>
        [Display(Name = "lblSubjectTypeId", ResourceType = typeof(PdcCourse))]
        public int SubjectTypeId { get; set; }
    }

    /// <summary>
    /// Program view model
    /// </summary>
    public class ProgramViewModel
    {
        /// <summary>
        /// Gets the campus code identifier.
        /// </summary>
        /// <value>
        /// The campus code identifier.
        /// </value>
        public string CampusCodeId { get; internal set; }

        /// <summary>
        /// Gets or sets the name of the campus.
        /// </summary>
        /// <value>
        /// The name of the campus.
        /// </value>
        public string CampusName { get; set; }

        /// <summary>
        /// Gets or sets the course mapping percent.
        /// </summary>
        /// <value>
        /// The course mapping percent.
        /// </value>
        public decimal CourseMappingPercent { get; set; }

        /// <summary>
        /// Gets or sets the courses.
        /// </summary>
        /// <value>
        /// The courses.
        /// </value>
        public List<PdcCourseViewModel> Courses { get; set; }

        /// <summary>
        /// Gets or sets the major identifier.
        /// </summary>
        /// <value>
        /// The major identifier.
        /// </value>
        public int? MajorId { get; set; }

        /// <summary>
        /// Gets or sets the matric term.
        /// </summary>
        /// <value>
        /// The matric term.
        /// </value>
        public string MatricTerm { get; set; }

        /// <summary>
        /// Gets or sets the matric term desc.
        /// </summary>
        /// <value>
        /// The matric term desc.
        /// </value>
        public string MatricTermDesc { get; set; }

        /// <summary>
        /// Gets or sets the matric year.
        /// </summary>
        /// <value>
        /// The matric year.
        /// </value>
        public string MatricYear { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the rvoe.
        /// </summary>
        /// <value>
        /// The rvoe.
        /// </value>
        public RvoeViewModel RVOE { get; set; }

        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        /// <value>
        /// The types.
        /// </value>
        public List<DropdownListViewModel> Types { get; set; }
    }

    /// <summary>
    /// RvoeViewModel
    /// </summary>
    public class RvoeViewModel
    {
        /// <summary>
        /// Gets the course mapping percent.
        /// </summary>
        /// <value>
        /// The course mapping percent.
        /// </value>
        public decimal CourseMappingPercent { get; internal set; }

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
        public string IssuingDate { get; set; }

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
        /// Gets the rvoe agreement.
        /// </summary>
        /// <value>
        /// The rvoe agreement.
        /// </value>
        public string RvoeAgreement { get; internal set; }
    }
}