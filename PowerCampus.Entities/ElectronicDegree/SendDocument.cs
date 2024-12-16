// --------------------------------------------------------------------
// <copyright file="SendDocument.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities.ElectronicDegree
{
    /// <summary>
    ///  Model to specify the Electronic Degree Record to be sent for SEP
    /// </summary>
    public class SendDocument
    {
        /// <summary>
        /// Gets or sets the cancelation code.
        /// </summary>
        /// <value>
        /// The cancelation code.
        /// </value>
        public string CancelationCode { get; set; }

        /// <summary>
        /// Gets or sets the cancelation description.
        /// </summary>
        /// <value>
        /// The cancelation description.
        /// </value>
        public string CancelationDescription { get; set; }

        /// <summary>
        /// Gets or sets the electronic degree identifier.
        /// </summary>
        /// <value>
        /// The electronic degree identifier.
        /// </value>
        public int ElectronicDegreeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the operator.
        /// </summary>
        /// <value>
        /// The name of the operator.
        /// </value>
        public string OperatorName { get; set; }
    }
}