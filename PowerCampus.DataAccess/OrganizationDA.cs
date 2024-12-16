// --------------------------------------------------------------------
// <copyright file="OrganizationDA.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
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
    /// OrganizationDA Class
    /// </summary>
    public class OrganizationDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationDA"/> class.
        /// </summary>
        public OrganizationDA()
        {
            _factory = new DatabaseProviderFactory();
        }

        /// <summary>
        /// Gets the charges.
        /// </summary>
        /// <param name="OrganizationId">The organization identifier.</param>
        /// <param name="YearTermSession">The yts.</param>
        /// <returns></returns>
        public Organization GetCharges(string OrganizationId, string YearTermSession)
        {
            try
            {
                // _logService.Debug("Method starts - GetCharges");
                DataSet peopleResultDataset = new DataSet();
                Organization organization = new Organization();
                string LastYear = string.Empty;
                string LastTerm = string.Empty;
                string LastSession = string.Empty;

                #region Get Charges

                string[] YTS = YearTermSession.Split('/');

                if (YTS.Length > 2)
                {
                    LastYear = YTS[0];
                    LastTerm = YTS[1];
                    LastSession = YTS[2];
                }
                else
                {
                    LastYear = YTS[0];
                    LastTerm = YTS[1];
                }

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spGetChargeCreditbyPeopleOrgCodeId");

                    database.AddInParameter(command, "@AcademicYear", DbType.String, LastYear);
                    database.AddInParameter(command, "@AcademicTerm", DbType.String, LastTerm);
                    database.AddInParameter(command, "@AcademicSession", DbType.String, LastSession);
                    database.AddInParameter(command, "@PeopleOrgCodeId", DbType.String, OrganizationId);
                    database.LoadDataSet(command, peopleResultDataset, "PeopleResult");
                }
                if (peopleResultDataset.Tables[0].Rows.Count > 0)
                {
                    organization.OrganizationChargeCredit = new List<ChargeCredit>();
                    foreach (DataRow row in peopleResultDataset.Tables[0].Rows)
                    {
                        decimal totalUnit = 0;
                        totalUnit = decimal.Parse(row["TaxAmount"].ToString()) + decimal.Parse(row["Amount"].ToString());

                        var chargeCredit = new ChargeCredit
                        {
                            EntryDate = row["EntryDate"].ToString(),
                            ChargeNumberSource = Convert.ToInt32(row["ChargeCreditNumber"]),
                            ChargeCreditCode = row["ChargeCreditCode"].ToString(),
                            ChargeCreditDesc = row["Description"].ToString(),
                            UnitAmount = string.IsNullOrEmpty(row["Amount"].ToString()) ? 0 : decimal.Parse(row["Amount"].ToString()),
                            TotalTaxes = string.IsNullOrEmpty(row["TaxAmount"].ToString()) ? 0 : decimal.Parse(row["TaxAmount"].ToString()),
                            TotalUnit = totalUnit,
                            CanCreatePPD = (bool)row["CanCreatePPD"]
                        };

                        organization.OrganizationChargeCredit.Add(chargeCredit);
                    }
                }
                else
                {
                    organization.OrganizationChargeCredit.Add(null);
                }

                #endregion Get Charges

                // _logService.Debug("Method ends - GetCharges");
                return organization;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - OrganizationDA - GetCharges", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets the taxpayer information.
        /// </summary>
        /// <param name="organizationCodeId">The organization code identifier.</param>
        /// <returns></returns>
        public Organization GetTaxpayerInfo(string organizationCodeId)
        {
            try
            {
                // _logService.Debug("Method starts - GetTaxpayerInfo");
                DataSet peopleOrgTaxpayerInfoDataSet = new DataSet();
                Organization organization = new Organization();

                #region Get Defaults

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spRetrievePeopleOrgTaxpayerInfo");

                    database.AddInParameter(command, "@PeopleOrgCodeId", DbType.String, organizationCodeId);
                    database.LoadDataSet(command, peopleOrgTaxpayerInfoDataSet, "PeopleOrgTaxpayerInfo");
                }
                if (peopleOrgTaxpayerInfoDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in peopleOrgTaxpayerInfoDataSet.Tables[0].Rows)
                    {
                        var result = new Organization
                        {
                            OrganizationCodeId = organizationCodeId,
                            FiscalRecordsDefault = new FiscalRecordDefaults
                            {
                                InvoicePreferredTaxpayerId = string.IsNullOrEmpty(row["InvoicePreferredTaxpayerId"].ToString()) ? (int?)null
                                    : int.Parse(row["InvoicePreferredTaxpayerId"].ToString()),
                                InvoiceTaxpayerId = string.IsNullOrEmpty(row["InvoiceTaxpayerId"].ToString()) ? (int?)null
                                    : int.Parse(row["InvoiceTaxpayerId"].ToString()),
                                TaxpayerId = string.IsNullOrEmpty(row["TaxpayerId"].ToString()) ? string.Empty
                                    : row["TaxpayerId"].ToString(),
                                Email = string.IsNullOrEmpty(row["Email"].ToString()) ? string.Empty
                                    : row["Email"].ToString(),
                                CorporateName = string.IsNullOrEmpty(row["CorporateName"].ToString()) ? string.Empty
                                    : row["CorporateName"].ToString(),
                                CFDIUsageCode = string.IsNullOrEmpty(row["CFDIUsageCode"].ToString()) ? string.Empty
                                    : row["CFDIUsageCode"].ToString(),
                                CFDIUsageDesc = string.IsNullOrEmpty(row["CFDIUsageDesc"].ToString()) ? string.Empty
                                    : row["CFDIUsageDesc"].ToString(),
                                PostalCode = string.IsNullOrEmpty(row["PostalCode"].ToString()) ? string.Empty
                                    : row["PostalCode"].ToString(),
                                TaxRegimen = string.IsNullOrEmpty(row["TaxRegimen"].ToString()) ? string.Empty
                                    : row["TaxRegimen"].ToString(),
                                TaxRegimenDesc = string.IsNullOrEmpty(row["TaxRegimenDesc"].ToString()) ? string.Empty
                                    : row["TaxRegimenDesc"].ToString(),
                                CFDICatalog = new List<CFDIUsageCatalog>()
                            }
                        };
                        organization = result;
                    }
                    List<CFDIUsageCatalog> CFDICatalogList = new List<CFDIUsageCatalog>();
                    CatalogDA catalogDA = new CatalogDA();
                    organization.FiscalRecordsDefault.CFDICatalog = new List<CFDIUsageCatalog>();
                    CFDICatalogList = catalogDA.GetCFDIUsage();

                    if (!string.IsNullOrEmpty(organization.FiscalRecordsDefault.TaxpayerId))
                    {
                        if (organization.FiscalRecordsDefault.TaxpayerId.Length.Equals(12))
                        {
                            organization.FiscalRecordsDefault.CFDICatalog =
                                CFDICatalogList.Where(m => m.AppliesToMoralPerson == true && m.AppliesToPhysicalPerson == true).Select(m => new CFDIUsageCatalog
                                {
                                    Code = m.Code,
                                    Description = m.Description
                                }).ToList();
                        }
                        else if (organization.FiscalRecordsDefault.TaxpayerId.Length.Equals(13))
                            organization.FiscalRecordsDefault.CFDICatalog = CFDICatalogList;
                    }
                }
                else
                {
                    foreach (DataRow row in peopleOrgTaxpayerInfoDataSet.Tables[1].Rows)
                    {
                        organization.OrganizationCodeId = organizationCodeId;
                        organization.Email = string.IsNullOrEmpty(row["Email"].ToString()) ? string.Empty
                                    : row["Email"].ToString();
                        organization.FiscalRecordsDefault = new FiscalRecordDefaults
                        {
                            InvoicePreferredTaxpayerId = (int?)null,
                            InvoiceTaxpayerId = (int?)null,
                            TaxpayerId = string.Empty,
                            Email = organization.Email,
                            CorporateName = string.Empty,
                            CFDIUsageCode = string.Empty,
                            CFDIUsageDesc = string.Empty,
                            CFDICatalog = new List<CFDIUsageCatalog>()
                        };
                    }
                }

                #endregion Get Defaults

                organization.OrganizationName = CommonDA.GetPeopleOrgFullName(organizationCodeId);
                // _logService.Debug("Method ends - GetTaxpayerInfo");
                return organization;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - OrganizationDA - GetTaxpayerInfo", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets the year term session.
        /// </summary>
        /// <param name="organizationCodeId">The organization code identifier.</param>
        /// <returns></returns>
        public Organization GetYearTermSession(string organizationCodeId)
        {
            try
            {
                // _logService.Debug("Method starts - GetYearTermSession");

                #region Get Year, Term, Session

                DataSet BillingYTSDataSet = new DataSet();
                Database database = _factory.CreateDefault();
                Organization organization = new Organization();

                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand getBillingCommand = database.GetStoredProcCommand("spGetBillingYTSbyPeopleOrgCodeId");
                    database.AddInParameter(getBillingCommand, "@PeopleOrgCodeId", DbType.String, organizationCodeId);
                    database.LoadDataSet(getBillingCommand, BillingYTSDataSet, "BillingYTS");
                }
                organization.YearTermSession = new List<YTS>();

                if (BillingYTSDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in BillingYTSDataSet.Tables[0].Rows)
                    {
                        var YearTermSession = new YTS
                        {
                            Year = string.IsNullOrEmpty(row["Year"].ToString()) ? string.Empty : row["Year"].ToString(),
                            Term = string.IsNullOrEmpty(row["Term"].ToString()) ? string.Empty : row["Term"].ToString(),
                            Term_Desc = string.IsNullOrEmpty(row["Term_Desc"].ToString()) ? string.Empty : row["Term_Desc"].ToString(),
                            Session = string.IsNullOrEmpty(row["Session"].ToString()) ? string.Empty : row["Session"].ToString(),
                            Session_Desc = string.IsNullOrEmpty(row["Session"].ToString()) ? string.Empty : row["Session_Desc"].ToString()
                        };

                        organization.YearTermSession.Add(YearTermSession);
                    }
                }

                // _logService.Debug("Method ends - GetTaxpayerInfo");
                return organization;

                #endregion Get Year, Term, Session
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - OrganizationDA - GetYearTermSession", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Saves the taxpayer identifier.
        /// </summary>
        /// <param name="organization">The organization.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public int SaveTaxpayerId(Organization organization, string userName)
        {
            try
            {
                // _logService.Debug("Method starts - SaveTaxpayerId");
                Database database = _factory.CreateDefault();
                int invoicePreferredTaxpayerId;
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spUpdInvoicePreferredTaxpayer");
                    database.AddInParameter(command, "@PeopleOrgCodeId", DbType.String, organization.OrganizationCodeId);
                    database.AddInParameter(command, "@TaxpayerId", DbType.String, organization.FiscalRecordsDefault.TaxpayerId);
                    database.AddInParameter(command, "@ReceiverEmail", DbType.String, organization.FiscalRecordsDefault.Email);
                    database.AddInParameter(command, "@CFDIUsageCode", DbType.String, organization.FiscalRecordsDefault.CFDIUsageCode);
                    database.AddInParameter(command, "@CFDIUsageDesc", DbType.String, organization.FiscalRecordsDefault.CFDIUsageDesc);
                    database.AddInParameter(command, "@Opid", DbType.String, userName);
                    database.AddInParameter(command, "@Terminal", DbType.String, "001");
                    database.AddOutParameter(command, "@InvoicePreferredTaxpayerId", DbType.Int32, 0);
                    database.ExecuteNonQuery(command);
                    invoicePreferredTaxpayerId = (int)command.Parameters["@InvoicePreferredTaxpayerId"].Value;
                }
                // _logService.Debug("Method ends - SaveTaxpayerId");
                return invoicePreferredTaxpayerId;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - OrganizationDA - SaveTaxpayerId", ex.Message);
                throw;
            }
        }
    }
}