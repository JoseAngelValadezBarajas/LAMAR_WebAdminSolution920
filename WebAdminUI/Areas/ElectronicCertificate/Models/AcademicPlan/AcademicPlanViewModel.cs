// --------------------------------------------------------------------
// <copyright file="AcademicPlanViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Areas.ElectronicCertificate.Models.Shared;
using WebAdminUI.Resources.ElectronicCertificate;

namespace WebAdminUI.Areas.ElectronicCertificate.Models
{
    /// <summary>
    ///AcademicPlan view model
    /// </summary>
    public class AcademicPlanViewModel
    {
        /// <summary>
        /// Gets or sets the campus.
        /// </summary>
        /// <value>
        /// The campus.
        /// </value>
        [Display(Name = "lblCampus", ResourceType = typeof(AcademicPlan))]
        public string Campus { get; set; }

        /// <summary>
        /// Gets or sets the campus list.
        /// </summary>
        /// <value>
        /// The campus list.
        /// </value>
        public List<DropdownListViewModel> CampusList { get; set; }

        /// <summary>
        /// Gets or sets the matric term.
        /// </summary>
        /// <value>
        /// The matric term.
        /// </value>
        [Display(Name = "lblMatricTerm", ResourceType = typeof(AcademicPlan))]
        public string MatricTerm { get; set; }

        /// <summary>
        /// Gets or sets the matric year.
        /// </summary>
        /// <value>
        /// The matric year.
        /// </value>
        [Display(Name = "lblMatricYear", ResourceType = typeof(AcademicPlan))]
        public string MatricYear { get; set; }
    }
}