// --------------------------------------------------------------------
// <copyright file="PowerCampusSerializer.cs" company="Ellucian">
//     Copyright 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using PowerCampus.Entities;
using System;
using System.IO;

namespace SelfService.Helpers
{
    /// <summary>
    /// A helper class to serialize and deserialize objects
    /// </summary>
    internal static class PowerCampusSerializer
    {
        /// <summary>
        /// Reads the json.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="jsonName">Name of the json.</param>
        /// <returns></returns>
        internal static string ReadJson(string filePath, string jsonName)
        {
            string json = string.Empty;
            try
            {
                string path = string.Empty;
                if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(jsonName))
                    return null;

                path = Path.Combine(filePath, $"{jsonName}.json");

                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    using (StreamReader reader = new StreamReader(path))
                        json = reader.ReadToEnd();
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.LogWebError(Constants.WebAdminUi, typeof(PowerCampusSerializer).FullName, exception.Message, exception);
            }
            return json;
        }
    }
}