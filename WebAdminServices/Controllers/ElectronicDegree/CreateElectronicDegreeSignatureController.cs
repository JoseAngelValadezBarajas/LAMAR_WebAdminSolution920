// --------------------------------------------------------------------
// <copyright file="CreateElectronicDegreeSignatureController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business.ElectronicDegree;
using System.Collections.Generic;
using System.Web.Http;

namespace WebAdminServices.Controllers.ElectronicDegree
{
    /// <summary>
    /// Creates the signature for the XML of ElectronicDegree
    /// </summary>
    [Route("api/v1/CreateElectronicDegreeSignature")]
    public class CreateElectronicDegreeSignatureController : ApiController
    {
        /// <summary>
        /// The xml services for Electronic Degree
        /// </summary>
        private readonly ElectronicDegreeXmlServices services = new ElectronicDegreeXmlServices();

        /// <summary>
        /// Process a POST request
        /// </summary>
        /// <param name="tituloElectronico"></param>
        /// <returns></returns>
        public IEnumerable<string> Post([FromBody] TituloElectronico tituloElectronico)
        {
            foreach (TituloElectronicoFirmaResponsable firmante in tituloElectronico.FirmaResponsables)
            {
                string originalString = services.CreateOriginalString(tituloElectronico, firmante);
                (string signature, _, _) = services.CreateSignatureStamp(originalString, firmante.thumbprint);
                yield return signature;
            }
        }
    }
}