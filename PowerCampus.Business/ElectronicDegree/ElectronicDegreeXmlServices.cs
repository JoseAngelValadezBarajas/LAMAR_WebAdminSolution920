// --------------------------------------------------------------------
// <copyright file="ElectronicDegreeXmlServices.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PowerCampus.Business.ElectronicDegree
{
    /// <summary>
    /// ElectronicDegreeXmlServices
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.ElectronicDegree.IElectronicDegreeXmlServices" />
    public class ElectronicDegreeXmlServices : IElectronicDegreeXmlServices
    {
        /// <summary>
        /// The hash algorithm
        /// </summary>
        private const string HASH_ALGORITHM = "SHA256";

        /// <summary>
        /// The serializer for TituloElectronico
        /// </summary>
        private readonly XmlSerializer serializer = new XmlSerializer(typeof(TituloElectronico));

        /// <summary>
        /// Creates the original string.
        /// </summary>
        /// <param name="tituloElectronico">The titulo electronico.</param>
        /// <param name="firmante">The firmante.</param>
        /// <returns></returns>
        public string CreateOriginalString(TituloElectronico tituloElectronico, TituloElectronicoFirmaResponsable firmante)
        {
            //! Optional (String) fields are not longer validated. Those fields need to be included whether they have value or not

            string originalString = "|";

            //Titulo electronico
            originalString += $"{tituloElectronico.version}|";
            originalString += $"{tituloElectronico.folioControl}|";

            //Firmantes
            originalString += $"{firmante.curp}|";
            originalString += $"{firmante.idCargo}|";
            originalString += $"{firmante.cargo}|";
            originalString += $"{firmante.abrTitulo}|";

            //Institucion
            originalString += $"{tituloElectronico.Institucion.cveInstitucion}|";
            originalString += $"{tituloElectronico.Institucion.nombreInstitucion}|";

            //Carrera
            originalString += $"{tituloElectronico.Carrera.cveCarrera}|";
            originalString += $"{tituloElectronico.Carrera.nombreCarrera}|";
            originalString += $"{tituloElectronico.Carrera.fechaInicio}|";
            originalString += $"{tituloElectronico.Carrera.fechaTerminacion}|";
            originalString += $"{tituloElectronico.Carrera.idAutorizacionReconocimiento}|";
            originalString += $"{tituloElectronico.Carrera.autorizacionReconocimiento}|";
            originalString += $"{tituloElectronico.Carrera.numeroRvoe}|";

            //Profesionista
            originalString += $"{tituloElectronico.Profesionista.curp}|";
            originalString += $"{tituloElectronico.Profesionista.nombre}|";
            originalString += $"{tituloElectronico.Profesionista.primerApellido}|";
            originalString += $"{tituloElectronico.Profesionista.segundoApellido}|";
            originalString += $"{tituloElectronico.Profesionista.correoElectronico}|";

            //Expedicion
            originalString += $"{tituloElectronico.Expedicion.fechaExpedicion}|";
            originalString += $"{tituloElectronico.Expedicion.idModalidadTitulacion}|";
            originalString += $"{tituloElectronico.Expedicion.modalidadTitulacion}|";
            originalString += $"|"; //!This field is meant to be used for students picture

            //! According to customer's tests. Both date values cannot co-exist
            if (string.IsNullOrEmpty(tituloElectronico.Expedicion.fechaExamenProfesional))
                originalString += $"{tituloElectronico.Expedicion.fechaExencionExamenProfesional}|";
            if (string.IsNullOrEmpty(tituloElectronico.Expedicion.fechaExencionExamenProfesional))
                originalString += $"{tituloElectronico.Expedicion.fechaExamenProfesional}|";

            originalString += $"{tituloElectronico.Expedicion.cumplioServicioSocial}|";
            originalString += $"{tituloElectronico.Expedicion.idFundamentoLegalServicioSocial}|";
            originalString += $"{tituloElectronico.Expedicion.fundamentoLegalServicioSocial}|";
            originalString += $"{tituloElectronico.Expedicion.idEntidadFederativa}|";
            originalString += $"{tituloElectronico.Expedicion.entidadFederativa}|";

            //Antecedente
            originalString += $"{tituloElectronico.Antecedente.institucionProcedencia}|";
            originalString += $"{tituloElectronico.Antecedente.idTipoEstudioAntecedente}|";
            originalString += $"{tituloElectronico.Antecedente.tipoEstudioAntecedente}|";
            originalString += $"{tituloElectronico.Antecedente.idEntidadFederativa}|";
            originalString += $"{tituloElectronico.Antecedente.entidadFederativa}|";
            originalString += $"{tituloElectronico.Antecedente.fechaInicio}|";
            originalString += $"{tituloElectronico.Antecedente.fechaTerminacion}|";
            originalString += $"{tituloElectronico.Antecedente.noCedula}|";

            return $"|{originalString}|";
        }

        /// <summary>
        /// Creates the signature stamp.
        /// </summary>
        /// <param name="originalString">The original string.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Certificate with thumbprint {thumbprint} does not exists</exception>
        /// <exception cref="InvalidOperationException">Certificate with thumbprint {thumbprint} does not exists or</exception>
        public (string signature, string serialNumber, string publicCertificate) CreateSignatureStamp(string originalString, string thumbprint)
        {
            X509Store myStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            myStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificates = myStore.Certificates
                                                             .Find(X509FindType.FindByThumbprint, thumbprint, false);

            if (certificates.Count == 0)
                throw new InvalidOperationException($"Certificate with thumbprint {thumbprint} does not exists");
            X509Certificate2 certificate = certificates[0];
            string signature = GetSignature(originalString, certificate);
            string serialNumber = GetSerialNumber(certificate);
            string publicCertificate = GetPublicCertificate(certificate);

            myStore.Close();

            return (signature, serialNumber, publicCertificate);
        }

        /// <summary>
        /// Creates the XML for electronic degree.
        /// </summary>
        /// <param name="tituloElectronico">The titulo electronico.</param>
        /// <returns></returns>
        public string CreateXmlForElectronicDegree(TituloElectronico tituloElectronico)
        {
            foreach (TituloElectronicoFirmaResponsable firmante in tituloElectronico.FirmaResponsables)
            {
                string originalString = CreateOriginalString(tituloElectronico, firmante);
                LoggerHelper.LogInformation(new LogDetail
                {
                    Layer = typeof(ElectronicDegreeXmlServices).FullName,
                    Message = $"Original String: {originalString} "
                });
                (string signature, string serialNumber, string publicCertificate) = CreateSignatureStamp(originalString, firmante.thumbprint);
                firmante.sello = signature;
                firmante.noCertificadoResponsable = serialNumber;
                firmante.certificadoResponsable = publicCertificate;
            }

            return GetXml(tituloElectronico);
        }

        /// <summary>
        /// Creates the XML request for electronic degree.
        /// </summary>
        /// <param name="tituloElectronico">The titulo electronico.</param>
        /// <returns></returns>
        public string CreateXmlRequestForElectronicDegree(TituloElectronico tituloElectronico)
        {
            return GetXml(tituloElectronico);
        }

        /// <summary>
        /// Serializes the electronic degree.
        /// </summary>
        /// <param name="tituloElectronico">The titulo electronico.</param>
        /// <returns></returns>
        public MemoryStream SerializeElectronicDegree(TituloElectronico tituloElectronico)
        {
            tituloElectronico.schemaLocation = "https://www.siged.sep.gob.mx/titulos/schema.xsd";

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = false;
            settings.ConformanceLevel = ConformanceLevel.Document;

            MemoryStream stream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(stream, settings);
            serializer.Serialize(writer, tituloElectronico);

            stream.Position = 0;

            XDocument document = XDocument.Load(stream);
            document.Root.Attributes().FirstOrDefault(a => a.Name.LocalName == "xsd")?.Remove();

            stream = new MemoryStream();
            document.Save(stream);

            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Gets the public certificate.
        /// </summary>
        /// <param name="certificate">The certificate.</param>
        /// <returns></returns>
        private string GetPublicCertificate(X509Certificate2 certificate) => Convert.ToBase64String(certificate.RawData);

        /// <summary>
        /// Gets the serial number.
        /// </summary>
        /// <param name="certificate">The certificate.</param>
        /// <returns></returns>
        private string GetSerialNumber(X509Certificate2 certificate)
        {
            string serialNumber = string.Empty;

            for (int i = 1; i < certificate.SerialNumber.Length; i += 2)
                serialNumber += certificate.SerialNumber[i];

            return serialNumber;
        }

        /// <summary>
        /// Gets the signature.
        /// </summary>
        /// <param name="originalString">The original string.</param>
        /// <param name="certificate">The certificate.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        private string GetSignature(string originalString, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            if (!certificate.HasPrivateKey)
                throw new InvalidOperationException();
            csp.FromXmlString(certificate.PrivateKey.ToXmlString(true));

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(originalString);
            byte[] signedhash = csp.SignData(data, CryptoConfig.MapNameToOID(HASH_ALGORITHM));
            string base64 = Convert.ToBase64String(signedhash, Base64FormattingOptions.None);
            return base64;
        }

        /// <summary>
        /// Gets the XML.
        /// </summary>
        /// <param name="tituloElectronico">The titulo electronico.</param>
        /// <returns></returns>
        private string GetXml(TituloElectronico tituloElectronico)
        {
            MemoryStream stream = SerializeElectronicDegree(tituloElectronico);
            string xml = Convert.ToBase64String(stream.ToArray());
            if (xml.StartsWith("77u/"))
                xml = xml.Substring(4);
            return xml;
        }
    }
}