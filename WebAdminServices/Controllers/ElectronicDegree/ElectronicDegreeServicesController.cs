// --------------------------------------------------------------------
// <copyright file="ElectronicDegreeServicesController.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PowerCampus.Business.ElectronicDegree;
using PowerCampus.Business.SepService;
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.Entities.ElectronicDegree;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers.ElectronicDegree
{
    [LoggingActionFilterAttribute]
    public class ElectronicDegreeServicesController : ApiController
    {
        /// <summary>
        /// The sep password key
        /// </summary>
        private const string SEP_PASSWORD_KEY = "SepPassword";

        /// <summary>
        /// The sep user key
        /// </summary>
        private const string SEP_USER_KEY = "SepUser";

        /// <summary>
        /// The information services
        /// </summary>
        private readonly IElectronicDegreeInformationServices infoServices;

        /// <summary>
        /// The XML services
        /// </summary>
        private readonly ElectronicDegreeXmlServices xmlServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElectronicDegreeServicesController"/> class.
        /// </summary>
        public ElectronicDegreeServicesController()
        {
            xmlServices = new ElectronicDegreeXmlServices();
            infoServices = new ElectronicDegreeInformationServices();
        }

        [Route("api/ElectronicDegreeServices/Cancel")]
        [HttpPost]
        public async Task<IHttpActionResult> CancelDocument(SendDocument document)
        {
            ElectronicDegreeInfo electronicDegreeInfo = infoServices.GetElectronicDegreeInfo(document.ElectronicDegreeId);
            TituloElectronico tituloElectronico = MapElectronicDegreeInfo(electronicDegreeInfo);

            string user, pwd;
            GetUserAndPassword(tituloElectronico, out user, out pwd);

            ElectronicDegreeWebServices webServices = new ElectronicDegreeWebServices(user, pwd);

            LoggerHelper.LogInformation(new LogDetail
            {
                Layer = typeof(ElectronicDegreeServicesController).FullName,
                Message = $"Canceling document for {document.ElectronicDegreeId}"
            });

            SepServicesResponse sepResponse;

            try
            {
                cancelaTituloElectronicoResponse1 response = await webServices.CancelDocument(tituloElectronico.folioControl, document.CancelationCode);

                sepResponse = new SepServicesResponse()
                {
                    message = response.cancelaTituloElectronicoResponse.mensaje
                };

                if (response.cancelaTituloElectronicoResponse.codigo == 1)
                    sepResponse.status = SepStatus.Success;
                else
                    sepResponse.status = SepStatus.UnableToCancel;

                LoggerHelper.LogInformation(new LogDetail
                {
                    Layer = typeof(ElectronicDegreeServicesController).FullName,
                    Message = $"Response from SEP Service {response.cancelaTituloElectronicoResponse.codigo} and message {sepResponse.message}"
                });
            }
            catch (Exception ex)
            {
                sepResponse = new SepServicesResponse() { status = SepStatus.WebServiceUnavailable };
            }

            if (sepResponse.status != SepStatus.Success)
                return Ok(sepResponse);

            ElectronicDegreeInfoRequest infoRequest = infoServices.GetElectronicDegreeInfoRequest(document.ElectronicDegreeId);

            infoRequest.ResponseMessage = sepResponse.message;
            infoRequest.Status = 'C';
            infoRequest.CancellationReason = document.CancelationDescription;

            infoServices.UpdateElectronicDegreeInfoRequest(infoRequest);

            infoServices.UpdateElectronicDegreeInfoStatus(document.ElectronicDegreeId, 'C');

            return Ok(sepResponse);
        }

        /// <summary>
        /// Sends the document to SEP
        /// </summary>
        /// <returns></returns>
        [Route("api/ElectronicDegreeServices/Send")]
        [HttpPost]
        public async Task<IHttpActionResult> SendDocument(SendDocument document)
        {
            ElectronicDegreeInfo electronicDegreeInfo = infoServices.GetElectronicDegreeInfo(document.ElectronicDegreeId);
            TituloElectronico tituloElectronico = MapElectronicDegreeInfo(electronicDegreeInfo);

            string user, pwd;
            GetUserAndPassword(tituloElectronico, out user, out pwd);

            ElectronicDegreeWebServices webServices = new ElectronicDegreeWebServices(user, pwd);

            LoggerHelper.LogInformation(new LogDetail
            {
                Layer = typeof(ElectronicDegreeServicesController).FullName,
                Message = $"Creating XML for {document.ElectronicDegreeId}"
            });
            string base64 = xmlServices.CreateXmlForElectronicDegree(tituloElectronico);

            LoggerHelper.LogInformation(new LogDetail
            {
                Layer = typeof(ElectronicDegreeServicesController).FullName,
                Message = $"Base64: {base64}"
            });

            SepServicesResponse sepResponse;

            try
            {
                cargaTituloElectronicoResponse1 response = await webServices.SendDocument(base64,
                                                                                          $"{tituloElectronico.folioControl}.xml");

                sepResponse = new SepServicesResponse()
                {
                    lotNumber = response.cargaTituloElectronicoResponse.numeroLote,
                    message = response.cargaTituloElectronicoResponse.mensaje
                };
                LoggerHelper.LogInformation(new LogDetail
                {
                    Layer = typeof(ElectronicDegreeServicesController).FullName,
                    Message = $"Response from SEP Service {sepResponse.lotNumber} and message {sepResponse.message}"
                });

                if (!string.IsNullOrEmpty(sepResponse.lotNumber))
                    sepResponse.status = SepStatus.Success;
                else
                    sepResponse.status = SepStatus.InvalidRequest;
            }
            catch (Exception ex)
            {
                sepResponse = new SepServicesResponse() { status = SepStatus.WebServiceUnavailable };
                LoggerHelper.LogInformation(new LogDetail
                {
                    Layer = typeof(ElectronicDegreeServicesController).FullName,
                    Message = $"Response from SEP Service {ex.Message}"
                });
            }

            if (sepResponse.status != SepStatus.Success)
                return Ok(sepResponse);

            ElectronicDegreeInfoRequest infoRequest = infoServices.GetElectronicDegreeInfoRequest(document.ElectronicDegreeId);
            if (infoRequest == null)
            {
                infoRequest = new ElectronicDegreeInfoRequest()
                {
                    BatchNumber = sepResponse.lotNumber,
                    CreateDatetime = DateTime.Now,
                    CreateUserName = document.OperatorName,
                    ElectronicDegreeInformationId = document.ElectronicDegreeId,
                    RequestXML = Encoding.UTF8.GetString(Convert.FromBase64String(base64)),
                    ResponseMessage = sepResponse.message,
                    RevisionDatetime = DateTime.Now,
                    Status = 'S'
                };

                _ = infoServices.InsertElectronicDegreeInfoRequest(infoRequest);
            }
            else
            {
                infoRequest.ResponseMessage = sepResponse.message;
                infoRequest.BatchNumber = sepResponse.lotNumber;

                infoServices.UpdateElectronicDegreeInfoRequest(infoRequest);
            }

            infoServices.UpdateElectronicDegreeInfoStatus(document.ElectronicDegreeId, 'S');

            return Ok(sepResponse);
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        [Route("api/ElectronicDegreeServices/Update")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateStatus(SendDocument document)
        {
            ElectronicDegreeInfo electronicDegreeInfo = infoServices.GetElectronicDegreeInfo(document.ElectronicDegreeId);
            TituloElectronico tituloElectronico = MapElectronicDegreeInfo(electronicDegreeInfo);

            string user, pwd;
            GetUserAndPassword(tituloElectronico, out user, out pwd);

            ElectronicDegreeWebServices webServices = new ElectronicDegreeWebServices(user, pwd);

            SepServicesResponse sepResponse;
            ElectronicDegreeInfoRequest infoRequest = infoServices.GetElectronicDegreeInfoRequest(document.ElectronicDegreeId);
            try
            {
                consultaProcesoTituloElectronicoResponse1 response = await webServices.CheckDocument(infoRequest.BatchNumber);

                sepResponse = new SepServicesResponse()
                {
                    lotNumber = response.consultaProcesoTituloElectronicoResponse.numeroLote,
                    message = response.consultaProcesoTituloElectronicoResponse.mensaje
                };
                LoggerHelper.LogInformation(new LogDetail
                {
                    Layer = typeof(ElectronicDegreeServicesController).FullName,
                    Message = $"Response from CheckDocument SEP Service {sepResponse.lotNumber} and message {sepResponse.message}"
                });

                if (response.consultaProcesoTituloElectronicoResponse.estatusLote != 1)
                {
                    sepResponse.status = SepStatus.InvalidElectronicDegree;
                    return Ok(sepResponse);
                }

                descargaTituloElectronicoResponse1 downloadResponse = await webServices.DownloadDocument(infoRequest.BatchNumber);

                //string base64String = "UEsDBBQACAgIAOpYe1AAAAAAAAAAAAAAAAAfAAAAUmVzdWx0YWRvQ2FyZ2FUaXR1bG9zMjU3OTQ1Lnhsc+1XzU9TQRD/7esrBZXaAoookmfRyEcl1YMaiBZEjBikpICaWIMFGm2s1JT6FQ/Wr5smJl69ePTix0UPGgM3DyYaPZhwAv8DE008IM/ZefvoB6KQ+BEj0+zO7m9ndmZn5+1u37z2Tt57vHYKBdQCB2bMEhTlYCJXwAM4FTZjmqaEpKy5RP8UhZGkXxoGOjBCPIWLhanwQ1oNp7DnEj8Xn82jMZfVPkzWUziFQfbj1KJsSyqDJnLXs1C9a65Fm/ouacjP+V8z65+zX1JMH3KRE+88r1xtsL7hKZr1kT7Oc3+gEsEZFAPnY4OJ5In4kPEnaA/7EBXSh12UMHep70YV+1TGdTnXD1niuSVHIyeFW0SaX9Tu5NUJHNFaWe4W1z6u3VQLPGWdCUa2YhVeykhevq2S2Im2VDya+IUDNXo1NSvJiY2+Wn9tbWCgviVSZ3ci9TX6Bop+dd740XBs+Fi+kA8urM8KNQXy56G+lNpMG+YrlCqYTYnuQAO2AQMSbzBsy6obqVOGbcC3xcejrQOk2oR6+pFqrl6+UoFGEH405xuzfMu3x67lmAwGs1NsRyPtV9aorT9HuUBzEhW8HZ9NIyf/xwyJCxv/tDBcWySO/xAXdKdkyY5noyX/pRCvnwf3z4M3zIOXzMHvaDo8GYcpuTdTxLwsozMvz7iYIwPmFRmneZzPhRvwYkI+cGjGcGz0bCIdHU7ehE4mBB4I5nolfWX2pTeNS5B3iipt4fb9nYdC1Oro7Wvr6+9dDuzt6G0Pd/a0d4a6S4F9oa7OkNEe6u4Lh7pILjo41HThdGILjSQT8aQxHDOGkiPpVDJhnEnFzsWjp2Mj6ZiRip2Ij6ZT5E2zEQgEtpLzkplYRs70k8cryIFlsE92D56VvnKNwzrZl2sVszuiKX6AbJfyibiC6mGs5LaXxz005/T9j28PDvYEBxhvYLyR66uMZOTdoGiTjCVMXKGRcV3aWkXlGktf5/oeHUuCrAj+afALv4rgZNDmAv00QrNq7hxPHVSKNQ+3daiLTqxkTBRgXzkCoLVIsnqCemK2p/E67Z6Deg7VE6wn1+Eg+4L1FLVa41LXOSstdWVs35PHd4lXYCd2a2V4wk/nVmRpvbKC7FNaGv+9D7yfkFDxdXAErTd9kUrjYs5+y0lKX84sSly4VWTlXngh72N5GwP2fq+mQvcc1lCporKWyjoq1RwDoAbWf4Yl+vsk9yF3/2cUtrQ//wd9A1BLBwh/73mDnAMAAAAQAABQSwECFAAUAAgICADqWHtQf+95g5wDAAAAEAAAHwAAAAAAAAAAAAAAAAAAAAAAUmVzdWx0YWRvQ2FyZ2FUaXR1bG9zMjU3OTQ1Lnhsc1BLBQYAAAAAAQABAE0AAADpAwAAAAA=";
                //byte[] zipBytes = Convert.FromBase64String(base64String);
                byte[] zipBytes = downloadResponse.descargaTituloElectronicoResponse.titulosBase64;

                Stream ms = GetExcelFile(zipBytes);

                if (ms == null)
                {
                    sepResponse.status = SepStatus.UnableToProcessZip;
                    return Ok(sepResponse);
                }

                (bool success, string code, string description) = GetExcelInformation(ms);

                if (!success)
                {
                    sepResponse.status = SepStatus.UnableToProcessXls;
                    return Ok(sepResponse);
                }

                sepResponse.status = SepStatus.Success;
                sepResponse.message = description;

                LoggerHelper.LogInformation(new LogDetail
                {
                    Layer = typeof(ElectronicDegreeServicesController).FullName,
                    Message = $"Response from DownloadDocument SEP Service Code: {code} and message {sepResponse.message}"
                });

                if (code == "1" || code == "2")
                    infoRequest.Status = 'A';
                else
                    infoRequest.Status = 'R';
            }
            catch (Exception ex)
            {
                sepResponse = new SepServicesResponse() { status = SepStatus.WebServiceUnavailable };
            }

            infoServices.UpdateElectronicDegreeInfoRequest(infoRequest);

            infoServices.UpdateElectronicDegreeInfoStatus(document.ElectronicDegreeId, infoRequest.Status);

            return Ok(sepResponse);
        }

        /// <summary>
        /// Gets the user and password.
        /// </summary>
        /// <param name="tituloElectronico">The titulo electronico.</param>
        /// <param name="user">The user.</param>
        /// <param name="pwd">The password.</param>
        private static void GetUserAndPassword(TituloElectronico tituloElectronico, out string user, out string pwd)
        {
            user = System.Configuration.ConfigurationManager.AppSettings[$"{SEP_USER_KEY}{tituloElectronico.Institucion.cveInstitucion}"];
            pwd = System.Configuration.ConfigurationManager.AppSettings[$"{SEP_PASSWORD_KEY}{tituloElectronico.Institucion.cveInstitucion}"];
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pwd))
            {
                user = System.Configuration.ConfigurationManager.AppSettings[SEP_USER_KEY];
                pwd = System.Configuration.ConfigurationManager.AppSettings[SEP_PASSWORD_KEY];
            }
        }

        /// <summary>
        /// Gets the excel file.
        /// </summary>
        /// <param name="zipBytes">The zip bytes.</param>
        /// <returns></returns>
        private Stream GetExcelFile(byte[] zipBytes)
        {
            Stream data = new MemoryStream(zipBytes);

            ZipArchive archive = new ZipArchive(data);
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (entry.FullName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    Stream deflateStream = entry.Open();
                    Stream stream = new MemoryStream();
                    deflateStream.CopyTo(stream);
                    stream.Position = 0;
                    return stream;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the excel information.
        /// </summary>
        /// <param name="ms">The ms.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private (bool success, string code, string description) GetExcelInformation(Stream ms)
        {
            try
            {
                HSSFWorkbook hssfwb = new HSSFWorkbook(ms);
                //using (FileStream file = new FileStream(@"c:\test.xls", FileMode.Open, FileAccess.Read))
                //{
                //    hssfwb = new HSSFWorkbook(file);
                //}

                ISheet sheet = hssfwb.GetSheet("Resultado");
                string code = sheet.GetRow(1).GetCell(1).NumericCellValue.ToString();
                string description = sheet.GetRow(1).GetCell(2).StringCellValue;

                return (true, code, description);
            }
            catch (Exception ex)
            {
                return (false, null, null);
            }
        }

        private TituloElectronico MapElectronicDegreeInfo(ElectronicDegreeInfo edi)
        {
            TituloElectronico tituloElectronico = new TituloElectronico();
            tituloElectronico.folioControl = edi.Folio;

            tituloElectronico.FirmaResponsables = edi.Signer
                                                     .Select(s => new TituloElectronicoFirmaResponsable()
                                                     {
                                                         abrTitulo = s.EdAbreviationTitle,
                                                         cargo = s.EdSignerLaborPosition,
                                                         idCargo = s.EdSignerLaborPositionCode.ToString(),
                                                         curp = s.EdSignerCurp,
                                                         nombre = s.EdSignerName,
                                                         primerApellido = s.EdSignerFirstSurname,
                                                         segundoApellido = s.EdSignerSecondSurname,
                                                         thumbprint = s.EdSignerThumprint
                                                     })
                                                     .ToArray();

            tituloElectronico.Antecedente = new TituloElectronicoAntecedente()
            {
                entidadFederativa = edi.OriginInstFederalEntity,
                idEntidadFederativa = edi.OriginInstFederalEntityCode.ToString(),
                fechaInicio = edi.BackgroundStudyStartDate,
                fechaTerminacion = edi.BackgroundStudyEndDate,
                idTipoEstudioAntecedente = edi.BackgroundStudyTypeCode.ToString(),
                tipoEstudioAntecedente = edi.BackgroundStudyType,
                institucionProcedencia = edi.OriginInstitution,
                noCedula = edi.LicenseNumber
            };

            tituloElectronico.Carrera = new TituloElectronicoCarrera()
            {
                autorizacionReconocimiento = edi.AuthorizationType,
                idAutorizacionReconocimiento = edi.AuthorizationTypeCode.ToString(),
                cveCarrera = edi.MajorCode,
                fechaInicio = edi.MajorStartDate,
                fechaTerminacion = edi.MajorEndDate,
                nombreCarrera = edi.Major,
                numeroRvoe = edi.RvoeAgreementNumber
            };

            tituloElectronico.Expedicion = new TituloElectronicoExpedicion()
            {
                cumplioServicioSocial = edi.FulfilledSocialService ? "1" : "0",
                entidadFederativa = edi.FederalEntity,
                idEntidadFederativa = edi.FederalEntityCode.ToString(),
                fechaExamenProfesional = edi.ExaminationDate,
                fechaExencionExamenProfesional = edi.ExaminationExemptionDate,
                fechaExpedicion = edi.ExpeditionDate,
                idFundamentoLegalServicioSocial = edi.LegalBaseCode.ToString(),
                fundamentoLegalServicioSocial = edi.LegalBase,
                idModalidadTitulacion = edi.GraduationRequirementCode.ToString(),
                modalidadTitulacion = edi.GraduationRequirement
            };

            tituloElectronico.Institucion = new TituloElectronicoInstitucion()
            { cveInstitucion = edi.InstitutionCode, nombreInstitucion = edi.InstitutionName };

            tituloElectronico.Profesionista = new TituloElectronicoProfesionista()
            {
                correoElectronico = edi.Student.Email,
                curp = edi.Student.Curp,
                nombre = edi.Student.Name,
                primerApellido = edi.Student.FirstSurname,
                segundoApellido = edi.Student.SecondSurname
            };

            return tituloElectronico;
        }
    }
}