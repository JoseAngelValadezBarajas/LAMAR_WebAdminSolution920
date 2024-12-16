// --------------------------------------------------------------------
// <copyright file="IReceiverServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IReceiverServices
    {
        /// <summary>
        /// Deletes the tax payer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        bool DeleteTaxpayer(int id);

        /// <summary>
        /// Gets the tax payer by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="foreignId">The foreign identifier.</param>
        /// <returns></returns>
        Receiver GetTaxPayerbyId(int id, int? foreignId);

        /// <summary>
        /// Gets the tax payers.
        /// </summary>
        /// <param name="taxpayerId">The taxpayer identifier.</param>
        /// <param name="keyword">The keyword for the search.</param>
        /// <returns></returns>
        List<Receiver> GetTaxPayers(string taxpayerId, string keyword = null);

        /// <summary>
        /// Saves the tax payers.
        /// </summary>
        /// <param name="Receiver">The receiver model.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        int SaveTaxPayers(Receiver Receiver, string userName);

        /// <summary>
        /// Updates the tax payer.
        /// </summary>
        /// <param name="Receiver">The receiver.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        int UpdateTaxPayer(Receiver Receiver, string userName);
    }
}