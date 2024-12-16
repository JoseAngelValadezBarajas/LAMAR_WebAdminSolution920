// --------------------------------------------------------------------
// <copyright file="FiscalRecordDetail.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
namespace PowerCampus.Entities
{
    /// <summary>
    /// Instance of InvoiceDetail table.
    /// </summary>
    public class FiscalRecordDetail
    {
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can create credit note.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can create credit note; otherwise, <c>false</c>.
        /// </value>
        public bool CanCreateCreditNote { get; set; }

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
        /// Gets or sets the charge credit number.
        /// </summary>
        /// <value>
        /// The charge credit number.
        /// </value>
        public int ChargeCreditNumber { get; set; }

        /// <summary>
        /// Gets or sets the charge credit source.
        /// </summary>
        /// <value>
        /// The charge credit source.
        /// </value>
        public int? ChargeCreditSource { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the invoice detail identifier.
        /// </summary>
        /// <value>
        /// The invoice detail identifier.
        /// </value>
        public int InvoiceDetailId { get; set; }

        /// <summary>
        /// Gets or sets the invoice header identifier.
        /// </summary>
        /// <value>
        /// The invoice header identifier.
        /// </value>
        public int InvoiceHeaderId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a global credit note.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is a global credit note; otherwise, <c>false</c>.
        /// </value>
        public bool IsAGlobalCreditNote { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is appd tax credit note.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is appd tax credit note; otherwise, <c>false</c>.
        /// </value>
        public bool IsAPPDTaxCreditNote { get; set; }

        /// <summary>
        /// Gets or sets the is a tax.
        /// </summary>
        /// <value>
        /// The is a tax.
        /// </value>
        public int IsATax { get; set; }

        /// <summary>
        /// Gets or sets the people org code identifier.
        /// </summary>
        /// <value>
        /// The people org code identifier.
        /// </value>
        public string PeopleOrgCodeId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the people org.
        /// </summary>
        /// <value>
        /// The full name of the people org.
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
        /// Gets or sets the quantity.
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
        /// Gets or sets Subject to tax code.
        /// </summary>
        /// <value>
        /// The Subject to tax code.
        /// </value>
        public string SubjectToTax { get; set; }

        /// <summary>
        /// Gets or sets the tax code.
        /// </summary>
        /// <value>
        /// The tax code.
        /// </value>
        public string TaxCode { get; set; }

        /// <summary>
        /// Gets or sets the type of the tax factor.
        /// </summary>
        /// <value>
        /// The type of the tax factor.
        /// </value>
        public string TaxFactorType { get; set; }

        /// <summary>
        /// Gets or sets the total taxes.
        /// </summary>
        /// <value>
        /// The total taxes.
        /// </value>
        public decimal TotalTaxes { get; set; }

        /// <summary>
        /// Gets or sets the transfer amount.
        /// </summary>
        /// <value>
        /// The transfer amount.
        /// </value>
        public decimal TransferAmount { get; set; }

        /// <summary>
        /// Gets or sets the transfer rate.
        /// </summary>
        /// <value>
        /// The transfer rate.
        /// </value>
        public decimal? TransferRate { get; set; }

        /// <summary>
        /// Gets or sets the type of the transfer tax.
        /// </summary>
        /// <value>
        /// The type of the transfer tax.
        /// </value>
        public string TransferTaxType { get; set; }

        /// <summary>
        /// Gets or sets the unit amount.  InvoiceDetail.UnitValue column
        /// </summary>
        /// <value>
        /// The unit amount.
        /// </value>
        public decimal UnitAmount { get; set; }

        /// <summary>
        /// Gets or sets the unit description.
        /// </summary>
        /// <value>
        /// The unit description.
        /// </value>
        public string UnitDescription { get; set; }

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