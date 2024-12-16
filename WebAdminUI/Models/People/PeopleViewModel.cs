// --------------------------------------------------------------------
// <copyright file="PeopleViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Helpers;
using WebAdminUI.Models.FiscalRecords;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.People
{
    /// <summary>
    /// PeopleViewModel class
    /// </summary>
    public class PeopleViewModel
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
        [Display(Name = nameof(PeopleViewModelResource.CFDI), ResourceType = typeof(PeopleViewModelResource))]
        public string CFDIUsageDesc { get; set; }

        /// <summary>
        /// Gets or sets the name of the corporate.
        /// </summary>
        /// <value>
        /// The name of the corporate.
        /// </value>
        [Display(Name = nameof(PeopleViewModelResource.CorporateName), ResourceType = typeof(PeopleViewModelResource))]
        public string CorporateName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the invoice preferred taxpayer identifier.
        /// </summary>
        /// <value>
        /// The invoice preferred taxpayer identifier.
        /// </value>
        public int? InvoicePreferredTaxpayerId { get; set; }

        /// <summary>
        /// Gets or sets the invoice taxpayer identifier.
        /// </summary>
        /// <value>
        /// The invoice taxpayer identifier.
        /// </value>
        public int? InvoiceTaxpayerId { get; set; }

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
        public List<ChargeCreditViewModel> PeopleChargeCredit { get; set; }

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
        /// Gets or sets the receiver email.
        /// </summary>
        /// <value>
        /// The receiver email.
        /// </value>
        [EmailAddress(ErrorMessageResourceType = typeof(PeopleViewModelResource), ErrorMessageResourceName = "ValidationMessageEmail", ErrorMessage = null)]
        [Display(Name = nameof(PeopleViewModelResource.Email), ResourceType = typeof(PeopleViewModelResource))]
        [CustomEmailRegularExpression]
        public string ReceiverEmail { get; set; }

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
        [Display(Name = nameof(PeopleViewModelResource.TaxpayerId), ResourceType = typeof(PeopleViewModelResource))]
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