// --------------------------------------------------------------------
// <copyright file="AcademicPlansMapper.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;
using System.Linq;
using WebAdminUI.Areas.ElectronicCertificate.Models;

namespace WebAdminUI.Areas.ElectronicCertificate.Mappers
{
    /// <summary>
    /// AcademicPlansMapper
    /// </summary>
    internal static class AcademicPlansMapper
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="pdcRvoes">The PDC rvoes.</param>
        /// <returns></returns>
        internal static List<ProgramViewModel> ToViewModel(this List<PdcRvoe> pdcRvoes)
        {
            List<ProgramViewModel> institutionCampusViewModels = new List<ProgramViewModel>();
            foreach (PdcRvoe pdcRvoe in pdcRvoes)
            {
                institutionCampusViewModels.Add(new ProgramViewModel
                {
                    CourseMappingPercent = pdcRvoe.CourseMappingPercent,
                    MajorId = pdcRvoe.MajorId,
                    Name = pdcRvoe.PDCName,
                    RVOE = new RvoeViewModel
                    {
                        CourseMappingPercent = pdcRvoe.CourseMappingPercent,
                        Id = pdcRvoe.RvoeId,
                        IssuingDate = pdcRvoe.IssuingDate.Date.ToString("yyyy-MM-dd"),
                        MaximumGrade = pdcRvoe.MaximumGrade,
                        MinimumGrade = pdcRvoe.MinimumGrade,
                        MinimumPassingGrade = pdcRvoe.MinimumPassingGrade,
                        RvoeAgreement = pdcRvoe.RvoeAgreement
                    }
                });
            }

            return institutionCampusViewModels;
        }

        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="pdcRvoe">The PDC rvoe.</param>
        /// <returns></returns>
        internal static ProgramViewModel ToViewModel(this PdcRvoe pdcRvoe)
        {
            ProgramViewModel programViewModel = null;
            if (pdcRvoe != null)
            {
                programViewModel = new ProgramViewModel
                {
                    CampusCodeId = pdcRvoe.CampusCodeId,
                    CampusName = pdcRvoe.CampusName,
                    CourseMappingPercent = pdcRvoe.CourseMappingPercent,
                    Courses = pdcRvoe.Courses != null ? pdcRvoe.Courses.Select(m => new PdcCourseViewModel()
                    {
                        AcademicPlanCourseCatalogId = m.AcademicPlanCourseCatalogId,
                        Classification = m.Classification,
                        ClassificationDesc = m.ClassificationDesc,
                        Credits = m.Credits,
                        CreditsString = m.Credits.ToString(),
                        Discipline = m.Discipline,
                        DisciplineDesc = m.DisciplineDesc,
                        EventId = m.EventId,
                        EventName = m.EventName,
                        EventSubType = m.EventSubType,
                        EventSubTypeDesc = m.EventSubTypeDesc,
                        SepId = m.SepId,
                        SubjectTypeId = m.SubjectTypeId,
                    }).ToList() : null,
                    MajorId = pdcRvoe.MajorId,
                    MatricTerm = pdcRvoe.MatricTerm,
                    MatricTermDesc = pdcRvoe.MatricTermDesc,
                    MatricYear = pdcRvoe.MatricYear,
                    Name = pdcRvoe.PDCName,
                    RVOE = new RvoeViewModel
                    {
                        CourseMappingPercent = pdcRvoe.CourseMappingPercent,
                        Id = pdcRvoe.RvoeId,
                        IssuingDate = pdcRvoe.IssuingDate.Date.ToString("yyyy-MM-dd"),
                        MaximumGrade = pdcRvoe.MaximumGrade,
                        MinimumGrade = pdcRvoe.MinimumGrade,
                        MinimumPassingGrade = pdcRvoe.MinimumPassingGrade,
                        RvoeAgreement = pdcRvoe.RvoeAgreement
                    },
                    Types = pdcRvoe.SubjectType.ToViewModel()
                };
            }

            return programViewModel;
        }
    }
}