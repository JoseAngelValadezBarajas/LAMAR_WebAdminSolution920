// --------------------------------------------------------------------
// <copyright file="OperatorsViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.ElectronicCertificate;

namespace WebAdminUI.Areas.ElectronicCertificate.Models.Operators
{
    /// <summary>
    /// Operators View Model
    /// </summary>
    public class OperatorsViewModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Display(Name = "lblName", ResourceType = typeof(Operator))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the number campuses.
        /// </summary>
        /// <value>
        /// The number campuses.
        /// </value>
        [Display(Name = "lblNumberCampus", ResourceType = typeof(Operator))]
        public int NumberCampuses { get; set; }

        /// <summary>
        /// Gets or sets the number identifier.
        /// </summary>
        /// <value>
        /// The number identifier.
        /// </value>
        [Display(Name = "lblNumberId", ResourceType = typeof(Operator))]
        public string NumberID { get; set; }

        /// <summary>
        /// Gets or sets the visualization permissions.
        /// </summary>
        /// <value>
        /// The visualization permissions.
        /// </value>
        [Display(Name = "lblNumberVisualizationPer", ResourceType = typeof(Operator))]
        public int NumberPermissions { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        [Display(Name = "lblOperatorId", ResourceType = typeof(Operator))]
        public string OperatorID { get; set; }
    }
}