// --------------------------------------------------------------------
// <copyright file="TaxProfileRateDA.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
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
    /// TaxProfileRateDA
    /// </summary>
    public class TaxProfileDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxProfileDA"/> class.
        /// </summary>
        public TaxProfileDA()
        {
            _factory = new DatabaseProviderFactory();
        }

        /// <summary>
        /// Gets the tax profile details.
        /// </summary>
        /// <param name="taxProfileValidityId">The tax profile validity identifier.</param>
        /// <returns></returns>
        public List<TaxProfileDetail> GetTaxProfileDetails(int taxProfileValidityId)
        {
            var results = new List<TaxProfileDetail>();
            try
            {
                // _logService.Debug("Method starts - GetTaxProfileDetails");
                DataSet TaxProfileDataSet = new DataSet();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelTaxProfileDetailByValidity");

                    database.AddInParameter(command, "@TaxProfileValidityId", DbType.Int32, taxProfileValidityId);
                    database.LoadDataSet(command, TaxProfileDataSet, "TaxProfile");
                }
                if (TaxProfileDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TaxProfileDataSet.Tables[0].Rows)
                    {
                        var detail = new TaxProfileDetail
                        {
                            TaxProfileDetailId = int.Parse(row["TaxProfileDetailId"].ToString()),
                            TaxProfileValidityId = int.Parse(row["TaxProfileValidityId"].ToString()),
                            ChargeCreditDescription = row["ChargeDesc"].ToString(),
                            Sequence = int.Parse(row["Sequence"].ToString()),
                            Percentage = decimal.Parse(row["Percentage"].ToString())
                        };
                        var taxMapping = new FiscalRecordTaxMapping()
                        {
                            FiscalRecordTaxMappingId = string.IsNullOrEmpty(row["FiscalRecordTaxMappingId"].ToString()) ? 0 : int.Parse(row["FiscalRecordTaxMappingId"].ToString()),
                            TaxCode = string.IsNullOrEmpty(row["TaxCode"].ToString()) ? string.Empty : row["TaxCode"].ToString(),
                            TaxDescription = string.IsNullOrEmpty(row["TaxDesc"].ToString()) ? string.Empty : row["TaxDesc"].ToString(),
                            TaxRate = string.IsNullOrEmpty(row["TaxRate"].ToString()) ? string.Empty : row["TaxRate"].ToString(),
                            FactorType = string.IsNullOrEmpty(row["FactorType"].ToString()) ? string.Empty : row["FactorType"].ToString()
                        };
                        detail.TaxMapping = taxMapping;
                        results.Add(detail);
                    }
                }
                // _logService.Debug("Method ends - GetTaxProfileDetails");
                return results;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - TaxProfileDA - GetTaxProfileDetails", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the tax profile list.
        /// </summary>
        /// <returns></returns>
        public List<TaxProfile> GetTaxProfileList()
        {
            List<TaxProfile> results = new List<TaxProfile>();
            try
            {
                // _logService.Debug("Method starts - GetTaxProfileList");
                DataSet taxProfileDataSet = new DataSet();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelTaxProfile");

                    database.LoadDataSet(command, taxProfileDataSet, "CashReceiptInvoiceDetail");
                }
                if (taxProfileDataSet.Tables[0].Rows.Count > 0)
                {
                    results = taxProfileDataSet.Tables[0].AsEnumerable().Select(m => new TaxProfile()
                    {
                        TaxProfileId = m.Field<int>("TaxProfileId"),
                        Name = m.Field<string>("Name"),
                        Description = m.Field<string>("Description"),
                    }).ToList();
                }
                // _logService.Debug("Method ends - GetTaxProfileList");
                return results;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - TaxProfileDA - GetTaxProfileList", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the tax profile validities.
        /// </summary>
        /// <param name="taxProfile">The tax profile.</param>
        /// <returns></returns>
        public List<TaxProfileValidity> GetTaxProfileValidities(int taxProfile)
        {
            List<TaxProfileValidity> results = new List<TaxProfileValidity>();
            try
            {
                // _logService.Debug("Method starts - GetTaxProfileValidities");
                DataSet taxProfileValiditiesDataSet = new DataSet();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelTaxProfileValidityByProfile");

                    database.AddInParameter(command, "@TaxProfileId", DbType.Int32, taxProfile);
                    database.LoadDataSet(command, taxProfileValiditiesDataSet, "TaxProfile");
                }
                if (taxProfileValiditiesDataSet.Tables[0].Rows.Count > 0)
                {
                    results = taxProfileValiditiesDataSet.Tables[0].AsEnumerable().Select(m => new TaxProfileValidity()
                    {
                        TaxProfileId = m.Field<int>("TaxProfileId"),
                        TaxProfileValidityId = m.Field<int>("TaxProfileValidityId"),
                        StartDateTime = m.Field<DateTime>("StartDatetime"),
                        EndDateTime = m.Field<DateTime?>("EndDatetime")
                    }).ToList();
                }
                // _logService.Debug("Method ends - GetTaxProfileValidities");
                return results;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - TaxProfileDA - GetTaxProfileValidities", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Saves the tax mapping.
        /// </summary>
        /// <param name="taxMapping">The tax mapping.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public int SaveTaxMapping(FiscalRecordTaxMapping taxMapping, string userName)
        {
            try
            {
                // _logService.Debug("Method starts - SaveTaxMapping");
                int returnStatus = 0;
                Database database = _factory.CreateDefault();
                DbCommand command;
                if (taxMapping.FiscalRecordTaxMappingId == 0)
                {
                    using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                    {
                        command = database.GetStoredProcCommand("spInsFiscalRecordTaxMapping");
                        database.AddOutParameter(command, "@FiscalRecordTaxMappingId", DbType.Int32, 0);
                        database.AddInParameter(command, "@TaxProfileDetailId", DbType.Int32, taxMapping.TaxProfileDetailId);
                        database.AddInParameter(command, "@TaxCode", DbType.String, taxMapping.TaxCode);
                        database.AddInParameter(command, "@TaxRate", DbType.String, taxMapping.TaxRate);
                        database.AddInParameter(command, "@FactorType", DbType.String, taxMapping.FactorType);
                        database.AddInParameter(command, "@Opid", DbType.String, userName);
                        object internalValue = new object();
                        database.AddParameter(command, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, internalValue);
                        database.ExecuteNonQuery(command);
                        returnStatus = (int)database.GetParameterValue(command, "@ReturnStatus");
                    }
                    // _logService.Debug("Method ends - SaveTaxMapping");
                    return returnStatus;
                }
                else
                {
                    using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                    {
                        command = database.GetStoredProcCommand("spUpdFiscalRecordTaxMapping");
                        database.AddInParameter(command, "@FiscalRecordTaxMappingId", DbType.Int32, taxMapping.FiscalRecordTaxMappingId);
                        database.AddInParameter(command, "@TaxCode", DbType.String, taxMapping.TaxCode);
                        database.AddInParameter(command, "@TaxRate", DbType.String, taxMapping.TaxRate);
                        database.AddInParameter(command, "@FactorType", DbType.String, taxMapping.FactorType);
                        database.AddInParameter(command, "@Opid", DbType.String, userName);
                        object internalValue = new object();
                        database.AddParameter(command, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, internalValue);
                        database.ExecuteNonQuery(command);
                        returnStatus = (int)database.GetParameterValue(command, "@ReturnStatus");
                    }
                    // _logService.Debug("Method ends - SaveTaxMapping");
                    return returnStatus;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - TaxProfileDA - SaveTaxMapping", ex.Message);
                throw;
            }
        }
    }
}