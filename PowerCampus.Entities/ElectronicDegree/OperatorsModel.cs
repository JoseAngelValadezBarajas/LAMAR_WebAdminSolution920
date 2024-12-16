// --------------------------------------------------------------------
// <copyright file="OperatorsModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Collections.Generic;

namespace PowerCampus.Entities.ElectronicDegree
{
    /// <summary>
    /// InstitutionOptions
    /// </summary>
    public class InstitutionOptions
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the institution identifier.
        /// </summary>
        /// <value>
        /// The institution identifier.
        /// </value>
        public int InstitutionId { get; set; }
    }

    /// <summary>
    /// Operators Model
    /// </summary>
    public class OperatorsList
    {
        /// <summary>
        /// Gets or sets the electronic degree institution.
        /// </summary>
        /// <value>
        /// The electronic degree institution.
        /// </value>
        public int ElectronicDegreeInstitutionId { get; set; }

        /// <summary>
        /// Gets or sets the electronic degree operator identifier.
        /// </summary>
        /// <value>
        /// The electronic degree operator identifier.
        /// </value>
        public int ElectronicDegreeOperatorId { get; set; }

        /// <summary>
        /// Gets or sets the granted operator identifier.
        /// </summary>
        /// <value>
        /// The granted operator identifier.
        /// </value>
        public List<string> GrantedOperatorId { get; set; }

        /// <summary>
        /// Gets or sets the electronic degree granted operator identifier.
        /// </summary>
        /// <value>
        /// The electronic degree granted operator identifier.
        /// </value>
        public string GrantedOperatorsId { get; set; }

        /// <summary>
        /// Gets or sets the institution identifier.
        /// </summary>
        /// <value>
        /// The institution identifier.
        /// </value>
        public List<int> InstitutionId { get; set; }

        /// <summary>
        /// Gets or sets the institutions.
        /// </summary>
        /// <value>
        /// The institutions.
        /// </value>
        public int Institutions { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public string OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the people code identifier.
        /// </summary>
        /// <value>
        /// The people code identifier.
        /// </value>
        public string PeopleCodeId { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public int Permissions { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }
    }

    /// <summary>
    /// Permissions Model
    /// </summary>
    public class PermissionsList
    {
        /// <summary>
        /// Gets or sets the granted operator identifier.
        /// </summary>
        /// <value>
        /// The granted operator identifier.
        /// </value>
        public string GrantedOperatorId { get; set; }

        /// <summary>
        /// Gets or sets the institutions.
        /// </summary>
        /// <value>
        /// The institutions.
        /// </value>
        public int Institutions { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the people code identifier.
        /// </summary>
        /// <value>
        /// The people code identifier.
        /// </value>
        public string PeopleCodeId { get; set; }
    }
}