// --------------------------------------------------------------------
// <copyright file="CatalogDA.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PowerCampus.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace PowerCampus.DataAccess
{
    /// <summary>
    /// CatalogDA Class
    /// </summary>
    public class CatalogDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogDA"/> class.
        /// </summary>
        public CatalogDA()
        {
            _factory = new DatabaseProviderFactory();
        }
        public List<FiscalRecordCatalog> GetStates()
        {
            List<FiscalRecordCatalog> fiscalRecordCodeTableList = null;
            try
            {
                //_logService.Debug("Method starts - GetFiscalRecordCatalog");
                DataSet fiscalRecordCatalogDataSet = new DataSet();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelState");

                    database.LoadDataSet(command, fiscalRecordCatalogDataSet, "FiscalRecordCatalogByName");
                }
                if (fiscalRecordCatalogDataSet.Tables[0].Rows.Count > 0)
                    fiscalRecordCodeTableList = GetCatalog(Catalog.States, fiscalRecordCatalogDataSet);

                //_logService.Debug("Method ends - GetFiscalRecordCatalog");
                return fiscalRecordCodeTableList;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CatalogDA - GetAllChargeCredits", ex.Message + ex.StackTrace);
                throw;
            }
        }
        /// <summary>
        /// Gets all charge credits.
        /// </summary>
        /// <returns></returns>
        public List<ChargeCredit> GetAllChargeCredits()
        {
            var chargeCredits = new List<ChargeCredit>();
            try
            {
                // _logService.Debug("Method starts - GetAllChargeCredits");
                DataSet allChargeCredits = new DataSet();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelChargeCredit");

                    database.LoadDataSet(command, allChargeCredits, "AllChargeCredits");
                }
                if (allChargeCredits.Tables.Count > 0)
                {
                    if (allChargeCredits.Tables[0].Rows.Count > 0)
                    {
                        chargeCredits = allChargeCredits.Tables[0].AsEnumerable().Select(m => new ChargeCredit()
                        {
                            ChargeCreditCodeId = m.Field<int>("ChargeCreditCodeId"),
                            DistributionOrder = m.Field<string>("DistributionOrder"),
                            ChargeCreditCode = m.Field<string>("Code"),
                            ChargeCreditDesc = m.Field<string>("MediumDesc")
                        }).ToList();
                    }
                }
                else
                {
                    // _logService.Debug("Method ends - GetAllChargeCredits");
                    return null;
                }

                // _logService.Debug("Method ends - GetAllChargeCredits");
                return chargeCredits;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CatalogDA - GetAllChargeCredits", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets all record types.
        /// </summary>
        /// <returns></returns>
        public List<RecordType> GetAllRecordTypes()
        {
            List<RecordType> recordTypeList = new List<RecordType>();
            try
            {
                // _logService.Debug("Method starts - GetAllRecordTypes");
                DataSet allRecordTypeDataSet = new DataSet();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelAllRecordTypes");

                    database.LoadDataSet(command, allRecordTypeDataSet, "AllRecordTypes");
                }
                if (allRecordTypeDataSet.Tables.Count > 0)
                {
                    if (allRecordTypeDataSet.Tables[0].Rows.Count > 0)
                    {
                        recordTypeList = allRecordTypeDataSet.Tables[0].AsEnumerable().Select(m => new RecordType()
                        {
                            CodeValue = m.Field<string>("RecordType"),
                            Description = m.Field<string>("RecordTypeDesc"),
                            IsPeopleType = m.Field<int>("IsPeopleType") == 1 ? true : false,
                            IsOrgType = m.Field<int>("IsOrgType") == 1 ? true : false
                        }).ToList();
                    }
                }
                else
                {
                    // _logService.Debug("Method ends - GetAllRecordTypes");
                    return null;
                }

                // _logService.Debug("Method ends - GetAllRecordTypes");
                return recordTypeList;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CatalogDA - GetAllRecordTypes", ex.Message + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// Gets the cfdi usage.
        /// </summary>
        /// <returns></returns>
        public List<CFDIUsageCatalog> GetCFDIUsage()
        {
            List<CFDIUsageCatalog> cfdiUsageCatalog = null;
            try
            {
                // _logService.Debug("Method starts - GetCFDIUsage");
                var CFDIUsageDataSet = new DataSet();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelFiscalRecordCatalogByName");

                    database.AddInParameter(command, "@CatalogName", DbType.String, Catalog.CFDIUsage.ToString());
                    database.LoadDataSet(command, CFDIUsageDataSet, "FiscalRecordCatalogByName");
                }
                if (CFDIUsageDataSet.Tables[0].Rows.Count > 0)
                {
                    cfdiUsageCatalog = CFDIUsageDataSet.Tables[0].AsEnumerable().Select(m => new CFDIUsageCatalog()
                    {
                        Code = m.Field<string>("c_UsoCFDI"),
                        Description = m.Field<string>("c_UsoCFDI") + "-" + m.Field<string>("Descripcion"),
                        AppliesToPhysicalPerson = m.Field<bool>("AplicaParaPersonaFisica"),
                        AppliesToMoralPerson = m.Field<bool>("AplicaParaPersonaMoral")
                    }).OrderByDescending(m => m.Code).ToList();
                }
                // _logService.Debug("Method ends - GetCFDIUsage");
                return cfdiUsageCatalog;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CatalogDA - GetCFDIUsage", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the charge credit catalog non tax.
        /// </summary>
        /// <returns></returns>
        public List<FiscalRecordCatalog> GetChargeCreditCatalogNonTax(string codeValueKey)
        {
            List<FiscalRecordCatalog> chargeCreditCatalogList = new List<FiscalRecordCatalog>();
            try
            {
                // _logService.Debug("Method starts - GetChargeCreditCatalogNonTax");
                DataSet chargeCreditCatalogNonTaxDataSet = new DataSet();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("dbo.spSelNotTaxChargeCodes");

                    database.AddInParameter(command, "@CodeValueKey", DbType.String, codeValueKey);
                    database.LoadDataSet(command, chargeCreditCatalogNonTaxDataSet, "NotTaxChargeCodes");
                }
                if (chargeCreditCatalogNonTaxDataSet.Tables[0].Rows.Count > 0)
                {
                    chargeCreditCatalogList = chargeCreditCatalogNonTaxDataSet.Tables[0].AsEnumerable().Select(m => new FiscalRecordCatalog()
                    {
                        Id = m.Field<int>("ChargeCreditCodeId"),
                        Code = m.Field<string>("ChargeCode"),
                        Description = m.Field<string>("ChargeCode") + " - " + m.Field<string>("ChargeDesc")
                    }).ToList();
                }
                else
                {
                    // _logService.Debug("Method ends - GetChargeCreditCatalogNonTax");
                    return null;
                }
                // _logService.Debug("Method ends - GetChargeCreditCatalogNonTax");
                return chargeCreditCatalogList;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CatalogDA - GetChargeCreditCatalogNonTax", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the fiscal record cancel reason.
        /// </summary>
        /// <returns></returns>
        public List<FiscalRecordCancelReason> GetFiscalRecordCancelReason()
        {
            DataSet dataSet = new DataSet();
            List<FiscalRecordCancelReason> catalogList = null;
            try
            {
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelFiscalRecordCatalogByName");

                    database.AddInParameter(command, "@CatalogName", DbType.String, Catalog.CancelReason.ToString());
                    database.LoadDataSet(command, dataSet, "FiscalRecordCancelReason");
                }
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    catalogList = dataSet.Tables[0].AsEnumerable().Select(m => new FiscalRecordCancelReason()
                    {
                        ApplyToGlobal = m.Field<bool>("AplicaAGlobal"),
                        ApplyToIndividual = m.Field<bool>("AplicaAIndividual"),
                        Code = m.Field<string>("c_TipoCancelacion"),
                        Description = m.Field<string>("Descripcion"),
                        Name = m.Field<string>("Nombre")
                    }).ToList();
                }
                return catalogList;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - CatalogDA - GetTaxRegimen", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the fiscal record catalog.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public List<FiscalRecordCatalog> GetFiscalRecordCatalog(Catalog name)
        {
            List<FiscalRecordCatalog> fiscalRecordCodeTableList = null;
            try
            {
                // _logService.Debug("Method starts - GetFiscalRecordCatalog");
                DataSet fiscalRecordCatalogDataSet = new DataSet();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelFiscalRecordCatalogByName");

                    database.AddInParameter(command, "@CatalogName", DbType.String, name.ToString());
                    database.LoadDataSet(command, fiscalRecordCatalogDataSet, "FiscalRecordCatalogByName");
                }
                if (fiscalRecordCatalogDataSet.Tables[0].Rows.Count > 0)
                    fiscalRecordCodeTableList = GetCatalog(name, fiscalRecordCatalogDataSet);

                // _logService.Debug("Method ends - GetFiscalRecordCatalog");
                return fiscalRecordCodeTableList;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CatalogDA - GetFiscalRecordCatalog", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the fiscal record catalog by attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fiscalRecordCatalog">The fiscal record catalog.</param>
        /// <returns></returns>
        public List<FiscalRecordCatalog> GetFiscalRecordCatalogByAttribute(Catalog name, FiscalRecordCatalog fiscalRecordCatalog)
        {
            List<FiscalRecordCatalog> fiscalRecordCodeTableList = null;
            try
            {
                // _logService.Debug("Method starts - GetFiscalRecordCatalogByAttribute");
                DataSet fiscalRecordCatalogByAttributeDataSet = new DataSet();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelFiscalRecordCatalogByAttribute");

                    database.AddInParameter(command, "@CatalogName", DbType.String, name.ToString());
                    database.AddInParameter(command, "@AttributeName", DbType.String, GetCatalogAttributeName(name));
                    database.AddInParameter(command, "@AttributeValue", DbType.String, fiscalRecordCatalog.Code);
                    database.LoadDataSet(command, fiscalRecordCatalogByAttributeDataSet, "FiscalRecordCatalogByAttribute");
                }
                if (fiscalRecordCatalogByAttributeDataSet.Tables[0].Rows.Count > 0)
                    fiscalRecordCodeTableList = GetCatalog(name, fiscalRecordCatalogByAttributeDataSet);

                // _logService.Debug("Method ends - GetCashReceipt");
                return fiscalRecordCodeTableList;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CatalogDA - GetFiscalRecordCatalogByAttribute", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the fiscal record catalog by key.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fiscalRecordCatalog">The fiscal record catalog.</param>
        /// <returns></returns>
        public List<FiscalRecordCatalog> GetFiscalRecordCatalogByKey(Catalog name, FiscalRecordCatalog fiscalRecordCatalog)
        {
            List<FiscalRecordCatalog> fiscalRecordCodeTableList = null;
            try
            {
                // _logService.Debug("Method starts - GetFiscalRecordCatalogByKey");
                DataSet fiscalRecordCatalogByKeyDataSet = new DataSet();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spFilterFiscalRecordCatalogByKey");

                    database.AddInParameter(command, "@CatalogName", DbType.String, name.ToString());
                    database.AddInParameter(command, "@KeyValue", DbType.String, fiscalRecordCatalog.Code);
                    database.LoadDataSet(command, fiscalRecordCatalogByKeyDataSet, "CashReceiptInvoiceDetail");
                }
                if (fiscalRecordCatalogByKeyDataSet.Tables[0].Rows.Count > 0)
                    fiscalRecordCodeTableList = GetCatalog(name, fiscalRecordCatalogByKeyDataSet);

                // _logService.Debug("Method ends - GetFiscalRecordCatalogByKey");
                return fiscalRecordCodeTableList;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CatalogDA - GetFiscalRecordCatalogByKey", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the type of the fiscal record relation.
        /// </summary>
        /// <returns></returns>
        public List<FiscalRecordRelationType> GetFiscalRecordRelationType()
        {
            DataSet dataSet = new DataSet();
            List<FiscalRecordRelationType> catalogList = null;
            try
            {
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelFiscalRecordCatalogByName");

                    database.AddInParameter(command, "@CatalogName", DbType.String, Catalog.FiscalRecordRelation.ToString());
                    database.LoadDataSet(command, dataSet, "FiscalRecordRelationType");
                }
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    catalogList = dataSet.Tables[0].AsEnumerable().Select(m => new FiscalRecordRelationType()
                    {
                        Code = m.Field<string>("c_TipoRelacion"),
                        Description = m.Field<string>("Descripcion"),
                        //EndDate = m.Field<DateTime>("FechaFinVigencia"),
                        Name = m.Field<string>("Nombre"),
                        //StartDate = m.Field<DateTime>("FechaInicioVigencia")
                    }).ToList();
                }
                return catalogList;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - CatalogDA - GetTaxRegimen", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the charge credit catalog non tax.
        /// </summary>
        /// <returns></returns>
        public List<FiscalRecordCatalog> GetRecieptChargeCodes()
        {
            List<FiscalRecordCatalog> chargeCreditCatalogList = new List<FiscalRecordCatalog>();
            try
            {
                // _logService.Debug("Method starts - GetRecieptChargeCodes");
                DataSet recieptChargeCodesDataSet = new DataSet();

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelRecieptChargeCodes");

                    database.LoadDataSet(command, recieptChargeCodesDataSet, "RecieptChargeCodes");
                }
                if (recieptChargeCodesDataSet.Tables.Count > 0)
                {
                    if (recieptChargeCodesDataSet.Tables[0].Rows.Count > 0)
                    {
                        chargeCreditCatalogList = recieptChargeCodesDataSet.Tables[0].AsEnumerable().Select(m => new FiscalRecordCatalog()
                        {
                            Id = m.Field<int>("ChargeCreditCodeId"),
                            Code = m.Field<string>("ChargeCode"),
                            Description = m.Field<string>("ChargeCode") + " - " + m.Field<string>("ChargeDesc")
                        }).ToList();
                    }
                }
                else
                {
                    return null;
                }

                // _logService.Debug("Method ends - GetRecieptChargeCodes");
                return chargeCreditCatalogList;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CatalogDA - GetRecieptChargeCodes", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the tax regimen.
        /// </summary>
        /// <returns></returns>
        public List<TaxRegimenCatalog> GetTaxRegimen()
        {
            DataSet taxRegimenCatalogDataSet = new DataSet();
            List<TaxRegimenCatalog> codeTaxRegimenList = null;
            try
            {
                // _logService.Debug("Method starts - GetTaxRegimen");

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelFiscalRecordCatalogByName");

                    database.AddInParameter(command, "@CatalogName", DbType.String, Catalog.TaxRegimen.ToString());
                    database.LoadDataSet(command, taxRegimenCatalogDataSet, "FiscalRecordCatalogByName");
                }
                if (taxRegimenCatalogDataSet.Tables[0].Rows.Count > 0)
                {
                    codeTaxRegimenList = taxRegimenCatalogDataSet.Tables[0].AsEnumerable().Select(m => new TaxRegimenCatalog()
                    {
                        Id = m.Field<int>("TaxRegimenId"),
                        Code = m.Field<string>("TaxRegimenCode"),
                        Description = m.Field<string>("TaxRegimenCode") + " - " + m.Field<string>("TaxRegimentDesc"),
                        Status = m.Field<string>("Status"),
                        PhysicalPerson = m.Field<bool>("PhysicalPerson"),
                        MoralPerson = m.Field<bool>("MoralPerson")
                    }).ToList();
                }
                // _logService.Debug("Method ends - GetTaxRegimen");
                return codeTaxRegimenList;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "CatalogDA - GetTaxRegimen", ex.Message + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the catalog.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="dataset">The dataset.</param>
        /// <returns></returns>
        private List<FiscalRecordCatalog> GetCatalog(Catalog name, DataSet dataset)
        {
            List<FiscalRecordCatalog> fiscalRecordCodeTableList = null;
            // _logService.Debug("Method starts - GetCatalog");
            switch (name)
            {
                case Catalog.ProductServiceKey:
                case Catalog.UnityKey:
                case Catalog.Country:
                    fiscalRecordCodeTableList = dataset.Tables[0].AsEnumerable().Select(m => new FiscalRecordCatalog()
                    {
                        Code = m.Field<string>("code"),
                        Description = m.Field<string>("code") + " - " + m.Field<string>("Description"),
                    }).ToList();
                    break;

                case Catalog.PostalCode:
                    fiscalRecordCodeTableList = dataset.Tables[0].AsEnumerable().Select(m => new FiscalRecordCatalog()
                    {
                        Code = m.Field<string>("code"),
                        Description = m.Field<string>("Description"),
                    }).ToList();
                    break;

                case Catalog.PaymentType:
                    fiscalRecordCodeTableList = dataset.Tables[0].AsEnumerable().Select(m => new FiscalRecordCatalog()
                    {
                        Id = m.Field<int>("PaymentMethodId"),
                        Code = m.Field<string>("PaymentMethodCode"),
                        Description = m.Field<string>("PaymentMethodCode") + " - " + m.Field<string>("PaymentMethodDesc"),
                    }).ToList();
                    break;

                case Catalog.Tax:
                    fiscalRecordCodeTableList = dataset.Tables[0].AsEnumerable().Select(m => new FiscalRecordCatalog()
                    {
                        Code = m.Field<string>("c_Impuesto"),
                        Description = m.Field<string>("c_Impuesto") + " - " + m.Field<string>("Descripcion"),
                    }).ToList();
                    break;

                case Catalog.PaymentMethod:
                    fiscalRecordCodeTableList = dataset.Tables[0].AsEnumerable().Select(m => new FiscalRecordCatalog()
                    {
                        Code = m.Field<string>("c_MetodoPago"),
                        Description = m.Field<string>("c_MetodoPago") + " - " + m.Field<string>("Descripcion"),
                    }).ToList();
                    break;

                case Catalog.Currency:
                    fiscalRecordCodeTableList = dataset.Tables[0].AsEnumerable().Select(m => new FiscalRecordCatalog()
                    {
                        Code = m.Field<string>("c_Moneda"),
                        Description = m.Field<string>("c_Moneda") + " - " + m.Field<string>("Descripcion"),
                    }).ToList();
                    break;

                case Catalog.TaxRate:
                    fiscalRecordCodeTableList = dataset.Tables[0].AsEnumerable().Select(m => new FiscalRecordCatalog()
                    {
                        Code = m.Field<string>("Factor"),
                        Description = m.Field<decimal>("ValorMaximo").ToString(),
                        Status = m.Field<string>("Impuesto")
                    }).ToList();
                    break;

                case Catalog.FiscalRecordType:
                    fiscalRecordCodeTableList = dataset.Tables[0].AsEnumerable().Select(m => new FiscalRecordCatalog()
                    {
                        Code = m.Field<string>("c_TipoDeComprobante"),
                        Description = m.Field<string>("Descripcion"),
                    }).ToList();
                    break;

                case Catalog.FactorType:
                    fiscalRecordCodeTableList = dataset.Tables[0].AsEnumerable().Select(m => new FiscalRecordCatalog()
                    {
                        Code = m.Field<string>("c_TipoFactor"),
                    }).ToList();
                    break;

                case Catalog.Frequency:
                    fiscalRecordCodeTableList = dataset.Tables[0].AsEnumerable().Select(m => new FiscalRecordCatalog()
                    {
                        Code = m.Field<string>("c_Periodicidad"),
                        Description = m.Field<string>("Descripcion"),
                    }).ToList();
                    break;
                case Catalog.States:
                    fiscalRecordCodeTableList = dataset.Tables[0].AsEnumerable().Select(m => new FiscalRecordCatalog()
                    {
                        Code = m.Field<int>("Code_Value").ToString(),
                        Description = m.Field<string>("Description"),
                    }).ToList();
                    break;
            }
            // _logService.Debug("Method ends - GetCatalog");
            return fiscalRecordCodeTableList;
        }

        /// <summary>
        /// Gets the catalog attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private string GetCatalogAttributeName(Catalog name)
        {
            // _logService.Debug("Method starts - GetCatalogAttributeName");
            switch (name)
            {
                case Catalog.ProductServiceKey:
                    // _logService.Debug("Method ends - GetCatalogAttributeName");
                    return "c_ClaveProdServ";

                case Catalog.UnityKey:
                    // _logService.Debug("Method ends - GetCatalogAttributeName");
                    return "c_ClaveUnidad";

                case Catalog.PostalCode:
                    // _logService.Debug("Method ends - GetCatalogAttributeName");
                    return "c_CodigoPostal";

                case Catalog.PaymentType:
                    // _logService.Debug("Method ends - GetCatalogAttributeName");
                    return "PaymentMethodCode";

                case Catalog.Tax:
                    // _logService.Debug("Method ends - GetCatalogAttributeName");
                    return "c_Impuesto";

                case Catalog.PaymentMethod:
                    // _logService.Debug("Method ends - GetCatalogAttributeName");
                    return "c_MetodoPago";

                case Catalog.Currency:
                    // _logService.Debug("Method ends - GetCatalogAttributeName");
                    return "c_Moneda";

                case Catalog.Country:
                    // _logService.Debug("Method ends - GetCatalogAttributeName");
                    return "c_Pais";

                case Catalog.TaxRegimen:
                    // _logService.Debug("Method ends - GetCatalogAttributeName");
                    return "TaxRegimenCode";

                case Catalog.TaxRate:
                    // _logService.Debug("Method ends - GetCatalogAttributeName");
                    return "Impuesto";

                case Catalog.FiscalRecordType:
                    // _logService.Debug("Method ends - GetCatalogAttributeName");
                    return "c_TipoDeComprobante";

                case Catalog.FactorType:
                    // _logService.Debug("Method ends - GetCatalogAttributeName");
                    return "c_TipoFactor";

                case Catalog.FiscalRecordRelation:
                    // _logService.Debug("Method ends - GetCatalogAttributeName");
                    return "c_TipoRelacion";

                case Catalog.CFDIUsage:
                    // _logService.Debug("Method ends - GetCatalogAttributeName");
                    return "c_UsoCFDI";

                default:
                    // _logService.Debug("Method ends - GetCatalogAttributeName");
                    return string.Empty;
            }
        }
    }
}