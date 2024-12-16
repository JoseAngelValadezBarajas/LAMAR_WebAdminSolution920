// --------------------------------------------------------------------
// <copyright file="PPDCreateViewModel.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using PowerCampus.Entities.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Helpers;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.FiscalRecords
{
    /// <summary>
    /// PPDCreateViewModel class
    /// </summary>
    public class PPDCreateViewModel
    {
        /// <summary>
        /// Gets or sets the name of the cancel reason.
        /// </summary>
        /// <value>
        /// The name of the cancel reason.
        /// </value>
        public CancelReasonName? CancelReasonName { get; set; }

        /// <summary>
        /// Gets the cfdi related.
        /// </summary>
        /// <value>
        /// The cfdi related.
        /// </value>
        [Display(Name = "lblCFDIRelated", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string CFDIRelated { get; internal set; }

        /// <summary>
        /// Gets the cfdi related identifier.
        /// </summary>
        /// <value>
        /// The cfdi related identifier.
        /// </value>
        public int? CFDIRelatedId { get; internal set; }

        /// <summary>
        /// Gets or sets the charge credit number.
        /// </summary>
        /// <value>
        /// The charge credit number.
        /// </value>
        public int? ChargeCreditNumber { get; set; }

        /// <summary>
        /// Gets or sets the people org code identifier.
        /// </summary>
        /// <value>
        /// The people org code identifier.
        /// </value>
        public string PeopleOrgCodeId { get; set; }

        /// <summary>
        /// Gets the relation type desc.
        /// </summary>
        /// <value>
        /// The relation type desc.
        /// </value>
        [Display(Name = "lblRelationType", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string RelationTypeDesc { get; internal set; }

        #region Comprobante
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
        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        [Display(Name = "lblCurrency", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the payment condition.
        /// </summary>
        /// <value>
        /// The payment condition.
        /// </value>
        [Display(Name = "lblPaymentCondition", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string PaymentCondition { get; set; }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        /// <value>
        /// The payment method.
        /// </value>
        [Display(Name = "lblPaymentMethod", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>
        /// The type of the payment.
        /// </value>
        [Display(Name = "lblPaymentType", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string PaymentType { get; set; }

        /// <summary>
        /// Gets or sets the subtotal.
        /// </summary>
        /// <value>
        /// The subtotal.
        /// </value>
        [Display(Name = "lblSubTotal", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string Subtotal { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        [Display(Name = "lblTotal", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string Total { get; set; }

        /// <summary>
        /// Gets or sets the total taxes.
        /// </summary>
        /// <value>
        /// The total taxes.
        /// </value>
        [Display(Name = "lblTotalTaxes", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string TotalTaxes { get; set; }

        #endregion Comprobante

        #region Emisor

        /// <summary>
        /// Gets or sets the fiscal address.
        /// </summary>
        /// <value>
        /// The fiscal address.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml), ErrorMessageResourceName = "lblIssuerAddressRequired")]
        [Display(Name = "lblIssuerAddress", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string IssuerAddress { get; set; }

        /// <summary>
        /// Gets or sets the name of the corporate.
        /// </summary>
        /// <value>
        /// The name of the corporate.
        /// </value>
        [Display(Name = "lblIssuerCorporateName", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string IssuerCorporateName { get; set; }

        /// <summary>
        /// Gets or sets the iss invoice expedition identifier.
        /// </summary>
        /// <value>
        /// The iss invoice expedition identifier.
        /// </value>
        public int? IssuerInvoiceExpeditionId { get; set; }

        /// <summary>
        /// Gets or sets the iss invoice organization identifier.
        /// </summary>
        /// <value>
        /// The iss invoice organization identifier.
        /// </value>
        public int? IssuerInvoiceOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the serial.
        /// </summary>
        /// <value>
        /// The serial.
        /// </value>
        [Display(Name = "lblIssuerSerial", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string IssuerSerial { get; set; }

        /// <summary>
        /// Gets or sets the issuer taxpayer identifier.
        /// </summary>
        /// <value>
        /// The issuer taxpayer identifier.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml), ErrorMessageResourceName = "lblIssuerTaxpayerIdRequired")]
        [Display(Name = "lblIssuerTaxpayerId", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string IssuerTaxpayerId { get; set; }

        /// <summary>
        /// Gets or sets the issuer taxregimen.
        /// </summary>
        /// <value>
        /// The issuer taxregimen.
        /// </value>
        [Display(Name = "lblIssuerTaxRegimen", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string IssuerTaxregimen { get; set; }

        /// <summary>
        /// Gets or sets the list tax regimen catalogs.
        /// </summary>
        /// <value>
        /// The list tax regimen catalogs.
        /// </value>
        public List<TaxRegimenCatalog> IssuerTaxRegimenCatalogs { get; set; }

        /// <summary>
        /// Gets or sets the tax regimen identifier.
        /// </summary>
        /// <value>
        /// The tax regimen identifier.
        /// </value>
        public int? IssuerTaxRegimenId { get; set; }

        /// <summary>
        /// Gets or sets the placeof issue.
        /// </summary>
        /// <value>
        /// The placeof issue.
        /// </value>
        [Display(Name = "lblIssuerPlaceofIssue", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string PlaceofIssue { get; set; }

        #endregion Emisor

        #region Receptor

        /// <summary>
        /// Gets or sets the receiver cfdi list.
        /// </summary>
        /// <value>
        /// The receiver cfdi list.
        /// </value>
        [Display(Name = "lblReceiverCFDI", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public List<CFDIUsageCatalog> ReceiverCFDIList { get; set; }

        /// <summary>
        /// Gets or sets the receiver cfdi usage code.
        /// </summary>
        /// <value>
        /// The receiver cfdi usage code.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml), ErrorMessageResourceName = "lblReceiverCFDIUsageCodeRequired")]
        public string ReceiverCFDIUsageCode { get; set; }

        /// <summary>
        /// Gets or sets the cfdi usage desc.
        /// </summary>
        /// <value>
        /// The cfdi usage desc.
        /// </value>
        public string ReceiverCFDIUsageDesc { get; set; }

        /// <summary>
        /// Gets or sets the name of the receiver corporate.
        /// </summary>
        /// <value>
        /// The name of the receiver corporate.
        /// </value>
        [Display(Name = "lblReceiverCorporateName", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string ReceiverCorporateName { get; set; }

        /// <summary>
        /// Gets or sets the receiver email.
        /// </summary>
        /// <value>
        /// The receiver email.
        /// </value>
        [EmailAddress(ErrorMessageResourceType = typeof(PeopleViewModelResource), ErrorMessageResourceName = "ValidationMessageEmail", ErrorMessage = null)]
        [CustomEmailRegularExpression]
        [Required(ErrorMessageResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml), ErrorMessageResourceName = "lblReceiverEmailRequired")]
        [Display(Name = "lblReceiverEmail", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string ReceiverEmail { get; set; }

        /// <summary>
        /// Gets or sets the receiver fiscal identity number.
        /// </summary>
        /// <value>
        /// The receiver fiscal identity number.
        /// </value>
        [Display(Name = "lblReceiverFiscalIdentityNumber", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string ReceiverFiscalIdentityNumber { get; set; }

        /// <summary>
        /// Gets or sets the receiver fiscal residency.
        /// </summary>
        /// <value>
        /// The receiver fiscal residency.
        /// </value>
        [Display(Name = "lblReceiverFiscalAddress", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        public string ReceiverFiscalResidency { get; set; }

        /// <summary>
        /// Gets or sets the receiver tax address.
        /// </summary>
        /// <value>
        /// The receiver tax address.
        /// </value>
        [Display(Name = "lblReceiverTaxAddress", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        [Required(ErrorMessageResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml), ErrorMessageResourceName = "lblReceiverTaxAddressRequired")]
        public string ReceiverTaxAddress { get; set; }

        /// <summary>
        /// Gets or sets the reciver taxpayer identifier.
        /// </summary>
        /// <value>
        /// The reciver taxpayer identifier.
        /// </value>
        [Display(Name = "lblReceiverTaxpayerId", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        [Required(ErrorMessageResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml), ErrorMessageResourceName = "lblReceiverTaxpayerIdRequired")]
        public string ReceiverTaxpayerId { get; set; }

        /// <summary>
        /// Gets or sets the receiver tax regimen.
        /// </summary>
        /// <value>
        /// The receiver tax regimen.
        /// </value>
        [Display(Name = "lblReceiverTaxRegimen", ResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml))]
        [Required(ErrorMessageResourceType = typeof(Views.ChargeCredits.App_localResources.PPD_cshtml), ErrorMessageResourceName = "lblReceiverTaxRegimenRequired")]
        public string ReceiverTaxRegimen { get; set; }

        #endregion Receptor

        #region Conceptos

        /// <summary>
        /// Gets or sets the charge credit.
        /// </summary>
        /// <value>
        /// The charge credit.
        /// </value>
        public ChargeCreditViewModel ChargeCredit { get; set; }

        #endregion Conceptos
    }
}