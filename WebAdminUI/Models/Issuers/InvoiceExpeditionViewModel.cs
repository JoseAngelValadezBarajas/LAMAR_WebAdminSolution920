// --------------------------------------------------------------------
// <copyright file="InvoiceExpeditionViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.Issuers
{
    /// <summary>
    ///
    /// </summary>
    public class InvoiceExpeditionViewModel
    {
        /// <summary>
        ///  Gets or sets the State.
        /// </summary>
        public int CanDelete { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// Expedition Address Description.
        /// </value>
        [Display(Name = nameof(IssuerModelResource.Description), ResourceType = typeof(IssuerModelResource))]
        [Required(ErrorMessageResourceType = typeof(IssuerModelResource), ErrorMessageResourceName = "DescriptionRequired")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the InvoiceExpeditionId.
        /// </summary>
        /// <value>
        /// The InvoiceExpeditionId number.
        /// </value>
        public int? InvoiceExpeditionId { get; set; }

        /// <summary>
        /// Gets or sets the InvoiceOrganizationId.
        /// </summary>
        /// <value>
        /// The InvoiceOrganizationId number.
        /// </value>
        public int? InvoiceOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the PostalCode.
        /// </summary>
        /// <value>
        /// Expedition Address Postal Code or Zip.
        /// </value>
        /// [Required]
        [Display(Name = nameof(IssuerModelResource.ZipCode), ResourceType = typeof(IssuerModelResource))]
        [Required(ErrorMessageResourceType = typeof(IssuerModelResource), ErrorMessageResourceName = "ZipCodeRequired")]
        [RegularExpression(@"^(?!00000)[0-9]{5,5}$", ErrorMessageResourceType = typeof(IssuerModelResource), ErrorMessageResourceName = "ZipCodeInvalid")]
        public string PostalCode { get; set; }

        /// <summary>
        ///  Gets or sets the State.
        /// </summary>
        [Display(Name = nameof(IssuerModelResource.State), ResourceType = typeof(IssuerModelResource))]
        public string State { get; set; }
    }
}