// --------------------------------------------------------------------
// <copyright file="InvoiceOrganization.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

namespace PowerCampus.Entities
{
    public class InvoiceOrganization
    {
        /// <summary>
        /// Gets or sets the name of the corporate.
        /// </summary>
        /// <value>
        /// The name of the corporate.
        /// </value>
        public string CorporateName { get; set; }

        /// <summary>
        /// Gets or sets the invoice organization identifier.
        /// </summary>
        /// <value>
        /// The invoice organization identifier.
        /// </value>
        public int? InvoiceOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the taxpayer identifier.
        /// </summary>
        /// <value>
        /// The taxpayer identifier.
        /// </value>
        public string TaxpayerId { get; set; }

        #region TaxRegim

        /// <summary>
        /// Gets or sets the invoice org tax regimen identifier.
        /// </summary>
        /// <value>
        /// The invoice org tax regimen identifier.
        /// </value>
        public int? InvoiceOrgTaxRegimenId { get; set; }

        /// <summary>
        /// List of all Tax Regimen Catalogs
        /// </summary>
        public List<TaxRegimenCatalog> ListTaxRegimenCatalogs { get; set; }

        /// <summary>
        /// Gets or sets the tax regimen code.
        /// </summary>
        /// <value>
        /// The tax regimen code.
        /// </value>
        public string TaxRegimenCode { get; set; }

        /// <summary>
        /// Gets or sets the tax regimen description.
        /// </summary>
        /// <value>
        /// The tax regimen description.
        /// </value>
        public string TaxRegimenDesc { get; set; }

        /// <summary>
        /// Gets or sets the tax regimen identifier.
        /// </summary>
        /// <value>
        /// The tax regimen identifier.
        /// </value>
        public int? TaxRegimenId { get; set; }

        #endregion TaxRegim
    }
}