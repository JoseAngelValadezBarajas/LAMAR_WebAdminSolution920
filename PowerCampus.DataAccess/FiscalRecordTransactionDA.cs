// --------------------------------------------------------------------
// <copyright file="FiscalRecordTransactionDA.cs" company="Ellucian">
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
    /// FiscalRecordTransactionDA Class
    /// </summary>
    public class FiscalRecordTransactionDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FiscalRecordDA"/> class.
        /// </summary>
        public FiscalRecordTransactionDA()
        {
            _factory = new DatabaseProviderFactory();
        }

        /// <summary>
        /// Deletes the fiscal record transaction.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public int DeleteFiscalRecordTransaction(Guid guid)
        {
            try
            {
                // _logService.Debug("Method starts - DeleteFiscalRecordTransaction");
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    int returnStatus;
                    DbCommand command = database.GetStoredProcCommand("spDelFiscalRecordTransaction");

                    database.AddInParameter(command, "@Guid", DbType.Guid, guid);
                    object internalValue = new object();
                    database.AddParameter(command, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, internalValue);

                    database.ExecuteNonQuery(command);
                    returnStatus = (int)database.GetParameterValue(command, "@ReturnStatus");
                    // _logService.Debug("Method ends - DeleteFiscalRecordTransaction");
                    return returnStatus;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - DeleteFiscalRecordTransaction", e.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the fiscal record permissions.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public Account GetFiscalRecordPermissions(string userName)
        {
            try
            {
                DataSet permissionsDataSet = new DataSet();
                Account permissionsAccount = new Account();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelWebAdminPermissions");
                    database.AddInParameter(command, "@UserName", DbType.String, userName);
                    database.LoadDataSet(command, permissionsDataSet, "webAdmin");

                    if (permissionsDataSet.Tables[0].Rows.Count > 0)
                    {
                        permissionsAccount.permissions = new List<Permissions>();
                        permissionsAccount.permissions = permissionsDataSet.Tables[0].AsEnumerable().Select(m => new Permissions()
                        {
                            Description = m.Field<string>("Permission"),
                            Value = m.Field<bool>("Allowed")
                        }).ToList();
                    }

                    return permissionsAccount;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - GetFiscalRecordPermissions", e.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the fiscal record transaction by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public Account GetFiscalRecordTransactionByGuid(Guid guid)
        {
            try
            {
                // _logService.Debug("Method starts - GetFiscalRecordTransactionByGuid");
                DataSet fiscalRecordsDataSet = new DataSet();
                Account Account = new Account();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelFiscalRecordTransactionByGuid");

                    database.AddInParameter(command, "@Guid", DbType.Guid, guid);
                    database.LoadDataSet(command, fiscalRecordsDataSet, "FiscalRecords");

                    if (fiscalRecordsDataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in fiscalRecordsDataSet.Tables[0].Rows)
                        {
                            Account.GUID = guid;
                            Account.UserName = row["UserName"].ToString();
                            Account.ReceiptNumber = row["ReceiptNumber"].ToString();
                            Account.PeopleOrgCodeId = row["PeopleOrgCodeId"].ToString();
                            Account.CreateDateTime = (DateTime)row["CreateDateTime"];
                            Account.Action = (EnumFiscalRecordAction)Enum.Parse(typeof(EnumFiscalRecordAction), row["Action"].ToString());
                        }
                        // _logService.Debug("Method ends - GetFiscalRecordTransactionByGuid");
                        return Account;
                    }
                    else
                    {
                        LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - RetrieveFiscalRecords", string.Format("GetFiscalRecordTransactionByGuid - GUID {0} number doesn't exist in current database", guid));
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - RetrieveFiscalRecords", e.Message);
                throw;
            }
        }
    }
}