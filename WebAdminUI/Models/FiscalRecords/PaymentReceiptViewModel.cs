// <copyright file="PaymentReceiptViewModel.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAdminUI.Models.FiscalRecords
{
    /// <summary>
    /// PaymentReceiptViewModel
    /// </summary>
    public class PaymentReceiptViewModel : FiscalRecordViewModel
    {
        /// <summary>
        /// The amount of the payment
        /// </summary>
        [Display(Name = "Amount", ResourceType = typeof(Views.PaymentReceipts.App_LocalResources.CreateResource))]
        public string Amount { get; set; }

        /// <summary>
        /// The amount paid
        /// </summary>
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// Currency of the complement
        /// </summary>
        [Display(Name = "ComplementCurrency", ResourceType = typeof(Views.PaymentReceipts.App_LocalResources.CreateResource))]
        public string CurrencyComplement { get; set; }

        /// <summary>
        /// Detail of the PaymentComplement
        /// </summary>
        public ChargeCreditViewModel Detail { get; set; }

        /// <summary>
        /// The name of the bank. For foreigners issuers.
        /// </summary>
        [Display(Name = "OriginBankName", ResourceType = typeof(Views.PaymentReceipts.App_LocalResources.CreateResource))]
        public string ForeignerBankName { get; set; }

        /// <summary>
        /// Gets or sets the installment number.
        /// </summary>
        /// <value>
        /// The installment number.
        /// </value>
        public int InstallmentNumber { get; set; }

        /// <summary>
        /// The origin account
        /// </summary>
        [Display(Name = "OriginAccount", ResourceType = typeof(Views.PaymentReceipts.App_LocalResources.CreateResource))]
        public string OriginAccount { get; set; }

        /// <summary>
        /// The Fiscal Identity Number of the Issuer's origin-bank
        /// </summary>
        [Display(Name = "IssuerTaxPayerIdOriginAccount", ResourceType = typeof(Views.PaymentReceipts.App_LocalResources.CreateResource))]
        public string OriginAccountIssuerTaxPayerId { get; set; }

        /// <summary>
        /// The difference of the previous amount debt minus the amount paid
        /// </summary>
        public decimal OutstandingBalanceAmount { get; set; }

        /// <summary>
        /// The certificate related to the payment
        /// </summary>
        [Display(Name = "PaymentCertificate", ResourceType = typeof(Views.PaymentReceipts.App_LocalResources.CreateResource))]
        public string PaymentCertificate { get; set; }

        /// <summary>
        /// Date of the payment
        /// </summary>
        [Display(Name = "PaymentDate", ResourceType = typeof(Views.PaymentReceipts.App_LocalResources.CreateResource))]
        public string PaymentDate { get; set; }

        /// <summary>
        /// Original chain related to the payment
        /// </summary>
        [Display(Name = "PaymentChain", ResourceType = typeof(Views.PaymentReceipts.App_LocalResources.CreateResource))]
        public string PaymentOriginalChain { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment original chain.
        /// </summary>
        /// <value>
        /// The type of the payment original chain.
        /// </value>
        [Display(Name = "PaymentChainType", ResourceType = typeof(Views.PaymentReceipts.App_LocalResources.CreateResource))]
        public string PaymentOriginalChainType { get; set; }

        /// <summary>
        /// Digital Signature related to the payment
        /// </summary>
        [Display(Name = "PaymentSignature", ResourceType = typeof(Views.PaymentReceipts.App_LocalResources.CreateResource))]
        public string PaymentSignature { get; set; }

        /// <summary>
        /// Payment type of the complement
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Views.FiscalRecords.App_LocalResources.CreateResource), ErrorMessageResourceName = "ComplementPaymentTypeRequired")]
        [Display(Name = "ComplementPaymentType", ResourceType = typeof(Views.PaymentReceipts.App_LocalResources.CreateResource))]
        public string PaymentTypeComplement { get; set; }

        /// <summary>
        /// The previous amount debt
        /// </summary>
        public decimal PreviousBalanceAmount { get; set; }

        /// <summary>
        /// Gets or sets the subject to tax.
        /// </summary>
        /// <value>
        /// The subject to tax.
        /// </value>
        public string SubjectToTax { get; set; }

        /// <summary>
        /// The target account
        /// </summary>
        [Display(Name = "TargetAccount", ResourceType = typeof(Views.PaymentReceipts.App_LocalResources.CreateResource))]
        public string TargetAccount { get; set; }

        /// <summary>
        /// The Fiscal Identity Number of the Issuer's target-bank
        /// </summary>
        [Display(Name = "IssuerTaxPayerIdTargetAccount", ResourceType = typeof(Views.PaymentReceipts.App_LocalResources.CreateResource))]
        public string TargetAccountIssuerTaxPayerId { get; set; }

        /// <summary>
        /// The number of the operation associated with the payment
        /// </summary>
        [Display(Name = "TransactionNumber", ResourceType = typeof(Views.PaymentReceipts.App_LocalResources.CreateResource))]
        public string TransactionNumber { get; set; }
    }
}