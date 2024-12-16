// --------------------------------------------------------------------
// <copyright file="People.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// People
    /// </summary>
    public class People
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the fiscal records default.
        /// </summary>
        /// <value>
        /// The fiscal records default.
        /// </value>
        public FiscalRecordDefaults FiscalRecordsDefault { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the charge credit.
        /// </summary>
        /// <value>
        /// The charge credit.
        /// </value>
        public List<ChargeCredit> PeopleChargeCredit { get; set; }

        /// <summary>
        /// Gets or sets the people code identifier.
        /// </summary>
        /// <value>
        /// The people code identifier.
        /// </value>
        public string PeopleCodeId { get; set; }

        /// <summary>
        /// Gets or sets the person identifier.
        /// </summary>
        /// <value>
        /// The person identifier.
        /// </value>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the primary email.
        /// </summary>
        /// <value>
        /// The primary email.
        /// </value>
        public string PrimaryEmail { get; set; }

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

    /// <summary>
    /// PeopleCharges
    /// </summary>
    public class PeopleOrgCharges
    {
        /// <summary>
        /// Gets or sets the code identifier.
        /// </summary>
        /// <value>
        /// The code identifier.
        /// </value>
        public string CodeId { get; set; }

        /// <summary>
        /// Gets or sets the yts.
        /// </summary>
        /// <value>
        /// The yts.
        /// </value>
        public string YTS { get; set; }
    }
}