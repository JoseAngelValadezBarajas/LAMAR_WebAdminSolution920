// --------------------------------------------------------------------
// <copyright file="InvoiceReceiptViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.Issuers
{
    /// <summary>
    ///
    /// </summary>
    public class InvoiceReceiptViewModel
    {
        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [Display(Name = nameof(IssuerModelResource.EndDate), ResourceType = typeof(IssuerModelResource))]
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the initial Folio.
        /// </summary>
        /// <value>
        /// Initial Folio.
        /// </value>
        [Display(Name = nameof(IssuerModelResource.InitialFolio), ResourceType = typeof(IssuerModelResource))]
        public int? Folio { get; set; }

        /// <summary>
        /// Invoice Expedition id Description
        /// </summary>
        [Display(Name = nameof(IssuerModelResource.ExpeditionDescription), ResourceType = typeof(IssuerModelResource))]
        public string InvoiceExpeditionDescription { get; set; }

        /// <summary>
        /// Gets or sets the InvoiceExpeditionId.
        /// </summary>
        /// <value>
        /// The InvoiceExpeditionId number.
        /// </value>
        [Display(Name = nameof(IssuerModelResource.ExpeditionAddress), ResourceType = typeof(IssuerModelResource))]
        public int? InvoiceExpeditionId { get; set; }

        /// <summary>
        /// Gets or sets the InvoiceOrganizationId.
        /// </summary>
        /// <value>
        /// The InvoiceOrganizationId number.
        /// </value>
        public int? InvoiceOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the InvoiceReceiptId.
        /// </summary>
        /// <value>
        /// The InvoiceReceiptId number.
        /// </value>
        public int? InvoiceReceiptId { get; set; }

        /// <summary>
        /// Gets or sets the Last Folio Assigned.
        /// </summary>
        /// <value>
        /// Last Folio Assigned number.
        /// </value>
        [Display(Name = nameof(IssuerModelResource.LastFolio), ResourceType = typeof(IssuerModelResource))]
        public int? LastFolioAssigned { get; set; }

        /// <summary>
        /// All Expidition belongs to this Documnet.
        /// </summary>
        public List<InvoiceExpeditionViewModel> ListInvoiceExpedition { get; set; }

        /// <summary>
        /// Gets or sets the EndDate.
        /// </summary>
        /// <value>
        /// Receipt EndDate.
        /// </value>
        /// <summary>
        /// Gets or sets the option.
        /// </summary>
        /// <value>
        /// The option ByExpedition or ByTaxpayerId.
        /// </value>
        public int OptionBy { get; set; }

        /// <summary>
        /// Gets or sets the Receipt Serial Number.
        /// </summary>
        /// <value>
        /// Receipt Serial Number.
        /// </value>
        [Display(Name = nameof(IssuerModelResource.SerialNumber), ResourceType = typeof(IssuerModelResource))]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// Receipt Description.
        /// </value>

        [Display(Name = nameof(IssuerModelResource.StartDate), ResourceType = typeof(IssuerModelResource))]
        public string StartDate { get; set; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }
    }
}