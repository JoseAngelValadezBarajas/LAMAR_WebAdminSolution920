// --------------------------------------------------------------------
// <copyright file="IECTransactionServices.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System;

namespace PowerCampus.BusinessInterfaces.ElectronicCertificate
{
    /// <summary>
    /// IElectronicCertificateTransactionServices
    /// </summary>
    public interface IECTransactionServices
    {
        /// <summary>
        /// Deletes the transaction.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        void DeleteTransaction(Guid guid);

        /// <summary>
        /// Gets the transaction by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        Account GetTransactionByGuid(Guid guid);
    }
}