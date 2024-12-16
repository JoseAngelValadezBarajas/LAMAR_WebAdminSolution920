// --------------------------------------------------------------------
// <copyright file="ResponsiblesViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.ElectronicCertificate;

namespace WebAdminUI.Areas.ElectronicCertificate.Models.Responsibles
{
    /// <summary>
    /// Signers List
    /// </summary>
    public class ResponsiblesIndexList
    {
        /// <summary>
        /// Gets or sets the edit.
        /// </summary>
        /// <value>
        /// The edit.
        /// </value>
        [Display(Name = "lblEdit", ResourceType = typeof(Responsible))]
        public string Edit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the labor position.
        /// </summary>
        /// <value>
        /// The labor position.
        /// </value>
        [Display(Name = "lblPosition", ResourceType = typeof(Responsible))]
        public string Position { get; set; }

        /// <summary>
        /// Gets or sets the signer identifier.
        /// </summary>
        /// <value>
        /// The signer identifier.
        /// </value>
        public int ResponsibleId { get; set; }

        /// <summary>
        /// Gets or sets the signer.
        /// </summary>
        /// <value>
        /// The signer.
        /// </value>
        [Display(Name = "lblResponsibles", ResourceType = typeof(Responsible))]
        public string Responsibles { get; set; }

        /// <summary>
        /// Gets or sets the delete.
        /// </summary>
        /// <value>
        /// The delete.
        /// </value>
        [Display(Name = "lblStatus", ResourceType = typeof(Responsible))]
        public string Status { get; set; }
    }

    /// <summary>
    /// Signers View Model
    /// </summary>
    public class ResponsiblesViewModel
    {
        /// <summary>
        /// Gets or sets the signers list.
        /// </summary>
        /// <value>
        /// The signers list.
        /// </value>
        public List<ResponsiblesIndexList> ResponsiblesList { get; set; }
    }
}