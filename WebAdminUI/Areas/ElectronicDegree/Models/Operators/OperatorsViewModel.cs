// --------------------------------------------------------------------
// <copyright file="OperatorsViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.ElectronicDegree;

namespace WebAdminUI.Areas.ElectronicDegree.Models.Operators
{
    /// <summary>
    /// Institution Option View Model
    /// </summary>
    public class InstitutionOptionViewModel
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the institution identifier.
        /// </summary>
        /// <value>
        /// The institution identifier.
        /// </value>
        public int InstitutionId { get; set; }
    }

    /// <summary>
    /// Operators View Model
    /// </summary>
    public class OperatorsViewModel
    {
        /// <summary>
        /// Gets or sets the institution.
        /// </summary>
        /// <value>
        /// The institution.
        /// </value>
        [Display(Name = "lblInstitution", ResourceType = typeof(Operator))]
        public int Institution { get; set; }

        /// <summary>
        /// Gets or sets the institution option list.
        /// </summary>
        /// <value>
        /// The institution option list.
        /// </value>
        public List<InstitutionOptionViewModel> InstitutionOptionList { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Display(Name = "lblName", ResourceType = typeof(Operator))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the number identifier.
        /// </summary>
        /// <value>
        /// The number identifier.
        /// </value>
        [Display(Name = "lblNumberId", ResourceType = typeof(Operator))]
        public string NumberID { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        [Display(Name = "lblOperatorId", ResourceType = typeof(Operator))]
        public string OperatorID { get; set; }

        /// <summary>
        /// Gets or sets the visualization permissions.
        /// </summary>
        /// <value>
        /// The visualization permissions.
        /// </value>
        [Display(Name = "lblVisualizationPer", ResourceType = typeof(Operator))]
        public int Permissions { get; set; }
    }
}