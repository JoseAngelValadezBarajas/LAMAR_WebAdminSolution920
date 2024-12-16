// --------------------------------------------------------------------
// <copyright file="ChargeCreditsDA.cs" company="Ellucian">
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

namespace PowerCampus.DataAccess
{
    /// <summary>
    /// ChargeCreditsDA class
    /// </summary>
    public class ChargeCreditsDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChargeCreditsDA"/> class.
        /// </summary>
        public ChargeCreditsDA()
        {
            _factory = new DatabaseProviderFactory();
        }

        /// <summary>
        /// Charges the credits.
        /// </summary>
        /// <param name="chargeCreditNumber">The charge credit number.</param>
        /// <returns></returns>
        public ChargeCredit ChargeCredits(int chargeCreditNumber)
        {
            // _logService.Debug("Method starts - ChargeCredits");
            DataSet fiscalRecordDataset = new DataSet();
            ChargeCredit chargeCreditTable = new ChargeCredit();
            try
            {
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelChargeCreditToPPD");

                    database.AddInParameter(command, "@ChargeCreditNumber", DbType.Int32, chargeCreditNumber);
                    database.LoadDataSet(command, fiscalRecordDataset, "CashReceiptInvoiceDetail");
                }
                decimal Total = decimal.Parse(fiscalRecordDataset.Tables[0].Rows[0]["UnitAmount"].ToString())
                                + (string.IsNullOrEmpty(fiscalRecordDataset.Tables[0].Rows[0]["TotalTaxes"].ToString())
                                ? 0 : decimal.Parse(fiscalRecordDataset.Tables[0].Rows[0]["TotalTaxes"].ToString()));

                if (fiscalRecordDataset.Tables[0].Rows.Count > 0)
                {
                    chargeCreditTable = new ChargeCredit
                    {
                        ChargeCreditCode = fiscalRecordDataset.Tables[0].Rows[0]["ChargeCreditCode"].ToString(),
                        ChargeCreditDesc = fiscalRecordDataset.Tables[0].Rows[0]["ChargeCreditDesc"].ToString(),
                        ChargeCreditCodeId = int.Parse(fiscalRecordDataset.Tables[0].Rows[0]["ChargeCreditCodeId"].ToString()),
                        UnitAmount = decimal.Parse(fiscalRecordDataset.Tables[0].Rows[0]["UnitAmount"].ToString()),
                        ProductServiceKey = string.IsNullOrEmpty(fiscalRecordDataset.Tables[0].Rows[0]["ProductServiceKey"].ToString()) ? string.Empty : fiscalRecordDataset.Tables[0].Rows[0]["ProductServiceKey"].ToString(),
                        ProductServiceDesc = string.IsNullOrEmpty(fiscalRecordDataset.Tables[0].Rows[0]["ProductServiceDesc"].ToString()) ? string.Empty : fiscalRecordDataset.Tables[0].Rows[0]["ProductServiceDesc"].ToString(),
                        UnitKey = string.IsNullOrEmpty(fiscalRecordDataset.Tables[0].Rows[0]["UnityKey"].ToString()) ? string.Empty : fiscalRecordDataset.Tables[0].Rows[0]["UnityKey"].ToString(),
                        UnityName = string.IsNullOrEmpty(fiscalRecordDataset.Tables[0].Rows[0]["UnityName"].ToString()) ? string.Empty : fiscalRecordDataset.Tables[0].Rows[0]["UnityName"].ToString(),
                        TotalTaxes = string.IsNullOrEmpty(fiscalRecordDataset.Tables[0].Rows[0]["TotalTaxes"].ToString()) ? 0 : decimal.Parse(fiscalRecordDataset.Tables[0].Rows[0]["TotalTaxes"].ToString()),
                        SubjectToTax = fiscalRecordDataset.Tables[0].Rows[0]["SubjectToTax"].ToString(),
                        TotalUnit = Total
                    };
                }

                #region PaymentType & PaymentMethod

                CatalogDA catalogDA = new CatalogDA();

                List<FiscalRecordCatalog> CatalogListPaymentMethod = catalogDA.GetFiscalRecordCatalog(Catalog.PaymentMethod);
                chargeCreditTable.PaymentMethod = CatalogListPaymentMethod.Find(x => x.Code == Constants.PPDPaymentMethod).Description;
                List<FiscalRecordCatalog> CatalogListPaymentType = catalogDA.GetFiscalRecordCatalog(Catalog.PaymentType);
                chargeCreditTable.PaymentType = CatalogListPaymentType.Find(x => x.Code == Constants.PPDPaymentType).Description;

                #endregion PaymentType & PaymentMethod

                // _logService.Debug("Method ends - ChargeCredits");
                return chargeCreditTable;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - ChargeCreditsDA - ChargeCredits", ex.Message + ex.StackTrace);
                return null;
            }
        }
    }
}