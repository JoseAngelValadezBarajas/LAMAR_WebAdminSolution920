// --------------------------------------------------------------------
// <copyright file="IssuerDA.cs" company="Ellucian">
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
using System.Linq;

namespace PowerCampus.DataAccess
{
    /// <summary>
    /// Issuer DataAccess class
    /// </summary>
    public class IssuerDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="IssuerDA"/> class.
        /// </summary>
        public IssuerDA()
        {
            _factory = new DatabaseProviderFactory();
        }

        /// <summary>
        /// Creates the issuer set up.
        /// </summary>
        /// <param name="issuerDefault">The issuer default.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public int CreateIssuerSetUp(IssuerDefault issuerDefault, string userName)
        {
            try
            {
                // _logService.Debug("Method starts - CreateIssuerSetUp");
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command;
                    connection.Open();
                    if (issuerDefault.IssDefaultId.Equals(0))
                    {
                        command = database.GetStoredProcCommand("spInsFiscalRecordIssuerDefault");
                        command.Connection = connection;

                        database.AddInParameter(command, "@UserName", DbType.String, userName);
                        database.AddInParameter(command, "@InvoiceOrganizationId", DbType.Int32, issuerDefault.IssInvoiceOrganizationId);
                        database.AddInParameter(command, "@InvoiceExpeditionId", DbType.Int32, issuerDefault.IssInvoiceExpeditionId);
                        database.AddInParameter(command, "@InvoiceReceiptId", DbType.Int32, issuerDefault.IssInvoiceReceiptId);
                        database.AddInParameter(command, "@PaymentCondition", DbType.String, issuerDefault.IssPaymentConditions);
                        database.AddInParameter(command, "@CreditNoteReceiptId", DbType.Int32, issuerDefault.IssCreditNoteReceiptId);
                        database.AddInParameter(command, "@ChargeCreditCodeId", DbType.Int32, issuerDefault.IssChargeCreditCodeId);
                        database.AddInParameter(command, "@ChargeCreditCodeTaxId", DbType.Int32, issuerDefault.IssChargeCreditCodeTaxId);
                        database.AddOutParameter(command, "@FiscalRecordIssuerDefaultId", DbType.Int32, 0);
                        database.ExecuteNonQuery(command);

                        if ((int)command.Parameters["@FiscalRecordIssuerDefaultId"].Value > 0)
                        {
                            // _logService.Debug("Method ends - CreateIssuerSetUp");
                            return 1;
                        }
                        else
                        {
                            // _logService.Debug("Method ends - CreateIssuerSetUp");
                            return -1;
                        }
                    }
                    else
                    {
                        int returnStatus;
                        command = database.GetStoredProcCommand("spUpdFiscalRecordIssuerDefault");
                        command.Connection = connection;

                        database.AddInParameter(command, "@FiscalRecordIssuerDefaultId", DbType.Int32, issuerDefault.IssDefaultId);
                        database.AddInParameter(command, "@InvoiceOrganizationId", DbType.Int32, issuerDefault.IssInvoiceOrganizationId);
                        database.AddInParameter(command, "@InvoiceExpeditionId", DbType.Int32, issuerDefault.IssInvoiceExpeditionId);
                        database.AddInParameter(command, "@InvoiceReceiptId", DbType.Int32, issuerDefault.IssInvoiceReceiptId);
                        database.AddInParameter(command, "@PaymentCondition", DbType.String, issuerDefault.IssPaymentConditions);
                        database.AddInParameter(command, "@CreditNoteReceiptId", DbType.Int32, issuerDefault.IssCreditNoteReceiptId);
                        database.AddInParameter(command, "@ChargeCreditCodeId", DbType.Int32, issuerDefault.IssChargeCreditCodeId);
                        database.AddInParameter(command, "@ChargeCreditCodeTaxId", DbType.Int32, issuerDefault.IssChargeCreditCodeTaxId);
                        object internalValue = new object();
                        database.AddParameter(command, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, internalValue);
                        database.ExecuteNonQuery(command);
                        returnStatus = (int)database.GetParameterValue(command, "@ReturnStatus");
                        if (returnStatus > 0)
                        {
                            // _logService.Debug("Method ends - CreateIssuerSetUp");
                            return 1;
                        }
                        else
                        {
                            // _logService.Debug("Method ends - CreateIssuerSetUp");
                            return -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuerDA - CreateIssuerSetUp", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Delete issuer Address setup
        /// </summary>
        /// <param name="id">Expedition Id</param>
        public int DeleteIssuingAddress(int id)
        {
            try
            {
                // _logService.Debug("Method starts - DeleteIssuingAddress");
                int returnStatus;
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spDelInvoiceExpedition");

                    database.AddInParameter(command, "@InvoiceExpeditionId", DbType.Int32, id);
                    object internalValue = new object();
                    database.AddParameter(command, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, internalValue);
                    database.ExecuteNonQuery(command);
                    returnStatus = (int)database.GetParameterValue(command, "@ReturnStatus");
                    // _logService.Debug("Method ends - DeleteIssuingAddress");
                    return returnStatus;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuerDA - DeleteIssuingAddress", e.Message);
                return -1;
            };
        }

        /// <summary>
        /// Gets all issuers.
        /// </summary>
        /// <param name="prefer">The prefer.</param>
        /// <returns></returns>
        public List<Issuer> GetAllIssuers(string prefer)
        {
            try
            {
                // _logService.Debug("Method starts - GetAllIssuers");
                DataSet allIssuersDataSet = new DataSet();

                List<Issuer> lstReceiverModel = new List<Issuer>();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelInvoiceOrganization");

                    connection.Open();
                    command.Connection = connection;

                    database.AddInParameter(command, "@TaxPayerId", DbType.String, prefer);
                    database.LoadDataSet(command, allIssuersDataSet, "IssuersDataSet");

                    if (allIssuersDataSet.Tables[0].Rows.Count > 0)
                    {
                        List<Issuer> IssuerModelList = allIssuersDataSet.Tables[0].AsEnumerable().Select(m => new Issuer()
                        {
                            IssInvoiceOrganizationId = m.Field<int?>("InvoiceOrganizationId"),
                            IssTaxpayerId = m.Field<string>("TaxpayerId"),
                            IssCorporateName = m.Field<string>("CorporateName")
                        }).ToList();
                        // _logService.Debug("Method ends - GetAllIssuers");
                        return lstReceiverModel = IssuerModelList;
                    }
                    else
                    {
                        // _logService.Debug("Method ends - GetAllIssuers");
                        return lstReceiverModel = new List<Issuer>();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuerDA - GetAllIssuers", ex.Message);
                throw;
            };
        }

        /// <summary>
        /// Gets all issuers.
        /// </summary>
        /// <param name="prefer">The prefer.</param>
        /// <returns></returns>
        public List<InvoiceOrganization> GetIssuers(string prefer)
        {
            try
            {
                // _logService.Debug("Method starts - GetIssuers");
                DataSet allIssuersDataSet = new DataSet();
                List<InvoiceOrganization> lstReceiverModel = new List<InvoiceOrganization>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelInvoiceOrganization");

                    database.AddInParameter(command, "@TaxPayerId", DbType.String, prefer);
                    database.LoadDataSet(command, allIssuersDataSet, "InvoiceOrganization");

                    if (allIssuersDataSet.Tables[0].Rows.Count > 0)
                    {
                        List<InvoiceOrganization> IssuerModelList = allIssuersDataSet.Tables[0].AsEnumerable().Select(m => new InvoiceOrganization()
                        {
                            InvoiceOrganizationId = m.Field<int?>("InvoiceOrganizationId"),
                            TaxpayerId = m.Field<string>("TaxpayerId"),
                            CorporateName = m.Field<string>("CorporateName")
                        }).ToList();
                        // _logService.Debug("Debug - Exit - PowerCampus.DataAccess - IssuerDA - GetIssuers");
                        return lstReceiverModel = IssuerModelList;
                    }
                    else
                    {
                        // _logService.Debug("Method ends - GetIssuers");
                        return lstReceiverModel = new List<InvoiceOrganization>();
                    }
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuerDA - GetIssuers", e.Message);
                throw;
            };
        }

        /// <summary>
        /// Gets the issuers by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public InvoiceOrganization GetIssuersById(int id)
        {
            try
            {
                // _logService.Debug("Method starts - GetIssuersById");
                DataSet allIssuersDataSet = new DataSet();
                List<SqlParameter> param = new List<SqlParameter>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelInvoiceOrganizationById");

                    connection.Open();
                    command.Connection = connection;

                    database.AddInParameter(command, "@InvoiceOrganizationId", DbType.Int32, id);
                    database.LoadDataSet(command, allIssuersDataSet, "InvoiceOrganization");

                    InvoiceOrganization invoiceOrganization = new InvoiceOrganization();
                    if (allIssuersDataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in allIssuersDataSet.Tables[0].Rows)
                        {
                            invoiceOrganization.InvoiceOrganizationId = (int)row["InvoiceOrganizationId"];
                            invoiceOrganization.TaxpayerId = row["TaxpayerId"].ToString();
                            invoiceOrganization.TaxRegimenCode = row["TaxRegimenCode"].ToString();
                            invoiceOrganization.TaxRegimenDesc = row["TaxRegimenDesc"].ToString();
                            invoiceOrganization.CorporateName = row["CorporateName"].ToString();
                            invoiceOrganization.TaxRegimenId = (row["TaxRegimenId"] != DBNull.Value) ? (int?)row["TaxRegimenId"] : null;
                            invoiceOrganization.InvoiceOrgTaxRegimenId = (row["InvoiceOrgTaxRegimenId"] != DBNull.Value) ? (int?)row["InvoiceOrgTaxRegimenId"] : null;
                        }
                        // _logService.Debug("Method ends - GetIssuersById");
                        return invoiceOrganization;
                    }
                    else
                    {
                        // _logService.Debug("Method ends - GetIssuersById");
                        return invoiceOrganization;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuerDA - GetIssuersById", ex.Message);
                throw;
            };
        }

        /// <summary>
        /// Gets the issuer serial number.
        /// </summary>
        /// <param name="InvoiceTaxpayerId">The invoice taxpayer identifier.</param>
        /// <param name="InvoiceExpeditionId"></param>
        /// <returns></returns>
        public Issuer GetIssuerSerialNumber(int InvoiceTaxpayerId, int? InvoiceExpeditionId)
        {
            DataSet IssuerDataSet = new DataSet();
            Issuer response = new Issuer();
            try
            {
                // _logService.Debug("Method starts - GetIssuerSerialNumber");
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelInvoiceReceipt");

                    connection.Open();
                    command.Connection = connection;

                    database.AddInParameter(command, "@InvoiceOrganizationId", DbType.Int32, InvoiceTaxpayerId);
                    database.AddInParameter(command, "@InvoiceExpeditionId", DbType.Int32, InvoiceExpeditionId);
                    database.LoadDataSet(command, IssuerDataSet, "IssuerDataSet");

                    if (IssuerDataSet.Tables[0].Rows.Count > 0)
                    {
                        response.IssSerial = new List<IssuerSerial>();
                        List<IssuerSerial> IssuerModelList = IssuerDataSet.Tables[0].AsEnumerable().Select(m => new IssuerSerial()
                        {
                            IssInvoiceReceipt = m.Field<int>("InvoiceReceiptId"),
                            IssSerialNumber = m.Field<string>("SerialNumber").ToString(),
                            IssLastFolio = m.Field<int?>("LastFolioAssigned")
                        }).ToList();
                        response.IssSerial = IssuerModelList;
                    }
                    // _logService.Debug("Method ends - GetIssuerSerialNumber");
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuerDA - GetIssuerSerialNumber", ex.Message);
                throw;
            };
        }

        /// <summary>
        /// Gets the issuer.
        /// </summary>
        /// <param name="InvoiceTaxpayerId">The tax payer identifier.</param>
        /// <returns></returns>
        public Issuer GetIssuerTaxRegimen(int InvoiceTaxpayerId)
        {
            DataSet IssuerDataSet = new DataSet();
            Issuer response = new Issuer();

            try
            {
                // _logService.Debug("Method starts - GetIssuerTaxRegimen");
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelInvoiceTaxRegimen");

                    connection.Open();
                    command.Connection = connection;

                    database.AddInParameter(command, "@InvoiceOrgId", DbType.Int32, InvoiceTaxpayerId);
                    database.LoadDataSet(command, IssuerDataSet, "IssuerDataSet");

                    if (IssuerDataSet.Tables[0].Rows.Count > 0)
                    {
                        response.IssTaxRegimen = new List<TaxRegimen>();

                        List<TaxRegimen> IssuerTaxRegimenList = IssuerDataSet.Tables[0].AsEnumerable().Select(m => new TaxRegimen()
                        {
                            IssTaxRegInvoiceOrganizationId = m.Field<int?>("InvoiceOrganizationId"),
                            IssCodeValue = m.Field<string>("CODE_VALUE"),
                            IssLongDesc = m.Field<string>("LONG_DESC")
                        }).ToList();

                        response.IssTaxRegimen = IssuerTaxRegimenList;
                    }

                    if (IssuerDataSet.Tables[1].Rows.Count > 0)
                    {
                        response.IssIssuingAdd = new List<IssuingAddress>();

                        List<IssuingAddress> IssuerModelList = IssuerDataSet.Tables[1].AsEnumerable().Select(m => new IssuingAddress()
                        {
                            IssInvoiceOrganizationId = m.Field<int>("InvoiceOrganizationId"),
                            IssInoviceExpeditionId = m.Field<int>("InvoiceExpeditionId"),
                            IssByExpedition = m.Field<int>("ByExpedition"),
                            IssIssuingAddress = m.Field<string>("IssuingAddress"),
                            IssPlaceIssue = m.Field<string>("PlaceOfIssue")
                        }).ToList();

                        response.IssIssuingAdd = IssuerModelList;
                    }
                    // _logService.Debug("Method ends - GetIssuerTaxRegimen");
                    return response;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuerDA - GetIssuerTaxRegimen", ex.Message);
                throw;
            };
        }

        /// <summary>
        /// Get Issuing Address
        /// </summary>
        /// <param name="id"> TaxPayer Id</param>
        /// <param name="invoiceExpeditionId">invoiceExpeditionId is optional</param>
        /// <returns></returns>
        public List<InvoiceExpedition> GetIssuingAddress(int id, int? invoiceExpeditionId)
        {
            try
            {
                // _logService.Debug("Method starts - GetIssuingAddress");
                DataSet invoiceExpeditionDataSet = new DataSet();
                List<InvoiceExpedition> InvoiceExpeditions = new List<InvoiceExpedition>();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelInvoiceExpedition");

                    connection.Open();
                    command.Connection = connection;

                    database.AddInParameter(command, "@InvoiceOrganizationId", DbType.Int32, id);
                    if (invoiceExpeditionId > 0)
                        database.AddInParameter(command, "@InvoiceExpeditionId", DbType.Int32, invoiceExpeditionId);

                    database.LoadDataSet(command, invoiceExpeditionDataSet, "InvoiceExpedition");

                    if (invoiceExpeditionDataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in invoiceExpeditionDataSet.Tables[0].Rows)
                        {
                            InvoiceExpedition InvoiceExpedition = new InvoiceExpedition();
                            InvoiceExpedition.InvoiceOrganizationId = (int?)id;
                            InvoiceExpedition.InvoiceExpeditionId = (row["InvoiceExpeditionId"] != DBNull.Value) ? (int?)row["InvoiceExpeditionId"] : null;
                            InvoiceExpedition.Description = row["Description"].ToString();
                            InvoiceExpedition.PostalCode = row["PostalCode"].ToString();
                            InvoiceExpedition.State = row["State"].ToString();
                            InvoiceExpedition.CanDelete = int.Parse(row["CanDelete"].ToString());
                            InvoiceExpeditions.Add(InvoiceExpedition);
                        }
                        // _logService.Debug("Method ends - GetIssuingAddress");
                        return InvoiceExpeditions;
                    }
                    else
                    {
                        // _logService.Debug("Method ends - GetIssuingAddress");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuerDA - GetIssuingAddress", ex.Message);
                throw;
            };
        }

        /// <summary>
        /// Get list for doucument setups
        /// </summary>
        /// <param name="id"></param>
        /// <param name="invoiceReceiptId"></param>
        /// <returns></returns>
        public List<InvoiceReceipt> GetIssuingReceipt(int id, int? invoiceReceiptId)
        {
            try
            {
                // _logService.Debug("Method starts - GetIssuingReceipt");
                DataSet invoiceReceiptDataSet = new DataSet();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    List<InvoiceReceipt> InvoiceReceipts = new List<InvoiceReceipt>();
                    DateTime dateTime = new DateTime();

                    DbCommand command = database.GetStoredProcCommand("spSelInvoiceReceiptById");
                    database.AddInParameter(command, "@InvoiceOrganizationId", DbType.Int32, id);

                    if (invoiceReceiptId > 0)
                        database.AddInParameter(command, "@InvoiceReceiptId", DbType.Int32, invoiceReceiptId);

                    database.LoadDataSet(command, invoiceReceiptDataSet, "InvoiceReceiptById");

                    if (invoiceReceiptDataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in invoiceReceiptDataSet.Tables[0].Rows)
                        {
                            InvoiceReceipt InvoiceReceipt = new InvoiceReceipt();
                            InvoiceReceipt.InvoiceOrganizationId = (row["InvoiceOrganizationId"] != DBNull.Value) ? (int?)row["InvoiceOrganizationId"] : null;
                            InvoiceReceipt.InvoiceExpeditionId = (row["InvoiceExpeditionId"] != DBNull.Value) ? (int?)row["InvoiceExpeditionId"] : null;
                            InvoiceReceipt.InvoiceReceiptId = (int?)row["InvoiceReceiptId"];
                            InvoiceReceipt.SerialNumber = row["SerialNumber"].ToString();
                            InvoiceReceipt.Folio = (row["Folio"] != DBNull.Value) ? (int?)row["Folio"] : null;
                            InvoiceReceipt.LastFolioAssigned = (row["LastFolioAssigned"] != DBNull.Value) ? (int?)row["LastFolioAssigned"] : null;
                            if (DateTime.TryParse(row["StartDate"].ToString(), out dateTime)) InvoiceReceipt.StartDate = dateTime;
                            if (DateTime.TryParse(row["EndDate"].ToString(), out dateTime)) InvoiceReceipt.EndDate = dateTime;
                            InvoiceReceipt.Version = row["Version"].ToString();
                            InvoiceReceipts.Add(InvoiceReceipt);
                        }
                        // _logService.Debug("Method ends - GetIssuingReceipt");
                        return InvoiceReceipts;
                    }
                    else
                    {
                        // _logService.Debug("Method ends - GetIssuingReceipt");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuerDA - GetIssuingReceipt", ex.Message);
                throw;
            };
        }

        /// <summary>
        /// Create New Issuing Address For Fiscal Information.
        /// </summary>
        /// <param name="issuerAddress"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int SaveIssuingAddress(InvoiceExpedition issuerAddress, string userName)
        {
            try
            {
                // _logService.Debug("Method starts - SaveIssuingAddress");
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command;
                    connection.Open();
                    if (issuerAddress.InvoiceExpeditionId == null)
                    {
                        command = database.GetStoredProcCommand("spInsInvoiceExpedition");
                        command.Connection = connection;

                        database.AddInParameter(command, "@Description", DbType.String, (string.IsNullOrEmpty(issuerAddress.Description) == true) ? "" : issuerAddress.Description);
                        database.AddInParameter(command, "@PostalCode", DbType.String, (string.IsNullOrEmpty(issuerAddress.PostalCode) == true) ? "" : issuerAddress.PostalCode);
                        database.AddInParameter(command, "@Opid", DbType.String, userName);
                        database.AddOutParameter(command, "@InvoiceExpeditionId", DbType.Int32, 0);
                        database.AddInParameter(command, "@InvoiceOrganizationId", DbType.Int32, issuerAddress.InvoiceOrganizationId);

                        database.ExecuteNonQuery(command);

                        issuerAddress.InvoiceExpeditionId = (int)command.Parameters["@InvoiceExpeditionId"].Value;

                        // _logService.Debug("Method ends - GetIssuers");
                        return issuerAddress.InvoiceExpeditionId ?? -1;
                    }
                    else
                    {
                        command = database.GetStoredProcCommand("spUpdInvoiceExpedition");
                        command.Connection = connection;

                        database.AddInParameter(command, "@Description", DbType.String, (string.IsNullOrEmpty(issuerAddress.Description) == true) ? "" : issuerAddress.Description);
                        database.AddInParameter(command, "@PostalCode", DbType.String, (string.IsNullOrEmpty(issuerAddress.PostalCode) == true) ? "" : issuerAddress.PostalCode);
                        database.AddInParameter(command, "@Opid", DbType.String, userName);
                        database.AddInParameter(command, "@InvoiceExpeditionId", DbType.Int32, issuerAddress.InvoiceExpeditionId ?? 0);

                        database.ExecuteNonQuery(command);

                        // _logService.Debug("Method ends - SaveIssuingAddress");
                        return issuerAddress.InvoiceExpeditionId ?? -1;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuerDA - SaveIssuingAddress", ex.Message);
                throw;
            };
        }

        /// <summary>
        /// Create New Issuing Receipt or update For Fiscal Information.
        /// </summary>
        /// <param name="invoiceReceipt"></param>
        /// <param name="userName"></param>
        /// <returns>int</returns>
        public int SaveIssuingReceipt(InvoiceReceipt invoiceReceipt, string userName)
        {
            try
            {
                // _logService.Debug("Method starts - SaveIssuingReceipt");
                int returnStatus = 0;
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    if (invoiceReceipt.InvoiceReceiptId == null)
                    {
                        DbCommand command = database.GetStoredProcCommand("spInsInvoiceReceipt");

                        if (invoiceReceipt.InvoiceExpeditionId == null)
                        {
                            database.AddInParameter(command, "@InvoiceOrganizationId", DbType.Int32, invoiceReceipt.InvoiceOrganizationId);
                            database.AddInParameter(command, "@InvoiceExpeditionId", DbType.Int32, null);
                        }
                        else
                        {
                            database.AddInParameter(command, "@InvoiceOrganizationId", DbType.Int32, null);
                            database.AddInParameter(command, "@InvoiceExpeditionId", DbType.Int32, invoiceReceipt.InvoiceExpeditionId);
                        }

                        database.AddInParameter(command, "@SerialNumber", DbType.String, (string.IsNullOrEmpty(invoiceReceipt.SerialNumber) == true) ? "" : invoiceReceipt.SerialNumber);
                        database.AddInParameter(command, "@Folio", DbType.Int32, invoiceReceipt.Folio);
                        database.AddInParameter(command, "@Opid", DbType.String, userName);
                        database.AddOutParameter(command, "@InvoiceReceiptId", DbType.Int32, 0);
                        object internalValue = new object();
                        database.AddParameter(command, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, internalValue);
                        database.ExecuteNonQuery(command);
                        returnStatus = (int)database.GetParameterValue(command, "@ReturnStatus");
                    }
                }
                // _logService.Debug("Method ends - SaveIssuingReceipt");
                return returnStatus;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuerDA - SaveIssuingReceipt", ex.Message);
                throw;
            };
        }

        /// <summary>
        /// Create New Issuer Fiscal Information.
        /// </summary>
        /// <param name="issuer"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int SaveTaxPayers(InvoiceOrganization issuer, string userName)
        {
            try
            {
                // _logService.Debug("Method starts - SaveTaxPayers");
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command;
                    connection.Open();
                    if (issuer.InvoiceOrganizationId == null)
                    {
                        command = database.GetStoredProcCommand("spInsInvoiceOrganization");
                        command.Connection = connection;

                        database.AddInParameter(command, "@CorporateName", DbType.String, (string.IsNullOrEmpty(issuer.CorporateName) == true) ? "" : issuer.CorporateName);
                        database.AddInParameter(command, "@TaxRegimenId", DbType.Int32, issuer.TaxRegimenId);
                        database.AddInParameter(command, "@Opid", DbType.String, userName);
                        database.AddInParameter(command, "@TaxPayerId", DbType.String, issuer.TaxpayerId);
                        database.AddOutParameter(command, "@InvoiceOrganizationId", DbType.Int32, 0);
                        database.AddOutParameter(command, "@InvoiceOrgTaxRegimenId", DbType.Int32, 0);

                        database.ExecuteNonQuery(command);

                        issuer.InvoiceOrganizationId = (int)command.Parameters["@InvoiceOrganizationId"].Value;
                        issuer.InvoiceOrgTaxRegimenId = (int)command.Parameters["@InvoiceOrgTaxRegimenId"].Value;
                        // _logService.Debug("Method ends - SaveTaxPayers");
                        return issuer.InvoiceOrganizationId ?? -1;
                    }
                    else
                    {
                        command = database.GetStoredProcCommand("spUpdInvoiceOrganization");
                        command.Connection = connection;

                        database.AddInParameter(command, "@CorporateName", DbType.String, (string.IsNullOrEmpty(issuer.CorporateName) == true) ? "" : issuer.CorporateName);
                        database.AddInParameter(command, "@TaxRegimenId", DbType.Int32, issuer.TaxRegimenId);
                        database.AddInParameter(command, "@Opid", DbType.String, userName);

                        database.AddInParameter(command, "@InvoiceOrganizationId", DbType.Int32, issuer.InvoiceOrganizationId);
                        database.ExecuteNonQuery(command);

                        // _logService.Debug("Method ends - SaveTaxPayers");
                        return issuer.InvoiceOrganizationId ?? -1;
                    }
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuerDA - SaveTaxPayers", e.Message);
                return -1;
            }
        }

        /// <summary>
        /// Selects the issuer set up data.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public IssuerDefault SelectIssuerSetUpData(string userName)
        {
            IssuerDefault issuerDefault = new IssuerDefault();
            DataSet IssuerDataSet = new DataSet();

            try
            {
                // _logService.Debug("Method starts - SelectIssuerSetUpData");
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelFiscalRecordIssuerDefaultByUser");

                    connection.Open();
                    command.Connection = connection;

                    database.AddInParameter(command, "@UserName", DbType.String, userName);
                    database.LoadDataSet(command, IssuerDataSet, "FiscalRecordIssuer");

                    if (IssuerDataSet.Tables[0].Rows.Count > 0)
                    {
                        List<IssuerDefault> IssuerDefaultList = IssuerDataSet.Tables[0].AsEnumerable().Select(m => new IssuerDefault()
                        {
                            IssDefaultId = m.Field<int>("FiscalRecordIssuerDefaultId"),
                            IssInvoiceOrganizationId = m.Field<int?>("InvoiceOrganizationId"),
                            IssInvoiceTaxpayerId = m.Field<string>("TaxpayerId"),
                            IssInvoiceExpeditionId = m.Field<int?>("InvoiceExpeditionId"),
                            IssIssuingAddress = m.Field<string>("IssuingAddress"),
                            IssSerialNumber = m.Field<string>("SerialNumber"),
                            IssPaymentConditions = m.Field<string>("PaymentCondition"),
                            IssCorporateName = m.Field<string>("CorporateName"),
                            IssSerialNumberCreditNote = m.Field<string>("CreditNoteSerialNumber"),
                            IssInvoiceReceiptId = m.Field<int?>("InvoiceReceiptId"),
                            IssCreditNoteReceiptId = m.Field<int?>("CreditNoteReceiptId"),
                            IssChargeCreditCodeId = m.Field<int?>("ChargeCreditCodeId"),
                            IssChargeCreditCodeTaxId = m.Field<int?>("ChargeCreditCodeTaxId")
                        }).ToList();

                        issuerDefault.IssDefaultId = IssuerDefaultList[0].IssDefaultId;
                        issuerDefault.IssInvoiceOrganizationId = IssuerDefaultList[0].IssInvoiceOrganizationId;
                        issuerDefault.IssInvoiceTaxpayerId = string.IsNullOrEmpty(IssuerDefaultList[0].IssInvoiceTaxpayerId) ? string.Empty : IssuerDefaultList[0].IssInvoiceTaxpayerId.Trim();
                        issuerDefault.IssSerialNumber = string.IsNullOrEmpty(IssuerDefaultList[0].IssSerialNumber) ? string.Empty : IssuerDefaultList[0].IssSerialNumber.Trim();
                        issuerDefault.IssInvoiceExpeditionId = IssuerDefaultList[0].IssInvoiceExpeditionId;
                        issuerDefault.IssIssuingAddress = string.IsNullOrEmpty(IssuerDefaultList[0].IssIssuingAddress) ? string.Empty : IssuerDefaultList[0].IssIssuingAddress.Trim();
                        issuerDefault.IssPaymentConditions = string.IsNullOrEmpty(IssuerDefaultList[0].IssPaymentConditions) ? string.Empty : IssuerDefaultList[0].IssPaymentConditions;
                        issuerDefault.IssCorporateName = IssuerDefaultList[0].IssCorporateName;
                        issuerDefault.IssSerialNumberCreditNote = IssuerDefaultList[0].IssSerialNumberCreditNote;
                        issuerDefault.IssInvoiceReceiptId = IssuerDefaultList[0].IssInvoiceReceiptId;
                        issuerDefault.IssCreditNoteReceiptId = IssuerDefaultList[0].IssCreditNoteReceiptId;
                        issuerDefault.IssChargeCreditCodeId = IssuerDefaultList[0].IssChargeCreditCodeId;
                        issuerDefault.IssChargeCreditCodeTaxId = IssuerDefaultList[0].IssChargeCreditCodeTaxId;

                        // _logService.Debug("Method ends - SelectIssuerSetUpData");
                        return issuerDefault;
                    }
                    else
                    {
                        // _logService.Debug("Method ends - SelectIssuerSetUpData");
                        return issuerDefault;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuerDA - SelectIssuerSetUpData", ex.Message);
                throw;
            }
        }
    }
}