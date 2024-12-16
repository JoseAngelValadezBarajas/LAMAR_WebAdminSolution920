// --------------------------------------------------------------------
// <copyright file="CashReceiptDA.cs" company="Ellucian">
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
    /// CashReceiptDA
    /// Contains all the methods and functions about Cash Receipts
    /// </summary>
    public class CashReceiptDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CashReceiptDA"/> class.
        /// </summary>
        public CashReceiptDA()
        {
            _factory = new DatabaseProviderFactory();
        }

        /// <summary>
        /// Deletes the receipt payment mapping.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        public int DeleteReceiptPaymentMapping(int Id)
        {
            try
            {
                // _logService.Debug("Method starts - DeleteReceiptPaymentMapping");
                int returnStatus;

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spUpdReceiptPaymentMethodMapping");

                    database.AddInParameter(command, "@ChargeCreditCodeId", DbType.Int32, Id);
                    database.AddInParameter(command, "@PaymentTypeKey", DbType.String, DBNull.Value);
                    database.AddInParameter(command, "@Terminal", DbType.String, "0001");
                    database.AddInParameter(command, "@Opid", DbType.String, "");
                    object internalValue = new object();
                    database.AddParameter(command, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, internalValue);
                    database.ExecuteNonQuery(command);
                    returnStatus = (int)database.GetParameterValue(command, "@ReturnStatus");
                    // _logService.Debug("Method ends - DeleteReceiptPaymentMapping");
                    return returnStatus;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "DeleteReceiptPaymentMapping", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Get the cancelled global payment type.
        /// </summary>
        /// <param name="invoiceHeaderId">The cancelled global invoice header identifier.</param>
        /// <returns></returns>
        public List<FiscalRecordCatalog> GetCancelledGlobalPaymentType(int invoiceHeaderId)
        {
            DataSet globalPaymentTypeDataset = new DataSet();
            List<FiscalRecordCatalog> lstPaymentType = new List<FiscalRecordCatalog>();
            try
            {
                SettingsDA setting = new SettingsDA();
                Settings settings = new Settings
                {
                    Area = "SYSADMIN",
                    Section = "CURRENCY_FORMAT",
                    Label = "REPORT_FORMAT"
                };
                string currencyFormat = setting.GetSetting(settings);

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("WebAdmin.spSelGlobalInvoiceCancellationPaymentType");

                    database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, invoiceHeaderId);
                    database.LoadDataSet(command, globalPaymentTypeDataset, "GlobalInvoicePaymentType");
                }
                if (globalPaymentTypeDataset?.Tables.Count > 0 && globalPaymentTypeDataset.Tables[0].Rows.Count > 0)
                {
                    FiscalRecordCatalog paymentType;
                    foreach (DataRow row in globalPaymentTypeDataset.Tables[0].Rows)
                    {
                        paymentType = new FiscalRecordCatalog();
                        paymentType.Code = row["PaymentType"].ToString();
                        paymentType.Description = row["PaymentType"].ToString() + " - " + row["PaymentTypeDesc"].ToString() + " - " + ((decimal)(row["Amount"])).ToString(currencyFormat, CultureInfo.InvariantCulture);
                        lstPaymentType.Add(paymentType);
                    }
                }
                return lstPaymentType;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "GetCancelledGlobalPaymentType", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the cash receipt.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="isSubstitution">if set to <c>true</c> [is substitution].</param>
        /// <returns></returns>
        public CashReceipt GetCashReceipt(int id, bool isSubstitution)
        {
            try
            {
                string sqlString;
                string commandName;
                if (isSubstitution)
                {
                    sqlString = "spSelInvoiceDetailByHeaderIdForSubstitution";
                    commandName = "@InvoiceHeaderId";
                }
                else
                {
                    sqlString = "spSelCashReceiptInvoiceDetail";
                    commandName = "@ReceiptNumber";
                }
                // _logService.Debug("Method starts - GetCashReceipt");
                DataSet cashReceiptDataset = new DataSet();
                DataSet preferredReceiverDataSet = new DataSet();
                CashReceipt response = new CashReceipt();
                CatalogDA catalogDA = new CatalogDA();
                string paymentMethod = Constants.PaymentMethod;

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand(sqlString);

                    database.AddInParameter(command, commandName, DbType.Int32, id);
                    database.LoadDataSet(command, cashReceiptDataset, "CashReceiptInvoiceDetail");
                }

                DataTable headerTable;
                DataTable groupedDetailsTable;
                DataTable detailsTable;
                DataTable totalsTable;
                if (isSubstitution)
                {
                    headerTable = cashReceiptDataset.Tables[0];
                    groupedDetailsTable = cashReceiptDataset.Tables[1];
                    detailsTable = cashReceiptDataset.Tables[2];
                    totalsTable = cashReceiptDataset.Tables[0];
                }
                else
                {
                    headerTable = cashReceiptDataset.Tables[0];
                    groupedDetailsTable = cashReceiptDataset.Tables[1];
                    detailsTable = cashReceiptDataset.Tables[2];
                    totalsTable = cashReceiptDataset.Tables[4];
                }

                if (headerTable.Rows.Count > 0)
                {
                    // GET PREFERRED EMAIL, TAXPAYERID, PAYMENTMETHOD, INITIAL VALUES

                    #region Get Preferred

                    #region Receiver Info

                    List<Receiver> receiverModelList = isSubstitution ? headerTable.AsEnumerable().Select(m => new Receiver()
                    {
                        InvoiceTaxpayerId = m.Field<int?>("InvoiceTaxpayerId"),
                        TaxPayerId = m.Field<string>("TaxpayerId"),
                        PreferredCFDIUsageCode = m.Field<string>("CFDIUsageCode"),
                        FiscalIdentityNumber = m["FiscalIdentityNumber"] == DBNull.Value ? string.Empty : m.Field<string>("FiscalIdentityNumber")
                    }).ToList() : headerTable.AsEnumerable().Select(m => new Receiver()
                    {
                        InvoiceTaxpayerId = m.Field<int?>("InvoiceTaxpayerId"),
                        TaxPayerId = m.Field<string>("TaxpayerId"),
                        CorporateName = m.Field<string>("CorporateName"),
                        FiscalResidency = m["FiscalResidencyCode"] == DBNull.Value ? string.Empty : m.Field<string>("FiscalResidencyCode"),
                        FiscalResidencyDesc = m.Field<string>("FiscalResidencyDesc"),
                        FiscalIdentityNumber = m["FiscalIdentity"] == DBNull.Value ? string.Empty : m.Field<string>("FiscalIdentity"),
                        PreferredCFDIUsageCode = m.Field<string>("CFDIUsageCode")
                    }).ToList();

                    #endregion Receiver Info

                    CashReceipt cashReceiptModel = headerTable.AsEnumerable().Select(m => new CashReceipt()
                    {
                        preferredReceiverEmail = m.Field<string>("Email"),
                        peopleOrgId = m.Field<string>("PeopleCodeId")
                    }).FirstOrDefault();

                    response.peopleOrgId = cashReceiptModel.peopleOrgId;

                    if (string.IsNullOrEmpty(receiverModelList[0].TaxPayerId))
                    {
                        using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                        {
                            DbCommand command = database.GetStoredProcCommand("spRetrievePeopleOrgTaxpayerInfo");
                            database.AddInParameter(command, "@PeopleOrgCodeId", DbType.String, cashReceiptModel.peopleOrgId);
                            database.LoadDataSet(command, preferredReceiverDataSet, "PeopleOrgTaxpayerInfo");
                        }
                        if (preferredReceiverDataSet.Tables.Count > 0)
                        {
                            if (preferredReceiverDataSet.Tables[0].Rows.Count > 0)
                            {
                                List<Receiver> PreferredReceiver = preferredReceiverDataSet.Tables[0].AsEnumerable().Select(m => new Receiver()
                                {
                                    InvoiceTaxpayerId = m.Field<int>("InvoiceTaxpayerId"),
                                    TaxPayerId = m.Field<string>("TaxpayerId"),
                                    Email = m.Field<string>("Email"),
                                    CorporateName = m.Field<string>("CorporateName")
                                }).ToList();

                                List<CFDIUsageCatalog> PreferredCFDIUsage = preferredReceiverDataSet.Tables[0].AsEnumerable().Select(m => new CFDIUsageCatalog()
                                {
                                    Code = m.Field<string>("CFDIUsageCode"),
                                    Description = string.Concat(m.Field<string>("CFDIUsageCode"), "-", m.Field<string>("CFDIUsageDesc"))
                                }).ToList();

                                response.ReceiverList = PreferredReceiver;
                                response.preferredReceiverEmail = PreferredReceiver[0].Email;
                                response.CFDIList = new List<CFDIUsageCatalog>();

                                foreach (CFDIUsageCatalog preferredCfdi in PreferredCFDIUsage)
                                    response.CFDIList.Add(preferredCfdi);

                                List<CFDIUsageCatalog> CatalogListCFDI = catalogDA.GetCFDIUsage();

                                if (!string.IsNullOrEmpty(PreferredReceiver[0].TaxPayerId) && PreferredReceiver[0].TaxPayerId.Length.Equals(12))
                                {
                                    List<CFDIUsageCatalog> ListCFDI = CatalogListCFDI.Where(m => m.AppliesToMoralPerson == true && m.AppliesToPhysicalPerson == true).Select(m => new CFDIUsageCatalog
                                    {
                                        Code = m.Code,
                                        Description = m.Description
                                    }).ToList();

                                    foreach (CFDIUsageCatalog cfdi in ListCFDI)
                                        response.CFDIList.Add(cfdi);
                                }
                                else
                                {
                                    List<CFDIUsageCatalog> CfdiComplete = CatalogListCFDI.Where(m => !m.Code.Equals(PreferredCFDIUsage[0].Code)).Select(m => new CFDIUsageCatalog
                                    {
                                        Code = m.Code,
                                        Description = m.Description
                                    }).ToList();

                                    foreach (CFDIUsageCatalog cfdiCompleteList in CfdiComplete)
                                        response.CFDIList.Add(cfdiCompleteList);
                                }
                            }
                            else
                            {
                                response.ReceiverList = receiverModelList;
                                response.preferredReceiverEmail = cashReceiptModel.preferredReceiverEmail;

                                #region CFDI catalog

                                response.CFDIList = new List<CFDIUsageCatalog>();
                                List<CFDIUsageCatalog> CatalogListCFDI = catalogDA.GetCFDIUsage();
                                if (!string.IsNullOrEmpty(receiverModelList[0].TaxPayerId) && receiverModelList[0].TaxPayerId.Length.Equals(12))
                                {
                                    List<CFDIUsageCatalog> ListCFDI = CatalogListCFDI.Where(m => m.AppliesToMoralPerson == true && m.AppliesToPhysicalPerson == true).Select(m => new CFDIUsageCatalog
                                    {
                                        Code = m.Code,
                                        Description = m.Description
                                    }).ToList();

                                    foreach (CFDIUsageCatalog cfdi in ListCFDI)
                                        response.CFDIList.Add(cfdi);
                                }
                                else
                                {
                                    foreach (CFDIUsageCatalog cfdi in CatalogListCFDI)
                                        response.CFDIList.Add(cfdi);
                                }

                                #endregion CFDI catalog
                            }
                        }
                    }
                    else
                    {
                        response.ReceiverList = receiverModelList;
                        response.preferredReceiverEmail = cashReceiptModel.preferredReceiverEmail;

                        #region CFDI catalog

                        response.CFDIList = new List<CFDIUsageCatalog>();
                        List<CFDIUsageCatalog> CatalogListCFDI = catalogDA.GetCFDIUsage();
                        if (!string.IsNullOrEmpty(receiverModelList[0].TaxPayerId) && receiverModelList[0].TaxPayerId.Length.Equals(12))
                        {
                            List<CFDIUsageCatalog> ListCFDI = CatalogListCFDI.Where(m => m.AppliesToMoralPerson == true && m.AppliesToPhysicalPerson == true).Select(m => new CFDIUsageCatalog
                            {
                                Code = m.Code,
                                Description = m.Description
                            }).ToList();

                            foreach (CFDIUsageCatalog cfdi in ListCFDI)
                                response.CFDIList.Add(cfdi);
                        }
                        else
                        {
                            foreach (CFDIUsageCatalog cfdi in CatalogListCFDI)
                                response.CFDIList.Add(cfdi);
                        }

                        #endregion CFDI catalog
                    }

                    #endregion Get Preferred

                    response.PaymentType = isSubstitution ? new FiscalRecordCatalog()
                    {
                        Code = headerTable.Rows[0]["PaymentTypeKey"] == DBNull.Value ? string.Empty : (string)headerTable.Rows[0]["PaymentTypeKey"]
                    } : new FiscalRecordCatalog()
                    {
                        Description = headerTable.Rows[0]["PaymentTypeDesc"].ToString(),
                        Code = headerTable.Rows[0]["PaymentTypeCode"].ToString(),
                    };

                    if (isSubstitution)
                    {
                        paymentMethod = headerTable.Rows[0]["PaymentMethod"].ToString();
                        response.receiptNumber = detailsTable.Rows.Count <= 0 || detailsTable.Rows[0]["ReceiptNumber"] is DBNull ? 0 : (int)detailsTable.Rows[0]["ReceiptNumber"];
                        response.UUID = (string)headerTable.Rows[0]["UUID"];
                        response.Comments = headerTable.Rows[0]["Comments"] is DBNull ? null : (string)headerTable.Rows[0]["Comments"];
                        response.SerialNumber = (string)headerTable.Rows[0]["SerialNumber"];
                        response.IssuerTaxpayerId = (string)headerTable.Rows[0]["IssuerTaxpayerId"];
                        response.InvoiceExpeditionId = (int)headerTable.Rows[0]["InvoiceExpeditionId"];
                        response.PaymentCondition = headerTable.Rows[0]["PaymentCondition"] is DBNull ? null : (string)headerTable.Rows[0]["PaymentCondition"];
                    }
                    else
                    {
                        response.receiptNumber = id;
                        response.PostalCode = headerTable.Rows[0]["PostalCode"] is DBNull ? string.Empty : (string)headerTable.Rows[0]["PostalCode"];
                        response.RecTaxRegimen = headerTable.Rows[0]["TaxRegimenCode"] is DBNull ? string.Empty : string.Format("{0} - {1}", (string)headerTable.Rows[0]["TaxRegimenCode"], (string)headerTable.Rows[0]["TaxRegimenDesc"]);
                    }
                }

                #region PaymentType & PaymentMethod

                response.PaymentTypeList = new List<FiscalRecordCatalog>();
                List<FiscalRecordCatalog> CatalogListPaymentType = catalogDA.GetFiscalRecordCatalog(Catalog.PaymentType);
                if (CatalogListPaymentType != null)
                {
                    if (paymentMethod == Constants.PPDPaymentMethod)
                    {
                        response.PaymentType.Description = CatalogListPaymentType.Find(x => x.Code == response.PaymentType.Code).Description;
                    }
                    else
                    {
                        foreach (FiscalRecordCatalog paymentType in CatalogListPaymentType)
                            response.PaymentTypeList.Add(paymentType);
                    }
                }
                List<FiscalRecordCatalog> CatalogListPaymentMethod = catalogDA.GetFiscalRecordCatalog(Catalog.PaymentMethod);
                response.ReceiverPaymentMethodDefault = CatalogListPaymentMethod.Find(x => x.Code == paymentMethod).Description;

                #endregion PaymentType & PaymentMethod

                #region Cash receipt details

                // GET DETAILS OF CASH RECEIPT
                if (groupedDetailsTable.Rows.Count > 0)
                {
                    #region Get Details of Charges

                    response.chargesToInvoiceList = new List<ChargeCredit>();
                    List<ChargeCredit> chargeCreditList = new List<ChargeCredit>();
                    bool IsATaxCode = false;
                    if (detailsTable.Rows.Count > 0)
                    {
                        List<ChargeCredit> chargeCreditIsTax = new List<ChargeCredit>();
                        ChargeCredit chargeIsATax;
                        foreach (DataRow dr in detailsTable.Rows)
                        {
                            chargeIsATax = new ChargeCredit
                            {
                                IsATax = Convert.ToBoolean(dr["IsATax"]),
                                TaxCode = string.IsNullOrEmpty(dr["TaxCode"].ToString()) ? string.Empty : dr["TaxCode"].ToString()
                            };

                            if (chargeIsATax.IsATax.Equals(true) && chargeIsATax.TaxCode.Equals(string.Empty))
                                IsATaxCode = true;

                            chargeCreditIsTax.Add(chargeIsATax);
                        }
                    }

                    foreach (DataRow row in groupedDetailsTable.Rows)
                    {
                        ChargeCredit chargeCreditDetail = new ChargeCredit
                        {
                            Quantity = int.Parse(row["Quantity"].ToString()),
                            ChargeCreditCode = row["ChargeCreditCode"].ToString(),
                            ChargeCreditCodeId = int.Parse(row["ChargeCreditCodeId"].ToString()),
                            ChargeCreditDesc = row["ChargeCreditDesc"].ToString(),
                            UnitAmount = decimal.Parse(row["UnitAmount"].ToString()),
                            ProductServiceKey = string.IsNullOrEmpty(row["ProductServiceKey"].ToString()) ? string.Empty : row["ProductServiceKey"].ToString(),
                            ProductServiceDesc = string.IsNullOrEmpty(row["ProductServiceDesc"].ToString()) ? string.Empty : row["ProductServiceDesc"].ToString(),
                            UnitKey = string.IsNullOrEmpty(row["UnityKey"].ToString()) ? string.Empty : row["UnityKey"].ToString(),
                            UnityName = string.IsNullOrEmpty(row["UnityName"].ToString()) ? string.Empty : row["UnityName"].ToString(),
                            TotalTaxes = string.IsNullOrEmpty(row["TotalTaxes"].ToString()) ? 0 : decimal.Parse(row["TotalTaxes"].ToString()),
                            IsATax = IsATaxCode,
                            SubjectToTax = row["SubjectToTax"].ToString(),
                        };

                        chargeCreditDetail.IsEmptyProductServiceKey = string.IsNullOrEmpty(chargeCreditDetail.ProductServiceKey);

                        chargeCreditList.Add(chargeCreditDetail);
                    }
                    response.chargesToInvoiceList = chargeCreditList;

                    // GET TOTALS
                    if (totalsTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in totalsTable.Rows)
                        {
                            CashReceipt cashReceipt = new CashReceipt
                            {
                                SubTotal = string.IsNullOrEmpty(row["SubTotal"].ToString()) ? 0 : decimal.Parse(row["SubTotal"].ToString()),
                                TotalTT = string.IsNullOrEmpty(row["TotalTaxes"].ToString()) ? 0 : decimal.Parse(row["TotalTaxes"].ToString()),
                                Total = string.IsNullOrEmpty(row["Total"].ToString()) ? 0 : decimal.Parse(row["Total"].ToString())
                            };
                            response.SubTotal = cashReceipt.SubTotal;
                            response.TotalTT = cashReceipt.TotalTT;
                            response.Total = cashReceipt.Total;
                        }
                    }

                    #endregion Get Details of Charges
                }
                else
                    response.chargesToInvoiceList = null;

                #endregion Cash receipt details

                return response;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "GetCashReceipt", ex.Message + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Gets the cash receipt detail.
        /// </summary>
        /// <param name="receiptNumber">The receipt number.</param>
        /// <returns>The information about the application of a receipt</returns>
        public ChargeCreditApplication GetCashReceiptApplication(int receiptNumber)
        {
            DataSet cashReceiptDetailDataSet = new DataSet();
            ChargeCreditApplication response = null;
            try
            {
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("dbo.spSelChargeCreditsByReceiptNumber");

                    database.AddInParameter(command, "@ReceiptNumber", DbType.Int32, receiptNumber);
                    database.LoadDataSet(command, cashReceiptDetailDataSet, "ChargeCreditsByReceiptNumber");
                }

                #region Read DataSet

                if (cashReceiptDetailDataSet.Tables?.Count >= 2 && cashReceiptDetailDataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow receiptDetails = cashReceiptDetailDataSet.Tables[0].Rows[0];
                    response = new ChargeCreditApplication
                    {
                        ReceiptNumber = receiptNumber,
                        ChargeCreditNumber = receiptDetails.Field<int>("ChargeCreditNumber"),
                        EntryDate = receiptDetails.Field<DateTime>("EntryDate"),
                        TotalAmount = receiptDetails.Field<decimal>("TotalAmount"),
                        BalanceAmount = receiptDetails.Field<decimal>("BalanceAmount"),
                        CanCreateInvoice = receiptDetails.Field<bool>("CanCreateInvoice"),
                        IsVoid = receiptDetails.Field<bool>("IsVoid"),
                        IsReversed = receiptDetails.Field<bool>("IsReversed"),
                        PeopleOrgId = receiptDetails.Field<string>("PeopleOrgCodeId"),
                        PeopleOrgFullName = CommonDA.GetPeopleOrgFullName(receiptDetails.Field<string>("PeopleOrgCodeId"))
                    };

                    #region Read Charges

                    if (cashReceiptDetailDataSet.Tables[1].Rows.Count > 0)
                    {
                        response.Charges = cashReceiptDetailDataSet.Tables[1].AsEnumerable().Select(c => new ChargeCreditApplicationDetails
                        {
                            InvoiceHeaderId = c.Field<int?>("InvoiceHeaderId"),
                            InvoiceHeaderIdRelated = c.Field<int?>("InvoiceHeaderIdRelated"),
                            PaymentMethod = c.Field<string>("PaymentMethod"),
                            ApplicationDate = c.Field<DateTime>("ApplicationDate"),
                            ChargeCreditNumber = c.Field<int>("ChargeCreditNumber"),
                            ChargeCreditCode = c.Field<string>("Code"),
                            ChargeCreditDesc = c.Field<string>("Description"),
                            AmountApplied = c.Field<decimal>("AmountApplied"),
                            TotalAmount = c.Field<decimal>("Amount"),
                            CanCreateSupplement = c.Field<bool>("CanCreateSupplement")
                        }).ToList();
                    }

                    #endregion Read Charges
                }

                #endregion Read DataSet

                return response;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "GetCashReceiptApplication", ex.Message + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Gets the mapping product service.
        /// </summary>
        /// <returns></returns>
        public List<ReceiptPaymentMethodMapping> GetChargeCreditMapping()
        {
            try
            {
                DataSet chargeCreditMappingDataSet = new DataSet();
                List<ReceiptPaymentMethodMapping> lstChargeCreditMapping = new List<ReceiptPaymentMethodMapping>();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelFiscalRecordReceiptPaymentMapping");
                    database.LoadDataSet(command, chargeCreditMappingDataSet, "FiscalRecordReceiptPaymentMapping");
                }
                if (chargeCreditMappingDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in chargeCreditMappingDataSet.Tables[0].Rows)
                    {
                        ReceiptPaymentMethodMapping receiptPaymentMethodMapping = new ReceiptPaymentMethodMapping();

                        receiptPaymentMethodMapping.ReceiptPaymentMethodMappingId = (int)row["ReceiptPaymentMethodMappingId"];
                        receiptPaymentMethodMapping.ChargeCreditCodeId = (int)row["ChargeCreditCodeId"];
                        receiptPaymentMethodMapping.ChargeCreditDesc = row["ChargeCreditCode"] + " - " + row["ChargeCreditCodeDesc"].ToString();
                        receiptPaymentMethodMapping.PaymentMethodCode = row["PaymentMethodCode"].ToString();
                        receiptPaymentMethodMapping.PaymentMethodDesc = row["PaymentMethodCode"].ToString() + " - " + row["PaymentMethodDesc"].ToString();
                        lstChargeCreditMapping.Add(receiptPaymentMethodMapping);
                    }
                }
                return lstChargeCreditMapping;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "GetChargeCreditMapping", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the charge credits for a receipt.
        /// </summary>
        /// <param name="receiptNumber">The receipt number.</param>
        /// <returns></returns>
        public List<ChargeCredit> GetChargeCreditsByReceipt(int receiptNumber)
        {
            try
            {
                DataSet chargeCreditDataSet = new DataSet();
                List<ChargeCredit> chargeCredits = new List<ChargeCredit>();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("dbo.spSelCashReceiptInvoiceDetail");
                    database.AddInParameter(command, "@ReceiptNumber", DbType.Int32, receiptNumber);
                    database.LoadDataSet(command, chargeCreditDataSet, "ReceiptChargeCredits");
                }
                if (chargeCreditDataSet.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in chargeCreditDataSet.Tables[1].Rows)
                    {
                        chargeCredits.Add(new ChargeCredit
                        {
                            Quantity = int.Parse(row["Quantity"].ToString()),
                            ChargeCreditCode = row["ChargeCreditCode"].ToString(),
                            ChargeCreditCodeId = int.Parse(row["ChargeCreditCodeId"].ToString()),
                            ChargeCreditDesc = row["ChargeCreditDesc"].ToString(),
                            UnitAmount = decimal.Parse(row["UnitAmount"].ToString()),
                            ProductServiceKey = string.IsNullOrEmpty(row["ProductServiceKey"].ToString()) ? string.Empty : row["ProductServiceKey"].ToString(),
                            ProductServiceDesc = string.IsNullOrEmpty(row["ProductServiceDesc"].ToString()) ? string.Empty : row["ProductServiceDesc"].ToString(),
                            UnitKey = string.IsNullOrEmpty(row["UnityKey"].ToString()) ? string.Empty : row["UnityKey"].ToString(),
                            UnityName = string.IsNullOrEmpty(row["UnityName"].ToString()) ? string.Empty : row["UnityName"].ToString(),
                            TotalTaxes = string.IsNullOrEmpty(row["TotalTaxes"].ToString()) ? 0 : decimal.Parse(row["TotalTaxes"].ToString()),
                            SubjectToTax = row["SubjectToTax"].ToString(),
                        });
                    }
                }
                return chargeCredits;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "GetChargeCreditMapping", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// View the global fiscal records.
        /// </summary>
        /// <param name="globalInvoiceFiltersForCreation">The global invoice filters for creation.</param>
        /// <param name="isSubstitution">if set to <c>true</c> [is substitution].</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public CashReceipt GetGlobalCashReceipts(GlobalInvoiceFiltersForCreation globalInvoiceFiltersForCreation, bool isSubstitution, int id)
        {
            DataSet globalCashReceiptDataset = new DataSet();
            CashReceipt response = null;
            try
            {
                if (isSubstitution)
                {
                    Database database = _factory.CreateDefault();
                    using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                    {
                        DbCommand command = database.GetStoredProcCommand("spSelInvoiceDetailByHeaderIdForSubstitution");
                        database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, id);

                        database.LoadDataSet(command, globalCashReceiptDataset, "CashReceiptInvoiceDetailByDate");
                    }
                }
                else
                {
                    string startDateFormatted = globalInvoiceFiltersForCreation.StartDate.Substring(6, 4) + "-" + globalInvoiceFiltersForCreation.StartDate.Substring(3, 2) + "-" + globalInvoiceFiltersForCreation.StartDate.Substring(0, 2);
                    string endDateFormatted = globalInvoiceFiltersForCreation.EndDate.Substring(6, 4) + "-" + globalInvoiceFiltersForCreation.EndDate.Substring(3, 2) + "-" + globalInvoiceFiltersForCreation.EndDate.Substring(0, 2);

                    Database database = _factory.CreateDefault();
                    using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                    {
                        DbCommand command = database.GetStoredProcCommand("spSelCashReceiptInvoiceDetailByDate");
                        if (isSubstitution)
                        {
                            database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, id);
                        }
                        else
                        {
                            database.AddInParameter(command, "@StartDate", DbType.String, startDateFormatted);
                            database.AddInParameter(command, "@EndDate", DbType.String, endDateFormatted);
                            database.AddInParameter(command, "@InvExpeditionId", DbType.Int32, globalInvoiceFiltersForCreation.InvExpeditionId );
                            database.AddInParameter(command, "@AcademicYear", DbType.String, globalInvoiceFiltersForCreation.AcademicYear);
                            database.AddInParameter(command, "@AcademicTerm", DbType.String, globalInvoiceFiltersForCreation.AcademicTerm);
                            database.AddInParameter(command, "@AcademicSession", DbType.String, globalInvoiceFiltersForCreation.AcademicSession);

                            if (globalInvoiceFiltersForCreation.PaymentTypeId > 0)
                                database.AddInParameter(command, "@PaymentTypeId", DbType.Int32, globalInvoiceFiltersForCreation.PaymentTypeId);

                            if (globalInvoiceFiltersForCreation.ReceiptNumbers != null)
                            {
                                string receiptNumbersString = string.Empty;
                                for (int i = 0; i < globalInvoiceFiltersForCreation.ReceiptNumbers.Count; i++)
                                {
                                    if (i > 0)
                                        receiptNumbersString += ",";

                                    int receiptNumber = globalInvoiceFiltersForCreation.ReceiptNumbers[i];
                                    receiptNumbersString += receiptNumber.ToString();
                                }

                                database.AddInParameter(command, "@ReceiptNumbers", DbType.String, receiptNumbersString);
                            }
                        }

                        database.LoadDataSet(command, globalCashReceiptDataset, "CashReceiptInvoiceDetailByDate");
                    }
                }

                DataTable headerTable = null;
                DataTable groupedDetailsTable;
                DataTable detailsTable;
                DataTable totalsTable;
                if (isSubstitution)
                {
                    headerTable = globalCashReceiptDataset.Tables[0];
                    groupedDetailsTable = globalCashReceiptDataset.Tables[1];
                    detailsTable = globalCashReceiptDataset.Tables[2];
                    totalsTable = globalCashReceiptDataset.Tables[0];
                }
                else
                {
                    groupedDetailsTable = globalCashReceiptDataset.Tables[0];
                    detailsTable = globalCashReceiptDataset.Tables[1];
                    totalsTable = globalCashReceiptDataset.Tables[3];
                }

                if (globalCashReceiptDataset != null)
                {
                    if (globalCashReceiptDataset.Tables.Count > 0)
                    {
                        if (groupedDetailsTable.Rows.Count > 0)
                        {
                            response = new CashReceipt();

                            #region Get Header Info

                            if (isSubstitution)
                            {
                                response.StartDate = string.IsNullOrEmpty(headerTable.Rows[0]["StartDate"].ToString()) ? (DateTime?)null
                                          : DateTime.Parse(headerTable.Rows[0]["StartDate"].ToString());
                                response.EndDate = string.IsNullOrEmpty(headerTable.Rows[0]["EndDate"].ToString()) ? (DateTime?)null
                                            : DateTime.Parse(headerTable.Rows[0]["EndDate"].ToString());
                                response.preferredReceiverEmail = (string)headerTable.Rows[0]["Email"];
                                response.UUID = (string)headerTable.Rows[0]["UUID"];
                                response.Comments = headerTable.Rows[0]["Comments"] is DBNull ? null : (string)headerTable.Rows[0]["Comments"];
                                response.SerialNumber = (string)headerTable.Rows[0]["SerialNumber"];
                                response.IssuerTaxpayerId = (string)headerTable.Rows[0]["IssuerTaxpayerId"];
                                response.InvoiceExpeditionId = (int)headerTable.Rows[0]["InvoiceExpeditionId"];
                                response.PaymentType = new FiscalRecordCatalog()
                                {
                                    Code = headerTable.Rows[0]["PaymentTypeKey"] == DBNull.Value ? string.Empty : (string)headerTable.Rows[0]["PaymentTypeKey"]
                                };
                                response.FrequencyCode = headerTable.Rows[0]["FrequencyCode"] is DBNull ? null : (string)headerTable.Rows[0]["FrequencyCode"];
                                response.Year = headerTable.Rows[0]["Year"] is DBNull ? null : (string)headerTable.Rows[0]["Year"];
                                response.MonthCode = headerTable.Rows[0]["MonthCode"] is DBNull ? null : (string)headerTable.Rows[0]["MonthCode"];
                            }

                            #endregion Get Header Info

                            #region Get Details of Charges

                            response.chargesToInvoiceList = new List<ChargeCredit>();
                            List<ChargeCredit> chargeCreditList = new List<ChargeCredit>();
                            bool isATaxCode = false;

                            if (detailsTable.Rows.Count > 0)
                            {
                                List<ChargeCredit> chargeCreditIsTax = new List<ChargeCredit>();

                                ChargeCredit chargeIsATax;
                                foreach (DataRow dr in detailsTable.Rows)
                                {
                                    chargeIsATax = new ChargeCredit
                                    {
                                        IsATax = Convert.ToBoolean(dr["IsATax"]),
                                        TaxCode = string.IsNullOrEmpty(dr["TaxCode"].ToString()) ? string.Empty : dr["TaxCode"].ToString()
                                    };

                                    if (chargeIsATax.IsATax.Equals(true) && chargeIsATax.TaxCode.Equals(string.Empty))
                                        isATaxCode = true;

                                    chargeCreditIsTax.Add(chargeIsATax);
                                }
                            }

                            ChargeCredit chargeCreditDetai;
                            foreach (DataRow row in groupedDetailsTable.Rows)
                            {
                                chargeCreditDetai = new ChargeCredit
                                {
                                    Quantity = int.Parse(row["Quantity"].ToString()),
                                    ReceiptNumber = int.Parse(row["ReceiptNumber"].ToString()),
                                    ProductServiceKey = string.IsNullOrEmpty(row["ProductServiceKey"].ToString()) ? string.Empty : row["ProductServiceKey"].ToString(),
                                    UnitKey = string.IsNullOrEmpty(row["UnityKey"].ToString()) ? string.Empty : row["UnityKey"].ToString(),
                                    ChargeCreditDesc = row["ChargeCreditDesc"].ToString(),
                                    UnitAmount = decimal.Parse(row["UnitAmount"].ToString()),
                                    TotalTaxes = string.IsNullOrEmpty(row["TaxAmount"].ToString()) ? 0 : decimal.Parse(row["TaxAmount"].ToString()),
                                    IsATax = isATaxCode,
                                    SubjectToTax = row["SubjectToTax"].ToString()
                                };
                                chargeCreditList.Add(chargeCreditDetai);
                            }
                            response.chargesToInvoiceList = chargeCreditList.OrderBy(m => m.ReceiptNumber).ToList();

                            if (totalsTable.Rows.Count > 0)
                            {
                                CashReceipt cashReceipt;
                                foreach (DataRow row in totalsTable.Rows)
                                {
                                    cashReceipt = new CashReceipt
                                    {
                                        SubTotal = string.IsNullOrEmpty(row["SubTotal"].ToString()) ? 0 : decimal.Parse(row["SubTotal"].ToString()),
                                        TotalTT = string.IsNullOrEmpty(row["TotalTaxes"].ToString()) ? 0 : decimal.Parse(row["TotalTaxes"].ToString()),
                                        Total = string.IsNullOrEmpty(row["Total"].ToString()) ? 0 : decimal.Parse(row["Total"].ToString())
                                    };
                                    response.SubTotal = cashReceipt.SubTotal;
                                    response.TotalTT = cashReceipt.TotalTT;
                                    response.Total = cashReceipt.Total;
                                }
                            }

                            #endregion Get Details of Charges
                        }
                    }
                    else
                    {
                        response.chargesToInvoiceList = new List<ChargeCredit>();
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "GetGlobalCashReceipts", ex.Message + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Gets the global fiscal record.
        /// </summary>
        /// <returns></returns>
        public CashReceipt GetGlobalFiscalRecord()
        {
            CashReceipt response = new CashReceipt();
            try
            {
                CatalogDA catalogDA = new CatalogDA();
                ReceiverDA receiverDA = new ReceiverDA();
                FiscalRecordDA fiscalRecordDA = new FiscalRecordDA();
                response.CFDIList = new List<CFDIUsageCatalog>();
                response.ReceiverPaymentMethodList = new List<FiscalRecordCatalog>();
                response.PaymentTypeList = new List<FiscalRecordCatalog>();

                foreach (CFDIUsageCatalog cfdi in catalogDA.GetCFDIUsage())
                    response.CFDIList.Add(cfdi);

                List<FiscalRecordCatalog> CatalogListPaymentMethod = catalogDA.GetFiscalRecordCatalog(Catalog.PaymentMethod);
                response.ReceiverPaymentMethodDefault = CatalogListPaymentMethod.Find(x => x.Code == Constants.PaymentMethod).Description;
                List<FiscalRecordCatalog> CatalogListPaymentType = catalogDA.GetFiscalRecordCatalog(Catalog.PaymentType);
                foreach (FiscalRecordCatalog paymentType in from elements in CatalogListPaymentType where elements.Code != "99" select elements)
                    response.PaymentTypeList.Add(paymentType);

                response.FrequencyList = catalogDA.GetFiscalRecordCatalog(Catalog.Frequency);
                response.ReceiverList = receiverDA.GetTaxPayers(Constants.GlobalReceiverTaxPayerId);

                return response;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "GetGlobalFiscalRecord", ex.Message + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Gets the global cash receipts of the previously canceled global invoice with reason 04.
        /// </summary>
        /// <param name="invoiceHeaderId">The invoice header identifier of the canceled invoice with reason 04.</param>
        /// <returns></returns>
        public CashReceipt GetGlobalInvoiceDetail(int invoiceHeaderId)
        {
            DataSet globalCashReceiptDataset = new DataSet();
            CashReceipt response = null;
            try
            {
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("WebAdmin.spSelGlobalInvoiceDetailById");
                    database.AddInParameter(command, "@InvoiceHeaderId", DbType.String, invoiceHeaderId);

                    database.LoadDataSet(command, globalCashReceiptDataset, "GlobalInvoiceDetailById");

                    DataTable groupedDetailsTable = globalCashReceiptDataset.Tables[0];
                    DataTable detailsTable = globalCashReceiptDataset.Tables[1];
                    DataTable totalsTable = globalCashReceiptDataset.Tables[3];

                    if (globalCashReceiptDataset != null)
                    {
                        if (globalCashReceiptDataset.Tables.Count > 0)
                        {
                            if (groupedDetailsTable.Rows.Count > 0)
                            {
                                response = new CashReceipt();

                                #region Get Details of Charges

                                response.chargesToInvoiceList = new List<ChargeCredit>();
                                List<ChargeCredit> chargeCreditList = new List<ChargeCredit>();
                                bool isATaxCode = false;

                                if (detailsTable.Rows.Count > 0)
                                {
                                    List<ChargeCredit> chargeCreditIsTax = new List<ChargeCredit>();

                                    ChargeCredit chargeIsATax;
                                    foreach (DataRow dr in detailsTable.Rows)
                                    {
                                        chargeIsATax = new ChargeCredit
                                        {
                                            IsATax = Convert.ToBoolean(dr["IsATax"]),
                                            TaxCode = string.IsNullOrEmpty(dr["TaxCode"].ToString()) ? string.Empty : dr["TaxCode"].ToString()
                                        };

                                        if (chargeIsATax.IsATax.Equals(true) && chargeIsATax.TaxCode.Equals(string.Empty))
                                            isATaxCode = true;

                                        chargeCreditIsTax.Add(chargeIsATax);
                                    }
                                }

                                ChargeCredit chargeCreditDetai;
                                foreach (DataRow row in groupedDetailsTable.Rows)
                                {
                                    chargeCreditDetai = new ChargeCredit
                                    {
                                        Quantity = int.Parse(row["Quantity"].ToString()),
                                        ReceiptNumber = int.Parse(row["ReceiptNumber"].ToString()),
                                        ProductServiceKey = string.IsNullOrEmpty(row["ProductServiceKey"].ToString()) ? string.Empty : row["ProductServiceKey"].ToString(),
                                        UnitKey = string.IsNullOrEmpty(row["UnityKey"].ToString()) ? string.Empty : row["UnityKey"].ToString(),
                                        ChargeCreditDesc = row["ChargeCreditDesc"].ToString(),
                                        UnitAmount = decimal.Parse(row["UnitAmount"].ToString()),
                                        TotalTaxes = string.IsNullOrEmpty(row["TaxAmount"].ToString()) ? 0 : decimal.Parse(row["TaxAmount"].ToString()),
                                        IsATax = isATaxCode,
                                        SubjectToTax = row["SubjectToTax"].ToString()
                                    };
                                    chargeCreditList.Add(chargeCreditDetai);
                                }
                                response.chargesToInvoiceList = chargeCreditList.OrderBy(m => m.ReceiptNumber).ToList();

                                if (totalsTable.Rows.Count > 0)
                                {
                                    CashReceipt cashReceipt;
                                    foreach (DataRow row in totalsTable.Rows)
                                    {
                                        cashReceipt = new CashReceipt
                                        {
                                            SubTotal = string.IsNullOrEmpty(row["SubTotal"].ToString()) ? 0 : decimal.Parse(row["SubTotal"].ToString()),
                                            TotalTT = string.IsNullOrEmpty(row["TotalTaxes"].ToString()) ? 0 : decimal.Parse(row["TotalTaxes"].ToString()),
                                            Total = string.IsNullOrEmpty(row["Total"].ToString()) ? 0 : decimal.Parse(row["Total"].ToString())
                                        };
                                        response.SubTotal = cashReceipt.SubTotal;
                                        response.TotalTT = cashReceipt.TotalTT;
                                        response.Total = cashReceipt.Total;
                                    }
                                }

                                #endregion Get Details of Charges
                            }
                        }
                        else
                        {
                            response.chargesToInvoiceList = new List<ChargeCredit>();
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "GetGlobalInvoiceDetail", ex.Message + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// View the global fiscal records.
        /// </summary>
        /// <param name="globalInvoiceFiltersForCreation">The global invoice filters for creation.</param>
        /// <returns></returns>
        public List<FiscalRecordCatalog> GetGlobalPaymentType(GlobalInvoiceFiltersForCreation globalInvoiceFiltersForCreation)
        {
            DataSet globalPaymentTypeDataset = new DataSet();
            List<FiscalRecordCatalog> lstPaymentType = new List<FiscalRecordCatalog>();
            try
            {
                SettingsDA setting = new SettingsDA();
                Settings settings = new Settings
                {
                    Area = "SYSADMIN",
                    Section = "CURRENCY_FORMAT",
                    Label = "REPORT_FORMAT"
                };
                string currencyFormat = setting.GetSetting(settings);

                string startDate = globalInvoiceFiltersForCreation.StartDate;
                string endDate = globalInvoiceFiltersForCreation.EndDate;

                string StartDateSearch = startDate.Substring(6, 4) + "-" + startDate.Substring(3, 2) + "-" + startDate.Substring(0, 2);
                string EndDateSearch = endDate.Substring(6, 4) + "-" + endDate.Substring(3, 2) + "-" + endDate.Substring(0, 2);

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelGlobalInvoicePaymentMethod");

                    database.AddInParameter(command, "@StartDate", DbType.String, StartDateSearch);
                    database.AddInParameter(command, "@EndDate", DbType.String, EndDateSearch);
                    database.AddInParameter(command, "@InvExpeditionId", DbType.Int32, globalInvoiceFiltersForCreation.InvExpeditionId);
                    database.AddInParameter(command, "@AcademicYear", DbType.String, globalInvoiceFiltersForCreation.AcademicYear);
                    database.AddInParameter(command, "@AcademicTerm", DbType.String, globalInvoiceFiltersForCreation.AcademicTerm);
                    database.AddInParameter(command, "@AcademicSession", DbType.String, globalInvoiceFiltersForCreation.AcademicSession);

                    if (globalInvoiceFiltersForCreation.PaymentTypeId > 0)
                        database.AddInParameter(command, "@PaymentTypeId", DbType.Int32, globalInvoiceFiltersForCreation.PaymentTypeId);

                    if (globalInvoiceFiltersForCreation.ReceiptNumbers != null)
                    {
                        string receiptNumbersString = string.Empty;
                        for (int i = 0; i < globalInvoiceFiltersForCreation.ReceiptNumbers.Count; i++)
                        {
                            if (i > 0)
                                receiptNumbersString += ",";

                            int receiptNumber = globalInvoiceFiltersForCreation.ReceiptNumbers[i];
                            receiptNumbersString += receiptNumber.ToString();
                        }

                        database.AddInParameter(command, "@ReceiptNumbers", DbType.String, receiptNumbersString);
                    }

                    database.LoadDataSet(command, globalPaymentTypeDataset, "GlobalInvoicePaymentMethod");
                }
                if (globalPaymentTypeDataset?.Tables.Count > 0 && globalPaymentTypeDataset.Tables[0].Rows.Count > 0)
                {
                    FiscalRecordCatalog paymentType;
                    foreach (DataRow row in globalPaymentTypeDataset.Tables[0].Rows)
                    {
                        paymentType = new FiscalRecordCatalog();
                        paymentType.Code = row["PaymentMethod"].ToString();
                        paymentType.Description = row["PaymentMethod"].ToString() + " - " + row["PaymentMethodDesc"].ToString() + " - " + ((decimal)(row["Amount"])).ToString(currencyFormat, CultureInfo.InvariantCulture);
                        lstPaymentType.Add(paymentType);
                    }
                }
                return lstPaymentType;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "GetGlobalPaymentType", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Saves the tax payers.
        /// </summary>
        /// <param name="receiptPaymentMethodMapping">The receipt payment method mapping.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public int SaveReceiptPaymentMapping(ReceiptPaymentMethodMapping receiptPaymentMethodMapping, string userName)
        {
            try
            {
                Database database = _factory.CreateDefault();
                int returnStatus;
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spUpdReceiptPaymentMethodMapping");

                    database.AddInParameter(command, "@ChargeCreditCodeId", DbType.Int32, receiptPaymentMethodMapping.ChargeCreditCodeId);
                    database.AddInParameter(command, "@PaymentTypeKey", DbType.String, receiptPaymentMethodMapping.PaymentMethodCode);
                    database.AddInParameter(command, "@Terminal", DbType.String, "0001");
                    database.AddInParameter(command, "@Opid", DbType.String, userName);
                    object internalValue = new object();
                    database.AddParameter(command, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, internalValue);

                    database.ExecuteNonQuery(command);
                    returnStatus = (int)database.GetParameterValue(command, "@ReturnStatus");
                }
                return returnStatus;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "SaveReceiptPaymentMapping", ex.Message + ex.StackTrace);
                throw;
            }
        }
    }
}