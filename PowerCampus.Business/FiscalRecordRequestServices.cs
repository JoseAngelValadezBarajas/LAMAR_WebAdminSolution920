// --------------------------------------------------------------------
// <copyright file="FiscalRecordRequestServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.BusinessInterfaces;
using PowerCampus.DataAccess;
using PowerCampus.Entities;

namespace PowerCampus.Business
{
    /// <summary>
    /// FiscalRecordRequestServices class
    /// </summary>
    public class FiscalRecordRequestServices : IFiscalRecordRequestServices
    {
        private readonly FiscalRecordRequestDA _fiscalRecordRequestDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="FiscalRecordServices"/> class.
        /// </summary>
        public FiscalRecordRequestServices()
        {
            _fiscalRecordRequestDA = new FiscalRecordRequestDA();
        }

        /// <summary>
        /// Insert the invoice request.
        /// </summary>
        /// <param name="fiscalRecordRequest">The fiscal record request.</param>
        /// <returns></returns>
        public int InsertInvoiceRequest(FiscalRecordRequest fiscalRecordRequest)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "InsertInvoiceRequest", "InsertInvoiceRequest starts");
            int invoiceRequestId = _fiscalRecordRequestDA.InsertInvoiceRequest(fiscalRecordRequest);
            // LoggerHelper.LogWebError("FiscalRecords", "InsertInvoiceRequest", "InsertInvoiceRequest ends");
            return invoiceRequestId;
        }
    }
}