// --------------------------------------------------------------------
// <copyright file="ChargeCreditViewModel.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace WebAdminUI.Models.PaymentReceipt
{
    /// <summary>
    /// PaymentReceiptTotalsViewModel class.
    /// </summary>
    public class PaymentReceiptTotalsViewModel
    {
        /// <summary>
        /// Gets or sets the total payments amount.
        /// </summary>
        /// <value>
        /// The total payments amount.
        /// </value>
        [Display(Name = "lblTotalPaymentsAmount", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string TotalPaymentsAmount { get; set; }

        /// <summary>
        /// Gets or sets the total transferred base0.
        /// </summary>
        /// <value>
        /// The total transferred base0.
        /// </value>
        [Display(Name = "lblTotalTransferredBase0", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string TotalTransferredBase0 { get; set; }

        /// <summary>
        /// Gets or sets the total transferred base16.
        /// </summary>
        /// <value>
        /// The total transferred base16.
        /// </value>
        [Display(Name = "lblTotalTransferredBase16", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string TotalTransferredBase16 { get; set; }

        /// <summary>
        /// Gets or sets the total transferred base8.
        /// </summary>
        /// <value>
        /// The total transferred base8.
        /// </value>
        [Display(Name = "lblTotalTransferredBase8", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string TotalTransferredBase8 { get; set; }

        /// <summary>
        /// Gets or sets the total transferred base exempt.
        /// </summary>
        /// <value>
        /// The total transferred base exempt.
        /// </value>
        [Display(Name = "lblTotalTransferredBaseExempt", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string TotalTransferredBaseExempt { get; set; }

        /// <summary>
        /// Gets or sets the total transferred taxes0.
        /// </summary>
        /// <value>
        /// The total transferred taxes0.
        /// </value>
        [Display(Name = "lblTotalTransferredTaxes0", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string TotalTransferredTaxes0 { get; set; }

        /// <summary>
        /// Gets or sets the total transferred taxes16.
        /// </summary>
        /// <value>
        /// The total transferred taxes16.
        /// </value>
        [Display(Name = "lblTotalTransferredTaxes16", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string TotalTransferredTaxes16 { get; set; }

        /// <summary>
        /// Gets or sets the total transferred taxes8.
        /// </summary>
        /// <value>
        /// The total transferred taxes8.
        /// </value>
        [Display(Name = "lblTotalTransferredTaxes8", ResourceType = typeof(Views.FiscalRecords.App_LocalResources.EditResource))]
        public string TotalTransferredTaxes8 { get; set; }
    }
}