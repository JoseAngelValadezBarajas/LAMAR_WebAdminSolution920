// --------------------------------------------------------------------
// <copyright file="IFiscalRecordServices.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IFiscalRecordServices
    {
        /// <summary>
        /// Creates the fiscal record.
        /// </summary>
        /// <param name="fiscalRecord">The fiscal record.</param>
        /// <returns></returns>
        int CreateFiscalRecord(CreateFiscalRecord fiscalRecord);

        /// <summary>
        /// Creates the global fiscal record.
        /// </summary>
        /// <param name="globalfiscalRecord">The globalfiscal record.</param>
        /// <returns></returns>
        int CreateGlobalFiscalRecord(CreateFiscalRecord globalfiscalRecord);

        /// <summary>
        /// Creates the global fiscal record for reason 04.
        /// </summary>
        /// <param name="globalfiscalRecord">The global fiscal record.</param>
        /// <returns></returns>
        int CreateGlobalFiscalRecordForReason04(CreateFiscalRecord globalfiscalRecord);

        /// <summary>
        /// Creates the payment receipt.
        /// </summary>
        /// <param name="createPaymentReceipt">The create payment receipt.</param>
        /// <returns></returns>
        int CreatePaymentReceipt(CreatePaymentReceipt createPaymentReceipt);

        /// <summary>
        /// Deletes the fiscal record.
        /// </summary>
        /// <param name="invoiceHeaderId">The fiscal record request.</param>
        /// <param name="userName"></param>
        /// <returns></returns>
        int DeleteFiscalRecord(int invoiceHeaderId, string userName);

        /// <summary>
        /// Gets all fiscal records.
        /// </summary>
        /// <param name="invoiceFilters">The invoice filters.</param>
        /// <returns></returns>
        List<FiscalRecord> GetAllFiscalRecords(InvoiceFilters invoiceFilters);

        /// <summary>
        /// Gets the certificate files.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <param name="certificateFileId">The certificate file identifier.</param>
        /// <returns></returns>
        FiscalRecordCertificate GetCertificateFiles(int invoiceHeaderId, int certificateFileId);

        /// <summary>
        /// Gets the fiscal record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        FiscalRecord GetFiscalRecord(int id);

        /// <summary>
        /// Gets the fiscal record by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        FiscalRecord GetFiscalRecordById(int id);

        /// <summary>
        /// Get the fiscal records related to a fiscal record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        List<FiscalRecord>[] GetFiscalRecordsRelated(int id);

        /// <summary>
        /// Gets for substitution.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <param name="isGlobal">if set to <c>true</c> [is global].</param>
        /// <returns></returns>
        CashReceipt GetForSubstitution(int invoiceHeaderId, bool isGlobal);

        /// <summary>
        /// Gets the global invoice cancellation details.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <returns></returns>
        GlobalInvoiceCancellationDetails GetGlobalInvoiceCancellationDetails(int invoiceHeaderId);

        /// <summary>
        /// Gets the available global invoice filters.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        GlobalInvoiceFilters GetGlobalInvoiceFilters(string startDate, string endDate);

        /// <summary>
        /// Gets the payment complement.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <param name="receiptNumber">The receipt number.</param>
        /// <returns></returns>
        PaymentComplement GetPaymentComplement(int invoiceHeaderId, int receiptNumber);

        /// <summary>
        /// Get the payment receipts related to a fiscal record
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        List<PaymentReceipt>[] GetPaymentReceiptsRelated(int id);
    }
}