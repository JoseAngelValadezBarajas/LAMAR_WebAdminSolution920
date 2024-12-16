// --------------------------------------------------------------------
// <copyright file="FiscalRecordViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using PowerCampus.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdministration.Models.Shared;
using WebAdminUI.Helpers;
using WebAdminUI.Models.PaymentReceipt;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.FiscalRecords
{
    /// <summary>
    ///
    /// </summary>
    public class FiscalRecordCertificateViewModel
    {
        /// <summary>
        /// Gets or sets the approved date time.
        /// </summary>
        /// <value>
        /// The approved date time.
        /// </value>
        public DateTime? ApprovedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the fiscal record certificate identifier.
        /// </summary>
        /// <value>
        /// The fiscal record certificate identifier.
        /// </value>
        public int? FiscalRecordCertificateId { get; set; }

        /// <summary>
        /// Gets or sets the PDF file.
        /// </summary>
        /// <value>
        /// The PDF file.
        /// </value>
        public byte[] PdfFile { get; set; }

        /// <summary>
        /// Gets or sets the XML file.
        /// </summary>
        /// <value>
        /// The XML file.
        /// </value>
        public string XmlFile { get; set; }
    }

    /// <summary>
    /// FiscalRecordOriginViewModel
    /// </summary>
    public class FiscalRecordOriginViewModel
    {
        /// <summary>
        /// Get te fiscal record origin.
        /// </summary>
        public List<FiscalRecordRelatedViewModel> Origin { get; set; }

        /// <summary>
        /// Get all the parents of the original fiscal record.
        /// </summary>
        public List<FiscalRecordRelatedViewModel> Parent { get; set; }
    }

    /// <summary>
    /// FiscalRecordRelatedViewModel
    /// </summary>
    public class FiscalRecordRelatedViewModel
    {
        /// <summary>
        /// Gets or sets the expedition date time.
        /// </summary>
        /// <value>
        /// The expedition date time.
        /// </value>
        [Display(Name = "lblExpeditionDate", ResourceType = typeof(Resources.FiscalRecordModelResource))]
        public DateTime ExpeditionDateTime { get; set; }

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
        [Display(Name = "lblFolio", ResourceType = typeof(Resources.FiscalRecordModelResource))]
        public string Folio { get; set; }

        /// <summary>
        /// Gets or sets the fiscal record identifier.
        /// </summary>
        /// <value>
        /// The fiscal record identifier.
        /// </value>
        public int InvoiceHeaderId { get; set; }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        /// <value>
        /// The payment method.
        /// </value>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the relationship type.
        /// </summary>
        /// <value>
        /// The relationship type.
        /// </value>
        [Display(Name = "lblRelationshipType", ResourceType = typeof(FiscalRecordModelResource))]
        public string RelationshipType { get; set; }

        /// <summary>
        /// Gets or sets the relationship type 2.
        /// </summary>
        /// <value>
        /// The relationship type.
        /// </value>
        [Display(Name = "lblRelationshipType", ResourceType = typeof(FiscalRecordModelResource))]
        public string RelationshipType2 { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [Display(Name = "lblStatus", ResourceType = typeof(Resources.FiscalRecordModelResource))]
        public string RequestState { get; set; }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        /// <value>
        /// The serial number.
        /// </value>
        [Display(Name = "lblSerial", ResourceType = typeof(Resources.FiscalRecordModelResource))]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        [Display(Name = "lblTotal", ResourceType = typeof(Resources.FiscalRecordModelResource))]
        public decimal Total { get; set; }

        /// <summary>
        /// Gets or sets the UUID.
        /// </summary>
        /// <value>
        /// The UUID.
        /// </value>
        [Display(Name = "lblUUID", ResourceType = typeof(Resources.FiscalRecordModelResource))]
        public string UUID { get; set; }

        /// <summary>
        /// Gets or sets the type of the voucher.
        /// </summary>
        /// <value>
        /// The type of the voucher.
        /// </value>
        [Display(Name = "lblType", ResourceType = typeof(Resources.FiscalRecordModelResource))]
        public string VoucherType { get; set; }
    }

    /// <summary>
    /// FiscalRecordViewModel
    /// </summary>
    public class FiscalRecordViewModel
    {
        /// <summary>
        /// Gets or sets the approved date time.
        /// </summary>
        /// <value>
        /// The approved date time.
        /// </value>
        [Display(Name = "DatetimeOfStamp", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string approvedDateTime { get; set; }

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
        [Display(Name = "DatetimeOfStampCancellation", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string CancellationDatetime { get; set; }

        /// <summary>
        /// Gets or sets the cancel reason.
        /// </summary>
        /// <value>
        /// The cancel reason.
        /// </value>
        [Display(Name = "CancelReason", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string CancelReason { get; set; }

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
        /// Gets or sets the cancel reasons.
        /// </summary>
        /// <value>
        /// The cancel reasons.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource), ErrorMessageResourceName = "lblCancelReasonRequired")]
        [Display(Name = "lblCancellationReason", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public List<ListOptionViewModel> CancelReasons { get; set; }

        /// <summary>
        /// Gets or sets whether user can generate a credit note for the tax record
        /// </summary>
        /// <value>
        /// True if user can generate a credit note, false in another case
        /// </value>
        [Display(Name = "lblAddCreditNote", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public bool CanCreateCreditNote { get; set; }

        /// <summary>
        /// Gets or sets the cfdi related.
        /// </summary>
        /// <value>
        /// The cfdi related.
        /// </value>
        [Display(Name = "CFDIRelated", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string CFDIRelated { get; set; }

        /// <summary>
        /// Gets or sets the cfdi related 2.
        /// </summary>
        /// <value>
        /// The cfdi related 2.
        /// </value>
        [Display(Name = "CFDIRelated", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string CFDIRelated2 { get; set; }

        /// <summary>
        /// Gets or sets the cfdi related identifier.
        /// </summary>
        /// <value>
        /// The cfdi related identifier.
        /// </value>
        public int? CFDIRelatedId { get; set; }

        /// <summary>
        /// Gets or sets the cfdi related sustitution.
        /// </summary>
        /// <value>
        /// The cfdi related sustitution.
        /// </value>
        [Display(Name = "CFDIRelatedSubstitution", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string CFDIRelatedSubstitution { get; set; }

        /// <summary>
        /// Gets or sets the cfdi usage catalog list.
        /// </summary>
        /// <value>
        /// The cfdi usage catalog list.
        /// </value>
        [Display(Name = "lblCFDIUse", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public List<CFDIUsageCatalog> CFDIUsageCatalogList { get; set; }

        /// <summary>
        /// Gets or sets the CFDI Usage Code.
        /// </summary>
        /// <value>
        /// The CFDI Usage Code.
        /// </value>
        public string CFDIUsageCode { get; set; }

        /// <summary>
        /// Gets or sets the CFDI Usage Desc.
        /// </summary>
        /// <value>
        /// The CFDI Usage Code.
        /// </value>
        [Display(Name = "lblCFDIUse", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string CFDIUsageDesc { get; set; }

        /// <summary>
        /// Gets or sets the charge credit.
        /// </summary>
        /// <value>
        /// The charge credit.
        /// </value>
        public List<ChargeCreditViewModel> ChargeCredit { get; set; }

        /// <summary>
        /// Gets or sets the charge credit code identifier.
        /// </summary>
        /// <value>
        /// The charge credit code identifier.
        /// </value>
        [Display(Name = "lblCodeAmountRefund", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public int ChargeCreditCodeId { get; set; }

        /// <summary>
        /// Gets or sets the charge credit code tax identifier.
        /// </summary>
        /// <value>
        /// The charge credit code tax identifier.
        /// </value>
        [Display(Name = "lblCodeTaxRefund", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public int ChargeCreditCodeTaxId { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        [Display(Name = "lblComments", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string comments { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        [Display(Name = "lblCurrency", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string currency { get; set; }

        /// <summary>
        /// Gets or sets end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [Display(Name = "EndDate", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string endDate { get; set; }

        /// <summary>
        /// Gets or sets the error text.
        /// </summary>
        /// <value>
        /// The error text.
        /// </value>
        [Display(Name = "StatusDetails", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string errorText { get; set; }

        /// <summary>
        /// Gets or sets the expedition date time.
        /// </summary>
        /// <value>
        /// The expedition date time.
        /// </value>
        [Display(Name = "DatetimeOfIssue", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string expeditionDateTime { get; set; }

        /// <summary>
        /// Gets or sets the fiscal record certificate list.
        /// </summary>
        /// <value>
        /// The fiscal record certificate list.
        /// </value>
        public List<FiscalRecordCertificateViewModel> FiscalRecordCertificateList { get; set; }

        /// <summary>
        /// Gets or fiscal record status enum.
        /// </summary>
        /// <value>
        /// The fiscal record status enum.
        /// </value>
        public enumFiscalRecordStatus FiscalRecordStatusEnum { get; set; }

        /// <summary>
        /// Gets or sets the type of the fiscal record.
        /// </summary>
        /// <value>
        /// The type of the fiscal record.
        /// </value>
        [Display(Name = "FiscalRecType", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string fiscalRecordType { get; set; }

        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        [Display(Name = "Folio", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string folio { get; set; }

        /// <summary>
        /// Gets or sets the type of the fiscal record.
        /// </summary>
        /// <value>
        /// The type of the fiscal record.
        /// </value>
        [Display(Name = "lblFrequency", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
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
        public List<ChargeCreditViewModel> InvoiceDetails { get; set; }

        /// <summary>
        /// Gets or sets the invoice expedition identifier.
        /// </summary>
        /// <value>
        /// The invoice expedition identifier.
        /// </value>
        public int InvoiceExpeditionId { get; set; }

        /// <summary>
        /// Gets or sets the fiscal record identifier.
        /// </summary>
        /// <value>
        /// The fiscal record identifier.
        /// </value>
        public int InvoiceHeaderId { get; set; }

        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        /// <value>
        /// The requestId.
        /// </value>
        public int invoiceStatus { get; set; }

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
        /// Gets or sets whether fiscal record cancellation is in progress
        /// </summary>
        /// <value>
        /// True if fiscal record cancellation is in progress, false in another case
        /// </value>
        public bool IsCancellationInProgress { get; set; }

        /// <summary>
        /// Gets or sets the iss corporate name.
        /// </summary>
        /// <value>
        /// The iss corporate name.
        /// </value>
        public string issCorporateName { get; set; }

        /// <summary>
        /// Gets or sets the iss issuer invoice organization identifier.
        /// </summary>
        /// <value>
        /// The iss issuer invoice organization identifier.
        /// </value>
        public int? IssIssuerInvoiceOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the iss issuing add iss issuing address.
        /// </summary>
        /// <value>
        /// The iss issuing add iss issuing address.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "IssAddRequired")]
        [Display(Name = "lblIssuerAddress", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string issIssuingAddIssIssuingAddress { get; set; }

        /// <summary>
        /// Gets or sets the iss issuing add iss place issue.
        /// </summary>
        /// <value>
        /// The iss issuing add iss place issue.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "IssPlaceRequired")]
        [Display(Name = "lblIssuerPlaceofIssue", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string issIssuingAddIssPlaceIssue { get; set; }

        /// <summary>
        /// Gets or sets the iss serial.
        /// </summary>
        /// <value>
        /// The iss serial.
        /// </value>
        public List<IssuerSerial> IssSerial { get; set; }

        /// <summary>
        /// Gets or sets the issuer corporate name.
        /// </summary>
        /// <value>
        /// The issuer corporate name.
        /// </value>
        [Display(Name = "lblIssuerCorporateName", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string IssuerCorporateName { get; set; }

        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        /// <value>
        /// The issuer.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml), ErrorMessageResourceName = "IssuerIssTaxpayerIdRequired")]
        [Display(Name = "lblIssuerTaxpayerId", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string issuerIssTaxpayerId { get; set; }

        /// <summary>
        /// Gets or sets the issuer tax regimen code.
        /// </summary>
        /// <value>
        /// The issuer iss tax regimen code.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "IssTaxRegRequired")]
        [Display(Name = "lblIssuerTaxRegimen", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string IssuerTaxRegimen { get; set; }

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The month
        /// </value>
        [Display(Name = "lblMonth", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string Month { get; set; }

        /// <summary>
        /// Gets or sets the payment condition.
        /// </summary>
        /// <value>
        /// The payment condition.
        /// </value>
        [Display(Name = "lblPaymentCondition", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string paymentCondition { get; set; }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        /// <value>
        /// The payment method.
        /// </value>
        [Display(Name = "lblPaymentMethod", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string paymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the payment method desc.
        /// </summary>
        /// <value>
        /// The payment method desc.
        /// </value>
        public string paymentMethodDesc { get; set; }

        /// <summary>
        /// Gets or sets the payment receipt totals.
        /// </summary>
        /// <value>
        /// The payment receipt totals.
        /// </value>
        public PaymentReceiptTotalsViewModel PaymentReceiptTotals { get; set; }

        /// <summary>
        /// Gets or sets the payment type.
        /// </summary>
        /// <value>
        /// The payment type.
        /// </value>
        [Display(Name = "PaymentType", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string paymentType { get; set; }

        /// <summary>
        /// Gets or sets the payment type desc.
        /// </summary>
        /// <value>
        /// The payment type desc.
        /// </value>
        public string paymentTypeDesc { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>
        /// The type of the payment.
        /// </value>
        [Display(Name = "lblPaymentType", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public List<FiscalRecordCatalog> PaymentTypeList { get; set; }

        /// <summary>
        /// Gets or sets the people org code id.
        /// </summary>
        /// <value>
        /// The people org code id.
        /// </value>
        [Display(Name = "PeopleOrganizationID", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string PeopleOrgCodeId { get; set; }

        /// <summary>
        /// Gets or sets the people org code identifier format.
        /// </summary>
        /// <value>
        /// The people org code identifier format.
        /// </value>
        public string PeopleOrgCodeIdFormat { get; set; }

        /// <summary>
        /// Gets or sets the name of the people org.
        /// </summary>
        /// <value>
        /// The name of the people org.
        /// </value>
        public string peopleOrgName { get; set; }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        /// <value>
        /// The receipt number.
        /// </value>
        public int ReceiptNumber { get; set; }

        /// <summary>
        /// Gets or sets the receiver corporate name.
        /// </summary>
        /// <value>
        /// The receiver corporate name.
        /// </value>
        [Display(Name = "lblReceiverCorporateName", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string receiverCorporateName { get; set; }

        /// <summary>
        /// Gets or sets the receiver email.
        /// </summary>
        /// <value>
        /// The receiver email.
        /// </value>
        [EmailAddress(ErrorMessageResourceType = typeof(PeopleViewModelResource), ErrorMessageResourceName = "ValidationMessageEmail", ErrorMessage = null)]
        [Display(Name = nameof(PeopleViewModelResource.Email), ResourceType = typeof(PeopleViewModelResource))]
        [CustomEmailRegularExpression]
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "ReceiverEmail")]
        public string receiverEmail { get; set; }

        /// <summary>
        /// Gets or sets the receiver fiscal identity number.
        /// </summary>
        /// <value>
        /// The receiver fiscal identity number.
        /// </value>
        [Display(Name = "lblReceiverFiscalIdentityNumber", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string receiverFiscalIdentityNumber { get; set; }

        /// <summary>
        /// Gets or sets the receiver fiscal residency.
        /// </summary>
        /// <value>
        /// The receiver fiscal residency.
        /// </value>
        [Display(Name = "lblReceiverFiscalAddress", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string receiverFiscalResidency { get; set; }

        /// <summary>
        /// Gets or sets the receiver fiscal residency desc.
        /// </summary>
        /// <value>
        /// The receiver fiscal residency desc.
        /// </value>
        [Display(Name = "lblReceiverFiscalAddress", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string receiverFiscalResidencyDesc { get; set; }

        /// <summary>
        /// Gets or sets the receiver.
        /// </summary>
        /// <value>
        /// The receiver.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml), ErrorMessageResourceName = "ReceiverTaxPayerIdRequired")]
        [Display(Name = "lblReceiverTaxpayerId", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string receiverTaxPayerId { get; set; }

        /// <summary>
        /// Gets or sets the receiver tax regimen code.
        /// </summary>
        /// <value>
        /// The receiver tax regimen code.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml), ErrorMessageResourceName = "ReceiverTaxRegimenRequired")]
        [Display(Name = "lblReceiverTaxRegimen", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string ReceiverTaxRegimen { get; set; }

        /// <summary>
        /// Gets or sets the issuer tax regimen code.
        /// </summary>
        /// <value>
        /// The issuer iss tax regimen code.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml), ErrorMessageResourceName = "RecTaxAddressRequired")]
        [Display(Name = "RecTaxAddress", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string RecTaxAddress { get; set; }

        /// <summary>
        /// Gets or sets the type of the relation.
        /// </summary>
        /// <value>
        /// The type of the relation.
        /// </value>
        [Display(Name = "RelationshipTypeTitle", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string RelationType { get; set; }

        /// <summary>
        /// Gets or sets the type of the relation 2.
        /// </summary>
        /// <value>
        /// The type of the relation 2.
        /// </value>
        [Display(Name = "RelationshipTypeTitle", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string RelationType2 { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [Display(Name = "Status", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string requestState { get; set; }

        /// <summary>
        /// Gets or sets the request state identifier.
        /// </summary>
        /// <value>
        /// The request state identifier.
        /// </value>
        public int requestStateId { get; set; }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        /// <value>
        /// The serial number.
        /// </value>
        [Display(Name = "lblIssuerSerial", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string serialNumber { get; set; }

        /// <summary>
        /// Gets or sets the stamp date time.
        /// </summary>
        /// <value>
        /// The stamp date time.
        /// </value>
        [Display(Name = "DatetimeOfStamp", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string StampDateTime { get; set; }

        /// <summary>
        /// Gets or sets start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [Display(Name = "StartDate", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string startDate { get; set; }

        /// <summary>
        /// Gets or sets the subtotal.
        /// </summary>
        /// <value>
        /// The subtotal.
        /// </value>
        [Display(Name = "SubTotal", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string subtotal { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        [Display(Name = "Total", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string total { get; set; }

        /// <summary>
        /// Gets or sets the total transfer taxed.
        /// </summary>
        /// <value>
        /// The total tranfer taxed.
        /// </value>
        [Display(Name = "TotalTransferredTaxes", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string totalTransferTaxed { get; set; }

        /// <summary>
        /// Gets or sets the uuid.
        /// </summary>
        /// <value>
        /// The uuid.
        /// </value>
        [Display(Name = "lblUUID", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public string uuid { get; set; }

        /// <summary>
        /// Gets or sets version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public double Version { get; set; }

        /// <summary>
        /// Gets or sets the type of the voucher.
        /// </summary>
        /// <value>
        /// The type of the voucher.
        /// </value>
        [Display(Name = "FiscalRecType", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string VoucherType { get; set; }

        /// <summary>
        /// Gets or sets the Year.
        /// </summary>
        /// <value>
        /// The Year.
        /// </value>
        [Display(Name = "lblYear", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string Year { get; set; }
    }
}