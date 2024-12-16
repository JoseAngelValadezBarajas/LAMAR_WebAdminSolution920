// --------------------------------------------------------------------
// <copyright file="ChargeCreditViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace WebAdminUI.Models.FiscalRecords
{
    /// <summary>
    /// ChargeCreditViewModel
    /// </summary>
    public class ChargeCreditViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance can create credit note.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can create credit note; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "lblAddCreditNote", ResourceType = typeof(Views.CreditNotes.App_localResources.CreditNotes_cshtml))]
        public bool CanCreateCreditNote { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can create PPD.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can create PPD; otherwise, <c>false</c>.
        /// </value>
        public bool CanCreatePPD { get; set; }

        /// <summary>
        /// Gets or sets the charge credit code.
        /// </summary>
        /// <value>
        /// The charge credit code.
        /// </value>
        [Display(Name = "IdNumber", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string ChargeCreditCode { get; set; }

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
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "ChargeCreditDescRequired")]
        [Display(Name = "Description", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string ChargeCreditDesc { get; set; }

        /// <summary>
        /// Gets or sets the charge number source.
        /// </summary>
        /// <value>
        /// The charge number source.
        /// </value>
        public int? ChargeNumberSource { get; set; }

        /// <summary>
        /// Gets or sets the distribution order
        /// </summary>
        /// <value>
        /// The distribution order
        /// </value>
        public string DistributionOrder { get; set; }

        /// <summary>
        /// Gets or sets the entry date.
        /// </summary>
        /// <value>
        /// The entry date.
        /// </value>
        public string EntryDate { get; set; }

        /// <summary>
        /// Gets or sets the invoice detail identifier.
        /// </summary>
        /// <value>
        /// The invoice detail identifier.
        /// </value>
        public int InvoiceDetailId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a tax.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is a tax; otherwise, <c>false</c>.
        /// </value>
        public bool isATax { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is empty product service key.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is empty product service key; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmptyProductServiceKey { get; set; }

        /// <summary>
        /// Gets or sets the people/organization code id.
        /// </summary>
        /// <value>
        /// The people/organization code id.
        /// </value>
        [Display(Name = "PeopleOrganizationID", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string PeopleOrgCodeId { get; set; }

        /// <summary>
        /// Gets or sets the people org code identifier complete.
        /// </summary>
        /// <value>
        /// The people org code identifier complete.
        /// </value>
        public string PeopleOrgCodeIdComplete { get; set; }

        /// <summary>
        /// Gets or sets the people/organization full name.
        /// </summary>
        /// <value>
        /// The people/organization full name.
        /// </value>
        [Display(Name = "PeopleOrganizationName", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string PeopleOrgFullName { get; set; }

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
        [Display(Name = "ProductServiceCode", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string ProductServiceKey { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        [Display(Name = "Quantity", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        /// <value>
        /// The receipt number.
        /// </value>
        [Display(Name = "receiptNumber", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public int? ReceiptNumber { get; set; }

        /// <summary>
        /// Gets or sets the subject to tax.
        /// </summary>
        /// <value>
        /// The subject to tax.
        /// </value>
        [Display(Name = "SubjectToTax", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string SubjectToTax { get; set; }

        /// <summary>
        /// Gets or sets the tax charge number.
        /// </summary>
        /// <value>
        /// The tax charge number.
        /// </value>
        public int? TaxChargeNumber { get; set; }

        /// <summary>
        /// Gets or sets the total taxes.
        /// </summary>
        /// <value>
        /// The total taxes.
        /// </value>
        [Display(Name = "TaxAmount", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string TotalTaxes { get; set; }

        /// <summary>
        /// Gets or sets the total unit.
        /// </summary>
        /// <value>
        /// The total unit.
        /// </value>
        [Display(Name = "Amount", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string TotalUnit { get; set; }

        /// <summary>
        /// Gets or sets the transfer rate.
        /// </summary>
        /// <value>
        /// The transfer rate.
        /// </value>
        public decimal TransferRate { get; set; }

        /// <summary>
        /// Gets or sets the unit amount.
        /// </summary>
        /// <value>
        /// The unit amount.
        /// </value>
        [Display(Name = "UnitAmount", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string UnitAmount { get; set; }

        /// <summary>
        /// Gets or sets the unity key.
        /// </summary>
        /// <value>
        /// The unity key.
        /// </value>
        [Display(Name = "UnitCode", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string UnityKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the unity.
        /// </summary>
        /// <value>
        /// The name of the unity.
        /// </value>
        [Display(Name = "Unit", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource))]
        public string UnityName { get; set; }
    }
}