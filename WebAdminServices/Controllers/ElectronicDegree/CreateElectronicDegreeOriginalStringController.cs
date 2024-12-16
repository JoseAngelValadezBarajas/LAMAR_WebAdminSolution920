// --------------------------------------------------------------------
// <copyright file="CreateElectronicDegreeXmlController - Copy.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business.ElectronicDegree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;

namespace WebAdminServices.Controllers.ElectronicDegree
{
    /// <summary>
    /// Creates the Original String base64-encoded
    /// </summary>
    [Route("api/v1/CreateElectronicDegreeOriginalString")]
    public class CreateElectronicDegreeOriginalStringController : ApiController
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
                string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(originalString));
                yield return base64;
            }
        }
    }
}