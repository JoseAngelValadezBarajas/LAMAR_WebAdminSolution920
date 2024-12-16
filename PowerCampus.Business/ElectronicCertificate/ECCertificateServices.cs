// --------------------------------------------------------------------
// <copyright file="ECCertificateServices.cs" company="Ellucian">
//     Copyright 2020-2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using PowerCampus.BusinessInterfaces.ElectronicCertificate;
using PowerCampus.DataAccess.ElectronicCertificate;
using PowerCampus.Entities;
using PowerCampus.Entities.ElectronicCertificate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PowerCampus.Business.ElectronicCertificate
{
    /// <summary>
    /// ECCertificateServices
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.ElectronicCertificate.IECCertificateServices" />
    public class ECCertificateServices : IECCertificateServices
    {
        /// <summary>
        /// The decimal format
        /// </summary>
        private const string decimalFormat = "{0:0.00##}";

        /// <summary>
        /// The hash algorithm
        /// </summary>
        private const string HASH_ALGORITHM = "SHA256";

        /// <summary>
        /// The certificate da
        /// </summary>
        private readonly ECCertificateDA _certificateDA;

        /// <summary>
        /// The serializer
        /// </summary>
        private readonly XmlSerializer serializer = new XmlSerializer(typeof(Dec));

        /// <summary>
        /// Initializes a new instance of the <see cref="ECCertificateServices"/> class.
        /// </summary>
        public ECCertificateServices() => _certificateDA = new ECCertificateDA();

        #region Private Fields
        #endregion Private Fields

        #region Electronic Certificate PowerCampus

        /// <summary>
        /// Creates the specified certificate information.
        /// </summary>
        /// <param name="certificateInfo">The certificate information.</param>
        /// <returns></returns>
        public bool Create(CertificateInfo certificateInfo)
        {
            int? electronicCertificateId = _certificateDA.Create(certificateInfo);
            CertificateInfo newCertificateInfo = _certificateDA.GetECCertificateDetail(electronicCertificateId.Value);
            if (newCertificateInfo.Id == 0)
                _ = _certificateDA.Delete(electronicCertificateId.Value);
            return _certificateDA.CreateXml(electronicCertificateId.Value, ToXml(MapCertificateInfo(newCertificateInfo)), certificateInfo.OperatorId);
        }

        /// <summary>
        /// Creates the original string.
        /// </summary>
        /// <param name="d">The electronic certificate.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string CreateOriginalString(Dec d)
        {
            StringBuilder originalString = new StringBuilder();
            originalString.Append("||");
            originalString.Append(d.version + "|");
            originalString.Append(d.tipoCertificado + "|");
            originalString.Append(d.ServicioFirmante.idEntidad + "|");
            originalString.Append(d.Ipes.idNombreInstitucion + "|");
            originalString.Append(d.Ipes?.idCampus + "|");
            originalString.Append(d.Ipes?.idEntidadFederativa + "|");
            originalString.Append(d.Ipes.Responsable.curp + "|");
            originalString.Append(d.Ipes.Responsable.idCargo + "|");
            originalString.Append(d.Rvoe.numero + "|");
            originalString.Append(d.Rvoe.fechaExpedicion.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "|");
            originalString.Append(d.Carrera.idCarrera + "|");
            originalString.Append(d.Carrera.idTipoPeriodo + "|");
            originalString.Append(d.Carrera.clavePlan + "|");
            originalString.Append(d.Carrera.idNivelEstudios + "|");
            originalString.Append(d.Carrera.calificacionMinima + "|");
            originalString.Append(d.Carrera.calificacionMaxima + "|");
            originalString.Append(d.Carrera.calificacionMinimaAprobatoria + "|");
            originalString.Append(d.Alumno.numeroControl + "|");
            originalString.Append(d.Alumno.curp + "|");
            originalString.Append(d.Alumno.nombre + "|");
            originalString.Append(d.Alumno.primerApellido + "|");
            originalString.Append(d.Alumno.segundoApellido + "|");
            originalString.Append(d.Alumno.idGenero + "|");
            originalString.Append(d.Alumno.fechaNacimiento.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "|");
            originalString.Append(d.Expedicion.idTipoCertificacion + "|");
            originalString.Append(d.Expedicion.fecha.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "|");
            originalString.Append(d.Expedicion.idLugarExpedicion + "|");
            originalString.Append(d.Asignaturas.total + "|");
            originalString.Append(d.Asignaturas.asignadas + "|");
            originalString.Append(d.Asignaturas.promedio + "|");
            originalString.Append(d.Asignaturas.totalCreditos + "|");
            originalString.Append(d.Asignaturas.creditosObtenidos + "|");
            originalString.Append(d.Asignaturas.numeroCiclos + "|");
            for (int i = 0; i < d.Asignaturas.Asignatura.Length; i++)
            {
                originalString.Append(d.Asignaturas.Asignatura[i].idAsignatura + "|");
                originalString.Append(d.Asignaturas.Asignatura[i].ciclo + "|");
                originalString.Append(d.Asignaturas.Asignatura[i].calificacion + "|");
                originalString.Append(d.Asignaturas.Asignatura[i].idTipoAsignatura + "|");
                originalString.Append(d.Asignaturas.Asignatura[i].creditos + "|");
            }
            originalString.Append("|");
            return originalString.ToString();
        }

        /// <summary>
        /// Creates the signature stamp.
        /// </summary>
        /// <param name="originalString">The original string.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Certificate with thumbprint {thumbprint} does not exists</exception>
        /// <exception cref="System.InvalidOperationException">Certificate with thumbprint {thumbprint} does not exists</exception>
        /// <exception cref="System.NotImplementedException"></exception>
        public (string signature, string serialNumber, string publicCertificate) CreateSignatureStamp(string originalString, string thumbprint)
        {
            string signature = string.Empty;
            string serialNumber = string.Empty;
            string publicCertificate = string.Empty;

            ThumbprintStatus status = GetThumbprintStatus(thumbprint);

            if (status != ThumbprintStatus.NoInstalled || status != ThumbprintStatus.NoPrivateKey)
            {
                X509Certificate2 certificate = GetComputerCertificate(thumbprint);
                if (certificate != null)
                {
                    signature = GetSignature(originalString, certificate);
                    serialNumber = GetSerialNumber(certificate);
                    publicCertificate = GetPublicCertificate(certificate);
                }
            }

            return (signature, serialNumber, publicCertificate);
        }

        /// <summary>
        /// Deletes the specified electronic certificate identifier.
        /// </summary>
        /// <param name="id">The electronic certificate identifier.</param>
        /// <returns></returns>
        public bool Delete(int id) => _certificateDA.Delete(id);

        #endregion Electronic Certificate PowerCampus

        #region Electronic Certificate Sep

        /// <summary>
        /// Gets the ec advanced search.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        public List<Certificate> GetECAdvancedSearch(Search search) => _certificateDA.GetECAdvancedSearch(search);

        /// <summary>
        /// Gets the ec basic search.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public List<Certificate> GetECBasicSearch(string keyword, string status) => _certificateDA.GetECBasicSearch(keyword, status);

        /// <summary>
        /// Gets the ec campus by status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public List<CodeTable> GetECCampusByStatus(string status) => _certificateDA.GetECCampusByStatus(status);

        /// <summary>
        /// Gets the ec certificate detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public CertificateInfo GetECCertificateDetail(int id) => _certificateDA.GetECCertificateDetail(id);

        /// <summary>
        /// Gets the ec certificate status.
        /// </summary>
        /// <returns></returns>
        public List<string> GetECCertificateStatus() => _certificateDA.GetECCertificateStatus();

        /// <summary>
        /// Gets the ec certification type by status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public List<CodeTable> GetECCertificationTypeByStatus(string status) => _certificateDA.GetECCertificationTypeByStatus(status);

        /// <summary>
        /// Gets the ec download files.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
    public List<CertificateFile> GetECDownloadFiles(List<int> ids)
    {
            List<CertificateFile> certificateFiles = _certificateDA.GetECDownloadFiles(ids);
            foreach (CertificateFile certificateFile in certificateFiles)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(certificateFile.XmlFile);

                // Create an XML declaration.
                XmlDeclaration xmldecl = xml.CreateXmlDeclaration("1.0", null, null);
                xmldecl.Encoding = "UTF-8";

                // Add the new node to the document.
                XmlElement root = xml.DocumentElement;
                xml.InsertBefore(xmldecl, root);

                certificateFile.XmlFile = xml.InnerXml;
            }
            return certificateFiles;
        }


        /// <summary>
        /// Gets the ec major by status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public List<CodeTable> GetECMajorByStatus(string status) => _certificateDA.GetECMajorByStatus(status);

        /// <summary>
        /// Gets the ec PDF file.
        /// </summary>
        /// <param name="electronicCertificateFileId">The electronic certificate file identifier.</param>
        /// <returns></returns>
        public CertificateFile GetECPdfFile(int electronicCertificateFileId) => _certificateDA.GetECPdfFile(electronicCertificateFileId);

        /// <summary>
        /// Gets the ec XML file.
        /// </summary>
        /// <param name="electronicCertificateFileId">The electronic certificate file identifier.</param>
        /// <returns></returns>
        public CertificateFile GetECXmlFile(int electronicCertificateFileId)
        {
            CertificateFile cerfificateFile = _certificateDA.GetECXmlFile(electronicCertificateFileId);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(cerfificateFile.XmlFile);

            // Create an XML declaration.
            XmlDeclaration xmldecl = xml.CreateXmlDeclaration("1.0", null, null);
            xmldecl.Encoding = "UTF-8";

            // Add the new node to the document.
            XmlElement root = xml.DocumentElement;
            xml.InsertBefore(xmldecl, root);

            cerfificateFile.XmlFile = xml.InnerXml;

            return cerfificateFile;
        }


        /// <summary>
        /// Gets the thumbprint status.
        /// </summary>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <returns></returns>
        public ThumbprintStatus GetThumbprintStatus(string thumbprint)
        {
            X509Certificate2 certificate = GetComputerCertificate(thumbprint);
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            if (certificate == null)
            {
                return ThumbprintStatus.NoInstalled;
            }
            else
            {
                if (!certificate.HasPrivateKey)
                    return ThumbprintStatus.NoPrivateKey;

                try
                {
                    using (certificate.GetRSAPrivateKey())
                    {
                        csp.FromXmlString(certificate.PrivateKey.ToXmlString(true));
                        if (csp == null)
                        {
                            return ThumbprintStatus.NoInstalled;
                        }
                        else
                        {
                            return ThumbprintStatus.Installed;
                        }
                    };
                }
                catch (Exception ex)
                {
                    LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.EcBusiness} - ECCertificateServices - GetThumbprintStatus", $"Error GetRSAPrivateKey - {ex.Message}");
                    return ThumbprintStatus.NoInstalled;
                }
            }
        }

        /// <summary>
        /// Stamps the specified electronic certificate sep.
        /// </summary>
        /// <param name="dec">The electronic certificate sep.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <param name="certificateOperation">The certificate operation.</param>
        /// <returns></returns>
        public bool Stamp(Dec dec, string thumbprint, CertificateOperation certificateOperation)
        {
            string originalString = CreateOriginalString(dec);
            LoggerHelper.LogWebError(Constants.EcArea, "Stamp", $"Original String: {originalString} ");
            (string signature, string serialNumber, string publicCertificate) = CreateSignatureStamp(originalString, thumbprint);
            if (dec != null)
            {
                dec.sello = signature;
                dec.noCertificadoResponsable = serialNumber;
                dec.certificadoResponsable = publicCertificate;
            }
            certificateOperation.XmlFile = ToXml(dec);
            return _certificateDA.Stamp(certificateOperation);
        }

        /// <summary>
        /// Converts to dec.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public Dec ToDec(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Dec));
            using (StringReader sr = new StringReader(xml))
            {
                return (Dec)serializer.Deserialize(sr);
            }
        }

        /// <summary>
        /// Gets the XML.
        /// </summary>
        /// <param name="electronicCertificateSep">The electronic certificate sep.</param>
        /// <returns></returns>
        public string ToXml(Dec electronicCertificateSep)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false,
                ConformanceLevel = ConformanceLevel.Document
            };

            MemoryStream stream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(stream, settings);
            serializer.Serialize(writer, electronicCertificateSep);

            stream.Position = 0;

            XDocument document = XDocument.Load(stream);
            document.Root.Attributes().FirstOrDefault(a => a.Name.LocalName == "xsd")?.Remove();

            return document.ToString();
        }

        #endregion Electronic Certificate Sep

        #region Computer Certificate
        #endregion Computer Certificate

        #region Operation-Generation

        /// <summary>
        /// Updates the ec certificate status.
        /// </summary>
        /// <param name="certificateOperation">The certificate operation.</param>
        /// <returns></returns>
        public bool UpdateECCertificateStatus(CertificateOperation certificateOperation) => _certificateDA.UpdateECCertificateStatus(certificateOperation);

        #endregion Operation-Generation

        #region Private Methods

        /// <summary>
        /// Gets the computer certificate.
        /// </summary>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <returns></returns>
        private X509Certificate2 GetComputerCertificate(string thumbprint)
        {
            string cleanThumbprint = Regex.Replace(thumbprint, @"[^\da-fA-F]", string.Empty).ToUpper();
            if (cleanThumbprint != thumbprint.ToUpper())
            {
                return null;
            }

            X509Store myStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            try
            {
                myStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2Collection certificates = myStore.Certificates.Find(X509FindType.FindByThumbprint, cleanThumbprint, false);
                if (certificates?.Count > 0)
                {
                    if (certificates[0].Thumbprint == cleanThumbprint)
                    {
                        return certificates[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.EcBusiness} - ECCertificateServices - GetComputerCertificate", $"{thumbprint} not found - {exception.Message}");
                throw;
            }
            finally
            {
                myStore.Close();
            }

            return null;
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
        /// <exception cref="InvalidOperationException">No private key for the certificate.</exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        private string GetSignature(string originalString, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            if (!certificate.HasPrivateKey)
                throw new InvalidOperationException($"No private key for the certificate.");
            using (certificate.GetRSAPrivateKey())
            {
                csp.FromXmlString(certificate.PrivateKey.ToXmlString(true));
            };

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(originalString);
            byte[] signedhash = csp.SignData(data, CryptoConfig.MapNameToOID(HASH_ALGORITHM));
            string base64 = Convert.ToBase64String(signedhash, Base64FormattingOptions.None);
            return base64;
        }

        /// <summary>
        /// Maps the certificate information.
        /// </summary>
        /// <param name="certificateInfo">The c.</param>
        /// <returns></returns>
        private Dec MapCertificateInfo(CertificateInfo certificateInfo)
        {
            if (certificateInfo == null)
            {
                return null;
            }

            Dec dec = new Dec
            {
                folioControl = certificateInfo.Folio
            };

            #region
            dec.ServicioFirmante = new DecServicioFirmante
            {
                idEntidad = certificateInfo.SigningInstitutionId
            };

            #endregion Private Methods

            #region DecIpes

            dec.Ipes = new DecIpes
            {
                campus = certificateInfo.CampusName,
                entidadFederativa = certificateInfo.FederalEntity,
                idCampus = certificateInfo.CampusSepCode,
                idEntidadFederativa = certificateInfo.FederalEntityCode,
                idNombreInstitucion = certificateInfo.InstitutionSepId,
                nombreInstitucion = certificateInfo.InstitutionName,
                Responsable = new DecIpesResponsable
                {
                    curp = certificateInfo.ResponsibleCurp,
                    nombre = certificateInfo.ResponsibleName,
                    primerApellido = certificateInfo.ResponsibleFirstSurname,
                    segundoApellido = certificateInfo.ResponsibleSecondSurname,
                    idCargo = certificateInfo.ResponsibleJobTitleId
                }
            };

            #endregion DecIpes

            #region DecRvoe

            dec.Rvoe = new DecRvoe
            {
                numero = certificateInfo.RvoeAgreementNumber,
                fechaExpedicion = certificateInfo.ExpeditionDate.Value
            };

            #endregion DecRvoe

            #region DecCarrera

            dec.Carrera = new DecCarrera
            {
                idCarrera = certificateInfo.MajorId.ToString(),
                idTipoPeriodo = certificateInfo.PeriodTypeId,
                clavePlan = certificateInfo.PlanCode,
                idNivelEstudios = certificateInfo.StudyLevelId,
                calificacionMinima = certificateInfo.MinimumGrade.ToString(),
                calificacionMaxima = certificateInfo.MaximumGrade.ToString(),
                calificacionMinimaAprobatoria = string.Format(decimalFormat, certificateInfo.MinimumPassingGrade)
            };

            #endregion DecCarrera

            #region DecAlumno

            dec.Alumno = new DecAlumno
            {
                numeroControl = certificateInfo.PeopleId,
                curp = certificateInfo.Curp,
                nombre = certificateInfo.Name,
                primerApellido = certificateInfo.FirstSurname,
                segundoApellido = certificateInfo.SecondSurname,
                idGenero = certificateInfo.Gender.Equals("M") ? "251" : "250",
                fechaNacimiento = certificateInfo.BirthDate.Value
            };

            #endregion DecAlumno

            #region DecExpedicion

            dec.Expedicion = new DecExpedicion
            {
                idTipoCertificacion = certificateInfo.CertificationTypeId,
                tipoCertificacion = certificateInfo.CertificationType,
                fecha = certificateInfo.IssuingDate.Value,
                idLugarExpedicion = certificateInfo.IssuingFederalEntityCode,
                lugarExpedicion = certificateInfo.IssuingFederalEntity
            };

            #endregion DecExpedicion

            #region DecAsignaturas

            dec.Asignaturas = new DecAsignaturas
            {
                Asignatura = MapCertificateInfoCourse(certificateInfo.Courses),
                total = certificateInfo.TotalCourse,
                asignadas = certificateInfo.CourseAssigned,
                promedio = string.Format(decimalFormat, certificateInfo.Average),
                totalCreditos = string.Format(decimalFormat, certificateInfo.TotalCredit),
                creditosObtenidos = string.Format(decimalFormat, certificateInfo.CreditEarned),
                numeroCiclos = certificateInfo.TotalCycle,
            };

            #endregion DecAsignaturas

            return dec;
        }

        /// <summary>
        /// Maps the certificate information course.
        /// </summary>
        /// <param name="courses">The courses.</param>
        /// <returns></returns>
        private DecAsignaturasAsignatura[] MapCertificateInfoCourse(List<CertificateCourseInfo> courses)
        {
            List<DecAsignaturasAsignatura> certificateCourses = new List<DecAsignaturasAsignatura>();
            foreach (CertificateCourseInfo course in courses)
            {
                certificateCourses.Add(new DecAsignaturasAsignatura
                {
                    calificacion = string.Format(decimalFormat, course.FinalGrade),
                    ciclo = course.EventCycle,
                    creditos = string.Format(decimalFormat, course.Credits),
                    idAsignatura = course.SepId,
                    idObservaciones = course.GradeRemarkShortDesc,
                    idTipoAsignatura = course.SubjectTypeShortDesc
                });
            }
            return certificateCourses.ToArray();
        }

        #endregion Private Methods
    }
}