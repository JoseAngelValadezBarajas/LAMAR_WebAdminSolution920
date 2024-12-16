// --------------------------------------------------------------------
// <copyright file="IssuingCertificateViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.ElectronicCertificate;

namespace WebAdminUI.Areas.ElectronicCertificate.Models.Generation
{
    /// <summary>
    /// IssuingCertificateViewModel
    /// </summary>
    public class IssuingCertificateViewModel
    {
        /// <summary>
        /// Gets or sets the type of the certification.
        /// </summary>
        /// <value>
        /// The type of the certification.
        /// </value>
        [Display(Name = "lblCertificationType", ResourceType = typeof(Generate))]
        public string CertificationType { get; set; }

        /// <summary>
        /// Gets or sets the issuing date.
        /// </summary>
        /// <value>
        /// The issuing date.
        /// </value>
        [Display(Name = "lblIssuingDate", ResourceType = typeof(Generate))]
        public string IssuingDate { get; set; }

        /// <summary>
        /// Gets or sets the issuing place.
        /// </summary>
        /// <value>
        /// The issuing place.
        /// </value>
        [Display(Name = "lblIssuingPlace", ResourceType = typeof(Generate))]
        public string IssuingPlace { get; set; }
    }
}