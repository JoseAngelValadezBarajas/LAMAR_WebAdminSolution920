// --------------------------------------------------------------------
// <copyright file="IFiscalRecordServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces
{
    /// <summary>
    /// ITaxProfileRateServices Interface
    /// </summary>
    public interface ITaxProfileServices
    {
        /// <summary>
        /// Gets the tax profile detail list.
        /// </summary>
        /// <param name="validityId">The validity identifier.</param>
        /// <returns></returns>
        List<TaxProfileDetail> GetTaxProfileDetails(int validityId);

        /// <summary>
        /// Gets the tax profile list.
        /// </summary>
        /// <returns></returns>
        List<TaxProfile> GetTaxProfileList();

        /// <summary>
        /// Gets the tax profile validity list.
        /// </summary>
        /// <returns></returns>
        List<TaxProfileValidity> GetTaxProfileValidityList(int taxProfileId);

        /// <summary>
        /// Saves the tax mapping.
        /// </summary>
        /// <param name="fiscalRecordTaxMapping">The fiscal record tax mapping.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        int SaveTaxMapping(FiscalRecordTaxMapping fiscalRecordTaxMapping, string userName);
    }
}