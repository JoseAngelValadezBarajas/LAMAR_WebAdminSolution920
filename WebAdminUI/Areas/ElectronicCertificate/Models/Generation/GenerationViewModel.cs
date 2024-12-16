// --------------------------------------------------------------------
// <copyright file="GenerationViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.ElectronicCertificate;

namespace WebAdminUI.Areas.ElectronicCertificate.Models.Generation
{
    /// <summary>
    /// GenerateViewModel
    /// </summary>
    public class GenerationViewModel
    {
        #region Student

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        [Display(Name = "lblBirthDate", ResourceType = typeof(Generate))]
        public string BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the curp.
        /// </summary>
        /// <value>
        /// The curp.
        /// </value>
        [Display(Name = "lblCurp", ResourceType = typeof(Generate))]
        public string Curp { get; set; }

        /// <summary>
        /// Gets or sets the first surname.
        /// </summary>
        /// <value>
        /// The first surname.
        /// </value>
        [Display(Name = "lblFirstSurname", ResourceType = typeof(Generate))]
        public string FirstSurname { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Display(Name = "lblGenderIdentity", ResourceType = typeof(Generate))]
        public string GenderIdentity { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Display(Name = "lblName", ResourceType = typeof(Generate))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the people identifier.
        /// </summary>
        /// <value>
        /// The people identifier.
        /// </value>
        [Display(Name = "lblPeopleID", ResourceType = typeof(Generate))]
        public string PeopleId { get; set; }

        /// <summary>
        /// Gets or sets the second surname.
        /// </summary>
        /// <value>
        /// The second surname.
        /// </value>
        [Display(Name = "lblSecondSurname", ResourceType = typeof(Generate))]
        public string SecondSurname { get; set; }

        #endregion Student

        #region Studies Programs

        /// <summary>
        /// Gets or sets the studies program detail view model.
        /// </summary>
        /// <value>
        /// The studies program detail view model.
        /// </value>
        public StudiesProgramDetailViewModel StudiesProgramDetailViewModel { get; set; }

        #endregion Studies Programs

        #region Courses

        /// <summary>
        /// Gets or sets the courses view model.
        /// </summary>
        /// <value>
        /// The courses view model.
        /// </value>
        public CoursesViewModel CoursesViewModel { get; set; }

        #endregion Courses

        #region Issuing Certificate

        /// <summary>
        /// Gets or sets the issuing certificate view model.
        /// </summary>
        /// <value>
        /// The issuing certificate view model.
        /// </value>
        public IssuingCertificateViewModel IssuingCertificateViewModel { get; set; }

        #endregion Issuing Certificate

        #region Preview-Generate

        // TODO

        #endregion Preview-Generate
    }
}