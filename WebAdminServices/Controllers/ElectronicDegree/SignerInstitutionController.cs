// --------------------------------------------------------------------
// <copyright file="SignerInstitutionController.cs" company="Ellucian">
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
    /// SignerInstitution Controller
    /// </summary>
    /// <seealso cref="ApiController" />
    [LoggingActionFilterAttribute]
    public class SignerInstitutionController : ApiController
    {
        private readonly ISignerInstitutionServices _signerInstitutionServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignerInstitutionController"/> class.
        /// </summary>
        public SignerInstitutionController() => _signerInstitutionServices = new SignerInstitutionServices();

        /// <summary>
        /// Creates the specified signer institution.
        /// </summary>
        /// <param name="signerInstitution">The signer institution.</param>
        /// <returns></returns>
        [Route("api/SignerInstitution/Create")]
        // [Authorize]
        [HttpPost]
        public IHttpActionResult Create(InstitutionList signerInstitution)
        {
            _signerInstitutionServices.CreateSignerInstitution(signerInstitution);
            return Ok();
        }

        /// <summary>
        /// Institutionses this instance.
        /// </summary>
        /// <returns></returns>
        [Route("api/Institutions")]
        [HttpGet]
        public IHttpActionResult Institutions()
        {
            List<InstitutionList> signerInstitutionList = _signerInstitutionServices.GetInstitutions();
            if (signerInstitutionList == null)
                return NotFound();
            return Ok(signerInstitutionList);
        }

        /// <summary>
        /// Institutions the signers.
        /// </summary>
        /// <returns></returns>
        [Route("api/InstitutionSigners")]
        [HttpGet]
        public IHttpActionResult InstitutionSigners()
        {
            List<InstitutionSignerList> institutionSignerLists = _signerInstitutionServices.GetInstitutionSignerList();
            if (institutionSignerLists == null)
                return NotFound();
            return Ok(institutionSignerLists);
        }

        /// <summary>
        /// Signerses this instance.
        /// </summary>
        /// <returns></returns>
        [Route("api/Signers")]
        [HttpGet]
        public IHttpActionResult Signers()
        {
            List<SignerList> signerLists = _signerInstitutionServices.GetSigners();
            if (signerLists == null)
                return NotFound();
            return Ok(signerLists);
        }
    }
}