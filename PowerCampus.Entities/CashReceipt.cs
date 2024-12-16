// --------------------------------------------------------------------
// <copyright file="CashReceipt.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.Enum;
using System;
using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// Get or sets information about CashReceipt
    /// </summary>
    public class CashReceipt
    {
        /// <summary>
        /// Gets or sets the name of the cancel reason.
        /// </summary>
        /// <value>
        /// The name of the cancel reason.
        /// </value>
        public CancelReasonName? CancelReasonName { get; set; }

        /// <summary>
        /// Gets or sets the cfdi.
        /// </summary>
        /// <value>
        /// The cfdi.
        /// </value>
        public List<CFDIUsageCatalog> CFDIList { get; set; }

        /// <summary>
        /// Gets or sets the cfdi related.
        /// </summary>
        /// <value>
        /// The cfdi related.
        /// </value>
        public string CFDIRelated { get; set; }

        /// <summary>
        /// Gets or sets the cfdi related identifier.
        /// </summary>
        /// <value>
        /// The cfdi related identifier.
        /// </value>
        public int? CFDIRelatedId { get; set; }

        /// <summary>
        /// Gets or sets the charges to invoice.
        /// </summary>
        /// <value>
        /// The charges to invoice.
        /// </value>
        public List<ChargeCredit> chargesToInvoiceList { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the frequency code.
        /// </summary>
        /// <value>
        /// The frequency code.
        /// </value>
        public string FrequencyCode { get; set; }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        public List<FiscalRecordCatalog> FrequencyList { get; set; }

        /// <summary>
        /// Gets or sets the invoice expedition identifier.
        /// </summary>
        /// <value>
        /// The invoice expedition identifier.
        /// </value>
        public int? InvoiceExpeditionId { get; set; }

        /// <summary>
        /// Gets or sets the issuer default.
        /// </summary>
        /// <value>
        /// The issuer default.
        /// </value>
        public IssuerDefault issuerDefault { get; set; }

        /// <summary>
        /// Gets or sets the issuer taxpayer identifier.
        /// </summary>
        /// <value>
        /// The issuer taxpayer identifier.
        /// </value>
        public string IssuerTaxpayerId { get; set; }

        /// <summary>
        /// Gets or sets the month code.
        /// </summary>
        /// <value>
        /// The month code.
        /// </value>
        public string MonthCode { get; set; }

        /// <summary>
        /// Gets or sets the payment condition.
        /// </summary>
        /// <value>
        /// The payment condition.
        /// </value>
        public string PaymentCondition { get; set; }

        /// <summary>
        /// Gets or sets the payment type identifier.
        /// </summary>
        /// <value>
        /// The payment type identifier.
        /// </value>
        public FiscalRecordCatalog PaymentType { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>
        /// The type of the payment.
        /// </value>
        public List<FiscalRecordCatalog> PaymentTypeList { get; set; }

        /// <summary>
        /// Gets or sets the people org identifier.
        /// </summary>
        /// <value>
        /// The people org identifier.
        /// </value>
        public string peopleOrgId { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the preferred cfdi.
        /// </summary>
        /// <value>
        /// The preferred cfdi.
        /// </value>
        public string PreferredCFDI { get; set; }

        /// <summary>
        /// Gets or sets the preferred receiver email.
        /// </summary>
        /// <value>
        /// The preferred receiver email.
        /// </value>
        public string preferredReceiverEmail { get; set; }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        /// <value>
        /// The receipt number.
        /// </value>
        public int receiptNumber { get; set; }

        /// <summary>
        /// Gets or sets the receiver.
        /// </summary>
        /// <value>
        /// The receiver.
        /// </value>
        public List<Receiver> ReceiverList { get; set; }

        /// <summary>
        /// Gets or sets the receiver payment method default.
        /// </summary>
        /// <value>
        /// The receiver payment method default.
        /// </value>
        public string ReceiverPaymentMethodDefault { get; set; }

        /// <summary>
        /// Gets or sets the receiver payment method.
        /// </summary>
        /// <value>
        /// The receiver payment method.
        /// </value>
        public List<FiscalRecordCatalog> ReceiverPaymentMethodList { get; set; }

        /// <summary>
        /// Gets or sets the record tax regimen.
        /// </summary>
        /// <value>
        /// The record tax regimen.
        /// </value>
        public string RecTaxRegimen { get; set; }

        /// <summary>
        /// Gets or sets the relation type code.
        /// </summary>
        /// <value>
        /// The relation type code.
        /// </value>
        public string RelationTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the relation type desc.
        /// </summary>
        /// <value>
        /// The relation type desc.
        /// </value>
        public string RelationTypeDesc { get; set; }

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
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the sub total.
        /// </summary>
        /// <value>
        /// The sub total.
        /// </value>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Gets or sets the total (sum subtotal with total taxes).
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public decimal Total { get; set; }

        /// <summary>
        /// Gets or sets the total transferred taxes.
        /// </summary>
        /// <value>
        /// The total tt.
        /// </value>
        public decimal TotalTT { get; set; }

        /// <summary>
        /// Gets or sets the UUID.
        /// </summary>
        /// <value>
        /// The UUID.
        /// </value>
        public string UUID { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public string Year { get; set; }
    }
}