// --------------------------------------------------------------------
// <copyright file="GenerateController.cs" company="Ellucian">
//     Copyright 2020 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business.ElectronicDegree;
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers.ElectronicDegree
{
    /// <summary>
    /// Generate Electronic Degree Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class GenerateController : ApiController
    {
        /// <summary>
        /// The generate services
        /// </summary>
        private readonly IGenerateServices _generateServices;

        /// <summary>
        /// The xml services for Electronic Degree
        /// </summary>
        private readonly ElectronicDegreeXmlServices xmlServices = new ElectronicDegreeXmlServices();

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateController" /> class.
        /// </summary>
        public GenerateController() => _generateServices = new GenerateServices();

        /// <summary>
        /// Backgrounds the studies.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <returns></returns>
        [Route("api/BackgroundStudies/{peopleId}")]
        [HttpGet]
        public IHttpActionResult BackgroundStudies(string peopleId)
        {
            List<BackgroundStudies> backgroundStudies = _generateServices.GetBackgroundStudies(peopleId);
            if (backgroundStudies == null)
                return NotFound();
            return Ok(backgroundStudies);
        }

        /// <summary>
        /// Creates the specified electronic degree information.
        /// </summary>
        /// <param name="electronicDegreeInfo">The electronic degree information.</param>
        /// <returns></returns>
        [Route("api/ElectronicDegree/Create")]
        [HttpPost]
        public IHttpActionResult Create(ElectronicDegreeInfo electronicDegreeInfo)
        {
            int result = _generateServices.Create(electronicDegreeInfo);

            TituloElectronico tituloElectronico = MapElectronicDegreeInfo(electronicDegreeInfo);
            string electronicDegreeXml = xmlServices.CreateXmlRequestForElectronicDegree(tituloElectronico);
            string originalString = string.Empty;
            foreach (TituloElectronicoFirmaResponsable firmante in tituloElectronico.FirmaResponsables)
            {
                originalString += string.Format("{0}\r\n", xmlServices.CreateOriginalString(tituloElectronico, firmante));
            }
            _generateServices.UpdateXml(electronicDegreeXml, originalString, result);
            return Ok(result);
        }

        /// <summary>
        /// Institutions the folio.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="ElectronicDegreeInstMajorId">The electronic degree inst major identifier.</param>
        /// <returns></returns>
        [Route("api/InstitutionFolio/{peopleId}/{ElectronicDegreeInstMajorId}")]
        [HttpGet]
        public IHttpActionResult InstitutionFolio(string peopleId, int ElectronicDegreeInstMajorId)
        {
            string folio = _generateServices.GetInstitutionFolio(peopleId, ElectronicDegreeInstMajorId);
            return Ok(folio);
        }

        /// <summary>
        /// Institutions the major.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        [Route("api/InstitutionMajor/{peopleId}/{operatorId}")]
        [HttpGet]
        public IHttpActionResult InstitutionMajor(string peopleId, string operatorId)
        {
            List<InstitutionMajor> institutionMajorsList = _generateServices.GetInstitutionMajor(peopleId, operatorId);
            if (institutionMajorsList == null)
                return NotFound();
            return Ok(institutionMajorsList);
        }

        /// <summary>
        /// Institutions the signer.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        [Route("api/InstitutionSigner/{institutionId}")]
        [HttpGet]
        public IHttpActionResult InstitutionSigner(int institutionId)
        {
            List<InstitutionSignerList> institutionSignerList = _generateServices.GetInstitutionSigners(institutionId);
            if (institutionSignerList == null)
                return NotFound();
            return Ok(institutionSignerList);
        }

        /// <summary>
        /// Issuings the degree.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="instMajorId">The inst major identifier.</param>
        /// <param name="transcriptDegreeId">The transcript degree identifier.</param>
        /// <returns></returns>
        [Route("api/IssuingDegree/{peopleId}/{instMajorId}/{transcriptDegreeId}")]
        [HttpGet]
        public IHttpActionResult IssuingDegree(string peopleId, int instMajorId, int transcriptDegreeId)
        {
            IssuingDegree issuingDegree = _generateServices.GetIssuingDegree(peopleId, instMajorId, transcriptDegreeId);
            if (issuingDegree == null)
                return NotFound();
            return Ok(issuingDegree);
        }

        /// <summary>
        /// Peoples the specified people identifier.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <returns></returns>
        [Route("api/People/{peopleId}")]
        [HttpGet]
        public IHttpActionResult People(string peopleId)
        {
            List<PeopleModel> peopleList = _generateServices.GetPeople(peopleId);
            if (peopleList == null)
                return NotFound();
            return Ok(peopleList);
        }

        /// <summary>
        /// Maps the electronic degree information.
        /// </summary>
        /// <param name="edi">The edi.</param>
        /// <returns></returns>
        private TituloElectronico MapElectronicDegreeInfo(ElectronicDegreeInfo edi)
        {
            TituloElectronico tituloElectronico = new TituloElectronico();
            tituloElectronico.folioControl = edi.Folio;
            //mod addv 14082024
            tituloElectronico.FirmaResponsables = edi.Signer
                                                     .Select(s => new TituloElectronicoFirmaResponsable()
                                                     {
                                                         nombre = s.EdSignerName,
                                                         primerApellido = s.EdSignerFirstSurname,
                                                         segundoApellido = s.EdSignerSecondSurname,
                                                         curp = s.EdSignerCurp,
                                                         idCargo = s.EdSignerLaborPositionCode.ToString(),
                                                         cargo = s.EdSignerLaborPosition,
                                                         abrTitulo = s.EdAbreviationTitle,                                                                                                                                                                   
                                                         thumbprint = s.EdSignerThumprint
                                                     })
                                                     .ToArray();
            //mod addv 14082024
            tituloElectronico.Antecedente = new TituloElectronicoAntecedente()
            {
                institucionProcedencia = edi.OriginInstitution,
                idTipoEstudioAntecedente = edi.BackgroundStudyTypeCode.ToString(),
                tipoEstudioAntecedente = edi.BackgroundStudyType,
                idEntidadFederativa = edi.OriginInstFederalEntityCode.ToString(),
                entidadFederativa = edi.OriginInstFederalEntity,                
                fechaInicio = edi.BackgroundStudyStartDate,
                fechaTerminacion = edi.BackgroundStudyEndDate,                
                noCedula = edi.LicenseNumber
            };

            //mod addv 14082024
            tituloElectronico.Carrera = new TituloElectronicoCarrera()
            {
                cveCarrera = edi.MajorCode,
                nombreCarrera = edi.Major,                
                fechaInicio = edi.MajorStartDate,
                fechaTerminacion = edi.MajorEndDate,
                idAutorizacionReconocimiento = edi.AuthorizationTypeCode.ToString(),
                autorizacionReconocimiento = edi.AuthorizationType,
                numeroRvoe = edi.RvoeAgreementNumber
            };

            //mod addv 14082024
            tituloElectronico.Expedicion = new TituloElectronicoExpedicion()
            {
                fechaExpedicion = edi.ExpeditionDate,
                idModalidadTitulacion = edi.GraduationRequirementCode.ToString(),
                modalidadTitulacion = edi.GraduationRequirement,
                fechaExamenProfesional = edi.ExaminationDate,
                fechaExencionExamenProfesional = edi.ExaminationExemptionDate,
                cumplioServicioSocial = edi.FulfilledSocialService ? "1" : "0",
                idFundamentoLegalServicioSocial = edi.LegalBaseCode.ToString(),
                fundamentoLegalServicioSocial = edi.LegalBase,
                idEntidadFederativa = edi.FederalEntityCode.ToString(),
                entidadFederativa = edi.FederalEntity
                
            };

            tituloElectronico.Institucion = new TituloElectronicoInstitucion()
            { cveInstitucion = edi.InstitutionCode, nombreInstitucion = edi.InstitutionName };

            //mod addv 14082024
            tituloElectronico.Profesionista = new TituloElectronicoProfesionista()
            {                
                curp = edi.Curp,
                nombre = edi.Name,
                primerApellido = edi.FirstSurname,
                segundoApellido = edi.SecondSurname,
                correoElectronico = edi.Email
            };

            return tituloElectronico;
        }
    }
}