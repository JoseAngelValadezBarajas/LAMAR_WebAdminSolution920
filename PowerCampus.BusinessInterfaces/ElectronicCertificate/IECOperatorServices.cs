﻿// --------------------------------------------------------------------
// <copyright file="IECOperatorServices.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces.ElectronicCertificate
{
    /// <summary>
    /// IOperator Services
    /// </summary>
    public interface IECOperatorServices
    {
        /// <summary>
        /// Creates the operators.
        /// </summary>
        /// <param name="operators">The operators.</param>
        void CreateOperators(OperatorsList operators);

        /// <summary>
        /// Creates the permissions.
        /// </summary>
        /// <param name="operators">The operators.</param>
        void CreatePermissions(OperatorsList operators);

        /// <summary>
        /// Deletes the specified operator identifier.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        void Delete(string operatorId);

        /// <summary>
        /// Deletes the institution.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        void DeleteInstitution(string institutionId, string operatorId);

        /// <summary>
        /// Deletes the permission.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="grantedOperatorId">The granted operator identifier.</param>
        void DeletePermission(string operatorId, string grantedOperatorId);

        /// <summary>
        /// Gets the institutions.
        /// </summary>
        /// <returns></returns>
        List<InstitutionCampusOperator> GetInstitutions();

        /// <summary>
        /// Gets the operator.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        List<OperatorsList> GetOperator(string operatorId);

        /// <summary>
        /// Gets the operators.
        /// </summary>
        /// <returns></returns>
        List<OperatorsList> GetOperators();

        /// <summary>
        /// Gets the operators permissions.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        List<OperatorsList> GetOperatorsPermissions(string operatorId);

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        OperatorsList GetPermissions(string operatorId);

        /// <summary>
        /// Searches the operators.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        List<OperatorsList> SearchOperators(string operatorId);

        /// <summary>
        /// Searches the operators perm.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        List<OperatorsList> SearchOperatorsPerm(string operatorId);
    }
}