// --------------------------------------------------------------------
// <copyright file="FiscalRecordTransactionServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.BusinessInterfaces;
using PowerCampus.DataAccess;
using PowerCampus.Entities;
using System;

namespace PowerCampus.Business
{
    /// <summary>
    ///
    /// </summary>
    public class FiscalRecordTransactionServices : IFiscalRecordTransactionServices
    {
        /// <summary>
        /// The _fiscal record transaction da
        /// </summary>
        private readonly FiscalRecordTransactionDA _fiscalRecordTransactionDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="FiscalRecordTransactionServices"/> class.
        /// </summary>
        public FiscalRecordTransactionServices()
        {
            _fiscalRecordTransactionDA = new FiscalRecordTransactionDA();
        }

        /// <summary>
        /// Deletes the fiscal record transaction.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public int DeleteFiscalRecordTransaction(Guid guid)
        {
            // _logService.Debug("DeleteFiscalRecordTransaction starts");
            int deleteFiscalRecordTransaction = _fiscalRecordTransactionDA.DeleteFiscalRecordTransaction(guid);
            // _logService.Debug("DeleteFiscalRecordTransaction ends");
            return deleteFiscalRecordTransaction;
        }

        /// <summary>
        /// Gets the fiscal record permissions.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Account GetFiscalRecordPermissions(string userName)
        {
            Account account = _fiscalRecordTransactionDA.GetFiscalRecordPermissions(userName);
            return account;
        }

        /// <summary>
        /// Gets the fiscal record transaction by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public Account GetFiscalRecordTransactionByGuid(Guid guid)
        {
            // _logService.Debug("GetFiscalRecordTransactionByGuid starts");
            Account account = _fiscalRecordTransactionDA.GetFiscalRecordTransactionByGuid(guid);
            // _logService.Debug("GetFiscalRecordTransactionByGuid ends");
            return account;
        }
    }
}