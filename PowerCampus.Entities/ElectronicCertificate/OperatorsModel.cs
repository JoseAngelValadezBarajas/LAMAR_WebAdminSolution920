// --------------------------------------------------------------------
// <copyright file="OperatorsModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Collections.Generic;

namespace PowerCampus.Entities.ElectronicCertificate
{
    /// <summary>
    ///InstitutionCampus
    /// </summary>
    public class InstitutionCampusList
    {
        /// <summary>
        /// Gets or sets the institution campuses.
        /// </summary>
        /// <value>
        /// The institution campuses.
        /// </value>
        public List<InstitutionCampusOperator> InstitutionCampusesList { get; set; }

        /// <summary>
        /// Gets or sets the issuing places.
        /// </summary>
        /// <value>
        /// The issuing places.
        /// </value>
        public List<IssuingPlace> IssuingPlacesList { get; set; }
    }

    /// <summary>
    /// InstitutionCampusOperator
    /// </summary>
    public class InstitutionCampusOperator
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
        public string CampusName { get; set; }
    }

    /// <summary>
    /// IssuingPlace
    /// </summary>
    public class IssuingPlace
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
        /// Gets or sets the issuing place identifier.
        /// </summary>
        /// <value>
        /// The issuing place identifier.
        /// </value>
        public int IssuingPlaceId { get; set; }
    }

    /// <summary>
    /// Operators Model
    /// </summary>
    public class OperatorsList
    {
        /// <summary>
        /// Gets or sets the institution campus identifier.
        /// </summary>
        /// <value>
        /// The institution campus identifier.
        /// </value>
        public List<string> CampusCodeId { get; set; }

        /// <summary>
        /// Gets or sets the campus identifier.
        /// </summary>
        /// <value>
        /// The campus identifier.
        /// </value>
        public string CampusId { get; set; }

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
        /// Gets or sets the operators lists.
        /// </summary>
        /// <value>
        /// The operators lists.
        /// </value>
        public List<OperatorsList> OperatorsLists { get; set; }

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
        /// Gets or sets the permissions lists.
        /// </summary>
        /// <value>
        /// The permissions lists.
        /// </value>
        public List<OperatorsList> PermissionsLists { get; set; }

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
        /// Gets or sets the institutions.
        /// </summary>
        /// <value>
        /// The institutions.
        /// </value>
        public int Campuses { get; set; }

        /// <summary>
        /// Gets or sets the granted operator identifier.
        /// </summary>
        /// <value>
        /// The granted operator identifier.
        /// </value>
        public string GrantedOperatorId { get; set; }

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