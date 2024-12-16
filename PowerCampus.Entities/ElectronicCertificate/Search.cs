// --------------------------------------------------------------------
// <copyright file="Search.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities.ElectronicCertificate
{
    /// <summary>
    /// Search
    /// </summary>
    public class Search
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is advanced.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is advanced; otherwise, <c>false</c>.
        /// </value>
        public bool Advanced { get; set; }

        /// <summary>
        /// Gets or sets the campus code identifier.
        /// </summary>
        /// <value>
        /// The campus code identifier.
        /// </value>
        public string CampusCodeId { get; set; }

        /// <summary>
        /// Gets or sets the certification type identifier.
        /// </summary>
        /// <value>
        /// The certification type identifier.
        /// </value>
        public string CertificationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        public string Folio { get; set; }

        /// <summary>
        /// Gets or sets the issuing date.
        /// </summary>
        /// <value>
        /// The issuing date.
        /// </value>
        public string IssuingDate { get; set; }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        /// <value>
        /// The keywords.
        /// </value>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the major identifier.
        /// </summary>
        /// <value>
        /// The major identifier.
        /// </value>
        public int? MajorId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the student keyword.
        /// </summary>
        /// <value>
        /// The student keyword.
        /// </value>
        public string Student { get; set; }
    }
}