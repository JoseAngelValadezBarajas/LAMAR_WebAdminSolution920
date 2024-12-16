// --------------------------------------------------------------------
// <copyright file="ISettingsService.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;

namespace PowerCampus.BusinessInterfaces
{
    /// <summary>
    /// ISettingsService Interface
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Gets the setting.
        /// </summary>
        /// <param name="Settings">The settings model.</param>
        /// <returns></returns>
        string GetSetting(Settings Settings);
    }
}