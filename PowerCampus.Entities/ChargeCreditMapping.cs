// --------------------------------------------------------------------
// <copyright file="ChargeCreditMapping.cs" company="Ellucian">
//     Copyright 2018 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities
{
    /// <summary>
    /// ChargeCreditMapping class.
    /// </summary>
    public class ChargeCreditMapping
    {
        /// <summary>
        /// Gets or sets the charge credit code identifier.
        /// </summary>
        /// <value>
        /// The charge credit code identifier.
        /// </value>
        public int ChargeCreditCodeId { get; set; }

        /// <summary>
        /// Gets or sets the charge credit desc.
        /// </summary>
        /// <value>
        /// The charge credit desc.
        /// </value>
        public string ChargeCreditDesc { get; set; }

        /// <summary>
        /// Gets or sets the type of the factor.
        /// </summary>
        /// <value>
        /// The type of the factor.
        /// </value>
        public string FactorType { get; set; }

        /// <summary>
        /// Gets or sets the charge product service mapping.
        /// </summary>
        /// <value>
        /// The charge credit number.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the product service desc.
        /// </summary>
        /// <value>
        /// The product service desc.
        /// </value>
        public string ProductServiceDesc { get; set; }

        /// <summary>
        /// Gets or sets the product service key.
        /// </summary>
        /// <value>
        /// The product service key.
        /// </value>
        public string ProductServiceKey { get; set; }

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
        public string TaxDescription { get; set; }

        /// <summary>
        /// Gets or sets the tax rate.
        /// </summary>
        /// <value>
        /// The tax rate.
        /// </value>
        public string TaxRate { get; set; }

        /// <summary>
        /// Gets or sets the unity desc.
        /// </summary>
        /// <value>
        /// The unity desc.
        /// </value>
        public string UnityDesc { get; set; }

        /// <summary>
        /// Gets or sets the unity key.
        /// </summary>
        /// <value>
        /// The unity key.
        /// </value>
        public string UnityKey { get; set; }
    }
}