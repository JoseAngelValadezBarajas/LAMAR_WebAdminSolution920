// --------------------------------------------------------------------
// <copyright file="PeopleViewModel.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Helpers;
using WebAdminUI.Models.FiscalRecords;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.AcademicInfoViewModel
{
    public class AcademicInfoViewModelCertificate
    {
        /// <summary>
        /// Gets or sets the RowType list.
        /// </summary>
        /// <value>
        /// The RowTyoe list.
        /// </value>
        public string RowType { get; set; }

        /// <summary>
        /// Gets or sets the Year list.
        /// </summary>
        /// <value>
        /// The Year list.
        /// </value>
        public string ACADEMIC_YEAR { get; set; }

        /// <summary>
        /// Gets or sets the AcademicTerm list.
        /// </summary>
        /// <value>
        /// The cfdi list.
        /// </value>
        public string ACADEMIC_TERM { get; set; }

        /// <summary>
        /// Gets or sets the AcademicSession list.
        /// </summary>
        /// <value>
        /// The cfdi list.
        /// </value>
        public string ACADEMIC_SESSION { get; set; }

        /// <summary>
        /// Gets or sets the Date list.
        /// </summary>
        /// <value>
        /// The Date list.
        /// </value>
        public DateTime Date { get; set; }
    }
}



