// --------------------------------------------------------------------
// <copyright file="CreatePaymentReceipt.cs" company="Ellucian">
//     Copyright 2018 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System;

namespace PowerCampus.Entities
{
    /// <summary>
    /// CreatePaymentReceipt class.
    /// </summary>
    /// <seealso cref="PowerCampus.Entities.CreateFiscalRecord" />
    public class CreatePaymentReceipt : CreateFiscalRecord
    {
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the amount paid.
        /// </summary>
        /// <value>
        /// The amount paid.
        /// </value>
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// Gets or sets the name of the bank.
        /// </summary>
        /// <value>
        /// The name of the bank.
        /// </value>
        public string BankName { get; set; }

        /// <summary>
        /// Gets or sets the beneficiary account.
        /// </summary>
        /// <value>
        /// The beneficiary account.
        /// </value>
        public string BeneficiaryAccount { get; set; }

        /// <summary>
        /// Gets or sets the currency complement.
        /// </summary>
        /// <value>
        /// The currency complement.
        /// </value>
        public string CurrencyComplement { get; set; }

        /// <summary>
        /// Gets or sets the installment number.
        /// </summary>
        /// <value>
        /// The installment number.
        /// </value>
        public int InstallmentNumber { get; set; }

        /// <summary>
        /// Gets or sets the issuer tax payer identifier beneficiary account.
        /// </summary>
        /// <value>
        /// The issuer tax payer identifier beneficiary account.
        /// </value>
        public string IssuerTaxPayerIdBeneficiaryAccount { get; set; }

        /// <summary>
        /// Gets or sets the issuer tax payer identifier source account.
        /// </summary>
        /// <value>
        /// The issuer tax payer identifier source account.
        /// </value>
        public string IssuerTaxPayerIdSourceAccount { get; set; }

        /// <summary>
        /// Gets or sets the outstanding balance amount.
        /// </summary>
        /// <value>
        /// The outstanding balance amount.
        /// </value>
        public decimal OutstandingBalanceAmount { get; set; }

        /// <summary>
        /// Gets or sets the payment certificate.
        /// </summary>
        /// <value>
        /// The payment certificate.
        /// </value>
        public string PaymentCertificate { get; set; }

        /// <summary>
        /// Gets or sets the payment chain.
        /// </summary>
        /// <value>
        /// The payment chain.
        /// </value>
        public string PaymentChain { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment chain.
        /// </summary>
        /// <value>
        /// The type of the payment chain.
        /// </value>
        public string PaymentChainType { get; set; }

        /// <summary>
        /// Gets or sets the payment date.
        /// </summary>
        /// <value>
        /// The payment date.
        /// </value>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Gets or sets the payment stamp.
        /// </summary>
        /// <value>
        /// The payment stamp.
        /// </value>
        public string PaymentStamp { get; set; }

        /// <summary>
        /// Gets or sets the payment type complement.
        /// </summary>
        /// <value>
        /// The payment type complement.
        /// </value>
        public string PaymentTypeComplement { get; set; }

        /// <summary>
        /// Gets or sets the previous balance amount.
        /// </summary>
        /// <value>
        /// The previous balance amount.
        /// </value>
        public decimal PreviousBalanceAmount { get; set; }

        /// <summary>
        /// Gets or sets the source account.
        /// </summary>
        /// <value>
        /// The source account.
        /// </value>
        public string SourceAccount { get; set; }

        /// <summary>
        /// Gets or sets the transaction number.
        /// </summary>
        /// <value>
        /// The transaction number.
        /// </value>
        public string TransactionNumber { get; set; }
    }
}