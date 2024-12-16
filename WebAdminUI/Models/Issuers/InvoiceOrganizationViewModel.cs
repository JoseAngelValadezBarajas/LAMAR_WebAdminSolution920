// --------------------------------------------------------------------
// <copyright file="InvoiceOrganizationViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.Issuers
{
    /// <summary>
    ///
    /// </summary>
    public class InvoiceOrganizationViewModel
    {
        /// <summary>
        /// Gets or sets the name of the corporate.
        /// </summary>
        /// <value>
        /// The name of the corporate.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(IssuerModelResource), ErrorMessageResourceName = "CorporateNameRequired")]
        [RegularExpression(@"[^|^a-z^ñ]{1,254}", ErrorMessageResourceType = typeof(IssuerModelResource), ErrorMessageResourceName = "InvalidCorporateName")]
        [Display(Name = nameof(IssuerModelResource.CorporateName), ResourceType = typeof(IssuerModelResource))]
        public string CorporateName { get; set; }

        /// <summary>
        /// Gets or sets the invoice organization identifier.
        /// </summary>
        /// <value>
        /// The invoice organization identifier.
        /// </value>
        public int? InvoiceOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the taxpayer identifier.
        /// </summary>
        /// <value>
        /// The taxpayer identifier.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(IssuerModelResource), ErrorMessageResourceName = "TaxpayerIdRequired")]
        [StringLength(13, MinimumLength = 12, ErrorMessageResourceType = typeof(IssuerModelResource), ErrorMessageResourceName = "MaxlengthTaxpayer")]
        [RegularExpression(@"[A-Z, Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9, A-Z]", ErrorMessageResourceType = typeof(IssuerModelResource), ErrorMessageResourceName = "InvalidTaxpayer")]
        [Display(Name = nameof(IssuerModelResource.TaxpayerId), ResourceType = typeof(IssuerModelResource))]
        public string TaxpayerId { get; set; }

        #region TaxRegim

        /// <summary>
        /// Gets or sets the invoice org tax regimen identifier.
        /// </summary>
        /// <value>
        /// The invoice org tax regimen identifier.
        /// </value>
        public int? InvoiceOrgTaxRegimenId { get; set; }

        /// <summary>
        /// List of all Tax Regimen Catalogs
        /// </summary>
        public List<TaxRegimenCatalog> ListTaxRegimenCatalogs { get; set; }

        /// <summary>
        /// Gets or sets the tax regimen identifier.
        /// </summary>
        /// <value>
        /// The tax regimen identifier.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(IssuerModelResource), ErrorMessageResourceName = "TaxRegimenRequired")]
        [Display(Name = nameof(IssuerModelResource.TaxRegimen), ResourceType = typeof(IssuerModelResource))]
        public int? TaxRegimenId { get; set; }

        #endregion TaxRegim
    }
}