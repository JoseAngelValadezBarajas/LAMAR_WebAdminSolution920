// --------------------------------------------------------------------
// <copyright file="GlobalInvoiceDetailViewModel.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Collections.Generic;

namespace WebAdminUI.Models.FiscalRecords
{
    /// <summary>
    /// GlobalInvoiceDetailViewModel
    /// </summary>
    public class GlobalInvoiceDetailViewModel
    {
        /// <summary>
        /// Gets or sets the cash receipts.
        /// </summary>
        /// <value>
        /// The cash receipts of the canceled global invoice.
        /// </value>
        public List<InvoiceCashReceiptViewModel> CashReceipts { get; set; }

        /// <summary>
        /// Gets or sets the new invoices.
        /// </summary>
        /// <value>
        /// The new invoices.
        /// </value>
        public List<FiscalRecordViewModel> NewInvoices { get; set; }
    }
}