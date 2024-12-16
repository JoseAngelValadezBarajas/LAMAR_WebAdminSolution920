// --------------------------------------------------------------------
// <copyright file="FiscalRecord.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.Enum;
using System;
using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// enumFiscalRecordStatus
    /// </summary>
    public enum enumFiscalRecordStatus
    {
        Active = 0,
        Canceled = 1,
        RequestedFiscalRecord = 2,
        ProviderCannotCreate = 3,
        ProviderIsCreating = 4,
        ProviderCannotCancel = 5,
        RequestedCancellation = 6,
        ProviderIsCanceling = 7,
        Null = 8
    }

    /// <summary>
    /// enumRelationshipType
    /// </summary>
    public enum enumRelationshipType
    {
        // Without relationship
        Null = 0,

        // 01 - Nota de crédito de los documentos relacionados
        CreditNote = 1,

        // 02 -	Nota de débito de los documentos relacionados
        DebitNote = 2,

        // 03 - Devolución de mercancía sobre facturas o traslados previos
        ReturnMerchandise = 3,

        // 04 - Sustitución de los CFDI previos
        ReplacementCFDI = 4,

        // 05 - Traslados de mercancias facturados previamente
        TransfersInvoiced = 5,

        // 06 -  Factura generada por los traslados previos
        InvoiceByTransfers = 6,

        // 07 - CFDI por aplicación de anticipo
        AdvancePayment = 7
    }

    /// <summary>
    ///
    /// </summary>
    public class FiscalRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FiscalRecord"/> class.
        /// </summary>
        public FiscalRecord()
        {
            Issuer = new Issuer();
            Receiver = new Receiver();
        }

        /// <summary>
        /// Gets or sets the approved datetime.
        /// </summary>
        /// <value>
        /// The approved datetime.
        /// </value>
        public DateTime? ApprovedDatetime { get; set; }

        /// <summary>
        /// Gets or sets whether user can cancel a fiscal record
        /// </summary>
        /// <value>
        /// True if user can cancel a fiscal record, false in another case
        /// </value>
        public bool CanCancelInvoice { get; set; }

        /// <summary>
        /// Gets or sets the cancellation datetime.
        /// </summary>
        /// <value>
        /// The cancellation datetime.
        /// </value>
        public DateTime? CancellationDatetime { get; set; }

        /// <summary>
        /// Gets or sets the cancel reason catalog list.
        /// </summary>
        /// <value>
        /// The cancel reason catalog list.
        /// </value>
        public List<FiscalRecordCancelReason> CancelReasonCatalogList { get; set; }

        /// <summary>
        /// Gets or sets the cancel reason desc.
        /// </summary>
        /// <value>
        /// The cancel reason desc.
        /// </value>
        public string CancelReasonDesc { get; set; }

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
        /// Gets or sets whether user can generate a credit note for the tax record
        /// </summary>
        /// <value>
        /// True if user can generate a credit note, false in another case
        /// </value>
        public bool CanCreateCreditNote { get; set; }

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
        /// Gets or sets the cfdi related substitution.
        /// </summary>
        /// <value>
        /// The cfdi related substitution.
        /// </value>
        public string CFDIRelatedSubstitution { get; set; }

        /// <summary>
        /// Gets or sets the type of the cfdi relation.
        /// </summary>
        /// <value>
        /// The type of the cfdi relation.
        /// </value>
        public string CFDIRelationType { get; set; }

        /// <summary>
        /// Gets or sets the cfdi relation type2.
        /// </summary>
        /// <value>
        /// The cfdi relation type2.
        /// </value>
        public string CFDIRelationType2 { get; set; }

        /// <summary>
        /// Gets or sets the cfdi relation type desc.
        /// </summary>
        /// <value>
        /// The cfdi relation type desc.
        /// </value>
        public string CFDIRelationTypeDesc { get; set; }

        /// <summary>
        /// Gets or sets the cfdi relation type desc 2.
        /// </summary>
        /// <value>
        /// The cfdi relation type desc 2.
        /// </value>
        public string CFDIRelationTypeDesc2 { get; set; }

        /// <summary>
        /// Gets or sets the cfdi usage catalog list.
        /// </summary>
        /// <value>
        /// The cfdi usage catalog list.
        /// </value>
        public List<CFDIUsageCatalog> CFDIUsageCatalogList { get; set; }

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
        /// Gets or sets the charge credit.
        /// </summary>
        /// <value>
        /// The charge credit.
        /// </value>
        public List<ChargeCredit> ChargeCredit { get; set; }

        /// <summary>
        /// Gets or sets the charge credit code identifier.
        /// </summary>
        /// <value>
        /// The charge credit code identifier.
        /// </value>
        public int ChargeCreditCodeId { get; set; }

        /// <summary>
        /// Gets or sets the charge credit code tax identifier.
        /// </summary>
        /// <value>
        /// The charge credit code tax identifier.
        /// </value>
        public int ChargeCreditCodeTaxId { get; set; }

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
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the expedition date time.
        /// </summary>
        /// <value>
        /// The expedition date time.
        /// </value>
        public DateTime? ExpeditionDateTime { get; set; }

        /// <summary>
        /// Gets or sets the fiscal record certificate list (InvoiceCertificate, InvoiceCertificateFile tables)
        /// </summary>
        /// <value>
        /// The fiscal record certificate list.
        /// </value>
        public List<FiscalRecordCertificate> FiscalRecordCertificateList { get; set; }

        /// <summary>
        /// Gets or sets the fiscal record detail list.  (InvoiceDetails table)
        /// </summary>
        /// <value>
        /// The fiscal record detail list.
        /// </value>
        public List<FiscalRecordDetail> FiscalRecordDetailList { get; set; }

        /// <summary>
        /// Gets or sets the type of the fiscal record.
        /// </summary>
        /// <value>
        /// The type of the fiscal record.
        /// </value>
        public string FiscalRecordType { get; set; }

        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        public string Folio { get; set; }

        /// <summary>
        /// Gets or sets the FrequencyCode.
        /// </summary>
        /// <value>
        /// The FrequencyCode.
        /// </value>
        public string Frequency { get; set; }

        /// <summary>
        /// Gets or sets whether fiscal record has a document related
        /// </summary>
        /// <value>
        /// True if fiscal record has a document related, false in another case
        /// </value>
        public bool HasInvoiceRelated { get; set; }

        /// <summary>
        /// Gets or sets the invoice details.
        /// </summary>
        /// <value>
        /// The invoice details.
        /// </value>
        public List<ChargeCredit> InvoiceDetails { get; set; }

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
        /// Gets or sets the invoice status.
        /// </summary>
        /// <value>
        /// The invoice status.
        /// </value>
        public int InvoiceStatus { get; set; }

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
        /// Gets or sets whether is cancellation complete or not
        /// </summary>
        /// <value>
        /// True if the cancellation for reason 04 is complete, false in another case
        /// </value>
        public bool IsCancellationComplete { get; set; }

        /// <summary>
        /// Gets or sets the issuer. (InvoiceOrganization table)
        /// </summary>
        /// <value>
        /// The issuer.
        /// </value>
        public Issuer Issuer { get; set; }

        /// <summary>
        /// Gets or sets the MonthCode.
        /// </summary>
        /// <value>
        /// The MonthCode.
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
        /// Gets or sets the payment method: Available values: PUE/PPD
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
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>
        /// The type of the payment.
        /// </value>
        public List<FiscalRecordCatalog> PaymentTypeList { get; set; }

        /// <summary>
        /// Gets or sets the people org code identifier.
        /// </summary>
        /// <value>
        /// The people org code identifier.
        /// </value>
        public string PeopleOrgCodeId { get; set; }

        /// <summary>
        /// Gets or sets the receiver. (InvoiceTaxpayer table)
        /// </summary>
        /// <value>
        /// The receiver.
        /// </value>
        public Receiver Receiver { get; set; }

        /// <summary>
        /// Gets or sets the relationship type.
        /// </summary>
        /// <value>
        /// The relationship type.
        /// </value>
        public enumRelationshipType RelationshipType { get; set; }

        /// <summary>
        /// Gets or sets the relation type code.
        /// </summary>
        /// <value>
        /// The relation type code.
        /// </value>
        public string RelationTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the relation type code 2.
        /// </summary>
        /// <value>
        /// The relation type code 2.
        /// </value>
        public string RelationTypeCode2 { get; set; }

        /// <summary>
        /// Gets or sets the relation type desc.
        /// </summary>
        /// <value>
        /// The relation type desc.
        /// </value>
        public string RelationTypeDesc { get; set; }

        /// <summary>
        /// Gets or sets the relation type desc 2.
        /// </summary>
        /// <value>
        /// The relation type desc 2.
        /// </value>
        public string RelationTypeDesc2 { get; set; }

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
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public decimal Total { get; set; }

        /// <summary>
        /// Gets or sets the total transfer taxes.
        /// </summary>
        /// <value>
        /// The total transfer taxes.
        /// </value>
        public decimal TotalTransferTaxes { get; set; }

        /// <summary>
        /// Gets or sets the UUID.
        /// </summary>
        /// <value>
        /// The UUID.
        /// </value>
        public string UUID { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the type of the voucher.
        /// </summary>
        /// <value>
        /// The type of the voucher.
        /// </value>
        public string VoucherType { get; set; }

        /// <summary>
        /// Gets or sets the Year
        /// </summary>
        /// <value>
        /// The Year.
        /// </value>
        public string Year { get; set; }

        //TODO MOVER A FiscalRecordCertificate

        #region TODO MOVER A FiscalRecordRequest

        /// <summary>
        /// Gets or sets the error text.
        /// </summary>
        /// <value>
        /// The error text.
        /// </value>
        public string ErrorText { get; set; }

        /// <summary>
        /// Gets or sets the state of the request.
        /// </summary>
        /// <value>
        /// The state of the request.
        /// </value>
        public enumFiscalRecordStatus RequestState { get; set; }

        /// <summary>
        /// Gets or sets the request state identifier.
        /// </summary>
        /// <value>
        /// The request state identifier.
        /// </value>
        public int RequestStateId { get; set; }

        #endregion TODO MOVER A FiscalRecordRequest
    }
}