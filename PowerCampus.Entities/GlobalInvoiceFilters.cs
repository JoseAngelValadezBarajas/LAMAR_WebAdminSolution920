// --------------------------------------------------------------------
// <copyright file="GlobalInvoiceFilters.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.Enum;
using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// GlobalInvoiceFilters
    /// </summary>
    public class GlobalInvoiceFilters
    {
        /// <summary>
        /// Gets or sets the academic periods.
        /// </summary>
        /// <value>
        /// The academic periods.
        /// </value>
        public List<AcademicPeriod> AcademicPeriods { get; set; }

        /// <summary>
        /// Gets or sets the payment types.
        /// </summary>
        /// <value>
        /// The payment types.
        /// </value>
        public List<FiscalRecordCatalog> PaymentTypes { get; set; }
    }
}