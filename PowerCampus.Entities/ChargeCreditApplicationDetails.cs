// --------------------------------------------------------------------
// <copyright file="ChargeCreditApplicationDetails.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System;

namespace PowerCampus.Entities
{
    /// <summary>
    /// ChargeCreditApplicationDetails
    /// Contains all the fields of one charge credit applicated to a receipt
    /// </summary>
    public class ChargeCreditApplicationDetails
    {
        /// <summary>
        /// Gets or sets the amount applied to the charge.
        /// </summary>
        /// <value>
        /// The amount applied to the charge.
        /// </value>
        public decimal AmountApplied { get; set; }

        /// <summary>
        /// Gets or sets the application date.
        /// </summary>
        /// <value>
        /// The application date.
        /// </value>
        public DateTime ApplicationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can create supplement.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can create supplement; otherwise, <c>false</c>.
        /// </value>
        public bool CanCreateSupplement { get; set; }

        /// <summary>
        /// Gets or sets the charge credit code.
        /// </summary>
        /// <value>
        /// The charge credit code.
        /// </value>
        public string ChargeCreditCode { get; set; }

        /// <summary>
        /// Gets or sets the charge credit desc.
        /// </summary>
        /// <value>
        /// The charge credit desc.
        /// </value>
        public string ChargeCreditDesc { get; set; }

        /// <summary>
        /// Gets or sets the charge credit number.
        /// </summary>
        /// <value>
        /// Charge credit number.
        /// </value>
        public int ChargeCreditNumber { get; set; }

        /// <summary>
        /// Gets or sets the invoice header id.
        /// </summary>
        /// <value>
        /// The invoice header id, accept null value.
        /// </value>
        public int? InvoiceHeaderId { get; set; }

        /// <summary>
        /// Gets or sets the invoice header id related.
        /// </summary>
        /// <value>
        /// The invoice header id related, accept null value.
        /// </value>
        public int? InvoiceHeaderIdRelated { get; set; }

        /// <summary>
        /// Gets or sets the payment method of the invoice.
        /// </summary>
        /// <value>
        /// The payment method of the invoice.
        /// </value>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the total amount of the charge.
        /// </summary>
        /// <value>
        /// The total amount of the charge.
        /// </value>
        public decimal TotalAmount { get; set; }
    }
}