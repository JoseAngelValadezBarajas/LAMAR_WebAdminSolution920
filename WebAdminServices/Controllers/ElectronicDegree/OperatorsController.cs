// --------------------------------------------------------------------
// <copyright file="OperatorsController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business.ElectronicDegree;
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers.ElectronicDegree
{
    /// <summary>
    /// Operators Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class OperatorsController : ApiController
    {
        private readonly IOperatorServices _operatorServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorsController"/> class.
        /// </summary>
        public OperatorsController() => _operatorServices = new OperatorServices();

        /// <summary>
        /// Creates the operators.
        /// </summary>
        /// <param name="operators">The operators.</param>
        /// <returns></returns>
        [Route("api/Operators/Create")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult CreateOperators(OperatorsList operators)
        {
            _operatorServices.CreateOperators(operators);
            return Ok();
        }

        /// <summary>
        /// Creates the permissions.
        /// </summary>
        /// <param name="operators">The operators.</param>
        /// <returns></returns>
        [Route("api/Permissions/Create")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult CreatePermissions(OperatorsList operators)
        {
            _operatorServices.CreatePermissions(operators);
            return Ok();
        }

        /// <summary>
        /// Deletes the specified operator identifier.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        [Route("api/Operators/Delete/{operatorId}")]
        [HttpDelete]
        [Authorize]
        public IHttpActionResult Delete(string operatorId)
        {
            _operatorServices.Delete(operatorId);
            return Ok();
        }

        /// <summary>
        /// Deletes the institution.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        [Route("api/Operators/Institutions/Delete/{institutionId}/{operatorId}")]
        [HttpDelete]
        [Authorize]
        public IHttpActionResult DeleteInstitution(int institutionId, string operatorId)
        {
            _operatorServices.DeleteInstitution(institutionId, operatorId);
            return Ok();
        }

        /// <summary>
        ///Deletes the permission.
        /// </summary>
        /// <param name="operatorId"></param>
        /// <param name="grantedOperatorId"></param>
        /// <returns></returns>
        [Route("api/Permissions/Delete/{operatorId}/{grantedOperatorId}")]
        [HttpDelete]
        [Authorize]
        public IHttpActionResult DeletePermission(string operatorId, string grantedOperatorId)
        {
            _operatorServices.DeletePermission(operatorId, grantedOperatorId);
            return Ok();
        }

        /// <summary>
        /// Institutionses this instance.
        /// </summary>
        /// <returns></returns>
        [Route("api/Operators/Institutions")]
        [HttpGet]
        public IHttpActionResult Institutions()
        {
            List<InstitutionOptions> institutionOptions = _operatorServices.GetInstitutions();
            if (institutionOptions == null)
                return NotFound();
            return Ok(institutionOptions);
        }

        /// <summary>
        /// Institutions the signers.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        [Route("api/Operators/InstitutionSigners/{institutionId}")]
        [HttpGet]
        public IHttpActionResult InstitutionSigners(int institutionId)
        {
            List<InstitutionSignerList> institutionSignerLists = _operatorServices.GetInstitutionSigners(institutionId);
            if (institutionSignerLists == null)
                return NotFound();
            return Ok(institutionSignerLists);
        }

        /// <summary>
        /// Search Operators the specified operator identifier for the autocomplete option.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        [Route("api/Operator/{operatorId}")]
        [HttpGet]
        public IHttpActionResult Operator(string operatorId)
        {
            List<OperatorsList> operatorsList = _operatorServices.SearchOperators(operatorId);
            if (operatorsList == null)
                return NotFound();
            return Ok(operatorsList);
        }

        /// <summary>
        /// Get the operator information to edit view.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        [Route("api/Operators/Edit/{operatorId}")]
        [HttpGet]
        public IHttpActionResult OperatorDetail(string operatorId)
        {
            List<OperatorsList> operatorsLists = _operatorServices.GetOperator(operatorId);
            if (operatorsLists == null)
                return NotFound();
            return Ok(operatorsLists);
        }

        /// <summary>
        /// Operatorses this instance.
        /// </summary>
        /// <returns></returns>
        [Route("api/Operators")]
        [HttpGet]
        public IHttpActionResult Operators()
        {
            List<OperatorsList> operatorsLists = _operatorServices.GetOperators();
            if (operatorsLists == null)
                return NotFound();
            return Ok(operatorsLists);
        }

        /// <summary>
        /// Permissionses the specified operator identifier.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        [Route("api/Permissions/{operatorId}")]
        [HttpGet]
        public IHttpActionResult Permissions(string operatorId)
        {
            List<OperatorsList> permissionsLists = _operatorServices.GetOperatorsPermissions(operatorId);
            if (permissionsLists == null)
                return NotFound();
            return Ok(permissionsLists);
        }

        /// <summary>
        /// Permissionses the list.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        [Route("api/Permissions/List/{operatorId}")]
        [HttpGet]
        public IHttpActionResult PermissionsList(string operatorId)
        {
            OperatorsList permissionsLists = _operatorServices.GetPermissions(operatorId);
            if (permissionsLists == null)
                return NotFound();
            return Ok(permissionsLists);
        }
    }
}