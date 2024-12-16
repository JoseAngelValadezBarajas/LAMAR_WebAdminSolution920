// --------------------------------------------------------------------
// <copyright file="MajorsViewModel.cs" company="Ellucian">
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
    /// Majors View Model
    /// </summary>
    public class MajorsViewModel
    {
        /// <summary>
        /// Gets or sets the cve.
        /// </summary>
        /// <value>
        /// The cve.
        /// </value>
        [Display(Name = "lblCVE", ResourceType = typeof(Major))]
        public string Cve { get; set; }

        /// <summary>
        /// Gets or sets the delete.
        /// </summary>
        /// <value>
        /// The delete.
        /// </value>
        [Display(Name = "lblDelete", ResourceType = typeof(Major))]
        public string Delete { get; set; }

        /// <summary>
        /// Gets or sets the edit.
        /// </summary>
        /// <value>
        /// The edit.
        /// </value>
        [Display(Name = "lblEdit", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        public string Edit { get; set; }

        /// <summary>
        /// Gets or sets the education level.
        /// </summary>
        /// <value>
        /// The education level.
        /// </value>
        [Display(Name = "lblEducationLevel", ResourceType = typeof(Major))]
        public string EducationLevel { get; set; }

        /// <summary>
        /// Gets or sets the electronic degree inst major identifier.
        /// </summary>
        /// <value>
        /// The electronic degree inst major identifier.
        /// </value>
        public int? ElectronicDegreeInstMajorId { get; set; }

        /// <summary>
        /// Gets or sets the institutions.
        /// </summary>
        /// <value>
        /// The institutions.
        /// </value>
        [Display(Name = "lblInstitutions", ResourceType = typeof(Major))]
        public string Institutions { get; set; }

        /// <summary>
        /// Gets or sets the major identifier.
        /// </summary>
        /// <value>
        /// The major identifier.
        /// </value>
        public int MajorId { get; set; }

        /// <summary>
        /// Gets or sets the name of the major.
        /// </summary>
        /// <value>
        /// The name of the major.
        /// </value>
        [Display(Name = "lblMajorName", ResourceType = typeof(Major))]
        public string MajorName { get; set; }

        /// <summary>
        /// Gets or sets the rvoe.
        /// </summary>
        /// <value>
        /// The rvoe.
        /// </value>
        [Display(Name = "lblRvoe", ResourceType = typeof(Major))]
        public List<DropDownListModel> Rvoe { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MajorsViewModel"/> is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if selected; otherwise, <c>false</c>.
        /// </value>
        public bool Selected { get; set; }

        /// <summary>
        /// Gets or sets the selected rvoe.
        /// </summary>
        /// <value>
        /// The selected rvoe.
        /// </value>
        [Display(Name = "lblSelectedRvoe", ResourceType = typeof(Major))]
        public List<RvoeList> SelectedRvoes { get; set; }
    }
}