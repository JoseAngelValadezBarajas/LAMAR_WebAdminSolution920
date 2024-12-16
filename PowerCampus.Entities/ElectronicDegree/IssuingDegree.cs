// --------------------------------------------------------------------
// <copyright file="IssuingDegree.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

namespace PowerCampus.Entities.ElectronicDegree
{
    /// <summary>
    /// GraduationRequirement Catalog
    /// </summary>
    public class GraduationRequirementCatalog
    {
        /// <summary>
        /// Gets or sets the graduation requirement code.
        /// </summary>
        /// <value>
        /// The graduation requirement code.
        /// </value>
        public int GraduationRequirementCode { get; set; }

        /// <summary>
        /// Gets or sets the graduation requirement desc.
        /// </summary>
        /// <value>
        /// The graduation requirement desc.
        /// </value>
        public string GraduationRequirementDesc { get; set; }
    }

    /// <summary>
    /// IssuingDegree Model
    /// </summary>
    public class IssuingDegree
    {
        /// <summary>
        /// Gets or sets the excemption prof examination date.
        /// </summary>
        /// <value>
        /// The excemption prof examination date.
        /// </value>
        public string ExemptionProfExaminationDate { get; set; }

        /// <summary>
        /// Gets or sets the graduation catalog.
        /// </summary>
        /// <value>
        /// The graduation catalog.
        /// </value>
        public List<GraduationRequirementCatalog> GraduationCatalog { get; set; }

        /// <summary>
        /// Gets or sets the graduation requirement code.
        /// </summary>
        /// <value>
        /// The graduation requirement code.
        /// </value>
        public int GraduationRequirementCode { get; set; }

        /// <summary>
        /// Gets or sets the graduation requirement desc.
        /// </summary>
        /// <value>
        /// The graduation requirement desc.
        /// </value>
        public string GraduationRequirementDesc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has service.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has service; otherwise, <c>false</c>.
        /// </value>
        public bool HasService { get; set; }

        /// <summary>
        /// Gets or sets the issuing date.
        /// </summary>
        /// <value>
        /// The issuing date.
        /// </value>
        public string IssuingDate { get; set; }

        /// <summary>
        /// Gets or sets the legal base code.
        /// </summary>
        /// <value>
        /// The legal base code.
        /// </value>
        public int? LegalBaseCode { get; set; }

        /// <summary>
        /// Gets or sets the legal base desc.
        /// </summary>
        /// <value>
        /// The legal base desc.
        /// </value>
        public string LegalBaseDesc { get; set; }

        /// <summary>
        /// Gets or sets the prof examination date.
        /// </summary>
        /// <value>
        /// The prof examination date.
        /// </value>
        public string ProfExaminationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether /[show service].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show service]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowService { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the state catalog.
        /// </summary>
        /// <value>
        /// The state catalog.
        /// </value>
        public List<StateCatalog> StateCatalog { get; set; }

        /// <summary>
        /// Gets or sets the state catalog bs.
        /// </summary>
        /// <value>
        /// The state catalog bs.
        /// </value>
        public List<StateCatalog> StateCatalogBS { get; set; }

        /// <summary>
        /// Gets or sets the state code.
        /// </summary>
        /// <value>
        /// The state code.
        /// </value>
        public string StateCode { get; set; }

        /// <summary>
        /// Gets or sets the state desc.
        /// </summary>
        /// <value>
        /// The state desc.
        /// </value>
        public string StateDesc { get; set; }
    }

    /// <summary>
    /// State Catalog
    /// </summary>
    public class StateCatalog
    {
        /// <summary>
        /// Gets or sets the state code.
        /// </summary>
        /// <value>
        /// The state code.
        /// </value>
        public string StateCode { get; set; }

        /// <summary>
        /// Gets or sets the state desc.
        /// </summary>
        /// <value>
        /// The state desc.
        /// </value>
        public string StateDesc { get; set; }
    }
}