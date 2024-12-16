// --------------------------------------------------------------------
// <copyright file="GlobalInvoiceFiltersForCreation.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// GlobalInvoiceFiltersForCreation
    /// </summary>
    public class GlobalInvoiceFiltersForCreation
    {
        /// <summary>
        /// Gets or sets the academic session.
        /// </summary>
        /// <value>
        /// The academic session.
        /// </value>
        public string AcademicSession { get; set; }

        /// <summary>
        /// Gets or sets the academic term.
        /// </summary>
        /// <value>
        /// The academic term.
        /// </value>
        public string AcademicTerm { get; set; }

        /// <summary>
        /// Gets or sets the academic year.
        /// </summary>
        /// <value>
        /// The academic year.
        /// </value>
        public string AcademicYear { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the payment type identifier.
        /// </summary>
        /// <value>
        /// The payment type identifier.
        /// </value>
        public int PaymentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the receipt numbers.
        /// </summary>
        /// <value>
        /// The receipt numbers.
        /// </value>
        public List<int> ReceiptNumbers { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public string StartDate { get; set; }
        public int InvExpeditionId { get; set; }
    }
}