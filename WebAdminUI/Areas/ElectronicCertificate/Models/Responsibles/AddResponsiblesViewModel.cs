// --------------------------------------------------------------------
// <copyright file="AddResponsiblesViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicCertificate;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.ElectronicCertificate;

namespace WebAdminUI.Areas.ElectronicCertificate.Models.Responsibles
{
    /// <summary>
    /// Signers View Model
    /// </summary>
    public class AddResponsiblesViewModel
    {
        /// <summary>
        /// Gets or sets the academic record.
        /// </summary>
        /// <value>
        /// The academic record.
        /// </value>
        [Display(Name = "lblAcademicRecord", ResourceType = typeof(Responsible))]
        public string AcademicRecord { get; set; }

        /// <summary>
        /// Gets or sets the add.
        /// </summary>
        /// <value>
        /// The add.
        /// </value>
        [Display(Name = "lblAdd", ResourceType = typeof(Responsible))]
        public string Add { get; set; }

        /// <summary>
        /// Gets or sets the add signer.
        /// </summary>
        /// <value>
        /// The add signer.
        /// </value>
        [Display(Name = "lblAddResponsible", ResourceType = typeof(Responsible))]
        public string AddResponsible { get; set; }

        /// <summary>
        /// Gets or sets the cancel.
        /// </summary>
        /// <value>
        /// The cancel.
        /// </value>
        [Display(Name = "lblCancel", ResourceType = typeof(Responsible))]
        public string Cancel { get; set; }

        /// <summary>
        /// Gets or sets the curp.
        /// </summary>
        /// <value>
        /// The curp.
        /// </value>
        [Display(Name = "lblCurp", ResourceType = typeof(Responsible))]
        [Required(ErrorMessageResourceType = typeof(Responsible), ErrorMessageResourceName = "lblErrorCurp")]
        [MaxLength(20)]
        public string Curp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [curp exists].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [curp exists]; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "lblErrorCurpExists", ResourceType = typeof(Responsible))]
        public string CurpExists { get; set; }

        /// <summary>
        /// Gets or sets the delete.
        /// </summary>
        /// <value>
        /// The delete.
        /// </value>
        [Display(Name = "lblDelete", ResourceType = typeof(Responsible))]
        public string Delete { get; set; }

        /// <summary>
        /// Gets or sets the edit.
        /// </summary>
        /// <value>
        /// The edit.
        /// </value>
        [Display(Name = "lblEdit", ResourceType = typeof(Responsible))]
        public string Edit { get; set; }

        /// <summary>
        /// Gets or sets the edit signer.
        /// </summary>
        /// <value>
        /// The edit signer.
        /// </value>
        [Display(Name = "lblEditResponsible", ResourceType = typeof(Responsible))]
        public string EditResponsible { get; set; }

        /// <summary>
        /// Gets or sets the electronic degree.
        /// </summary>
        /// <value>
        /// The electronic degree.
        /// </value>
        [Display(Name = "lblElectronicCertificate", ResourceType = typeof(Responsible))]
        public string ElectronicCertificate { get; set; }

        /// <summary>
        /// Gets or sets the fiel.
        /// </summary>
        /// <value>
        /// The fiel.
        /// </value>
        [Display(Name = "lblFiel", ResourceType = typeof(Responsible))]
        public string Fiel { get; set; }

        /// <summary>
        /// Gets or sets the first surname.
        /// </summary>
        /// <value>
        /// The first surname.
        /// </value>
        [Display(Name = "lblFirstSurname", ResourceType = typeof(Responsible))]
        [Required(ErrorMessageResourceType = typeof(Responsible), ErrorMessageResourceName = "lblErrorFirstSurname")]
        [MaxLength(120)]
        public string FirstSurname { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "lblActive", ResourceType = typeof(Responsible))]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Display(Name = "lblName", ResourceType = typeof(Responsible))]
        [Required(ErrorMessageResourceType = typeof(Responsible), ErrorMessageResourceName = "lblErrorName")]
        [MaxLength(120)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the labor position.
        /// </summary>
        /// <value>
        /// The labor position.
        /// </value>
        [Display(Name = "lblPosition", ResourceType = typeof(Responsible))]
        [Required(ErrorMessageResourceType = typeof(Responsible), ErrorMessageResourceName = "lblErrorPosition")]
        public ResponsibleModel Position { get; set; }

        /// <summary>
        /// Gets or sets the signer.
        /// </summary>
        /// <value>
        /// The signer.
        /// </value>
        [Display(Name = "lblResponsible", ResourceType = typeof(Responsible))]
        public string Responsible { get; set; }

        /// <summary>
        /// Gets or sets the signer identifier.
        /// </summary>
        /// <value>
        /// The signer identifier.
        /// </value>
        public int ResponsibleId { get; set; }

        /// <summary>
        /// Gets or sets the signer position identifier.
        /// </summary>
        /// <value>
        /// The signer position identifier.
        /// </value>
        public int ResponsiblePositionId { get; set; }

        /// <summary>
        /// Gets or sets the signers.
        /// </summary>
        /// <value>
        /// The signers.
        /// </value>
        [Display(Name = "lblResposibles", ResourceType = typeof(Responsible))]
        public string Responsibles { get; set; }

        /// <summary>
        /// Gets or sets the save.
        /// </summary>
        /// <value>
        /// The save.
        /// </value>
        [Display(Name = "lblSave", ResourceType = typeof(Responsible))]
        public string Save { get; set; }

        /// <summary>
        /// Gets or sets the second surname.
        /// </summary>
        /// <value>
        /// The second surname.
        /// </value>
        [Display(Name = "lblSecondSurname", ResourceType = typeof(Responsible))]
        [MaxLength(120)]
        public string SecondSurname { get; set; }

        /// <summary>
        /// Gets or sets the setup.
        /// </summary>
        /// <value>
        /// The setup.
        /// </value>
        [Display(Name = "lblSetup", ResourceType = typeof(Responsible))]
        public string Setup { get; set; }

        /// <summary>
        /// Gets or sets the thumbprint.
        /// </summary>
        /// <value>
        /// The thumbprint.
        /// </value>
        [Display(Name = "lblThumbprint", ResourceType = typeof(Responsible))]
        [MaxLength(100)]
        public string Thumbprint { get; set; }

        /// <summary>
        /// Gets or sets the thumbprint valid.
        /// </summary>
        /// <value>
        /// The thumbprint valid.
        /// </value>
        [Display(Name = "lblErrorThumbprintExist", ResourceType = typeof(Responsible))]
        public string ThumbprintExist { get; set; }

        /// <summary>
        /// Gets or sets the thumbprint no installed.
        /// </summary>
        /// <value>
        /// The thumbprint no installed.
        /// </value>
        [Display(Name = "lblErrorThumbprintNoInstalled", ResourceType = typeof(Responsible))]
        public string ThumbprintNoInstalled { get; set; }

        /// <summary>
        /// Gets or sets the thumbprint no private key.
        /// </summary>
        /// <value>
        /// The thumbprint no private key.
        /// </value>
        [Display(Name = "lblErrorThumbprintNoPrivateKey", ResourceType = typeof(Responsible))]
        public string ThumbprintNoPrivateKey { get; set; }
    }
}