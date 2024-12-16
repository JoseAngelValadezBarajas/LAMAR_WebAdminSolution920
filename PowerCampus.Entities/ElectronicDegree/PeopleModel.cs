// --------------------------------------------------------------------
// <copyright file="GenerateModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System;

namespace PowerCampus.Entities.ElectronicDegree
{
    /// <summary>
    /// People
    /// </summary>
    public class PeopleModel
    {
        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the curp.
        /// </summary>
        /// <value>
        /// The curp.
        /// </value>
        public string Curp { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first surname.
        /// </summary>
        /// <value>
        /// The first surname.
        /// </value>
        public string FirstSurname { get; set; }

        /// <summary>
        /// Gets or sets the gender identity.
        /// </summary>
        /// <value>
        /// The gender identity.
        /// </value>
        public string GenderIdentity { get; set; }

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

        /// <summary>
        /// Gets or sets the second surname.
        /// </summary>
        /// <value>
        /// The second surname.
        /// </value>
        public string SecondSurname { get; set; }
    }
}