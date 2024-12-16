// --------------------------------------------------------------------
// <copyright file="ReceiverServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.BusinessInterfaces;
using PowerCampus.DataAccess;
using PowerCampus.Entities;
using System.Collections.Generic;

namespace PowerCampus.Business
{
    /// <summary>
    /// ReceiverServices Class
    /// </summary>
    public class ReceiverServices : IReceiverServices
    {
        /// <summary>
        /// The _receiver da
        /// </summary>
        private readonly ReceiverDA _receiverDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceiverServices"/> class.
        /// </summary>
        public ReceiverServices()
        {
            _receiverDA = new ReceiverDA();
        }

        /// <summary>
        /// Deletes the tax payer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public bool DeleteTaxpayer(int id)
        {
            return _receiverDA.DeleteTaxpayer(id);
        }

        /// <summary>
        /// Gets the tax payer by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="foreignId">The foreign identifier.</param>
        /// <returns></returns>
        public Receiver GetTaxPayerbyId(int id, int? foreignId)
        {
            // _logService.Debug("GetTaxPayerbyId starts");
            Receiver receiver = _receiverDA.GetTaxPayerbyId(id, foreignId);
            // _logService.Debug("GetTaxPayerbyId ends");
            return receiver;
        }

        /// <summary>
        /// Gets the tax payers.
        /// </summary>
        /// <param name="taxpayerId">The taxpayer identifier.</param>
        /// <param name="keyword">The keyword.</param>
        /// <returns></returns>
        public List<Receiver> GetTaxPayers(string taxpayerId, string keyword = null)
        {
            List<Receiver> lstReceiver = _receiverDA.GetTaxPayers(taxpayerId, keyword);
            return lstReceiver;
        }

        /// <summary>
        /// Saves the tax payers.
        /// </summary>
        /// <param name="Receiver">The receiver model.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public int SaveTaxPayers(Receiver Receiver, string userName)
        {
            // _logService.Debug("SaveTaxPayers starts");
            int taxpayerId = _receiverDA.SaveTaxPayers(Receiver, userName);
            // _logService.Debug("SaveTaxPayers ends");
            return taxpayerId;
        }

        /// <summary>
        /// Updates the tax payers.
        /// </summary>
        /// <param name="Receiver">The receiver.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public int UpdateTaxPayer(Receiver Receiver, string userName)
        {
            // _logService.Debug("UpdateTaxPayer starts");
            int taxpayerId = _receiverDA.UpdateTaxPayer(Receiver, userName);
            // _logService.Debug("UpdateTaxPayer ends");
            return taxpayerId;
        }
    }
}