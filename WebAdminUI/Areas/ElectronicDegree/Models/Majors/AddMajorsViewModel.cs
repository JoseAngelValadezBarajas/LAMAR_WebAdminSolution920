// --------------------------------------------------------------------
// <copyright file="AddMajorsViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.ElectronicDegree;

namespace WebAdminUI.Areas.ElectronicDegree.Models.Majors
{
    /// <summary>
    /// Add Majors View Model
    /// </summary>
    public class AddMajorsViewModel
    {
        /// <summary>
        /// Gets or sets the cve.
        /// </summary>
        /// <value>
        /// The cve.
        /// </value>
        [Display(Name = "lblCVE", ResourceType = typeof(Major))]
        [Required(ErrorMessageResourceType = typeof(Major), ErrorMessageResourceName = "lblErrorCVE")]
        [MaxLength(7)]
        public string Cve { get; set; }

        /// <summary>
        /// Gets or sets the length of the cve invalid.
        /// </summary>
        /// <value>
        /// The length of the cve invalid.
        /// </value>
        [Display(Name = "lblCveInvalidLength", ResourceType = typeof(Major))]
        public string CveInvalidLength { get; set; }

        /// <summary>
        /// Gets or sets the cve valid.
        /// </summary>
        /// <value>
        /// The cve valid.
        /// </value>
        [Display(Name = "lblExistsCve", ResourceType = typeof(Major))]
        public string CveValid { get; set; }

        /// <summary>
        /// Gets or sets the education level.
        /// </summary>
        /// <value>
        /// The education level.
        /// </value>
        [Display(Name = "lblEducationLevel", ResourceType = typeof(Major))]
        [Required(ErrorMessageResourceType = typeof(Major), ErrorMessageResourceName = "lblErrorEducationLevel")]
        public List<ListOption> EducationLevel { get; set; }

        /// <summary>
        /// Gets or sets the education level identifier.
        /// </summary>
        /// <value>
        /// The education level identifier.
        /// </value>
        [Display(Name = "lblEducationLevel", ResourceType = typeof(Major))]
        [Required(ErrorMessageResourceType = typeof(Major), ErrorMessageResourceName = "lblErrorEducationLevel")]
        public int EducationLevelId { get; set; }

        /// <summary>
        /// Gets or sets the equivalents identifier.
        /// </summary>
        /// <value>
        /// The equivalents identifier.
        /// </value>
        public int EquivalentsId { get; set; }

        /// <summary>
        /// Gets or sets the legal base catalog.
        /// </summary>
        /// <value>
        /// The legal base catalog.
        /// </value>
        [Display(Name = "lblLegalBaseCatalog", ResourceType = typeof(Major))]
        public CodeLegalBase[] LegalBaseCatalog { get; set; }

        /// <summary>
        /// Gets or sets the legal base identifier.
        /// </summary>
        /// <value>
        /// The legal base identifier.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(Major), ErrorMessageResourceName = "lblErrorLegalBaseId")]
        public int LegalBaseId { get; set; }

        /// <summary>
        /// Gets or sets the name of the major.
        /// </summary>
        /// <value>
        /// The name of the major.
        /// </value>
        [Display(Name = "lblMajorName", ResourceType = typeof(Major))]
        [Required(ErrorMessageResourceType = typeof(Major), ErrorMessageResourceName = "lblErrorMajorName")]
        public string MajorName { get; set; }
    }
}