// --------------------------------------------------------------------
// <copyright file="AcademicPlanCourse.cs" company="Ellucian">
//     Copyright 2020-2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

namespace PowerCampus.Entities.ElectronicCertificate
{
    /// <summary>
    /// AcademicPlanCourse
    /// </summary>
    public class AcademicPlanCourse
    {
        /// <summary>
        /// Gets or sets the course details.
        /// </summary>
        /// <value>
        /// The course details.
        /// </value>
        public List<AcademicPlanCourseDetail> CourseDetails { get; set; }

        /// <summary>
        /// Gets or sets the grade remarks.
        /// </summary>
        /// <value>
        /// The grade remarks.
        /// </value>
        public List<DropDownSource> GradeRemarks { get; set; }

        /// <summary>
        /// Gets or sets the subjects.
        /// </summary>
        /// <value>
        /// The subjects.
        /// </value>
        public List<DropDownSource> Subjects { get; set; }

        /// <summary>
        /// Gets or sets the total courses.
        /// </summary>
        /// <value>
        /// The total courses.
        /// </value>
        public int TotalCourses { get; set; }

        /// <summary>
        /// Gets or sets the total credits.
        /// </summary>
        /// <value>
        /// The total credits.
        /// </value>
        public decimal? TotalCredits { get; set; }
    }

    /// <summary>
    /// AcademicPlanCourseDetail
    /// </summary>
    public class AcademicPlanCourseDetail
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
        /// Gets or sets a value indicating whether [inlcude in temporal table].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [inlcude in temporal table]; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeInTemporalTable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is include.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is include; otherwise, <c>false</c>.
        /// </value>
        public bool IsInclude { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public string OperatorId { get; set; }

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
    /// DropDownSource
    /// </summary>
    public class DropDownSource
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

    /// <summary>
    /// TranscriptDetailCertificate
    /// </summary>
    public class TranscriptDetailCertificate
    {
        /// <summary>
        /// Gets or sets the courses to insert.
        /// </summary>
        /// <value>
        /// The courses to insert.
        /// </value>
        public List<AcademicPlanCourseDetail> CoursesToInsert { get; set; }

        /// <summary>
        /// Gets or sets the courses to update.
        /// </summary>
        /// <value>
        /// The courses to update.
        /// </value>
        public List<AcademicPlanCourseDetail> CoursesToUpdate { get; set; }
    }
}