// --------------------------------------------------------------------
// <copyright file="SaveAuthorizationTypeMapping.cs" company="Ellucian">
//     Copyright 2017 - 2020 Ellucian Company ~L~.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace WebAdminUI.Areas.ElectronicDegree.Models.Catalogs
{
    /// <summary>
    /// Handles the information for saving the authorization type mapping
    /// </summary>
    public class SaveAuthorizationTypeMapping
    {
        /// <summary>
        /// Gets or sets the type of the authorization.
        /// </summary>
        /// <value>
        /// The type of the authorization.
        /// </value>
        public string AuthorizationType { get; set; }

        /// <summary>
        /// Gets or sets the rvoes.
        /// </summary>
        /// <value>
        /// The rvoes.
        /// </value>
        public string[] Rvoes { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }
    }
}