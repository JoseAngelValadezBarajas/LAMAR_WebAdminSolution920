// --------------------------------------------------------------------
// <copyright file="ReceiverModel.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Foolproof;
using PowerCampus.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Models.Issuers;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.Receivers
{
    /// <summary>
    ///
    /// </summary>
    public class ReceiverViewModel
    {
        /// <summary>
        /// Gets or sets the name of the corporate.
        /// </summary>
        /// <value>
        /// The name of the corporate.
        /// </value>
        [Display(Name = nameof(ReceiverModelResource.CorporateName), ResourceType = typeof(ReceiverModelResource))]
        [Required(ErrorMessageResourceType = typeof(ReceiverModelResource), ErrorMessageResourceName = "Required")]
        [StringLength(255, ErrorMessageResourceType = typeof(ReceiverModelResource), ErrorMessageResourceName = "MaxLength")]
        public string CorporateName { get; set; }

        /// <summary>
        /// Gets or sets the fiscal identity number.
        /// </summary>
        /// <value>
        /// The fiscal identity number.
        /// </value>
        [Display(Name = nameof(ReceiverModelResource.FiscalIdentityNumber), ResourceType = typeof(ReceiverModelResource))]
        [RequiredIf("TaxPayerId", "XEXX010101000", ErrorMessageResourceType = typeof(ReceiverModelResource), ErrorMessageResourceName = "Required")]
        public string FiscalIdentityNumber { get; set; }

        /// <summary>
        /// Gets or sets the fiscal residency.
        /// </summary>
        /// <value>
        /// The fiscal residency.
        /// </value>
        [Display(Name = nameof(ReceiverModelResource.FiscalResidency), ResourceType = typeof(ReceiverModelResource))]
        [RequiredIf("TaxPayerId", "XEXX010101000", ErrorMessageResourceType = typeof(ReceiverModelResource), ErrorMessageResourceName = "Required")]
        public string FiscalResidency { get; set; }

        /// <summary>
        /// Gets or sets the fiscal residency desc.
        /// </summary>
        /// <value>
        /// The fiscal residency desc.
        /// </value>
        public string FiscalResidencyDesc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has invoice.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has invoice; otherwise, <c>false</c>.
        /// </value>
        public bool HasInvoice { get; set; }

        /// <summary>
        /// Gets or sets the invoice foreign taxpayer identifier.
        /// </summary>
        /// <value>
        /// The invoice foreign taxpayer identifier.
        /// </value>
        public int? InvoiceForeignTaxpayerId { get; set; }

        /// <summary>
        /// InvoiceOrganization
        /// </summary>
        public InvoiceOrganizationViewModel InvoiceOrganization { get; set; }

        /// <summary>
        /// Gets or sets the invoice tax payer identifier.
        /// </summary>
        /// <value>
        /// The invoice tax payer identifier.
        /// </value>
        public int? InvoiceTaxpayerId { get; set; }

        /// <summary>
        /// Gets or sets the tax address.
        /// </summary>
        /// <value>
        /// The tax address.
        /// </value>
        [Display(Name = nameof(ReceiverModelResource.TaxAddress), ResourceType = typeof(ReceiverModelResource))]
        [Required(ErrorMessageResourceType = typeof(ReceiverModelResource), ErrorMessageResourceName = "Required")]
        [StringLength(5, ErrorMessageResourceType = typeof(ReceiverModelResource), ErrorMessageResourceName = "MaxLength")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the tax payer identifier.
        /// </summary>
        /// <value>
        /// The tax payer identifier.
        /// </value>
        [Display(Name = nameof(ReceiverModelResource.TaxPayerId), ResourceType = typeof(ReceiverModelResource))]
        [Required(ErrorMessageResourceType = typeof(ReceiverModelResource), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"[A-Z, Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9, A-Z]", ErrorMessageResourceType = typeof(ReceiverModelResource), ErrorMessageResourceName = "InvalidTaxPayer")]
        public string TaxPayerId { get; set; }

        /// <summary>
        /// Gets or sets the tax regimen code.
        /// </summary>
        /// <value>
        /// The tax regimen code.
        /// </value>
        public string TaxRegimenCode { get; set; }

        /// <summary>
        /// Gets or sets the tax regimen desc.
        /// </summary>
        /// <value>
        /// The tax regimen desc.
        /// </value>
        public string TaxRegimenDesc { get; set; }

        /// <summary>
        /// Gets or sets the tax regimen.
        /// </summary>
        /// <value>
        /// The tax address.
        /// </value>
        [Display(Name = nameof(ReceiverModelResource.TaxRegimen), ResourceType = typeof(ReceiverModelResource))]
        [Required(ErrorMessageResourceType = typeof(ReceiverModelResource), ErrorMessageResourceName = "Required")]
        public int? TaxRegimenId { get; set; }
        [Display(Name = nameof(ReceiverModelResource.StreetName), ResourceType = typeof(ReceiverModelResource))]
        public string StreetName { get; set; }
        [Display(Name = nameof(ReceiverModelResource.StreetNumber), ResourceType = typeof(ReceiverModelResource))]
        public string StreetNumber { get; set; }
        [Display(Name = nameof(ReceiverModelResource.ApartmentNumber), ResourceType = typeof(ReceiverModelResource))]
        public string ApartmentNumber { get; set; }
        [Display(Name = nameof(ReceiverModelResource.Neighborhood), ResourceType = typeof(ReceiverModelResource))]
        public string Neighborhood { get; set; }
        [Display(Name = nameof(ReceiverModelResource.Location), ResourceType = typeof(ReceiverModelResource))]
        public string Location { get; set; }
        [Display(Name = nameof(ReceiverModelResource.StateProvinceId), ResourceType = typeof(ReceiverModelResource))]
        public string StateProvinceId { get; set; }
        [Display(Name = nameof(ReceiverModelResource.CountryId), ResourceType = typeof(ReceiverModelResource))]
        public string CountryId { get; set; }
        public List<FiscalRecordCatalog> ListStates { get; set; }
    }
}