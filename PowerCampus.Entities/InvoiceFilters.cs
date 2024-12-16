// --------------------------------------------------------------------
// <copyright file="InvoiceFilters.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.Enum;
using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// InvoiceFilters
    /// </summary>
    public class InvoiceFilters
    {
        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the fiscal record type.
        /// </summary>
        /// <value>
        /// The fiscal record type.
        /// </value>
        public string FiscalRecordType { get; set; }

        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        public string Folio { get; set; }

        /// <summary>
        /// Gets or sets the keyword.
        /// </summary>
        /// <value>
        /// The keyword.
        /// </value>
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets the people/organization identifier.
        /// </summary>
        /// <value>
        /// The people/organization identifier.
        /// </value>
        public string PeopleOrgCodeId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public enumFiscalRecordStatus Status { get; set; }
    }
}