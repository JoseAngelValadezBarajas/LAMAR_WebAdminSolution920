// --------------------------------------------------------------------
// <copyright file="CashReceiptViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using PowerCampus.Entities.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Helpers;

namespace WebAdminUI.Models.FiscalRecords
{
    /// <summary>
    /// CashReceiptViewModel
    /// </summary>
    public class CashReceiptViewModel
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
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "CFDIRequired")]
        [Display(Name = "CFDI", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public List<CFDIUsageCatalog> CFDIList { get; set; }

        /// <summary>
        /// Gets or sets the cfdi related.
        /// </summary>
        /// <value>
        /// The cfdi related.
        /// </value>
        [Display(Name = "lblRelatedUUID", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string CFDIRelated { get; set; }

        /// <summary>
        /// Gets or sets the cfdi related identifier.
        /// </summary>
        /// <value>
        /// The cfdi related identifier.
        /// </value>
        public int? CFDIRelatedId { get; set; }

        /// <summary>
        /// Gets or sets the CFDI usage.
        /// </summary>
        /// <value>
        /// The CFDI usage.
        /// </value>
        [Display(Name = "CFDI", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string CfdiUsage { get; set; }

        /// <summary>
        /// Gets or sets the charges to invoice.
        /// </summary>
        /// <value>
        /// The charges to invoice.
        /// </value>
        public List<ChargeCreditViewModel> chargesToInvoiceList { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        [Display(Name = "IssComments", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the name of the corporate.
        /// </summary>
        /// <value>
        /// The name of the corporate.
        /// </value>
        [StringLength(255, ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "MaxLength")]
        [Display(Name = "ReceiverCorpName", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string CorporateName { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "EndDateRequired")]
        [Display(Name = "EndDate", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the fiscal identity number.
        /// </summary>
        /// <value>
        /// The fiscal identity number.
        /// </value>
        [Display(Name = "ReceiverFiscalID", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string FiscalIdentityNumber { get; set; }

        /// <summary>
        /// Gets or sets the fiscal residency.
        /// </summary>
        /// <value>
        /// The fiscal residency.
        /// </value>
        [Display(Name = "ReceiverFiscalAddress", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string FiscalResidency { get; set; }

        /// <summary>
        /// Gets or sets the fiscal residency code.
        /// </summary>
        /// <value>
        /// The fiscal residency code.
        /// </value>
        public string FiscalResidencyCode { get; set; }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "FrequencyRequired")]
        [Display(Name = "Frequency", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public FiscalRecordCatalog Frequency { get; set; }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        [Display(Name = "Frequency", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public List<FiscalRecordCatalog> FrequencyList { get; set; }

        /// <summary>
        /// Gets or sets the invoice expedition identifier.
        /// </summary>
        /// <value>
        /// The invoice expedition identifier.
        /// </value>
        public int? InvoiceExpeditionId { get; set; }

        /// <summary>
        /// Gets or sets the invoice tax payer identifier.
        /// </summary>
        /// <value>
        /// The invoice tax payer identifier.
        /// </value>
        public int? InvoiceTaxPayerId { get; set; }

        /// <summary>
        /// Gets or sets whether it is cancellation 04 or not
        /// </summary>
        /// <value>
        /// True if it is cancellation 04, false in another case
        /// </value>
        public bool IsCancellation04 { get; set; }

        /// <summary>
        /// Gets or sets the name of the iss corporate.
        /// </summary>
        /// <value>
        /// The name of the iss corporate.
        /// </value>
        [Display(Name = "IssNameCorp", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string IssCorporateName { get; set; }

        /// <summary>
        /// Gets or sets the iss invoice expedition identifier.
        /// </summary>
        /// <value>
        /// The iss invoice expedition identifier.
        /// </value>
        public int IssInvoiceExpeditionId { get; set; }

        /// <summary>
        /// Gets or sets the iss invoice organization identifier.
        /// </summary>
        /// <value>
        /// The iss invoice organization identifier.
        /// </value>
        public int? IssInvoiceOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the iss issuing address.
        /// </summary>
        /// <value>
        /// The iss issuing address.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "IssAddRequired")]
        [Display(Name = "IssExpeditionAddr", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string IssIssuingAddress { get; set; }

        /// <summary>
        /// Gets or sets the payment condition.
        /// </summary>
        /// <value>
        /// The payment condition.
        /// </value>
        [Display(Name = "PaymentCondition", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string IssPaymentCondition { get; set; }

        /// <summary>
        /// Gets or sets the iss place issue.
        /// </summary>
        /// <value>
        /// The iss place issue.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "IssPlaceRequired")]
        [Display(Name = "IssPlaceIssue", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string IssPlaceIssue { get; set; }

        /// <summary>
        /// Gets or sets the iss serial.
        /// </summary>
        /// <value>
        /// The iss serial.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "IssSerialRequired")]
        [Display(Name = "IssSerial", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string IssSerial { get; set; }

        /// <summary>
        /// Gets or sets the iss taxpayer identifier.
        /// </summary>
        /// <value>
        /// The iss taxpayer identifier.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "IssTaxPIRequired")]
        [Display(Name = "IssTaxPayerID", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string IssTaxpayerId { get; set; }

        /// <summary>
        /// Gets or sets the iss tax regimen.
        /// </summary>
        /// <value>
        /// The iss tax regimen.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "IssTaxRegRequired")]
        [Display(Name = "IssTaxRegimen", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public List<TaxRegimen> IssTaxRegimen { get; set; }

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "MonthRequired")]
        [Display(Name = "Month", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public FiscalRecordCatalog Month { get; set; }

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The type of the month.
        /// </value>
        [Display(Name = "Month", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public List<FiscalRecordCatalog> MonthList { get; set; }

        /// <summary>
        /// Gets or sets the payment type identifier.
        /// </summary>
        /// <value>
        /// The payment type identifier.
        /// </value>
        [Display(Name = "PaymentType", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
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
        /// Gets or sets the tax address.
        /// </summary>
        /// <value>
        /// The tax address.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "RequiredPostalCode")]
        [Display(Name = "PostalCode", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the preferred cfdi usage code.
        /// </summary>
        /// <value>
        /// The preferred cfdi usage code.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "RequiredPreferredCFDIUsageCode")]
        public string PreferredCFDIUsageCode { get; set; }

        /// <summary>
        /// Gets or sets the preferred receiver email.
        /// </summary>
        /// <value>
        /// The preferred receiver email.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "ReceiverEmail")]
        [EmailAddress(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "ErrorEmail")]
        [Display(Name = "preferredReceiverEmail", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        [CustomEmailRegularExpression]
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
        [Display(Name = "paymentMethodDescDefault", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string ReceiverPaymentMethodDefault { get; set; }

        /// <summary>
        /// Gets or sets the receiver payment method.
        /// </summary>
        /// <value>
        /// The receiver payment method.
        /// </value>
        public List<FiscalRecordCatalog> ReceiverPaymentMethodList { get; set; }

        /// <summary>
        /// Gets or sets the tax address.
        /// </summary>
        /// <value>
        /// The tax address.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "TaxAddressRequired")]
        [Display(Name = "TaxAddress", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string RecTaxAddress { get; set; }

        /// <summary>
        /// Gets or sets the receiver tax regimen.
        /// </summary>
        /// <value>
        /// The receiver tax regimen.
        /// </value>
        [Display(Name = "RecTaxRegimen", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "RecTaxRegimenRequired")]
        public string RecTaxRegimen { get; set; }

        /// <summary>
        /// Gets or sets the relation type desc.
        /// </summary>
        /// <value>
        /// The relation type desc.
        /// </value>
        [Display(Name = "lblRelationType", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string RelationTypeDesc { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "StartDateRequired")]
        [Display(Name = "StartDate", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the sub total.
        /// </summary>
        /// <value>
        /// The sub total.
        /// </value>
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        [Display(Name = "SubTotal", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string SubTotal { get; set; }

        /// <summary>
        /// Gets or sets the tax payer identifier.
        /// </summary>
        /// <value>
        /// The tax payer identifier.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "TaxPayerIdRequired")]
        [Display(Name = "TaxpayerID", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string TaxPayerId { get; set; }

        /// <summary>
        /// Gets or sets the total (sum subtotal with total taxes).
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        [Display(Name = "Total", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string Total { get; set; }

        /// <summary>
        /// Gets or sets the total transferred taxes.
        /// </summary>
        /// <value>
        /// The total tt.
        /// </value>
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        [Display(Name = "TotalTransferredTaxes", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string TotalTT { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "YearRequired")]
        [Display(Name = "Year", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public FiscalRecordCatalog Year { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        [Display(Name = "Year", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public List<FiscalRecordCatalog> YearList { get; set; }

        #region Emision Facturas ACOLLI 04112021
        /// <summary>
        /// Gets or sets the ISSUE DATE.
        /// </summary>
        /// <value>
        /// ISSUE DATE.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "IssueDateRequired")]
        [Display(Name = "IssueDate", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string IssueDate { get; set; }

        /// <summary>
        /// Gets or sets the ISSUE TIME.
        /// </summary>
        /// <value>
        /// ISSUE TIME.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "IssueTimeRequired")]
        [Display(Name = "IssueTime", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string IssueTime { get; set; }

        #endregion

    }
}