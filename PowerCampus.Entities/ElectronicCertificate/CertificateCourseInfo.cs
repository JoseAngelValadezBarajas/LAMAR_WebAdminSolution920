// --------------------------------------------------------------------
// <copyright file="CertificateCourseInfo.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities.ElectronicCertificate
{
    /// <summary>
    /// The input model to create an ElectronicCertificateCourse record
    /// </summary>
    public class CertificateCourseInfo
    {
        /// <summary>
        /// Gets or sets the course code.
        /// </summary>
        /// <value>
        /// The course code.
        /// </value>
        public string CourseCode { get; set; }

        /// <summary>
        /// Gets or sets the credits.
        /// </summary>
        /// <value>
        /// The credits.
        /// </value>
        public decimal? Credits { get; set; }

        /// <summary>
        /// Gets or sets the event cycle.
        /// </summary>
        /// <value>
        /// The event cycle.
        /// </value>
        public string EventCycle { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>
        /// The name of the event.
        /// </value>
        public string EventName { get; set; }

        /// <summary>
        /// Gets or sets the final grade.
        /// </summary>
        /// <value>
        /// The final grade.
        /// </value>
        public string FinalGrade { get; set; }

        /// <summary>
        /// Gets or sets the grade remark.
        /// </summary>
        /// <value>
        /// The grade remark.
        /// </value>
        public string GradeRemark { get; set; }

        /// <summary>
        /// Gets or sets the grade remark identifier.
        /// </summary>
        /// <value>
        /// The grade remark identifier.
        /// </value>
        public string GradeRemarkId { get; set; }

        /// <summary>
        /// Gets or sets the grade remark short desc.
        /// </summary>
        /// <value>
        /// The grade remark short desc.
        /// </value>
        public string GradeRemarkShortDesc { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the sep identifier.
        /// </summary>
        /// <value>
        /// The sep identifier.
        /// </value>
        public string SepId { get; set; }

        /// <summary>
        /// Gets or sets the type of the subject.
        /// </summary>
        /// <value>
        /// The type of the subject.
        /// </value>
        public string SubjectType { get; set; }

        /// <summary>
        /// Gets or sets the subject type identifier.
        /// </summary>
        /// <value>
        /// The subject type identifier.
        /// </value>
        public string SubjectTypeId { get; set; }

        /// <summary>
        /// Gets or sets the subject type short desc.
        /// </summary>
        /// <value>
        /// The subject type short desc.
        /// </value>
        public string SubjectTypeShortDesc { get; set; }

        /// <summary>
        /// Gets or sets the transcript detail identifier.
        /// </summary>
        /// <value>
        /// The transcript detail identifier.
        /// </value>
        public int TranscriptDetailId { get; set; }
    }
}