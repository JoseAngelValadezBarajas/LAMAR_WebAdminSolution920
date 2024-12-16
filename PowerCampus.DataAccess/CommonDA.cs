// --------------------------------------------------------------------
// <copyright file="CommonDA.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace PowerCampus.DataAccess
{
    /// <summary>
    /// Common Data Access - Provide access to DB stored procedures use in general
    /// </summary>
    public static class CommonDA
    {
        /// <summary>
        /// Gets the full name of the people org.
        /// </summary>
        /// <param name="peopleOrgCodeId">The people org code identifier.</param>
        /// <returns></returns>
        public static string GetPeopleOrgFullName(string peopleOrgCodeId)
        {
            try
            {
                // _logService.Debug("Method starts - GetPeopleOrgFullName");
                string result;
                DatabaseProviderFactory _factory = new DatabaseProviderFactory();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetSqlStringCommand("select dbo.fnPeopleOrgShortName(@People_Org_Code_Id)");

                    database.AddInParameter(command, "@People_Org_Code_Id", DbType.String, peopleOrgCodeId);
                    result = (string)database.ExecuteScalar(command);
                }
                // _logService.Debug("Method ends - GetPeopleOrgFullName");
                return result;
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - CommonDA - GetPeopleOrgFullName", e.Message + e.StackTrace);
                throw;
            }
        }
    }
}