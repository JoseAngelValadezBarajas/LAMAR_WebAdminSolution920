// --------------------------------------------------------------------
// <copyright file="ICatalogServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces
{
    /// <summary>
    /// ICatalogServices Interface
    /// </summary>
    public interface ICatalogServices
    {
        List<FiscalRecordCatalog> GetStates();
        /// <summary>
        /// Gets all charge credits.
        /// </summary>
        /// <returns></returns>
        List<ChargeCredit> GetAllChargeCredits();

        /// <summary>
        /// Gets all record types.
        /// </summary>
        /// <returns></returns>
        List<RecordType> GetAllRecordTypes();

        /// <summary>
        /// Gets the cfdi usage.
        /// </summary>
        /// <returns></returns>
        List<CFDIUsageCatalog> GetCFDIUsage();

        /// <summary>
        /// Gets the charge credit catalog non tax.
        /// </summary>
        /// <param name="codeValueKey">The code value key.</param>
        /// <returns></returns>
        List<FiscalRecordCatalog> GetChargeCreditCatalogNonTax(string codeValueKey);

        /// <summary>
        /// Gets the fiscal record cancel reason.
        /// </summary>
        /// <param name="applyToIndividual">if set to <c>true</c> [apply to individual].</param>
        /// <param name="applyToGlobal">if set to <c>true</c> [apply to global].</param>
        /// <returns></returns>
        List<FiscalRecordCancelReason> GetFiscalRecordCancelReason(bool applyToIndividual, bool applyToGlobal);

        /// <summary>
        /// Gets the fiscal record catalog.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        List<FiscalRecordCatalog> GetFiscalRecordCatalog(Catalog name);

        /// <summary>
        /// Gets the fiscal record catalog by attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fiscalRecordCatalog">The fiscal record catalog.</param>
        /// <returns></returns>
        List<FiscalRecordCatalog> GetFiscalRecordCatalogByAttribute(Catalog name, FiscalRecordCatalog fiscalRecordCatalog);

        /// <summary>
        /// Gets the fiscal record catalog by key.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fiscalRecordCatalog">The fiscal record catalog.</param>
        /// <returns></returns>
        List<FiscalRecordCatalog> GetFiscalRecordCatalogByKey(Catalog name, FiscalRecordCatalog fiscalRecordCatalog);

        /// <summary>
        /// Gets the reciept charge codes.
        /// </summary>
        /// <returns></returns>
        List<FiscalRecordCatalog> GetRecieptChargeCodes();

        /// <summary>
        /// Gets the tax regimen.
        /// </summary>
        /// <returns></returns>
        List<TaxRegimenCatalog> GetTaxRegimen();
    }
}