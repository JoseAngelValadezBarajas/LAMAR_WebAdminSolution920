// --------------------------------------------------------------------
// <copyright file="EditInsMappingViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;
using WebAdminUI.Areas.ElectronicDegree.Models.Majors;

namespace WebAdminUI.Areas.ElectronicDegree.Models.Institution
{
    /// <summary>
    /// EditInsMappingViewModel
    /// </summary>
    public class EditInsMappingViewModel
    {
        /// <summary>
        /// Gets or sets the majors view model list.
        /// </summary>
        /// <value>
        /// The majors view model list.
        /// </value>
        public List<AddInstitutionViewModel> AddInstitutionViewModelList { get; set; }

        /// <summary>
        /// Gets or sets the institution code.
        /// </summary>
        /// <value>
        /// The institution code.
        /// </value>
        public string InstitutionCode { get; set; }

        /// <summary>
        /// Gets or sets the institution view model list.
        /// </summary>
        /// <value>
        /// The institution view model list.
        /// </value>
        public AddInstitutionViewModel InstitutionViewModelList { get; set; }

        /// <summary>
        /// Gets or sets the name of the institutution.
        /// </summary>
        /// <value>
        /// The name of the institutution.
        /// </value>
        public string InstitututionName { get; set; }

        /// <summary>
        /// Gets or sets the majors.
        /// </summary>
        /// <value>
        /// The majors.
        /// </value>
        public int? Majors { get; set; }

        /// <summary>
        /// Gets or sets the majors view model list.
        /// </summary>
        /// <value>
        /// The majors view model list.
        /// </value>
        public List<MajorsViewModel> MajorsViewModelList { get; set; }

        /// <summary>
        /// Gets or sets the selected institutution identifier.
        /// </summary>
        /// <value>
        /// The selected institutution identifier.
        /// </value>
        public int SelectedInstitututionId { get; set; }
    }

    /// <summary>
    /// EditMajorInsMappingViewModel
    /// </summary>
    public class EditMajorInsMappingViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the institution code.
        /// </summary>
        /// <value>
        /// The institution code.
        /// </value>
        public string InstitutionCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the institutution.
        /// </summary>
        /// <value>
        /// The name of the institutution.
        /// </value>
        public string InstitututionName { get; set; }

        /// <summary>
        /// Gets or sets the majors.
        /// </summary>
        /// <value>
        /// The majors.
        /// </value>
        public int? Majors { get; set; }

        /// <summary>
        /// Gets or sets the majors view model list.
        /// </summary>
        /// <value>
        /// The majors view model list.
        /// </value>
        public List<MajorsViewModel> MajorsViewModelList { get; set; }
    }
}