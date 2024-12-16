// --------------------------------------------------------------------
// <copyright file="FiscalRecordDA.cs" company="Ellucian">
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
    /// FiscalRecordDA Class
    /// </summary>
    public class FiscalRecordDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FiscalRecordDA"/> class.
        /// </summary>
        public FiscalRecordDA()
        {
            _factory = new DatabaseProviderFactory();
        }

        /// <summary>
        /// Creates the fiscal record.
        /// </summary>
        /// <param name="fiscalRecordModel">The fiscal record model.</param>
        /// <returns></returns>
        public int CreateFiscalRecord(CreateFiscalRecord fiscalRecordModel)
        {
            int newInvoiceHeaderId = 0;
            try
            {
                bool isPaymentComplement = fiscalRecordModel.FiscalRecordType == Constants.FiscalRecordTypePago;
                bool isPPD = fiscalRecordModel.PaymentMethod == Constants.PPDPaymentMethod;
                // _logService.Debug("Method starts - CreateFiscalRecord");
                if (isPaymentComplement || fiscalRecordModel.Detail.Count > 0)
                {
                    DataSet cashReceiptDataset = new DataSet();
                    decimal total = decimal.Zero, subTotal = decimal.Zero, totalTaxes = decimal.Zero;

                    Database database = _factory.CreateDefault();
                    using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                    {
                        connection.Open();
                        DbTransaction trans = connection.BeginTransaction();
                        try
                        {
                            string sqlString;
                            string commandName;
                            int? commandValue;

                            #region Is Global Fiscal Record

                            if (fiscalRecordModel.IsGlobal && !fiscalRecordModel.IsSubstitution)
                            {
                                string initialDate = fiscalRecordModel.StartDate;
                                string finalDate = fiscalRecordModel.EndDate;

                                string StartDate = initialDate.Substring(6, 4) + "-" + initialDate.Substring(3, 2) + "-" + initialDate.Substring(0, 2);
                                string EndDate = finalDate.Substring(6, 4) + "-" + finalDate.Substring(3, 2) + "-" + finalDate.Substring(0, 2);

                                DbCommand selCashReceiptInvoiceDetailCommand = database.GetStoredProcCommand("dbo.spSelCashReceiptInvoiceDetailByDate");
                                if (fiscalRecordModel.ReceiptNumbersIncludedForGlobal != null && fiscalRecordModel.ReceiptNumbersIncludedForGlobal.Count > 0)
                                {
                                    database.AddInParameter(selCashReceiptInvoiceDetailCommand, "@StartDate", DbType.String, StartDate);
                                    database.AddInParameter(selCashReceiptInvoiceDetailCommand, "@EndDate", DbType.String, EndDate);
                                    database.AddInParameter(selCashReceiptInvoiceDetailCommand, "@InvExpeditionId", DbType.Int32, fiscalRecordModel.InvoiceExpeditionId);

                                    string receiptNumbersString = string.Empty;
                                    for (int i = 0; i < fiscalRecordModel.ReceiptNumbersIncludedForGlobal.Count; i++)
                                    {
                                        if (i > 0)
                                            receiptNumbersString += ",";

                                        int receiptNumber = fiscalRecordModel.ReceiptNumbersIncludedForGlobal[i];
                                        receiptNumbersString += receiptNumber.ToString();
                                    }

                                    database.AddInParameter(selCashReceiptInvoiceDetailCommand, "@ReceiptNumbers", DbType.String, receiptNumbersString);
                                }

                                database.LoadDataSet(selCashReceiptInvoiceDetailCommand, cashReceiptDataset, "CashReceiptInvoiceDetail", trans);
                            }

                            #endregion Is Global Fiscal Record

                            #region Is Individual Fiscal Record

                            else
                            {
                                if (fiscalRecordModel.IsSubstitution)
                                {
                                    sqlString = "dbo.spSelInvoiceDetailByHeaderIdForSubstitution";
                                    commandName = "@InvoiceHeaderId";
                                    commandValue = fiscalRecordModel.CFDIRelatedId;
                                }
                                else
                                {
                                    if (isPaymentComplement)
                                    {
                                        sqlString = "WebAdmin.spSelPaymentComplementDetail";
                                        commandName = "@InvoiceHeaderId";
                                        commandValue = fiscalRecordModel.CFDIRelatedId;
                                    }
                                    else if (isPPD)
                                    {
                                        sqlString = "dbo.spSelChargeCreditToPPD";
                                        commandName = "@ChargeCreditNumber";
                                        commandValue = fiscalRecordModel.ChargeCreditNumber;
                                    }
                                    else
                                    {
                                        sqlString = "dbo.spSelCashReceiptInvoiceDetail";
                                        commandName = "@ReceiptNumber";
                                        commandValue = fiscalRecordModel.ReceiptNumber;
                                    }
                                }

                                DbCommand selCashReceiptInvoiceDetailCommand = database.GetStoredProcCommand(sqlString);
                                database.AddInParameter(selCashReceiptInvoiceDetailCommand, commandName, DbType.Int32, commandValue);
                                if (isPaymentComplement)
                                    database.AddInParameter(selCashReceiptInvoiceDetailCommand, "@ReceiptNumber ", DbType.Int32, fiscalRecordModel.ReceiptNumber);
                                database.LoadDataSet(selCashReceiptInvoiceDetailCommand, cashReceiptDataset, "CashReceiptInvoiceDetail", trans);
                            }

                            #endregion Is Individual Fiscal Record

                            DataTable detailsTable;
                            DataTable detailsTaxTable;
                            DataTable totalsTable;
                            DataTable groupedDetailsTable = null;

                            if (fiscalRecordModel.IsGlobal && !fiscalRecordModel.IsSubstitution)
                            {
                                detailsTable = cashReceiptDataset.Tables[1];
                                detailsTaxTable = cashReceiptDataset.Tables[2];
                                totalsTable = cashReceiptDataset.Tables[3];
                            }
                            else
                            {
                                if (fiscalRecordModel.IsSubstitution || isPaymentComplement)
                                {
                                    detailsTable = cashReceiptDataset.Tables[2];
                                    detailsTaxTable = cashReceiptDataset.Tables[3];
                                    totalsTable = cashReceiptDataset.Tables[0];
                                    groupedDetailsTable = cashReceiptDataset.Tables[1];
                                }
                                else
                                {
                                    if (isPPD)
                                    {
                                        detailsTable = cashReceiptDataset.Tables[1];
                                        detailsTaxTable = cashReceiptDataset.Tables[2];
                                        totalsTable = cashReceiptDataset.Tables[0];
                                    }
                                    else
                                    {
                                        detailsTable = cashReceiptDataset.Tables[2];
                                        detailsTaxTable = cashReceiptDataset.Tables[3];
                                        totalsTable = cashReceiptDataset.Tables[4];
                                    }
                                }
                            }

                            if (!isPaymentComplement && totalsTable.Rows.Count > 0)
                            {
                                if (isPPD && !fiscalRecordModel.IsSubstitution)
                                {
                                    foreach (DataRow row in totalsTable.Rows)
                                    {
                                        subTotal = decimal.Parse(row["UnitAmount"].ToString());
                                        totalTaxes = string.IsNullOrEmpty(row["TotalTaxes"].ToString())
                                                     ? 0 : decimal.Parse(row["TotalTaxes"].ToString());
                                        total = decimal.Round(subTotal + totalTaxes, 2);
                                    }
                                }
                                else
                                {
                                    foreach (DataRow row in totalsTable.Rows)
                                    {
                                        subTotal = decimal.Parse(row["SubTotal"].ToString());
                                        totalTaxes = decimal.Parse(row["TotalTaxes"].ToString());
                                        total = decimal.Parse(row["Total"].ToString());
                                    }
                                }
                            }

                            if (detailsTable.Rows.Count > 0)
                            {
                                #region Insert InvoiceHeader

                                DbCommand insertInvoiceHeaderCommand = database.GetStoredProcCommand("dbo.spInsInvoiceHeader");

                                database.AddOutParameter(insertInvoiceHeaderCommand, "@InvoiceHeaderId", DbType.Int32, 0);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PeopleOrgCodeId", DbType.String, fiscalRecordModel.PeopleOrgCodeId);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@TaxpayerId", DbType.String, fiscalRecordModel.IssuerTaxPayerId);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@InvoiceExpeditionId", DbType.Int32, fiscalRecordModel.InvoiceExpeditionId);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentType", DbType.String, fiscalRecordModel.PaymentType);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentCondition", DbType.String, fiscalRecordModel.PaymentCondition.Trim());
                                database.AddInParameter(insertInvoiceHeaderCommand, "@ReceiverTaxpayerId", DbType.String, fiscalRecordModel.ReceiverTaxpayerId);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@ReceiverEmail", DbType.String, fiscalRecordModel.ReceiverEmail);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@Subtotal", DbType.Decimal, subTotal);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@Total", DbType.Decimal, total);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@TotalTransferTaxes", DbType.Decimal, totalTaxes);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@ExchangeRate", DbType.Decimal, decimal.One);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@Currency", DbType.String, fiscalRecordModel.Currency);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentMethod", DbType.String, fiscalRecordModel.PaymentMethod);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@CityOfIssue", DbType.String, fiscalRecordModel.CityOfIssue);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentAccountNumber", DbType.String, fiscalRecordModel.PaymentAccountNumber);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@TaxRegimen", DbType.String, fiscalRecordModel.TaxRegimen);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@Comments", DbType.String, fiscalRecordModel.Comments);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIUsageCode", DbType.String, fiscalRecordModel.CFDIUsageCode);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIUsageDesc", DbType.String, fiscalRecordModel.CFDIUsageDesc);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@TaxRegimenDesc", DbType.String, fiscalRecordModel.TaxRegimenDesc);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentTypeDesc", DbType.String, fiscalRecordModel.PaymentTypeDesc);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentMethodDesc", DbType.String, fiscalRecordModel.PaymentMethodDesc);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalIdentityNumber", DbType.String, fiscalRecordModel.FiscalIdentityNumber);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalResidency", DbType.String, fiscalRecordModel.FiscalResidency);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalResidencyDesc", DbType.String, fiscalRecordModel.FiscalResidencyDesc);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@SerialNumber", DbType.String, fiscalRecordModel.SerialNumber);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalRecordType", DbType.String, fiscalRecordModel.FiscalRecordType);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@FrequencyCode", DbType.String, fiscalRecordModel.FrequencyCode);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@MonthCode", DbType.String, fiscalRecordModel.MonthCode);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@Year", DbType.String, fiscalRecordModel.Year);
                                if (!isPaymentComplement)
                                {
                                    database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIRelated", DbType.String, fiscalRecordModel.CFDIRelated);
                                    database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIRelationType", DbType.String, fiscalRecordModel.CFDIRelationTypeKey);
                                }
                                if (fiscalRecordModel.IsGlobal)
                                {
                                    DateTime startDate1 = DateTime.ParseExact(fiscalRecordModel.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    DateTime endDate1 = DateTime.ParseExact(fiscalRecordModel.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    database.AddInParameter(insertInvoiceHeaderCommand, "@StartDate", DbType.DateTime, startDate1);
                                    database.AddInParameter(insertInvoiceHeaderCommand, "@EndDate", DbType.DateTime, endDate1);
                                }
                                else
                                {
                                    database.AddInParameter(insertInvoiceHeaderCommand, "@StartDate", DbType.DateTime, null);
                                    database.AddInParameter(insertInvoiceHeaderCommand, "@EndDate", DbType.DateTime, null);
                                }

                                database.ExecuteNonQuery(insertInvoiceHeaderCommand, trans);
                                newInvoiceHeaderId = (int)database.GetParameterValue(insertInvoiceHeaderCommand, "@InvoiceHeaderId");

                                #endregion Insert InvoiceHeader

                                if (newInvoiceHeaderId > 0)
                                {
                                    if (fiscalRecordModel.IsSubstitution)
                                    {
                                        DbCommand updateInvoiceHeaderForCancellation = database.GetStoredProcCommand("dbo.spUpdInvoiceHeaderForCancellation");

                                        database.AddInParameter(updateInvoiceHeaderForCancellation, "@InvoiceHeaderId", DbType.Int32, fiscalRecordModel.CFDIRelatedId);
                                        database.AddInParameter(updateInvoiceHeaderForCancellation, "@CancelReason", DbType.String, fiscalRecordModel.CancelReasonKey);
                                        database.AddParameter(updateInvoiceHeaderForCancellation, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, new object());

                                        database.ExecuteNonQuery(updateInvoiceHeaderForCancellation, trans);

                                        if ((int)database.GetParameterValue(updateInvoiceHeaderForCancellation, "@ReturnStatus") <= 0)
                                        {
                                            LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - CreateFiscalRecord", "Error on dbo.spUpdInvoiceHeaderForCancellation, ReturnStatus <= 0");
                                            trans.Rollback();
                                            return 0;
                                        }
                                    }

                                    List<FiscalRecordDetail> currentFiscalRecordDetails = new List<FiscalRecordDetail>();
                                    List<FiscalRecordDetail> currentFiscalRecordDetailTaxes = new List<FiscalRecordDetail>();
                                    if (detailsTable.Rows.Count > 0)
                                    {
                                        bool hasError = false;

                                        #region Get Current Fiscal Record Details

                                        foreach (DataRow row in detailsTable.Rows)
                                        {
                                            currentFiscalRecordDetails.Add(new FiscalRecordDetail
                                            {
                                                InvoiceHeaderId = newInvoiceHeaderId,
                                                ReceiptNumber = isPPD ? (int?)null : int.Parse(row["ReceiptNumber"].ToString()),
                                                ChargeCreditNumber = int.Parse(row["ChargeCreditNumber"].ToString()),
                                                ChargeCreditCodeId = fiscalRecordModel.IsGlobal ? 0 : int.Parse(row["ChargeCreditCodeId"].ToString()),
                                                ChargeCreditSource = isPPD ? (int?)null : int.Parse(row["ReceiptChrgCrdNumber"].ToString()),
                                                ChargeCreditCode = row["ChargeCreditCode"].ToString(),
                                                Description = row["ChargeCreditDesc"].ToString(),
                                                Amount = decimal.Parse(row["UnitAmount"].ToString()),
                                                IsATax = int.Parse(row["IsATax"].ToString()),
                                                TransferTaxType = isPPD ? null : row["ChargeCreditCode"].ToString(),
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
                                            currentFiscalRecordDetailTaxes.Add(new FiscalRecordDetail
                                            {
                                                ChargeCreditSource = (int)taxRow["ChargeCreditSource"],
                                                TaxCode = (string)taxRow["TaxKey"],
                                                Description = (string)taxRow["TaxDescription"],
                                                TaxFactorType = (string)taxRow["FactorTypeKey"],
                                                TransferRate = taxRow["TaxRate"] is DBNull ? null : (decimal?)taxRow["TaxRate"]
                                            });
                                        }

                                        #endregion Get Current Fiscal Record Details

                                        #region Insert InvoiceDetail and InvoiceDetailTax

                                        foreach (FiscalRecordDetail fiscalRecordDetail in currentFiscalRecordDetails)
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
                                            if (!isPaymentComplement && !fiscalRecordModel.IsGlobal && fiscalRecordDetail.IsATax == 0)
                                            {
                                                newDescription = fiscalRecordModel.Detail.FirstOrDefault
                                                    (m => m.ChargeCreditCodeId == fiscalRecordDetail.ChargeCreditCodeId).Description;
                                                newUnitName = fiscalRecordModel.Detail.First
                                                    (m => m.ChargeCreditCodeId == fiscalRecordDetail.ChargeCreditCodeId).UnitDescription;
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
                                                = currentFiscalRecordDetailTaxes.Find(t => t.ChargeCreditSource.Value == fiscalRecordDetail.ChargeCreditNumber
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

                                            if (isPPD)
                                            {
                                                #region Insert InvoiceChargeCredit

                                                /*Associate ChargeCredit with a Fiscal Record in InvoiceChargeCredit Table*/
                                                DbCommand insInvoiceChargeCreditCommand = database.GetStoredProcCommand("dbo.spInsInvoiceChargeCredit");

                                                database.AddInParameter(insInvoiceChargeCreditCommand, "@InvoiceHeaderId", DbType.Int32, newInvoiceHeaderId);
                                                database.AddInParameter(insInvoiceChargeCreditCommand, "@ChargeCreditNumber", DbType.Int32, fiscalRecordDetail.ChargeCreditNumber);
                                                database.AddParameter(insInvoiceChargeCreditCommand, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, new object());

                                                database.ExecuteNonQuery(insInvoiceChargeCreditCommand, trans);

                                                if ((int)database.GetParameterValue(insInvoiceChargeCreditCommand, "@ReturnStatus") <= 0)
                                                {
                                                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - CreateFiscalRecord", "Error on dbo.spInsInvoiceChargeCredit, ReturnStatus <= 0");
                                                    trans.Rollback();
                                                    return 0;
                                                }

                                                #endregion Insert InvoiceChargeCredit
                                            }
                                        }

                                        if (hasError)
                                        {
                                            trans.Rollback();
                                            return 0;
                                        }

                                        int invoicePaymentReceiptId = 0;
                                        if (isPaymentComplement)
                                        {
                                            #region Insert InsInvoicePaymentReceipt

                                            DbCommand insertInvoicePaymentReceiptCommand = database.GetStoredProcCommand("dbo.spInsInvoicePaymentReceipt");

                                            CreatePaymentReceipt createPaymentReceipt = (CreatePaymentReceipt)fiscalRecordModel;
                                            if (totalsTable.Rows.Count > 0)
                                            {
                                                // SAT: 12 hours because we do not have the exact hour
                                                createPaymentReceipt.PaymentDate = ((DateTime)groupedDetailsTable.Rows[0]["ApplicationDate"]).AddHours(12);
                                                createPaymentReceipt.PreviousBalanceAmount = (decimal)totalsTable.Rows[0]["PreviousBalanceAmount"];
                                                createPaymentReceipt.Amount = (decimal)totalsTable.Rows[0]["AmountPaid"];
                                                createPaymentReceipt.AmountPaid = (decimal)totalsTable.Rows[0]["AmountPaid"];
                                                createPaymentReceipt.OutstandingBalanceAmount = (decimal)totalsTable.Rows[0]["OutstandingBalanceAmount"];
                                            }
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@InvoiceHeaderId", DbType.Int32, newInvoiceHeaderId);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@PaymentDate", DbType.DateTime, createPaymentReceipt.PaymentDate);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@PaymentType", DbType.String, createPaymentReceipt.PaymentTypeComplement);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@Currency", DbType.String, createPaymentReceipt.CurrencyComplement);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@ExchangeRate", DbType.Decimal, decimal.One);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@Amount", DbType.Decimal, createPaymentReceipt.Amount);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@TransactionNumber", DbType.String, createPaymentReceipt.TransactionNumber);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@IssuerTaxpayerIdSourceAccount", DbType.Int32, createPaymentReceipt.IssuerTaxPayerIdSourceAccount);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@IssuerTaxpayerIdBeneficiaryAcc", DbType.Int32, createPaymentReceipt.IssuerTaxPayerIdBeneficiaryAccount);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@BankName", DbType.String, createPaymentReceipt.BankName);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@SourceAccount", DbType.String, createPaymentReceipt.SourceAccount);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@BeneficiaryAccount", DbType.String, createPaymentReceipt.BeneficiaryAccount);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@PaymentChainType", DbType.String, createPaymentReceipt.PaymentChainType);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@PaymentCertificate", DbType.String, createPaymentReceipt.PaymentCertificate);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@PaymentChain", DbType.String, createPaymentReceipt.PaymentChain);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@PaymentStamp", DbType.String, createPaymentReceipt.PaymentStamp);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@InstallmentNumber", DbType.Int32, createPaymentReceipt.InstallmentNumber);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@PreviousBalanceAmount", DbType.Decimal, createPaymentReceipt.PreviousBalanceAmount);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@AmountPaid", DbType.Decimal, createPaymentReceipt.AmountPaid);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@OutstandingBalanceAmount", DbType.Decimal, createPaymentReceipt.OutstandingBalanceAmount);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@ReceiptNumber", DbType.Int32, createPaymentReceipt.ReceiptNumber);
                                            database.AddInParameter(insertInvoicePaymentReceiptCommand, "@DocumentId", DbType.String, createPaymentReceipt.CFDIRelated);
                                            database.AddOutParameter(insertInvoicePaymentReceiptCommand, "@InvoicePaymentReceiptId", DbType.Int32, 0);

                                            database.ExecuteNonQuery(insertInvoicePaymentReceiptCommand, trans);

                                            invoicePaymentReceiptId = (int)database.GetParameterValue(insertInvoicePaymentReceiptCommand, "@InvoicePaymentReceiptId");
                                            if (invoicePaymentReceiptId <= 0)
                                            {
                                                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - CreateFiscalRecord", "Error on dbo.spInsInvoicePaymentReceipt, InvoicePaymentReceiptId <= 0");
                                                trans.Rollback();
                                                return 0;
                                            }

                                            #endregion Insert InsInvoicePaymentReceipt
                                        }

                                        #endregion Insert InvoiceDetail and InvoiceDetailTax

                                        #region Insert Invoice Request

                                        string spInsInvoiceRequest;
                                        string spInsInvoiceRequestParameter;
                                        int invoiceRequestValue;
                                        if (isPPD)
                                        {
                                            spInsInvoiceRequestParameter = "@InvoiceHeaderId";
                                            spInsInvoiceRequest = "dbo.spInsInvoiceRequestPPD";
                                            invoiceRequestValue = newInvoiceHeaderId;
                                        }
                                        else if (isPaymentComplement)
                                        {
                                            spInsInvoiceRequestParameter = "@InvoicePaymentReceiptId";
                                            spInsInvoiceRequest = "dbo.spInsInvoiceRequestPaymentReceipt";
                                            invoiceRequestValue = invoicePaymentReceiptId;
                                        }
                                        else
                                        {
                                            spInsInvoiceRequestParameter = "@InvoiceHeaderId";
                                            spInsInvoiceRequest = "dbo.spInsInvoiceRequest";
                                            invoiceRequestValue = newInvoiceHeaderId;
                                        }

                                        DbCommand insertInsInvoiceRequestCommand = database.GetStoredProcCommand(spInsInvoiceRequest);

                                        database.AddInParameter(insertInsInvoiceRequestCommand, spInsInvoiceRequestParameter, DbType.Int32, invoiceRequestValue);
                                        database.AddInParameter(insertInsInvoiceRequestCommand, "@Status", DbType.Int32, 1);
                                        database.AddOutParameter(insertInsInvoiceRequestCommand, "@InvoiceRequestId", DbType.Int32, 0);

                                        if (spInsInvoiceRequest== "dbo.spInsInvoiceRequest" || spInsInvoiceRequest == "dbo.spInsInvoiceRequestPPD")
                                            database.AddInParameter(insertInsInvoiceRequestCommand, "@IssueDate", DbType.String, fiscalRecordModel.ExpeditionDate.ToString());

                                        database.ExecuteNonQuery(insertInsInvoiceRequestCommand, trans);

                                        if ((int)database.GetParameterValue(insertInsInvoiceRequestCommand, "@InvoiceRequestId") <= 0)
                                        {
                                            LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - CreateFiscalRecord", "Error on dbo.spInsInvoiceRequest, InvoiceRequestId <= 0");
                                            trans.Rollback();
                                            return 0;
                                        }

                                        #endregion Insert Invoice Request
                                    }
                                    trans.Commit();
                                }
                                else
                                {
                                    trans.Rollback();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - CreateFiscalRecord", e.Message + e.StackTrace);
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
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - CreateFiscalRecord", e.Message + e.StackTrace);
                return 0;
            }
        }

        /// <summary>
        /// Creates the fiscal record.
        /// </summary>
        /// <param name="fiscalRecordModel">The fiscal record model.</param>
        /// <returns></returns>
        public int CreateGlobalForReason04(CreateFiscalRecord fiscalRecordModel)
        {
            int newInvoiceHeaderId = 0;
            try
            {
                if (fiscalRecordModel.Detail.Count > 0)
                {
                    DataSet cashReceiptDataset = new DataSet();
                    decimal total = 0, subTotal = 0, totalTaxes = 0;

                    Database database = _factory.CreateDefault();
                    using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                    {
                        connection.Open();
                        DbTransaction trans = connection.BeginTransaction();
                        try
                        {
                            DbCommand selCashReceiptInvoiceDetailCommand = database.GetStoredProcCommand("WebAdmin.spSelGlobalInvoiceDetailById");
                            database.AddInParameter(selCashReceiptInvoiceDetailCommand, "@InvoiceHeaderId", DbType.String, fiscalRecordModel.InvoiceHeaderId);
                            database.LoadDataSet(selCashReceiptInvoiceDetailCommand, cashReceiptDataset, "CashReceiptInvoiceDetail", trans);

                            DataTable detailsTable = cashReceiptDataset.Tables[1];
                            DataTable detailsTaxTable = cashReceiptDataset.Tables[2];
                            DataTable totalsTable = cashReceiptDataset.Tables[3];

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

                                DbCommand insertInvoiceHeaderCommand = database.GetStoredProcCommand("dbo.spInsInvoiceHeader");

                                database.AddOutParameter(insertInvoiceHeaderCommand, "@InvoiceHeaderId", DbType.Int32, 0);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PeopleOrgCodeId", DbType.String, fiscalRecordModel.PeopleOrgCodeId);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@TaxpayerId", DbType.String, fiscalRecordModel.IssuerTaxPayerId);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@InvoiceExpeditionId", DbType.Int32, fiscalRecordModel.InvoiceExpeditionId);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentType", DbType.String, fiscalRecordModel.PaymentType);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentCondition", DbType.String, fiscalRecordModel.PaymentCondition.Trim());
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
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentTypeDesc", DbType.String, fiscalRecordModel.PaymentTypeDesc);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@PaymentMethodDesc", DbType.String, fiscalRecordModel.PaymentMethodDesc);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalIdentityNumber", DbType.String, fiscalRecordModel.FiscalIdentityNumber);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalResidency", DbType.String, fiscalRecordModel.FiscalResidency);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalResidencyDesc", DbType.String, fiscalRecordModel.FiscalResidencyDesc);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@SerialNumber", DbType.String, fiscalRecordModel.SerialNumber);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@FiscalRecordType", DbType.String, Constants.FiscalRecordTypeIngreso);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIRelated", DbType.String, fiscalRecordModel.CFDIRelated);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@CFDIRelationType", DbType.String, fiscalRecordModel.CFDIRelationTypeKey);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@FrequencyCode", DbType.String, fiscalRecordModel.FrequencyCode);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@MonthCode", DbType.String, fiscalRecordModel.MonthCode);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@Year", DbType.String, fiscalRecordModel.Year);

                                DateTime startDate1 = DateTime.ParseExact(fiscalRecordModel.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                DateTime endDate1 = DateTime.ParseExact(fiscalRecordModel.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@StartDate", DbType.DateTime, startDate1);
                                database.AddInParameter(insertInvoiceHeaderCommand, "@EndDate", DbType.DateTime, endDate1);

                                database.ExecuteNonQuery(insertInvoiceHeaderCommand, trans);
                                newInvoiceHeaderId = (int)database.GetParameterValue(insertInvoiceHeaderCommand, "@InvoiceHeaderId");

                                #endregion Insert InvoiceHeader

                                if (newInvoiceHeaderId > 0)
                                {
                                    List<FiscalRecordDetail> currentFiscalRecordDetails = new List<FiscalRecordDetail>();
                                    List<FiscalRecordDetail> currentFiscalRecordDetailTaxes = new List<FiscalRecordDetail>();
                                    if (detailsTable.Rows.Count > 0)
                                    {
                                        bool hasError = false;

                                        #region Get Current Fiscal Record Details

                                        foreach (DataRow row in detailsTable.Rows)
                                        {
                                            currentFiscalRecordDetails.Add(new FiscalRecordDetail
                                            {
                                                InvoiceHeaderId = newInvoiceHeaderId,
                                                ReceiptNumber = int.Parse(row["ReceiptNumber"].ToString()),
                                                ChargeCreditNumber = int.Parse(row["ChargeCreditNumber"].ToString()),
                                                ChargeCreditCodeId = 0,
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
                                        if (detailsTaxTable != null)
                                        {
                                            foreach (DataRow taxRow in detailsTaxTable.Rows)
                                            {
                                                currentFiscalRecordDetailTaxes.Add(new FiscalRecordDetail
                                                {
                                                    ChargeCreditSource = (int)taxRow["ChargeCreditSource"],
                                                    TaxCode = (string)taxRow["TaxKey"],
                                                    Description = (string)taxRow["TaxDescription"],
                                                    TaxFactorType = (string)taxRow["FactorTypeKey"],
                                                    TransferRate = taxRow["TaxRate"] is DBNull ? null : (decimal?)taxRow["TaxRate"]
                                                });
                                            }
                                        }

                                        #endregion Get Current Fiscal Record Details

                                        #region Insert InvoiceDetail

                                        foreach (FiscalRecordDetail fiscalRecordDetail in currentFiscalRecordDetails)
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
                                                = currentFiscalRecordDetailTaxes.Find(t => t.ChargeCreditSource.Value == fiscalRecordDetail.ChargeCreditNumber
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

                                        #region Insert Invoice Request

                                        DbCommand insertInsInvoiceRequestCommand = database.GetStoredProcCommand("dbo.spInsInvoiceRequest");

                                        database.AddInParameter(insertInsInvoiceRequestCommand, "@InvoiceHeaderId", DbType.Int32, newInvoiceHeaderId);
                                        database.AddInParameter(insertInsInvoiceRequestCommand, "@Status", DbType.Int32, 1);
                                        database.AddOutParameter(insertInsInvoiceRequestCommand, "@InvoiceRequestId", DbType.Int32, 0);

                                        database.ExecuteNonQuery(insertInsInvoiceRequestCommand, trans);

                                        if ((int)database.GetParameterValue(insertInsInvoiceRequestCommand, "@InvoiceRequestId") <= 0)
                                        {
                                            LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - CreateFiscalRecord", "Error on dbo.spInsInvoiceRequest, InvoiceRequestId <= 0");
                                            trans.Rollback();
                                            return 0;
                                        }

                                        #endregion Insert Invoice Request
                                    }
                                    trans.Commit();
                                }
                                else
                                {
                                    trans.Rollback();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - CreateFiscalRecord", e.Message + e.StackTrace);
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
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - CreateFiscalRecord", e.Message + e.StackTrace);
                return 0;
            }
        }

        /// <summary>
        /// Deletes the fiscal record.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int DeleteFiscalRecord(int invoiceHeaderId, string userName)
        {
            try
            {
                int returnStatus;

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spDelInvoiceHeader");

                    database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, invoiceHeaderId);
                    database.AddInParameter(command, "@UserName", DbType.String, userName);
                    object internalValue = new object();
                    database.AddParameter(command, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, internalValue);
                    database.ExecuteNonQuery(command);
                    returnStatus = (int)database.GetParameterValue(command, "@ReturnStatus");
                }
                return returnStatus;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - DeleteFiscalRecord", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the fiscal record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="trans">The database transaction.</param>
        /// <returns></returns>
        public FiscalRecord GetFiscalRecord(int id, IDbTransaction trans = null)
        {
            try
            {
                DataSet cashReceiptDataSet = new DataSet();
                CatalogDA catalogDA = new CatalogDA();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("dbo.spSelFiscalRecordById");

                    database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, id);

                    if (trans != null)
                        database.LoadDataSet(command, cashReceiptDataSet, "cashReceipt", (DbTransaction)trans);
                    else
                        database.LoadDataSet(command, cashReceiptDataSet, "cashReceipt");
                }
                if (cashReceiptDataSet != null)
                {
                    if (cashReceiptDataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in cashReceiptDataSet.Tables[0].Rows)
                        {
                            #region FiscalRecord Info

                            FiscalRecord fiscalRecord = new FiscalRecord
                            {
                                InvoiceHeaderId = int.Parse(row["InvoiceHeaderId"].ToString()),
                                PeopleOrgCodeId = row["People_Org_Code_Id"].ToString(),
                                Frequency = row["FrequencyDescription"].ToString(),
                                MonthCode = row["MonthCode"].ToString(),
                                Year = row["Year"].ToString(),
                                Version = row["Version"].ToString(),

                                #region Receiver Information

                                Receiver = new Receiver
                                {
                                    TaxPayerId = row["ReceiverTaxpayerId"].ToString(),
                                    CorporateName = row["ReceiverCorporateName"].ToString(),
                                    Email = row["ReceiverEmail"].ToString(),
                                    FiscalResidency = row["FiscalResidency"].ToString(),
                                    FiscalResidencyDesc = row["FiscalResidencyDesc"].ToString(),
                                    FiscalIdentityNumber = row["FiscalIdentityNumber"].ToString(),
                                    PostalCode = row["ReceiverPostalCode"].ToString(),
                                    TaxRegimenCode = row["ReceiverTaxRegimen"].ToString(),
                                    TaxRegimenDesc = row["ReceiverTaxRegimenDesc"].ToString()
                                },

                                #endregion Receiver Information

                                #region Issuer Information

                                Issuer = new Issuer
                                {
                                    IssTaxpayerId = row["IssuerTaxpayerId"].ToString(),
                                    IssCorporateName = row["IssuerCorporateName"].ToString(),
                                    TaxRegimenCode = row["IssuerTaxRegimen"].ToString(),
                                    TaxRegimenDesc = row["IssuerTaxRegimenDesc"].ToString(),
                                    IssIssuingAdd = new List<IssuingAddress>() {
                                        new  IssuingAddress() {
                                            IssIssuingAddress = row["IssuingAddressDesc"].ToString(),
                                            IssPlaceIssue = row["PlaceOfIssue"].ToString(),
                                            IssPostalCode = row["ExpeditionPostalCode"].ToString(),
                                            IssInoviceExpeditionId = int.Parse(row["InvoiceExpeditionId"].ToString())
                                        }
                                    },
                                },

                                #endregion Issuer Information

                                CFDIUsageCode = row["CFDIUsageCode"].ToString(),
                                CFDIUsageDesc = row["CFDIUsageDesc"].ToString(),
                                SerialNumber = row["SerialNumber"].ToString(),
                                Folio = row["Folio"].ToString(),
                                InvoiceStatus = int.Parse(row["InvoiceStatus"].ToString()),
                                UUID = row["UUID"].ToString(),
                                FiscalRecordType = row["FiscalRecordType"].ToString(),
                                VoucherType = row["VoucherType"].ToString(),
                                PaymentMethod = row["PaymentMethod"].ToString(),
                                PaymentMethodDesc = row["PaymentMethodDesc"].ToString(),
                                PaymentType = row["PaymentType"].ToString(),
                                PaymentTypeDesc = row["PaymentTypeDesc"].ToString(),
                                PaymentCondition = row["PaymentCondition"].ToString(),
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
                                            : DateTime.Parse(row["EndDate"].ToString()),
                                CFDIRelated = row["CFDIRelated"] is DBNull ? null : (string)row["CFDIRelated"],
                                RelationTypeCode = row["CFDIRelationType"] is DBNull ? null : (string)row["CFDIRelationType"],
                                RelationTypeDesc = row["CFDIRelationTypeDesc"] is DBNull ? null : (string)row["CFDIRelationTypeDesc"],
                                CFDIRelated2 = row["CFDIRelated2"] is DBNull ? null : (string)row["CFDIRelated2"],
                                CFDIRelatedSubstitution = row["SustitutionUUID"] is DBNull ? null : (string)row["SustitutionUUID"],
                                RelationTypeCode2 = row["CFDIRelationType2"] is DBNull ? null : (string)row["CFDIRelationType2"],
                                RelationTypeDesc2 = row["CFDIRelationTypeDesc2"] is DBNull ? null : (string)row["CFDIRelationTypeDesc2"],
                                CanCreateCreditNote = (bool)row["CanCreateCreditNote"],
                                CanCancelInvoice = (bool)row["CanCancelInvoice"],
                                HasInvoiceRelated = (bool)row["HasInvoiceRelated"],
                                CancelReasonKey = row["CancelReason"] is DBNull ? null : (string)row["CancelReason"],
                                CancelReasonDesc = row["CancelReasonDesc"] is DBNull ? null : (string)row["CancelReasonDesc"],
                                CancellationDatetime = string.IsNullOrEmpty(row["CancellationDatetime"].ToString()) ? (DateTime?)null
                                            : DateTime.Parse(row["CancellationDatetime"].ToString()),
                                IsCancellationComplete = row["IsCancellationComplete"] is DBNull ? false : int.Parse(row["IsCancellationComplete"].ToString()) > 0
                            };

                            #endregion FiscalRecord Info

                            #region Fiscal Record Details

                            List<FiscalRecordDetail> fiscalRecordDetails = new List<FiscalRecordDetail>();
                            cashReceiptDataSet = new DataSet();
                            using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                            {
                                DbCommand command = database.GetStoredProcCommand("dbo.spSelInvoiceDetailByHeaderId");

                                database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, id);

                                if (trans != null)
                                    database.LoadDataSet(command, cashReceiptDataSet, "cashReceipt", (DbTransaction)trans);
                                else
                                    database.LoadDataSet(command, cashReceiptDataSet, "cashReceipt");
                            }
                            if (cashReceiptDataSet != null && cashReceiptDataSet.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow detailRow in cashReceiptDataSet.Tables[0].Rows)
                                {
                                    decimal totalUnit = 0;
                                    totalUnit = Convert.ToInt32(detailRow["Quantity"]) * decimal.Parse(detailRow["UnitAmount"].ToString());
                                    FiscalRecordDetail fiscalRecordDetail;
                                    if (!string.IsNullOrEmpty(fiscalRecord.PeopleOrgCodeId))
                                    {
                                        //Fiscal Record for Person/Organization
                                        fiscalRecordDetail = new FiscalRecordDetail
                                        {
                                            InvoiceDetailId = detailRow.Table.Columns.Contains("InvoiceDetailId") ? Convert.ToInt32(detailRow["InvoiceDetailId"]) : 0,
                                            Quantity = Convert.ToInt32(detailRow["Quantity"]),
                                            ChargeCreditCode = detailRow["ChargeCreditCode"].ToString(),
                                            ChargeCreditDesc = detailRow["ChargeCreditDesc"].ToString(),
                                            UnitAmount = string.IsNullOrEmpty(detailRow["UnitAmount"].ToString()) ? 0 : decimal.Parse(detailRow["UnitAmount"].ToString()),
                                            ProductServiceKey = detailRow["ProductServiceKey"].ToString(),
                                            ProductServiceDesc = detailRow["ProductServiceDesc"].ToString(),
                                            UnityKey = detailRow["UnityKey"].ToString(),
                                            UnityName = detailRow["UnityName"].ToString(),
                                            TotalTaxes = string.IsNullOrEmpty(detailRow["TotalTaxes"].ToString()) ? 0 : decimal.Parse(detailRow["TotalTaxes"].ToString()),
                                            ReceiptNumber = detailRow.Field<int?>("ReceiptNumber"),
                                            Amount = totalUnit,
                                            SubjectToTax = detailRow["SubjectToTax"].ToString()
                                        };
                                    }
                                    else
                                    {
                                        //Global Fiscal Record Details
                                        fiscalRecordDetail = new FiscalRecordDetail
                                        {
                                            Quantity = Convert.ToInt32(detailRow["Quantity"]),
                                            ReceiptNumber = detailRow.Field<int?>("ReceiptNumber"),
                                            ProductServiceKey = detailRow["ProductServiceKey"].ToString(),
                                            UnityKey = detailRow["UnityKey"].ToString(),
                                            ChargeCreditDesc = detailRow["Description"].ToString(),
                                            UnitAmount = string.IsNullOrEmpty(detailRow["UnitAmount"].ToString()) ? 0 : decimal.Parse(detailRow["UnitAmount"].ToString()),
                                            TotalTaxes = string.IsNullOrEmpty(detailRow["TaxAmount"].ToString()) ? 0 : decimal.Parse(detailRow["TaxAmount"].ToString()),
                                            PeopleOrgCodeId = detailRow["PeopleOrgCodeId"].ToString(),
                                            PeopleOrgFullName = CommonDA.GetPeopleOrgFullName(detailRow["PeopleOrgCodeId"].ToString()),
                                            CanCreateCreditNote = string.IsNullOrEmpty(detailRow["CanCreateCreditNote"].ToString()) ? false : bool.Parse(detailRow["CanCreateCreditNote"].ToString()),
                                            Amount = totalUnit,
                                            SubjectToTax = detailRow["SubjectToTax"].ToString()
                                        };
                                    }
                                    fiscalRecordDetails.Add(fiscalRecordDetail);
                                }
                            }

                            #endregion Fiscal Record Details

                            #region Fiscal Record Documents

                            cashReceiptDataSet = new DataSet();
                            using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                            {
                                DbCommand command = database.GetStoredProcCommand("dbo.spSelInvoiceCertificateByHeaderId");
                                database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, id);

                                if (trans != null)
                                    database.LoadDataSet(command, cashReceiptDataSet, "CashReceiptInvoiceDetail", (DbTransaction)trans);
                                else
                                    database.LoadDataSet(command, cashReceiptDataSet, "CashReceiptInvoiceDetail");
                            }
                            List<FiscalRecordCertificate> documents = new List<FiscalRecordCertificate>();
                            if (cashReceiptDataSet != null && cashReceiptDataSet.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow item in cashReceiptDataSet.Tables[0].Rows)
                                {
                                    FiscalRecordCertificate document = new FiscalRecordCertificate
                                    {
                                        FiscalRecordCertificateId = (item["InvoiceCertificateId"] != DBNull.Value) ? (int?)item["InvoiceCertificateId"] : null,
                                        ApprovedDateTime = (item["ApprovedDatetime"] != DBNull.Value) ? (DateTime?)item["ApprovedDatetime"] : null,
                                        PdfFile = string.IsNullOrEmpty(item["PDFDocument"].ToString()) ? null : (byte[])item["PDFDocument"],
                                    };
                                    if (document.FiscalRecordCertificateId == null)
                                    {
                                        documents = null;
                                        break;
                                    }
                                    else
                                        documents.Add(document);
                                }
                            }
                            fiscalRecord.FiscalRecordCertificateList = documents;

                            #endregion Fiscal Record Documents

                            fiscalRecord.FiscalRecordDetailList = fiscalRecordDetails;

                            fiscalRecord.PaymentTypeList = catalogDA.GetFiscalRecordCatalog(Catalog.PaymentType);
                            return fiscalRecord;
                        }
                        return null;
                    }
                    else
                    {
                        LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - GetFiscalRecord", string.Format("Fiscal Record {0} not found.", id.ToString()));
                        return null;
                    }
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - GetFiscalRecord", string.Format("Fiscal Record {0} not found.", id.ToString()));
                    return null;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - GetFiscalRecord", e.Message + e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the fiscal record by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public FiscalRecord GetFiscalRecordById(int id)
        {
            try
            {
                DataSet cashReceiptDataSet = new DataSet();
                CatalogDA catalogDA = new CatalogDA();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelFiscalRecordById");
                    database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, id);
                    database.LoadDataSet(command, cashReceiptDataSet, "cashReceipt");
                }
                if (cashReceiptDataSet != null)
                {
                    if (cashReceiptDataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in cashReceiptDataSet.Tables[0].Rows)
                        {
                            FiscalRecord fiscalRecord = new FiscalRecord
                            {
                                InvoiceHeaderId = int.Parse(row["InvoiceHeaderId"].ToString()),
                                PeopleOrgCodeId = row["People_Org_Code_Id"].ToString(),
                                Frequency = row["FrequencyCode"].ToString(),
                                MonthCode = row["MonthCode"].ToString(),
                                Year = row["Year"].ToString(),
                                Version = row["Version"].ToString(),
                                StartDate = string.IsNullOrEmpty(row["StartDate"].ToString()) ? (DateTime?)null
                                            : DateTime.Parse(row["StartDate"].ToString()),
                                EndDate = string.IsNullOrEmpty(row["EndDate"].ToString()) ? (DateTime?)null
                                            : DateTime.Parse(row["EndDate"].ToString()),
                            };

                            return fiscalRecord;
                        }
                        return null;
                    }
                    else
                    {
                        LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - GetFiscalRecordById", string.Format("Fiscal Record {0} not found.", id.ToString()));
                        return null;
                    }
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - GetFiscalRecordById", string.Format("Fiscal Record {0} not found.", id.ToString()));
                    return null;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - GetFiscaGetFiscalRecordByIdRecord", e.Message + e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the fiscal record certificate files.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <returns></returns>
        public List<FiscalRecordCertificate> GetFiscalRecordCertificateFiles(int invoiceHeaderId)
        {
            try
            {
                // _logService.Debug("Method starts - GetFiscalRecordCertificateFiles");
                List<FiscalRecordCertificate> fiscalRecordCertificateFiles = new List<FiscalRecordCertificate>();
                DataSet fiscalRecordCertificateFilesDataSet = new DataSet();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelInvoiceCertificateByHeaderId");

                    database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, invoiceHeaderId);
                    database.LoadDataSet(command, fiscalRecordCertificateFilesDataSet, "FiscalRecordCertificateFilesDataSet");
                }
                if (fiscalRecordCertificateFilesDataSet != null && fiscalRecordCertificateFilesDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in fiscalRecordCertificateFilesDataSet.Tables[0].Rows)
                    {
                        FiscalRecordCertificate document = new FiscalRecordCertificate
                        {
                            FiscalRecordCertificateId = (item["InvoiceCertificateId"] != DBNull.Value) ? (int?)item["InvoiceCertificateId"] : null,
                            XmlFile = item["XMLDocument"].ToString(),
                            PdfFile = string.IsNullOrEmpty(item["PDFDocument"].ToString()) ? null
                                            : (byte[])item["PDFDocument"],
                            ApprovedDateTime = string.IsNullOrEmpty(item["ApprovedDatetime"].ToString()) ? (DateTime?)null
                            : DateTime.Parse(item["ApprovedDatetime"].ToString())
                        };
                        fiscalRecordCertificateFiles.Add(document);
                    }
                }
                return fiscalRecordCertificateFiles;
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - GetFiscalRecordCertificateFiles", e.Message + e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Get the fiscal records related to a fiscal record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public List<FiscalRecord>[] GetFiscalRecordsRelated(int id)
        {
            DataSet fiscalRecordsDataSet = new DataSet();
            List<FiscalRecord>[] response = null;
            try
            {
                // _logService.Debug("Method starts - GetFiscalRecordsRelated");

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelInvoiceRelated");
                    database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, id);
                    database.LoadDataSet(command, fiscalRecordsDataSet, "FiscalRecordsDataSet");
                }

                #region Read DataSet

                if (fiscalRecordsDataSet.Tables?.Count > 1)
                {
                    response = new List<FiscalRecord>[3];

                    #region Read Fiscal Records

                    for (int i = 0; i < 3; i++)
                    {
                        if (fiscalRecordsDataSet.Tables[i].Rows.Count > 0)
                        {
                            response[i] = fiscalRecordsDataSet.Tables[i].AsEnumerable().Select(fr => new FiscalRecord
                            {
                                InvoiceHeaderId = fr.Field<int>("InvoiceHeaderId"),
                                ExpeditionDateTime = fr.Field<DateTime?>("ExpeditionDatetime"),
                                VoucherType = fr.Field<string>("VoucherType"),
                                FiscalRecordType = fr.Field<string>("FiscalRecordType"),
                                Folio = fr.Field<int?>("Folio").ToString(),
                                SerialNumber = fr.Field<string>("SerialNumber"),
                                Total = fr.Field<decimal>("Total"),
                                RequestState = fr["RequestStatus"] == DBNull.Value ? enumFiscalRecordStatus.Null : (enumFiscalRecordStatus)Enum.Parse(typeof(enumFiscalRecordStatus), fr["RequestStatus"].ToString()),
                                RelationshipType = fr.Field<string>("FiscalRecordType").Equals(Constants.FiscalRecordTypeEgreso) ? enumRelationshipType.CreditNote : (fr.Field<string>("FiscalRecordType").Equals(Constants.FiscalRecordTypePago) ? enumRelationshipType.AdvancePayment : enumRelationshipType.Null),
                                Receiver = null,
                                Issuer = null,
                                PaymentMethod = fr["PaymentMethod"] == DBNull.Value ? string.Empty : fr.Field<string>("PaymentMethod").ToString(),
                                UUID = fr["UUID"] == DBNull.Value ? string.Empty : fr.Field<string>("UUID").ToString(),
                                CFDIRelated = fr["CFDIRelated"] == DBNull.Value ? string.Empty : fr.Field<string>("CFDIRelated").ToString(),
                                CFDIRelationType = fr["CFDIRelationType"] == DBNull.Value ? string.Empty : fr.Field<string>("CFDIRelationType").ToString(),
                                CFDIRelationTypeDesc = fr["CFDIRelationTypeDesc"] == DBNull.Value ? string.Empty : fr.Field<string>("CFDIRelationTypeDesc").ToString(),
                                CFDIRelated2 = fr["CFDIRelated2"] == DBNull.Value ? string.Empty : fr.Field<string>("CFDIRelated2").ToString(),
                                CFDIRelationType2 = fr["CFDIRelationType2"] == DBNull.Value ? string.Empty : fr.Field<string>("CFDIRelationType2").ToString(),
                                CFDIRelationTypeDesc2 = fr["CFDIRelationTypeDesc2"] == DBNull.Value ? string.Empty : fr.Field<string>("CFDIRelationTypeDesc2").ToString()
                            }).ToList();
                        }
                    }

                    #endregion Read Fiscal Records
                }

                #endregion Read DataSet

                // _logService.Debug("Method ends - GetFiscalRecordsRelated");
                return response;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - GetFiscalRecordsRelated", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the global invoice cancellation details.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <returns></returns>
        public GlobalInvoiceCancellationDetails GetGlobalInvoiceCancellationDetails(int invoiceHeaderId)
        {
            GlobalInvoiceCancellationDetails globalInvoiceCancellationDetails = new GlobalInvoiceCancellationDetails();

            try
            {
                Database database = _factory.CreateDefault();
                DataSet cancellationDetailsDataSet = new DataSet();
                DbCommand command = database.GetStoredProcCommand("WebAdmin.spSelGlobalInvoiceCancellationDetails");
                database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, invoiceHeaderId);
                database.LoadDataSet(command, cancellationDetailsDataSet, "GlobalInvoiceCancellationDetails");

                if (cancellationDetailsDataSet != null && cancellationDetailsDataSet.Tables[0].Rows.Count > 0)
                {
                    globalInvoiceCancellationDetails.CashReceipts = new List<InvoiceCashReceipt>();
                    foreach (DataRow item in cancellationDetailsDataSet.Tables[0].Rows)
                    {
                        globalInvoiceCancellationDetails.CashReceipts.Add(new InvoiceCashReceipt
                        {
                            Amount = item.Field<decimal>("Amount"),
                            EntryDate = item.Field<DateTime>("EntryDate"),
                            PeopleOrgFullName = CommonDA.GetPeopleOrgFullName(item.Field<string>("PeopleOrgCodeId")),
                            PeopleOrgId = item.Field<string>("PeopleOrgCodeId"),
                            ReceiptNumber = item.Field<int>("ReceiptNumber"),
                            TaxAmount = item.Field<decimal>("TaxAmount")
                        });
                    }
                }

                if (cancellationDetailsDataSet != null && cancellationDetailsDataSet.Tables[1].Rows.Count > 0)
                {
                    globalInvoiceCancellationDetails.NewInvoices = new List<FiscalRecord>();
                    foreach (DataRow item in cancellationDetailsDataSet.Tables[1].Rows)
                    {
                        globalInvoiceCancellationDetails.NewInvoices.Add(new FiscalRecord
                        {
                            InvoiceHeaderId = item.Field<int>("InvoiceHeaderId"),
                            PeopleOrgCodeId = item.Field<string>("PeopleOrgCodeId"),
                            Issuer = new Issuer
                            {
                                IssTaxpayerId = item.Field<string>("TaxpayerId"),
                            },
                            Receiver = new Receiver
                            {
                                TaxPayerId = item.Field<string>("ReceiverTaxpayerId")
                            },
                            SerialNumber = item.Field<string>("SerialNumber"),
                            Folio = item.Field<int>("Folio").ToString(),
                            ExpeditionDateTime = item.Field<DateTime>("ExpeditionDatetime"),
                            FiscalRecordType = item.Field<string>("FiscalRecordType"),
                            Total = item.Field<decimal>("Total"),
                            InvoiceStatus = item.Field<byte>("InvoiceStatus"),
                            RequestStateId = string.IsNullOrEmpty(item["RequestStatus"].ToString()) ? -1 : int.Parse(item["RequestStatus"].ToString()),
                            RequestState = item["RequestStatus"].ToString() == "" ? enumFiscalRecordStatus.Null
                                : (enumFiscalRecordStatus)Enum.Parse(typeof(enumFiscalRecordStatus), item["RequestStatus"].ToString()),
                            CancelReasonKey = item["CancelReason"] is DBNull ? null : (string)item["CancelReason"],
                        });
                    }
                }

                return globalInvoiceCancellationDetails;
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - CreateFiscalRecord", e.Message + e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the available global invoice filters.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public GlobalInvoiceFilters GetGlobalInvoiceFilters(string startDate, string endDate)
        {
            DataSet filtersDataset = new DataSet();

            GlobalInvoiceFilters globalInvoiceFilters = new GlobalInvoiceFilters
            {
                AcademicPeriods = new List<AcademicPeriod>(),
                PaymentTypes = new List<FiscalRecordCatalog>()
            };

            try
            {
                string formattedStartDate = startDate.Substring(6, 4) + "-" + startDate.Substring(3, 2) + "-" + startDate.Substring(0, 2);
                string formattedEndDate = endDate.Substring(6, 4) + "-" + endDate.Substring(3, 2) + "-" + endDate.Substring(0, 2);

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("WebAdmin.spSelPaymentTypePeriodByReceiptDate");
                    database.AddInParameter(command, "@StartDate", DbType.String, formattedStartDate);
                    database.AddInParameter(command, "@EndDate", DbType.String, formattedEndDate);
                    database.LoadDataSet(command, filtersDataset, "GlobalInvoiceFilters");
                }

                DataTable paymentTypesTable = filtersDataset.Tables[0];
                DataTable academicPeriodsTable = filtersDataset.Tables[1];

                if (filtersDataset != null)
                {
                    if (filtersDataset.Tables.Count > 0)
                    {
                        if (paymentTypesTable.Rows.Count > 0)
                        {
                            FiscalRecordCatalog paymentType;
                            foreach (DataRow row in paymentTypesTable.Rows)
                            {
                                paymentType = new FiscalRecordCatalog
                                {
                                    Id = int.Parse(row["PaymentTypeId"].ToString()),
                                    Description = row["Description"].ToString()
                                };
                                globalInvoiceFilters.PaymentTypes.Add(paymentType);
                            }
                        }

                        if (academicPeriodsTable.Rows.Count > 0)
                        {
                            AcademicPeriod academicPeriod;
                            foreach (DataRow row in academicPeriodsTable.Rows)
                            {
                                academicPeriod = new AcademicPeriod
                                {
                                    Year = row["AcademicYear"].ToString(),
                                    Term = row["AcademicTerm"].ToString(),
                                    TermDesc = row["AcademicTermDesc"].ToString(),
                                    Session = row["AcademicSession"].ToString(),
                                    SessionDesc = row["AcademicSessionDesc"] is DBNull ? null : row["AcademicSessionDesc"].ToString(),
                                    TermSortOrder = row["AcademicTermSortOrder"].ToString(),
                                    SessionSortOrder = row["AcademicSessionSortOrder"] is DBNull ? null : row["AcademicSessionSortOrder"].ToString()
                                };

                                globalInvoiceFilters.AcademicPeriods.Add(academicPeriod);
                            }
                        }
                    }
                }
                return globalInvoiceFilters;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "GetGlobalInvoiceFilters", ex.Message + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Gets the payment complement.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier.</param>
        /// <param name="receiptNumber">The receipt number.</param>
        /// <returns></returns>
        public PaymentComplement GetPaymentComplement(int invoiceHeaderId, int receiptNumber)
        {
            try
            {
                DataSet dataSet = new DataSet();
                PaymentComplement paymentComplement = null;
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("WebAdmin.spSelPaymentComplementDetail");

                    database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, invoiceHeaderId);
                    database.AddInParameter(command, "@ReceiptNumber", DbType.Int32, receiptNumber);

                    database.LoadDataSet(command, dataSet, "PaymentComplement");
                }
                if (dataSet != null)
                {
                    if (dataSet.Tables[0].Rows.Count > 0 && dataSet.Tables[1].Rows.Count > 0)
                    {
                        DataRow totalsRow = dataSet.Tables[0].Rows[0];
                        DataRow groupedDetailsRow = dataSet.Tables[1].Rows[0];

                        paymentComplement = new PaymentComplement
                        {
                            AmountPaid = (decimal)totalsRow["AmountPaid"],
                            ChargeCreditCode = (string)groupedDetailsRow["ChargeCreditCode"],
                            ChargeCreditCodeId = (int)groupedDetailsRow["ChargeCreditCodeId"],
                            ChargeCreditDesc = (string)groupedDetailsRow["ChargeCreditDesc"],
                            InstallmentNumber = (int)totalsRow["InstallmentNumber"],
                            OutstandingBalanceAmount = (decimal)totalsRow["OutstandingBalanceAmount"],
                            PaymentDate = (DateTime)groupedDetailsRow["ApplicationDate"],
                            PreviousBalanceAmount = (decimal)totalsRow["PreviousBalanceAmount"],
                            ProductServiceDesc = (string)groupedDetailsRow["ProductServiceDesc"],
                            ProductServiceKey = (string)groupedDetailsRow["ProductServiceKey"],
                            ReceiptChargeCreditCodeId = (int)groupedDetailsRow["ReceiptChargeCreditCode"],
                            ReceiptNumber = (int)groupedDetailsRow["ReceiptNumber"],
                            SubjectToTax = (string)groupedDetailsRow["SubjectToTax"],
                            TotalTaxes = groupedDetailsRow["TotalTaxes"] is DBNull ? 0 : (decimal)groupedDetailsRow["TotalTaxes"],
                            UnitAmount = (decimal)groupedDetailsRow["UnitAmount"],
                            UnityKey = (string)groupedDetailsRow["UnityKey"],
                            UnityName = (string)groupedDetailsRow["UnityName"]
                        };

                        DataTable detailsTable = dataSet.Tables[2];
                        if (detailsTable.Rows.Count > 0)
                        {
                            foreach (DataRow detailsRow in detailsTable.Rows)
                            {
                                if ((int)detailsRow["IsATax"] == 1)
                                {
                                    paymentComplement.FactorType = (string)detailsRow["FactorType"];
                                    paymentComplement.TransferRate = detailsRow["TaxRate"] is DBNull ? decimal.Zero : (decimal)detailsRow["TaxRate"];
                                }
                            }
                        }

                        DataTable detailsTaxTable = dataSet.Tables[3];
                        if (detailsTaxTable.Rows.Count > 0)
                        {
                            foreach (DataRow detailsTaxRow in detailsTaxTable.Rows)
                            {
                                paymentComplement.FactorType = (string)detailsTaxRow["FactorTypeKey"];
                                paymentComplement.TransferRate = detailsTaxRow["TaxRate"] is DBNull ? decimal.Zero : (decimal)detailsTaxRow["TaxRate"];
                            }
                        }
                    }
                    else
                    {
                        LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - GetPaymentComplement", string.Format("Fiscal Record {0} not found.", $"{invoiceHeaderId}, {receiptNumber}"));
                    }
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - GetPaymentComplement", string.Format("Fiscal Record {0} not found.", $"{invoiceHeaderId}, {receiptNumber}"));
                }

                return paymentComplement;
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - GetPaymentComplement", e.Message + e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Get the fiscal records related to a fiscal record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public List<PaymentReceipt>[] GetPaymentReceiptsRelated(int id)
        {
            DataSet fiscalRecordsDataSet = new DataSet();
            List<PaymentReceipt>[] response = null;
            try
            {
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelInvoicePaymentReceiptRelated");
                    database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, id);
                    database.LoadDataSet(command, fiscalRecordsDataSet, "FiscalRecordsDataSet");
                }

                #region Read DataSet

                if (fiscalRecordsDataSet.Tables?.Count >= 4)
                {
                    response = new List<PaymentReceipt>[4];

                    #region Read Fiscal Records

                    // 0 - Fiscal Record by id
                    // 1 - Parent
                    // 2 - Childs
                    // 3 - Siblings
                    for (int i = 0; i < 4; i++)
                    {
                        response[i] = fiscalRecordsDataSet.Tables[i].AsEnumerable().Select(fr => new PaymentReceipt
                        {
                            InvoiceHeaderId = fr.Field<int>("InvoiceHeaderId"),
                            ExpeditionDateTime = fr.Field<DateTime?>("ExpeditionDatetime"),
                            VoucherType = fr.Field<string>("VoucherType"),
                            FiscalRecordType = fr.Field<string>("FiscalRecordType"),
                            Folio = fr.Field<int?>("Folio").ToString(),
                            SerialNumber = fr.Field<string>("SerialNumber"),
                            Total = fr.Field<decimal>("Total"),
                            RequestState = fr["RequestStatus"] == DBNull.Value ? enumFiscalRecordStatus.Null : (enumFiscalRecordStatus)Enum.Parse(typeof(enumFiscalRecordStatus), fr["RequestStatus"].ToString()),
                            RelationshipType = fr.Field<string>("FiscalRecordType").Equals(Constants.FiscalRecordTypeEgreso) ? enumRelationshipType.CreditNote : (fr.Field<string>("FiscalRecordType").Equals(Constants.FiscalRecordTypePago) ? enumRelationshipType.AdvancePayment : enumRelationshipType.Null),
                            Receiver = null,
                            Issuer = null,
                            Amount = fr.Field<decimal?>("Amount"),
                            AmountPaid = fr.Field<decimal?>("AmountPaid"),
                            BankName = fr.Field<string>("BankName"),
                            BeneficiaryAccount = fr.Field<string>("BeneficiaryAccount"),
                            Currency = fr.Field<string>("Currency"),
                            DocumentId = fr.Field<string>("DocumentId"),
                            ExchangeRate = fr.Field<decimal?>("ExchangeRate"),
                            InstallmentNumber = fr.Field<int?>("InstallmentNumber"),
                            InvoicePaymentReceiptId = fr.Field<int?>("InvoicePaymentReceiptId"),
                            IssuerTaxpayerIdBeneficiaryAccount = fr.Field<int?>("IssuerTaxpayerIdBeneficiaryAcc"),
                            IssuerTaxpayerIdSourceAccount = fr.Field<int?>("IssuerTaxpayerIdSourceAccount"),
                            OutstandingBalanceAmount = fr.Field<decimal?>("OutstandingBalanceAmount"),
                            PaymentCertificate = fr.Field<string>("PaymentCertificate"),
                            PaymentChain = fr.Field<string>("PaymentChain"),
                            PaymentChainType = fr.Field<string>("PaymentChainType"),
                            PaymentDate = fr.Field<DateTime?>("PaymentDate"),
                            PaymentStamp = fr.Field<string>("PaymentStamp"),
                            PreviousBalanceAmount = fr.Field<decimal?>("PreviousBalanceAmount"),
                            ReceiptNumber = fr.Field<int?>("ReceiptNumber"),
                            SourceAccount = fr.Field<string>("SourceAccount"),
                            TransactionNumber = fr.Field<string>("TransactionNumber"),
                            PaymentTypeComplement = fr.Field<string>("PaymentType")
                        }).ToList();
                    }

                    #endregion Read Fiscal Records
                }

                #endregion Read DataSet

                return response;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - GetPaymentReceiptsRelated", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Retrieve Fiscal Records
        /// </summary>
        /// <param name="invoiceFilters">The invoice filters</param>
        /// <returns></returns>
        public List<FiscalRecord> RetrieveFiscalRecords(InvoiceFilters invoiceFilters)
        {
            List<FiscalRecord> fiscalRecordModelList = new List<FiscalRecord>();
            try
            {
                DataSet fiscalRecordsDataSet = new DataSet();
                string startDateSearch = null;
                string endDateSearch = null;
                if (!string.IsNullOrEmpty(invoiceFilters.StartDate))
                {
                    string startDate = invoiceFilters.StartDate;
                    startDateSearch = startDate.Substring(6, 4) + "-" + startDate.Substring(3, 2) + "-" + startDate.Substring(0, 2);
                }

                if (!string.IsNullOrEmpty(invoiceFilters.EndDate))
                {
                    string endDate = invoiceFilters.EndDate;
                    endDateSearch = endDate.Substring(6, 4) + "-" + endDate.Substring(3, 2) + "-" + endDate.Substring(0, 2);
                }

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("dbo.spSelFiscalRecords");

                    database.AddInParameter(command, "@StartDate", DbType.String, startDateSearch);
                    database.AddInParameter(command, "@EndDate", DbType.String, endDateSearch);
                    database.AddInParameter(command, "@PeopleOrgCodeId", DbType.String, invoiceFilters.PeopleOrgCodeId);
                    database.AddInParameter(command, "@Folio", DbType.String, invoiceFilters.Folio);
                    database.AddInParameter(command, "@FiscalRecordType", DbType.String, invoiceFilters.FiscalRecordType);
                    database.AddInParameter(command, "@Keyword", DbType.String, invoiceFilters.Keyword);

                    if (invoiceFilters.Status != enumFiscalRecordStatus.Null)
                        database.AddInParameter(command, "@Status", DbType.Byte, (byte)invoiceFilters.Status);

                    database.LoadDataSet(command, fiscalRecordsDataSet, "FiscalRecordsDataSet");
                }
                if (fiscalRecordsDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in fiscalRecordsDataSet.Tables[0].Rows)
                    {
                        FiscalRecord FiscalRecord = new FiscalRecord
                        {
                            InvoiceHeaderId = int.Parse(row["InvoiceHeaderId"].ToString()),
                            PeopleOrgCodeId = row["People_Org_Code_Id"].ToString(),

                            Issuer = { IssTaxpayerId = row["TaxpayerId"].ToString(),
                                       IssIssuingAdd = new List<IssuingAddress>() {
                                        new  IssuingAddress() { IssIssuingAddress =row["Description"].ToString()}
                                      }},
                            SerialNumber = row["SerialNumber"].ToString(),
                            Folio = row["Folio"].ToString(),
                            ExpeditionDateTime = (DateTime)row["expeditionDatetime"],
                            Receiver = { TaxPayerId = row["ReceiverTaxpayerId"].ToString() },
                            FiscalRecordType = row["FiscalRecordType"].ToString(),
                            Total = decimal.Parse(row["Total"].ToString()),
                            InvoiceStatus = string.IsNullOrEmpty(row["InvoiceStatus"].ToString()) ? -1 : int.Parse(row["InvoiceStatus"].ToString()),
                            RequestStateId = string.IsNullOrEmpty(row["RequestStatus"].ToString()) ? -1 :
                                int.Parse(row["RequestStatus"].ToString()),
                            RequestState = row["RequestStatus"].ToString() == "" ?
                                enumFiscalRecordStatus.Null :
                                (enumFiscalRecordStatus)Enum.Parse(typeof(enumFiscalRecordStatus), row["RequestStatus"].ToString()),
                            CancelReasonKey = row["CancelReason"] is System.DBNull ? null : (string)row["CancelReason"],
                            IsCancellationComplete = row["IsCancellationComplete"] is DBNull ? false : int.Parse(row["IsCancellationComplete"].ToString()) > 0
                        };
                        fiscalRecordModelList.Add(FiscalRecord);
                    }
                }
                return fiscalRecordModelList;
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - RetrieveFiscalRecords", e.Message + e.StackTrace);
                throw;
            }
        }
    }
}