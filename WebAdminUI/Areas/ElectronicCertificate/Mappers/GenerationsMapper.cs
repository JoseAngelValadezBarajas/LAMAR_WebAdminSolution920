// --------------------------------------------------------------------
// <copyright file="GenerationsMapper.cs" company="Ellucian">
//     Copyright 2020-2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicCertificate;
using System;
using System.Collections.Generic;
using WebAdminUI.Areas.ElectronicCertificate.Models.Generation;

namespace WebAdminUI.Areas.ElectronicCertificate.Mappers
{
    /// <summary>
    /// GenerationsMapper
    /// </summary>
    internal static class GenerationsMapper
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="studiesPrograms">The studies programs.</param>
        /// <returns></returns>
        internal static List<StudiesProgramViewModel> ToViewModel(this List<StudiesProgram> studiesPrograms)
        {
            List<StudiesProgramViewModel> studiesProgramsViewModel = new List<StudiesProgramViewModel>();
            if (studiesPrograms != null)
            {
                foreach (StudiesProgram studiesProgram in studiesPrograms)
                {
                    studiesProgramsViewModel.Add(new StudiesProgramViewModel
                    {
                        Campus = studiesProgram.Campus,
                        HasCampusCode = studiesProgram.HasCampusCode,
                        HasCoursesMapping = studiesProgram.HasCoursesMapping,
                        HasInstitutionCode = studiesProgram.HasInstitutionCode,
                        HasOperatorCampus = studiesProgram.HasOperatorCampus,
                        HasResponsibleCampus = studiesProgram.HasResponsibleCampus,
                        HasRvoeInformation = studiesProgram.HasRvoeInformation,
                        HasSigningInstitution = studiesProgram.HasSigningInstitution,
                        Id = studiesProgram.Id,
                        Program = studiesProgram.Program,
                        Term = studiesProgram.Term
                    });
                }
            }
            return studiesProgramsViewModel;
        }

        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="studiesProgramDetail">The studies program detail.</param>
        /// <returns></returns>
        internal static StudiesProgramDetailViewModel ToViewModel(this StudiesProgramDetail studiesProgramDetail)
        {
            StudiesProgramDetailViewModel studiesProgramDetailViewModel = new StudiesProgramDetailViewModel();
            if (studiesProgramDetail != null)
            {
                studiesProgramDetailViewModel.CampusName = studiesProgramDetail.CampusName;
                studiesProgramDetailViewModel.SigningInstitutionId = studiesProgramDetail.SigningInstitutionId;
                studiesProgramDetailViewModel.CampusSepId = studiesProgramDetail.CampusSepId;
                studiesProgramDetailViewModel.FederalEntityCatalogMapping = studiesProgramDetail.FederalEntityCatalogMapping;
                studiesProgramDetailViewModel.FederalEntityCatalogMappingCode = studiesProgramDetail.FederalEntityCatalogMappingCode;
                studiesProgramDetailViewModel.FederalEntityCatalogMappingShortDesc = studiesProgramDetail.FederalEntityCatalogMappingShortDesc;
                studiesProgramDetailViewModel.Id = studiesProgramDetail.Id;
                studiesProgramDetailViewModel.InstitutionCampusId = studiesProgramDetail.InstitutionCampusId;
                studiesProgramDetailViewModel.InstitutionName = studiesProgramDetail.InstitutionName;
                studiesProgramDetailViewModel.InstitutionSepId = studiesProgramDetail.InstitutionSepId;
                studiesProgramDetailViewModel.StudiesProgramMajor = studiesProgramDetail.StudiesProgramMajor != null ?
                new StudiesProgramMajorViewModel
                {
                    Code = studiesProgramDetail.StudiesProgramMajor.Code,
                    Id = studiesProgramDetail.StudiesProgramMajor.Id?.ToString(),
                    MaximumGrade = studiesProgramDetail.StudiesProgramMajor.MaximumGrade?.ToString(),
                    MinimumGrade = studiesProgramDetail.StudiesProgramMajor.MinimumGrade.ToString(),
                    MinimumPassingGrade = studiesProgramDetail.StudiesProgramMajor.MinimumPassingGrade?.ToString(),
                    Name = studiesProgramDetail.StudiesProgramMajor.Name,
                    PeriodType = studiesProgramDetail.StudiesProgramMajor.PeriodType,
                    PeriodTypeId = studiesProgramDetail.StudiesProgramMajor.PeriodTypeId,
                    PeriodTypeShortDesc = studiesProgramDetail.StudiesProgramMajor.PeriodTypeShortDesc,
                    PlanCode = studiesProgramDetail.StudiesProgramMajor.PlanCode,
                    StudyLevel = studiesProgramDetail.StudiesProgramMajor.StudyLevel,
                    StudyLevelShortDesc = studiesProgramDetail.StudiesProgramMajor.StudyLevelShortDesc
                } : new StudiesProgramMajorViewModel();
                studiesProgramDetailViewModel.StudiesProgramResponsible = studiesProgramDetail.StudiesProgramResponsible != null ?
                new StudiesProgramResponsibleViewModel
                {
                    Curp = studiesProgramDetail.StudiesProgramResponsible.Curp,
                    FirstSurname = studiesProgramDetail.StudiesProgramResponsible.FirstSurname,
                    JobTitle = studiesProgramDetail.StudiesProgramResponsible.JobTitle,
                    JobTitleShortDesc = studiesProgramDetail.StudiesProgramResponsible.JobTitleShortDesc,
                    Name = studiesProgramDetail.StudiesProgramResponsible.Name,
                    SecondSurname = studiesProgramDetail.StudiesProgramResponsible.SecondSurname
                } : new StudiesProgramResponsibleViewModel();
                studiesProgramDetailViewModel.StudiesProgramRvoe = studiesProgramDetail.StudiesProgramRvoe != null ?
                new StudiesProgramRvoeViewModel
                {
                    IssuingDate = studiesProgramDetail.StudiesProgramRvoe.IssuingDate?.ToString("yyyy/MM/dd"),
                    Number = studiesProgramDetail.StudiesProgramRvoe.Number
                } : new StudiesProgramRvoeViewModel();
            }

            return studiesProgramDetailViewModel;
        }

        internal static CoursesViewModel ToViewModel(this AcademicPlanCourse academiPlanCourse)
        {
            CoursesViewModel coursesViewModel = new CoursesViewModel();
            if (academiPlanCourse != null)
            {
                coursesViewModel.TotalCourses = academiPlanCourse.TotalCourses;
                coursesViewModel.TotalCredits = academiPlanCourse.TotalCredits;

                coursesViewModel.AsignedCourses = 0;
                coursesViewModel.ObtainedCredits = 0;
                decimal totalFinalGrades = 0;
                decimal average = 0;

                coursesViewModel.CoursesDetailViewModel = new List<CoursesDetailViewModel>();
                foreach (AcademicPlanCourseDetail academicPlanCourseDetail in academiPlanCourse.CourseDetails)
                {
                    CoursesDetailViewModel coursesDetailViewModel = new CoursesDetailViewModel();
                    coursesDetailViewModel.CourseCycle = academicPlanCourseDetail.CourseCycle;
                    coursesDetailViewModel.CourseCode = academicPlanCourseDetail.CourseCode;
                    coursesDetailViewModel.CourseName = academicPlanCourseDetail.CourseName;
                    coursesDetailViewModel.Credit = academicPlanCourseDetail.Credit;
                    coursesDetailViewModel.FinalGrade = academicPlanCourseDetail.FinalGrade;
                    coursesDetailViewModel.GradeRemarkId = academicPlanCourseDetail.GradeRemarkId;
                    coursesDetailViewModel.SepId = academicPlanCourseDetail.SepId;
                    coursesDetailViewModel.SubjectTypeId = academicPlanCourseDetail.SubjectTypeId;
                    coursesDetailViewModel.TranscriptDetailId = academicPlanCourseDetail.TranscriptDetailId;
                    coursesDetailViewModel.IsInclude = academicPlanCourseDetail.IsInclude;
                    coursesDetailViewModel.TranscriptDetailCertificateId = academicPlanCourseDetail.TranscriptDetailCertificateId;
                    coursesDetailViewModel.Section = academicPlanCourseDetail.Section;
                    coursesDetailViewModel.EventType = academicPlanCourseDetail.EventType;
                    coursesDetailViewModel.CreditType = academicPlanCourseDetail.CreditType;
                    coursesDetailViewModel.CourseCycleId = academicPlanCourseDetail.CourseCycleId;
                    coursesViewModel.CoursesDetailViewModel.Add(coursesDetailViewModel);

                    if (coursesDetailViewModel.IsInclude && coursesDetailViewModel.SubjectTypeId != 4)
                    {
                        coursesViewModel.AsignedCourses++;
                        coursesViewModel.ObtainedCredits = coursesViewModel.ObtainedCredits + academicPlanCourseDetail.Credit;
                        totalFinalGrades = totalFinalGrades + coursesDetailViewModel.FinalGrade;
                    }
                }

                if (coursesViewModel.AsignedCourses > 0)
                    average = totalFinalGrades / coursesViewModel.AsignedCourses;

                average = Math.Round(average, 3);
                average = Math.Truncate(average * 100) / 100;
                coursesViewModel.Average = string.Format("{0:0.00}", average);

                coursesViewModel.GradeRemarks = new List<DropDownViewModel>();
                if (academiPlanCourse.GradeRemarks != null)
                {
                    foreach (DropDownSource dropDownItem in academiPlanCourse.GradeRemarks)
                    {
                        DropDownViewModel dropDownViewModel = new DropDownViewModel();
                        dropDownViewModel.Code = dropDownItem.Code;
                        dropDownViewModel.Desc = dropDownItem.Code + " - " + dropDownItem.Desc;
                        dropDownViewModel.Id = dropDownItem.Id;
                        coursesViewModel.GradeRemarks.Add(dropDownViewModel);
                    }
                }

                if (academiPlanCourse.Subjects != null)
                {
                    coursesViewModel.Subjects = new List<DropDownViewModel>();
                    foreach (DropDownSource dropDownItem in academiPlanCourse.Subjects)
                    {
                        DropDownViewModel dropDownViewModel = new DropDownViewModel();
                        dropDownViewModel.Code = dropDownItem.Code;
                        dropDownViewModel.Desc = dropDownItem.Code + " - " + dropDownItem.Desc;
                        dropDownViewModel.Id = dropDownItem.Id;
                        coursesViewModel.Subjects.Add(dropDownViewModel);
                    }
                }
            }

            return coursesViewModel;
        }
    }
}