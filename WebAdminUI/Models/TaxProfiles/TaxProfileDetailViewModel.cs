// --------------------------------------------------------------------
// <copyright file="TaxProfileDetailViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.TaxProfiles
{
    /// <summary>
    /// TaxProfileDetailViewModel
    /// </summary>
    public class TaxProfileDetailViewModel
    {
        /// <summary>
        /// Gets or sets the charge credit description.
        /// </summary>
        /// <value>
        /// The charge credit description.
        /// </value>
        [Display(Name = nameof(TaxProfileListViewModelResource.DetailsChargeDescription), ResourceType = typeof(TaxProfileListViewModelResource))]
        public string ChargeCreditDescription { get; set; }

        /// <summary>
        /// Gets or sets the type of the factor.
        /// </summary>
        /// <value>
        /// The type of the factor.
        /// </value>
        public string FactorType { get; set; }

        /// <summary>
        /// Gets or sets the fiscal record tax mapping identifier.
        /// </summary>
        /// <value>
        /// The fiscal record tax mapping identifier.
        /// </value>
        public int FiscalRecordTaxMappingId { get; set; }

        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        /// <value>
        /// The percentage.
        /// </value>
        [Display(Name = nameof(TaxProfileListViewModelResource.DetailsPercentage), ResourceType = typeof(TaxProfileListViewModelResource))]
        public decimal Percentage { get; set; }

        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        [Display(Name = nameof(TaxProfileListViewModelResource.DetailsSequence), ResourceType = typeof(TaxProfileListViewModelResource))]
        public int Sequence { get; set; }

        /// <summary>
        /// Gets or sets the tax code.
        /// </summary>
        /// <value>
        /// The tax code.
        /// </value>
        public string TaxCode { get; set; }

        /// <summary>
        /// Gets or sets the tax description.
        /// </summary>
        /// <value>
        /// The tax description.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(TaxProfileListViewModelResource), ErrorMessageResourceName = "TaxRequired")]
        public string TaxDescription { get; set; }

        /// <summary>
        /// Gets or sets the taxes catalog.
        /// </summary>
        /// <value>
        /// The taxes catalog.
        /// </value>
        [Display(Name = nameof(TaxProfileListViewModelResource.DetailsTax), ResourceType = typeof(TaxProfileListViewModelResource))]
        public List<FiscalRecordCatalog> TaxesCatalog { get; set; }

        /// <summary>
        /// Gets or sets the tax profile detail identifier.
        /// </summary>
        /// <value>
        /// The tax profile detail identifier.
        /// </value>
        public int TaxProfileDetailId { get; set; }

        /// <summary>
        /// Gets or sets the tax rate.
        /// </summary>
        /// <value>
        /// The tax rate.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(TaxProfileListViewModelResource), ErrorMessageResourceName = "TaxRateRequired")]
        public string TaxRate { get; set; }

        /// <summary>
        /// Gets or sets the tax rates catalog.
        /// </summary>
        /// <value>
        /// The tax rates catalog.
        /// </value>
        [Display(Name = nameof(TaxProfileListViewModelResource.DetailsRate), ResourceType = typeof(TaxProfileListViewModelResource))]
        public List<FiscalRecordCatalog> TaxRatesCatalog { get; set; }
    }
}