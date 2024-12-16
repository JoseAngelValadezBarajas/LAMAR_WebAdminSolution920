// --------------------------------------------------------------------
// <copyright file="CFDIUsageCatalog.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
namespace PowerCampus.Entities
{
    /// <summary>
    /// CFDIUsageCatalog Class
    /// </summary>
    /// <seealso cref="PowerCampus.Entities.FiscalRecordCatalog" />
    public class CFDIUsageCatalog : FiscalRecordCatalog
    {
        /// <summary>
        /// Gets or sets a value indicating whether [applies to moral person].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [applies to moral person]; otherwise, <c>false</c>.
        /// </value>
        public bool AppliesToMoralPerson { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [applies to physical person].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [applies to physical person]; otherwise, <c>false</c>.
        /// </value>
        public bool AppliesToPhysicalPerson { get; set; }
    }
}