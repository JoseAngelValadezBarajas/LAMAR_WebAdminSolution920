// --------------------------------------------------------------------
// <copyright file="ChargeCreditWithTaxesViewModel.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.ChargeCreditMappings
{
    /// <summary>
    /// ChargeCreditWithTaxesViewModel class.
    /// </summary>
    public class ChargeCreditWithTaxesViewModel
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name = "Code", ResourceType = typeof(ChargeCreditMappingModelResource))]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the taxes.
        /// </summary>
        /// <value>
        /// The taxes.
        /// </value>
        public List<string> Taxes { get; set; }

        /// <summary>
        /// Gets or sets the tax profiles.
        /// </summary>
        /// <value>
        /// The tax profiles.
        /// </value>
        [Display(Name = "TaxProfile", ResourceType = typeof(ChargeCreditMappingModelResource))]
        public List<string> TaxProfiles { get; set; }
    }
}