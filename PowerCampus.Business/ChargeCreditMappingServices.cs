// --------------------------------------------------------------------
// <copyright file="ChargeCreditServices.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.BusinessInterfaces;
using PowerCampus.DataAccess;
using PowerCampus.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PowerCampus.Business
{
    /// <summary>
    /// ChargeCreditServices
    /// </summary>
    /// <seealso cref="IChargeCreditMappingServices" />
    public class ChargeCreditMappingServices : IChargeCreditMappingServices
    {
        /// <summary>
        /// The catalog da
        /// </summary>
        private readonly CatalogDA _catalogDA;

        /// <summary>
        /// The charge credit mapping da
        /// </summary>
        private readonly ChargeCreditMappingDA _chargeCreditMappingDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceiverServices"/> class.
        /// </summary>
        public ChargeCreditMappingServices()
        {
            _chargeCreditMappingDA = new ChargeCreditMappingDA();
            _catalogDA = new CatalogDA();
        }

        /// <summary>
        /// Deletes the charge credit mapping.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public int DeleteChargeCreditMapping(int id)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "DeleteChargeCreditMapping", "DeleteChargeCreditMapping starts");
            int deleteId = _chargeCreditMappingDA.DeleteChargeCreditMapping(id);
            // LoggerHelper.LogWebError("FiscalRecords", "DeleteChargeCreditMapping", "DeleteChargeCreditMapping ends");
            return deleteId;
        }

        /// <summary>
        /// Gets the mapping product service.
        /// </summary>
        /// <returns></returns>
        public List<ChargeCreditMapping> GetChargeCreditMapping()
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetChargeCreditMapping", "GetChargeCreditMapping starts");
            List<ChargeCreditMapping> result = _chargeCreditMappingDA.GetChargeCreditMapping();
            // LoggerHelper.LogWebError("FiscalRecords", "GetChargeCreditMapping", "GetChargeCreditMapping ends");
            return result?.OrderBy(m => m.ChargeCreditDesc).ToList();
        }

        /// <summary>
        /// Gets the charge credits with taxes.
        /// </summary>
        /// <param name="chargeCreditCode">The charge credit code.</param>
        /// <returns></returns>
        public List<ChargeCreditWithTaxes> GetChargeCreditsWithTaxes(string chargeCreditCode)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetChargeCreditTaxes", "GetChargeCreditTaxes starts");
            List<ChargeCreditWithTaxes> chargeCreditWithTaxes = new List<ChargeCreditWithTaxes>();

            List<FiscalRecordCatalog> chargeCreditCatalog = _catalogDA.GetChargeCreditCatalogNonTax(chargeCreditCode);
            if (chargeCreditCatalog?.Count > 0)
            {
                List<ChargeCreditMapping> chargeCreditMappings = _chargeCreditMappingDA.GetChargeCreditMapping() ?? new List<ChargeCreditMapping>();
                foreach (FiscalRecordCatalog chargeCredit in chargeCreditCatalog)
                {
                    if (chargeCreditMappings.FindIndex(ccm => ccm.ChargeCreditCodeId == chargeCredit.Id) < 0)
                    {
                        chargeCreditWithTaxes.Add(new ChargeCreditWithTaxes()
                        {
                            ChargeCreditTaxes = _chargeCreditMappingDA.GetChargeCreditTaxes(chargeCredit.Code)?.OrderBy(m => m.Description).ToList(),
                            Code = chargeCredit.Code,
                            Description = chargeCredit.Description,
                            Id = chargeCredit.Id
                        });
                    }
                }
            }
            // LoggerHelper.LogWebError("FiscalRecords", "GetChargeCreditTaxes", "GetChargeCreditTaxes ends");
            return chargeCreditWithTaxes;
        }

        /// <summary>
        /// Gets the special iva tax.
        /// </summary>
        /// <returns></returns>
        public List<ChargeCreditTaxes> GetSpecialIvaTax()
        {
            List<ChargeCreditTaxes> result = _chargeCreditMappingDA.GetSpecialIvaTax();
            return result?.OrderBy(m => m.Description).ToList();
        }

        /// <summary>
        /// Saves the charge credit mappings.
        /// </summary>
        /// <param name="chargeCreditMappings">The charge credit mappings.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public bool SaveChargeCreditMappings(List<ChargeCreditMapping> chargeCreditMappings, string userName)
        {
            bool result = false;
            // LoggerHelper.LogWebError("FiscalRecords", "SaveChargeCreditMapping", "SaveChargeCreditMapping starts");
            List<ChargeCreditMapping> chargeCreditMappingsSaved = _chargeCreditMappingDA.GetChargeCreditMapping();

            if (chargeCreditMappings?.Count > 0)
            {
                foreach (ChargeCreditMapping chargeCreditMapping in chargeCreditMappings)
                {
                    if (chargeCreditMappingsSaved == null || chargeCreditMappingsSaved.FindIndex(saved => saved.ChargeCreditCodeId == chargeCreditMapping.ChargeCreditCodeId) < 0)
                    {
                        _ = _chargeCreditMappingDA.SaveChargeCreditMapping(chargeCreditMapping, userName);
                        if (!string.IsNullOrEmpty(chargeCreditMapping.TaxCode))
                        {
                            ChargeCreditTaxes chargeCreditTaxes = new ChargeCreditTaxes()
                            {
                                ChargeCreditCodeId = chargeCreditMapping.ChargeCreditCodeId,
                                Description = chargeCreditMapping.TaxDescription,
                                FactorType = chargeCreditMapping.FactorType,
                                TaxCode = chargeCreditMapping.TaxCode,
                                TaxRate = string.IsNullOrEmpty(chargeCreditMapping.TaxRate) ? (decimal?)null : decimal.Parse(chargeCreditMapping.TaxRate)
                            };
                            _ = _chargeCreditMappingDA.SaveChargeTaxMapping(chargeCreditTaxes, userName);
                        }
                    }
                }
                result = true;
            }
            // LoggerHelper.LogWebError("FiscalRecords", "SaveChargeCreditMapping", "SaveChargeCreditMapping ends");
            return result;
        }
    }
}