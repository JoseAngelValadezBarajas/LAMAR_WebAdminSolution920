// --------------------------------------------------------------------
// <copyright file="IssuerDefaultViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.Issuers
{
    /// <summary>
    ///  Doucumnet Setp Type enum
    /// </summary>
    public enum DoucumnetSetpType
    {
        /// <summary>
        /// In issuer Doucemnet is created by expedition address or issuer address by
        /// </summary>
        ByExpedition,

        /// <summary>
        /// In issuer Doucemnet is created by Taxpayer Id
        /// </summary>
        ByTaxPayerId,

        /// <summary>
        /// none
        /// </summary>
        None
    }

    /// <summary>
    /// IssuerDefault
    /// </summary>
    public class IssuerDefaultViewModel
    {
        /// <summary>
        /// Gets or sets the iss default identifier.
        /// </summary>
        /// <value>
        /// The iss default identifier.
        /// </value>
        public int IssDefaultId { get; set; }

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
        public int IssInvoiceOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the iss invoice receipt identifier.
        /// </summary>
        /// <value>
        /// The iss invoice receipt identifier.
        /// </value>
        public int IssInvoiceReceiptId { get; set; }

        /// <summary>
        /// Gets or sets the iss invoice taxpayer identifier.
        /// </summary>
        /// <value>
        /// The iss invoice taxpayer identifier.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.Issuers.App_LocalResources.IssuerResource), ErrorMessageResourceName = "IssuerTaxPayerIDRequired")]
        [Display(Name = "IssuerTaxPayerID", ResourceType = typeof(Views.Issuers.App_LocalResources.IssuerResource))]
        public string IssInvoiceTaxpayerId { get; set; }

        /// <summary>
        /// Gets or sets the iss issuing address.
        /// </summary>
        /// <value>
        /// The iss issuing address.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.Issuers.App_LocalResources.IssuerResource), ErrorMessageResourceName = "IssuingAddressRequired")]
        [Display(Name = "IssuingAddress", ResourceType = typeof(Views.Issuers.App_LocalResources.IssuerResource))]
        public string IssIssuingAddress { get; set; }

        /// <summary>
        /// Gets or sets the iss payment conditions.
        /// </summary>
        /// <value>
        /// The iss payment conditions.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.Issuers.App_LocalResources.IssuerResource), ErrorMessageResourceName = "PaymentConditionsRequired")]
        [Display(Name = "PaymentContditions", ResourceType = typeof(Views.Issuers.App_LocalResources.IssuerResource))]
        public string IssPaymentConditions { get; set; }

        /// <summary>
        /// Gets or sets the iss serial number.
        /// </summary>
        /// <value>
        /// The iss serial number.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.Issuers.App_LocalResources.IssuerResource), ErrorMessageResourceName = "SerialRequired")]
        [Display(Name = "SerialInovice", ResourceType = typeof(Views.Issuers.App_LocalResources.IssuerResource))]
        public string IssSerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the iss serial number credit note.
        /// </summary>
        /// <value>
        /// The iss serial number credit note.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Views.Issuers.App_LocalResources.IssuerResource), ErrorMessageResourceName = "SerialRequired")]
        [Display(Name = "SerialCreditNote", ResourceType = typeof(Views.Issuers.App_LocalResources.IssuerResource))]
        public string IssSerialNumberCreditNote { get; set; }

        /// <summary>
        /// Gets or sets the name of the iss user.
        /// </summary>
        /// <value>
        /// The name of the iss user.
        /// </value>
        public string IssUserName { get; set; }
    }

    /// <summary>
    /// Issuer View Model
    /// </summary>
    public class IssuerViewModel
    {
        /// <summary>
        /// InvoiceOrganization
        /// </summary>
        public InvoiceOrganizationViewModel InvoiceOrganization { get; set; }

        #region Issuing address

        /// <summary>
        /// InvoiceExpedition
        /// </summary>
        public InvoiceExpeditionViewModel InvoiceExpedition { get; set; }

        /// <summary>
        /// InvoiceExpeditions
        /// </summary>
        public List<InvoiceExpeditionViewModel> InvoiceExpeditions { get; set; }

        /// <summary>
        /// InvoiceReceipts
        /// </summary>
        public List<InvoiceReceiptViewModel> InvoiceReceipts { get; set; }

        /// <summary>
        /// ReceiptType
        /// </summary>
        public DoucumnetSetpType ReceiptType { get; set; }

        /// <summary>
        /// State
        /// </summary>
        [Display(Name = nameof(IssuerModelResource.State), ResourceType = typeof(IssuerModelResource))]
        public string State { get; set; }

        #endregion Issuing address
    }
}