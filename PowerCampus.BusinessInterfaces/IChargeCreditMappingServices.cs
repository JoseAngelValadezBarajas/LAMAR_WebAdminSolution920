// --------------------------------------------------------------------
// <copyright file="IChargeCreditServices.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces
{
    /// <summary>
    /// IChargeCreditServices
    /// </summary>
    public interface IChargeCreditMappingServices
    {
        /// <summary>
        /// Deletes the charge credit mapping.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        int DeleteChargeCreditMapping(int id);

        /// <summary>
        /// Gets the mapping product service.
        /// </summary>
        /// <returns></returns>
        List<ChargeCreditMapping> GetChargeCreditMapping();

        /// <summary>
        /// Gets the charge credits with taxes.
        /// </summary>
        /// <param name="chargeCreditCode">The charge credit code.</param>
        /// <returns></returns>
        List<ChargeCreditWithTaxes> GetChargeCreditsWithTaxes(string chargeCreditCode);

        /// <summary>
        /// Gets the special iva tax.
        /// </summary>
        /// <returns></returns>
        List<ChargeCreditTaxes> GetSpecialIvaTax();

        /// <summary>
        /// Saves the charge credit mappings.
        /// </summary>
        /// <param name="chargeCreditMappings">The charge credit mappings.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        bool SaveChargeCreditMappings(List<ChargeCreditMapping> chargeCreditMappings, string userName);
    }
}