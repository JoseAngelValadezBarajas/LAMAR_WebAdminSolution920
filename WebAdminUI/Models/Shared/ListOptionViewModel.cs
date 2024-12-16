// --------------------------------------------------------------------
// <copyright file="ListOptionViewModel.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace WebAdministration.Models.Shared
{
    /// <summary>
    /// ListOptionViewModel
    /// </summary>
    public class ListOptionViewModel
    {
        /// <summary>
        /// Gets or sets the complement.
        /// </summary>
        /// <value>
        /// The complement.
        /// </value>
        public dynamic Complement { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public dynamic Value { get; set; }
    }
}