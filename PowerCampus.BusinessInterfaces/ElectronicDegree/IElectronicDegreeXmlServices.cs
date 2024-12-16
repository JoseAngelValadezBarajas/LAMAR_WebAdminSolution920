// --------------------------------------------------------------------
// <copyright file="IElectronicDegreeXmlServices.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.BusinessInterfaces.ElectronicDegree
{
    /// <summary>
    /// IElectronicDegreeXmlServices
    /// </summary>
    public interface IElectronicDegreeXmlServices
    {
        /// <summary>
        /// Creates the original string.
        /// </summary>
        /// <param name="tituloElectronico">The titulo electronico.</param>
        /// <param name="firmante">The firmante.</param>
        /// <returns></returns>
        string CreateOriginalString(TituloElectronico tituloElectronico, TituloElectronicoFirmaResponsable firmante);

        /// <summary>
        /// Creates the signature stamp.
        /// </summary>
        /// <param name="originalString">The original string.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <returns></returns>
        (string signature, string serialNumber, string publicCertificate) CreateSignatureStamp(string originalString, string thumbprint);

        /// <summary>
        /// Creates the XML for electronic degree.
        /// </summary>
        /// <param name="tituloElectronico">The titulo electronico.</param>
        /// <returns></returns>
        string CreateXmlForElectronicDegree(TituloElectronico tituloElectronico);

        /// <summary>
        /// Creates the XML request for electronic degree.
        /// </summary>
        /// <param name="tituloElectronico">The titulo electronico.</param>
        /// <returns></returns>
        string CreateXmlRequestForElectronicDegree(TituloElectronico tituloElectronico);

        /// <summary>
        /// Serializes the electronic degree.
        /// </summary>
        /// <param name="tituloElectronico">The titulo electronico.</param>
        /// <returns></returns>
        System.IO.MemoryStream SerializeElectronicDegree(TituloElectronico tituloElectronico);
    }
}