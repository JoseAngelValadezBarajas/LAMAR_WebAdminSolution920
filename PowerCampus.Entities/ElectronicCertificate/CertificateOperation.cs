// --------------------------------------------------------------------
// <copyright file="CertificateOperation.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
namespace PowerCampus.Entities.ElectronicCertificate
{
    /// <summary>
    /// CertificateOperation
    /// </summary>
    public class CertificateOperation
    {
        /// <summary>
        /// Gets or sets the certificate file identifier.
        /// </summary>
        /// <value>
        /// The certificate file identifier.
        /// </value>
        public int CertificateFileId { get; set; }

        /// <summary>
        /// Gets or sets the certificate identifier.
        /// </summary>
        /// <value>
        /// The certificate identifier.
        /// </value>
        public int CertificateId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [generate XML].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [generate XML]; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateXml { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public string OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the payment folio.
        /// </summary>
        /// <value>
        /// The payment folio.
        /// </value>
        public string PaymentFolio { get; set; }

        /// <summary>
        /// Gets or sets the PDF file.
        /// </summary>
        /// <value>
        /// The PDF file.
        /// </value>
        public string PdfFile { get; set; }

        /// <summary>
        /// Gets or sets the operation.
        /// </summary>
        /// <value>
        /// The operation.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the XML file.
        /// </summary>
        /// <value>
        /// The XML file.
        /// </value>
        public string XmlFile { get; set; }
    }
}