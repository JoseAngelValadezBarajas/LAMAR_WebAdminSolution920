// --------------------------------------------------------------------
// <copyright file="TaxRegimenCatalog.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities
{
    /// <summary>
    /// TaxRegimenCatalog Class
    /// </summary>
    /// <seealso cref="PowerCampus.Entities.FiscalRecordCatalog" />
    public class TaxRegimenCatalog : FiscalRecordCatalog
    {
        /// <summary>
        /// Gets or sets a value indicating whether [moral person].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [moral person]; otherwise, <c>false</c>.
        /// </value>
        public bool MoralPerson { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [physical person].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [physical person]; otherwise, <c>false</c>.
        /// </value>
        public bool PhysicalPerson { get; set; }
    }
}