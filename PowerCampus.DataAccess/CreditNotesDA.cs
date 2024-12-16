// --------------------------------------------------------------------
// <copyright file="CreditNotesDA.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PowerCampus.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace PowerCampus.DataAccess
{
    /// <summary>
    /// CreditNotesDA class
    /// </summary>
    public class CreditNotesDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditNotesDA"/> class.
        /// </summary>
        public CreditNotesDA()
        {
            _factory = new DatabaseProviderFactory();
        }

        /// <summary>
        /// Creates the fiscal record.
        /// </summary>
        /// <param name="fiscalRecordModel">The fiscal record model.</param>
        /// <returns></returns>
        public int CreateCreditNote(CreateFiscalRecord fiscalRecordModel)
        {
            int newInvoiceHeaderId = 0;
            try
            {
                if (fiscalRecordModel.Detail.Count > 0)
                {
                    string[] PaymentType = fiscalRecordModel.PaymentTypeDesc.Split('-');
                    string PaymentTypeCode = PaymentType[0];
                    string PaymentTypeDesc = PaymentType[1];

                    Database database = _factory.CreateDefault();
                    using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                    {
                        connection.Open();
                        DbTransaction trans = connection.BeginTransaction();
                        try
                        {
                            #region Insert InvoiceHeader

                            DbCommand insertInvoiceHeaderCommand = database.GetStoredProcCommand("spInsInvoiceHeader");

                            database.AddOutParameter(insertInvoiceHeaderCommand, "@InvoiceHeaderId", DbType.Int32, 0);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@PeopleOrgCodeId", DbType.String, fiscalRecordModel.PeopleOrgCodeId);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@TaxpayerId", DbType.String, fiscalRecordModel.IssuerTaxPayerId);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@InvoiceExpeditionId", DbType.Int32, fiscalRecordModel.InvoiceExpeditionId);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentType", DbType.String, PaymentTypeCode);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentCondition", DbType.String, fiscalRecordModel.PaymentCondition);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@ReceiverTaxpayerId", DbType.String, fiscalRecordModel.ReceiverTaxpayerId);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@ReceiverEmail", DbType.String, fiscalRecordModel.ReceiverEmail);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@Subtotal", DbType.Decimal, decimal.Parse(fiscalRecordModel.Subtotal, CultureInfo.InvariantCulture));
                            database.AddInParameter(insertInvoiceHeaderCommand, "@Total", DbType.Decimal, decimal.Parse(fiscalRecordModel.Total, CultureInfo.InvariantCulture));
                            database.AddInParameter(insertInvoiceHeaderCommand, "@TotalTransferTaxes", DbType.Decimal, decimal.Parse(fiscalRecordModel.TotalTransferTaxes, CultureInfo.InvariantCulture));
                            database.AddInParameter(insertInvoiceHeaderCommand, "@ExchangeRate", DbType.Int32, 1);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@Currency", DbType.String, fiscalRecordModel.Currency);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentMethod", DbType.String, fiscalRecordModel.PaymentMethod);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@CityOfIssue", DbType.String, fiscalRecordModel.CityOfIssue);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentAccountNumber", DbType.String, fiscalRecordModel.PaymentAccountNumber);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@TaxRegimen", DbType.String, fiscalRecordModel.TaxRegimen);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@Comments", DbType.String, fiscalRecordModel.Comments);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIUsageCode", DbType.String, fiscalRecordModel.CFDIUsageCode);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIUsageDesc", DbType.String, fiscalRecordModel.CFDIUsageDesc);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@TaxRegimenDesc", DbType.String, fiscalRecordModel.TaxRegimenDesc);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentTypeDesc", DbType.String, PaymentTypeDesc);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentMethodDesc", DbType.String, fiscalRecordModel.PaymentMethodDesc);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalIdentityNumber", DbType.String, fiscalRecordModel.FiscalIdentityNumber);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalResidency", DbType.String, fiscalRecordModel.FiscalResidency);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalResidencyDesc", DbType.String, fiscalRecordModel.FiscalResidencyDesc);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@SerialNumber", DbType.String, fiscalRecordModel.SerialNumber);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalRecordType", DbType.String, Constants.FiscalRecordTypeEgreso);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIRelated", DbType.String, fiscalRecordModel.CFDIRelated);
                            database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIRelationType", DbType.String, fiscalRecordModel.CFDIRelationTypeKey);

                            database.ExecuteNonQuery(insertInvoiceHeaderCommand, trans);
                            newInvoiceHeaderId = (int)database.GetParameterValue(insertInvoiceHeaderCommand, "@InvoiceHeaderId");

                            #endregion Insert InvoiceHeader

                            if (newInvoiceHeaderId > 0)
                            {
                                #region Details Credit Note

                                foreach (FiscalRecordDetail detail in fiscalRecordModel.Detail)
                                {
                                    DbCommand insertCreditNoteDetailCommand = database.GetStoredProcCommand("spInsCreditNoteDetail");

                                    database.AddInParameter(insertCreditNoteDetailCommand, "@InvoiceHeaderId", DbType.Int32, newInvoiceHeaderId);
                                    database.AddInParameter(insertCreditNoteDetailCommand, "@InvoiceDetailId", DbType.Int32, detail.InvoiceDetailId);
                                    database.AddInParameter(insertCreditNoteDetailCommand, "@IsATax", DbType.Int32, detail.IsATax);
                                    database.AddInParameter(insertCreditNoteDetailCommand, "@Amount", DbType.Decimal, detail.Amount);
                                    database.AddInParameter(insertCreditNoteDetailCommand, "@Description", DbType.String, detail.Description);
                                    database.AddInParameter(insertCreditNoteDetailCommand, "@UnitName", DbType.String, detail.UnitDescription);
                                    database.AddOutParameter(insertCreditNoteDetailCommand, "@CreditNoteDetailId", DbType.Int32, 0);

                                    database.ExecuteNonQuery(insertCreditNoteDetailCommand, trans);
                                }

                                #endregion Details Credit Note

                                #region Insert Invoice Request

                                DbCommand insertInvoiceRequestCommand = database.GetStoredProcCommand("spInsInvoiceRequest");

                                database.AddInParameter(insertInvoiceRequestCommand, "@InvoiceHeaderId", DbType.Int32, newInvoiceHeaderId);
                                database.AddInParameter(insertInvoiceRequestCommand, "@Status", DbType.Int32, 1);
                                database.AddOutParameter(insertInvoiceRequestCommand, "@InvoiceRequestId", DbType.Int32, 0);

                                database.ExecuteNonQuery(insertInvoiceRequestCommand, trans);

                                #endregion Insert Invoice Request

                                #region Insert Temp ChargeCredit

                                DbCommand insertCreditNoteChargeCreditCommand = database.GetStoredProcCommand("spInvoiceChargeCreditTemp");

                                database.AddInParameter(insertCreditNoteChargeCreditCommand, "@pInvoiceHeaderId", DbType.Int32, newInvoiceHeaderId);
                                database.AddInParameter(insertCreditNoteChargeCreditCommand, "@pChargeCreditId", DbType.Int32, fiscalRecordModel.ChargeCreditId);
                                database.AddInParameter(insertCreditNoteChargeCreditCommand, "@pSubtotal", DbType.Decimal, decimal.Parse(fiscalRecordModel.Subtotal, CultureInfo.InvariantCulture));

                                database.ExecuteNonQuery(insertCreditNoteChargeCreditCommand, trans);

                                #endregion Insert Temp ChargeCredit

                                //#region Insert Charge Credit for total amount without taxes

                                //DbCommand insertCreditNoteChargeCreditCommand = database.GetStoredProcCommand("spInsCreditNoteChargeCredit");

                                //database.AddInParameter(insertCreditNoteChargeCreditCommand, "@InvoiceHeaderId", DbType.Int32, newInvoiceHeaderId);
                                //database.AddInParameter(insertCreditNoteChargeCreditCommand, "@ChargeCreditCodeId", DbType.Int32, fiscalRecordModel.ChargeCreditId);
                                //database.AddInParameter(insertCreditNoteChargeCreditCommand, "@Amount", DbType.Decimal, decimal.Parse(fiscalRecordModel.Subtotal, CultureInfo.InvariantCulture));

                                //database.ExecuteNonQuery(insertCreditNoteChargeCreditCommand, trans);

                                //#endregion Insert Charge Credit for total amount without taxes

                                //#region Insert Charge Credit for total amount of taxes

                                //if (fiscalRecordModel.ChargeCreditIdTax > 0)
                                //{
                                //    DbCommand insertCreditNoteChargeCreditCommand2 = database.GetStoredProcCommand("spInsCreditNoteChargeCredit");

                                //    database.AddInParameter(insertCreditNoteChargeCreditCommand2, "@InvoiceHeaderId", DbType.Int32, newInvoiceHeaderId);
                                //    database.AddInParameter(insertCreditNoteChargeCreditCommand2, "@ChargeCreditCodeId", DbType.Int32, fiscalRecordModel.ChargeCreditIdTax);
                                //    database.AddInParameter(insertCreditNoteChargeCreditCommand2, "@Amount", DbType.Decimal, decimal.Parse(fiscalRecordModel.TotalTransferTaxes, CultureInfo.InvariantCulture));

                                //    database.ExecuteNonQuery(insertCreditNoteChargeCreditCommand2, trans);
                                //}

                                //#endregion Insert Charge Credit for total amount of taxes

                                trans.Commit();
                            }
                            else
                            {
                                trans.Rollback();
                            }
                        }
                        catch (Exception e)
                        {
                            LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - CreateCreditNote", e.Message + e.StackTrace);
                            trans.Rollback();
                            throw;
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
                return newInvoiceHeaderId;
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - CreditNoteDA - CreateCreditNote", e.Message + e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Creates the credit note.
        /// </summary>
        /// <param name="fiscalRecordModel">The fiscal record model.</param>
        /// <returns></returns>
        public int CreateCreditNoteForSubstitution(CreateFiscalRecord fiscalRecordModel)
        {
            int newInvoiceHeaderId = 0;
            try
            {
                if (fiscalRecordModel.Detail.Count > 0)
                {
                    string[] PaymentType = fiscalRecordModel.PaymentTypeDesc.Split('-');
                    string PaymentTypeCode = PaymentType[0];
                    string PaymentTypeDesc = PaymentType[1];
                    decimal total = 0, subTotal = 0, totalTaxes = 0;

                    Database database = _factory.CreateDefault();
                    using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                    {
                        connection.Open();
                        DbTransaction trans = connection.BeginTransaction();
                        try
                        {
                            // Get details and set amounts
                            DataSet originalCreditNoteDataset = new DataSet();
                            DbCommand selCashReceiptInvoiceDetailCommand = database.GetStoredProcCommand("spSelInvoiceDetailByHeaderIdForSubstitution");
                            database.AddInParameter(selCashReceiptInvoiceDetailCommand, "@InvoiceHeaderId", DbType.Int32, fiscalRecordModel.InvoiceHeaderId);
                            database.LoadDataSet(selCashReceiptInvoiceDetailCommand, originalCreditNoteDataset, "CashReceiptInvoiceDetail", trans);

                            DataTable detailsTable = originalCreditNoteDataset.Tables[2];
                            DataTable detailsTaxTable = originalCreditNoteDataset.Tables[3];
                            DataTable totalsTable = originalCreditNoteDataset.Tables[0];

                            if (totalsTable.Rows.Count > 0)
                            {
                                foreach (DataRow row in totalsTable.Rows)
                                {
                                    subTotal = decimal.Parse(row["SubTotal"].ToString());
                                    totalTaxes = decimal.Parse(row["TotalTaxes"].ToString());
                                    total = decimal.Parse(row["Total"].ToString());
                                }
                            }

                            if (detailsTable.Rows.Count > 0)
                            {
                                #region Insert InvoiceHeader

                                DbCommand insertInvoiceHeaderCommand = database.GetStoredProcCommand("spInsInvoiceHeader");

                                database.AddOutParameter(insertInvoiceHeaderCommand, "@InvoiceHeaderId", DbType.Int32, 0);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PeopleOrgCodeId", DbType.String, fiscalRecordModel.PeopleOrgCodeId);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@TaxpayerId", DbType.String, fiscalRecordModel.IssuerTaxPayerId);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@InvoiceExpeditionId", DbType.Int32, fiscalRecordModel.InvoiceExpeditionId);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentType", DbType.String, PaymentTypeCode);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentCondition", DbType.String, fiscalRecordModel.PaymentCondition);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@ReceiverTaxpayerId", DbType.String, fiscalRecordModel.ReceiverTaxpayerId);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@ReceiverEmail", DbType.String, fiscalRecordModel.ReceiverEmail);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@Subtotal", DbType.Decimal, subTotal);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@Total", DbType.Decimal, total);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@TotalTransferTaxes", DbType.Decimal, totalTaxes);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@ExchangeRate", DbType.Int32, 1);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@Currency", DbType.String, fiscalRecordModel.Currency);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentMethod", DbType.String, fiscalRecordModel.PaymentMethod);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@CityOfIssue", DbType.String, fiscalRecordModel.CityOfIssue);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentAccountNumber", DbType.String, fiscalRecordModel.PaymentAccountNumber);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@TaxRegimen", DbType.String, fiscalRecordModel.TaxRegimen);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@Comments", DbType.String, fiscalRecordModel.Comments);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIUsageCode", DbType.String, fiscalRecordModel.CFDIUsageCode);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIUsageDesc", DbType.String, fiscalRecordModel.CFDIUsageDesc);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@TaxRegimenDesc", DbType.String, fiscalRecordModel.TaxRegimenDesc);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentTypeDesc", DbType.String, PaymentTypeDesc);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentMethodDesc", DbType.String, fiscalRecordModel.PaymentMethodDesc);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalIdentityNumber", DbType.String, fiscalRecordModel.FiscalIdentityNumber);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalResidency", DbType.String, fiscalRecordModel.FiscalResidency);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalResidencyDesc", DbType.String, fiscalRecordModel.FiscalResidencyDesc);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@SerialNumber", DbType.String, fiscalRecordModel.SerialNumber);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalRecordType", DbType.String, Constants.FiscalRecordTypeEgreso);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIRelated", DbType.String, fiscalRecordModel.CFDIRelated);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIRelationType", DbType.String, fiscalRecordModel.CFDIRelationTypeKey);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIRelated2", DbType.String, fiscalRecordModel.CFDIRelated2);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIRelationType2", DbType.String, fiscalRecordModel.CFDIRelationTypeKey2);

                                database.ExecuteNonQuery(insertInvoiceHeaderCommand, trans);
                                newInvoiceHeaderId = (int)database.GetParameterValue(insertInvoiceHeaderCommand, "@InvoiceHeaderId");

                                #endregion Insert InvoiceHeader

                                if (newInvoiceHeaderId > 0)
                                {
                                    DbCommand updateInvoiceHeaderForCancellation = database.GetStoredProcCommand("dbo.spUpdInvoiceHeaderForCancellation");

                                    database.AddInParameter(updateInvoiceHeaderForCancellation, "@InvoiceHeaderId", DbType.Int32, fiscalRecordModel.InvoiceHeaderId);
                                    database.AddInParameter(updateInvoiceHeaderForCancellation, "@CancelReason", DbType.String, fiscalRecordModel.CancelReasonKey);
                                    database.AddParameter(updateInvoiceHeaderForCancellation, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, new object());

                                    database.ExecuteNonQuery(updateInvoiceHeaderForCancellation, trans);

                                    if ((int)database.GetParameterValue(updateInvoiceHeaderForCancellation, "@ReturnStatus") <= 0)
                                    {
                                        LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - CreateFiscalRecord", "Error on dbo.spUpdInvoiceHeaderForCancellation, ReturnStatus <= 0");
                                        trans.Rollback();
                                        return 0;
                                    }

                                    List<FiscalRecordDetail> currentCreditNoteDetails = new List<FiscalRecordDetail>();
                                    List<FiscalRecordDetail> currentCreditNoteDetailTaxes = new List<FiscalRecordDetail>();
                                    bool hasError = false;

                                    #region Get Current Fiscal Record Details

                                    foreach (DataRow row in detailsTable.Rows)
                                    {
                                        currentCreditNoteDetails.Add(new FiscalRecordDetail
                                        {
                                            InvoiceHeaderId = newInvoiceHeaderId,
                                            ReceiptNumber = int.Parse(row["ReceiptNumber"].ToString()),
                                            ChargeCreditNumber = int.Parse(row["ChargeCreditNumber"].ToString()),
                                            ChargeCreditCodeId = int.Parse(row["ChargeCreditCodeId"].ToString()),
                                            ChargeCreditSource = int.Parse(row["ReceiptChrgCrdNumber"].ToString()),
                                            ChargeCreditCode = row["ChargeCreditCode"].ToString(),
                                            Description = row["ChargeCreditDesc"].ToString(),
                                            Amount = decimal.Parse(row["UnitAmount"].ToString()),
                                            IsATax = int.Parse(row["IsATax"].ToString()),
                                            TransferTaxType = row["ChargeCreditCode"].ToString(),
                                            TransferRate = string.IsNullOrEmpty(row["TaxRate"].ToString()) ? 0 : decimal.Parse(row["TaxRate"].ToString()),
                                            ProductServiceKey = string.IsNullOrEmpty(row["ProductServiceKey"].ToString()) ? string.Empty : row["ProductServiceKey"].ToString(),
                                            ProductServiceDesc = string.IsNullOrEmpty(row["ProductServiceDesc"].ToString()) ? string.Empty : row["ProductServiceDesc"].ToString(),
                                            UnityKey = string.IsNullOrEmpty(row["UnityKey"].ToString()) ? string.Empty : row["UnityKey"].ToString(),
                                            UnitDescription = string.IsNullOrEmpty(row["UnityName"].ToString()) ? string.Empty : row["UnityName"].ToString(),
                                            TaxFactorType = string.IsNullOrEmpty(row["FactorType"].ToString()) ? string.Empty : row["FactorType"].ToString(),
                                            TaxCode = string.IsNullOrEmpty(row["TaxCode"].ToString()) ? string.Empty : row["TaxCode"].ToString(),
                                            SubjectToTax = string.IsNullOrEmpty(row["SubjectToTax"].ToString()) ? string.Empty : row["SubjectToTax"].ToString(),
                                        });
                                    }
                                    foreach (DataRow taxRow in detailsTaxTable.Rows)
                                    {
                                        currentCreditNoteDetailTaxes.Add(new FiscalRecordDetail
                                        {
                                            ChargeCreditSource = (int)taxRow["ChargeCreditSource"],
                                            TaxCode = (string)taxRow["TaxKey"],
                                            Description = (string)taxRow["Description"],
                                            TaxFactorType = (string)taxRow["FactorTypeKey"],
                                            TransferRate = taxRow["TaxRate"] is DBNull ? null : (decimal?)taxRow["TaxRate"]
                                        });
                                    }

                                    #endregion Get Current Fiscal Record Details

                                    #region Insert InvoiceDetail

                                    foreach (FiscalRecordDetail fiscalRecordDetail in currentCreditNoteDetails)
                                    {
                                        int invoiceDetailId;
                                        if (fiscalRecordDetail.IsATax.Equals(1))
                                        {
                                            fiscalRecordDetail.TransferTaxType = fiscalRecordDetail.TaxCode;
                                            fiscalRecordDetail.TransferAmount = fiscalRecordDetail.Amount;
                                            fiscalRecordDetail.Amount = 0;
                                        }
                                        else
                                        {
                                            fiscalRecordDetail.TransferTaxType = string.Empty;
                                            fiscalRecordDetail.TransferAmount = 0;
                                        }

                                        /*Description should change depending on user entry*/
                                        string newDescription = fiscalRecordDetail.Description;
                                        string newUnitName = string.Empty;
                                        if (fiscalRecordDetail.IsATax == 0)
                                        {
                                            newDescription = fiscalRecordModel.Detail.FirstOrDefault
                                                (m => m.ChargeCreditCodeId == fiscalRecordDetail.ChargeCreditNumber).Description;
                                            newUnitName = fiscalRecordModel.Detail.First
                                                (m => m.ChargeCreditCodeId == fiscalRecordDetail.ChargeCreditNumber).UnitDescription;
                                        }

                                        DbCommand insertInsInvoiceDetailCommand = database.GetStoredProcCommand("dbo.spInsInvoiceDetail");

                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@InvoiceHeaderId", DbType.Int32, newInvoiceHeaderId);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@Quantity", DbType.Int32, 1);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@ReceiptNumber", DbType.Int32, fiscalRecordDetail.ReceiptNumber);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@ChargeCreditNumber", DbType.Int32, fiscalRecordDetail.ChargeCreditNumber);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@ChargeCreditCode", DbType.String, fiscalRecordDetail.ChargeCreditCode);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@Description", DbType.String, newDescription);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@UnitValue", DbType.Decimal, fiscalRecordDetail.Amount);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@Amount", DbType.Decimal, fiscalRecordDetail.Amount);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@DeductedTaxType", DbType.String, string.Empty);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@DeductedAmount", DbType.Int32, 0);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@TransferTaxType", DbType.String, fiscalRecordDetail.TransferTaxType);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@TransferRate", DbType.Decimal, fiscalRecordDetail.TransferRate ?? 0);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@TransferAmount", DbType.Decimal, fiscalRecordDetail.TransferAmount);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@ChargeCreditSource", DbType.Int32, fiscalRecordDetail.ChargeCreditSource);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@Unit", DbType.String, fiscalRecordDetail.UnityKey);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@UnitName", DbType.String, newUnitName);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@ProductServiceKey", DbType.String, fiscalRecordDetail.ProductServiceKey);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@ProductServiceDesc", DbType.String, fiscalRecordDetail.ProductServiceDesc);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@TaxFactorType", DbType.String, fiscalRecordDetail.TaxFactorType);
                                        database.AddInParameter(insertInsInvoiceDetailCommand, "@SubjectToTax", DbType.String, fiscalRecordDetail.SubjectToTax);
                                        database.AddOutParameter(insertInsInvoiceDetailCommand, "@InvoiceDetailId", DbType.Int32, 0);

                                        database.ExecuteNonQuery(insertInsInvoiceDetailCommand, trans);
                                        invoiceDetailId = (int)database.GetParameterValue(insertInsInvoiceDetailCommand, "@InvoiceDetailId");
                                        if (invoiceDetailId <= 0)
                                        {
                                            LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - CreateFiscalRecord", "Error on dbo.spInsInvoiceDetail, InvoiceDetailId <= 0");
                                            hasError = true;
                                            break;
                                        }
                                        FiscalRecordDetail fiscalRecordDetailTax
                                            = currentCreditNoteDetailTaxes.Find(t => t.ChargeCreditSource.Value == fiscalRecordDetail.ChargeCreditNumber
                                                && t.InvoiceDetailId == 0);
                                        if (fiscalRecordDetailTax != null)
                                        {
                                            fiscalRecordDetailTax.InvoiceDetailId = invoiceDetailId;

                                            #region Insert InvoiceDetailTax

                                            DbCommand insertInsInvoiceDetailTaxCommand = database.GetStoredProcCommand("WebAdmin.spInsInvoiceDetailTax");

                                            database.AddInParameter(insertInsInvoiceDetailTaxCommand, "@InvoiceDetailId", DbType.Int32, fiscalRecordDetailTax.InvoiceDetailId);
                                            database.AddInParameter(insertInsInvoiceDetailTaxCommand, "@ChargeNumberSource", DbType.Int32, fiscalRecordDetailTax.ChargeCreditSource);
                                            database.AddInParameter(insertInsInvoiceDetailTaxCommand, "@TaxKey", DbType.String, fiscalRecordDetailTax.TaxCode);
                                            database.AddInParameter(insertInsInvoiceDetailTaxCommand, "@Description", DbType.String, fiscalRecordDetailTax.Description);
                                            database.AddInParameter(insertInsInvoiceDetailTaxCommand, "@FactorTypeKey", DbType.String, fiscalRecordDetailTax.TaxFactorType);
                                            database.AddInParameter(insertInsInvoiceDetailTaxCommand, "@TaxRate", DbType.Decimal, fiscalRecordDetailTax.TransferRate);
                                            database.AddOutParameter(insertInsInvoiceDetailTaxCommand, "@InvoiceDetailTaxId", DbType.Int32, 0);

                                            database.ExecuteNonQuery(insertInsInvoiceDetailTaxCommand, trans);

                                            if ((int)database.GetParameterValue(insertInsInvoiceDetailTaxCommand, "@InvoiceDetailTaxId") <= 0)
                                            {
                                                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - CreateFiscalRecord", "Error on WebAdmin.spInsInvoiceDetailTax, InvoiceDetailTaxId <= 0");
                                                hasError = true;
                                                break;
                                            }

                                            #endregion Insert InvoiceDetailTax
                                        }
                                    }

                                    if (hasError)
                                    {
                                        trans.Rollback();
                                        return 0;
                                    }

                                    #endregion Insert InvoiceDetail
                                }
                            }

                            if (newInvoiceHeaderId > 0)
                            {
                                #region Insert Invoice Request

                                DbCommand insertInvoiceRequestCommand = database.GetStoredProcCommand("spInsInvoiceRequest");

                                database.AddInParameter(insertInvoiceRequestCommand, "@InvoiceHeaderId", DbType.Int32, newInvoiceHeaderId);
                                database.AddInParameter(insertInvoiceRequestCommand, "@Status", DbType.Int32, 1);
                                database.AddOutParameter(insertInvoiceRequestCommand, "@InvoiceRequestId", DbType.Int32, 0);

                                database.ExecuteNonQuery(insertInvoiceRequestCommand, trans);

                                #endregion Insert Invoice Request

                                #region Insert Temp ChargeCredit

                                DbCommand insertCreditNoteChargeCreditCommand = database.GetStoredProcCommand("spInvoiceChargeCreditTemp");

                                database.AddInParameter(insertCreditNoteChargeCreditCommand, "@pInvoiceHeaderId", DbType.Int32, newInvoiceHeaderId);
                                database.AddInParameter(insertCreditNoteChargeCreditCommand, "@pChargeCreditId", DbType.Int32, fiscalRecordModel.ChargeCreditId);
                                database.AddInParameter(insertCreditNoteChargeCreditCommand, "@pSubtotal", DbType.Decimal, decimal.Parse(fiscalRecordModel.Subtotal, CultureInfo.InvariantCulture));

                                database.ExecuteNonQuery(insertCreditNoteChargeCreditCommand, trans);

                                #endregion Insert Temp ChargeCredit

                                //#region Insert Charge Credit for total amount without taxes

                                //DbCommand insertCreditNoteChargeCreditCommand = database.GetStoredProcCommand("spInsCreditNoteChargeCredit");

                                //database.AddInParameter(insertCreditNoteChargeCreditCommand, "@InvoiceHeaderId", DbType.Int32, newInvoiceHeaderId);
                                //database.AddInParameter(insertCreditNoteChargeCreditCommand, "@ChargeCreditCodeId", DbType.Int32, fiscalRecordModel.ChargeCreditId);
                                //database.AddInParameter(insertCreditNoteChargeCreditCommand, "@Amount", DbType.Decimal, decimal.Parse(fiscalRecordModel.Subtotal, CultureInfo.InvariantCulture));
                                //database.AddInParameter(insertCreditNoteChargeCreditCommand, "@IsSubstitution", DbType.Boolean, true);
                                //database.AddInParameter(insertCreditNoteChargeCreditCommand, "@CancelledInvoiceHeaderId", DbType.Int32, fiscalRecordModel.InvoiceHeaderId);

                                //database.ExecuteNonQuery(insertCreditNoteChargeCreditCommand, trans);

                                //#endregion Insert Charge Credit for total amount without taxes

                                //#region Insert Charge Credit for total amount of taxes

                                //if (fiscalRecordModel.ChargeCreditIdTax > 0)
                                //{
                                //    DbCommand insertCreditNoteChargeCreditCommand2 = database.GetStoredProcCommand("spInsCreditNoteChargeCredit");

                                //    database.AddInParameter(insertCreditNoteChargeCreditCommand2, "@InvoiceHeaderId", DbType.Int32, newInvoiceHeaderId);
                                //    database.AddInParameter(insertCreditNoteChargeCreditCommand2, "@ChargeCreditCodeId", DbType.Int32, fiscalRecordModel.ChargeCreditIdTax);
                                //    database.AddInParameter(insertCreditNoteChargeCreditCommand2, "@Amount", DbType.Decimal, decimal.Parse(fiscalRecordModel.TotalTransferTaxes, CultureInfo.InvariantCulture));
                                //    database.AddInParameter(insertCreditNoteChargeCreditCommand2, "@IsSubstitution", DbType.Boolean, true);
                                //    database.AddInParameter(insertCreditNoteChargeCreditCommand2, "@CancelledInvoiceHeaderId", DbType.Int32, fiscalRecordModel.InvoiceHeaderId);

                                //    database.ExecuteNonQuery(insertCreditNoteChargeCreditCommand2, trans);
                                //}

                                //#endregion Insert Charge Credit for total amount of taxes

                                trans.Commit();
                            }
                            else
                            {
                                trans.Rollback();
                            }
                        }
                        catch (Exception e)
                        {
                            LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - CreateCreditNote", e.Message + e.StackTrace);
                            trans.Rollback();
                            throw;
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
                return newInvoiceHeaderId;
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - CreditNoteDA - CreateCreditNote", e.Message + e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the fiscal record.
        /// </summary>
        /// <param name="InvoiceHeaderId">The invoice header identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="PeopleOrgCodeId">The people org code identifier.</param>
        /// <returns></returns>
        public FiscalRecord GetFiscalRecord(int InvoiceHeaderId, string userName, string PeopleOrgCodeId)
        {
            DataSet fiscalRecordDataSet = new DataSet();
            DataSet fiscalRecordDetailsDataSet = new DataSet();
            FiscalRecord fiscalRecord = new FiscalRecord();
            CatalogDA catalogDA = new CatalogDA();
            try
            {
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelFiscalRecordById");

                    database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, InvoiceHeaderId);
                    database.LoadDataSet(command, fiscalRecordDataSet, "CashReceiptInvoiceDetail");
                }
                if (fiscalRecordDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in fiscalRecordDataSet.Tables[0].Rows)
                    {
                        #region FiscalRecord Info

                        fiscalRecord = new FiscalRecord
                        {
                            InvoiceHeaderId = int.Parse(row["InvoiceHeaderId"].ToString()),
                            PeopleOrgCodeId = row["People_Org_Code_Id"].ToString(),
                            Version = row["Version"].ToString(),
                            InvoiceExpeditionId = int.Parse(row["InvoiceExpeditionId"].ToString()),
                            Receiver = new Receiver
                            {
                                TaxPayerId = row["ReceiverTaxpayerId"].ToString(),
                                CorporateName = row["ReceiverCorporateName"].ToString(),
                                Email = row["ReceiverEmail"].ToString(),
                                PostalCode = row["ReceiverPostalCode"].ToString(),
                                TaxRegimenCode = row["ReceiverTaxRegimen"].ToString(),
                                TaxRegimenDesc = row["ReceiverTaxRegimenDesc"].ToString(),
                                FiscalResidency = row["FiscalResidency"].ToString(),
                                FiscalResidencyDesc = row["FiscalResidencyDesc"].ToString(),
                                FiscalIdentityNumber = row["FiscalIdentityNumber"].ToString(),
                            },
                            Issuer = new Issuer
                            {
                                IssInvoiceOrganizationId = Convert.ToInt32(row["IssuerInvoiceOrganizationId"].ToString()),
                                IssTaxpayerId = row["IssuerTaxpayerId"].ToString(),
                                IssCorporateName = row["IssuerCorporateName"].ToString(),
                                TaxRegimenCode = row["IssuerTaxRegimen"].ToString(),
                                TaxRegimenDesc = row["IssuerTaxRegimenDesc"].ToString(),
                                IssIssuingAdd = new List<IssuingAddress>() {
                                        new  IssuingAddress() { IssIssuingAddress =row["IssuingAddressDesc"].ToString(),
                                        IssPlaceIssue = row["PlaceOfIssue"].ToString(),
                                        IssPostalCode = row["ExpeditionPostalCode"].ToString()}
                                    },
                            },
                            CFDIRelated = row["CFDIRelated"] is DBNull ? null : (string)row["CFDIRelated"],
                            RelationTypeCode = row["CFDIRelationType"] is DBNull ? null : (string)row["CFDIRelationType"],
                            RelationTypeDesc = row["CFDIRelationTypeDesc"] is DBNull ? null : (string)row["CFDIRelationTypeDesc"],
                            CFDIUsageCode = row["CFDIUsageCode"].ToString(),
                            CFDIUsageDesc = row["CFDIUsageDesc"].ToString(),
                            Folio = row["Folio"].ToString(),
                            InvoiceStatus = int.Parse(row["InvoiceStatus"].ToString()),
                            UUID = row["UUID"].ToString(),
                            FiscalRecordType = row["FiscalRecordType"].ToString(),
                            PaymentMethod = row["PaymentMethod"].ToString(),
                            PaymentMethodDesc = row["PaymentMethodDesc"].ToString(),
                            PaymentType = row["PaymentType"].ToString(),
                            PaymentTypeDesc = row["PaymentTypeDesc"].ToString(),
                            ExpeditionDateTime = DateTime.Parse(row["ExpeditionDateTime"].ToString()),
                            ApprovedDatetime = string.IsNullOrEmpty(row["ApprovedDatetime"].ToString()) ? (DateTime?)null
                                        : DateTime.Parse(row["ApprovedDatetime"].ToString()),
                            ErrorText = row["ErrorText"].ToString(),
                            Currency = row["Currency"].ToString(),
                            Comments = row["Comments"].ToString(),
                            SubTotal = string.IsNullOrEmpty(row["SubTotal"].ToString()) ? 0
                                        : decimal.Parse(row["SubTotal"].ToString()),
                            TotalTransferTaxes = string.IsNullOrEmpty(row["TotalTransferTaxes"].ToString()) ? 0
                                        : decimal.Parse(row["TotalTransferTaxes"].ToString()),
                            Total = string.IsNullOrEmpty(row["Total"].ToString()) ? 0
                                        : decimal.Parse(row["Total"].ToString()),
                            RequestStateId = string.IsNullOrEmpty(row["RequestState"].ToString()) ? -1 :
                                int.Parse(row["RequestState"].ToString()),
                            RequestState = row["RequestState"].ToString() == "" ?
                                        enumFiscalRecordStatus.Null :
                                        (enumFiscalRecordStatus)Enum.Parse(typeof(enumFiscalRecordStatus), row["RequestState"].ToString()),
                            StartDate = string.IsNullOrEmpty(row["StartDate"].ToString()) ? (DateTime?)null
                                        : DateTime.Parse(row["StartDate"].ToString()),
                            EndDate = string.IsNullOrEmpty(row["EndDate"].ToString()) ? (DateTime?)null
                                        : DateTime.Parse(row["EndDate"].ToString())
                        };

                        #endregion FiscalRecord Info

                        #region Catalogo CFDI

                        fiscalRecord.CFDIUsageCatalogList = new List<CFDIUsageCatalog>();

                        List<CFDIUsageCatalog> PreferredCFDIUsage = fiscalRecordDataSet.Tables[0].AsEnumerable().Select(m => new CFDIUsageCatalog()
                        {
                            Code = m.Field<string>("CFDIUsageCode"),
                            Description = m.Field<string>("CFDIUsageCode") + "-" + m.Field<string>("CFDIUsageDesc")
                        }).ToList();

                        if (!string.IsNullOrEmpty(PreferredCFDIUsage[0].Code))
                        {
                            foreach (CFDIUsageCatalog preferredCfdi in PreferredCFDIUsage)
                                fiscalRecord.CFDIUsageCatalogList.Add(preferredCfdi);
                        }

                        List<CFDIUsageCatalog> CatalogListCFDI = catalogDA.GetCFDIUsage();

                        if (!string.IsNullOrEmpty(fiscalRecord.Receiver.TaxPayerId) && fiscalRecord.Receiver.TaxPayerId.Length.Equals(12))
                        {
                            fiscalRecord.CFDIUsageCatalogList = CatalogListCFDI.Where(m => m.AppliesToMoralPerson.Equals(true) && m.AppliesToPhysicalPerson.Equals(true)).Select(
                                m => new CFDIUsageCatalog
                                {
                                    Code = m.Code,
                                    Description = m.Description
                                }).ToList();
                        }
                        else
                        {
                            List<CFDIUsageCatalog> CfdiComplete = CatalogListCFDI.Where(m => !m.Code.Equals(PreferredCFDIUsage[0].Code)).Select(m => new CFDIUsageCatalog
                            {
                                Code = m.Code,
                                Description = m.Description
                            }).ToList();

                            foreach (CFDIUsageCatalog cfdiCompleteList in CfdiComplete)
                                fiscalRecord.CFDIUsageCatalogList.Add(cfdiCompleteList);
                        }

                        #endregion Catalogo CFDI

                        #region Payment Type

                        fiscalRecord.PaymentTypeList = new List<FiscalRecordCatalog>();

                        List<FiscalRecordCatalog> PreferredPaymentType = fiscalRecordDataSet.Tables[0].AsEnumerable().Select(m => new FiscalRecordCatalog()
                        {
                            Code = m.Field<string>("PaymentType"),
                            Description = m.Field<string>("PaymentType") + "-" + m.Field<string>("PaymentTypeDesc")
                        }).ToList();

                        List<FiscalRecordCatalog> CatalogListPaymentType = catalogDA.GetFiscalRecordCatalog(Catalog.PaymentType);

                        IEnumerable<FiscalRecordCatalog> PaymentTypePreferred = CatalogListPaymentType.Where(m => m.Code.Equals(PreferredPaymentType[0].Code.ToString().Trim()));
                        foreach (FiscalRecordCatalog preferredPaymentType in PaymentTypePreferred)
                            fiscalRecord.PaymentTypeList.Add(preferredPaymentType);

                        IEnumerable<FiscalRecordCatalog> PaymentType = CatalogListPaymentType.Where(m => !m.Code.Equals(PreferredPaymentType[0].Code.ToString().Trim()));

                        foreach (FiscalRecordCatalog paymentType in PaymentType)
                            fiscalRecord.PaymentTypeList.Add(paymentType);

                        #endregion Payment Type

                        #region Serial for credit Note

                        fiscalRecordDataSet = new DataSet();
                        using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                        {
                            DbCommand command = database.GetStoredProcCommand("spSelFiscalRecordIssuerDefaultByUser");

                            database.AddInParameter(command, "@UserName", DbType.String, userName);
                            database.LoadDataSet(command, fiscalRecordDataSet, "CashReceiptInvoiceDetail");
                        }
                        foreach (DataRow dr in fiscalRecordDataSet.Tables[0].Rows)
                        {
                            fiscalRecord.SerialNumber = dr["CreditNoteSerialNumber"].ToString();
                            fiscalRecord.PaymentCondition = dr["PaymentCondition"].ToString();
                            fiscalRecord.ChargeCreditCodeId = string.IsNullOrEmpty(dr["ChargeCreditCodeId"].ToString()) ? 0 : Convert.ToInt32(dr["ChargeCreditCodeId"].ToString());
                            fiscalRecord.ChargeCreditCodeTaxId = string.IsNullOrEmpty(dr["ChargeCreditCodeTaxId"].ToString()) ? 0 : Convert.ToInt32(dr["ChargeCreditCodeTaxId"].ToString());
                        }

                        #endregion Serial for credit Note

                        #region Details

                        if (fiscalRecord.PaymentMethod.Equals(Constants.PPDPaymentMethod))
                        {
                            #region Details for PPD

                            decimal MaxAmountPPD = decimal.Parse(row["MaxAmountCreditNotePPD"].ToString());

                            using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                            {
                                DbCommand command = database.GetStoredProcCommand("spSelCreditNoteDetailByHeaderId");

                                database.AddInParameter(command, "@PeopleOrgCodeId", DbType.String, PeopleOrgCodeId);
                                database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, InvoiceHeaderId);
                                database.LoadDataSet(command, fiscalRecordDetailsDataSet, "CashReceiptInvoiceDetail");
                            }

                            fiscalRecord.InvoiceDetails = new List<ChargeCredit>();
                            ChargeCredit chargeCreditDetail = new ChargeCredit();
                            decimal Tax, TaxPercentaje, Amount = 0M, TotalTax = 0M;

                            //Validar si el detalle contiene impuestos
                            if (fiscalRecordDetailsDataSet.Tables[0].Rows.Count > 1) //Tiene impuesto asociado
                            {
                                decimal UnitAmount = decimal.Parse(fiscalRecordDetailsDataSet.Tables[0].Rows[0]["UnitAmount"].ToString());
                                decimal TaxAmount = decimal.Parse(fiscalRecordDetailsDataSet.Tables[0].Rows[1]["TotalTaxes"].ToString());

                                Tax = (TaxAmount * 100) / UnitAmount;
                                TaxPercentaje = (Tax / 100);
                                TaxPercentaje = 1 + TaxPercentaje;
                                Amount = MaxAmountPPD / TaxPercentaje;
                                TotalTax = MaxAmountPPD - Amount;
                                fiscalRecord.IsAPPDTaxCreditNote = true;

                                foreach (DataRow detail in fiscalRecordDetailsDataSet.Tables[0].Rows)
                                {
                                    chargeCreditDetail = new ChargeCredit
                                    {
                                        Quantity = int.Parse(detail["Quantity"].ToString()),
                                        ChargeCreditCode = detail["ChargeCreditCode"].ToString(),
                                        ChargeCreditDesc = detail["ChargeCreditDesc"].ToString(),
                                        ChargeCreditCodeId = int.Parse(detail["ChargeCreditCodeId"].ToString()),
                                        UnitAmount = decimal.Round(Amount, 2), //Rendondear Monto
                                        ProductServiceKey = string.IsNullOrEmpty(detail["ProductServiceKey"].ToString()) ? string.Empty : detail["ProductServiceKey"].ToString(),
                                        ProductServiceDesc = string.IsNullOrEmpty(detail["ProductServiceDesc"].ToString()) ? string.Empty : detail["ProductServiceDesc"].ToString(),
                                        UnitKey = string.IsNullOrEmpty(detail["UnityKey"].ToString()) ? string.Empty : detail["UnityKey"].ToString(),
                                        UnityName = string.IsNullOrEmpty(detail["UnityName"].ToString()) ? string.Empty : detail["UnityName"].ToString(),
                                        TotalTaxes = decimal.Round(TotalTax, 2), //Redondear Total Impuestos
                                        InvoiceDetailId = int.Parse(detail["InvoiceDetailId"].ToString()),
                                        IsATax = Convert.ToBoolean(detail["IsATax"]),
                                        ChargeNumberSource = string.IsNullOrEmpty(detail["ChargeNumberSource"].ToString()) ? (int?)null : int.Parse(detail["ChargeNumberSource"].ToString()),
                                        TaxChargeNumber = string.IsNullOrEmpty(detail["TaxChargeNumber"].ToString()) ? (int?)null : int.Parse(detail["TaxChargeNumber"].ToString()),
                                        SubjectToTax = detail["SubjectToTax"].ToString()
                                    };

                                    fiscalRecord.InvoiceDetails.Add(chargeCreditDetail);
                                }
                            }
                            else
                            {
                                chargeCreditDetail = new ChargeCredit
                                {
                                    Quantity = int.Parse(fiscalRecordDetailsDataSet.Tables[0].Rows[0]["Quantity"].ToString()),
                                    ChargeCreditCode = fiscalRecordDetailsDataSet.Tables[0].Rows[0]["ChargeCreditCode"].ToString(),
                                    ChargeCreditDesc = fiscalRecordDetailsDataSet.Tables[0].Rows[0]["ChargeCreditDesc"].ToString(),
                                    ChargeCreditCodeId = int.Parse(fiscalRecordDetailsDataSet.Tables[0].Rows[0]["ChargeCreditCodeId"].ToString()),
                                    ProductServiceKey = string.IsNullOrEmpty(fiscalRecordDetailsDataSet.Tables[0].Rows[0]["ProductServiceKey"].ToString()) ? string.Empty : fiscalRecordDetailsDataSet.Tables[0].Rows[0]["ProductServiceKey"].ToString(),
                                    ProductServiceDesc = string.IsNullOrEmpty(fiscalRecordDetailsDataSet.Tables[0].Rows[0]["ProductServiceDesc"].ToString()) ? string.Empty : fiscalRecordDetailsDataSet.Tables[0].Rows[0]["ProductServiceDesc"].ToString(),
                                    UnitKey = string.IsNullOrEmpty(fiscalRecordDetailsDataSet.Tables[0].Rows[0]["UnityKey"].ToString()) ? string.Empty : fiscalRecordDetailsDataSet.Tables[0].Rows[0]["UnityKey"].ToString(),
                                    UnityName = string.IsNullOrEmpty(fiscalRecordDetailsDataSet.Tables[0].Rows[0]["UnityName"].ToString()) ? string.Empty : fiscalRecordDetailsDataSet.Tables[0].Rows[0]["UnityName"].ToString(),
                                    InvoiceDetailId = int.Parse(fiscalRecordDetailsDataSet.Tables[0].Rows[0]["InvoiceDetailId"].ToString()),
                                    IsATax = Convert.ToBoolean(fiscalRecordDetailsDataSet.Tables[0].Rows[0]["IsATax"]),
                                    ChargeNumberSource = string.IsNullOrEmpty(fiscalRecordDetailsDataSet.Tables[0].Rows[0]["ChargeNumberSource"].ToString()) ? (int?)null : int.Parse(fiscalRecordDetailsDataSet.Tables[0].Rows[0]["ChargeNumberSource"].ToString()),
                                    TaxChargeNumber = string.IsNullOrEmpty(fiscalRecordDetailsDataSet.Tables[0].Rows[0]["TaxChargeNumber"].ToString()) ? (int?)null : int.Parse(fiscalRecordDetailsDataSet.Tables[0].Rows[0]["TaxChargeNumber"].ToString()),
                                    TotalTaxes = decimal.Parse(fiscalRecordDetailsDataSet.Tables[0].Rows[0]["TotalTaxes"].ToString()),
                                    UnitAmount = MaxAmountPPD,
                                    SubjectToTax = fiscalRecordDetailsDataSet.Tables[0].Rows[0]["SubjectToTax"].ToString(),
                                };
                                fiscalRecord.IsAPPDTaxCreditNote = false;

                                fiscalRecord.InvoiceDetails.Add(chargeCreditDetail);
                            }

                            #endregion Details for PPD
                        }
                        else
                        {
                            #region Details List

                            using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                            {
                                DbCommand command = database.GetStoredProcCommand("spSelCreditNoteDetailByHeaderId");

                                database.AddInParameter(command, "@PeopleOrgCodeId", DbType.String, PeopleOrgCodeId);
                                database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, InvoiceHeaderId);
                                database.LoadDataSet(command, fiscalRecordDetailsDataSet, "CashReceiptInvoiceDetail");
                            }
                            fiscalRecord.InvoiceDetails = new List<ChargeCredit>();
                            ChargeCredit chargeCreditDetail = new ChargeCredit();

                            foreach (DataRow detail in fiscalRecordDetailsDataSet.Tables[0].Rows)
                            {
                                if (!string.IsNullOrEmpty(PeopleOrgCodeId)) //Global
                                {
                                    chargeCreditDetail = new ChargeCredit
                                    {
                                        PeopleOrgCodeIdComplete = detail["PeopleOrgCodeId"].ToString(),
                                        Quantity = int.Parse(detail["Quantity"].ToString()),
                                        ChargeCreditCode = detail["ChargeCreditCode"].ToString(),
                                        ChargeCreditDesc = detail["ChargeCreditDesc"].ToString(),
                                        ChargeCreditCodeId = int.Parse(detail["ChargeCreditCodeId"].ToString()),
                                        UnitAmount = decimal.Parse(detail["UnitAmount"].ToString()),
                                        ProductServiceKey = string.IsNullOrEmpty(detail["ProductServiceKey"].ToString()) ? string.Empty : detail["ProductServiceKey"].ToString(),
                                        ProductServiceDesc = string.IsNullOrEmpty(detail["ProductServiceDesc"].ToString()) ? string.Empty : detail["ProductServiceDesc"].ToString(),
                                        UnitKey = string.IsNullOrEmpty(detail["UnityKey"].ToString()) ? string.Empty : detail["UnityKey"].ToString(),
                                        UnityName = string.IsNullOrEmpty(detail["UnityName"].ToString()) ? string.Empty : detail["UnityName"].ToString(),
                                        TotalTaxes = string.IsNullOrEmpty(detail["TotalTaxes"].ToString()) ? 0 : decimal.Parse(detail["TotalTaxes"].ToString()),
                                        ReceiptNumber = int.Parse(detail["ReceiptNumber"].ToString()),
                                        InvoiceDetailId = int.Parse(detail["InvoiceDetailId"].ToString()),
                                        IsATax = Convert.ToBoolean(detail["IsATax"]),
                                        ChargeNumberSource = string.IsNullOrEmpty(detail["ChargeNumberSource"].ToString()) ? (int?)null : int.Parse(detail["ChargeNumberSource"].ToString()),
                                        TaxChargeNumber = string.IsNullOrEmpty(detail["TaxChargeNumber"].ToString()) ? (int?)null : int.Parse(detail["TaxChargeNumber"].ToString()),
                                        SubjectToTax = detail["SubjectToTax"].ToString()
                                    };

                                    fiscalRecord.PeopleOrgCodeId = chargeCreditDetail.PeopleOrgCodeIdComplete;
                                    fiscalRecord.IsAGlobalCreditNote = true;
                                }
                                else
                                {
                                    chargeCreditDetail = new ChargeCredit
                                    {
                                        Quantity = int.Parse(detail["Quantity"].ToString()),
                                        ChargeCreditCode = detail["ChargeCreditCode"].ToString(),
                                        ChargeCreditDesc = detail["ChargeCreditDesc"].ToString(),
                                        ChargeCreditCodeId = int.Parse(detail["ChargeCreditCodeId"].ToString()),
                                        UnitAmount = decimal.Parse(detail["UnitAmount"].ToString()),
                                        ProductServiceKey = string.IsNullOrEmpty(detail["ProductServiceKey"].ToString()) ? string.Empty : detail["ProductServiceKey"].ToString(),
                                        ProductServiceDesc = string.IsNullOrEmpty(detail["ProductServiceDesc"].ToString()) ? string.Empty : detail["ProductServiceDesc"].ToString(),
                                        UnitKey = string.IsNullOrEmpty(detail["UnityKey"].ToString()) ? string.Empty : detail["UnityKey"].ToString(),
                                        UnityName = string.IsNullOrEmpty(detail["UnityName"].ToString()) ? string.Empty : detail["UnityName"].ToString(),
                                        TotalTaxes = string.IsNullOrEmpty(detail["TotalTaxes"].ToString()) ? 0 : decimal.Parse(detail["TotalTaxes"].ToString()),
                                        ReceiptNumber = int.Parse(detail["ReceiptNumber"].ToString()),
                                        InvoiceDetailId = int.Parse(detail["InvoiceDetailId"].ToString()),
                                        IsATax = Convert.ToBoolean(detail["IsATax"]),
                                        ChargeNumberSource = string.IsNullOrEmpty(detail["ChargeNumberSource"].ToString()) ? (int?)null : int.Parse(detail["ChargeNumberSource"].ToString()),
                                        TaxChargeNumber = string.IsNullOrEmpty(detail["TaxChargeNumber"].ToString()) ? (int?)null : int.Parse(detail["TaxChargeNumber"].ToString()),
                                        SubjectToTax = detail["SubjectToTax"].ToString()
                                    };

                                    fiscalRecord.IsAGlobalCreditNote = false;
                                }
                                fiscalRecord.InvoiceDetails.Add(chargeCreditDetail);
                            }

                            #endregion Details List
                        }

                        #endregion Details

                        #region Charge credit

                        //Si existen predefinidos en el setup agregarlos al principio

                        List<ChargeCredit> ChargeCreditList = new List<ChargeCredit>();
                        ChargeCreditList = catalogDA.GetAllChargeCredits();
                        fiscalRecord.ChargeCredit = new List<ChargeCredit>();

                        foreach (ChargeCredit chargeCredit in ChargeCreditList)
                        {
                            fiscalRecord.ChargeCredit.Add(chargeCredit);
                        }

                        #endregion Charge credit
                    }
                    return fiscalRecord;
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - CreditNoteDA - GetFiscalRecord", "GetFiscalRecord");
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - CreditNoteDA - GetFiscalRecord", ex.Message + ex.StackTrace);
                return null;
            }
        }
    }
}