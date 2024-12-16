// --------------------------------------------------------------------
// <copyright file="FiscalRecordServices.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using PowerCampus.BusinessInterfaces;
using PowerCampus.DataAccess;
using PowerCampus.Entities;
using PowerCampus.Entities.Enum;
using System.Collections.Generic;
using System.Linq;

namespace PowerCampus.Business
{
    /// <summary>
    /// FiscalRecordServices Class
    /// </summary>
    public class FiscalRecordServices : IFiscalRecordServices
    {
        /// <summary>
        /// The cash receipt da
        /// </summary>
        private readonly CashReceiptDA _cashReceiptDA;

        /// <summary>
        /// The catalog services
        /// </summary>
        private readonly CatalogServices _catalogServices;

        /// <summary>
        /// The fiscal record da
        /// </summary>
        private readonly FiscalRecordDA _fiscalRecordDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="FiscalRecordServices"/> class.
        /// </summary>
        public FiscalRecordServices()
        {
            _fiscalRecordDA = new FiscalRecordDA();
            _cashReceiptDA = new CashReceiptDA();
            _catalogServices = new CatalogServices();
        }

        /// <summary>
        /// Creates the fiscal record.
        /// </summary>
        /// <param name="fiscalRecord">The fiscal record.</param>
        /// <returns></returns>
        public int CreateFiscalRecord(CreateFiscalRecord fiscalRecord)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "CreateFiscalRecord", "CreateFiscalRecord starts");
            if (fiscalRecord == null)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CreateFiscalRecord", "CreateFiscalRecord ends - fiscalRecord == null");
                return -1;
            }
            bool? isSubstitution = SetSubstitutionInfoToSave(fiscalRecord);
            if (!isSubstitution.HasValue)
                return -1;
            fiscalRecord.FiscalRecordType = Constants.FiscalRecordTypeIngreso;
            fiscalRecord.IsGlobal = false;
            fiscalRecord.IsSubstitution = isSubstitution.Value;
            int invoiceHeaderId = _fiscalRecordDA.CreateFiscalRecord(fiscalRecord);
            // LoggerHelper.LogWebError("FiscalRecords", "CreateFiscalRecord", "CreateFiscalRecord ends");

            return invoiceHeaderId;
        }

        /// <summary>
        /// Creates the global fiscal record.
        /// </summary>
        /// <param name="fiscalRecord">The fiscal record.</param>
        /// <returns></returns>
        public int CreateGlobalFiscalRecord(CreateFiscalRecord fiscalRecord)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "CreateGlobalFiscalRecord", "CreateGlobalFiscalRecord starts");

            bool? isSubstitution = SetSubstitutionInfoToSave(fiscalRecord);
            if (!isSubstitution.HasValue)
                return -1;
            fiscalRecord.FiscalRecordType = Constants.FiscalRecordTypeIngreso;
            fiscalRecord.IsGlobal = true;
            fiscalRecord.IsSubstitution = isSubstitution.Value;
            int invoiceHeaderId = _fiscalRecordDA.CreateFiscalRecord(fiscalRecord);
            // LoggerHelper.LogWebError("FiscalRecords", "CreateGlobalFiscalRecord", "CreateGlobalFiscalRecord ends");
            return invoiceHeaderId;
        }

        /// <summary>
        /// Creates the global fiscal record for reason 04.
        /// </summary>
        /// <param name="globalfiscalRecord">The global fiscal record.</param>
        /// <returns></returns>
        public int CreateGlobalFiscalRecordForReason04(CreateFiscalRecord globalfiscalRecord)
        {
            int invoiceHeaderId = _fiscalRecordDA.CreateGlobalForReason04(globalfiscalRecord);
            return invoiceHeaderId;
        }

        /// <summary>
        /// Creates the payment receipt.
        /// </summary>
        /// <param name="createPaymentReceipt">The create payment receipt.</param>
        /// <returns></returns>
        public int CreatePaymentReceipt(CreatePaymentReceipt createPaymentReceipt)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "CreatePaymentReceipt", "CreatePaymentReceipt starts");
            if (createPaymentReceipt.Equals(null))
            {
                LoggerHelper.LogWebError("FiscalRecords", "CreatePaymentReceipt", "CreatePaymentReceipt ends - fiscalRecord == null");
                return -1;
            }
            if (createPaymentReceipt.ReceiptNumber is null || createPaymentReceipt.ReceiptNumber <= 0)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CreatePaymentReceipt", "CreatePaymentReceipt ends - ReceiptNumber == null or <= 0");
                return -1;
            }
            FiscalRecord fiscalRecord = GetFiscalRecord(createPaymentReceipt.CFDIRelatedId.Value);
            createPaymentReceipt.PeopleOrgCodeId = fiscalRecord.PeopleOrgCodeId;
            createPaymentReceipt.CFDIRelated = fiscalRecord.UUID;
            createPaymentReceipt.PaymentCondition = string.Empty;
            createPaymentReceipt.PaymentMethod = string.Empty;
            createPaymentReceipt.PaymentMethodDesc = string.Empty;
            createPaymentReceipt.PaymentType = string.Empty;
            createPaymentReceipt.PaymentTypeDesc = string.Empty;

            if (createPaymentReceipt.CFDIRelatedId is null || createPaymentReceipt.CFDIRelatedId <= 0)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CreatePaymentReceipt", "CreatePaymentReceipt ends - CFDIRelatedId == null or <= 0");
                return -1;
            }

            createPaymentReceipt.FiscalRecordType = Constants.FiscalRecordTypePago;
            createPaymentReceipt.IsGlobal = false;
            createPaymentReceipt.IsSubstitution = false;
            int invoiceHeaderId = _fiscalRecordDA.CreateFiscalRecord(createPaymentReceipt);

            // LoggerHelper.LogWebError("FiscalRecords", "CreatePaymentReceipt", "CreatePaymentReceipt ends");
            return invoiceHeaderId;
        }

        /// <summary>
        /// Deletes the fiscal record.
        /// </summary>
        /// <param name="invoiceHeaderId">The fiscal record request.</param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int DeleteFiscalRecord(int invoiceHeaderId, string userName)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "DeleteFiscalRecord", "DeleteFiscalRecord starts");
            int deletedInvoiceHeader = _fiscalRecordDA.DeleteFiscalRecord(invoiceHeaderId, userName);
            // LoggerHelper.LogWebError("FiscalRecords", "DeleteFiscalRecord", "DeleteFiscalRecord ends");
            return deletedInvoiceHeader;
        }

        /// <summary>
        /// Gets all fiscal records.
        /// </summary>
        /// <param name="invoiceFilters">The invoice filters.</param>
        /// <returns></returns>
        public List<FiscalRecord> GetAllFiscalRecords(InvoiceFilters invoiceFilters)
        {
            List<FiscalRecord> fiscalRecordsList = _fiscalRecordDA.RetrieveFiscalRecords(invoiceFilters);
            FiscalRecordCancelReason relationErrorCancelReason = _catalogServices.GetFiscalRecordCancelReason(CancelReasonName.ErrorRelacion.ToString());
            FiscalRecordCancelReason nominativeOperationCancelReason = _catalogServices.GetFiscalRecordCancelReason(CancelReasonName.OperacionNominativa.ToString());

            if (relationErrorCancelReason != null && nominativeOperationCancelReason != null && fiscalRecordsList != null)
            {
                foreach (FiscalRecord fiscalRecord in fiscalRecordsList)
                {
                    if (!string.IsNullOrEmpty(fiscalRecord.CancelReasonKey) && fiscalRecord.CancelReasonKey.Equals(relationErrorCancelReason.Code))
                        fiscalRecord.CancelReasonName = CancelReasonName.ErrorRelacion;
                    else if (!string.IsNullOrEmpty(fiscalRecord.CancelReasonKey) && fiscalRecord.CancelReasonKey.Equals(nominativeOperationCancelReason.Code))
                        fiscalRecord.CancelReasonName = CancelReasonName.OperacionNominativa;
                }
            }
            return fiscalRecordsList;
        }

        /// <summary>
        /// Gets the certificate files.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <param name="certificateFileId">The certificate file identifier.</param>
        /// <returns></returns>
        public FiscalRecordCertificate GetCertificateFiles(int invoiceHeaderId, int certificateFileId)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetCertificateFiles", "GetCertificateFiles starts");
            List<FiscalRecordCertificate> certificateFiles = _fiscalRecordDA.GetFiscalRecordCertificateFiles(invoiceHeaderId);
            FiscalRecordCertificate certificate = certificateFiles.Find(m => m.FiscalRecordCertificateId == certificateFileId);
            // LoggerHelper.LogWebError("FiscalRecords", "GetCertificateFiles", "GetCertificateFiles ends");
            return certificate;
        }

        /// <summary>
        /// Gets the fiscal record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public FiscalRecord GetFiscalRecord(int id)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetFiscalRecord", "GetFiscalRecord starts");
            FiscalRecord fiscalRecord = _fiscalRecordDA.GetFiscalRecord(id);
            FiscalRecordCancelReason relationErrorCancelReason = _catalogServices.GetFiscalRecordCancelReason(CancelReasonName.ErrorRelacion.ToString());
            FiscalRecordCancelReason nominativeOperationCancelReason = _catalogServices.GetFiscalRecordCancelReason(CancelReasonName.OperacionNominativa.ToString());

            if (fiscalRecord != null)
            {
                if (!string.IsNullOrEmpty(fiscalRecord.CancelReasonKey))
                {
                    if (relationErrorCancelReason != null && fiscalRecord.CancelReasonKey.Equals(relationErrorCancelReason.Code))
                        fiscalRecord.CancelReasonName = CancelReasonName.ErrorRelacion;
                    else if (nominativeOperationCancelReason != null && fiscalRecord.CancelReasonKey.Equals(nominativeOperationCancelReason.Code))
                        fiscalRecord.CancelReasonName = CancelReasonName.OperacionNominativa;
                }

                if (string.IsNullOrEmpty(fiscalRecord.PeopleOrgCodeId))
                    fiscalRecord.CancelReasonCatalogList = _catalogServices.GetFiscalRecordCancelReason(false, true);
                else
                    fiscalRecord.CancelReasonCatalogList = _catalogServices.GetFiscalRecordCancelReason(true, false);
            }
            // LoggerHelper.LogWebError("FiscalRecords", "GetFiscalRecord", "GetFiscalRecord ends");
            return fiscalRecord;
        }

        /// <summary>
        /// Gets the fiscal record by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public FiscalRecord GetFiscalRecordById(int id)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetFiscalRecordById", "GetFiscalRecordById starts");
            FiscalRecord fiscalRecord = _fiscalRecordDA.GetFiscalRecordById(id);
            return fiscalRecord;
        }

        /// <summary>
        /// Get the fiscal records related to a fiscal record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public List<FiscalRecord>[] GetFiscalRecordsRelated(int id)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetFiscalRecordsRelated", "GetFiscalRecordsRelated starts");
            if (id <= 0)
            {
                LoggerHelper.LogWebError("FiscalRecords", "GetFiscalRecordsRelated", "GetFiscalRecordsRelated ends - id <= 0");
                return null;
            }
            List<FiscalRecord>[] lstFiscalRecord = _fiscalRecordDA.GetFiscalRecordsRelated(id);
            // LoggerHelper.LogWebError("FiscalRecords", "GetFiscalRecordsRelated", "GetFiscalRecordsRelated ends");
            return lstFiscalRecord;
        }

        /// <summary>
        /// Gets for substitution.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <param name="isGlobal">if set to <c>true</c> [is global].</param>
        /// <returns></returns>
        public CashReceipt GetForSubstitution(int invoiceHeaderId, bool isGlobal)
        {
            CashReceipt cashReceipt;
            if (isGlobal)
            {
                cashReceipt = _cashReceiptDA.GetGlobalCashReceipts(null, true, invoiceHeaderId);
                CashReceipt cashReceiptGlobal = _cashReceiptDA.GetGlobalFiscalRecord();
                if (cashReceipt != null && cashReceiptGlobal != null)
                {
                    cashReceipt.CFDIList = cashReceiptGlobal.CFDIList;
                    cashReceipt.ReceiverPaymentMethodList = cashReceiptGlobal.ReceiverPaymentMethodList;
                    cashReceipt.ReceiverPaymentMethodDefault = cashReceiptGlobal.ReceiverPaymentMethodDefault;
                    cashReceipt.PaymentTypeList = cashReceiptGlobal.PaymentTypeList;
                    cashReceipt.FrequencyList = cashReceiptGlobal.FrequencyList;
                    cashReceipt.ReceiverList = cashReceiptGlobal.ReceiverList;
                }
            }
            else
            {
                cashReceipt = _cashReceiptDA.GetCashReceipt(invoiceHeaderId, true);
            }

            SetSubstitutionInfo(cashReceipt, invoiceHeaderId);
            return cashReceipt;
        }

        /// <summary>
        /// Gets the global invoice cancellation details.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <returns></returns>
        public GlobalInvoiceCancellationDetails GetGlobalInvoiceCancellationDetails(int invoiceHeaderId)
        {
            return _fiscalRecordDA.GetGlobalInvoiceCancellationDetails(invoiceHeaderId);
        }

        /// <summary>
        /// Gets the available global invoice filters.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public GlobalInvoiceFilters GetGlobalInvoiceFilters(string startDate, string endDate)
        {
            return _fiscalRecordDA.GetGlobalInvoiceFilters(startDate, endDate);
        }

        /// <summary>
        /// Gets the payment complement.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <param name="receiptNumber">The receipt number.</param>
        /// <returns></returns>
        public PaymentComplement GetPaymentComplement(int invoiceHeaderId, int receiptNumber)
        {
            if (invoiceHeaderId <= 0)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CreateFiscalRecord", "CreateFiscalRecord ends - invoiceHeaderId <= 0");
                return null;
            }
            if (receiptNumber <= 0)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CreateFiscalRecord", "CreateFiscalRecord ends - invoiceHeaderId <= 0");
                return null;
            }

            return _fiscalRecordDA.GetPaymentComplement(invoiceHeaderId, receiptNumber);
        }

        /// <summary>
        /// Get the payment receipts related to a fiscal record
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public List<PaymentReceipt>[] GetPaymentReceiptsRelated(int id)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetPaymentReceiptsRelated", "GetPaymentReceiptsRelated starts");
            if (id <= 0)
            {
                LoggerHelper.LogWebError("FiscalRecords", "GetPaymentReceiptsRelated", "GetPaymentReceiptsRelated ends - id <= 0");
                return null;
            }
            List<PaymentReceipt>[] lstFiscalRecord = _fiscalRecordDA.GetPaymentReceiptsRelated(id);
            // LoggerHelper.LogWebError("FiscalRecords", "GetPaymentReceiptsRelated", "GetPaymentReceiptsRelated ends");
            return lstFiscalRecord;
        }

        /// <summary>
        /// Sets the substitution information.
        /// </summary>
        /// <param name="cashReceipt">The cash receipt.</param>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        private void SetSubstitutionInfo(CashReceipt cashReceipt, int invoiceHeaderId)
        {
            if (cashReceipt != null)
            {
                cashReceipt.CFDIRelatedId = invoiceHeaderId;
                cashReceipt.CancelReasonName = CancelReasonName.ErrorRelacion;
                FiscalRecordRelationType fiscalRecordRelationType = _catalogServices.GetFiscalRecordRelationType(RelationTypeName.Sustitucion.ToString());
                if (fiscalRecordRelationType != null)
                {
                    cashReceipt.RelationTypeCode = fiscalRecordRelationType.Code;
                    cashReceipt.RelationTypeDesc = fiscalRecordRelationType.Description;
                }
            }
        }

        /// <summary>
        /// Sets the substitution information to save.
        /// </summary>
        /// <param name="fiscalRecord">The fiscal record.</param>
        /// <returns></returns>
        private bool? SetSubstitutionInfoToSave(CreateFiscalRecord fiscalRecord)
        {
            bool isSubstitution = false;
            if (fiscalRecord.CFDIRelatedId.HasValue
                && fiscalRecord.CFDIRelatedId.Value > 0
                && fiscalRecord.CancelReasonName.HasValue
                && fiscalRecord.CancelReasonName == CancelReasonName.ErrorRelacion)
            {
                FiscalRecordCancelReason fiscalRecordCancelReason = _catalogServices.GetFiscalRecordCancelReason(CancelReasonName.ErrorRelacion.ToString());
                FiscalRecordRelationType fiscalRecordRelationType = _catalogServices.GetFiscalRecordRelationType(RelationTypeName.Sustitucion.ToString());
                if (fiscalRecordCancelReason != null && fiscalRecordRelationType != null)
                {
                    fiscalRecord.CancelReasonKey = fiscalRecordCancelReason.Code;
                    fiscalRecord.CFDIRelationTypeKey = fiscalRecordRelationType.Code;
                    isSubstitution = true;
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "CreateFiscalRecord", "CreateFiscalRecord ends - CancelReason == null or RelationType = null");
                    return null;
                }
            }
            return isSubstitution;
        }
    }
}