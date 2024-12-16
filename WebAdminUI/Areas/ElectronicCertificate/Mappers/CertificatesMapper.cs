// --------------------------------------------------------------------
// <copyright file="CertificatesMapper.cs" company="Ellucian">
//     Copyright 2020-2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;
using WebAdminUI.Areas.ElectronicCertificate.Models.Generation;

namespace WebAdminUI.Areas.ElectronicCertificate.Mappers
{
    /// <summary>
    /// AcademicPlansMapper
    /// </summary>
    internal static class CertificatesMapper
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="certificates">The PDC rvoes.</param>
        /// <param name="isGenerations">if set to <c>true</c> [is generations].</param>
        /// <returns></returns>
        internal static List<ElectronicCertificateViewModel> ToViewModel(this List<Certificate> certificates, bool isGenerations)
        {
            List<ElectronicCertificateViewModel> electronicCertificateViewModels = new List<ElectronicCertificateViewModel>();
            foreach (Certificate certificate in certificates)
            {
                electronicCertificateViewModels.Add(new ElectronicCertificateViewModel
                {
                    Campus = certificate.Campus,
                    CertificationType = certificate.CertificationType,
                    ElectronicCertificateFileId = certificate.ElectronicCertificateFileId,
                    Folio = certificate.Folio,
                    HasPdfFile = !isGenerations && certificate.HasPdfFile,
                    HasXmlFile = !isGenerations && certificate.HasXmlFile,
                    Id = certificate.Id,
                    IssuingDate = certificate.IssuingDate.Date.ToString("yyyy-MM-dd"),
                    Notes = isGenerations ? string.Empty : certificate.Notes,
                    PaymentFolio = isGenerations ? string.Empty : certificate.PaymentFolio,
                    PdfSize = (certificate.PdfSize / 1024.0m).ToString("0.00"),
                    PeopleCodeId = certificate.PeopleCodeId,
                    Program = certificate.Program,
                    Status = certificate.Status,
                    Student = certificate.StudentName,
                    XmlSize = (certificate.XmlSize / 2.0m / 1024.0m).ToString("0.00")
                });
            }
            return electronicCertificateViewModels;
        }

        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="certificateInfo">The certificate information.</param>
        /// <returns></returns>
        internal static ElectronicCertificateInfoViewModel ToViewModel(this CertificateInfo certificateInfo)
        {
            ElectronicCertificateInfoViewModel electronicCertificateInfoViewModel = new ElectronicCertificateInfoViewModel();
            if (certificateInfo != null)
            {
                electronicCertificateInfoViewModel = new ElectronicCertificateInfoViewModel
                {
                    BirthDate = certificateInfo.BirthDate != null ? certificateInfo.BirthDate?.Date.ToString("yyyy-MM-dd") : string.Empty,
                    CampusName = certificateInfo.CampusName,
                    CampusSepCode = certificateInfo.CampusSepCode,
                    CertificationType = certificateInfo.CertificationType,
                    CertificationTypeId = certificateInfo.CertificationTypeId,
                    CourseAssigned = certificateInfo.CourseAssigned.ToString(),
                    CourseAverage = string.Format("{0:0.00}", certificateInfo.Average),
                    Courses = certificateInfo.Courses.ToViewModel(),
                    CreditEarned = string.Format("{0:0.00}", certificateInfo.CreditEarned),
                    Curp = certificateInfo.Curp,
                    ExpeditionDate = certificateInfo.ExpeditionDate != null ? certificateInfo.ExpeditionDate?.Date.ToString("yyyy-MM-dd") : string.Empty,
                    FederalEntity = certificateInfo.FederalEntity,
                    FederalEntityCode = certificateInfo.FederalEntityCode,
                    FirstSurname = certificateInfo.FirstSurname,
                    Folio = certificateInfo.Folio,
                    Gender = certificateInfo.Gender,
                    GPA = certificateInfo.GPA.ToString(),
                    Id = certificateInfo.Id,
                    InstitutionName = certificateInfo.InstitutionName,
                    InstitutionSepId = certificateInfo.InstitutionSepId,
                    IssuingDate = certificateInfo.IssuingDate != null ? certificateInfo.IssuingDate?.Date.ToString("yyyy-MM-dd") : string.Empty,
                    IssuingFederalEntity = certificateInfo.IssuingFederalEntity,
                    IssuingFederalEntityCode = certificateInfo.IssuingFederalEntityCode,
                    MajorId = certificateInfo.MajorId.ToString(),
                    MajorName = certificateInfo.MajorName,
                    MaximumGrade = certificateInfo.MaximumGrade.ToString(),
                    MinimumGrade = certificateInfo.MinimumGrade.ToString(),
                    MinimumPassingGrade = string.Format("{0:0.00}", certificateInfo.MinimumPassingGrade),
                    Name = certificateInfo.Name,
                    PeopleId = certificateInfo.PeopleId,
                    PeriodType = certificateInfo.PeriodType,
                    PeriodTypeId = certificateInfo.PeriodTypeId,
                    PlanCode = certificateInfo.PlanCode,
                    ResponsibleCurp = certificateInfo.ResponsibleCurp,
                    ResponsibleFirstSurname = certificateInfo.ResponsibleFirstSurname,
                    ResponsibleJobTitle = certificateInfo.ResponsibleJobTitle,
                    ResponsibleJobTitleId = certificateInfo.ResponsibleJobTitleId,
                    ResponsibleName = certificateInfo.ResponsibleName,
                    ResponsibleSecondSurname = certificateInfo.ResponsibleSecondSurname,
                    RvoeAgreementNumber = certificateInfo.RvoeAgreementNumber,
                    SecondSurname = certificateInfo.SecondSurname,
                    StudyLevel = certificateInfo.StudyLevel,
                    StudyLevelId = certificateInfo.StudyLevelId,
                    TotalCourse = certificateInfo.TotalCourse.ToString(),
                    TotalCredit = string.Format("{0:0.00}", certificateInfo.TotalCredit),
                    SigningInstitutionId = certificateInfo.SigningInstitutionId,
                    TotalCycle = certificateInfo.TotalCycle
                };
            }
            return electronicCertificateInfoViewModel;
        }

        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="coursesDto">The courses dto.</param>
        /// <returns></returns>
        internal static List<ElectronicCertificateCourseInfoViewModel> ToViewModel(this List<CertificateCourseInfo> coursesDto)
        {
            List<ElectronicCertificateCourseInfoViewModel> courses = new List<ElectronicCertificateCourseInfoViewModel>();
            foreach (CertificateCourseInfo certificateCourseInfo in coursesDto)
            {
                courses.Add(new ElectronicCertificateCourseInfoViewModel
                {
                    Credits = certificateCourseInfo.Credits.ToString(),
                    Cycle = certificateCourseInfo.EventCycle,
                    Grade = certificateCourseInfo.FinalGrade,
                    Id = certificateCourseInfo.Id,
                    SepId = certificateCourseInfo.SepId,
                    Name = certificateCourseInfo.EventName,
                    Observations = certificateCourseInfo.GradeRemark,
                    Type = certificateCourseInfo.SubjectType
                });
            }
            return courses;
        }
    }
}