// --------------------------------------------------------------------
// <copyright file="CatalogServices.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.BusinessInterfaces;
using PowerCampus.DataAccess;
using PowerCampus.Entities;
using PowerCampus.Entities.Enum;
using System.Collections.Generic;
using System.Linq;

namespace PowerCampus.Business
{
    /// <inheritdoc />
    /// <summary>
    /// CatalogServices Class
    /// </summary>
    /// <seealso cref="T:PowerCampus.BusinessInterfaces.ICatalogServices" />
    public class CatalogServices : ICatalogServices
    {
        public List<FiscalRecordCatalog> GetStates()
        {
            //_logService.Debug("GetStates starts");
            var catalogDA = new CatalogDA();
            List<FiscalRecordCatalog> lstObjImp = catalogDA.GetStates();
            //_logService.Debug("GetStates ends");
            return lstObjImp;
        }
        /// <summary>
        /// Gets all charge credits.
        /// </summary>
        /// <returns></returns>
        public List<ChargeCredit> GetAllChargeCredits()
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetAllRecordTypes", "GetAllRecordTypes starts");
            var catalogDA = new CatalogDA();
            List<ChargeCredit> lstChargeCredit = catalogDA.GetAllChargeCredits();
            // LoggerHelper.LogWebError("FiscalRecords", "GetAllRecordTypes", "GetAllRecordTypes ends");
            return lstChargeCredit;
        }

        /// <summary>
        /// Gets all record types.
        /// </summary>
        /// <returns></returns>
        public List<RecordType> GetAllRecordTypes()
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetAllRecordTypes", "GetAllRecordTypes starts");
            var catalogDA = new CatalogDA();
            List<RecordType> lstRecordType = catalogDA.GetAllRecordTypes();
            // LoggerHelper.LogWebError("FiscalRecords", "GetAllRecordTypes", "GetAllRecordTypes ends");
            return lstRecordType;
        }

        /// <summary>
        /// Gets the cfdi usage.
        /// </summary>
        /// <returns></returns>
        public List<CFDIUsageCatalog> GetCFDIUsage()
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetCFDIUsage", "GetCFDIUsage starts");
            var catalogDA = new CatalogDA();
            List<CFDIUsageCatalog> lstCFDIUsageCatalog = catalogDA.GetCFDIUsage();
            // LoggerHelper.LogWebError("FiscalRecords", "GetCFDIUsage", "GetCFDIUsage ends");
            return lstCFDIUsageCatalog;
        }

        /// <summary>
        /// Gets the charge credit catalog non tax.
        /// </summary>
        /// <returns></returns>
        public List<FiscalRecordCatalog> GetChargeCreditCatalogNonTax(string codeValueKey)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetChargeCreditCatalogNonTax", "GetChargeCreditCatalogNonTax starts");
            var catalogDA = new CatalogDA();
            List<FiscalRecordCatalog> lstFiscalRecordCatalog = catalogDA.GetChargeCreditCatalogNonTax(codeValueKey);
            // LoggerHelper.LogWebError("FiscalRecords", "GetChargeCreditCatalogNonTax", "GetChargeCreditCatalogNonTax ends");
            return lstFiscalRecordCatalog;
        }

        /// <summary>
        /// Gets the fiscal record cancel reason.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public FiscalRecordCancelReason GetFiscalRecordCancelReason(string name)
        {
            var catalogDA = new CatalogDA();
            List<FiscalRecordCancelReason> catalogList = catalogDA.GetFiscalRecordCancelReason();
            FiscalRecordCancelReason itemByName = null;
            if (catalogList != null)
                itemByName = catalogList.FirstOrDefault(rt => rt.Name == name);
            return itemByName;
        }

        /// <summary>
        /// Gets the fiscal record cancel reason.
        /// </summary>
        /// <param name="applyToIndividual">if set to <c>true</c> [apply to individual].</param>
        /// <param name="applyToGlobal">if set to <c>true</c> [apply to global].</param>
        /// <returns></returns>
        public List<FiscalRecordCancelReason> GetFiscalRecordCancelReason(bool applyToIndividual, bool applyToGlobal)
        {
            var catalogDA = new CatalogDA();
            List<FiscalRecordCancelReason> catalogList = catalogDA.GetFiscalRecordCancelReason();
            if (catalogList != null)
            {
                catalogList = catalogList.Where(e => (e.ApplyToGlobal == applyToGlobal
                    || e.ApplyToIndividual == applyToIndividual)
                    && e.Name != CancelReasonName.ErrorRelacion.ToString()).ToList();
            }
            return catalogList;
        }

        /// <summary>
        /// Gets the fiscal record catalog.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public List<FiscalRecordCatalog> GetFiscalRecordCatalog(Catalog name)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetFiscalRecordCatalog", "GetFiscalRecordCatalog starts");
            var catalogDA = new CatalogDA();
            List<FiscalRecordCatalog> lstFiscalRecordCatalog = catalogDA.GetFiscalRecordCatalog(name);
            // LoggerHelper.LogWebError("FiscalRecords", "GetFiscalRecordCatalog", "GetFiscalRecordCatalog ends");
            return lstFiscalRecordCatalog;
        }

        /// <summary>
        /// Gets the fiscal record catalog by attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fiscalRecordCatalog">The fiscal record catalog.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<FiscalRecordCatalog> GetFiscalRecordCatalogByAttribute(Catalog name, FiscalRecordCatalog fiscalRecordCatalog)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetFiscalRecordCatalogByAttribute", "GetFiscalRecordCatalogByAttribute starts");
            var catalogDA = new CatalogDA();
            List<FiscalRecordCatalog> lstFiscalRecordCatalog = catalogDA.GetFiscalRecordCatalogByAttribute(name, fiscalRecordCatalog);
            // LoggerHelper.LogWebError("FiscalRecords", "GetFiscalRecordCatalogByAttribute", "GetFiscalRecordCatalogByAttribute ends");
            return lstFiscalRecordCatalog;
        }

        /// <summary>
        /// Gets the fiscal record catalog by key.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fiscalRecordCatalog">The fiscal record catalog.</param>
        /// <returns></returns>
        public List<FiscalRecordCatalog> GetFiscalRecordCatalogByKey(Catalog name, FiscalRecordCatalog fiscalRecordCatalog)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetFiscalRecordCatalogByKey", "GetFiscalRecordCatalogByKey starts");
            var catalogDA = new CatalogDA();
            List<FiscalRecordCatalog> lstFiscalRecordCatalog = catalogDA.GetFiscalRecordCatalogByKey(name, fiscalRecordCatalog);
            // LoggerHelper.LogWebError("FiscalRecords", "GetFiscalRecordCatalogByKey", "GetFiscalRecordCatalogByKey ends");
            return lstFiscalRecordCatalog;
        }

        /// <summary>
        /// Gets the type of the fiscal record relation.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public FiscalRecordRelationType GetFiscalRecordRelationType(string name)
        {
            var catalogDA = new CatalogDA();
            List<FiscalRecordRelationType> list = catalogDA.GetFiscalRecordRelationType();
            FiscalRecordRelationType itemByName = null;
            if (list != null)
                itemByName = list.FirstOrDefault(rt => rt.Name == name);
            return itemByName;
        }

        /// <summary>
        /// Gets the charge credit catalog non tax.
        /// </summary>
        /// <returns></returns>
        public List<FiscalRecordCatalog> GetRecieptChargeCodes()
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetRecieptChargeCodes", "GetRecieptChargeCodes starts");
            var catalogDA = new CatalogDA();
            List<FiscalRecordCatalog> lstFiscalRecordCatalog = catalogDA.GetRecieptChargeCodes();
            // LoggerHelper.LogWebError("FiscalRecords", "GetRecieptChargeCodes", "GetRecieptChargeCodes ends");
            return lstFiscalRecordCatalog;
        }

        /// <summary>
        /// Gets the tax regimen.
        /// </summary>
        /// <returns></returns>
        public List<TaxRegimenCatalog> GetTaxRegimen()
        {
            // LoggerHelper.LogWebError("FiscalRecords", "GetTaxRegimen", "GetTaxRegimen starts");
            var catalogDA = new CatalogDA();
            List<TaxRegimenCatalog> lstTaxRegimenCatalog = catalogDA.GetTaxRegimen();
            // LoggerHelper.LogWebError("FiscalRecords", "GetTaxRegimen", "GetTaxRegimen ends");
            return lstTaxRegimenCatalog;
        }
    }
}