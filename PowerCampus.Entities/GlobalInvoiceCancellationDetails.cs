// --------------------------------------------------------------------
// <copyright file="GlobalInvoiceCancellationDetails.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// GlobalInvoiceCancellationDetails
    /// </summary>
    public class GlobalInvoiceCancellationDetails
    {
        /// <summary>
        /// Gets or sets the cash receipts.
        /// </summary>
        /// <value>
        /// The cash receipts of the canceled global invoice.
        /// </value>
        public List<InvoiceCashReceipt> CashReceipts { get; set; }

        /// <summary>
        /// Gets or sets the new invoices.
        /// </summary>
        /// <value>
        /// The new invoices.
        /// </value>
        public List<FiscalRecord> NewInvoices { get; set; }
    }
}