// --------------------------------------------------------------------
// <copyright file="Certificate.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System;

namespace PowerCampus.Entities.ElectronicCertificate
{
    /// <summary>
    /// Certificate
    /// </summary>
    public class Certificate
    {
        /// <summary>
        /// Gets or sets the major.
        /// </summary>
        /// <value>
        /// The major.
        /// </value>
        public string Campus { get; set; }

        /// <summary>
        /// Gets or sets the type of the certification.
        /// </summary>
        /// <value>
        /// The type of the certification.
        /// </value>
        public string CertificationType { get; set; }

        /// <summary>
        /// Gets or sets the electronic certificate file identifier.
        /// </summary>
        /// <value>
        /// The electronic certificate file identifier.
        /// </value>
        public int ElectronicCertificateFileId { get; set; }

        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        public string Folio { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has PDF file.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has PDF file; otherwise, <c>false</c>.
        /// </value>
        public bool HasPdfFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has XML file.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has XML file; otherwise, <c>false</c>.
        /// </value>
        public bool HasXmlFile { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the issuing date.
        /// </summary>
        /// <value>
        /// The issuing date.
        /// </value>
        public DateTime IssuingDate { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the payment folio.
        /// </summary>
        /// <value>
        /// The payment folio.
        /// </value>
        public string PaymentFolio { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [PDF size].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [PDF size]; otherwise, <c>false</c>.
        /// </value>
        public long PdfSize { get; set; }

        /// <summary>
        /// Gets or sets the people code identifier.
        /// </summary>
        /// <value>
        /// The people code identifier.
        /// </value>
        public string PeopleCodeId { get; set; }

        /// <summary>
        /// Gets or sets the program.
        /// </summary>
        /// <value>
        /// The program.
        /// </value>
        public string Program { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the name of the student.
        /// </summary>
        /// <value>
        /// The name of the student.
        /// </value>
        public string StudentName { get; set; }

        /// <summary>
        /// Gets or sets the size of the XML.
        /// </summary>
        /// <value>
        /// The size of the XML.
        /// </value>
        public int XmlSize { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class CertificateFile
    {
        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        public string Folio { get; set; }

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