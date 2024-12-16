// --------------------------------------------------------------------
// <copyright file="AddSignersViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicDegree;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.ElectronicDegree;

namespace WebAdminUI.Areas.ElectronicDegree.Models.Signers
{
    /// <summary>
    /// Signers View Model
    /// </summary>
    public class AddSignersViewModel
    {
        /// <summary>
        /// Gets or sets the abbreviation identifier.
        /// </summary>
        /// <value>
        /// The abbreviation identifier.
        /// </value>
        public int AbbreviationId { get; set; }

        /// <summary>
        /// Gets or sets the academic record.
        /// </summary>
        /// <value>
        /// The academic record.
        /// </value>
        [Display(Name = "lblAcademicRecord", ResourceType = typeof(Signer))]
        public string AcademicRecord { get; set; }

        /// <summary>
        /// Gets or sets the add.
        /// </summary>
        /// <value>
        /// The add.
        /// </value>
        [Display(Name = "lblAdd", ResourceType = typeof(Signer))]
        public string Add { get; set; }

        /// <summary>
        /// Gets or sets the add signer.
        /// </summary>
        /// <value>
        /// The add signer.
        /// </value>
        [Display(Name = "lblAddSigner", ResourceType = typeof(Signer))]
        public string AddSigner { get; set; }

        /// <summary>
        /// Gets or sets the cancel.
        /// </summary>
        /// <value>
        /// The cancel.
        /// </value>
        [Display(Name = "lblCancel", ResourceType = typeof(Signer))]
        public string Cancel { get; set; }

        /// <summary>
        /// Gets or sets the curp.
        /// </summary>
        /// <value>
        /// The curp.
        /// </value>
        [Display(Name = "lblCurp", ResourceType = typeof(Signer))]
        [Required(ErrorMessageResourceType = typeof(Signer), ErrorMessageResourceName = "lblErrorCurp")]
        [MaxLength(40)]
        public string Curp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [curp exists].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [curp exists]; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "lblErrorCurpExists", ResourceType = typeof(Signer))]
        public string CurpExists { get; set; }

        /// <summary>
        /// Gets or sets the delete.
        /// </summary>
        /// <value>
        /// The delete.
        /// </value>
        [Display(Name = "lblDelete", ResourceType = typeof(Signer))]
        public string Delete { get; set; }

        /// <summary>
        /// Gets or sets the edit.
        /// </summary>
        /// <value>
        /// The edit.
        /// </value>
        [Display(Name = "lblEdit", ResourceType = typeof(Signer))]
        public string Edit { get; set; }

        /// <summary>
        /// Gets or sets the edit signer.
        /// </summary>
        /// <value>
        /// The edit signer.
        /// </value>
        [Display(Name = "lblEditSigner", ResourceType = typeof(Signer))]
        public string EditSigner { get; set; }

        /// <summary>
        /// Gets or sets the electronic degree.
        /// </summary>
        /// <value>
        /// The electronic degree.
        /// </value>
        [Display(Name = "lblElectronicDegree", ResourceType = typeof(Signer))]
        public string ElectronicDegree { get; set; }

        /// <summary>
        /// Gets or sets the fiel.
        /// </summary>
        /// <value>
        /// The fiel.
        /// </value>
        [Display(Name = "lblFiel", ResourceType = typeof(Signer))]
        public string Fiel { get; set; }

        /// <summary>
        /// Gets or sets the first surname.
        /// </summary>
        /// <value>
        /// The first surname.
        /// </value>
        [Display(Name = "lblFirstSurname", ResourceType = typeof(Signer))]
        [Required(ErrorMessageResourceType = typeof(Signer), ErrorMessageResourceName = "lblErrorFirstSurname")]
        [MaxLength(120)]
        public string FirstSurname { get; set; }

        /// <summary>
        /// Gets or sets the institution.
        /// </summary>
        /// <value>
        /// The institution.
        /// </value>
        [Display(Name = "lblInstitution", ResourceType = typeof(Signer))]
        public string Institution { get; set; }

        /// <summary>
        /// Gets or sets the institutions code.
        /// </summary>
        /// <value>
        /// The institutions code.
        /// </value>
        [Display(Name = "lblInstitutionsCode", ResourceType = typeof(Signer))]
        public string InstitutionsCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the institutions.
        /// </summary>
        /// <value>
        /// The name of the institutions.
        /// </value>
        [Display(Name = "lblInstitutionsName", ResourceType = typeof(Signer))]
        public string InstitutionsName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "lblActive", ResourceType = typeof(Signer))]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the labor position.
        /// </summary>
        /// <value>
        /// The labor position.
        /// </value>
        [Display(Name = "lblLaborPosition", ResourceType = typeof(Signer))]
        [Required(ErrorMessageResourceType = typeof(Signer), ErrorMessageResourceName = "lblErrorLaborPosition")]
        public SignersModel LaborPosition { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Display(Name = "lblName", ResourceType = typeof(Signer))]
        [Required(ErrorMessageResourceType = typeof(Signer), ErrorMessageResourceName = "lblErrorName")]
        [MaxLength(120)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the professional title abbrevation.
        /// </summary>
        /// <value>
        /// The professional title abbrevation.
        /// </value>
        [Display(Name = "lblProfessionalTitleAbbreviation", ResourceType = typeof(Signer))]
        [Required(ErrorMessageResourceType = typeof(Signer), ErrorMessageResourceName = "lblErrorAbbreviation")]
        public SignersModel ProfessionalTitleAbbreviation { get; set; }

        /// <summary>
        /// Gets or sets the save.
        /// </summary>
        /// <value>
        /// The save.
        /// </value>
        [Display(Name = "lblSave", ResourceType = typeof(Signer))]
        public string Save { get; set; }

        /// <summary>
        /// Gets or sets the second surname.
        /// </summary>
        /// <value>
        /// The second surname.
        /// </value>
        [Display(Name = "lblSecondSurname", ResourceType = typeof(Signer))]
        [MaxLength(120)]
        public string SecondSurname { get; set; }

        /// <summary>
        /// Gets or sets the setup.
        /// </summary>
        /// <value>
        /// The setup.
        /// </value>
        [Display(Name = "lblSetup", ResourceType = typeof(Signer))]
        public string Setup { get; set; }

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
        /// Gets or sets the signer position identifier.
        /// </summary>
        /// <value>
        /// The signer position identifier.
        /// </value>
        public int SignerPositionId { get; set; }

        /// <summary>
        /// Gets or sets the signers.
        /// </summary>
        /// <value>
        /// The signers.
        /// </value>
        [Display(Name = "lblSigners", ResourceType = typeof(Signer))]
        public string Signers { get; set; }

        /// <summary>
        /// Gets or sets the thumbprint.
        /// </summary>
        /// <value>
        /// The thumbprint.
        /// </value>
        [Display(Name = "lblThumbprint", ResourceType = typeof(Signer))]
        [Required(ErrorMessageResourceType = typeof(Signer), ErrorMessageResourceName = "lblErrorThumprint")]
        [MaxLength(100)]
        public string Thumbprint { get; set; }

        /// <summary>
        /// Gets or sets the thumbprint valid.
        /// </summary>
        /// <value>
        /// The thumbprint valid.
        /// </value>
        [Display(Name = "lblErrorThumprintExists", ResourceType = typeof(Signer))]
        public string ThumbprintValid { get; set; }
    }
}