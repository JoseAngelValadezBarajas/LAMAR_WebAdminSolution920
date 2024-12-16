// --------------------------------------------------------------------
// <copyright file="AboutViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.Home
{
    /// <summary>
    /// AboutViewModel
    /// </summary>
    public class AboutViewModel
    {
        /// <summary>
        /// Gets or sets the build date.
        /// </summary>
        /// <value>
        /// The build date.
        /// </value>
        [Display(Name = nameof(AboutViewModelResource.lblBuildDate), ResourceType = typeof(AboutViewModelResource))]
        public string BuildDate { get; set; }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>
        /// The copyright.
        /// </value>
        [Display(Name = nameof(AboutViewModelResource.lblCopyright), ResourceType = typeof(AboutViewModelResource))]
        public string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the fiscal records services version.
        /// </summary>
        /// <value>
        /// The fiscal records services version.
        /// </value>
        [Display(Name = nameof(AboutViewModelResource.lblFiscalRecordsServicesVersion), ResourceType = typeof(AboutViewModelResource))]
        public string FiscalRecordsServicesVersion { get; set; }

        /// <summary>
        /// Gets or sets the fiscal records UI version.
        /// </summary>
        /// <value>
        /// The fiscal records UI version.
        /// </value>
        [Display(Name = nameof(AboutViewModelResource.lblFiscalRecordsUIVersion), ResourceType = typeof(AboutViewModelResource))]
        public string FiscalRecordsUIVersion { get; set; }

        /// <summary>
        /// Gets or sets the trademark.
        /// </summary>
        /// <value>
        /// The trademark.
        /// </value>
        [Display(Name = nameof(AboutViewModelResource.lblTrademark), ResourceType = typeof(AboutViewModelResource))]
        public string Trademark { get; set; }
    }
}