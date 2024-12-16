// --------------------------------------------------------------------
// <copyright file="SearchResult.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// SearchResult Entity
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gets or sets the full name.
        ///     Full name for People
        ///     Organization name for Organization
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        ///     PersonId for People
        ///     OrganizationId for Organization
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the organization list.
        /// </summary>
        /// <value>
        /// The organization list.
        /// </value>
        public List<Organization> OrganizationList { get; set; }

        /// <summary>
        /// Gets or sets the people list.
        /// </summary>
        /// <value>
        /// The people list.
        /// </value>
        public List<People> PeopleList { get; set; }

        /// <summary>
        /// Gets or sets the code identifier.
        /// </summary>
        /// <value>
        /// The code identifier.
        /// </value>
        public string PeopleOrgCodeId { get; set; }

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
        /// Gets or sets the record type list.
        /// </summary>
        /// <value>
        /// The record type list.
        /// </value>
        public List<string> RecordTypeList { get; set; }

        /// <summary>
        /// Gets or sets the record type list full.
        /// </summary>
        /// <value>
        /// The record type list full.
        /// </value>
        public List<RecordType> RecordTypeListFull { get; set; }
    }
}