// --------------------------------------------------------------------
// <copyright file="SettingsService.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.BusinessInterfaces;
using PowerCampus.DataAccess;
using PowerCampus.Entities;

namespace PowerCampus.Business
{
    /// <inheritdoc />
    /// <summary>
    /// Settings Service
    /// </summary>
    /// <seealso cref="T:PowerCampus.BusinessInterfaces.ISettingsService" />
    public class SettingsService : ISettingsService
    {
        /// <inheritdoc />
        /// <summary>
        /// Gets the setting.
        /// </summary>
        /// <param name="Settings">The settings model.</param>
        /// <returns></returns>
        public string GetSetting(Settings Settings)
        {
            // _logService.Debug("GetSetting starts");
            var settingsDa = new SettingsDA();
            string setting = settingsDa.GetSetting(Settings);
            // _logService.Debug("GetSetting ends");
            return setting;
        }
    }
}