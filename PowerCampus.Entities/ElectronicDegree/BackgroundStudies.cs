// --------------------------------------------------------------------
// <copyright file="BackgroundStudies.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

namespace PowerCampus.Entities.ElectronicDegree
{
    public class BackgroundStudies
    {
        public List<BackgroundStudyCatalog> BackgroundStudiesCatalog { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the institution name origin.
        /// </summary>
        /// <value>
        /// The institution name origin.
        /// </value>
        public string InstitutionNameOrigin { get; set; }

        /// <summary>
        /// Gets or sets the licence number.
        /// </summary>
        /// <value>
        /// The licence number.
        /// </value>
        public string LicenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the state code.
        /// </summary>
        /// <value>
        /// The state code.
        /// </value>
        public int? StateCode { get; set; }

        /// <summary>
        /// Gets or sets the state desc.
        /// </summary>
        /// <value>
        /// The state desc.
        /// </value>
        public string StateDesc { get; set; }
        public string LevelStudy { get; set; } //mod addv 13082024
    }

    public class BackgroundStudyCatalog
    {
        /// <summary>
        /// Gets or sets the background study type code.
        /// </summary>
        /// <value>
        /// The background study type code.
        /// </value>
        public int BackgroundStudyTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the background study type desc.
        /// </summary>
        /// <value>
        /// The background study type desc.
        /// </value>
        public string BackgroundStudyTypeDesc { get; set; }
    }
}