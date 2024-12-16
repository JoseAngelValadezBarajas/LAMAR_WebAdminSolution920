// --------------------------------------------------------------------
// <copyright file="SignersViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.ElectronicDegree;

namespace WebAdminUI.Areas.ElectronicDegree.Models.Signers
{
    /// <summary>
    /// Signers List
    /// </summary>
    public class SignersIndexList
    {
        /// <summary>
        /// Gets or sets the edit.
        /// </summary>
        /// <value>
        /// The edit.
        /// </value>
        [Display(Name = "lblEdit", ResourceType = typeof(Signer))]
        public string Edit { get; set; }

        /// <summary>
        /// Gets or sets the institutions.
        /// </summary>
        /// <value>
        /// The institutions.
        /// </value>
        [Display(Name = "lblInstitutionsName", ResourceType = typeof(Signer))]
        public int? Institutions { get; set; }

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
        [Display(Name = "lblLaborPosition", ResourceType = typeof(Signer))]
        public string LaborPosition { get; set; }

        /// <summary>
        /// Gets or sets the signer.
        /// </summary>
        /// <value>
        /// The signer.
        /// </value>
        [Display(Name = "lblSigner", ResourceType = typeof(Signer))]
        public string Signer { get; set; }

        /// <summary>
        /// Gets or sets the signer identifier.
        /// </summary>
        /// <value>
        /// The signer identifier.
        /// </value>
        public int SignerId { get; set; }

        /// <summary>
        /// Gets or sets the delete.
        /// </summary>
        /// <value>
        /// The delete.
        /// </value>
        [Display(Name = "lblStatus", ResourceType = typeof(Signer))]
        public string Status { get; set; }
    }

    /// <summary>
    /// Signers View Model
    /// </summary>
    public class SignersViewModel
    {
        /// <summary>
        /// Gets or sets the signers list.
        /// </summary>
        /// <value>
        /// The signers list.
        /// </value>
        public List<SignersIndexList> SignersList { get; set; }
    }
}