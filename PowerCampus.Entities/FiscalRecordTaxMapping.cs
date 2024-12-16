// --------------------------------------------------------------------
// <copyright file="TaxProfileRate.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
namespace PowerCampus.Entities
{
    /// <summary>
    /// TaxProfileRate
    /// </summary>
    public class FiscalRecordTaxMapping
    {
        /// <summary>
        /// Gets or sets the factor rype.
        /// </summary>
        /// <value>
        /// The factor rype.
        /// </value>
        public string FactorType { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int FiscalRecordTaxMappingId { get; set; }

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
        public string TaxRate { get; set; }
    }
}