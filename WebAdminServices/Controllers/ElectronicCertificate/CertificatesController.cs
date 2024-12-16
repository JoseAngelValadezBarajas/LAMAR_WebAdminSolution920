// --------------------------------------------------------------------
// <copyright file="ElectronicCertificatesController.cs" company="Ellucian">
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
    /// ElectronicCertificatesController
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [LoggingActionFilterAttribute]
    public class CertificatesController : ApiController
    {
        /// <summary>
        /// The services
        /// </summary>
        private readonly IECCertificateServices certificateServices;

        /// <summary>
        /// The institution campus service
        /// </summary>
        private readonly IECInstitutionCampusService institutionCampusServices;

        /// <summary>
        /// The responsible service
        /// </summary>
        private readonly IECResponsibleService responsibleServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificatesController" /> class.
        /// </summary>
        public CertificatesController()
        {
            certificateServices = new ECCertificateServices();
            institutionCampusServices = new ECInstitutionCampusService();
            responsibleServices = new ECResponsibleServices();
        }

        /// <summary>
        /// Saves the specified certificate information.
        /// </summary>
        /// <param name="certificateInfo">The certificate information.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/EC/Certificates")]
        public IHttpActionResult Create(CertificateInfo certificateInfo)
        {
            bool result = false;
            if (certificateInfo != null)
            {
                result = certificateServices.Create(certificateInfo);
            }
            if (result)
            {
                return Ok("POST Electronic Certificate correctly " + certificateInfo);
            }
            else
            {
                return InternalServerError();
            }
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The electronic certificate identifier.</param>
        /// <returns></returns>
        [Route("api/EC/Certificates/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            bool result = certificateServices.Delete(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Stamps the specified document.
        /// </summary>
        /// <param name="certificateOperation">The certificate operation.</param>
        /// <returns></returns>
        [Route("api/EC/Certificates/Stamp")]
        [HttpPost]
        public IHttpActionResult Stamp(CertificateOperation certificateOperation)
        {
            bool result = false;
            Dec dec = certificateServices.ToDec(certificateServices.GetECXmlFile(certificateOperation.CertificateFileId).XmlFile);
            if (dec != null)
            {
                int responsibleId = institutionCampusServices.GetResponsible(dec.Ipes?.idCampus);
                if (responsibleId > 0)
                {
                    string thumbprint = responsibleServices.GetEditResponsible(responsibleId)?.Thumbprint;
                    result = certificateServices.Stamp(dec, thumbprint, certificateOperation);
                }
                else
                {
                    return NotFound();
                }
            }

            if (result)
            {
                return Ok($"Electronic Certificate File Id: {certificateOperation.CertificateFileId} - Updated");
            }
            else
            {
                return InternalServerError();
            }
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="certificateOperation">The certificate operation.</param>
        /// <returns></returns>
        [Route("api/EC/Certificates/Status")]
        [HttpPost]
        public IHttpActionResult UpdateStatus(CertificateOperation certificateOperation)
        {
            bool result = certificateServices.UpdateECCertificateStatus(certificateOperation);
            return Ok(result);
        }

        /// <summary>
        /// Views the data.
        /// </summary>
        /// <param name="id">The electronic certificate identifier.</param>
        /// <returns></returns>
        [Route("api/EC/Certificates/{id}")]
        [HttpGet]
        public IHttpActionResult ViewData(int id)
        {
            CertificateInfo certificateInfo = certificateServices.GetECCertificateDetail(id);
            if (certificateInfo == null)
                return NotFound();
            return Ok(certificateInfo);
        }

        #region Operation-Generation

        /// <summary>
        /// Advanceds the search.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        [Route("api/EC/Certificates/Search/Advanced")]
        [HttpPost]
        public IHttpActionResult AdvancedSearch(Search search)
        {
            List<Certificate> certificates = certificateServices.GetECAdvancedSearch(search);
            if (certificates == null)
                return NotFound();
            return Ok(certificates);
        }

        /// <summary>
        /// Basics the search.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        [Route("api/EC/Certificates/Search/Basic")]
        [HttpPost]
        public IHttpActionResult BasicSearch(Search search)
        {
            List<Certificate> certificates = certificateServices.GetECBasicSearch(search.Keywords, search.Status);
            if (certificates == null)
                return NotFound();
            return Ok(certificates);
        }

        /// <summary>
        /// Campuseses the specified status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        [Route("api/EC/Certificates/Campuses/{status}")]
        [HttpGet]
        public IHttpActionResult Campuses(string status)
        {
            List<CodeTable> codeTables = certificateServices.GetECCampusByStatus(status);
            if (codeTables == null)
                return NotFound();
            return Ok(codeTables);
        }

        /// <summary>
        /// Certifications the type.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        [Route("api/EC/Certificates/CertificationTypes/{status}")]
        [HttpGet]
        public IHttpActionResult CertificationTypes(string status)
        {
            List<CodeTable> codeTables = certificateServices.GetECCertificationTypeByStatus(status);
            if (codeTables == null)
                return NotFound();
            return Ok(codeTables);
        }

        /// <summary>
        /// Downloads the PDF XML.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        [Route("api/EC/Certificates/DownloadFiles")]
        [HttpPost]
        public IHttpActionResult DownloadFiles(List<int> ids)
        {
            List<CertificateFile> certificateFiles = certificateServices.GetECDownloadFiles(ids);
            if (certificateFiles == null)
                return NotFound();
            return Ok(certificateFiles);
        }

        /// <summary>
        /// Downloads the PDF.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("api/EC/Certificates/Download/Pdf/{id}")]
        [HttpGet]
        public IHttpActionResult DownloadPdf(int id)
        {
            CertificateFile pdfFile = certificateServices.GetECPdfFile(id);
            if (pdfFile == null)
                return NotFound();
            return Ok(pdfFile);
        }

        /// <summary>
        /// Downloads the PDF XML.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Route("api/EC/Certificates/Download/Xml/{id}")]
        [HttpGet]
        public IHttpActionResult DownloadXml(int id)
        {
            CertificateFile certificateFile = certificateServices.GetECXmlFile(id);
            if (certificateFile == null)
                return NotFound();
            return Ok(certificateFile);
        }

        /// <summary>
        /// Majors the specified status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        [Route("api/EC/Certificates/Majors/{status}")]
        [HttpGet]
        public IHttpActionResult Majors(string status)
        {
            List<CodeTable> codeTables = certificateServices.GetECMajorByStatus(status);
            if (codeTables == null)
                return NotFound();
            return Ok(codeTables);
        }

        /// <summary>
        /// Statuses the specified status.
        /// </summary>
        /// <returns></returns>
        [Route("api/EC/Certificates/Status")]
        [HttpGet]
        public IHttpActionResult Status()
        {
            List<string> statuses = certificateServices.GetECCertificateStatus();
            if (statuses == null)
                return NotFound();
            return Ok(statuses);
        }

        #endregion Operation-Generation
    }
}