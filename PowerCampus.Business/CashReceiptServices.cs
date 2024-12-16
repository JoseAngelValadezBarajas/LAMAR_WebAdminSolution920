// --------------------------------------------------------------------
// <copyright file="CashReceiptServices.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using PowerCampus.BusinessInterfaces;
using PowerCampus.DataAccess;
using PowerCampus.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PowerCampus.Business
{
    /// <summary>
    /// CashReceiptServices class
    /// Contains the implementation of the ICashReceiptServices interface.
    /// Here are all the services used for Cash Receipts.
    /// <seealso cref="ICashReceiptServices"/>
    /// </summary>
    public class CashReceiptServices : ICashReceiptServices
    {
        private readonly CashReceiptDA _cashReceiptsDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="FiscalRecordServices"/> class.
        /// </summary>
        public CashReceiptServices()
        {
            _cashReceiptsDA = new CashReceiptDA();
        }

        /// <summary>
        /// Deletes the receipt payment mapping.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        public int DeleteReceiptPaymentMapping(int Id)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "DeleteReceiptPaymentMapping", "DeleteReceiptPaymentMapping starts");
            int result = _cashReceiptsDA.DeleteReceiptPaymentMapping(Id);
            // LoggerHelper.LogWebError("FiscalRecords", "DeleteReceiptPaymentMapping", "DeleteReceiptPaymentMapping ends");
            return result;
        }

        /// <summary>
        /// Get the cancelled global payment type.
        /// </summary>
        /// <param name="invoiceHeaderId">The cancelled global invoice header identifier.</param>
        /// <returns></returns>
        public List<FiscalRecordCatalog> GetCancelledGlobalPaymentType(int invoiceHeaderId)
        {
            List<FiscalRecordCatalog> lstPaymentType = _cashReceiptsDA.GetCancelledGlobalPaymentType(invoiceHeaderId);
            return lstPaymentType;
        }

        /// <summary>
        /// Gets the cash receipt.
        /// </summary>
        /// <param name="receiptNumber">The receipt number.</param>
        /// <returns></returns>
        public CashReceipt GetCashReceipt(int receiptNumber)
        {
            CashReceipt cashReceipt = new CashReceipt();
            // LoggerHelper.LogWebError("FiscalRecords", "GetCashReceipt", "GetCashReceipt starts");
            cashReceipt = _cashReceiptsDA.GetCashReceipt(receiptNumber, false);
            // LoggerHelper.LogWebError("FiscalRecords", "GetCashReceipt", "GetCashReceipt ends");
            return cashReceipt;
        }

        /// <summary>
        /// Gets the cash receipt application.
        /// </summary>
        /// <param name="receiptNumber">The receipt number.</param>
        /// <returns>The information about the application of a receipt</returns>
        public ChargeCreditApplication GetCashReceiptApplication(int receiptNumber)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetCashReceiptApplication", "GetCashReceiptApplication starts");
            if (receiptNumber <= 0)
            {
                LoggerHelper.LogWebError("FiscalRecords", "GetCashReceiptApplication", "GetCashReceiptApplication ends - receiptNumber <= 0");
                return null;
            }
            ChargeCreditApplication chargeCreditApplication = _cashReceiptsDA.GetCashReceiptApplication(receiptNumber);
            // LoggerHelper.LogWebError("FiscalRecords", "GetCashReceiptApplication", "GetCashReceiptApplication ends");
            return chargeCreditApplication;
        }

        /// <summary>
        /// Gets the mapping product service.
        /// </summary>
        /// <returns></returns>
        public List<ReceiptPaymentMethodMapping> GetChargeCreditMapping()
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetChargeCreditMapping", "GetChargeCreditMapping starts");
            List<ReceiptPaymentMethodMapping> result = _cashReceiptsDA.GetChargeCreditMapping();
            // LoggerHelper.LogWebError("FiscalRecords", "GetChargeCreditMapping", "GetChargeCreditMapping ends");
            return result?.OrderBy(m => m.ChargeCreditDesc).ToList();
        }

        /// <summary>
        /// Gets the charge credits for a receipt.
        /// </summary>
        /// <param name="receiptNumber">The receipt number.</param>
        /// <returns></returns>
        public List<ChargeCredit> GetChargeCreditsByReceipt(int receiptNumber)
        {
            List<ChargeCredit> chargeCredits = _cashReceiptsDA.GetChargeCreditsByReceipt(receiptNumber);
            return chargeCredits;
        }

        /// <summary>
        /// Gets the global cash receipts.
        /// </summary>
        /// <param name="globalInvoiceFiltersForCreation">The global invoice filters for creation.</param>
        /// <returns></returns>
        public CashReceipt GetGlobalCashReceipts(GlobalInvoiceFiltersForCreation globalInvoiceFiltersForCreation)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetGlobalCashReceipts", "GetGlobalFiscalRecord starts");
            CashReceipt globalCashReceipts = _cashReceiptsDA.GetGlobalCashReceipts(globalInvoiceFiltersForCreation, false, 0);
            // LoggerHelper.LogWebError("FiscalRecords", "GetGlobalCashReceipts", "GetGlobalFiscalRecord ends");
            return globalCashReceipts;
        }

        /// <summary>
        /// Gets the global fiscal record.
        /// </summary>
        /// <returns></returns>
        public CashReceipt GetGlobalFiscalRecord()
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetGlobalFiscalRecord", "GetGlobalFiscalRecord starts");
            CashReceipt globalFiscalRec = _cashReceiptsDA.GetGlobalFiscalRecord();
            // LoggerHelper.LogWebError("FiscalRecords", "GetGlobalFiscalRecord", "GetGlobalFiscalRecord ends");
            return globalFiscalRec;
        }

        /// <summary>
        /// Gets the global cash receipts of the previously canceled global invoice with reason 04.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier of the canceled invoice with reason 04.</param>
        /// <returns></returns>
        public CashReceipt GetGlobalInvoiceDetail(int invoiceHeaderId)
        {
            CashReceipt globalFiscalRecord = _cashReceiptsDA.GetGlobalInvoiceDetail(invoiceHeaderId);
            return globalFiscalRecord;
        }

        /// <summary>
        /// Gets the type of the global payment.
        /// </summary>
        /// <param name="globalInvoiceFiltersForCreation">The global invoice filters for creation.</param>
        /// <returns></returns>
        public List<FiscalRecordCatalog> GetGlobalPaymentType(GlobalInvoiceFiltersForCreation globalInvoiceFiltersForCreation)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetGlobalPaymentType", "GetGlobalPaymentType starts");
            List<FiscalRecordCatalog> lstPaymentType = _cashReceiptsDA.GetGlobalPaymentType(globalInvoiceFiltersForCreation);
            // LoggerHelper.LogWebError("FiscalRecords", "GetGlobalPaymentType", "GetGlobalPaymentType ends");
            return lstPaymentType;
        }

        /// <summary>
        /// Saves the receipt payment mapping.
        /// </summary>
        /// <returns></returns>
        public int SaveReceiptPaymentMapping(ReceiptPaymentMethodMapping receiptPaymentMethodMapping, string userName)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "SaveReceiptPaymentMapping", "SaveReceiptPaymentMapping starts");
            int result = _cashReceiptsDA.SaveReceiptPaymentMapping(receiptPaymentMethodMapping, userName);
            // LoggerHelper.LogWebError("FiscalRecords", "SaveReceiptPaymentMapping", "SaveReceiptPaymentMapping ends");
            return result;
        }
    }
}