// --------------------------------------------------------------------
// <copyright file="ElectronicDegreeTransactionDA.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace PowerCampus.DataAccess.ElectronicDegree
{
    /// <summary>
    /// ElectronicDegreeTransactionDA
    /// </summary>
    public class ElectronicDegreeTransactionDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElectronicDegreeTransactionDA"/> class.
        /// </summary>
        public ElectronicDegreeTransactionDA() => _factory = new DatabaseProviderFactory();

        /// <summary>
        /// Deletes the electronic degree transaction.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public void DeleteElectronicDegreeTransaction(Guid guid)
        {
            try
            {
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                ElectronicDegreeTransaction query = (from edt in context.ElectronicDegreeTransactions
                                                     where edt.Guid == guid
                                                     select edt).FirstOrDefault();

                context.ElectronicDegreeTransactions.DeleteOnSubmit(query);
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - ElectronicDegreeTransactionDA - DeleteElectronicDegreeTransaction", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the electronic degree permissions.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public Account GetElectronicDegreePermissions(string userName)
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
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "GetElectronicDegreePermissions", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the electronic degree transaction by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public Account GetElectronicDegreeTransactionByGuid(Guid guid)
        {
            try
            {
                Account Account = new Account();
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                ElectronicDegreeTransaction query = (from edt in context.ElectronicDegreeTransactions
                                                     join abtUsr in context.ABT_USERs
                                                     on edt.UserName
                                                     equals abtUsr.OPERATOR_ID
                                                     where edt.Guid == guid
                                                     select edt).FirstOrDefault();

                if (query != null)
                {
                    Account.GUID = guid;
                    Account.UserName = query.UserName;
                    Account.CreateDateTime = query.CreateDatetime;
                    return Account;
                }
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - ElectronicDegreeTransactionDA - GetElectronicDegreeByGuid", string.Format("GetElectronicDegreeByGuid - GUID {0} number doesn't exist in current database", guid));

                return null;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - ElectronicDegreeTransactionDA - GetElectronicDegreeByGuid", ex.Message);
                throw;
            }
        }
    }
}