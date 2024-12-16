// --------------------------------------------------------------------
// <copyright file="DropdownListViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace WebAdminUI.Areas.ElectronicCertificate.Models.Shared
{
    /// <summary>
    /// DropdownListViewModel
    /// </summary>
    public class DropdownListViewModel
    {
        /// <summary>
        /// Gets or sets the code value key.
        /// </summary>
        /// <value>
        /// The code value key.
        /// </value>
        public dynamic CodeValueKey { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the short description.
        /// </summary>
        /// <value>
        /// The short description.
        /// </value>
        public string ShortDescription { get; set; }
    }
}