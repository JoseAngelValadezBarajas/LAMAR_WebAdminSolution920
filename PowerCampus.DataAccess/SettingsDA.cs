// --------------------------------------------------------------------
// <copyright file="SettingsService.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PowerCampus.Entities;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace PowerCampus.DataAccess
{
    /// <summary>
    /// Settings DA
    /// </summary>
    public class SettingsDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsDA"/> class.
        /// </summary>
        public SettingsDA()
        {
            _factory = new DatabaseProviderFactory();
        }

        /// <summary>
        /// Gets the setting.
        /// </summary>
        /// <param name="Settings">The settings model.</param>
        /// <returns></returns>
        public string GetSetting(Settings Settings)
        {
            try
            {
                // _logService.Debug("Method starts - GetSetting");
                Database database = _factory.CreateDefault();
                string result;
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetSqlStringCommand("select dbo.fnGetAbtSetting(@AREA_NAME, @SECTION_NAME, @LABEL_NAME)");
                    database.AddInParameter(command, "@AREA_NAME", DbType.String, Settings.Area);
                    database.AddInParameter(command, "@SECTION_NAME", DbType.String, Settings.Section);
                    database.AddInParameter(command, "@LABEL_NAME", DbType.String, Settings.Label);
                    result = database.ExecuteScalar(command) as string;
                }
                // _logService.Debug("Method ends - GetSetting");
                return result;
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - SettingsDA - GetSetting", e.Message);
                throw;
            }
        }
    }
}