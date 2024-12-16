// --------------------------------------------------------------------
// <copyright file="AboutViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.General;

namespace WebAdminUI.Areas.General.Models
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
        [Display(Name = nameof(About.lblBuildDate), ResourceType = typeof(About))]
        public string BuildDate { get; set; }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>
        /// The copyright.
        /// </value>
        [Display(Name = nameof(About.lblCopyright), ResourceType = typeof(About))]
        public string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the trademark.
        /// </summary>
        /// <value>
        /// The trademark.
        /// </value>
        [Display(Name = nameof(About.lblTrademark), ResourceType = typeof(About))]
        public string Trademark { get; set; }

        /// <summary>
        /// Gets or sets the UI version.
        /// </summary>
        /// <value>
        /// The UI version.
        /// </value>
        [Display(Name = nameof(About.lblUIVersion), ResourceType = typeof(About))]
        public string UIVersion { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [Display(Name = nameof(About.lblVersion), ResourceType = typeof(About))]
        public string Version { get; set; }
    }
}