// --------------------------------------------------------------------
// <copyright file="FiscalRecordRequestDA.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PowerCampus.Entities;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace PowerCampus.DataAccess
{
    /// <summary>
    /// FiscalRecordRequestDA Class
    /// </summary>
    public class FiscalRecordRequestDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FiscalRecordDA"/> class.
        /// </summary>
        public FiscalRecordRequestDA()
        {
            _factory = new DatabaseProviderFactory();
        }

        /// <summary>
        /// Inserts the invoice request.
        /// </summary>
        /// <param name="fiscalRecordRequest">The fiscal record request.</param>
        /// <returns></returns>
        public int InsertInvoiceRequest(FiscalRecordRequest fiscalRecordRequest)
        {
            try
            {
                // _logService.Debug("Method starts - InsertInvoiceRequest");

                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    int invoiceRequestId = 0;
                    if (!fiscalRecordRequest.InvoicePaymentReceiptId.HasValue)
                    {
                        DbTransaction transaction = null;
                        try
                        {
                            int returnStatus = 1;
                            DbCommand command;
                            connection.Open();
                            transaction = connection.BeginTransaction();
                            if (!string.IsNullOrEmpty(fiscalRecordRequest.CancelReasonKey))
                            {
                                command = database.GetStoredProcCommand("spUpdInvoiceHeaderForCancellation");
                                database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, fiscalRecordRequest.InvoiceHeaderId);
                                database.AddInParameter(command, "@CancelReason", DbType.String, fiscalRecordRequest.CancelReasonKey);
                                database.AddParameter(command, "@ReturnStatus", DbType.Object, ParameterDirection.ReturnValue, "@ReturnStatus", DataRowVersion.Default, new object());
                                database.ExecuteNonQuery(command);
                                returnStatus = (int)database.GetParameterValue(command, "@ReturnStatus");
                            }
                            if (returnStatus > 0)
                            {
                                command = database.GetStoredProcCommand("spInsInvoiceRequest");

                                database.AddInParameter(command, "@InvoiceHeaderId", DbType.Int32, fiscalRecordRequest.InvoiceHeaderId);
                                database.AddInParameter(command, "@Status", DbType.Int32, fiscalRecordRequest.Status);
                                database.AddOutParameter(command, "@InvoiceRequestId", DbType.Int32, 0);
                                database.ExecuteNonQuery(command);
                                invoiceRequestId = (int)command.Parameters["@InvoiceRequestId"].Value;
                                if (invoiceRequestId > 0)
                                    transaction.Commit();
                            }
                            else
                                transaction.Rollback();
                        }
                        catch (Exception transException)
                        {
                            if (transaction != null)
                                transaction.Rollback();
                            LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - RetrieveFiscalRecords", transException.Message + transException.StackTrace);
                            throw;
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    else
                    {
                        DbCommand command = database.GetStoredProcCommand("spInsInvoiceRequestPaymentReceipt");

                        database.AddInParameter(command, "@InvoicePaymentReceiptId", DbType.Int32, fiscalRecordRequest.InvoicePaymentReceiptId.Value);
                        database.AddInParameter(command, "@Status", DbType.Int32, fiscalRecordRequest.Status);
                        database.AddOutParameter(command, "@InvoiceRequestId", DbType.Int32, 0);
                        database.ExecuteNonQuery(command);
                        invoiceRequestId = (int)command.Parameters["@InvoiceRequestId"].Value;
                    }

                    // _logService.Debug("Method ends - InsertInvoiceRequest");
                    return invoiceRequestId;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - FiscalRecordDA - RetrieveFiscalRecords", e.Message + e.StackTrace);
                throw;
            }
        }
    }
}