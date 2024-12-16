// --------------------------------------------------------------------
// <copyright file="TaxRegimen.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities
{
    /// <summary>
    /// TaxRegimen Class
    /// </summary>
    public class TaxRegimen
    {
        /// <summary>
        /// Gets or sets the iss code value.
        /// </summary>
        /// <value>
        /// The iss code value.
        /// </value>
        public string IssCodeValue { get; set; }

        /// <summary>
        /// Gets or sets the iss long desc.
        /// </summary>
        /// <value>
        /// The iss long desc.
        /// </value>
        public string IssLongDesc { get; set; }

        /// <summary>
        /// Gets or sets the iss tax reg invoice organization identifier.
        /// </summary>
        /// <value>
        /// The iss tax reg invoice organization identifier.
        /// </value>
        public int? IssTaxRegInvoiceOrganizationId { get; set; }
    }
}