// --------------------------------------------------------------------
// <copyright file="CreateFiscalRecord.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.Enum;
using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// CreateFiscalRecord entity to create a fiscal record
    /// </summary>
    public class CreateFiscalRecord
    {
        /// <summary>
        /// Gets or sets the cancel reason key.
        /// </summary>
        /// <value>
        /// The cancel reason key.
        /// </value>
        public string CancelReasonKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the cancel reason.
        /// </summary>
        /// <value>
        /// The name of the cancel reason.
        /// </value>
        public CancelReasonName? CancelReasonName { get; set; }

        /// <summary>
        /// Gets or sets the cfdi related.
        /// </summary>
        /// <value>
        /// The cfdi related.
        /// </value>
        public string CFDIRelated { get; set; }

        /// <summary>
        /// Gets or sets the cfdi related 2.
        /// </summary>
        /// <value>
        /// The cfdi related 2.
        /// </value>
        public string CFDIRelated2 { get; set; }

        /// <summary>
        /// Gets or sets the cfdi related identifier.
        /// </summary>
        /// <value>
        /// The cfdi related identifier.
        /// </value>
        public int? CFDIRelatedId { get; set; }

        /// <summary>
        /// Gets or sets the cfdi relation type key.
        /// </summary>
        /// <value>
        /// The cfdi relation type key.
        /// </value>
        public string CFDIRelationTypeKey { get; set; }

        /// <summary>
        /// Gets or sets the cfdi relation type key 2.
        /// </summary>
        /// <value>
        /// The cfdi relation type key 2.
        /// </value>
        public string CFDIRelationTypeKey2 { get; set; }

        /// <summary>
        /// Gets or sets the name of the cfdi relation type.
        /// </summary>
        /// <value>
        /// The name of the cfdi relation type.
        /// </value>
        public RelationTypeName? CFDIRelationTypeName { get; set; }

        /// <summary>
        /// Gets or sets the cfdi usage code.
        /// </summary>
        /// <value>
        /// The cfdi usage code.
        /// </value>
        public string CFDIUsageCode { get; set; }

        /// <summary>
        /// Gets or sets the cfdi usage desc.
        /// </summary>
        /// <value>
        /// The cfdi usage desc.
        /// </value>
        public string CFDIUsageDesc { get; set; }

        /// <summary>
        /// Gets or sets the charge credit Id for total amount without taxes.
        /// </summary>
        /// <value>
        /// The charge credit Id for total amount without taxes.
        /// </value>
        public int ChargeCreditId { get; set; }

        /// <summary>
        /// Gets or sets the charge credit Id for total amount of taxes.
        /// </summary>
        /// <value>
        /// The charge credit Id for total amount of taxes.
        /// </value>
        public int ChargeCreditIdTax { get; set; }

        /// <summary>
        /// Gets or sets the charge credit number. Fill this field to generate a PPD based on a CHARGECREDIT.ChargeCreditNumber
        /// </summary>
        /// <value>
        /// The charge credit number.
        /// </value>
        public int? ChargeCreditNumber { get; set; }

        /// <summary>
        /// Gets or sets the expedition location.
        /// </summary>
        /// <value>
        /// The expedition location.
        /// </value>
        public string CityOfIssue { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the detail.
        /// </summary>
        /// <value>
        /// The detail.
        /// </value>
        public List<FiscalRecordDetail> Detail { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the exchange rate.
        /// </summary>
        /// <value>
        /// The exchange rate.
        /// </value>
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Gets or sets the fiscal identity number.
        /// </summary>
        /// <value>
        /// The fiscal identity number.
        /// </value>
        public string FiscalIdentityNumber { get; set; }

        /// <summary>
        /// Gets or sets the type of the fiscal record.
        /// </summary>
        /// <value>
        /// The type of the fiscal record.
        /// </value>
        public string FiscalRecordType { get; set; }

        /// <summary>
        /// Gets or sets the fiscal residency.
        /// </summary>
        /// <value>
        /// The fiscal residency.
        /// </value>
        public string FiscalResidency { get; set; }

        /// <summary>
        /// Gets or sets the fiscal residency desc.
        /// </summary>
        /// <value>
        /// The fiscal residency desc.
        /// </value>
        public string FiscalResidencyDesc { get; set; }

        /// <summary>
        /// Gets or sets the frequency code.
        /// </summary>
        /// <value>
        /// The frequency code.
        /// </value>
        public string FrequencyCode { get; set; }

        /// <summary>
        /// Gets or sets the invoice expedition identifier.
        /// </summary>
        /// <value>
        /// The invoice expedition identifier.
        /// </value>
        public int InvoiceExpeditionId { get; set; }

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
        /// Gets or sets a value indicating whether it is a creation for reason 04.
        /// </summary>
        /// <value>
        ///   <c>true</c> if it is a creation for reason 04; otherwise, <c>false</c>.
        /// </value>
        public bool IsCreationForReason04 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is global.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is global; otherwise, <c>false</c>.
        /// </value>
        public bool IsGlobal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is substitution.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is substitution; otherwise, <c>false</c>.
        /// </value>
        public bool IsSubstitution { get; set; }

        /// <summary>
        /// Gets or sets the issuer tax payer identifier.
        /// </summary>
        /// <value>
        /// The issuer tax payer identifier.
        /// </value>
        public string IssuerTaxPayerId { get; set; }

        /// <summary>
        /// Gets or sets the month code.
        /// </summary>
        /// <value>
        /// The month code.
        /// </value>
        public string MonthCode { get; set; }

        /// <summary>
        /// Gets or sets the payment account number.
        /// </summary>
        /// <value>
        /// The payment account number.
        /// </value>
        public string PaymentAccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the payment condition.
        /// </summary>
        /// <value>
        /// The payment condition.
        /// </value>
        public string PaymentCondition { get; set; }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        /// <value>
        /// The payment method.
        /// </value>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the payment method desc.
        /// </summary>
        /// <value>
        /// The payment method desc.
        /// </value>
        public string PaymentMethodDesc { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>
        /// The type of the payment.
        /// </value>
        public string PaymentType { get; set; }

        /// <summary>
        /// Gets or sets the payment type desc.
        /// </summary>
        /// <value>
        /// The payment type desc.
        /// </value>
        public string PaymentTypeDesc { get; set; }

        /// <summary>
        /// Gets or sets the people org code id.
        /// </summary>
        /// <value>
        /// The people org code id.
        /// </value>
        public string PeopleOrgCodeId { get; set; }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        /// <value>
        /// The receipt number.
        /// </value>
        public int? ReceiptNumber { get; set; }

        /// <summary>
        /// Gets or sets the receipt numbers included for global invoice creation.
        /// </summary>
        /// <value>
        /// The receipt numbers included for global invoice creation.
        /// </value>
        public List<int> ReceiptNumbersIncludedForGlobal { get; set; }

        /// <summary>
        /// Gets or sets the receiver email.
        /// </summary>
        /// <value>
        /// The receiver email.
        /// </value>
        public string ReceiverEmail { get; set; }

        /// <summary>
        /// Gets or sets the reciever tax payer identifier.
        /// </summary>
        /// <value>
        /// The reciever tax payer identifier.
        /// </value>
        public string ReceiverTaxpayerId { get; set; }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        /// <value>
        /// The serial number.
        /// </value>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public string StartDate { get; set; }

        /// <summary>
		/// Gets or sets the sub total.
		/// </summary>
		/// <value>
		/// The sub total.
		/// </value>
		public string Subtotal { get; set; }

        /// <summary>
        /// Gets or sets the tax regimen.
        /// </summary>
        /// <value>
        /// The tax regimen.
        /// </value>
        public string TaxRegimen { get; set; }

        /// <summary>
        /// Gets or sets the tax regimen desc.
        /// </summary>
        /// <value>
        /// The tax regimen desc.
        /// </value>
        public string TaxRegimenDesc { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public string Total { get; set; }

        /// <summary>
        /// Gets or sets the total transfer taxes.
        /// </summary>
        /// <value>
        /// The total transfer taxes.
        /// </value>
        public string TotalTransferTaxes { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public string Year { get; set; }
        public string ExpeditionDate { get; set; }
    }
}