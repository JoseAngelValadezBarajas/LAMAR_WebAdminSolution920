// --------------------------------------------------------------------
// <copyright file="TaxProfile.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// TaxProfile
    /// </summary>
    public class TaxProfile
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int TaxProfileId { get; set; }

        /// <summary>
        /// Gets or sets the tax profile validity list.
        /// </summary>
        /// <value>
        /// The tax profile validity list.
        /// </value>
        public List<TaxProfileValidity> TaxProfileValidityList { get; set; }
    }
}