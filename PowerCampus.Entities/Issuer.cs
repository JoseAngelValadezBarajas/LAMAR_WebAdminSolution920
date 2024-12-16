// --------------------------------------------------------------------
// <copyright file="Issuer.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel;

namespace PowerCampus.Entities
{
    public class Issuer
    {
        /// <summary>
        /// Gets or sets the default issuer.
        /// </summary>
        /// <value>
        /// The default issuer.
        /// </value>
        public IssuerDefault DefaultIssuer { get; set; }

        /// <summary>
        /// Gets or sets the name of the iss corporate.
        /// </summary>
        /// <value>
        /// The name of the iss corporate.
        /// </value>
        [DisplayName("Corporate Name")]
        public string IssCorporateName { get; set; }

        /// <summary>
        /// Gets or sets the iss invoice organization identifier.
        /// </summary>
        /// <value>
        /// The iss invoice organization identifier.
        /// </value>
        public int? IssInvoiceOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the iss issuing address.
        /// </summary>
        /// <value>
        /// The iss issuing address.
        ///// </value>
        public List<IssuingAddress> IssIssuingAdd { get; set; }

        /// <summary>
        /// Gets or sets the iss serial.
        /// </summary>
        /// <value>
        /// The iss serial.
        /// </value>
        public List<IssuerSerial> IssSerial { get; set; }

        /// <summary>
        /// Gets or sets the iss taxpayer identifier.
        /// </summary>
        /// <value>
        /// The iss taxpayer identifier.
        /// </value>
        [DisplayName("Taxpayer Id")]
        public string IssTaxpayerId { get; set; }

        /// <summary>
        /// Gets or sets the iss tax regimen.
        /// </summary>
        /// <value>
        /// The iss tax regimen.
        /// </value>
        public List<TaxRegimen> IssTaxRegimen { get; set; }

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
    }

    /// <summary>
    /// IssuerDefault
    /// </summary>
    public class IssuerDefault
    {
        public int? IssChargeCreditCodeId { get; set; }
        public int? IssChargeCreditCodeTaxId { get; set; }
        public string IssCorporateName { get; set; }
        public int? IssCreditNoteReceiptId { get; set; }
        public int IssDefaultId { get; set; }
        public int? IssInvoiceExpeditionId { get; set; }
        public int? IssInvoiceOrganizationId { get; set; }
        public int? IssInvoiceReceiptId { get; set; }
        public string IssInvoiceTaxpayerId { get; set; }
        public string IssIssuingAddress { get; set; }
        public string IssPaymentConditions { get; set; }
        public string IssSerialNumber { get; set; }
        public string IssSerialNumberCreditNote { get; set; }
        public string IssUserName { get; set; }
    }

    public class IssuerSerial
    {
        public int IssInvoiceReceipt { get; set; }
        public int? IssLastFolio { get; set; }
        public string IssSerialNumber { get; set; }
    }

    /// <summary>
    /// IssuingAddress
    /// </summary>
    public class IssuingAddress
    {
        /// <summary>
        /// Gets or sets the iss by expedition.
        /// </summary>
        /// <value>
        /// The iss by expedition.
        /// </value>
        public int IssByExpedition { get; set; }

        /// <summary>
        /// Gets or sets the iss inovice expedition identifier.
        /// </summary>
        /// <value>
        /// The iss inovice expedition identifier.
        /// </value>
        public int IssInoviceExpeditionId { get; set; }

        /// <summary>
        /// Gets or sets the iss invoice organization identifier.
        /// </summary>
        /// <value>
        /// The iss invoice organization identifier.
        /// </value>
        public int IssInvoiceOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the iss issuing address.
        /// </summary>
        /// <value>
        /// The iss issuing address.
        /// </value>
        public string IssIssuingAddress { get; set; }

        /// <summary>
        /// Gets or sets the iss place issue.
        /// </summary>
        /// <value>
        /// The iss place issue.
        /// </value>
        public string IssPlaceIssue { get; set; }

        /// <summary>
        /// Gets or sets the iss postal code.
        /// </summary>
        /// <value>
        /// The iss postal code.
        /// </value>
        public string IssPostalCode { get; set; }
    }
}