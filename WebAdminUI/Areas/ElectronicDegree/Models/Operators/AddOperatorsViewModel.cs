// --------------------------------------------------------------------
// <copyright file="AddOperatorsViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.ElectronicDegree;

namespace WebAdminUI.Areas.ElectronicDegree.Models.Operators
{
    /// <summary>
    /// Add Operators View Model
    /// </summary>
    public class AddOperatorsViewModel
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
        /// Gets or sets the institutions.
        /// </summary>
        /// <value>
        /// The institutions.
        /// </value>
        [Display(Name = "lblInstitutions", ResourceType = typeof(Operator))]
        public List<InstitutionOptions> Institutions { get; set; }

        /// <summary>
        /// Gets or sets the institution signer list.
        /// </summary>
        /// <value>
        /// The institution signer list.
        /// </value>
        public List<InstitutionSignerList> InstitutionSignerLists { get; set; }

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
        public string NumberId { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        [Display(Name = "lblOperatorId", ResourceType = typeof(Operator))]
        public string OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the operator information.
        /// </summary>
        /// <value>
        /// The operator information.
        /// </value>
        public List<PermissionsList> PermissionList { get; set; }
    }
}