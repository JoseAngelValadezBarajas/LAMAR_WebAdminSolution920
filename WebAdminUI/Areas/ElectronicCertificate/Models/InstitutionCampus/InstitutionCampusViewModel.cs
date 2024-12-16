// --------------------------------------------------------------------
// <copyright file="InstitutionCampusViewModel.cs" company="Ellucian">
//     Copyright 2020-2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Areas.ElectronicCertificate.Models.Shared;
using WebAdminUI.Resources.ElectronicCertificate;

namespace WebAdminUI.Areas.ElectronicCertificate.Models.InstitutionCampus
{
    /// <summary>
    /// InstitutionCampusesViewModel
    /// </summary>
    public class InstitutionCampusesViewModel
    {
        /// <summary>
        /// Gets or sets the institution campuses.
        /// </summary>
        /// <value>
        /// The institution campuses.
        /// </value>
        public List<InstitutionCampusViewModel> InstitutionCampuses { get; set; }

        /// <summary>
        /// Gets the issuing place.
        /// </summary>
        /// <value>
        /// The issuing place.
        /// </value>
        public List<DropdownListViewModel> IssuingPlace { get; internal set; }
        

        /// <summary>
        /// Gets or sets the responsibles.
        /// </summary>
        /// <value>
        /// The responsibles.
        /// </value>
        public List<ResponsibleNameViewModel> Responsibles { get; set; }
    }

    /// <summary>
    /// InstitutionCampusViewModel
    /// </summary>
    public class InstitutionCampusViewModel
    {
        /// <summary>
        /// Gets or sets the campus code identifier.
        /// </summary>
        /// <value>
        /// The campus code identifier.
        /// </value>
        public string CampusCodeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the campus.
        /// </summary>
        /// <value>
        /// The name of the campus.
        /// </value>
        [Display(Name = "lblCampus", ResourceType = typeof(Institution))]
        public string CampusName { get; set; }

        /// <summary>
        /// Gets or sets the campus sep code.
        /// </summary>
        /// <value>
        /// The campus sep code.
        /// </value>
        [Display(Name = "lblCampusSepCode", ResourceType = typeof(Institution))]
        public string CampusSepCode { get; set; }

        /// <summary>
        /// Gets the federal entity identifier.
        /// </summary>
        /// <value>
        /// The federal entity identifier.
        /// </value>
        [Display(Name = "lblFederalEntity", ResourceType = typeof(Institution))]
        public int? FederalEntityId { get; set; }

        /// <summary>
        /// Gets or sets the folio format.
        /// </summary>
        /// <value>
        /// The folio format.
        /// </value>
        [Display(Name = "lblFolioFormat", ResourceType = typeof(Institution))]
        public string FolioFormat { get; set; }

        /// <summary>
        /// Gets or sets the institution campus identifier.
        /// </summary>
        /// <value>
        /// The institution campus identifier.
        /// </value>
        public int? InstitutionCampusId { get; set; }

        /// <summary>
        /// Gets or sets the institution code identifier.
        /// </summary>
        /// <value>
        /// The institution code identifier.
        /// </value>
        public string InstitutionCodeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the institution.
        /// </summary>
        /// <value>
        /// The name of the institution.
        /// </value>
        [Display(Name = "lblInstitution", ResourceType = typeof(Institution))]
        public string InstitutionName { get; set; }

        /// <summary>
        /// Gets or sets the sep identifier.
        /// </summary>
        /// <value>
        /// The sep identifier.
        /// </value>
        [Display(Name = "lblInsitutionSepId", ResourceType = typeof(Institution))]
        public string InstitutionSepId { get; set; }

        /// <summary>
        /// Gets or sets the responsible identifier.
        /// </summary>
        /// <value>
        /// The responsible identifier.
        /// </value>
        [Display(Name = "lblResponsible", ResourceType = typeof(Institution))]
        public int? ResponsibleId { get; set; }

        /// <summary>
        /// Gets or sets the signing institution identifier.
        /// </summary>
        /// <value>
        /// The signing institution identifier.
        /// </value>
        [Display(Name = "lblSigningInstitutionId", ResourceType = typeof(Institution))]
        public string SigningInstitutionId { get; set; }
    }

    /// <summary>
    /// ResponsibleNameViewModel
    /// </summary>
    public class ResponsibleNameViewModel
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the code value.
        /// </summary>
        /// <value>
        /// The code value.
        /// </value>
        public int Id { get; set; }
    }
}