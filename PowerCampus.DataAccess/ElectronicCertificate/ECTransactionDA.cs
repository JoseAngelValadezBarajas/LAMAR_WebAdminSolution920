// --------------------------------------------------------------------
// <copyright file="ElectronicCertificateTransactionDA.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.Entities;
using System;
using System.Data;
using System.Linq;

namespace PowerCampus.DataAccess.ElectronicCertificate
{
    /// <summary>
    /// ElectronicCertificateTransactionDA
    /// </summary>
    public class ECTransactionDA
    {
        /// <summary>
        /// The factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ECTransactionDA"/> class.
        /// </summary>
        public ECTransactionDA() => _factory = new DatabaseProviderFactory();

        /// <summary>
        /// Deletes the transaction.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        public void DeleteTransaction(Guid guid)
        {
            try
            {
                ElectronicCertificateContext context = new ElectronicCertificateContext();
                Transaction query = (from ect in context.Transactions
                                     where ect.Guid == guid
                                     select ect).FirstOrDefault();

                context.Transactions.DeleteOnSubmit(query);
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - ElectronicCertificateTransactionDA - DeleteTransaction", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the transaction by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public Account GetTransactionByGuid(Guid guid)
        {
            try
            {
                Account Account = new Account();

                ElectronicCertificateContext ecContext = new ElectronicCertificateContext();
                Transaction transactionQuery = (from ect in ecContext.Transactions
                                                where ect.Guid == guid
                                                select ect).FirstOrDefault();

                ElectronicDegreeContext edContext = new ElectronicDegreeContext();
                ABT_USER abtQuery = (from abtUsr in edContext.ABT_USERs
                                     where abtUsr.OPERATOR_ID == transactionQuery.UserName
                                     select abtUsr).FirstOrDefault();

                if (abtQuery != null)
                {
                    Account.GUID = guid;
                    Account.UserName = transactionQuery.UserName;
                    Account.CreateDateTime = transactionQuery.CreateDatetime;
                    return Account;
                }
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - ElectronicCertificateTransactionDA - GetElectronicCertificateTransactionByGuid",
                    string.Format("GetElectronicCertificateTransactionByGuid - GUID {0} number doesn't exist in current database", guid));

                return null;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - ElectronicCertificateTransactionDA - GetTransactionByGuid", ex.Message);
                throw;
            }
        }
    }
}