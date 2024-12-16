// --------------------------------------------------------------------
// <copyright file="ChargeCreditApplicationViewModel.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAdminUI.Models.CashReceipts
{
    /// <summary>
    /// ChargeCreditApplicationViewModel model view class
    /// Contains the data, formats, labels and validations for the receipt applied
    /// Here is the access to its charges on a list <seealso cref="ChargeCreditApplicationDetailsViewModel"/>
    /// </summary>
    public class ChargeCreditApplicationViewModel
    {
        /// <summary>
        /// Gets or sets the balance amount.
        /// </summary>
        /// <value>
        /// The balance amount.
        /// </value>
        [Display(Name = "lblBalanceAmount", ResourceType = typeof(Resources.CashReceiptsResources))]
        public decimal BalanceAmount { get; set; }

        /// <summary>
        /// Gets or sets if is possible to create invoice.
        /// </summary>
        /// <value>
        /// True if is possible to create invoice, False if not
        /// </value>
        public bool CanCreateInvoice { get; set; }

        /// <summary>
        /// Gets or sets the charges without PPD.
        /// </summary>
        /// <value>
        /// The charges whitout PPD.
        /// </value>
        public List<ChargeCreditApplicationDetailsViewModel> Charges { get; set; }

        /// <summary>
        /// Gets or sets the charges with PPD.
        /// </summary>
        /// <value>
        /// The charges whit PPD.
        /// </value>
        public List<ChargeCreditApplicationDetailsViewModel> ChargesWithPPD { get; set; }

        /// <summary>
        /// Gets or sets the entry date.
        /// </summary>
        /// <value>
        /// The entry date.
        /// </value>
        [Display(Name = "lblEntryDate", ResourceType = typeof(Resources.CashReceiptsResources))]
        public DateTime EntryDate { get; set; }

        /// <summary>
        /// Gets or sets if the receipt is reversed or not.
        /// </summary>
        /// <value>
        /// True if the receipt is reversed, False if not
        /// </value>
        public bool IsReversed { get; set; }

        /// <summary>
        /// Gets or sets if the receipt is void or not.
        /// </summary>
        /// <value>
        /// True if the receipt is void, False if not
        /// </value>
        public bool IsVoid { get; set; }

        /// <summary>
        /// Gets or sets the full name of the people org.
        /// </summary>
        /// <value>
        /// The full name of the people org.
        /// </value>
        public string PeopleOrgFullName { get; set; }

        /// <summary>
        /// Gets or sets the people org identifier.
        /// </summary>
        /// <value>
        /// The people org identifier.
        /// </value>
        public string PeopleOrgId { get; set; }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        /// <value>
        /// The receipt number.
        /// </value>
        [Display(Name = "lblReceiptNumber", ResourceType = typeof(Resources.CashReceiptsResources))]
        public int ReceiptNumber { get; set; }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        /// <value>
        /// The total amount.
        /// </value>
        [Display(Name = "lblTotalAmount", ResourceType = typeof(Resources.CashReceiptsResources))]
        public decimal TotalAmount { get; set; }
    }
}