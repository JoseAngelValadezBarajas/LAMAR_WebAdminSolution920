// --------------------------------------------------------------------
// <copyright file="PdcRvoe.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace PowerCampus.Entities.ElectronicCertificate
{
    /// <summary>
    /// PdcCourse
    /// </summary>
    public class PdcCourse
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
        /// Gets or sets the classification desc.
        /// </summary>
        /// <value>
        /// The classification desc.
        /// </value>
        public string ClassificationDesc { get; set; }

        /// <summary>
        /// Gets or sets the credits.
        /// </summary>
        /// <value>
        /// The credits.
        /// </value>
        public decimal? Credits { get; set; }

        /// <summary>
        /// Gets or sets the discipline.
        /// </summary>
        /// <value>
        /// The discipline.
        /// </value>
        public string Discipline { get; set; }

        /// <summary>
        /// Gets or sets the discipline desc.
        /// </summary>
        /// <value>
        /// The discipline desc.
        /// </value>
        public string DisciplineDesc { get; set; }

        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        /// <value>
        /// The event identifier.
        /// </value>
        public string EventId { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>
        /// The name of the event.
        /// </value>
        public string EventName { get; set; }

        /// <summary>
        /// Gets or sets the type of the event sub.
        /// </summary>
        /// <value>
        /// The type of the event sub.
        /// </value>
        public string EventSubType { get; set; }

        /// <summary>
        /// Gets or sets the event sub type desc.
        /// </summary>
        /// <value>
        /// The event sub type desc.
        /// </value>
        public string EventSubTypeDesc { get; set; }

        /// <summary>
        /// Gets or sets the rvoe identifier.
        /// </summary>
        /// <value>
        /// The rvoe identifier.
        /// </value>
        public int RvoeId { get; set; }

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
    }

    /// <summary>
    /// PdcCourses
    /// </summary>
    public class PdcCourses
    {
        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public string OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the institution campus.
        /// </summary>
        /// <value>
        /// The institution campus.
        /// </value>
        public List<PdcCourse> PdcCourse { get; set; }
    }

    /// <summary>
    /// PdcRvoe
    /// </summary>
    public class PdcRvoe
    {
        /// <summary>
        /// Gets or sets the campus code identifier.
        /// </summary>
        /// <value>
        /// The campus code identifier.
        /// </value>
        public string CampusCodeId { get; set; }

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
        public List<PdcCourse> Courses { get; set; }

        /// <summary>
        /// Gets or sets the issuing date.
        /// </summary>
        /// <value>
        /// The issuing date.
        /// </value>
        public DateTime IssuingDate { get; set; }

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
        /// Gets or sets the name of the PDC.
        /// </summary>
        /// <value>
        /// The name of the PDC.
        /// </value>
        public string PDCName { get; set; }

        /// <summary>
        /// Gets or sets the rvoe agreement.
        /// </summary>
        /// <value>
        /// The rvoe agreement.
        /// </value>
        public string RvoeAgreement { get; set; }

        /// <summary>
        /// Gets or sets the rvoe identifier.
        /// </summary>
        /// <value>
        /// The rvoe identifier.
        /// </value>
        public int RvoeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the subject.
        /// </summary>
        /// <value>
        /// The type of the subject.
        /// </value>
        public List<CodeTable> SubjectType { get; set; }
    }
}