// --------------------------------------------------------------------
// <copyright file="TaxProfileRateServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.BusinessInterfaces;
using PowerCampus.DataAccess;
using PowerCampus.Entities;
using System.Collections.Generic;

namespace PowerCampus.Business
{
    /// <summary>
    /// TaxProfileServices
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.ITaxProfileServices" />
    public class TaxProfileServices : ITaxProfileServices
    {
        /// <summary>
        /// Gets the tax profile detail list.
        /// </summary>
        /// <param name="validityId">The validity identifier.</param>
        /// <returns></returns>
        public List<TaxProfileDetail> GetTaxProfileDetails(int validityId)
        {
            // _logService.Debug("GetTaxProfileDetails starts");
            var taxProfileRateDA = new TaxProfileDA();
            List<TaxProfileDetail> lstTaxProfileDetails = taxProfileRateDA.GetTaxProfileDetails(validityId);
            // _logService.Debug("GetTaxProfileDetails ends");
            return lstTaxProfileDetails;
        }

        /// <summary>
        /// Gets the tax profile list.
        /// </summary>
        /// <returns></returns>
        public List<TaxProfile> GetTaxProfileList()
        {
            // _logService.Debug("GetTaxProfileList starts");
            var taxProfileRateDA = new TaxProfileDA();
            var taxProfiles = taxProfileRateDA.GetTaxProfileList();
            foreach (var item in taxProfiles)
            {
                item.TaxProfileValidityList = GetTaxProfileValidityList(item.TaxProfileId);
            }
            // _logService.Debug("GetTaxProfileList ends");
            return taxProfiles;
        }

        /// <summary>
        /// Gets the tax profile validity list.
        /// </summary>
        /// <param name="taxProfileId">The tax profile identifier.</param>
        /// <returns></returns>
        public List<TaxProfileValidity> GetTaxProfileValidityList(int taxProfileId)
        {
            // _logService.Debug("GetTaxProfileValidityList starts");
            var da = new TaxProfileDA();
            List<TaxProfileValidity> lstTaxProfileValidity = da.GetTaxProfileValidities(taxProfileId);
            // _logService.Debug("GetTaxProfileValidityList ends");
            return lstTaxProfileValidity;
        }

        /// <summary>
        /// Saves the tax mapping.
        /// </summary>
        /// <param name="fiscalRecordTaxMapping">The fiscal record tax mapping.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public int SaveTaxMapping(FiscalRecordTaxMapping fiscalRecordTaxMapping, string userName)
        {
            // _logService.Debug("SaveTaxMapping starts");
            var taxProfileDA = new TaxProfileDA();
            int taxMappingId = taxProfileDA.SaveTaxMapping(fiscalRecordTaxMapping, userName);
            // _logService.Debug("SaveTaxMapping ends");
            return taxMappingId;
        }
    }
}