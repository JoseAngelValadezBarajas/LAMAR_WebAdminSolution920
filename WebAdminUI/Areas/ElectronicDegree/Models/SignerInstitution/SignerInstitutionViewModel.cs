// --------------------------------------------------------------------
// <copyright file="SignerInstitutionViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.ElectronicDegree;

namespace WebAdminUI.Areas.ElectronicDegree.Models.SignerInstitution
{
    /// <summary>
    /// Institution-Signer Related View Model
    /// </summary>
    public class InstitutionSignerViewModel
    {
        /// <summary>
        /// Gets or sets the abreviation title.
        /// </summary>
        /// <value>
        /// The abreviation title.
        /// </value>
        public string AbreviationTitle { get; set; }

        /// <summary>
        /// Gets or sets the institution identifier.
        /// </summary>
        /// <value>
        /// The institution identifier.
        /// </value>
        public int? InstitutionId { get; set; }

        /// <summary>
        /// Gets or sets the institution signer identifier.
        /// </summary>
        /// <value>
        /// The institution signer identifier.
        /// </value>
        public int InstitutionSignerId { get; set; }

        /// <summary>
        /// Gets or sets the signer identifier.
        /// </summary>
        /// <value>
        /// The signer identifier.
        /// </value>
        public int SignerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the signer.
        /// </summary>
        /// <value>
        /// The name of the signer.
        /// </value>
        public string SignerName { get; set; }
    }

    /// <summary>
    /// Signer-Institution View Model
    /// </summary>
    public class SignerInstitutionViewModel
    {
        /// <summary>
        /// Gets or sets the institution code.
        /// </summary>
        /// <value>
        /// The institution code.
        /// </value>
        [Display(Name = "lblInstitutionCode", ResourceType = typeof(InstitutionSigner))]
        public string InstitutionCode { get; set; }

        /// <summary>
        /// Gets or sets the institution signer identifier.
        /// </summary>
        /// <value>
        /// The institution signer identifier.
        /// </value>
        public int InstitutionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the institution.
        /// </summary>
        /// <value>
        /// The name of the institution.
        /// </value>
        [Display(Name = "lblInstitutionName", ResourceType = typeof(InstitutionSigner))]
        public string InstitutionName { get; set; }

        /// <summary>
        /// Gets or sets the institution signer related.
        /// </summary>
        /// <value>
        /// The institution signer related.
        /// </value>
        public List<InstitutionSignerViewModel> InstitutionSignerViewModel { get; set; }

        /// <summary>
        /// Gets or sets the signer institution list.
        /// </summary>
        /// <value>
        /// The signer institution list.
        /// </value>
        public List<LaborPosition> SignerInstitutionList { get; set; }

        /// <summary>
        /// Gets or sets the signers.
        /// </summary>
        /// <value>
        /// The signers.
        /// </value>
        [Display(Name = "lblSigners", ResourceType = typeof(InstitutionSigner))]
        public string Signers { get; set; }

        /// <summary>
        /// Gets or sets the signers list.
        /// </summary>
        /// <value>
        /// The signers list.
        /// </value>
        public List<LaborPosition> SignersList { get; set; }
    }
}