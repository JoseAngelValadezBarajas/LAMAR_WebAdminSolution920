// --------------------------------------------------------------------
// <copyright file="AddInstitutionViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Areas.ElectronicDegree.Models.Majors;

namespace WebAdminUI.Areas.ElectronicDegree.Models.Institution
{
    /// <summary>
    /// AddInstitutionViewModel
    /// </summary>
    public class AddInstitutionViewModel
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name = "lblCode", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        [Required(ErrorMessageResourceType = typeof(Resources.ElectronicDegree.Institution), ErrorMessageResourceName = "lblErrorCode")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the code exists.
        /// </summary>
        /// <value>
        /// The code exists.
        /// </value>
        [Display(Name = "lblCodeExists", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        public bool CodeExists { get; set; }

        /// <summary>
        /// Gets or sets the name of the create user.
        /// </summary>
        /// <value>
        /// The name of the create user.
        /// </value>
        public string CreateUserName { get; set; }

        /// <summary>
        /// Gets or sets the degree folio exist.
        /// </summary>
        /// <value>
        /// The degree folio exists.
        /// </value>
        [Display(Name = "lblDegreeFolioExists", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        public bool DegreeFolioExists { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name = "lblEdit", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        public string Edit { get; set; }

        /// <summary>
        /// Gets or sets the equivalent identifier.
        /// </summary>
        /// <value>
        /// The equivalent identifier.
        /// </value>
        public string EquivalentId { get; set; }

        /// <summary>
        /// Gets or sets the equivalents.
        /// </summary>
        /// <value>
        /// The equivalents.
        /// </value>
        [Display(Name = "lblEquivalents", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        public List<DropDownListModel> Equivalents { get; set; }

        /// <summary>
        /// Gets or sets the degree folio.
        /// </summary>
        /// <value>
        /// The degree folio.
        /// </value>
        [Display(Name = "lblFolio", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        [Required(ErrorMessageResourceType = typeof(Resources.ElectronicDegree.Institution), ErrorMessageResourceName = "lblErrorFolio")]
        public string Folio { get; set; }

        /// <summary>
        /// Gets or sets the folio format.
        /// </summary>
        /// <value>
        /// The folio format.
        /// </value>
        [Display(Name = "lblDegreeFolio", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        [Required(ErrorMessageResourceType = typeof(Resources.ElectronicDegree.Institution), ErrorMessageResourceName = "lblErrorDegreeFolio")]
        public string FolioFormat { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the majors number.
        /// </summary>
        /// <value>
        /// The majors number.
        /// </value>
        public int MajorsNumber { get; set; }

        /// <summary>
        /// Gets or sets the majors view model list.
        /// </summary>
        /// <value>
        /// The majors view model list.
        /// </value>
        public List<MajorsViewModel> MajorsViewModelList { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Display(Name = "lblName", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        [Required(ErrorMessageResourceType = typeof(Resources.ElectronicDegree.Institution), ErrorMessageResourceName = "lblErrorName")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name exist.
        /// </summary>
        /// <value>
        /// The name exists.
        /// </value>
        [Display(Name = "lblNameExists", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        public bool NameExists { get; set; }

        /// <summary>
        /// Gets or sets the name of the revision user.
        /// </summary>
        /// <value>
        /// The name of the revision user.
        /// </value>
        public string RevisionUserName { get; set; }
    }
}