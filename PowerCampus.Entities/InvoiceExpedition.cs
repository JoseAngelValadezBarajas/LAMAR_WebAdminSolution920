// --------------------------------------------------------------------
// <copyright file="InvoiceExpedition.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities
{
    public class InvoiceExpedition
    {
        /// <summary>
        ///  Gets or sets the State.
        /// </summary>
        public int CanDelete { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// Expedition Address Description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the InvoiceExpeditionId.
        /// </summary>
        /// <value>
        /// The InvoiceExpeditionId number.
        /// </value>
        public int? InvoiceExpeditionId { get; set; }

        /// <summary>
        /// Gets or sets the InvoiceOrganizationId.
        /// </summary>
        /// <value>
        /// The InvoiceOrganizationId number.
        /// </value>
        public int? InvoiceOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the PostalCode.
        /// </summary>
        /// <value>
        /// Expedition Address Postal Code or Zip.
        /// </value>
        /// [Required]
        public string PostalCode { get; set; }

        /// <summary>
        ///  Gets or sets the State.
        /// </summary>
        public string State { get; set; }
    }
}