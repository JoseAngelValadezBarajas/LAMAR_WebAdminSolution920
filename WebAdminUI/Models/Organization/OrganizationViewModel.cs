// --------------------------------------------------------------------
// <copyright file="OrganizationViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Helpers;
using WebAdminUI.Models.FiscalRecords;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.Organization
{
    /// <summary>
    /// OrganizationViewModel
    /// </summary>
    public class OrganizationViewModel
    {
        /// <summary>
        /// Gets or sets the cfdi list.
        /// </summary>
        /// <value>
        /// The cfdi list.
        /// </value>
        public List<CFDIUsageCatalog> CFDIList { get; set; }

        /// <summary>
        /// Gets or sets the cfdi usage code.
        /// </summary>
        /// <value>
        /// The cfdi usage code.
        /// </value>
        public string CFDIUsageCode { get; set; }

        /// <summary>
        /// Gets or sets the cfdi usage desc.
        /// </summary>
        /// <value>
        /// The cfdi usage desc.
        /// </value>
        [Display(Name = nameof(OrganizationViewModelResource.CFDI), ResourceType = typeof(OrganizationViewModelResource))]
        public string CFDIUsageDesc { get; set; }

        /// <summary>
        /// Gets or sets the name of the corporate.
        /// </summary>
        /// <value>
        /// The name of the corporate.
        /// </value>
        [Display(Name = nameof(OrganizationViewModelResource.CorporateName), ResourceType = typeof(OrganizationViewModelResource))]
        public string CorporateName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [EmailAddress(ErrorMessageResourceType = typeof(OrganizationViewModelResource), ErrorMessageResourceName = "ValidationMessageEmail", ErrorMessage = null)]
        [Display(Name = nameof(OrganizationViewModelResource.Email), ResourceType = typeof(OrganizationViewModelResource))]
        [CustomEmailRegularExpression]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the charge credit.
        /// </summary>
        /// <value>
        /// The charge credit.
        /// </value>
        public List<ChargeCreditViewModel> OrganizationChargeCredit { get; set; }

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
        /// Gets or sets the taxpayer identifier.
        /// </summary>
        /// <value>
        /// The taxpayer identifier.
        /// </value>
        [Display(Name = nameof(OrganizationViewModelResource.TaxpayerId), ResourceType = typeof(OrganizationViewModelResource))]
        public string TaxpayerId { get; set; }

        /// <summary>
        /// Gets or sets the year term session.
        /// </summary>
        /// <value>
        /// The year term session.
        /// </value>
        public List<YTS> YearTermSession { get; set; }
    }
}