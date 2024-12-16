// --------------------------------------------------------------------
// <copyright file="InvoiceCashReceipt.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System;

namespace PowerCampus.Entities
{
    /// <summary>
    /// InvoiceCashReceipt
    /// </summary>
    public class InvoiceCashReceipt
    {
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        /// <value>
        /// The receipt number.
        /// </value>
        public DateTime EntryDate { get; set; }

        /// <summary>
        /// Gets or sets the people/organization fullname.
        /// </summary>
        /// <value>
        /// The people/organization fullname.
        /// </value>
        public string PeopleOrgFullName { get; set; }

        /// <summary>
        /// Gets or sets the people organization identifier.
        /// </summary>
        /// <value>
        /// The people organization identifier.
        /// </value>
        public string PeopleOrgId { get; set; }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        /// <value>
        /// The receipt number.
        /// </value>
        public int ReceiptNumber { get; set; }

        /// <summary>
        /// Gets or sets the tax amount.
        /// </summary>
        /// <value>
        /// The tax amount.
        /// </value>
        public decimal TaxAmount { get; set; }
    }
}