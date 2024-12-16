// --------------------------------------------------------------------
// <copyright file="InvoiceCashReceiptViewModel.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace WebAdminUI.Models.FiscalRecords
{
    /// <summary>
    /// InvoiceCashReceiptViewModel
    /// </summary>
    public class InvoiceCashReceiptViewModel
    {
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        [Display(Name = "lblAmount", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CancelGlobalResource))]
        public string Amount { get; set; }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        /// <value>
        /// The receipt number.
        /// </value>
        [Display(Name = "lblEntryDate", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CancelGlobalResource))]
        public string EntryDate { get; set; }

        /// <summary>
        /// Gets or sets the people/organization fullname.
        /// </summary>
        /// <value>
        /// The people/organization fullname.
        /// </value>
        [Display(Name = "lblName", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CancelGlobalResource))]
        public string PeopleOrgFullName { get; set; }

        /// <summary>
        /// Gets or sets the people organization identifier.
        /// </summary>
        /// <value>
        /// The people organization identifier.
        /// </value>
        [Display(Name = "lblPeopleOrgId", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CancelGlobalResource))]
        public string PeopleOrgId { get; set; }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        /// <value>
        /// The receipt number.
        /// </value>
        [Display(Name = "lblReceiptNumber", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CancelGlobalResource))]
        public int ReceiptNumber { get; set; }

        /// <summary>
        /// Gets or sets the tax amount.
        /// </summary>
        /// <value>
        /// The tax amount.
        /// </value>
        [Display(Name = "lblTaxAmount", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.CancelGlobalResource))]
        public string TaxAmount { get; set; }
    }
}