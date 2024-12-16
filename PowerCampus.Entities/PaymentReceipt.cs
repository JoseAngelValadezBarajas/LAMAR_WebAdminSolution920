// --------------------------------------------------------------------
// <copyright file="PaymentReceipt.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System;

namespace PowerCampus.Entities
{
    public class PaymentReceipt : FiscalRecord
    {
        public decimal? Amount { get; set; }
        public decimal? AmountPaid { get; set; }
        public string BankName { get; set; }
        public string BeneficiaryAccount { get; set; }
        public string DocumentId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public int? InstallmentNumber { get; set; }
        public int? InvoicePaymentReceiptId { get; set; }
        public int? IssuerTaxpayerIdBeneficiaryAccount { get; set; }
        public string IssuerTaxpayerIdBeneficiaryAccountName { get; set; }
        public int? IssuerTaxpayerIdSourceAccount { get; set; }
        public string IssuerTaxpayerIdSourceAccountName { get; set; }
        public decimal? OutstandingBalanceAmount { get; set; }
        public string PaymentCertificate { get; set; }
        public string PaymentChain { get; set; }
        public string PaymentChainType { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentStamp { get; set; }
        public string PaymentTypeComplement { get; set; }
        public decimal? PreviousBalanceAmount { get; set; }
        public int? ReceiptNumber { get; set; }
        public string SourceAccount { get; set; }
        public string TransactionNumber { get; set; }
    }
}