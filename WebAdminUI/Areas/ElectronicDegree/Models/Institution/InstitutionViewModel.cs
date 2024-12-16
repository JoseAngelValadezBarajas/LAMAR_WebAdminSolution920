// --------------------------------------------------------------------
// <copyright file="InstitutionViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAdminUI.Areas.ElectronicDegree.Models.Institution
{
    /// <summary>
    /// Institutions View Model
    /// </summary>
    public class InstitutionsViewModel
    {
        /// <summary>
        /// Gets or sets the institution list.
        /// </summary>
        /// <value>
        /// The institution list.
        /// </value>
        public List<InstitutionsViewModel> InstitutionList { get; set; }
    }

    /// <summary>
    /// Institution View Model
    /// </summary>
    public class InstitutionViewModel
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name = "lblCode", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name = "lblDelete", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        public string Delete { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name = "lblEdit", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        public string Edit { get; set; }

        /// <summary>
        /// Gets or sets the equivalent.
        /// </summary>
        /// <value>
        /// The equivalent.
        /// </value>
        [Display(Name = "lblPCValue", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        public string Equivalent { get; set; }

        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        [Display(Name = "lblFolio", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        public string Folio { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name = "lblMajors", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        public int Majors { get; set; }

        /// <summary>
        /// Gets or sets the majors number.
        /// </summary>
        /// <value>
        /// The majors number.
        /// </value>
        [Display(Name = "lblMajorsNumber", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        public int MajorsNumber { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Display(Name = "lblName", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name = "lblSigners", ResourceType = typeof(Resources.ElectronicDegree.Institution))]
        public int Signers { get; set; }
    }
}