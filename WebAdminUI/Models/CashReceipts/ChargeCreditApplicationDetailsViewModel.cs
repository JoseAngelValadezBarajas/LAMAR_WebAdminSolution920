// --------------------------------------------------------------------
// <copyright file="ChargeCreditApplicationDetailsViewModel.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

namespace WebAdminUI.Models.CashReceipts
{
    /// <summary>
    /// ChargeCreditApplicationDetailsViewModel model view class
    /// Contains the data, formats, labels and validations for the charges credit of a receipt applied
    /// </summary>
    public class ChargeCreditApplicationDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the amount applied to the charge.
        /// </summary>
        /// <value>
        /// The amount applied to the charge.
        /// </value>
        [Display(Name = "lblAmountApplied", ResourceType = typeof(Resources.CashReceiptsResources))]
        public decimal AmountApplied { get; set; }

        /// <summary>
        /// Gets or sets the application date.
        /// </summary>
        /// <value>
        /// The application date.
        /// </value>
        [Display(Name = "lblApplicationDate", ResourceType = typeof(Resources.CashReceiptsResources))]
        public DateTime ApplicationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can create supplement.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can create supplement; otherwise, <c>false</c>.
        /// </value>
        public bool CanCreateSupplement { get; set; }

        /// <summary>
        /// Gets or sets the charge credit desc.
        /// </summary>
        /// <value>
        /// The charge credit desc.
        /// </value>
        [Display(Name = "lblChargeDescription", ResourceType = typeof(Resources.CashReceiptsResources))]
        public string ChargeCreditDesc { get; set; }

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
        /// Gets or sets the total amount of the charge.
        /// </summary>
        /// <value>
        /// The total amount of the charge.
        /// </value>
        [Display(Name = "lblTotalAmount", ResourceType = typeof(Resources.CashReceiptsResources))]
        public decimal TotalAmount { get; set; }
    }
}