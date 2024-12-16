// --------------------------------------------------------------------
// <copyright file="OperatorsController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business.ElectronicCertificate;
using PowerCampus.BusinessInterfaces.ElectronicCertificate;
using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers.ElectronicCertificate
{
    /// <summary>
    /// Operators Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class OperatorsECController : ApiController
    {
        private readonly IECOperatorServices _operatorServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorsECController"/> class.
        /// </summary>
        public OperatorsECController() => _operatorServices = new ECOperatorServices();

        /// <summary>
        /// Creates the operators.
        /// </summary>
        /// <param name="operators">The operators.</param>
        /// <returns></returns>
        [Route("api/EC/Operators/Save")]
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
        [Route("api/EC/Operators/Permissions")]
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
        [Route("api/EC/Operators/Delete/{operatorId}")]
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
        [Route("api/EC/Operators/Institutions/Delete/{institutionId}/{operatorId}")]
        [HttpDelete]
        [Authorize]
        public IHttpActionResult DeleteInstitution(string institutionId, string operatorId)
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
        [Route("api/EC/Operators/Permissions/Delete/{operatorId}/{grantedOperatorId}")]
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
        [Route("api/EC/Operators/Institutions")]
        [HttpGet]
        public IHttpActionResult Institutions()
        {
            List<InstitutionCampusOperator> institutionOptions = _operatorServices.GetInstitutions();
            if (institutionOptions == null)
                return NotFound();
            return Ok(institutionOptions);
        }

        /// <summary>
        /// Search Operators the specified operator identifier for the autocomplete option.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        [Route("api/EC/Operator/{operatorId}")]
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
        [Route("api/EC/Operators/Edit/{operatorId}")]
        [HttpGet]
        public IHttpActionResult OperatorDetail(string operatorId)
        {
            List<OperatorsList> operatorsLists = _operatorServices.GetOperator(operatorId);
            if (operatorsLists == null)
                return NotFound();
            return Ok(operatorsLists);
        }

        /// <summary>
        /// Search Operators permissions the specified operator identifier for the autocomplete option.
        /// </summary>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        [Route("api/EC/OperatorPerm/{operatorId}")]
        [HttpGet]
        public IHttpActionResult OperatorPerm(string operatorId)
        {
            List<OperatorsList> operatorsList = _operatorServices.SearchOperatorsPerm(operatorId);
            if (operatorsList == null)
                return NotFound();
            return Ok(operatorsList);
        }

        /// <summary>
        /// Operatorses this instance.
        /// </summary>
        /// <returns></returns>
        [Route("api/EC/Operators/Operator")]
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
        [Route("api/EC/Permissions/Edit/{operatorId}")]
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
        [Route("api/EC/Permissions/List/{operatorId}")]
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