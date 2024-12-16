// --------------------------------------------------------------------
// <copyright file="CoursesViewModel.cs" company="Ellucian">
//     Copyright 2020-2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.ElectronicCertificate;

namespace WebAdminUI.Areas.ElectronicCertificate.Models.Generation
{
    /// <summary>
    /// CoursesDetailViewModel
    /// </summary>
    public class CoursesDetailViewModel
    {
        /// <summary>
        /// Gets or sets the course code.
        /// </summary>
        /// <value>
        /// The course code.
        /// </value>
        public string CourseCode { get; set; }

        /// <summary>
        /// Gets or sets the course cycle.
        /// </summary>
        /// <value>
        /// The course cycle.
        /// </value>
        public string CourseCycle { get; set; }

        /// <summary>
        /// Gets or sets the course cycle identifier.
        /// </summary>
        /// <value>
        /// The course cycle identifier.
        /// </value>
        public int CourseCycleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the course.
        /// </summary>
        /// <value>
        /// The name of the course.
        /// </value>
        public string CourseName { get; set; }

        /// <summary>
        /// Gets or sets the credit.
        /// </summary>
        /// <value>
        /// The credit.
        /// </value>
        public decimal Credit { get; set; }

        /// <summary>
        /// Gets or sets the type of the credit.
        /// </summary>
        /// <value>
        /// The type of the credit.
        /// </value>
        public string CreditType { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        public string EventType { get; set; }

        /// <summary>
        /// Gets or sets the final grade.
        /// </summary>
        /// <value>
        /// The final grade.
        /// </value>
        public decimal FinalGrade { get; set; }

        /// <summary>
        /// Gets or sets the grade remark identifier.
        /// </summary>
        /// <value>
        /// The grade remark identifier.
        /// </value>
        public int GradeRemarkId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is include.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is include; otherwise, <c>false</c>.
        /// </value>
        public bool IsInclude { get; set; }

        /// <summary>
        /// Gets or sets the section.
        /// </summary>
        /// <value>
        /// The section.
        /// </value>
        public string Section { get; set; }

        /// <summary>
        /// Gets or sets the sep identifier.
        /// </summary>
        /// <value>
        /// The sep identifier.
        /// </value>
        public string SepId { get; set; }

        /// <summary>
        /// Gets or sets the subject type identifier.
        /// </summary>
        /// <value>
        /// The subject type identifier.
        /// </value>
        public int SubjectTypeId { get; set; }

        /// <summary>
        /// Gets or sets the transcript detail certificate identifier.
        /// </summary>
        /// <value>
        /// The transcript detail certificate identifier.
        /// </value>
        public int? TranscriptDetailCertificateId { get; set; }

        /// <summary>
        /// Gets or sets the transcript detail identifier.
        /// </summary>
        /// <value>
        /// The transcript detail identifier.
        /// </value>
        public int TranscriptDetailId { get; set; }
    }

    /// <summary>
    /// CoursesViewModel
    /// </summary>
    public class CoursesViewModel
    {
        /// <summary>
        /// Gets or sets the asigned courses.
        /// </summary>
        /// <value>
        /// The asigned courses.
        /// </value>
        [Display(Name = "lblAsignedCourses", ResourceType = typeof(Generate))]
        public int AsignedCourses { get; set; }

        /// <summary>
        /// Gets or sets the average.
        /// </summary>
        /// <value>
        /// The average.
        /// </value>
        [Display(Name = "lblAverage", ResourceType = typeof(Generate))]
        public string Average { get; set; }

        /// <summary>
        /// Gets or sets the courses detail view model.
        /// </summary>
        /// <value>
        /// The courses detail view model.
        /// </value>
        public List<CoursesDetailViewModel> CoursesDetailViewModel { get; set; }

        /// <summary>
        /// Gets or sets the grade remarks.
        /// </summary>
        /// <value>
        /// The grade remarks.
        /// </value>
        public List<DropDownViewModel> GradeRemarks { get; set; }

        /// <summary>
        /// Gets or sets the obtatained credits.
        /// </summary>
        /// <value>
        /// The obtatained credits.
        /// </value>
        [Display(Name = "lblObtainedCredits", ResourceType = typeof(Generate))]
        public decimal ObtainedCredits { get; set; }

        /// <summary>
        /// Gets or sets the subjects.
        /// </summary>
        /// <value>
        /// The subjects.
        /// </value>
        public List<DropDownViewModel> Subjects { get; set; }

        /// <summary>
        /// Gets or sets the total courses.
        /// </summary>
        /// <value>
        /// The total courses.
        /// </value>
        [Display(Name = "lblTotalCourses", ResourceType = typeof(Generate))]
        public decimal TotalCourses { get; set; }

        /// <summary>
        /// Gets or sets the total credits.
        /// </summary>
        /// <value>
        /// The total credits.
        /// </value>
        [Display(Name = "lblTotalCredits", ResourceType = typeof(Generate))]
        public decimal? TotalCredits { get; set; }
    }

    /// <summary>
    /// DropDownViewModel
    /// </summary>
    public class DropDownViewModel
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the desc.
        /// </summary>
        /// <value>
        /// The desc.
        /// </value>
        public string Desc { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
    }
}