// --------------------------------------------------------------------
// <copyright file="InstitutionCampus.cs" company="Ellucian">
//     Copyright 2020-2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

namespace PowerCampus.Entities.ElectronicCertificate
{
    /// <summary>
    /// InstitutionCampus Class
    /// </summary>
    public class InstitutionCampus
    {
        /// <summary>
        /// Gets or sets the campus code identifier.
        /// </summary>
        /// <value>
        /// The campus code identifier.
        /// </value>
        public string CampusCodeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the campus.
        /// </summary>
        /// <value>
        /// The name of the campus.
        /// </value>
        public string CampusName { get; set; }

        /// <summary>
        /// Gets or sets the campus sep code.
        /// </summary>
        /// <value>
        /// The campus sep code.
        /// </value>
        public string CampusSepCode { get; set; }

        /// <summary>
        /// Gets or sets the federal entity identifier.
        /// </summary>
        /// <value>
        /// The federal entity identifier.
        /// </value>
        public int? FederalEntityId { get; set; }

        /// <summary>
        /// Gets or sets the folio format.
        /// </summary>
        /// <value>
        /// The folio format.
        /// </value>
        public string FolioFormat { get; set; }

        /// <summary>
        /// Gets or sets the institution campus identifier.
        /// </summary>
        /// <value>
        /// The institution campus identifier.
        /// </value>
        public int? InstitutionCampusId { get; set; }

        /// <summary>
        /// Gets or sets the institution code identifier.
        /// </summary>
        /// <value>
        /// The institution code identifier.
        /// </value>
        public string InstitutionCodeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the institution.
        /// </summary>
        /// <value>
        /// The name of the institution.
        /// </value>
        public string InstitutionName { get; set; }

        /// <summary>
        /// Gets or sets the sep identifier.
        /// </summary>
        /// <value>
        /// The sep identifier.
        /// </value>
        public string InstitutionSepId { get; set; }

        /// <summary>
        /// Gets or sets the responsible identifier.
        /// </summary>
        /// <value>
        /// The responsible identifier.
        /// </value>
        public int? ResponsibleId { get; set; }

        /// <summary>
        /// Gets or sets the signing institution identifier.
        /// </summary>
        /// <value>
        /// The signing institution identifier.
        /// </value>
        public string SigningInstitutionId { get; set; }
    }

    /// <summary>
    /// InstitutionCampuses
    /// </summary>
    public class InstitutionCampuses
    {
        /// <summary>
        /// Gets or sets the institution campus.
        /// </summary>
        /// <value>
        /// The institution campus.
        /// </value>
        public List<InstitutionCampus> InstitutionCampus { get; set; }

        /// <summary>
        /// Gets or sets the issuing place.
        /// </summary>
        /// <value>
        /// The issuing place.
        /// </value>
        public List<CodeTable> IssuingPlace { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public string OperatorId { get; set; }
    }
}