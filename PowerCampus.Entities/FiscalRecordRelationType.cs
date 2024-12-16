// --------------------------------------------------------------------
// <copyright file="TaxRegimenCatalog.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System;

namespace PowerCampus.Entities
{
    /// <summary>
    /// FiscalRecordRelationType class.
    /// </summary>
    /// <seealso cref="FiscalRecordCatalog" />
    public class FiscalRecordRelationType : FiscalRecordCatalog
    {
        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }
    }
}