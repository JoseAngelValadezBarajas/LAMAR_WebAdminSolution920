// --------------------------------------------------------------------
// <copyright file="OperatorServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.DataAccess.ElectronicDegree;
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;

namespace PowerCampus.Business.ElectronicDegree
{
    /// <summary>
    /// Operator Services
    /// </summary>
    public class OperatorServices : IOperatorServices
    {
        private readonly OperatorsDA _operatorDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorServices"/> class.
        /// </summary>
        public OperatorServices() => _operatorDA = new OperatorsDA();

        /// <summary>
        /// Creates the operators.
        /// </summary>
        /// <param name="operators">The operators.</param>
        public void CreateOperators(OperatorsList operators) => _operatorDA.CreateOperators(operators);

        /// <summary>
        /// Creates the permissions.
        /// </summary>
        /// <param name="operators">The operators.</param>
        public void CreatePermissions(OperatorsList operators) => _operatorDA.CreatePermissions(operators);

        /// <summary>
        /// Deletes the specified operator identifier.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        public void Delete(string operatorId) => _operatorDA.Delete(operatorId);

        /// <summary>
        /// Deletes the institution.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        public void DeleteInstitution(int institutionId, string operatorId) => _operatorDA.DeleteInstitution(institutionId, operatorId);

        /// <summary>
        /// Deletes the permission.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="grantedOperatorId">The granted operator identifier.</param>
        public void DeletePermission(string operatorId, string grantedOperatorId) => _operatorDA.DeletePermission(operatorId, grantedOperatorId);

        /// <summary>
        /// Gets the institutions.
        /// </summary>
        /// <returns></returns>
        public List<InstitutionOptions> GetInstitutions() => _operatorDA.GetInstitutions();

        /// <summary>
        /// Gets the institution signers.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        public List<InstitutionSignerList> GetInstitutionSigners(int institutionId) => _operatorDA.GetInstitutionSigners(institutionId);

        /// <summary>
        /// Gets the operator.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public List<OperatorsList> GetOperator(string operatorId) => _operatorDA.GetOperator(operatorId);

        /// <summary>
        /// Gets the operators.
        /// </summary>
        /// <returns></returns>
        public List<OperatorsList> GetOperators() => _operatorDA.GetOperators();

        /// <summary>
        /// Gets the operators permissions.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public List<OperatorsList> GetOperatorsPermissions(string operatorId) => _operatorDA.GetOperatorPermissions(operatorId);

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public OperatorsList GetPermissions(string operatorId) => _operatorDA.GetPermissions(operatorId);

        /// <summary>
        /// Searches the operators.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public List<OperatorsList> SearchOperators(string operatorId) => _operatorDA.SearchOperators(operatorId);
    }
}