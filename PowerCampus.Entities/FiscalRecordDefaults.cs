// --------------------------------------------------------------------
// <copyright file="FiscalRecordDefaults.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// FiscalRecordDefaults Class to store default setups that belongs the person
    /// </summary>
    public class FiscalRecordDefaults
    {
        /// <summary>
        /// Gets or sets the cfdi catalog.
        /// </summary>
        /// <value>
        /// The cfdi catalog.
        /// </value>
        public List<CFDIUsageCatalog> CFDICatalog { get; set; }

        /// <summary>
        /// Gets or sets the cfdi usage code.
        /// </summary>
        /// <value>
        /// The cfdi usage code.
        /// </value>
        public string CFDIUsageCode { get; set; }

        /// <summary>
        /// Gets or sets the cfdi usage desc.
        /// </summary>
        /// <value>
        /// The cfdi usage desc.
        /// </value>
        public string CFDIUsageDesc { get; set; }

        /// <summary>
        /// Gets or sets the name of the corporate.
        /// </summary>
        /// <value>
        /// The name of the corporate.
        /// </value>
        public string CorporateName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the invoice preferred taxpayer identifier.
        /// </summary>
        /// <value>
        /// The invoice preferred taxpayer identifier.
        /// </value>
        public int? InvoicePreferredTaxpayerId { get; set; }

        /// <summary>
        /// Gets or sets the invoice taxpayer identifier.
        /// </summary>
        /// <value>
        /// The invoice taxpayer identifier.
        /// </value>
        public int? InvoiceTaxpayerId { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the taxpayer identifier.
        /// </summary>
        /// <value>
        /// The taxpayer identifier.
        /// </value>
        public string TaxpayerId { get; set; }

        /// <summary>
        /// Gets or sets the tax regimen.
        /// </summary>
        /// <value>
        /// The tax regimen.
        /// </value>
        public string TaxRegimen { get; set; }

        /// <summary>
        /// Gets or sets the tax regimen desc.
        /// </summary>
        /// <value>
        /// The tax regimen desc.
        /// </value>
        public string TaxRegimenDesc { get; set; }
    }
}