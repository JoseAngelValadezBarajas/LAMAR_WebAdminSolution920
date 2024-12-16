// --------------------------------------------------------------------
// <copyright file="IFiscalRecordRequestServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;

namespace PowerCampus.BusinessInterfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IFiscalRecordRequestServices
    {
        /// <summary>
        /// Insers the invoice request.
        /// </summary>
        /// <param name="fiscalRecordRequest">The fiscal record request.</param>
        /// <returns></returns>
        int InsertInvoiceRequest(FiscalRecordRequest fiscalRecordRequest);
    }
}