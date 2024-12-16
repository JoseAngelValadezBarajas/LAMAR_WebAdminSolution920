// --------------------------------------------------------------------
// <copyright file="AddOperatorsViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.ElectronicCertificate;

namespace WebAdminUI.Areas.ElectronicCertificate.Models.Operators
{
    /// <summary>
    /// Add Operators View Model
    /// </summary>
    public class AddOperatorsViewModel
    {
        /// <summary>
        /// Gets or sets the institution options.
        /// </summary>
        /// <value>
        /// The institution options.
        /// </value>
        [Display(Name = "lblCampuses", ResourceType = typeof(Operator))]
        public List<InstitutionCampusOperator> InstitutionOptions { get; set; }

        /// <summary>
        /// Gets or sets the issuing places.
        /// </summary>
        /// <value>
        /// The issuing places.
        /// </value>
        [Display(Name = "lblIssuingPlace", ResourceType = typeof(Operator))]
        public List<IssuingPlace> IssuingPlaces { get; set; }

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
        /// Gets or sets the operator list.
        /// </summary>
        /// <value>
        /// The operator list.
        /// </value>
        public List<OperatorsList> OperatorList { get; set; }

        /// <summary>
        /// Gets or sets the operator information.
        /// </summary>
        /// <value>
        /// The operator information.
        /// </value>
        public List<PermissionsList> PermissionList { get; set; }
    }
}