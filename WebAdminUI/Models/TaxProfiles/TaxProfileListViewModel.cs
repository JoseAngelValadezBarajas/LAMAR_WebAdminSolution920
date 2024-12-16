// --------------------------------------------------------------------
// <copyright file="TaxProfileListViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.TaxProfiles
{
    /// <summary>
    /// TaxProfileListViewModel
    /// </summary>
    public class TaxProfileListViewModel
    {
        /// <summary>
        /// Gets or sets the tax profile description.
        /// </summary>
        /// <value>
        /// The tax profile description.
        /// </value>
        [Display(Name = nameof(TaxProfileListViewModelResource.TaxProfileDescription), ResourceType = typeof(TaxProfileListViewModelResource))]
        public string TaxProfileDescription { get; set; }

        /// <summary>
        /// Gets or sets the tax profile identifier.
        /// </summary>
        /// <value>
        /// The tax profile identifier.
        /// </value>
        public int TaxProfileId { get; set; }

        /// <summary>
        /// Gets or sets the name of the tax profile.
        /// </summary>
        /// <value>
        /// The name of the tax profile.
        /// </value>
        [Display(Name = nameof(TaxProfileListViewModelResource.TaxProfileName), ResourceType = typeof(TaxProfileListViewModelResource))]
        public string TaxProfileName { get; set; }

        /// <summary>
        /// Gets or sets the tax profile validities.
        /// </summary>
        /// <value>
        /// The tax profile validities.
        /// </value>
        [Display(Name = nameof(TaxProfileListViewModelResource.TaxProfileValidities), ResourceType = typeof(TaxProfileListViewModelResource))]
        public List<TaxProfileValidityViewModel> TaxProfileValidities { get; set; }
    }
}