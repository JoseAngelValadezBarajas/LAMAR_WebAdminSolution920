// --------------------------------------------------------------------
// <copyright file="FiscalRecordCertificate.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System;

namespace PowerCampus.Entities
{
    public class FiscalRecordCertificate
    {
        /// <summary>
        /// Gets or sets the approved date time.
        /// </summary>
        /// <value>
        /// The approved date time.
        /// </value>
        public DateTime? ApprovedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the fiscal record certificate identifier.
        /// </summary>
        /// <value>
        /// The fiscal record certificate identifier.
        /// </value>
        public int? FiscalRecordCertificateId { get; set; }

        /// <summary>
        /// Gets or sets the PDF file.
        /// </summary>
        /// <value>
        /// The PDF file.
        /// </value>
        public byte[] PdfFile { get; set; }

        /// <summary>
        /// Gets or sets the XML file.
        /// </summary>
        /// <value>
        /// The XML file.
        /// </value>
        public string XmlFile { get; set; }
    }
}