// --------------------------------------------------------------------
// <copyright file="ChargeCreditDA.cs" company="Ellucian">
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
    /// ChargeCreditDA
    /// </summary>
    public class ChargeCreditMappingDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChargeCreditMappingDA"/> class.
        /// </summary>
        public ChargeCreditMappingDA()
        {
            _factory = new DatabaseProviderFactory();
        }

        /// <summary>
        /// Deletes the fiscal record transaction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public int DeleteChargeCreditMapping(int id)
        {
            try
            {
                // _logService.Debug("DMethod starts - DeleteChargeCreditMapping");
                int returnStatus;

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spDelFiscalRecordChargeMapping");

                    database.AddInParameter(command, "@FiscalRecordChargeMappingId", DbType.Int32, id);
                    object internalValue = new object();
                    database.AddParameter(command, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, internalValue);
                    database.ExecuteNonQuery(command);
                    returnStatus = (int)database.GetParameterValue(command, "@ReturnStatus");
                    // _logService.Debug("Debug - Exit - PowerCampus.DataAccess - ChargeCreditMappingDA - DeleteChargeCreditMapping");
                    return returnStatus;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ChargeCreditMappingDA - DeleteChargeCreditMapping", e.Message + e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the mapping product service.
        /// </summary>
        /// <returns></returns>
        public List<ChargeCreditMapping> GetChargeCreditMapping()
        {
            try
            {
                // _logService.Debug("Method starts - GetChargeCreditMapping");
                DataSet chargeCreditMappingDataSet = new DataSet();
                List<ChargeCreditMapping> lstChargeCreditMapping = new List<ChargeCreditMapping>();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("dbo.spSelFiscalRecordChargeMapping");

                    database.LoadDataSet(command, chargeCreditMappingDataSet, "ChargeCreditMapping");
                }
                if (chargeCreditMappingDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in chargeCreditMappingDataSet.Tables[0].Rows)
                    {
                        ChargeCreditMapping chargeCreditMapping = new ChargeCreditMapping();
                        chargeCreditMapping.Id = (int)row["FiscalRecordChargeMappingId"];
                        chargeCreditMapping.ChargeCreditCodeId = (int)row["ChargeCreditCodeId"];
                        chargeCreditMapping.ChargeCreditDesc = row["ChargeCreditCodeValue"] + " - " + row["ChargeCreditDesc"].ToString();
                        chargeCreditMapping.ProductServiceKey = row["ProductServiceKey"].ToString();
                        chargeCreditMapping.ProductServiceDesc = row["ProductServiceKey"].ToString() + " - " + row["ProductServiceDesc"].ToString();
                        chargeCreditMapping.UnityKey = row["UnityKey"].ToString();
                        chargeCreditMapping.UnityDesc = row["UnityKey"].ToString() + " - " + row["UnityDesc"].ToString();
                        chargeCreditMapping.TaxDescription = row["TaxDescription"].ToString();
                        chargeCreditMapping.TaxRate = row["TaxRate"].ToString();
                        chargeCreditMapping.FactorType = row["FactorType"].ToString();
                        lstChargeCreditMapping.Add(chargeCreditMapping);
                    }
                    // _logService.Debug("Method ends - GetChargeCreditMapping");
                    return lstChargeCreditMapping;
                }
                else
                {
                    // _logService.Debug("Method ends - GetChargeCreditMapping");
                    return null;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ChargeCreditMappingDA - GetChargeCreditMapping", e.Message + e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the mapping product tax service.
        /// </summary>
        /// <returns></returns>
        public List<ChargeCreditTaxes> GetChargeCreditTaxes(string chargeCreditCode)
        {
            try
            {
                string[] chargeCreditDesc = chargeCreditCode.Split('-');
                string chargeCreditCodeValue = chargeCreditDesc[0].Trim();
                DataSet chargeCreditTaxesDataSet = new DataSet();
                List<ChargeCreditTaxes> lstChargeCreditTaxes = new List<ChargeCreditTaxes>();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("WebAdmin.spSelChargeCreditTaxMapping");

                    database.AddInParameter(command, "@ChargeCreditCode", DbType.String, chargeCreditCodeValue);
                    database.LoadDataSet(command, chargeCreditTaxesDataSet, "ChargeCreditTaxes");
                }
                if (chargeCreditTaxesDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in chargeCreditTaxesDataSet.Tables[0].Rows)
                    {
                        ChargeCreditTaxes chargeCreditTaxes = new ChargeCreditTaxes();
                        chargeCreditTaxes.ChargeCreditTaxCode = row["ChargeCreditTaxCode"].ToString();
                        chargeCreditTaxes.Description = row["TaxDesc"].ToString();
                        chargeCreditTaxes.TaxCode = row["TaxCode"].ToString();
                        chargeCreditTaxes.TaxRate = string.IsNullOrEmpty(row["TaxRate"].ToString()) ? (decimal?)null : Convert.ToDecimal(row["TaxRate"]);
                        chargeCreditTaxes.FactorType = row["FactorType"].ToString();

                        lstChargeCreditTaxes.Add(chargeCreditTaxes);
                    }
                }
                return lstChargeCreditTaxes;
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ChargeCreditMappingDA - GetChargeCreditMapping", e.Message + e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the special iva tax.
        /// </summary>
        /// <returns></returns>
        public List<ChargeCreditTaxes> GetSpecialIvaTax()
        {
            try
            {
                DataSet chargeCreditTaxesDataSet = new DataSet();
                List<ChargeCreditTaxes> chargeCreditTaxes = new List<ChargeCreditTaxes>();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("WebAdmin.spRetrieveSpecialIVATax ");

                    database.LoadDataSet(command, chargeCreditTaxesDataSet, "chargeCreditTaxes");
                }
                if (chargeCreditTaxesDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in chargeCreditTaxesDataSet.Tables[0].Rows)
                    {
                        ChargeCreditTaxes chargeCreditTax = new ChargeCreditTaxes();
                        chargeCreditTax.TaxCode = (string)row["TaxKey"];
                        chargeCreditTax.Description = (string)row["TaxDescription"];
                        chargeCreditTax.FactorType = (string)row["FactorTypeKey"];
                        chargeCreditTax.TaxRate = string.IsNullOrEmpty(row["TaxRate"].ToString()) ? (decimal?)null : Convert.ToDecimal(row["TaxRate"]);

                        chargeCreditTaxes.Add(chargeCreditTax);
                    }
                    return chargeCreditTaxes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ChargeCreditMappingDA - GetSpecialIvaTax", e.Message + e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Saves the charge credit mapping.
        /// </summary>
        /// <param name="chargeCreditMapping">The charge credit mapping.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public int SaveChargeCreditMapping(ChargeCreditMapping chargeCreditMapping, string userName)
        {
            try
            {
                // _logService.Debug("Method starts - SaveChargeCreditMapping");
                int fiscalRecordChargeMappingId;

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("dbo.spInsFiscalRecordChargeMapping");

                    database.AddInParameter(command, "@ChargeCreditCodeId", DbType.Int32, chargeCreditMapping.ChargeCreditCodeId);
                    database.AddInParameter(command, "@ProductServiceKey", DbType.String, chargeCreditMapping.ProductServiceKey);
                    database.AddInParameter(command, "@UnityKey", DbType.String, chargeCreditMapping.UnityKey);
                    database.AddInParameter(command, "@Opid", DbType.String, userName);
                    database.AddOutParameter(command, "@FiscalRecordChargeMappingId", DbType.Int32, 0);

                    database.ExecuteNonQuery(command);

                    fiscalRecordChargeMappingId = (int)command.Parameters["@FiscalRecordChargeMappingId"].Value;

                    // _logService.Debug("Method ends - SaveChargeCreditMapping");
                    return fiscalRecordChargeMappingId;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ChargeCreditMappingDA - SaveChargeCreditMapping", e.Message + e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Saves the charge tax mapping.
        /// </summary>
        /// <param name="chargeCreditTax">The charge credit tax.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public int SaveChargeTaxMapping(ChargeCreditTaxes chargeCreditTax, string userName)
        {
            try
            {
                int fiscalRecordChargeMappingId = 0;

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("WebAdmin.spInsFiscalRecordChargeTaxMapping");

                    database.AddInParameter(command, "@ChargeCreditCodeId", DbType.Int32, chargeCreditTax.ChargeCreditCodeId);
                    database.AddInParameter(command, "@TaxKey", DbType.String, chargeCreditTax.TaxCode);
                    database.AddInParameter(command, "@TaxDescription", DbType.String, chargeCreditTax.Description);
                    database.AddInParameter(command, "@FactorTypeKey", DbType.String, chargeCreditTax.FactorType);
                    database.AddInParameter(command, "@TaxRate", DbType.Decimal, chargeCreditTax.TaxRate);
                    database.AddInParameter(command, "@Opid", DbType.String, userName);
                    database.AddOutParameter(command, "@FiscalRecordChrgTaxMappingId", DbType.Int32, 0);

                    database.ExecuteNonQuery(command);

                    fiscalRecordChargeMappingId = (int)command.Parameters["@FiscalRecordChrgTaxMappingId"].Value;

                    return fiscalRecordChargeMappingId;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ChargeCreditMappingDA - SaveChargeTaxMapping", e.Message + e.StackTrace);
                throw;
            }
        }
    }
}