// --------------------------------------------------------------------
// <copyright file="PaymentComplement.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------using System;

using System;
using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// PaymentComplement class.
    /// </summary>
    public class PaymentComplement
    {
        /// <summary>
        /// Gets or sets the amount paid.
        /// </summary>
        /// <value>
        /// The amount paid.
        /// </value>
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// Gets or sets the charge credit code.
        /// </summary>
        /// <value>
        /// The charge credit code.
        /// </value>
        public string ChargeCreditCode { get; set; }

        /// <summary>
        /// Gets or sets the charge credit code identifier.
        /// </summary>
        /// <value>
        /// The charge credit code identifier.
        /// </value>
        public int ChargeCreditCodeId { get; set; }

        /// <summary>
        /// Gets or sets the charge credit desc.
        /// </summary>
        /// <value>
        /// The charge credit desc.
        /// </value>
        public string ChargeCreditDesc { get; set; }

        /// <summary>
        /// Gets or sets the type of the factor.
        /// </summary>
        /// <value>
        /// The type of the factor.
        /// </value>
        public string FactorType { get; set; }

        /// <summary>
        /// Gets or sets the installment number.
        /// </summary>
        /// <value>
        /// The installment number.
        /// </value>
        public int InstallmentNumber { get; set; }

        /// <summary>
        /// Gets or sets the outstanding balance amount.
        /// </summary>
        /// <value>
        /// The outstanding balance amount.
        /// </value>
        public decimal OutstandingBalanceAmount { get; set; }

        /// <summary>
        /// Gets or sets the payment date.
        /// </summary>
        /// <value>
        /// The payment date.
        /// </value>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Gets or sets the previous balance amount.
        /// </summary>
        /// <value>
        /// The previous balance amount.
        /// </value>
        public decimal PreviousBalanceAmount { get; set; }

        /// <summary>
        /// Gets or sets the product service desc.
        /// </summary>
        /// <value>
        /// The product service desc.
        /// </value>
        public string ProductServiceDesc { get; set; }

        /// <summary>
        /// Gets or sets the product service key.
        /// </summary>
        /// <value>
        /// The product service key.
        /// </value>
        public string ProductServiceKey { get; set; }

        /// <summary>
        /// Gets or sets the receipt charge credit code identifier.
        /// </summary>
        /// <value>
        /// The receipt charge credit code identifier.
        /// </value>
        public int ReceiptChargeCreditCodeId { get; set; }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        /// <value>
        /// The receipt number.
        /// </value>
        public int ReceiptNumber { get; set; }

        /// <summary>
        /// Gets or sets the subject to tax.
        /// </summary>
        /// <value>
        /// The subject to tax.
        /// </value>
        public string SubjectToTax { get; set; }

        /// <summary>
        /// Gets or sets the total taxes.
        /// </summary>
        /// <value>
        /// The total taxes.
        /// </value>
        public decimal TotalTaxes { get; set; }

        /// <summary>
        /// Gets or sets the transfer rate.
        /// </summary>
        /// <value>
        /// The transfer rate.
        /// </value>
        public decimal TransferRate { get; set; }

        /// <summary>
        /// Gets or sets the unit amount.
        /// </summary>
        /// <value>
        /// The unit amount.
        /// </value>
        public decimal UnitAmount { get; set; }

        /// <summary>
        /// Gets or sets the unity key.
        /// </summary>
        /// <value>
        /// The unity key.
        /// </value>
        public string UnityKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the unity.
        /// </summary>
        /// <value>
        /// The name of the unity.
        /// </value>
        public string UnityName { get; set; }
    }
}