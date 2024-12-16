// --------------------------------------------------------------------
// <copyright file="FiscalRecordCancelReason.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System;

namespace PowerCampus.Entities
{
    /// <summary>
    /// FiscalRecordCancelReason class.
    /// </summary>
    /// <seealso cref="FiscalRecordCatalog" />
    public class FiscalRecordCancelReason : FiscalRecordCatalog
    {
        /// <summary>
        /// Gets or sets a value indicating whether [apply to global].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [apply to global]; otherwise, <c>false</c>.
        /// </value>
        public bool ApplyToGlobal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [apply to individual].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [apply to individual]; otherwise, <c>false</c>.
        /// </value>
        public bool ApplyToIndividual { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}