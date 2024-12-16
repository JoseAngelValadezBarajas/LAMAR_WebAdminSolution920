// --------------------------------------------------------------------
// <copyright file="PeopleMapper.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using WebAdminUI.Helpers;
using WebAdminUI.Models.FiscalRecords;
using WebAdminUI.Models.People;

namespace WebAdminUI.PeopleMappers
{
    /// <summary>
    /// Mapper for people view models
    /// </summary>
    internal static class PeopleMapper
    {
        /// <summary>
        /// Converts to dataentity.
        /// </summary>
        /// <param name="peopleViewModel">The people view model.</param>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        internal static People ToDataEntity(this PeopleViewModel peopleViewModel, Account account)
        {
            var people = new People
            {
                PeopleCodeId = account.PeopleOrgCodeId,
                FiscalRecordsDefault = new FiscalRecordDefaults
                {
                    TaxpayerId = peopleViewModel.TaxpayerId,
                    Email = peopleViewModel.ReceiverEmail,
                    CFDIUsageCode = peopleViewModel.CFDIUsageCode,
                    CFDIUsageDesc = peopleViewModel.CFDIUsageDesc
                }
            };
            return people;
        }

        /// <summary>
        /// To the people entity view model.
        /// </summary>
        /// <param name="peopleData">The people data.</param>
        /// <param name="account">The user account.</param>
        /// <returns></returns>
        internal static PeopleViewModel ToViewModel(this People peopleData, Account account)
        {
            var peopleViewModel = new PeopleViewModel();
            if (peopleData != null)
            {
                peopleViewModel.FullName = peopleData.FullName;
                peopleViewModel.PeopleCodeId = PowerCampusSystemFormat.FormatPeopleCodeId(account.SearchPeopleOrgCodeId);
                peopleViewModel.YearTermSession = peopleData.YearTermSession;

                if (peopleData.FiscalRecordsDefault != null)
                {
                    peopleViewModel.InvoicePreferredTaxpayerId = peopleData.FiscalRecordsDefault.InvoicePreferredTaxpayerId;
                    peopleViewModel.InvoiceTaxpayerId = peopleData.FiscalRecordsDefault.InvoiceTaxpayerId;
                    peopleViewModel.TaxpayerId = peopleData.FiscalRecordsDefault.TaxpayerId;
                    peopleViewModel.ReceiverEmail = peopleData.FiscalRecordsDefault.Email;
                    peopleViewModel.CorporateName = peopleData.FiscalRecordsDefault.CorporateName;
                    peopleViewModel.CFDIUsageCode = peopleData.FiscalRecordsDefault.CFDIUsageCode;
                    peopleViewModel.CFDIList = peopleData.FiscalRecordsDefault.CFDICatalog;
                }
                else
                    peopleViewModel.CFDIList = new List<CFDIUsageCatalog>();
            }

            return peopleViewModel;
        }

        /// <summary>
        /// To the people view charges.
        /// </summary>
        /// <param name="people">The people.</param>
        /// <param name="account">The user account.</param>
        /// <returns></returns>
        internal static PeopleViewModel ToViewModelWithCharges(this People people, Account account)
        {
            var peopleViewModel = new PeopleViewModel
            {
                PeopleChargeCredit = new List<ChargeCreditViewModel>()
            };

            if (people?.PeopleChargeCredit.Count > 0)
            {
                decimal TotalTaxes = 0;
                DateTime entryDates;
                foreach (ChargeCredit charges in people.PeopleChargeCredit)
                {
                    TotalTaxes = (decimal)charges.TotalTaxes;
                    entryDates = DateTime.Parse(charges.EntryDate, CultureInfo.InvariantCulture);

                    peopleViewModel.PeopleChargeCredit.Add(new ChargeCreditViewModel
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

            return peopleViewModel;
        }

        /// <summary>
        /// To the view model year term session.
        /// </summary>
        /// <param name="people">The people.</param>
        /// <returns></returns>
        internal static PeopleViewModel ToViewModelYearTermSession(this People people)
        {
            PeopleViewModel peopleViewModel = new PeopleViewModel();

            if (people != null)
                peopleViewModel.YearTermSession = people.YearTermSession;
            else
                peopleViewModel.YearTermSession = new List<YTS>();

            return peopleViewModel;
        }
    }
}