// --------------------------------------------------------------------
// <copyright file="InstitutionModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace PowerCampus.Entities.ElectronicDegree
{
    public class InstitutionDAModel
    {
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
        /// Gets or sets the name of the create user.
        /// </summary>
        /// <value>
        /// The name of the create user.
        /// </value>
        public string CreateUserName { get; set; }

        /// <summary>
        /// Gets or sets the electronic degree institution identifier.
        /// </summary>
        /// <value>
        /// The electronic degree institution identifier.
        /// </value>
        public int ElectronicDegreeInstitutionId { get; set; }

        /// <summary>
        /// Gets or sets the equivalent identifier.
        /// </summary>
        /// <value>The equivalent identifier.</value>
        public string EquivalentId { get; set; }

        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        public string Folio { get; set; }

        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        public string FolioFormat { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

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
        /// Gets or sets the name of the revision user.
        /// </summary>
        /// <value>
        /// The name of the revision user.
        /// </value>
        public string RevisionUserName { get; set; }
    }

    public class InstitutionMajorRvoe
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="InstitutionMajorRvoe"/> is checked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if checked; otherwise, <c>false</c>.
        /// </value>
        public bool Checked { get; set; }

        /// <summary>
        /// Gets or sets the electronic degree inst major identifier.
        /// </summary>
        /// <value>
        /// The electronic degree inst major identifier.
        /// </value>
        public int ElectronicDegreeInstMajorId { get; set; }

        /// <summary>
        /// Gets or sets the rvoe identifier.
        /// </summary>
        /// <value>
        /// The rvoe identifier.
        /// </value>
        public int RvoeId { get; set; }
    }

    public class InstitutionModel
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the equivalent.
        /// </summary>
        /// <value>
        /// The equivalent.
        /// </value>
        public string Equivalent { get; set; }

        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        public string Folio { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the major list.
        /// </summary>
        /// <value>
        /// The major list.
        /// </value>
        public List<MajorList> MajorList { get; set; }

        /// <summary>
        /// Gets or sets the majors.
        /// </summary>
        /// <value>
        /// The majors.
        /// </value>
        public int? Majors { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the signers.
        /// </summary>
        /// <value>
        /// The signers.
        /// </value>
        public int? Signers { get; set; }
    }

    public class MajorMappingModel
    {
        /// <summary>
        /// Gets or sets the institution identifier.
        /// </summary>
        /// <value>
        /// The institution identifier.
        /// </value>
        public int InstitutionId { get; set; }

        /// <summary>
        /// Gets or sets the major delete identifier list.
        /// </summary>
        /// <value>
        /// The major delete identifier list.
        /// </value>
        public List<int> MajorDeleteIdList { get; set; }

        /// <summary>
        /// Gets or sets the major identifier list.
        /// </summary>
        /// <value>
        /// The major identifier list.
        /// </value>
        public List<int> MajorIdList { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }
    }

    public class MapInstitutionMajorRvoe
    {
        /// <summary>
        /// Gets or sets the institution major rvoe.
        /// </summary>
        /// <value>
        /// The institution major rvoe.
        /// </value>
        public List<InstitutionMajorRvoe> institutionMajorRvoe { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }
    }
}