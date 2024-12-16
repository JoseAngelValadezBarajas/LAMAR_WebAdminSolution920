// --------------------------------------------------------------------
// <copyright file="GenerateElectronicDegreeViewModel.cs" company="Ellucian">
//     Copyright 2020 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Resources.ElectronicDegree;

namespace WebAdminUI.Areas.ElectronicDegree.Models
{
    /// <summary>
    /// Electonic Degree Model
    /// </summary>
    public class ElectronicDegreeModel
    {
        /// <summary>
        /// Gets or sets the delete.
        /// </summary>
        /// <value>
        /// The delete.
        /// </value>
        [Display(Name = "lblDelete", ResourceType = typeof(Generate))]
        public string Delete { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [Display(Name = "lblDisplayName", ResourceType = typeof(Generate))]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the education level.
        /// </summary>
        /// <value>
        /// The education level.
        /// </value>
        [Display(Name = "lblEducationLevel", ResourceType = typeof(Generate))]
        public string EducationLevel { get; set; }

        /// <summary>
        /// Gets or sets the electronic degree information identifier.
        /// </summary>
        /// <value>
        /// The electronic degree information identifier.
        /// </value>
        public int ElectronicDegreeInformationId { get; set; }

        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        [Display(Name = "lblFolio", ResourceType = typeof(Generate))]
        public string Folio { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has XML.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has XML; otherwise, <c>false</c>.
        /// </value>
        public bool HasXML { get; internal set; }

        /// <summary>
        /// Gets or sets the major.
        /// </summary>
        /// <value>
        /// The major.
        /// </value>
        [Display(Name = "lblMajor", ResourceType = typeof(Generate))]
        public string Major { get; set; }

        /// <summary>
        /// Gets or sets the major code.
        /// </summary>
        /// <value>
        /// The major code.
        /// </value>
        public string MajorCode { get; set; }

        /// <summary>
        /// Gets or sets the people code identifier.
        /// </summary>
        /// <value>
        /// The people code identifier.
        /// </value>
        public string PeopleCodeId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [Display(Name = "lblStatus", ResourceType = typeof(Generate))]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the view data.
        /// </summary>
        /// <value>
        /// The view data.
        /// </value>
        [Display(Name = "lblViewData", ResourceType = typeof(Generate))]
        public string ViewData { get; set; }
    }

    /// <summary>
    /// Electronic Degree Model List
    /// </summary>
    public class ElectronicDegreeModelList
    {
        /// <summary>
        /// Gets or sets the code cancelation catalog.
        /// </summary>
        /// <value>
        /// The code cancelation catalog.
        /// </value>
        public List<CodeCancelationCatalog> CodeCancelationCatalog { get; set; }

        /// <summary>
        /// Gets or sets the electronic degree parameters.
        /// </summary>
        /// <value>
        /// The electronic degree parameters.
        /// </value>
        public ElectronicDegreeParameters ElectronicDegreeParameters { get; set; }

        /// <summary>
        /// Gets or sets the electronic degrees model.
        /// </summary>
        /// <value>
        /// The electronic degrees model.
        /// </value>
        public List<ElectronicDegreeModel> ElectronicDegreesModel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is sent available.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is sent available; otherwise, <c>false</c>.
        /// </value>
        public bool IsSentAvailable { get; set; }
    }

    /// <summary>
    /// Electronic Degree Parameters
    /// </summary>
    public class ElectronicDegreeParameters
    {
        /// <summary>
        /// Gets or sets the type of the education.
        /// </summary>
        /// <value>
        /// The type of the education.
        /// </value>
        [Display(Name = "lblEducationLevel", ResourceType = typeof(Generate))]
        public List<ListOption> EducationType { get; set; }

        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        [Display(Name = "lblFolio", ResourceType = typeof(Generate))]
        public string Folio { get; set; }

        /// <summary>
        /// Gets or sets the major.
        /// </summary>
        /// <value>
        /// The major.
        /// </value>
        [Display(Name = "lblMajor", ResourceType = typeof(Generate))]
        public List<ListOption> Major { get; set; }

        /// <summary>
        /// Gets or sets the student.
        /// </summary>
        /// <value>
        /// The student.
        /// </value>
        [Display(Name = "lblStudent", ResourceType = typeof(Generate))]
        public string Student { get; set; }
    }

    /// <summary>
    /// Electronic Degree Student Model
    /// </summary>
    public class ElectronicDegreeStudentModel
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [Display(Name = "lblStudet", ResourceType = typeof(Generate))]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the people code identifier.
        /// </summary>
        /// <value>
        /// The people code identifier.
        /// </value>
        public string PeopleCodeId { get; set; }
    }

    /// <summary>
    /// Generate Electronic Degree View Model
    /// </summary>
    public class GenerateViewModel
    {
        /// <summary>
        /// Gets or sets the type of the authorization.
        /// </summary>
        /// <value>
        /// The type of the authorization.
        /// </value>
        [Display(Name = "lblAuthorizationType", ResourceType = typeof(Generate))]
        public string AuthorizationType { get; set; }

        /// <summary>
        /// Gets or sets the authorization type catalog.
        /// </summary>
        /// <value>
        /// The authorization type catalog.
        /// </value>
        [Display(Name = "lblAuthorizationTypeCatalog", ResourceType = typeof(Generate))]
        public string AuthorizationTypeCatalog { get; set; }

        /// <summary>
        /// Gets or sets the authorization type error.
        /// </summary>
        /// <value>
        /// The authorization type error.
        /// </value>
        [Display(Name = "lblErrorAuthorizationType", ResourceType = typeof(Generate))]
        public string AuthorizationTypeError { get; set; }

        /// <summary>
        /// Gets or sets the completed social service.
        /// </summary>
        /// <value>
        /// The completed social service.
        /// </value>
        public string CompletedSocialService { get; set; }

        /// <summary>
        /// Gets or sets the curp.
        /// </summary>
        /// <value>
        /// The curp.
        /// </value>
        [Display(Name = "lblCurp", ResourceType = typeof(Generate))]
        public string Curp { get; set; }

        /// <summary>
        /// Gets or sets the curp error.
        /// </summary>
        /// <value>
        /// The curp error.
        /// </value>
        [Display(Name = "lblErrorCurp", ResourceType = typeof(Generate))]
        public string CurpError { get; set; }

        /// <summary>
        /// Gets or sets the degree folio.
        /// </summary>
        /// <value>
        /// The degree folio.
        /// </value>
        [Display(Name = "lblDegreeFolio", ResourceType = typeof(Generate))]
        public string DegreeFolio { get; set; }

        /// <summary>
        /// Gets or sets the degree folio error.
        /// </summary>
        /// <value>
        /// The degree folio error.
        /// </value>
        [Display(Name = "lblErrorDegreeFolio", ResourceType = typeof(Generate))]
        public string DegreeFolioError { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Display(Name = "lblEmail", ResourceType = typeof(Generate))]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the email error.
        /// </summary>
        /// <value>
        /// The email error.
        /// </value>
        [Display(Name = "lblErrorEmail", ResourceType = typeof(Generate))]
        public string EmailError { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [Display(Name = "lblEndDate", ResourceType = typeof(Generate))]
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the end date error.
        /// </summary>
        /// <value>
        /// The end date error.
        /// </value>
        [Display(Name = "lblErrorEndDate", ResourceType = typeof(Generate))]
        public string EndDateError { get; set; }

        /// <summary>
        /// Gets or sets the exemption professional examination date.
        /// </summary>
        /// <value>
        /// The exemption professional examination date.
        /// </value>
        [Display(Name = "lblExemptionProfessionalExamDate", ResourceType = typeof(Generate))]
        public string ExemptionProfessionalExaminationDate { get; set; }

        /// <summary>
        /// Gets or sets the first signer.
        /// </summary>
        /// <value>
        /// The first signer.
        /// </value>
        [Display(Name = "lblFirstSigner", ResourceType = typeof(Generate))]
        public string FirstSigner { get; set; }

        /// <summary>
        /// Gets or sets the first surname.
        /// </summary>
        /// <value>
        /// The first surname.
        /// </value>
        [Display(Name = "lblFirstSurname", ResourceType = typeof(Generate))]
        public string FirstSurname { get; set; }

        /// <summary>
        /// Gets or sets the first surname error.
        /// </summary>
        /// <value>
        /// The first surname error.
        /// </value>
        [Display(Name = "lblErrorFirstSurname", ResourceType = typeof(Generate))]
        public string FirstSurnameError { get; set; }

        /// <summary>
        /// Gets or sets the graduation req type catalog error.
        /// </summary>
        /// <value>
        /// The graduation req type catalog error.
        /// </value>
        [Display(Name = "lblErrorGraduationTypeCatalog", ResourceType = typeof(Generate))]
        public string GraduationReqTypeCatalogError { get; set; }

        /// <summary>
        /// Gets or sets the type of the graduation requirement.
        /// </summary>
        /// <value>
        /// The type of the graduation requirement.
        /// </value>
        [Display(Name = "lblGraduationReqType", ResourceType = typeof(Generate))]
        public string GraduationRequirementType { get; set; }

        /// <summary>
        /// Gets or sets the graduation requirement type catalog.
        /// </summary>
        /// <value>
        /// The graduation requirement type catalog.
        /// </value>
        [Display(Name = "lblGraduationReqTypeCatalog", ResourceType = typeof(Generate))]
        public List<GraduationRequirementCatalog> GraduationRequirementTypeCatalog { get; set; }

        /// <summary>
        /// Gets or sets the institution major.
        /// </summary>
        /// <value>
        /// The institution major.
        /// </value>
        public List<string> InstitutionMajor { get; set; }

        /// <summary>
        /// Gets or sets the institution origin.
        /// </summary>
        /// <value>
        /// The institution origin.
        /// </value>
        [Display(Name = "lblInstitutionOrigin", ResourceType = typeof(Generate))]
        public string InstitutionOrigin { get; set; }

        /// <summary>
        /// Gets or sets the institution origin error.
        /// </summary>
        /// <value>
        /// The institution origin error.
        /// </value>
        [Display(Name = "lblInstitutionOriginError", ResourceType = typeof(Generate))]
        public string InstitutionOriginError { get; set; }

        /// <summary>
        /// Gets or sets the institutions code.
        /// </summary>
        /// <value>
        /// The institutions code.
        /// </value>
        [Display(Name = "lblInstitutionsCode", ResourceType = typeof(Generate))]
        public string InstitutionsCode { get; set; }

        /// <summary>
        /// Gets or sets the institutions code error.
        /// </summary>
        /// <value>
        /// The institutions code error.
        /// </value>
        [Display(Name = "lblErrorInstitutionCode", ResourceType = typeof(Generate))]
        public string InstitutionsCodeError { get; set; }

        /// <summary>
        /// Gets or sets the name of the institutions.
        /// </summary>
        /// <value>
        /// The name of the institutions.
        /// </value>
        [Display(Name = "lblInstitutionsName", ResourceType = typeof(Generate))]
        public string InstitutionsName { get; set; }

        /// <summary>
        /// Gets or sets the name of the institutions.
        /// </summary>
        /// <value>
        /// The name of the institutions.
        /// </value>
        [Display(Name = "lblErrorInstitutionName", ResourceType = typeof(Generate))]
        public string InstitutionsNameError { get; set; }

        /// <summary>
        /// Gets or sets the issuing date.
        /// </summary>
        /// <value>
        /// The issuing date.
        /// </value>
        [Display(Name = "lblIssuingDegree", ResourceType = typeof(Generate))]
        public string IssuingDate { get; set; }

        /// <summary>
        /// Gets or sets the issuing date error.
        /// </summary>
        /// <value>
        /// The issuing date error.
        /// </value>
        [Display(Name = "lblErrorIssuingDate", ResourceType = typeof(Generate))]
        public string IssuingDateError { get; set; }

        /// <summary>
        /// Gets or sets the licence number.
        /// </summary>
        /// <value>
        /// The licence number.
        /// </value>
        [Display(Name = "lblLicenceNumber", ResourceType = typeof(Generate))]
        public string LicenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the licence number error.
        /// </summary>
        /// <value>
        /// The licence number error.
        /// </value>
        [Display(Name = "lblErrorLicenceNumber", ResourceType = typeof(Generate))]
        public string LicenceNumberError { get; set; }

        /// <summary>
        /// Gets or sets the major identifier.
        /// </summary>
        /// <value>
        /// The major identifier.
        /// </value>
        [Display(Name = "lblMajorID", ResourceType = typeof(Generate))]
        public int MajorId { get; set; }

        /// <summary>
        /// Gets or sets the major identifier error.
        /// </summary>
        /// <value>
        /// The major identifier error.
        /// </value>
        [Display(Name = "lblErrorMajorId", ResourceType = typeof(Generate))]
        public string MajorIdError { get; set; }

        /// <summary>
        /// Gets or sets the name of the major.
        /// </summary>
        /// <value>
        /// The name of the major.
        /// </value>
        [Display(Name = "lblMajorName", ResourceType = typeof(Generate))]
        public string MajorName { get; set; }

        /// <summary>
        /// Gets or sets the major name error.
        /// </summary>
        /// <value>
        /// The major name error.
        /// </value>
        [Display(Name = "lblErrorMajorName", ResourceType = typeof(Generate))]
        public string MajorNameError { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Display(Name = "lblName", ResourceType = typeof(Generate))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Display(Name = "lblErrorName", ResourceType = typeof(Generate))]
        public string NameError { get; set; }

        /// <summary>
        /// Gets or sets the people identifier.
        /// </summary>
        /// <value>
        /// The people identifier.
        /// </value>
        [Display(Name = "lblPeopleID", ResourceType = typeof(Generate))]
        public string PeopleId { get; set; }

        /// <summary>
        /// Gets or sets the professional examination date.
        /// </summary>
        /// <value>
        /// The professional examination date.
        /// </value>
        [Display(Name = "lblProfessionalExaminationDate", ResourceType = typeof(Generate))]
        public string ProfessionalExaminationDate { get; set; }

        /// <summary>
        /// Gets or sets the rvoe.
        /// </summary>
        /// <value>
        /// The rvoe.
        /// </value>
        [Display(Name = "lblRvoe", ResourceType = typeof(Generate))]
        public string Rvoe { get; set; }

        /// <summary>
        /// Gets or sets the second signer.
        /// </summary>
        /// <value>
        /// The second signer.
        /// </value>
        [Display(Name = "lblSecondSigner", ResourceType = typeof(Generate))]
        public string SecondSigner { get; set; }

        /// <summary>
        /// Gets or sets the second surname.
        /// </summary>
        /// <value>
        /// The second surname.
        /// </value>
        [Display(Name = "lblSecondSurname", ResourceType = typeof(Generate))]
        public string SecondSurname { get; set; }

        /// <summary>
        /// Gets or sets the second surname error.
        /// </summary>
        /// <value>
        /// The second surname error.
        /// </value>
        [Display(Name = "lblErrorSecondSurname", ResourceType = typeof(Generate))]
        public string SecondSurnameError { get; set; }

        /// <summary>
        /// Gets or sets the social service.
        /// </summary>
        /// <value>
        /// The social service.
        /// </value>
        [Display(Name = "lblSocialService", ResourceType = typeof(Generate))]
        public string SocialService { get; set; }

        /// <summary>
        /// Gets or sets the social services error.
        /// </summary>
        /// <value>
        /// The social services error.
        /// </value>
        [Display(Name = "lblErrorSocialService", ResourceType = typeof(Generate))]
        public string SocialServiceError { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [Display(Name = "lblStartDate", ResourceType = typeof(Generate))]
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the start date error.
        /// </summary>
        /// <value>
        /// The start date error.
        /// </value>
        [Display(Name = "lblErrorStartDate", ResourceType = typeof(Generate))]
        public string StartDateError { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        [Display(Name = "lblState", ResourceType = typeof(Generate))]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the state catalog list.
        /// </summary>
        /// <value>
        /// The state catalog list.
        /// </value>
        [Display(Name = "lblStateCatalog", ResourceType = typeof(Generate))]
        public List<string> StateCatalog { get; set; }

        /// <summary>
        /// Gets or sets the state catalog error.
        /// </summary>
        /// <value>
        /// The state catalog error.
        /// </value>
        [Display(Name = "lblErrorStateCatalog", ResourceType = typeof(Generate))]
        public string StateCatalogError { get; set; }

        /// <summary>
        /// Gets or sets the state error.
        /// </summary>
        /// <value>
        /// The state error.
        /// </value>
        [Display(Name = "lblErrorState", ResourceType = typeof(Generate))]
        public string StateError { get; set; }

        /// <summary>
        /// Gets or sets the third signer.
        /// </summary>
        /// <value>
        /// The third signer.
        /// </value>
        [Display(Name = "lblThirdSigner", ResourceType = typeof(Generate))]
        public string ThirdSigner { get; set; }

        /// <summary>
        /// Gets or sets the type background study.
        /// </summary>
        /// <value>
        /// The type background study.
        /// </value>
        [Display(Name = "lblTypeOfBackground", ResourceType = typeof(Generate))]
        public string TypeBackgroundStudy { get; set; }

        /// <summary>
        /// Gets or sets the type background study error.
        /// </summary>
        /// <value>
        /// The type background study error.
        /// </value>
        [Display(Name = "lblTypeBackgroundStudyError", ResourceType = typeof(Generate))]
        public string TypeBackgroundStudyError { get; set; }

        //mod addv 13082024
        [Display(Name = "LblLevelStudy", ResourceType = typeof(Generate))]
        public string LevelStudy { get; set; }

    }
}