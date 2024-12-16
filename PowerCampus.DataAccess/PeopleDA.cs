// --------------------------------------------------------------------
// <copyright file="PeopleDA.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
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
    /// PeopleDA DataAccess Class
    /// </summary>
    public class PeopleDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeopleDA"/> class.
        /// </summary>
        public PeopleDA()
        {
            _factory = new DatabaseProviderFactory();
        }

        /// <summary>
        /// Gets the charges.
        /// </summary>
        /// <param name="peopleCodeId">The people code identifier.</param>
        /// <param name="YearTermSession">The yts.</param>
        /// <returns></returns>
        public People GetCharges(string peopleCodeId, string YearTermSession)
        {
            try
            {
                // _logService.Debug("Method starts - GetCharges");
                DataSet peopleResultDataset = new DataSet();
                People people = new People();
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
                    database.AddInParameter(command, "@PeopleOrgCodeId", DbType.String, peopleCodeId);
                    database.LoadDataSet(command, peopleResultDataset, "PeopleResultDataset");
                }
                if (peopleResultDataset.Tables[0].Rows.Count > 0)
                {
                    people.PeopleChargeCredit = new List<ChargeCredit>();
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

                        people.PeopleChargeCredit.Add(chargeCredit);
                    }
                }
                else
                {
                    people.PeopleChargeCredit.Add(null);
                }

                #endregion Get Charges

                // _logService.Debug("Method ends - GetCharges");
                return people;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - PeopleDA - GetCharges", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets the taxpayer information.
        /// </summary>
        /// <param name="peopleOrgCodeId">The people org code identifier.</param>
        /// <returns></returns>
        public People GetTaxpayerInfo(string peopleOrgCodeId)
        {
            try
            {
                // _logService.Debug("Method starts - GetTaxpayerInfo");
                DataSet peopleResultDataset = new DataSet();
                People people = new People();

                #region Get defaults People

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spRetrievePeopleOrgTaxpayerInfo");

                    database.AddInParameter(command, "@PeopleOrgCodeId", DbType.String, peopleOrgCodeId);
                    database.LoadDataSet(command, peopleResultDataset, "PeopleResultDataset");
                }
                if (peopleResultDataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in peopleResultDataset.Tables[0].Rows)
                    {
                        var result = new People
                        {
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
                        people = result;
                    }

                    List<CFDIUsageCatalog> CFDICatalogList = new List<CFDIUsageCatalog>();
                    CatalogDA da = new CatalogDA();
                    people.FiscalRecordsDefault.CFDICatalog = new List<CFDIUsageCatalog>();
                    CFDICatalogList = da.GetCFDIUsage();

                    if (!string.IsNullOrEmpty(people.FiscalRecordsDefault.TaxpayerId))
                    {
                        if (people.FiscalRecordsDefault.TaxpayerId.Length.Equals(12))
                        {
                            people.FiscalRecordsDefault.CFDICatalog =
                                CFDICatalogList.Where(m => m.AppliesToMoralPerson == true && m.AppliesToPhysicalPerson == true).Select(m => new CFDIUsageCatalog
                                {
                                    Code = m.Code,
                                    Description = m.Description
                                }).ToList();
                        }
                        else if (people.FiscalRecordsDefault.TaxpayerId.Length.Equals(13))
                            people.FiscalRecordsDefault.CFDICatalog = CFDICatalogList;
                    }
                }
                else
                {
                    //There's no Taxpayer Info just retrieve Primary Email
                    foreach (DataRow row in peopleResultDataset.Tables[1].Rows)
                    {
                        people.PrimaryEmail = string.IsNullOrEmpty(row["Email"].ToString()) ? string.Empty
                                    : row["Email"].ToString();
                        people.FiscalRecordsDefault = new FiscalRecordDefaults
                        {
                            InvoicePreferredTaxpayerId = (int?)null,
                            InvoiceTaxpayerId = (int?)null,
                            TaxpayerId = string.Empty,
                            Email = people.PrimaryEmail,
                            CorporateName = string.Empty,
                            CFDIUsageCode = string.Empty,
                            CFDIUsageDesc = string.Empty,
                            CFDICatalog = new List<CFDIUsageCatalog>()
                        };
                    }
                }
                people.FullName = CommonDA.GetPeopleOrgFullName(peopleOrgCodeId);

                #endregion Get defaults People

                // _logService.Debug("Method ends - GetTaxpayerInfo");
                return people;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - PeopleDA - GetTaxpayerInfo", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets the year term session.
        /// </summary>
        /// <param name="peopleCodeId">The people code identifier.</param>
        /// <returns></returns>
        public People GetYearTermSession(string peopleCodeId)
        {
            try
            {
                // _logService.Debug("Method starts - GetYearTermSession");

                #region Get Year, Term, Session

                DataSet BillingYTSDataSet = new DataSet();
                Database database = _factory.CreateDefault();
                People people = new People();

                BillingYTSDataSet = new DataSet();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand dbCommand = database.GetStoredProcCommand("spGetBillingYTSbyPeopleOrgCodeId");
                    database.AddInParameter(dbCommand, "@PeopleOrgCodeId", DbType.String, peopleCodeId);
                    database.LoadDataSet(dbCommand, BillingYTSDataSet, "BillingYTS");
                }
                people.YearTermSession = new List<YTS>();

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

                        people.YearTermSession.Add(YearTermSession);
                    }
                }

                // _logService.Debug("Method ends - GetYearTermSession");
                return people;

                #endregion Get Year, Term, Session
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - PeopleDA - GetYearTermSession", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Saves the taxpayer identifier.
        /// </summary>
        /// <param name="people">The people.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public int SaveTaxpayerId(People people, string userName)
        {
            try
            {
                // _logService.Debug("Method starts - SaveTaxpayerId");
                Database database = _factory.CreateDefault();
                int invoicePreferredTaxpayerId = 0;
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spUpdInvoicePreferredTaxpayer");

                    database.AddInParameter(command, "@PeopleOrgCodeId", DbType.String, people.PeopleCodeId);
                    database.AddInParameter(command, "@TaxpayerId", DbType.String, people.FiscalRecordsDefault.TaxpayerId);
                    database.AddInParameter(command, "@ReceiverEmail", DbType.String, people.FiscalRecordsDefault.Email);
                    database.AddInParameter(command, "@CFDIUsageCode", DbType.String, people.FiscalRecordsDefault.CFDIUsageCode);
                    database.AddInParameter(command, "@CFDIUsageDesc", DbType.String, people.FiscalRecordsDefault.CFDIUsageDesc);
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
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - PeopleDA - SaveTaxpayerId", ex.Message);
                throw;
            }
        }
    }
}