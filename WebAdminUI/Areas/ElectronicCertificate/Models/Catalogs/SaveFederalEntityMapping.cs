// --------------------------------------------------------------------
// <copyright file="SaveFederalEntityMapping.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace WebAdminUI.Areas.ElectronicCertificate.Models.Catalogs
{
    /// <summary>
    /// Model for saving a Federal Entity Mapping
    /// </summary>
    public class SaveFederalEntityMapping
    {
        /// <summary>
        /// Gets or sets the federal entity.
        /// </summary>
        /// <value>
        /// The federal entity.
        /// </value>
        public string FederalEntity { get; set; }

        /// <summary>
        /// Gets or sets the states.
        /// </summary>
        /// <value>
        /// The states.
        /// </value>
        public string[] States { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }
    }
}