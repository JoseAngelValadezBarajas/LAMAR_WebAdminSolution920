// --------------------------------------------------------------------
// <copyright file="CreditNotesServices.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using PowerCampus.BusinessInterfaces;
using PowerCampus.DataAccess;
using PowerCampus.Entities;
using PowerCampus.Entities.Enum;
using System.Collections.Generic;

namespace PowerCampus.Business
{
    /// <summary>
    /// CreditNotesSerivices class
    /// </summary>
    public class CreditNotesServices : ICreditNotesServices
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
        /// The credit notes da
        /// </summary>
        private readonly CreditNotesDA _creditNotesDA;

        /// <summary>
        /// The issuer da
        /// </summary>
        private readonly IssuerDA _issuerDA;

        /// <summary>
        /// The receiver da
        /// </summary>
        private readonly ReceiverDA _receiverDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditNotesServices"/> class.
        /// </summary>
        public CreditNotesServices()
        {
            _creditNotesDA = new CreditNotesDA();
            _issuerDA = new IssuerDA();
            _receiverDA = new ReceiverDA();
            _catalogServices = new CatalogServices();
        }

        /// <summary>
        /// Creates the credit note.
        /// </summary>
        /// <param name="fiscalRecordModel">The fiscal record model.</param>
        /// <returns></returns>
        public int CreateCreditNote(CreateFiscalRecord fiscalRecordModel)
        {
            FiscalRecordRelationType fiscalRecordRelationType = _catalogServices.GetFiscalRecordRelationType(RelationTypeName.NotaCredito.ToString());
            if (fiscalRecordRelationType != null)
            {
                fiscalRecordModel.CFDIRelationTypeKey = fiscalRecordRelationType.Code;
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "CreateCreditNote", "CreateCreditNote ends - RelationType = null");
                return 0;
            }
            bool? isSubstitution = SetSubstitutionInfoToSave(fiscalRecordModel);
            if (!isSubstitution.HasValue)
                return -1;

            int invoiceHeaderId;
            if (isSubstitution.Value == true)
                invoiceHeaderId = _creditNotesDA.CreateCreditNoteForSubstitution(fiscalRecordModel);
            else
                invoiceHeaderId = _creditNotesDA.CreateCreditNote(fiscalRecordModel);

            return invoiceHeaderId;
        }

        /// <summary>
        /// Gets the fiscal record.
        /// </summary>
        /// <param name="InvoiceHeaderId">The invoice header identifier.</param>
        /// <param name="userName"></param>
        /// /// <param name="PeopleOrgCodeId"></param>
        /// <returns></returns>
        public FiscalRecord GetFiscalRecord(int InvoiceHeaderId, string userName, string PeopleOrgCodeId)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetFiscalRecord", "GetFiscalRecord starts");
            FiscalRecord fiscalRecord = _creditNotesDA.GetFiscalRecord(InvoiceHeaderId, userName, PeopleOrgCodeId);

            if (fiscalRecord.Version == "3.3")
            {
                string email = fiscalRecord.Receiver.Email;
                fiscalRecord.Receiver = _receiverDA.GetTaxPayers(fiscalRecord.Receiver.TaxPayerId)[0];
                fiscalRecord.Receiver.Email = email;
            }

            // LoggerHelper.LogWebError("FiscalRecords", "GetFiscalRecord", "GetFiscalRecord ends");
            return fiscalRecord;
        }

        /// <summary>
        /// Gets for substitution.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="peopleOrgcodeId">The people orgcode identifier.</param>
        /// <returns></returns>
        public FiscalRecord GetForSubstitution(int id, string userName, string peopleOrgcodeId)
        {
            FiscalRecord fiscalRecord = _creditNotesDA.GetFiscalRecord(id, userName, peopleOrgcodeId);

            string email = fiscalRecord.Receiver.Email;
            fiscalRecord.Receiver = _receiverDA.GetTaxPayers(fiscalRecord.Receiver.TaxPayerId)[0];
            fiscalRecord.Receiver.Email = email;

            List<Issuer> issuerList = _issuerDA.GetAllIssuers(fiscalRecord.Issuer.IssTaxpayerId);
            if (issuerList != null && issuerList.Count > 0)
            {
                fiscalRecord.Issuer.IssCorporateName = issuerList[0].IssCorporateName;

                if (issuerList[0].IssInvoiceOrganizationId.HasValue)
                {
                    Issuer currentIssuer = _issuerDA.GetIssuerTaxRegimen(issuerList[0].IssInvoiceOrganizationId.Value);
                    fiscalRecord.Issuer.IssIssuingAdd = currentIssuer.IssIssuingAdd;
                    fiscalRecord.Issuer.TaxRegimenCode = currentIssuer.IssTaxRegimen[0].IssCodeValue;
                    fiscalRecord.Issuer.TaxRegimenDesc = currentIssuer.IssTaxRegimen[0].IssLongDesc;
                }
            }

            fiscalRecord.CancelReasonName = CancelReasonName.ErrorRelacion;
            return fiscalRecord;
        }

        /// <summary>
        /// Sets the substitution information to save.
        /// </summary>
        /// <param name="fiscalRecord">The fiscal record.</param>
        /// <returns></returns>
        private bool? SetSubstitutionInfoToSave(CreateFiscalRecord fiscalRecord)
        {
            bool isSubstitution = false;
            if (fiscalRecord.CFDIRelated != null
                && fiscalRecord.CFDIRelated.Length > 0
                && fiscalRecord.CFDIRelated2 != null
                && fiscalRecord.CFDIRelated2.Length > 0
                && fiscalRecord.CancelReasonName.HasValue
                && fiscalRecord.CancelReasonName == CancelReasonName.ErrorRelacion)
            {
                FiscalRecordCancelReason fiscalRecordCancelReason = _catalogServices.GetFiscalRecordCancelReason(CancelReasonName.ErrorRelacion.ToString());
                FiscalRecordRelationType fiscalRecordRelationType = _catalogServices.GetFiscalRecordRelationType(RelationTypeName.NotaCredito.ToString());
                FiscalRecordRelationType fiscalRecordRelationType2 = _catalogServices.GetFiscalRecordRelationType(RelationTypeName.Sustitucion.ToString());
                if (fiscalRecordCancelReason != null && fiscalRecordRelationType != null && fiscalRecordRelationType2 != null)
                {
                    fiscalRecord.CancelReasonKey = fiscalRecordCancelReason.Code;
                    fiscalRecord.CFDIRelationTypeKey = fiscalRecordRelationType.Code;
                    fiscalRecord.CFDIRelationTypeKey2 = fiscalRecordRelationType2.Code;
                    isSubstitution = true;
                }
                else
                {
                    LoggerHelper.LogWebError("CreditNotes", "CreateCreditNotes", "CreateCreditNotes ends - CancelReason == null or RelationType = null");
                    return null;
                }
            }
            return isSubstitution;
        }
    }
}