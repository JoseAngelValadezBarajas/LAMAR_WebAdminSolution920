// --------------------------------------------------------------------
// <copyright file="SaveBackgroundStudyTypeMapping.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace WebAdminUI.Areas.ElectronicDegree.Models.Catalogs
{
    /// <summary>
    /// Model for saving a Background Study Type Mapping
    /// </summary>
    public class SaveBackgroundStudyTypeMapping
    {
        /// <summary>
        /// Gets or sets the type of the background study.
        /// </summary>
        /// <value>
        /// The type of the background study.
        /// </value>
        public string BackgroundStudyType { get; set; }

        /// <summary>
        /// Gets or sets the levels.
        /// </summary>
        /// <value>
        /// The levels.
        /// </value>
        public string[] Levels { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }
    }
}