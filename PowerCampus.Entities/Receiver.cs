// --------------------------------------------------------------------
// <copyright file="Receiver.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities
{
    /// <summary>
    /// Receiver Class
    /// </summary>
    public class Receiver
    {
        /// <summary>
        /// Gets or sets the name of the corporate.
        /// </summary>
        /// <value>
        /// The name of the corporate.
        /// </value>
        public string CorporateName { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the fiscal identity number.
        /// </summary>
        /// <value>
        /// The fiscal identity number.
        /// </value>
        public string FiscalIdentityNumber { get; set; }

        /// <summary>
        /// Gets or sets the fiscal residency.
        /// </summary>
        /// <value>
        /// The fiscal residency.
        /// </value>
        public string FiscalResidency { get; set; }

        /// <summary>
        /// Gets or sets the fiscal residency desc.
        /// </summary>
        /// <value>
        /// The fiscal residency desc.
        /// </value>
        public string FiscalResidencyDesc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has invoice.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has invoice; otherwise, <c>false</c>.
        /// </value>
        public bool HasInvoice { get; set; }

        /// <summary>
        /// Gets or sets the invoice foreign taxpayer identifier.
        /// </summary>
        /// <value>
        /// The invoice foreign taxpayer identifier.
        /// </value>
        public int? InvoiceForeignTaxpayerId { get; set; }

        /// <summary>
        /// Gets or sets the invoice tax payer identifier.
        /// </summary>
        /// <value>
        /// The invoice tax payer identifier.
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
        /// Gets or sets the preferred cfdi usage code.
        /// </summary>
        /// <value>
        /// The preferred cfdi usage code.
        /// </value>
        public string PreferredCFDIUsageCode { get; set; }

        /// <summary>
        /// Gets or sets the tax payer identifier.
        /// </summary>
        /// <value>
        /// The tax payer identifier.
        /// </value>
        public string TaxPayerId { get; set; }

        /// <summary>
        /// Gets or sets the tax regimen code.
        /// </summary>
        /// <value>
        /// The tax regimen code.
        /// </value>
        public string TaxRegimenCode { get; set; }

        /// <summary>
        /// Gets or sets the tax regimen desc.
        /// </summary>
        /// <value>
        /// The tax regimen desc.
        /// </value>
        public string TaxRegimenDesc { get; set; }

        /// <summary>
        /// Gets or sets the tax regimen.
        /// </summary>
        /// <value>
        /// The tax regimen.
        /// </value>
        public int? TaxRegimenId { get; set; }
        public string StreetName { get; set; }

        public string StreetNumber { get; set; }

        public string ApartmentNumber { get; set; }

        public string Neighborhood { get; set; }

        public string Location { get; set; }

        public string StateProvinceId { get; set; }

        public string CountryId { get; set; }
    }
}