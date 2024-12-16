// --------------------------------------------------------------------
// <copyright file="RecordTypeViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace WebAdminUI.Models.SearchResult
{
    /// <summary>
    /// RecordTypeViewModel class
    /// </summary>
    public class RecordTypeViewModel
    {
        /// <summary>
        /// Gets or sets the code value.
        /// </summary>
        /// <value>
        /// The code value.
        /// </value>
        public string CodeValue { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is org type.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is org type; otherwise, <c>false</c>.
        /// </value>
        public bool IsOrgType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is people type.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is people type; otherwise, <c>false</c>.
        /// </value>
        public bool IsPeopleType { get; set; }
    }
}