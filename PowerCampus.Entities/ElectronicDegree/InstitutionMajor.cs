// --------------------------------------------------------------------
// <copyright file="InstitutionMajor.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities.ElectronicDegree
{
    /// <summary>
    /// GenerateModel
    /// </summary>
    public class InstitutionMajor
    {
        /// <summary>
        /// Gets or sets the authorization code.
        /// </summary>
        /// <value>
        /// The authorization code.
        /// </value>
        public int AuthorizationCode { get; set; }

        /// <summary>
        /// Gets or sets the type of the authorization.
        /// </summary>
        /// <value>
        /// The type of the authorization.
        /// </value>
        public string AuthorizationType { get; set; }

        /// <summary>
        /// Gets or sets the electronic degree institution identifier.
        /// </summary>
        /// <value>
        /// The electronic degree institution identifier.
        /// </value>
        public int ElectronicDegreeInstitutionId { get; set; }

        /// <summary>
        /// Gets or sets the electronic degree inst major identifier.
        /// </summary>
        /// <value>
        /// The electronic degree inst major identifier.
        /// </value>
        public int ElectronicDegreeInstMajorId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has pc organization.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has pc organization; otherwise, <c>false</c>.
        /// </value>
        public int HasPCOrganization { get; set; }

        /// <summary>
        /// Gets or sets the institution code.
        /// </summary>
        /// <value>
        /// The institution code.
        /// </value>
        public string InstitutionCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the institution.
        /// </summary>
        /// <value>
        /// The name of the institution.
        /// </value>
        public string InstitutionName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is operator of institution.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is operator of institution; otherwise, <c>false</c>.
        /// </value>
        public int IsOperatorOfInstitution { get; set; }

        /// <summary>
        /// Gets or sets the major code.
        /// </summary>
        /// <value>
        /// The major code.
        /// </value>
        public string MajorCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the major.
        /// </summary>
        /// <value>
        /// The name of the major.
        /// </value>
        public string MajorName { get; set; }

        /// <summary>
        /// Gets or sets the matric year.
        /// </summary>
        /// <value>
        /// The matric year.
        /// </value>
        public string MatricYear { get; set; }

        /// <summary>
        /// Gets or sets the number of signers.
        /// </summary>
        /// <value>
        /// The number of signers.
        /// </value>
        public int NumberOfSigners { get; set; }

        /// <summary>
        /// Gets or sets the program desc.
        /// </summary>
        /// <value>
        /// The program desc.
        /// </value>
        public string ProgramDesc { get; set; }

        /// <summary>
        /// Gets or sets the program end date.
        /// </summary>
        /// <value>
        /// The program end date.
        /// </value>
        public string ProgramEndDate { get; set; }

        /// <summary>
        /// Gets or sets the program start date.
        /// </summary>
        /// <value>
        /// The program start date.
        /// </value>
        public string ProgramStartDate { get; set; }

        /// <summary>
        /// Gets or sets the rvoe agreement number.
        /// </summary>
        /// <value>
        /// The rvoe agreement number.
        /// </value>
        public string RvoeAgreementNumber { get; set; }

        /// <summary>
        /// Gets or sets the study level.
        /// </summary>
        /// <value>
        /// The study level.
        /// </value>
        public string StudyLevel { get; set; }

        /// <summary>
        /// Gets or sets the term desc.
        /// </summary>
        /// <value>
        /// The term desc.
        /// </value>
        public string TermDesc { get; set; }

        /// <summary>
        /// Gets or sets the transcript degree identifier.
        /// </summary>
        /// <value>
        /// The transcript degree identifier.
        /// </value>
        public int TranscriptDegreeId { get; set; }
    }
}