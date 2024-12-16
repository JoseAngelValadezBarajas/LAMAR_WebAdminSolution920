// --------------------------------------------------------------------
// <copyright file="CreateXmlController.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Business.ElectronicDegree;
using System.Web.Http;

namespace WebAdminServices.Controllers.ElectronicDegree
{
    /// <summary>
    /// Creates the XML document from a custom object
    /// </summary>
    [Route("api/v1/CreateElectronicDegreeXml")]
    public class CreateElectronicDegreeXml1Controller : ApiController
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
        public string Post([FromBody] TituloElectronico tituloElectronico) => services.CreateXmlForElectronicDegree(tituloElectronico);
    }
}