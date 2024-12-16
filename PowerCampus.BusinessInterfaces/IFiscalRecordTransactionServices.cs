// --------------------------------------------------------------------
// <copyright file="IFiscalRecordTransactionServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System;

namespace PowerCampus.BusinessInterfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IFiscalRecordTransactionServices
    {
        /// <summary>
        /// Deletes the fiscal record transaction.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        int DeleteFiscalRecordTransaction(Guid guid);

        /// <summary>
        /// Gets the fiscal record permissions.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        Account GetFiscalRecordPermissions(string userName);

        /// <summary>
        /// Gets the fiscal record transaction by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        Account GetFiscalRecordTransactionByGuid(Guid guid);
    }
}