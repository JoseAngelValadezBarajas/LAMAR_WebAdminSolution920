// --------------------------------------------------------------------
// <copyright file="SepServicesResponse.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities.ElectronicDegree
{
    public enum SepStatus
    {
        Success = 0,
        InvalidRequest = 1,
        WebServiceUnavailable = 2,
        InvalidElectronicDegree = 3,
        UnableToProcessXls = 4,
        UnableToProcessZip = 5,
        UnableToCancel = 6,
    }

    /// <summary>
    /// Handles the response information from SEP web services
    /// </summary>
    public class SepServicesResponse
    {
        /// <summary>
        /// Gets or sets the lot number.
        /// </summary>
        /// <value>
        /// The lot number.
        /// </value>
        public string lotNumber { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string message { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public SepStatus status { get; set; }
    }
}