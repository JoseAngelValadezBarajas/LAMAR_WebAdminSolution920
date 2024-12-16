// --------------------------------------------------------------------
// <copyright file="CertificateStatus.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
namespace PowerCampus.Entities.ElectronicCertificate
{
    /// <summary>
    /// The status of the electronic certificate: (G) enerated, (S)tamped, (P) rocessed correctly, (E) rror, (C)anceled.
    /// </summary>
    public static class CertificateStatus
    {
        /// <summary>
        /// The canceled status
        /// </summary>
        public const string Canceled = "C";

        /// <summary>
        /// The error status
        /// </summary>
        public const string Error = "E";

        /// <summary>
        /// The generated status
        /// </summary>
        public const string Generated = "G";

        /// <summary>
        /// The processed status
        /// </summary>
        public const string Processed = "P";

        /// <summary>
        /// The stamped status
        /// </summary>
        public const string Stamped = "S";
    }
}