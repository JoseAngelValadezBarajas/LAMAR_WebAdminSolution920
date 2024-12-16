// --------------------------------------------------------------------
// <copyright file="InstitutionCampusesController.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business.ElectronicCertificate;
using PowerCampus.BusinessInterfaces.ElectronicCertificate;
using PowerCampus.Entities.ElectronicCertificate;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers.ElectronicCertificate
{
    /// <summary>
    /// InstitutionCampuses Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    [Authorize]
    public class InstitutionCampusesController : ApiController
    {
        private readonly IECInstitutionCampusService _institutionCampusServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstitutionCampusesController"/> class.
        /// </summary>
        public InstitutionCampusesController() => _institutionCampusServices = new ECInstitutionCampusService();

        /// <summary>
        /// Gets the institutions.
        /// </summary>
        /// <returns></returns>
        [Route("api/EC/InstitutionCampuses")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            InstitutionCampuses institutionCampuses = _institutionCampusServices.GetECInstitutionCampus();
            if (institutionCampuses == null)
                return NotFound();
            return Ok(institutionCampuses);
        }

        /// <summary>
        /// Gets the institutions.
        /// </summary>
        /// <param name="institutionCampuses">The institution campuses.</param>
        /// <returns></returns>
        [Route("api/EC/InstitutionCampuses/Save")]
        [HttpPost]
        public IHttpActionResult Save(InstitutionCampuses institutionCampuses)
        {
            foreach (InstitutionCampus institutionCampus in institutionCampuses.InstitutionCampus)
            {
                if (institutionCampus.InstitutionCampusId > 0)
                    _institutionCampusServices.UpdateECInstitutionCampus(institutionCampus, institutionCampuses.OperatorId);
                else
                {
                    if (!string.IsNullOrEmpty(institutionCampus.InstitutionCodeId)
                        && !string.IsNullOrEmpty(institutionCampus.CampusCodeId)
                        && (!string.IsNullOrEmpty(institutionCampus.FolioFormat)
                            || !string.IsNullOrEmpty(institutionCampus.CampusSepCode)
                            || !string.IsNullOrEmpty(institutionCampus.InstitutionSepId)
                            || institutionCampus.ResponsibleId > 0
                            || institutionCampus.FederalEntityId > 0)
                        )
                        _institutionCampusServices.CreateECInstitutionCampus(institutionCampus, institutionCampuses.OperatorId);
                }
            }
            return Ok();
        }
    }
}