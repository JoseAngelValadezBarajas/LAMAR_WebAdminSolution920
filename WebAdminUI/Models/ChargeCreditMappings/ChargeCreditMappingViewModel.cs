// --------------------------------------------------------------------
// <copyright file="ChargeCreditMappingViewModel.cs" company="Ellucian">
//     Copyright 2018 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdministration.Models.Shared;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.ChargeCreditMappings
{
    /// <summary>
    /// ChargeCreditMappingViewModel class.
    /// </summary>
    public class ChargeCreditMappingViewModel
    {
        /// <summary>
        /// Gets or sets the charge credit code.
        /// </summary>
        /// <value>
        /// The charge credit code.
        /// </value>
        [Display(Name = "ChargeCreditCode", ResourceType = typeof(ChargeCreditMappingModelResource))]
        [Required(ErrorMessageResourceType = typeof(ChargeCreditMappingModelResource), ErrorMessageResourceName = "ChargeCreditRequired")]
        public string ChargeCreditCode { get; set; }

        /// <summary>
        /// Gets or sets the charge credit code identifier.
        /// </summary>
        /// <value>
        /// The charge credit code identifier.
        /// </value>
        [ScaffoldColumn(false)]
        public int ChargeCreditCodeId { get; set; }

        /// <summary>
        /// Gets or sets the charge credit list.
        /// </summary>
        /// <value>
        /// The charge credit list.
        /// </value>
        public List<ChargeCreditMappingViewModel> ChargeCreditList { get; set; }

        /// <summary>
        /// Gets or sets the charge credit special taxes.
        /// </summary>
        /// <value>
        /// The charge credit special taxes.
        /// </value>
        public List<ListOptionViewModel> ChargeCreditSpecialTaxes { get; set; }

        /// <summary>
        /// Gets or sets the charge credit number.
        /// </summary>
        /// <value>
        /// The charge credit number.
        /// </value>
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the product service desc.
        /// </summary>
        /// <value>
        /// The product service desc.
        /// </value>
        [Display(Name = "ProductService", ResourceType = typeof(ChargeCreditMappingModelResource))]
        [Required(ErrorMessageResourceType = typeof(ChargeCreditMappingModelResource), ErrorMessageResourceName = "ProductServiceRequired")]
        public string ProductServiceDesc { get; set; }

        /// <summary>
        /// Gets or sets the product service key.
        /// </summary>
        /// <value>
        /// The product service key.
        /// </value>
        [ScaffoldColumn(false)]
        public string ProductServiceKey { get; set; }

        /// <summary>
        /// Gets or sets the Taxes Desc list.
        /// </summary>
        /// <value>
        /// The Taxes Desc list.
        /// </value>
        [Display(Name = nameof(ChargeCreditMappingModelResource.Taxes), ResourceType = typeof(ChargeCreditMappingModelResource))]
        public List<string> TaxesDescList { get; set; }

        /// <summary>
        /// Gets or sets the Tax Profile.
        /// </summary>
        /// <value>
        /// The Tax Profile.
        /// </value>
        [Display(Name = nameof(ChargeCreditMappingModelResource.TaxProfile), ResourceType = typeof(ChargeCreditMappingModelResource))]
        public string TaxProfile { get; set; }

        /// <summary>
        /// Gets or sets the unity desc.
        /// </summary>
        /// <value>
        /// The unity desc.
        /// </value>
        [Display(Name = "Unity", ResourceType = typeof(ChargeCreditMappingModelResource))]
        [Required(ErrorMessageResourceType = typeof(ChargeCreditMappingModelResource), ErrorMessageResourceName = "UnityRequired")]
        public string UnityDesc { get; set; }

        /// <summary>
        /// Gets or sets the product service desc.
        /// </summary>
        /// <value>
        /// The product service desc.
        /// </value>
        /// <summary>
        /// Gets or sets the unity key.
        /// </summary>
        /// <value>
        /// The unity key.
        /// </value>
        [ScaffoldColumn(false)]
        public string UnityKey { get; set; }
    }
}