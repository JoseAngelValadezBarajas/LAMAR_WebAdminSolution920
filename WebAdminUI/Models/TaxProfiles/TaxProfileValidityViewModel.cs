// --------------------------------------------------------------------
// <copyright file="TaxProfileValidityViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

namespace WebAdminUI.Models.TaxProfiles
{
    /// <summary>
    /// TaxProfileValidityViewModel
    /// </summary>
    public class TaxProfileValidityViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the tax profile details.
        /// </summary>
        /// <value>
        /// The tax profile details.
        /// </value>
        public List<TaxProfileDetailViewModel> TaxProfileDetails { get; set; }

        /// <summary>
        /// Gets or sets the validity from to.
        /// </summary>
        /// <value>
        /// The validity from to.
        /// </value>
        public string ValidityFromTo { get; set; }
    }
}