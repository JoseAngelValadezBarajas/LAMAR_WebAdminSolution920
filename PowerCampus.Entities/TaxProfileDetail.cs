// --------------------------------------------------------------------
// <copyright file="TaxProfileDetail.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
namespace PowerCampus.Entities
{
    /// <summary>
    /// TaxProfileDetail
    /// </summary>
    public class TaxProfileDetail
    {
        /// <summary>
        /// Gets or sets the charge credit description.
        /// </summary>
        /// <value>
        /// The charge credit description.
        /// </value>
        public string ChargeCreditDescription { get; set; }

        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        /// <value>
        /// The percentage.
        /// </value>
        public decimal Percentage { get; set; }

        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        public int Sequence { get; set; }

        /// <summary>
        /// Gets or sets the tax profile rate.
        /// </summary>
        /// <value>
        /// The tax profile rate.
        /// </value>
        public FiscalRecordTaxMapping TaxMapping { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int TaxProfileDetailId { get; set; }

        /// <summary>
        /// Gets or sets the tax profile validity identifier.
        /// </summary>
        /// <value>
        /// The tax profile validity identifier.
        /// </value>
        public int TaxProfileValidityId { get; set; }
    }
}