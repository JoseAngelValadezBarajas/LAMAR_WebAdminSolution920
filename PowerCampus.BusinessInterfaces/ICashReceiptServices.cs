// --------------------------------------------------------------------
// <copyright file="ICashReceiptServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces
{
    /// <summary>
    /// ICashReceiptServices interface
    /// Contains the interface for the services used for Cash Receipts
    /// </summary>
    public interface ICashReceiptServices
    {
        /// <summary>
        /// Deletes the receipt payment mapping.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        int DeleteReceiptPaymentMapping(int Id);

        /// <summary>
        /// Get the cancelled global payment type.
        /// </summary>
        /// <param name="invoiceHeaderId">The cancelled global invoice header identifier.</param>
        /// <returns></returns>
        List<FiscalRecordCatalog> GetCancelledGlobalPaymentType(int invoiceHeaderId);

        /// <summary>
        /// Gets the cash receipt.
        /// </summary>
        /// <param name="receiptNumber">The receipt number.</param>
        /// <returns></returns>
        CashReceipt GetCashReceipt(int receiptNumber);

        /// <summary>
        /// Gets the cash receipt detail.
        /// </summary>
        /// <param name="receiptNumber">The receipt number.</param>
        /// <returns></returns>
        ChargeCreditApplication GetCashReceiptApplication(int receiptNumber);

        /// <summary>
        /// Gets the mapping product service.
        /// </summary>
        /// <returns></returns>
        List<ReceiptPaymentMethodMapping> GetChargeCreditMapping();

        /// <summary>
        /// Gets the charge credits for a receipt.
        /// </summary>
        /// <param name="receiptNumber">The receipt number.</param>
        /// <returns></returns>
        List<ChargeCredit> GetChargeCreditsByReceipt(int receiptNumber);

        /// <summary>
        /// Gets the global cash receipts.
        /// </summary>
        /// <param name="globalInvoiceFiltersForCreation">The global invoice filters for creation.</param>
        /// <returns></returns>
        CashReceipt GetGlobalCashReceipts(GlobalInvoiceFiltersForCreation globalInvoiceFiltersForCreation);

        /// <summary>
        /// Gets the global fiscal record.
        /// </summary>
        /// <returns></returns>
        CashReceipt GetGlobalFiscalRecord();

        /// <summary>
        /// Gets the global cash receipts of the previously canceled global invoice with reason 04.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier of the canceled invoice with reason 04.</param>
        /// <returns></returns>
        CashReceipt GetGlobalInvoiceDetail(int invoiceHeaderId);

        /// <summary>
        /// Gets the type of the global payment.
        /// </summary>
        /// <param name="globalInvoiceFiltersForCreation">The global invoice filters for creation.</param>
        /// <returns></returns>
        List<FiscalRecordCatalog> GetGlobalPaymentType(GlobalInvoiceFiltersForCreation globalInvoiceFiltersForCreation);

        /// <summary>
        /// Saves the receipt payment mapping.
        /// </summary>
        /// <param name="receiptPaymentMethodMapping">The receipt payment method mapping.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        int SaveReceiptPaymentMapping(ReceiptPaymentMethodMapping receiptPaymentMethodMapping, string userName);
    }
}