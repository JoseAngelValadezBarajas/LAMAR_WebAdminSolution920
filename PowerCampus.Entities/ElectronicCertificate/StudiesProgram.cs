// --------------------------------------------------------------------
// <copyright file="StudiesProgram.cs" company="Ellucian">
//     Copyright 2020-2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities.ElectronicCertificate
{
    /// <summary>
    /// StudiesProgram
    /// </summary>
    public class StudiesProgram
    {
        /// <summary>
        /// Gets or sets the campus.
        /// </summary>
        /// <value>
        /// The campus.
        /// </value>
        public string Campus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has campus code.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has campus code; otherwise, <c>false</c>.
        /// </value>
        public bool HasCampusCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has courses mapping.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has courses mapping; otherwise, <c>false</c>.
        /// </value>
        public bool HasCoursesMapping { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has institution code.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has institution code; otherwise, <c>false</c>.
        /// </value>
        public bool HasInstitutionCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has operator campus.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has operator campus; otherwise, <c>false</c>.
        /// </value>
        public bool HasOperatorCampus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has responsible campus.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has responsible campus; otherwise, <c>false</c>.
        /// </value>
        public bool HasResponsibleCampus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has rvoe information.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has rvoe information; otherwise, <c>false</c>.
        /// </value>
        public bool HasRvoeInformation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has signing institution.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has signing institution; otherwise, <c>false</c>.
        /// </value>
        public bool HasSigningInstitution { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the program.
        /// </summary>
        /// <value>
        /// The program.
        /// </value>
        public string Program { get; set; }

        /// <summary>
        /// Gets or sets the term.
        /// </summary>
        /// <value>
        /// The term.
        /// </value>
        public string Term { get; set; }
    }
}