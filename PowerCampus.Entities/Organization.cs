// --------------------------------------------------------------------
// <copyright file="Organization.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// Organization
    /// </summary>
    public class Organization
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the fiscal records default.
        /// </summary>
        /// <value>
        /// The fiscal records default.
        /// </value>
        public FiscalRecordDefaults FiscalRecordsDefault { get; set; }

        /// <summary>
        /// Gets or sets the charge credit.
        /// </summary>
        /// <value>
        /// The charge credit.
        /// </value>
        public List<ChargeCredit> OrganizationChargeCredit { get; set; }

        /// <summary>
        /// Gets or sets the organization code identifier.
        /// </summary>
        /// <value>
        /// The organization code identifier.
        /// </value>
        public string OrganizationCodeId { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the name of the organization.
        /// </summary>
        /// <value>
        /// The name of the organization.
        /// </value>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Gets or sets the type of the record.
        /// </summary>
        /// <value>
        /// The type of the record.
        /// </value>
        public string RecordType { get; set; }

        /// <summary>
        /// Gets or sets the year term session.
        /// </summary>
        /// <value>
        /// The year term session.
        /// </value>
        public List<YTS> YearTermSession { get; set; }
    }
}