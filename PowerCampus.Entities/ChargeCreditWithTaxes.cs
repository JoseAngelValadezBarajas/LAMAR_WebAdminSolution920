// --------------------------------------------------------------------
// <copyright file="ChargeCreditWithTaxes.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// ChargeCreditWithTaxes class.
    /// </summary>
    public class ChargeCreditWithTaxes
    {
        /// <summary>
        /// The charge credit taxes
        /// </summary>
        public List<ChargeCreditTaxes> ChargeCreditTaxes { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
    }
}