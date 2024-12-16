// --------------------------------------------------------------------
// <copyright file="SignersController.cs" company="Ellucian">
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
    /// Signers Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class SignersController : ApiController
    {
        private readonly ISignerServices _signerServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignersController"/> class.
        /// </summary>
        public SignersController() => _signerServices = new SignerServices();

        /// <summary>
        /// Create Signer.
        /// </summary>
        /// <param name="signerList"></param>
        [Route("api/Signers/Create")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult Create(SignerList signerList)
        {
            _signerServices.CreateSigner(signerList);
            return Ok();
        }

        /// <summary>
        /// Gets the edit signer.
        /// </summary>
        /// <param name="signerId">The signer identifier.</param>
        /// <returns></returns>
        [Route("api/Signers/Edit")]
        [HttpGet]
        public IHttpActionResult Edit(int signerId)
        {
            SignerList signerList = _signerServices.GetEditSigner(signerId);
            if (signerList == null)
                return NotFound();
            return Ok(signerList);
        }

        /// <summary>
        /// Gets the signers.
        /// </summary>
        /// <returns></returns>
        [Route("api/Index")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            List<SignerList> signer = _signerServices.GetSigners();
            if (signer == null)
                return NotFound();
            return Ok(signer);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <returns></returns>
        [Route("api/Signers/LaborCatalog")]
        [HttpGet]
        public IHttpActionResult LaborCatalog()
        {
            SignersModel signer = _signerServices.GetLaborCatalog();
            if (signer == null)
                return NotFound();
            return Ok(signer);
        }

        /// <summary>
        /// Gets the title abbreviation.
        /// </summary>
        /// <returns></returns>
        [Route("api/Signers/TitleAbbreviation")]
        [HttpGet]
        public IHttpActionResult TitleAbbreviation()
        {
            SignersModel signer = _signerServices.GetTitleAbbreviation();
            if (signer == null)
                return NotFound();
            return Ok(signer);
        }

        /// <summary>
        /// Updates the signer.
        /// </summary>
        /// <param name="signerList">The signer list.</param>
        /// <returns></returns>
        [Route("api/Signers/Update")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult Update(SignerList signerList)
        {
            _signerServices.UpdateSigner(signerList);
            return Ok();
        }

        /// <summary>
        /// Validates the curp.
        /// </summary>
        /// <param name="Curp">The curp.</param>
        /// <returns></returns>
        [Route("api/Signers/ValidateCurp/{Curp}")]
        [HttpGet]
        public IHttpActionResult ValidateCurp(string Curp)
        {
            bool Exists = _signerServices.ValidateCurp(Curp);
            return Ok(Exists);
        }

        /// <summary>
        /// Validates the thumprint.
        /// </summary>
        /// <param name="Thumprint">The thumprint.</param>
        /// <returns></returns>
        [Route("api/Signers/ValidateThumprint/{Thumprint}")]
        [HttpGet]
        public IHttpActionResult ValidateThumprint(string Thumprint)
        {
            bool IsValid = _signerServices.ExistThumprint(Thumprint);
            bool Exist = _signerServices.ValidateThumprint(Thumprint);

            if (IsValid && !Exist)
                return Ok(true);

            return Ok(false);
        }
    }
}