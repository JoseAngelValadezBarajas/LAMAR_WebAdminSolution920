// --------------------------------------------------------------------
// <copyright file="ThumbprintStatus.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
namespace PowerCampus.Entities.ElectronicCertificate
{
    /// <summary>
    /// ThumbprintStatus
    /// </summary>
    public enum ThumbprintStatus
    {
        /// <summary>
        /// The assigned
        /// </summary>
        Assigned,

        /// <summary>
        /// The installed
        /// </summary>
        Installed,

        /// <summary>
        /// The no installed
        /// </summary>
        NoInstalled,

        /// <summary>
        /// The no private key
        /// </summary>
        NoPrivateKey
    }
}