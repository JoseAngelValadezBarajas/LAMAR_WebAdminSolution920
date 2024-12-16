// --------------------------------------------------------------------
// <copyright file="MajorModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace PowerCampus.Entities.ElectronicDegree
{
    public class ListOption
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Value { get; set; }
    }

    /// <summary>
    /// Major List
    /// </summary>
    public class MajorList
    {
        /// <summary>
        /// Gets or sets the agreement number.
        /// </summary>
        /// <value>
        /// The agreement number.
        /// </value>
        public string AgreementNumber { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the create date time.
        /// </summary>
        /// <value>
        /// The create date time.
        /// </value>
        public DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// Gets or sets the electronic degree inst major identifier.
        /// </summary>
        /// <value>
        /// The electronic degree inst major identifier.
        /// </value>
        public int? ElectronicDegreeInstMajorId { get; set; }

        /// <summary>
        /// Gets or sets the electronic degree major identifier.
        /// </summary>
        /// <value>
        /// The electronic degree major identifier.
        /// </value>
        public int ElectronicDegreeMajorId { get; set; }

        /// <summary>
        /// Gets or sets the institutions.
        /// </summary>
        /// <value>
        /// The institutions.
        /// </value>
        public int Institutions { get; set; }

        /// <summary>
        /// Gets or sets the legal base identifier.
        /// </summary>
        /// <value>
        /// The legal base identifier.
        /// </value>
        public int LegalBaseId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the revision date time.
        /// </summary>
        /// <value>
        /// The revision date time.
        /// </value>
        public DateTime? RevisionDateTime { get; set; }

        /// <summary>
        /// Gets or sets the rvoe identifier.
        /// </summary>
        /// <value>
        /// The rvoe identifier.
        /// </value>
        public List<RvoeList> RvoeIds { get; set; }

        /// <summary>
        /// Gets or sets the selected.
        /// </summary>
        /// <value>
        /// The selected.
        /// </value>
        public bool Selected { get; set; }

        /// <summary>
        /// Gets or sets the study level.
        /// </summary>
        /// <value>
        /// The study level.
        /// </value>
        public string StudyLevel { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }
    }

    /// <summary>
    /// Major Model
    /// </summary>
    public class MajorModel
    {
        /// <summary>
        /// Gets or sets the cve.
        /// </summary>
        /// <value>
        /// The cve.
        /// </value>
        public int Cve { get; set; }

        /// <summary>
        /// Gets or sets the education level.
        /// </summary>
        /// <value>
        /// The education level.
        /// </value>
        public List<ListOption> EducationLevel { get; set; }

        /// <summary>
        /// Gets or sets the equivalents.
        /// </summary>
        /// <value>
        /// The equivalents.
        /// </value>
        public List<ListOption> Equivalents { get; set; }

        /// <summary>
        /// Gets or sets the name of the major.
        /// </summary>
        /// <value>
        /// The name of the major.
        /// </value>
        public string MajorName { get; set; }
    }

    /// <summary>
    /// RvoeList Model
    /// </summary>
    public class RvoeList
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the rvoe.
        /// </summary>
        /// <value>
        /// The rvoe.
        /// </value>
        public int? Rvoe { get; set; }
    }
}