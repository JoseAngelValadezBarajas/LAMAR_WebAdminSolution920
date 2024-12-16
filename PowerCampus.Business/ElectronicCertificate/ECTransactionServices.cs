// --------------------------------------------------------------------
// <copyright file="ECTransactionServices.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.BusinessInterfaces.ElectronicCertificate;
using PowerCampus.DataAccess.ElectronicCertificate;
using PowerCampus.Entities;
using System;

namespace PowerCampus.Business.ElectronicCertificate
{
    /// <summary>
    /// ElectronicCertificateTransactionServices
    /// </summary>
    public class ECTransactionServices : IECTransactionServices
    {
        private readonly ECTransactionDA _electronicCertificateTransactionDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="ECTransactionServices"/> class.
        /// </summary>
        public ECTransactionServices()
        {
            _electronicCertificateTransactionDA = new ECTransactionDA();
        }

        /// <summary>
        /// Deletes the transaction.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        public void DeleteTransaction(Guid guid)
        {
            _electronicCertificateTransactionDA.DeleteTransaction(guid);
        }

        /// <summary>
        /// Gets the transaction by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public Account GetTransactionByGuid(Guid guid)
        {
            Account account = _electronicCertificateTransactionDA.GetTransactionByGuid(guid);
            return account;
        }
    }
}