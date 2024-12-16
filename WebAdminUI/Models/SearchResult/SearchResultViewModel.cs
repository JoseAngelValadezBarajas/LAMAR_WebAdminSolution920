// --------------------------------------------------------------------
// <copyright file="PeopleViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Models.Organization;
using WebAdminUI.Models.People;

namespace WebAdminUI.Models.SearchResult
{
    /// <summary>
    /// SearchResultViewModel Class
    /// </summary>
    public class SearchResultViewModel
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
        public List<OrganizationViewModel> OrganizationList { get; set; }

        /// <summary>
        /// Gets or sets the people list.
        /// </summary>
        /// <value>
        /// The people list.
        /// </value>
        public List<PeopleViewModel> PeopleList { get; set; }

        /// <summary>
        /// Gets or sets the code identifier.
        /// </summary>
        /// <value>
        /// The code identifier.
        /// </value>
        [Display(Name = "lblPeopleId", ResourceType = typeof(Views.Search.App_LocalResources.Advanced_cshtml))]
        public string PeopleOrgCodeId { get; set; }

        /// <summary>
        /// Gets or sets the primary email.
        /// </summary>
        /// <value>
        /// The primary email.
        /// </value>
        [Display(Name = "lblEmail", ResourceType = typeof(Views.Search.App_LocalResources.Advanced_cshtml))]
        public string PrimaryEmail { get; set; }

        /// <summary>
        /// Gets or sets the record type list.
        /// </summary>
        /// <value>
        /// The record type list.
        /// </value>
        [Display(Name = "lblRecordType", ResourceType = typeof(Views.Search.App_LocalResources.Advanced_cshtml))]
        public string RecordTypeList { get; set; }

        /// <summary>
        /// Gets or sets the record type list.
        /// </summary>
        /// <value>
        /// The record type list.
        /// </value>
        public List<RecordTypeViewModel> RecordTypeListFull { get; set; }
    }
}