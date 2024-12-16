// --------------------------------------------------------------------
// <copyright file="SignersController.cs" company="Ellucian">
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
    /// Responsibles Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class ResponsiblesController : ApiController
    {
        #region Private Fields

        /// <summary>
        /// The ec certificate services
        /// </summary>
        private readonly IECCertificateServices _certificateServices;

        /// <summary>
        /// The e c responsible service
        /// </summary>
        private readonly IECResponsibleService _responsibleServices;

        #endregion Private Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponsiblesController"/> class.
        /// </summary>
        public ResponsiblesController()
        {
            _responsibleServices = new ECResponsibleServices();
            _certificateServices = new ECCertificateServices();
        }

        /// <summary>
        /// Creates the specified responsible list.
        /// </summary>
        /// <param name="responsibleList">The responsible list.</param>
        /// <returns></returns>
        [Route("api/EC/Responsibles/Create")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult Create(ResponsibleList responsibleList)
        {
            _responsibleServices.CreateResponsible(responsibleList);
            return Ok();
        }

        /// <summary>
        /// Edits the specified responsible identifier.
        /// </summary>
        /// <param name="responsibleId">The responsible identifier.</param>
        /// <returns></returns>
        [Route("api/EC/Responsibles/Edit/{responsibleId}")]
        [HttpGet]
        public IHttpActionResult Edit(int responsibleId)
        {
            ResponsibleList responsibleList = _responsibleServices.GetEditResponsible(responsibleId);
            if (responsibleList == null)
                return NotFound();
            return Ok(responsibleList);
        }

        /// <summary>
        /// Get responsibles.
        /// </summary>
        /// <returns></returns>
        [Route("api/EC/Responsibles/Index")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            List<ResponsibleList> responsible = _responsibleServices.GetResponsibles();
            if (responsible == null)
                return NotFound();
            return Ok(responsible);
        }

        /// <summary>
        /// Labors the catalog.
        /// </summary>
        /// <returns></returns>
        [Route("api/EC/Responsibles/PositionCatalog")]
        [HttpGet]
        public IHttpActionResult PositionCatalog()
        {
            ResponsibleModel responsible = _responsibleServices.GetPositionCatalog();
            if (responsible == null)
                return NotFound();
            return Ok(responsible);
        }

        /// <summary>
        /// Get responsibles.
        /// </summary>
        /// <returns></returns>
        [Route("api/EC/Responsibles/Names")]
        [HttpGet]
        public IHttpActionResult Responsibles()
        {
            List<ResponsibleName> responsible = _responsibleServices.GetResponsibleNames();
            if (responsible == null)
                return NotFound();
            return Ok(responsible);
        }

        /// <summary>
        /// Updates the specified responsible list.
        /// </summary>
        /// <param name="responsibleList">The responsible list.</param>
        /// <returns></returns>
        [Route("api/EC/Responsibles/Update")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult Update(ResponsibleList responsibleList)
        {
            _responsibleServices.UpdateResponsible(responsibleList);
            return Ok();
        }

        /// <summary>
        /// Validates the curp.
        /// </summary>
        /// <param name="Curp">The curp.</param>
        /// <returns></returns>
        [Route("api/EC/Responsibles/ValidateCurp/{Curp}")]
        [HttpGet]
        public IHttpActionResult ValidateCurp(string Curp)
        {
            bool Exists = _responsibleServices.ValidateCurp(Curp);
            return Ok(Exists);
        }

        /// <summary>
        /// Validates the thumbprint for the specified responsible id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <returns></returns>
        [Route("api/EC/Responsibles/{id}/Thumbprint/{thumbprint}")]
        [HttpGet]
        public IHttpActionResult ValidateThumbprint(int id, string thumbprint)
        {
            if (string.IsNullOrEmpty(thumbprint))
            {
                return NotFound();
            }
            ThumbprintStatus status = _certificateServices.GetThumbprintStatus(thumbprint);
            if (status == ThumbprintStatus.Installed)
            {
                if (_responsibleServices.IsThumbprintAssigned(id, thumbprint))
                {
                    status = ThumbprintStatus.Assigned;
                }
            }

            return Ok(status);
        }
    }
}