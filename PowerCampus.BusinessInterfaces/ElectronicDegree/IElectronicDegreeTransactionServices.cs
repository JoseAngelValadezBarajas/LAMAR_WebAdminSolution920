// --------------------------------------------------------------------
// <copyright file="IElectronicDegreeTransactionServices.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System;

namespace PowerCampus.BusinessInterfaces.ElectronicDegree
{
    /// <summary>
    /// IElectronicDegree Transaction Services
    /// </summary>
    public interface IElectronicDegreeTransactionServices
    {
        /// <summary>
        /// Deletes the electronic degree transaction.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        void DeleteElectronicDegreeTransaction(Guid guid);

        /// <summary>
        /// Gets the electronic degree permissions.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        Account GetElectronicDegreePermissions(string userName);

        /// <summary>
        /// Gets the electronic degree transaction by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        Account GetElectronicDegreeTransactionByGuid(Guid guid);
    }
}