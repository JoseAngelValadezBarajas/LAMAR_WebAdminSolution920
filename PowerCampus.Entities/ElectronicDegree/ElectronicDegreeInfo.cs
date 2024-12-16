// --------------------------------------------------------------------
// <copyright file="ElectronicDegreeInformation.cs" company="Ellucian">
//     Copyright 2020 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

namespace PowerCampus.Entities.ElectronicDegree
{
    public class ElectronicDegreeInfo
    {
        /// <summary>
        /// Gets or sets the type of the authorization.
        /// </summary>
        /// <value>
        /// The type of the authorization.
        /// </value>
        public string AuthorizationType { get; set; }

        /// <summary>
        /// Gets or sets the authorization type code.
        /// </summary>
        /// <value>
        /// The authorization type code.
        /// </value>
        public int AuthorizationTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the background study end date.
        /// </summary>
        /// <value>
        /// The background study end date.
        /// </value>
        public string BackgroundStudyEndDate { get; set; }

        /// <summary>
        /// Gets or sets the background study start date.
        /// </summary>
        /// <value>
        /// The background study start date.
        /// </value>
        public string BackgroundStudyStartDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the background study.
        /// </summary>
        /// <value>
        /// The type of the background study.
        /// </value>
        public string BackgroundStudyType { get; set; }

        /// <summary>
        /// Gets or sets the background study type code.
        /// </summary>
        /// <value>
        /// The background study type code.
        /// </value>
        public int BackgroundStudyTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the create user.
        /// </summary>
        /// <value>
        /// The name of the create user.
        /// </value>
        public string CreateUserName { get; set; }

        /// <summary>
        /// Gets or sets the curp.
        /// </summary>
        /// <value>
        /// The curp.
        /// </value>
        public string Curp { get; set; }

        /// <summary>
        /// Gets or sets the education level.
        /// </summary>
        /// <value>
        /// The education level.
        /// </value>
        public string EducationLevel { get; set; }

        /// <summary>
        /// Gets or sets the electronic degree information identifier.
        /// </summary>
        /// <value>
        /// The electronic degree information identifier.
        /// </value>
        public int ElectronicDegreeInformationId { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the examination date.
        /// </summary>
        /// <value>
        /// The examination date.
        /// </value>
        public string ExaminationDate { get; set; }

        /// <summary>
        /// Gets or sets the examination exemption date.
        /// </summary>
        /// <value>
        /// The examination exemption date.
        /// </value>
        public string ExaminationExemptionDate { get; set; }

        /// <summary>
        /// Gets or sets the expedition date.
        /// </summary>
        /// <value>
        /// The expedition date.
        /// </value>
        public string ExpeditionDate { get; set; }

        /// <summary>
        /// Gets or sets the federal entity.
        /// </summary>
        /// <value>
        /// The federal entity.
        /// </value>
        public string FederalEntity { get; set; }

        /// <summary>
        /// Gets or sets the federal entity code.
        /// </summary>
        /// <value>
        /// The federal entity code.
        /// </value>
        public string FederalEntityCode { get; set; }

        /// <summary>
        /// Gets or sets the first surname.
        /// </summary>
        /// <value>
        /// The first surname.
        /// </value>
        public string FirstSurname { get; set; }

        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        public string Folio { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [fulfilled social service].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [fulfilled social service]; otherwise, <c>false</c>.
        /// </value>
        public bool FulfilledSocialService { get; set; }

        /// <summary>
        /// Gets or sets the graduation requirement.
        /// </summary>
        /// <value>
        /// The graduation requirement.
        /// </value>
        public string GraduationRequirement { get; set; }

        /// <summary>
        /// Gets or sets the graduation requirement code.
        /// </summary>
        /// <value>
        /// The graduation requirement code.
        /// </value>
        public int GraduationRequirementCode { get; set; }

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
        /// Gets or sets the legal base.
        /// </summary>
        /// <value>
        /// The legal base.
        /// </value>
        public string LegalBase { get; set; }

        /// <summary>
        /// Gets or sets the legal base code.
        /// </summary>
        /// <value>
        /// The legal base code.
        /// </value>
        public int LegalBaseCode { get; set; }

        /// <summary>
        /// Gets or sets the license number.
        /// </summary>
        /// <value>
        /// The license number.
        /// </value>
        public string LicenseNumber { get; set; }

        /// <summary>
        /// Gets or sets the major.
        /// </summary>
        /// <value>
        /// The major.
        /// </value>
        public string Major { get; set; }

        /// <summary>
        /// Gets or sets the major code.
        /// </summary>
        /// <value>
        /// The major code.
        /// </value>
        public string MajorCode { get; set; }

        /// <summary>
        /// Gets or sets the major end date.
        /// </summary>
        /// <value>
        /// The major end date.
        /// </value>
        public string MajorEndDate { get; set; }

        /// <summary>
        /// Gets or sets the major start date.
        /// </summary>
        /// <value>
        /// The major start date.
        /// </value>
        public string MajorStartDate { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the original string.
        /// </summary>
        /// <value>
        /// The original string.
        /// </value>
        public string OriginalString { get; set; }

        /// <summary>
        /// Gets or sets the origin inst federal entity.
        /// </summary>
        /// <value>
        /// The origin inst federal entity.
        /// </value>
        public string OriginInstFederalEntity { get; set; }

        /// <summary>
        /// Gets or sets the origin inst federal entity code.
        /// </summary>
        /// <value>
        /// The origin inst federal entity code.
        /// </value>
        public string OriginInstFederalEntityCode { get; set; }

        /// <summary>
        /// Gets or sets the origin institution.
        /// </summary>
        /// <value>
        /// The origin institution.
        /// </value>
        public string OriginInstitution { get; set; }

        /// <summary>
        /// Gets or sets the people code identifier.
        /// </summary>
        /// <value>
        /// The people code identifier.
        /// </value>
        public string PeopleCodeId { get; set; }

        /// <summary>
        /// Gets or sets the request XML.
        /// </summary>
        /// <value>
        /// The request XML.
        /// </value>
        public string RequestXML { get; set; }


        //public string RequestXML_Stamp { get; set; }
        /// <summary>
        /// Gets or sets the rvoe agreement number.
        /// </summary>
        /// <value>
        /// The rvoe agreement number.
        /// </value>
        public string RvoeAgreementNumber { get; set; }

        /// <summary>
        /// Gets or sets the second surname.
        /// </summary>
        /// <value>
        /// The second surname.
        /// </value>
        public string SecondSurname { get; set; }

        /// <summary>
        /// Gets or sets the signer.
        /// </summary>
        /// <value>
        /// The signer.
        /// </value>
        public List<InstitutionSignerList> Signer { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the student.
        /// </summary>
        /// <value>
        /// The student.
        /// </value>
        public ElectronicDegreeStudent Student { get; set; }
    }

    public class ElectronicDegreeInfoRequest
    {
        public string BatchNumber { get; set; }
        public string CancellationReason { get; set; }
        public System.DateTime CreateDatetime { get; set; }
        public string CreateUserName { get; set; }
        public int ElectronicDegreeInformationId { get; set; }
        public int ElectronicDegreeRequestId { get; set; }
        public string RequestXML { get; set; }
        //public string RequestXML_Stamp { get; set; }
        public string ResponseMessage { get; set; }
        public System.DateTime RevisionDatetime { get; set; }
        public char Status { get; set; }
    }

    public class ElectronicDegreeStudent
    {
        /// <summary>
        /// Gets or sets the curp.
        /// </summary>
        /// <value>
        /// The curp.
        /// </value>
        public string Curp { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }

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