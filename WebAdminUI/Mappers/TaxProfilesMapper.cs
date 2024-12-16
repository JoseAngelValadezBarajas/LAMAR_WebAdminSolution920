// --------------------------------------------------------------------
// <copyright file="TaxProfilesMapper.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using System.Collections.Generic;
using WebAdminUI.Models.TaxProfiles;

namespace WebAdminUI.TaxProfilesMappers
{
    /// <summary>
    /// Mapper for tax profiles view models
    /// </summary>
    internal static class TaxProfilesMapper
    {
        /// <summary>
        /// Fiscals the record tax mapping.
        /// </summary>
        /// <param name="taxProfileDetailViewModel">The tax profile detail view model.</param>
        /// <returns></returns>
        internal static FiscalRecordTaxMapping ToDataEntity(this TaxProfileDetailViewModel taxProfileDetailViewModel)
        {
            var taxMapping = new FiscalRecordTaxMapping
            {
                FiscalRecordTaxMappingId = taxProfileDetailViewModel.FiscalRecordTaxMappingId,
                TaxProfileDetailId = taxProfileDetailViewModel.TaxProfileDetailId,
                TaxCode = taxProfileDetailViewModel.TaxCode,
                TaxRate = taxProfileDetailViewModel.TaxRate,
                FactorType = taxProfileDetailViewModel.FactorType
            };
            return taxMapping;
        }

        /// <summary>
        /// To the tax profile ListView model.
        /// </summary>
        /// <param name="taxProfile">The tax profile.</param>
        /// <param name="account">The user account.</param>
        /// <returns></returns>
        internal static TaxProfileListViewModel ToViewModel(this TaxProfile taxProfile, Account account)
        {
            var taxProfileListViewModel = new TaxProfileListViewModel
            {
                TaxProfileId = taxProfile.TaxProfileId,
                TaxProfileName = taxProfile.Name,
                TaxProfileDescription = taxProfile.Description,
                TaxProfileValidities = new List<TaxProfileValidityViewModel>()
            };

            if (taxProfile.TaxProfileValidityList != null)
            {
                foreach (TaxProfileValidity item in taxProfile.TaxProfileValidityList)
                {
                    taxProfileListViewModel.TaxProfileValidities.Add(new TaxProfileValidityViewModel
                    {
                        Id = item.TaxProfileValidityId,
                        ValidityFromTo = item.StartDateTime.ToString(account.reportFormats.DateFormat) + " - " +
                                (item.EndDateTime.HasValue ? item.EndDateTime.Value.ToString(account.reportFormats.DateFormat) : string.Empty)
                    });
                }
            }
            return taxProfileListViewModel;
        }

        /// <summary>
        /// To the tax profile detail view model.
        /// </summary>
        /// <param name="taxProfileDetail">The tax profile detail.</param>
        /// <returns></returns>
        internal static TaxProfileDetailViewModel ToViewModel(this TaxProfileDetail taxProfileDetail)
        {
            var taxProfileListViewModel = new TaxProfileDetailViewModel
            {
                FiscalRecordTaxMappingId = taxProfileDetail.TaxMapping.FiscalRecordTaxMappingId,
                TaxProfileDetailId = taxProfileDetail.TaxProfileDetailId,
                Sequence = taxProfileDetail.Sequence,
                ChargeCreditDescription = taxProfileDetail.ChargeCreditDescription,
                Percentage = taxProfileDetail.Percentage,
                TaxDescription = taxProfileDetail.TaxMapping.TaxDescription,
                TaxRate = taxProfileDetail.TaxMapping.TaxRate,
                TaxCode = taxProfileDetail.TaxMapping.TaxCode
            };
            return taxProfileListViewModel;
        }
    }
}