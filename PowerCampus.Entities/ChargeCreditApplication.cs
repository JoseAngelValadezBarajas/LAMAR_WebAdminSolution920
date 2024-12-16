// --------------------------------------------------------------------
// <copyright file="ChargeCreditApplication.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// CashReceiptApplication
    /// Contains all the fields of the Cash Receipt application
    /// Here is the access to its charges on a list <seealso cref="ChargeCreditApplicationDetails"/>
    /// </summary>
    public class ChargeCreditApplication
    {
        /// <summary>
        /// Gets or sets the balance amount.
        /// </summary>
        /// <value>
        /// The balance amount.
        /// </value>
        public decimal BalanceAmount { get; set; }

        /// <summary>
        /// Gets or sets if is possible to create invoice.
        /// </summary>
        /// <value>
        /// True if is possible to create invoice, False if not
        /// </value>
        public bool CanCreateInvoice { get; set; }

        /// <summary>
        /// Gets or sets the charge credit number.
        /// </summary>
        /// <value>
        /// Receipt Charge credit number.
        /// </value>
        public int ChargeCreditNumber { get; set; }

        /// <summary>
        /// Gets or sets the charges applied to the receipt.
        /// </summary>
        /// <value>
        /// The charges applied to the receipt.
        /// </value>
        public List<ChargeCreditApplicationDetails> Charges { get; set; }

        /// <summary>
        /// Gets or sets the entry date.
        /// </summary>
        /// <value>
        /// The entry date.
        /// </value>
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
        public int ReceiptNumber { get; set; }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        /// <value>
        /// The total amount.
        /// </value>
        public decimal TotalAmount { get; set; }
    }
}