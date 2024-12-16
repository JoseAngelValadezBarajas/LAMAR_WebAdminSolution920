// --------------------------------------------------------------------
// <copyright file="IECCertificateServices.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces.ElectronicCertificate
{
    /// <summary>
    /// IECCertificateServices
    /// </summary>
    public interface IECCertificateServices
    {
        #region Electronic Certificate PowerCampus

        /// <summary>
        /// Creates the specified certificate information.
        /// </summary>
        /// <param name="certificateInfo">The certificate information.</param>
        /// <returns></returns>
        bool Create(CertificateInfo certificateInfo);

        /// <summary>
        /// Deletes the specified electronic certificate identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        bool Delete(int id);

        #endregion Electronic Certificate PowerCampus

        #region Electronic Certificate Sep

        /// <summary>
        /// Creates the original string.
        /// </summary>
        /// <param name="electronicCertificate">The electronic certificate.</param>
        /// <returns></returns>
        string CreateOriginalString(Dec electronicCertificate);

        /// <summary>
        /// Creates the signature stamp.
        /// </summary>
        /// <param name="originalString">The original string.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <returns></returns>
        (string signature, string serialNumber, string publicCertificate) CreateSignatureStamp(string originalString, string thumbprint);

        /// <summary>
        /// Stamps the specified electronic certificate sep.
        /// </summary>
        /// <param name="electronicCertificateSep">The electronic certificate sep.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <param name="certificateOperation">The certificate operation.</param>
        /// <returns></returns>
        bool Stamp(Dec electronicCertificateSep, string thumbprint, CertificateOperation certificateOperation);

        /// <summary>
        /// Converts to dec.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        Dec ToDec(string xml);

        /// <summary>
        /// Converts to xml.
        /// </summary>
        /// <param name="electronicCertificateSep">The electronic certificate sep.</param>
        /// <returns></returns>
        string ToXml(Dec electronicCertificateSep);

        #endregion Electronic Certificate Sep

        #region Computer Certificate

        /// <summary>
        /// Gets the thumbprint status
        /// </summary>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <returns></returns>
        ThumbprintStatus GetThumbprintStatus(string thumbprint);

        #endregion Computer Certificate

        #region Operation-Generation

        /// <summary>
        /// Gets the ec advanced search.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        List<Certificate> GetECAdvancedSearch(Search search);

        /// <summary>
        /// Gets the ec basic search.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        List<Certificate> GetECBasicSearch(string keyword, string status);

        /// <summary>
        /// Gets the ec campus by status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        List<CodeTable> GetECCampusByStatus(string status);

        /// <summary>
        /// Gets the ec certificate detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        CertificateInfo GetECCertificateDetail(int id);

        /// <summary>
        /// Gets the ec certificate status.
        /// </summary>
        /// <returns></returns>
        List<string> GetECCertificateStatus();

        /// <summary>
        /// Gets the ec certification type by status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        List<CodeTable> GetECCertificationTypeByStatus(string status);

        /// <summary>
        /// Gets the ec download files.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        List<CertificateFile> GetECDownloadFiles(List<int> ids);

        /// <summary>
        /// Gets the ec major by status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        List<CodeTable> GetECMajorByStatus(string status);

        /// <summary>
        /// Gets the ec PDF file.
        /// </summary>
        /// <param name="electronicCertificateFileId">The electronic certificate file identifier.</param>
        /// <returns></returns>
        CertificateFile GetECPdfFile(int electronicCertificateFileId);

        /// <summary>
        /// Gets the ec XML file.
        /// </summary>
        /// <param name="electronicCertificateFileId">The electronic certificate file identifier.</param>
        /// <returns></returns>
        CertificateFile GetECXmlFile(int electronicCertificateFileId);

        /// <summary>
        /// Updates the ec certificate status.
        /// </summary>
        /// <param name="certificateOperation">The certificate operation.</param>
        /// <returns></returns>
        bool UpdateECCertificateStatus(CertificateOperation certificateOperation);

        #endregion Operation-Generation
    }
}