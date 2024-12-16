// --------------------------------------------------------------------
// <copyright file="ApiRoute.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace WebAdminUI
{
    /// <summary>
    /// ApiRoute
    /// </summary>
    public static class ApiRoute
    {
        #region Electronic Certificate

        #region AcademicPlans Controller

        /// <summary>
        /// The ec academic plan campus
        /// </summary>
        public const string EcAcademicPlanCampus = "/api/EC/AcademicPlans";

        /// <summary>
        /// The ec academic plan courses
        /// </summary>
        public const string EcAcademicPlanCourses = "/api/EC/AcademicPlans/Courses/";

        /// <summary>
        /// The ec academic plan matric term
        /// </summary>
        public const string EcAcademicPlanMatricTerm = "/api/EC/AcademicPlans/MatricTerms/";

        /// <summary>
        /// The ec academic plan matric year
        /// </summary>
        public const string EcAcademicPlanMatricYear = "/api/EC/AcademicPlans/MatricYears/";

        /// <summary>
        /// The ec academic plan programs
        /// </summary>
        public const string EcAcademicPlanPrograms = "/api/EC/AcademicPlans/Programs/";

        /// <summary>
        /// The ec save academic plan courses
        /// </summary>
        public const string EcSaveAcademicPlanCourses = "/api/EC/AcademicPlans/Courses/Save";

        #endregion AcademicPlans Controller

        #region AccountsEC Controller

        /// <summary>
        /// The ec academic plan course by student
        /// </summary>
        public const string EcAcademicPlanCourseByStudent = "/api/EC/AcademicPlanCourseByStudent/";

        /// <summary>
        /// The ec accounts delete
        /// </summary>
        public const string EcAccountDelete = "/api/EC/AccountsEC/Delete";

        /// <summary>
        /// The ec account get account by unique identifier
        /// </summary>
        public const string EcAccountGetAccountByGuid = "/api/EC/AccountsEC/GetAccountByGuid/";

        #endregion AccountsEC Controller

        #region Certificates Controller

        /// <summary>
        /// The ec certificates create
        /// </summary>
        public const string EcCertificates = "/api/EC/Certificates";

        /// <summary>
        /// The ec certificates advanced search
        /// </summary>
        public const string EcCertificatesAdvancedSearch = "/api/EC/Certificates/Search/Advanced";

        /// <summary>
        /// The ec certificates basic search
        /// </summary>
        public const string EcCertificatesBasicSearch = "/api/EC/Certificates/Search/Basic";

        /// <summary>
        /// The ec certificates campuses
        /// </summary>
        public const string EcCertificatesCampuses = "api/EC/Certificates/Campuses/";

        /// <summary>
        /// The ec certificates certification types
        /// </summary>
        public const string EcCertificatesCertificationTypes = "/api/EC/Certificates/CertificationTypes/";

        /// <summary>
        /// The ec certificates delete
        /// </summary>
        public const string EcCertificatesDelete = "/api/EC/Certificates/";

        /// <summary>
        /// The ec certificates detail
        /// </summary>
        public const string EcCertificatesDetail = "/api/EC/Certificates/";

        /// <summary>
        /// The ec certificates files
        /// </summary>
        public const string EcCertificatesFiles = "/api/EC/Certificates/DownloadFiles";

        /// <summary>
        /// The ec certificates majors
        /// </summary>
        public const string EcCertificatesMajors = "/api/EC/Certificates/Majors/";

        /// <summary>
        /// The ec certificates PDF
        /// </summary>
        public const string EcCertificatesPdf = "/api/EC/Certificates/Download/Pdf/";

        /// <summary>
        /// The ec certificates stamp
        /// </summary>
        public const string EcCertificatesStamp = "/api/EC/Certificates/Stamp";

        /// <summary>
        /// The ec certificates statuses
        /// </summary>
        public const string EcCertificatesStatuses = "/api/EC/Certificates/Status";

        /// <summary>
        /// The ec certificates update status
        /// </summary>
        public const string EcCertificatesUpdateStatus = "/api/EC/Certificates/Status";

        /// <summary>
        /// The ec certificates XML
        /// </summary>
        public const string EcCertificatesXml = "/api/EC/Certificates/Download/Xml/";

        #endregion Certificates Controller

        #region FederalEntitiesEC Controller

        /// <summary>
        /// The ec catalog mapping
        /// </summary>
        public const string EcCatalogMapping = "/api/EC/FederalEntities/Mapping";

        /// <summary>
        /// The ec certification types
        /// </summary>
        public const string EcCertificationTypes = "/api/EC/CertificationTypes";

        /// <summary>
        /// The ec federal entities code catalog
        /// </summary>
        public const string EcFederalEntitiesCodeCatalog = "/api/EC/FederalEntities/CodeCatalog";

        /// <summary>
        /// The ec federal entities code catalog without foreign
        /// </summary>
        public const string EcFederalEntitiesCodeCatalogWithoutForeign = "/api/EC/FederalEntities/CodeCatalogWithoutForeign";

        /// <summary>
        /// The ec save federal entity
        /// </summary>
        public const string EcSaveFederalEntity = "/api/EC/FederalEntities/Mapping";

        /// <summary>
        /// The ec states catalog
        /// </summary>
        public const string EcStatesCatalog = "/api/EC/FederalEntities/States";

        #endregion FederalEntitiesEC Controller

        #region Generations Controller

        /// <summary>
        /// The ec courses outside academic plan
        /// </summary>
        public const string EcCoursesOutsideAcademicPlan = "/api/EC/CoursesOutsideAcademicPlan/";

        /// <summary>
        /// The ec create transcript detail certificate
        /// </summary>
        public const string EcCreateTranscriptDetailCertificate = "/api/EC/CreateTranscriptDetailCertificate/";

        /// <summary>
        /// The ec generations folio
        /// </summary>
        public const string EcGenerationsFolio = "/api/EC/Generations/Folio/";

        /// <summary>
        /// The ec period types
        /// </summary>
        public const string EcPeriodTypes = "/api/EC/PeriodTypes";

        /// <summary>
        /// The ec studies program detail
        /// </summary>
        public const string EcStudiesProgramDetail = "/api/EC/StudiesProgramDetail/";

        /// <summary>
        /// The ec institution campuses
        /// </summary>
        public const string EcStudiesPrograms = "/api/EC/StudiesPrograms/";

        /// <summary>
        /// The ec validate operator campus
        /// </summary>
        public const string EcValidateOperatorCampus = "/api/EC/ValidateOperatorCampus/";

        #endregion Generations Controller

        #region InstitutionCampuses Controller

        /// <summary>
        /// The ec position catalog
        /// The ec institution campuses
        /// </summary>
        public const string EcInstitutionCampuses = "/api/EC/InstitutionCampuses";

        /// <summary>
        /// The ec save institution campuses
        /// </summary>
        public const string EcSaveInstitutionCampuses = "/api/EC/InstitutionCampuses/Save";

        #endregion InstitutionCampuses Controller

        #region OperatorsEC Controller

        /// <summary>
        /// The ec delete permissions
        /// </summary>
        public const string EcDeletePermissions = "/api/EC/Operators/Permissions/Delete/";

        /// <summary>
        /// The ec operators create
        /// </summary>
        public const string EcOperatorsCreate = "/api/EC/Operators/Save";

        /// <summary>
        /// The ec edit operator
        /// </summary>
        public const string EcOperatorsDelete = "/api/EC/Operators/Delete/";

        /// <summary>
        /// The ec operators edit
        /// </summary>
        public const string EcOperatorsEdit = "/api/EC/Operators/Edit/";

        /// <summary>
        /// The ec operators institutions
        /// </summary>
        public const string EcOperatorsInstitutions = "/api/EC/Operators/Institutions";

        /// <summary>
        /// The ec operators institutions delete
        /// </summary>
        public const string EcOperatorsInstitutionsDelete = "/api/EC/Operators/Institutions/Delete/";

        /// <summary>
        /// The ec operators operator
        /// </summary>
        public const string EcOperatorsOperator = "/api/EC/Operators/Operator";

        /// <summary>
        /// The ec operators permissions
        /// </summary>
        public const string EcOperatorsPermissions = "/api/EC/Operators/Permissions";

        /// <summary>
        /// The ec permissions edit
        /// </summary>
        public const string ECPermissionsEdit = "/api/EC/Permissions/Edit/";

        /// <summary>
        /// The ec permissions list
        /// </summary>
        public const string EcPermissionsList = "/api/EC/Permissions/List/";

        /// <summary>
        /// The ec search operator
        /// </summary>
        public const string EcSearchOperator = "/api/EC/Operator/";

        /// <summary>
        /// The ec searck operator permissions
        /// </summary>
        public const string EcSearchOperatorPerm = "/api/EC/OperatorPerm/";

        #endregion OperatorsEC Controller

        #region Responsibles Controller

        /// <summary>
        /// The ec responsibles
        /// </summary>
        public const string EcResponsible = "/api/EC/Responsibles/Names";

        /// <summary>
        /// The ec responsibles
        /// </summary>
        public const string EcResponsibles = "/api/EC/Responsibles/";

        /// <summary>
        /// The ec create responsibles
        /// </summary>
        public const string EcResponsiblesCreate = "/api/EC/Responsibles/Create";

        /// <summary>
        /// The ec edit responsibles
        /// </summary>
        public const string EcResponsiblesEdit = "/api/EC/Responsibles/Edit/";

        /// <summary>
        /// The ec responsibles index
        /// </summary>
        public const string EcResponsiblesIndex = "/api/EC/Responsibles/Index";

        /// <summary>
        /// The ec position catalog
        /// </summary>
        public const string EcResponsiblesPostitionCatalog = "/api/EC/Responsibles/PositionCatalog";

        /// <summary>
        /// The ec update resposibles
        /// </summary>
        public const string EcUpdateResposibles = "/api/EC/Responsibles/Update";

        /// <summary>
        /// The ec validate curp
        /// </summary>
        public const string EcValidateCurp = "/api/EC/Responsibles/ValidateCurp/";

        #endregion Responsibles Controller

        #endregion Electronic Certificate

        #region Shared

        /// <summary>
        /// The people
        /// </summary>
        public const string People = "/api/People/";

        #endregion Shared
    }
}