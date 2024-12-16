// --------------------------------------------------------------------
// <copyright file="AcademicPeriod.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.Enum;
using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// AcademicPeriod
    /// </summary>
    public class AcademicPeriod
    {
        /// <summary>
        /// Gets or sets the academic session.
        /// </summary>
        /// <value>
        /// The academic session.
        /// </value>
        public string Session { get; set; }

        /// <summary>
        /// Gets or sets the academic session description.
        /// </summary>
        /// <value>
        /// The academic session description.
        /// </value>
        public string SessionDesc { get; set; }

        /// <summary>
        /// Gets or sets the academic session sort order.
        /// </summary>
        /// <value>
        /// The academic session sort order.
        /// </value>
        public string SessionSortOrder { get; set; }

        /// <summary>
        /// Gets or sets the academic term.
        /// </summary>
        /// <value>
        /// The academic term.
        /// </value>
        public string Term { get; set; }

        /// <summary>
        /// Gets or sets the academic term description.
        /// </summary>
        /// <value>
        /// The academic term description.
        /// </value>
        public string TermDesc { get; set; }

        /// <summary>
        /// Gets or sets the academic term sort order.
        /// </summary>
        /// <value>
        /// The academic term sort order.
        /// </value>
        public string TermSortOrder { get; set; }

        /// <summary>
        /// Gets or sets the academic year.
        /// </summary>
        /// <value>
        /// The academic year.
        /// </value>
        public string Year { get; set; }
    }
}