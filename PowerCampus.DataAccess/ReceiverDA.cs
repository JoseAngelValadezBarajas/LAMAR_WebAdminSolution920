// --------------------------------------------------------------------
// <copyright file="ReceiverDA.cs" company="Ellucian">
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

namespace PowerCampus.DataAccess
{
    /// <summary>
    /// Reciever DataAccess class
    /// </summary>
    public class ReceiverDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FiscalRecordDA"/> class.
        /// </summary>
        public ReceiverDA()
        {
            _factory = new DatabaseProviderFactory();
        }

        /// <summary>
        /// Deletes the tax payer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public bool DeleteTaxpayer(int id)
        {
            try
            {
                int returnStatus;
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("WebAdmin.spDelInvoiceTaxpayer");

                    database.AddInParameter(command, "@InvoiceTaxpayerId", DbType.Int32, id);
                    object internalValue = new object();
                    database.AddParameter(command, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, internalValue);
                    database.ExecuteNonQuery(command);
                    returnStatus = (int)database.GetParameterValue(command, "@ReturnStatus");
                    return returnStatus > 0;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiverDA - DeleteTaxPayer", e.Message);
                return false;
            };
        }

        /// <summary>
        /// Gets the tax payer by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="foreignId">The foreign identifier.</param>
        /// <returns></returns>
        public Receiver GetTaxPayerbyId(int id, int? foreignId)
        {
            try
            {
                // _logService.Debug("Method starts - GetTaxPayerbyId");
                DataSet invoiceTaxpayerDataSet = new DataSet();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelInvoiceTaxpayerbyId");

                    database.AddInParameter(command, "@InvoiceTaxpayerId", DbType.Int32, id);
                    database.AddInParameter(command, "@InvoiceForeignTaxpayerId", DbType.Int32, foreignId);
                    database.LoadDataSet(command, invoiceTaxpayerDataSet, "InvoiceTaxpayerDataSet");
                }
                if (invoiceTaxpayerDataSet.Tables[0].Rows.Count > 0)
                {
                    Receiver receiver = new Receiver();
                    foreach (DataRow row in invoiceTaxpayerDataSet.Tables[0].Rows)
                    {
                        receiver.InvoiceTaxpayerId = (int?)row["InvoiceTaxpayerId"];
                        receiver.InvoiceForeignTaxpayerId = (row["InvoiceForeignTaxpayerId"] != DBNull.Value) ? (int?)row["InvoiceForeignTaxpayerId"] : null;
                        receiver.TaxPayerId = row["TaxPayerId"].ToString();
                        receiver.CorporateName = row["CorporateName"].ToString();
                        receiver.PostalCode = string.IsNullOrEmpty(row["PostalCode"].ToString()) ? string.Empty : row["PostalCode"].ToString();
                        receiver.TaxRegimenId = (row["TaxRegimenId"] != DBNull.Value) ? (int?)row["TaxRegimenID"] : null;
                        receiver.FiscalResidency = row["FiscalResidency"].ToString();
                        receiver.FiscalResidencyDesc = row["FiscalResidencyDesc"].ToString();
                        receiver.FiscalIdentityNumber = row["FiscalIdentityNumber"].ToString();
                        receiver.StreetName = row["StreetName"].ToString();
                        receiver.StreetNumber = row["StreetNumber"].ToString();
                        receiver.ApartmentNumber = row["ApartmentNumber"].ToString();
                        receiver.Neighborhood = row["Neighborhood"].ToString();
                        receiver.Location = row["Location"].ToString();
                        receiver.StateProvinceId = row["StateProvinceId"].ToString();
                        receiver.CountryId = row["CountryId"].ToString();
                    }
                    // _logService.Debug("Method ends - GetTaxPayerbyId");
                    return receiver;
                }
                else
                {
                    // _logService.Debug("Method ends - GetTaxPayerbyId");
                    return null;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiverDA - GetTaxPayerbyId", e.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the fiscal record transaction by unique identifier.
        /// </summary>
        /// <param name="taxpayerId">The taxpayer identifier.</param>
        /// <param name="keyword">The keyword for the search.</param>
        /// <returns></returns>
        public List<Receiver> GetTaxPayers(string taxpayerId, string keyword = null)
        {
            try
            {
                DataSet invoiceTaxpayerDataSet = new DataSet();
                List<Receiver> lstReceiverModel = new List<Receiver>();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelInvoiceTaxpayer");
                    database.AddInParameter(command, "@TaxpayerId", DbType.String, taxpayerId);
                    database.AddInParameter(command, "@Keyword", DbType.String, keyword);
                    database.LoadDataSet(command, invoiceTaxpayerDataSet, "InvoiceTaxpayerDataSet");
                }
                if (invoiceTaxpayerDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in invoiceTaxpayerDataSet.Tables[0].Rows)
                    {
                        Receiver Receiver = new Receiver();
                        Receiver.CorporateName = row["CorporateName"].ToString();
                        Receiver.FiscalIdentityNumber = row["FiscalIdentityNumber"].ToString();
                        Receiver.FiscalResidency = row["FiscalResidency"].ToString();
                        Receiver.FiscalResidencyDesc = row["FiscalResidencyDesc"].ToString();
                        Receiver.HasInvoice = Convert.ToBoolean(row["HasInvoice"]);
                        Receiver.InvoiceForeignTaxpayerId = (row["InvoiceForeignTaxpayerId"] != DBNull.Value) ? (int?)row["InvoiceForeignTaxpayerId"] : null;
                        Receiver.InvoiceTaxpayerId = (int?)row["InvoiceTaxpayerId"];
                        Receiver.PostalCode = string.IsNullOrEmpty(row["PostalCode"].ToString()) ? string.Empty : row["PostalCode"].ToString();
                        Receiver.TaxPayerId = row["TaxPayerId"].ToString();
                        Receiver.TaxRegimenCode = row["TaxRegimenCode"].ToString();
                        Receiver.TaxRegimenDesc = row["TaxRegimenDesc"].ToString();
                        Receiver.TaxRegimenId = (row["TaxRegimenId"] != DBNull.Value) ? (int?)row["TaxRegimenID"] : null;
                        Receiver.StreetName = row["StreetName"].ToString();
                        Receiver.StreetNumber = row["StreetNumber"].ToString();
                        Receiver.ApartmentNumber = row["ApartmentNumber"].ToString();
                        Receiver.Neighborhood = row["Neighborhood"].ToString();
                        Receiver.Location = row["Location"].ToString();
                        Receiver.StateProvinceId = row["StateProvinceId"].ToString();
                        Receiver.CountryId = row["CountryId"].ToString();
                        lstReceiverModel.Add(Receiver);
                    }
                    return lstReceiverModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiverDA - GetTaxPayers", e.Message);
                throw;
            }
        }

        /// <summary>
        /// Saves the tax payers.
        /// </summary>
        /// <param name="Receiver">The receiver model.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public int SaveTaxPayers(Receiver Receiver, string userName)
        {
            try
            {
                // _logService.Debug("Method starts - SaveTaxPayers");
                Database database = _factory.CreateDefault();

                if (Receiver.FiscalIdentityNumber == null)
                {
                    int invoiceTaxpayerId = 0;
                    using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                    {
                        DbCommand command = database.GetStoredProcCommand("spInsInvoiceTaxpayer");
                        command.Connection = connection;

                        database.AddOutParameter(command, "@InvoiceTaxpayerId", DbType.Int32, 0);
                        database.AddInParameter(command, "@TaxPayerId", DbType.String, Receiver.TaxPayerId);
                        database.AddInParameter(command, "@CorporateName", DbType.String, (string.IsNullOrEmpty(Receiver.CorporateName) == true) ? "" : Receiver.CorporateName);
                        database.AddInParameter(command, "@PostalCode", DbType.String, Receiver.PostalCode);
                        database.AddInParameter(command, "@TaxRegimenId", DbType.Int32, Receiver.TaxRegimenId);
                        database.AddInParameter(command, "@StreetName", DbType.String, Receiver.StreetName);
                        database.AddInParameter(command, "@StreetNumber", DbType.String, Receiver.StreetNumber);
                        database.AddInParameter(command, "@ApartmentNumber", DbType.String, Receiver.ApartmentNumber);
                        database.AddInParameter(command, "@Neighborhood", DbType.String, Receiver.Neighborhood);
                        database.AddInParameter(command, "@Location", DbType.String, Receiver.Location);
                        database.AddInParameter(command, "@StateProvinceId", DbType.Int32, Receiver.StateProvinceId);
                        database.AddInParameter(command, "@CountryId", DbType.Int32, 1);
                        database.AddInParameter(command, "@Opid", DbType.String, userName);
                        database.ExecuteNonQuery(command);
                        invoiceTaxpayerId = (int)command.Parameters["@InvoiceTaxpayerId"].Value;
                    }
                    // _logService.Debug("Method ends");
                    return invoiceTaxpayerId;
                }
                else
                {
                    int returnStatus = 0;
                    using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                    {
                        DbCommand command = database.GetStoredProcCommand("spInsInvoiceForeignTaxpayer");
                        command.Connection = connection;
                        database.AddInParameter(command, "@InvoiceTaxpayerId", DbType.Int32, Receiver.InvoiceTaxpayerId);
                        database.AddInParameter(command, "@CorporateName", DbType.String, (string.IsNullOrEmpty(Receiver.CorporateName) == true) ? "" : Receiver.CorporateName);
                        database.AddInParameter(command, "@PostalCode", DbType.String, Receiver.PostalCode);
                        database.AddInParameter(command, "@TaxRegimenId", DbType.Int32, Receiver.TaxRegimenId);
                        database.AddInParameter(command, "@FiscalResidency", DbType.String, Receiver.FiscalResidency);
                        database.AddInParameter(command, "@FiscalIdentityNumber", DbType.String, Receiver.FiscalIdentityNumber);
                        object internalValue = new object();
                        database.AddParameter(command, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, internalValue);
                        database.ExecuteNonQuery(command);
                        returnStatus = (int)database.GetParameterValue(command, "@ReturnStatus");
                    }
                    // _logService.Debug("Method ends - SaveTaxPayers");
                    return returnStatus;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiverDA - SaveTaxPayers", e.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the tax payer.
        /// </summary>
        /// <param name="receiver">The receiver.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public int UpdateTaxPayer(Receiver receiver, string userName)
        {
            try
            {
                // _logService.Debug("Method starts - UpdateTaxPayer");
                Database database = _factory.CreateDefault();
                if (receiver.FiscalIdentityNumber == null)
                {
                    int returnStatus = 0;
                    using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                    {
                        DbCommand command = database.GetStoredProcCommand("spUpdInvoiceTaxpayerById");
                        database.AddInParameter(command, "@InvoiceTaxpayerId", DbType.Int32, receiver.InvoiceTaxpayerId);
                        database.AddInParameter(command, "@CorporateName", DbType.String, (string.IsNullOrEmpty(receiver.CorporateName) == true) ? "" : receiver.CorporateName);
                        database.AddInParameter(command, "@PostalCode", DbType.String, receiver.PostalCode);
                        database.AddInParameter(command, "@TaxRegimenId", DbType.Int32, receiver.TaxRegimenId);
                        database.AddInParameter(command, "@StreetName", DbType.String, receiver.StreetName);
                        database.AddInParameter(command, "@StreetNumber", DbType.String, receiver.StreetNumber);
                        database.AddInParameter(command, "@ApartmentNumber", DbType.String, receiver.ApartmentNumber);
                        database.AddInParameter(command, "@Neighborhood", DbType.String, receiver.Neighborhood);
                        database.AddInParameter(command, "@Location", DbType.String, receiver.Location);
                        database.AddInParameter(command, "@StateProvinceId", DbType.Int32, receiver.StateProvinceId);
                        database.AddInParameter(command, "@CountryId", DbType.Int32, 1);
                        database.AddInParameter(command, "@Opid", DbType.String, userName);
                        object internalValue = new object();
                        database.AddParameter(command, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, internalValue);
                        database.ExecuteNonQuery(command);
                        returnStatus = (int)database.GetParameterValue(command, "@ReturnStatus");
                    }
                    // _logService.Debug("Method ends - GetTaxPayers");
                    return returnStatus;
                }
                else
                {
                    int returnStatus = 0;
                    using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                    {
                        DbCommand command = database.GetStoredProcCommand("spUpdInvoiceForeignTaxpayer");
                        database.AddInParameter(command, "@InvoiceForeignTaxpayerId", DbType.Int32, receiver.InvoiceForeignTaxpayerId);
                        database.AddInParameter(command, "@CorporateName", DbType.String, (string.IsNullOrEmpty(receiver.CorporateName) == true) ? "" : receiver.CorporateName);
                        database.AddInParameter(command, "@PostalCode", DbType.String, receiver.PostalCode);
                        database.AddInParameter(command, "@TaxRegimenId", DbType.Int32, receiver.TaxRegimenId);
                        database.AddInParameter(command, "@FiscalResidency", DbType.String, receiver.FiscalResidency);
                        database.AddInParameter(command, "@FiscalIdentityNumber", DbType.String, receiver.FiscalIdentityNumber);
                        object internalValue = new object();
                        database.AddParameter(command, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, internalValue);
                        database.ExecuteNonQuery(command);
                        returnStatus = (int)database.GetParameterValue(command, "@ReturnStatus");
                    }
                    // _logService.Debug("Method ends - UpdateTaxPayer");
                    return returnStatus;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ReceiverDA - UpdateTaxPayer", e.Message);
                throw;
            }
        }
    }
}