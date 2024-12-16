// --------------------------------------------------------------------
// <copyright file="ChargeCredit.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities
{
    /// <summary>
    /// ChargeCreditType Enumerator
    /// </summary>
    public enum ChargeCreditType
    {
        /// <summary>
        /// Charge
        /// </summary>
        C,

        /// <summary>
        /// Debit
        /// </summary>
        D,

        /// <summary>
        /// Receipt
        /// </summary>
        R,

        /// <summary>
        /// Financial
        /// </summary>
        F
    }

    /// <summary>
    /// Creates an instance of CHARGECREDIT table.
    /// </summary>
    public class ChargeCredit
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance can create credit note.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can create credit note; otherwise, <c>false</c>.
        /// </value>
        public bool CanCreateCreditNote { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can create PPD.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can create PPD; otherwise, <c>false</c>.
        /// </value>
        public bool CanCreatePPD { get; set; }

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
        /// Gets or sets the charge number source.   (ChargeCreditNumber column)
        /// </summary>
        /// <value>
        /// The charge number source.
        /// </value>
        public int? ChargeNumberSource { get; set; }

        /// <summary>
        /// Gets or sets the distribution order
        /// </summary>
        /// <value>
        /// The distribution order
        /// </value>
        public string DistributionOrder { get; set; }

        /// <summary>
        /// Gets or sets the entry date.
        /// </summary>
        /// <value>
        /// The entry date.
        /// </value>
        public string EntryDate { get; set; }

        /// <summary>
        /// Gets or sets the invoice detail identifier.
        /// </summary>
        /// <value>
        /// The invoice detail identifier.
        /// </value>
        public int InvoiceDetailId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a tax.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is a tax; otherwise, <c>false</c>.
        /// </value>
        public bool IsATax { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is empty product service key.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is empty product service key; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmptyProductServiceKey { get; set; }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        /// <value>
        /// The payment method.
        /// </value>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>
        /// The type of the payment.
        /// </value>
        public string PaymentType { get; set; }

        /// <summary>
        /// Gets or sets the people/organization code id.
        /// </summary>
        /// <value>
        /// The people/organization code id.
        /// </value>
        public string PeopleOrgCodeId { get; set; }

        /// <summary>
        /// Gets or sets the people org code identifier complete.
        /// </summary>
        /// <value>
        /// The people org code identifier complete.
        /// </value>
        public string PeopleOrgCodeIdComplete { get; set; }

        /// <summary>
        /// Gets or sets the people/organization full name.
        /// </summary>
        /// <value>
        /// The people/organization full name.
        /// </value>
        public string PeopleOrgFullName { get; set; }

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
        /// Gets or sets the quantity of each chargecode.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        /// <value>
        /// The receipt number.
        /// </value>
        public int? ReceiptNumber { get; set; }

        /// <summary>
        /// Gets or sets the subject to tax.
        /// </summary>
        /// <value>
        /// The subject to tax.
        /// </value>
        public string SubjectToTax { get; set; }

        /// <summary>
        /// Gets or sets the tax charge number.
        /// </summary>
        /// <value>
        /// The tax charge number.
        /// </value>
        public int? TaxChargeNumber { get; set; }

        /// <summary>
        /// Gets or sets the tax code.
        /// </summary>
        /// <value>
        /// The tax code.
        /// </value>
        public string TaxCode { get; set; }

        /// <summary>
        /// Gets or sets the total taxes.
        /// </summary>
        /// <value>
        /// The total taxes.
        /// </value>
        public decimal? TotalTaxes { get; set; }

        /// <summary>
        /// Gets or sets the total of Quantity + UnitAmount.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public decimal TotalUnit { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public ChargeCreditType Type { get; set; }

        /// <summary>
        /// Gets or sets the unit amount.
        /// </summary>
        /// <value>
        /// The unit amount.
        /// </value>
        public decimal UnitAmount { get; set; }

        /// <summary>
        /// Gets or sets the unit key.
        /// </summary>
        /// <value>
        /// The unit key.
        /// </value>
        public string UnitKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the unity.
        /// </summary>
        /// <value>
        /// The name of the unity.
        /// </value>
        public string UnityName { get; set; }
    }
}