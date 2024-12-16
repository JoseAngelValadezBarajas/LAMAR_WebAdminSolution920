// --------------------------------------------------------------------
// <copyright file="OrganizationMapper.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using WebAdminUI.Helpers;
using WebAdminUI.Models.FiscalRecords;
using WebAdminUI.Models.Organization;

namespace WebAdminUI.OrganizationMappers
{
    /// <summary>
    /// Mapper for organization view models
    /// </summary>
    internal static class OrganizationMapper
    {
        /// <summary>
        /// To the organization.
        /// </summary>
        /// <param name="organizationViewModel">The organization view model.</param>
        /// <returns></returns>
        internal static Organization ToDataEntity(this OrganizationViewModel organizationViewModel)
        {
            var organization = new Organization
            {
                OrganizationCodeId = organizationViewModel.OrganizationCodeId.Replace("-", string.Empty),
                FiscalRecordsDefault = new FiscalRecordDefaults
                {
                    TaxpayerId = organizationViewModel.TaxpayerId,
                    Email = organizationViewModel.Email,
                    CFDIUsageCode = organizationViewModel.CFDIUsageCode,
                    CFDIUsageDesc = organizationViewModel.CFDIUsageDesc
                }
            };
            return organization;
        }

        /// <summary>
        /// To the organization view model.
        /// </summary>
        /// <param name="organization">The organization.</param>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        internal static OrganizationViewModel ToViewModel(this Organization organization, Account account)
        {
            var organizationViewModel = new OrganizationViewModel();
            if (organization != null)
            {
                organizationViewModel.OrganizationCodeId = PowerCampusSystemFormat.FormatPeopleCodeId(account.SearchPeopleOrgCodeId);
                organizationViewModel.OrganizationName = organization.OrganizationName;
                organizationViewModel.YearTermSession = organization.YearTermSession;

                if (organization.FiscalRecordsDefault != null)
                {
                    organizationViewModel.Email = organization.FiscalRecordsDefault.Email;
                    organizationViewModel.TaxpayerId = organization.FiscalRecordsDefault.TaxpayerId;
                    organizationViewModel.CorporateName = organization.FiscalRecordsDefault.CorporateName;
                    organizationViewModel.CFDIUsageCode = organization.FiscalRecordsDefault.CFDIUsageCode;
                    organizationViewModel.CFDIUsageDesc = organization.FiscalRecordsDefault.CFDIUsageDesc;
                    organizationViewModel.CFDIList = organization.FiscalRecordsDefault.CFDICatalog;
                }
                else
                    organizationViewModel.CFDIList = new List<CFDIUsageCatalog>();
            }
            return organizationViewModel;
        }

        /// <summary>
        /// To the organization view charges.
        /// </summary>
        /// <param name="organization">The organization.</param>
        /// <param name="account">The user account.</param>
        /// <returns></returns>
        internal static OrganizationViewModel ToViewModelWithCharges(this Organization organization, Account account)
        {
            var organizationViewModel = new OrganizationViewModel
            {
                OrganizationChargeCredit = new List<ChargeCreditViewModel>()
            };
            if (organization?.OrganizationChargeCredit.Count > 0)
            {
                decimal TotalTaxes = 0;
                DateTime entryDates;
                foreach (ChargeCredit charges in organization.OrganizationChargeCredit)
                {
                    TotalTaxes = (decimal)charges.TotalTaxes;
                    entryDates = DateTime.Parse(charges.EntryDate, CultureInfo.InvariantCulture);

                    organizationViewModel.OrganizationChargeCredit.Add(new ChargeCreditViewModel
                    {
                        EntryDate = entryDates.ToString(account.reportFormats.DateFormat),
                        ChargeNumberSource = charges.ChargeNumberSource,
                        ChargeCreditCode = charges.ChargeCreditCode,
                        ChargeCreditDesc = charges.ChargeCreditDesc,
                        UnitAmount = charges.UnitAmount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        TotalTaxes = TotalTaxes.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        TotalUnit = charges.TotalUnit.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        CanCreatePPD = charges.CanCreatePPD
                    });
                }
            }

            return organizationViewModel;
        }

        /// <summary>
        /// To the view model year term session.
        /// </summary>
        /// <param name="organization">The organization.</param>
        /// <returns></returns>
        internal static OrganizationViewModel ToViewModelYearTermSession(this Organization organization)
        {
            OrganizationViewModel organizationViewModel = new OrganizationViewModel();

            if (organization != null)
                organizationViewModel.YearTermSession = organization.YearTermSession;
            else
                organizationViewModel.YearTermSession = new List<YTS>();

            return organizationViewModel;
        }
    }
}