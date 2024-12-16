// --------------------------------------------------------------------
// <copyright file="ElectronicDegreeTransactionServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.DataAccess.ElectronicDegree;
using PowerCampus.Entities;
using System;

namespace PowerCampus.Business.ElectronicDegree
{
    /// <summary>
    /// ElectronicDegree Transaction Services
    /// </summary>
    public class ElectronicDegreeTransactionServices : IElectronicDegreeTransactionServices
    {
        private readonly ElectronicDegreeTransactionDA _electronicDegreeTransactionDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElectronicDegreeTransactionServices"/> class.
        /// </summary>
        public ElectronicDegreeTransactionServices()
        {
            _electronicDegreeTransactionDA = new ElectronicDegreeTransactionDA();
        }

        /// <summary>
        /// Deletes the electronic degree transaction.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public void DeleteElectronicDegreeTransaction(Guid guid)
        {
            _electronicDegreeTransactionDA.DeleteElectronicDegreeTransaction(guid);
        }

        /// <summary>
        /// Gets the electronic degree permissions.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public Account GetElectronicDegreePermissions(string userName)
        {
            Account account = _electronicDegreeTransactionDA.GetElectronicDegreePermissions(userName);
            return account;
        }

        /// <summary>
        /// Gets the electronic degree transaction by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public Account GetElectronicDegreeTransactionByGuid(Guid guid)
        {
            Account account = _electronicDegreeTransactionDA.GetElectronicDegreeTransactionByGuid(guid);
            return account;
        }
    }
}