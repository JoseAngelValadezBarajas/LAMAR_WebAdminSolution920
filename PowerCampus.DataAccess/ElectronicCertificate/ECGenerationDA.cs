// --------------------------------------------------------------------
// <copyright file="ECGenerationDA.cs" company="Ellucian">
//     Copyright 2020-2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.Entities.ElectronicCertificate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace PowerCampus.DataAccess.ElectronicCertificate
{
    /// <summary>
    /// ECGenerationDA
    /// </summary>
    public class ECGenerationDA
    {
        /// <summary>
        /// The factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ECGenerationDA"/> class.
        /// </summary>
        public ECGenerationDA() => _factory = new DatabaseProviderFactory();

        /// <summary>
        /// Gets the certification types.
        /// </summary>
        /// <returns></returns>
        public List<CodeTable> GetCertificationTypes()
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    IQueryable<CodeTable> query =
                        (from cpt in context.CODE_CERTIFICATETYPEs
                         where cpt.STATUS == "A"
                         select new CodeTable
                         {
                             CodeValueKey = cpt.CODE_VALUE_KEY,
                             Description = cpt.MEDIUM_DESC,
                             Id = cpt.CertificateTypeId,
                             ShortDescription = cpt.SHORT_DESC
                         });

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - ECGenerationDA - GetCertificationTypes", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the folio.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="institutionCampusId">The institution campus identifier.</param>
        /// <returns></returns>
        public string GetFolio(string peopleId, int institutionCampusId)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                    return context.spSelInstitutionCampusFolio(peopleId, institutionCampusId).FirstOrDefault()?.Folio;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - ECGenerationDA - GetFolio", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the operator campus validation.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public bool GetOperatorCampusValidation(string operatorId)
        {
            try
            {
                bool hasCampusRelated;
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                    hasCampusRelated = context.ElectronicCertificateOperators.Any(eco => eco.OperatorId == operatorId
                    && eco.CampusCodeId != string.Empty && eco.CampusCodeId != null);
                return hasCampusRelated;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - ECGenerationDA - GetOperatorCampusValidation", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the period types.
        /// </summary>
        /// <returns></returns>
        public List<CodeTable> GetPeriodTypes()
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    IQueryable<CodeTable> query =
                        (from cpt in context.CODE_PERIODTYPEs
                         where cpt.STATUS == "A"
                         select new CodeTable
                         {
                             CodeValueKey = cpt.CODE_VALUE_KEY,
                             Description = cpt.MEDIUM_DESC,
                             Id = cpt.PeriodTypeId,
                             ShortDescription = cpt.SHORT_DESC
                         });

                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - ECGenerationDA - GetPeriodTypes", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the studies program detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public StudiesProgramDetail GetStudiesProgramDetail(int id)
        {
            try
            {
                List<StudiesProgramDetail> studiesProgramDetails = null;
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    studiesProgramDetails = context.spSelAcademicPlanDetailByRvoe(id).Select(spd => new StudiesProgramDetail()
                    {
                        CampusName = spd.CampusName,
                        CampusSepId = spd.CampusSepId,
                        FederalEntityCatalogMapping = spd.FederalEntityLong,
                        FederalEntityCatalogMappingCode = spd.FederalEntityCode,
                        FederalEntityCatalogMappingShortDesc = spd.FederalEntityShort,
                        Id = id,
                        InstitutionCampusId = spd.InstitutionCampusId,
                        InstitutionName = spd.InstitutionName,
                        InstitutionSepId = spd.InstitutionSepId,
                        SigningInstitutionId = spd.SigningInstitutionId,
                        StudiesProgramMajor = new StudiesProgramMajor
                        {
                            Code = spd.MajorCode,
                            Id = spd.MajorId,
                            MaximumGrade = spd.MaximumGrade,
                            MinimumGrade = spd.MinimumGrade,
                            MinimumPassingGrade = spd.MinimumPassingGrade,
                            Name = spd.MajorName,
                            PeriodType = spd.PeriodTypeLong,
                            PeriodTypeId = spd.PeriodTypeId,
                            PeriodTypeShortDesc = spd.PeriodTypeShort,
                            PlanCode = spd.PlanCode,
                            StudyLevel = spd.StudyLevelLong,
                            StudyLevelShortDesc = spd.StudyLevelShort
                        },
                        StudiesProgramResponsible = new StudiesProgramResponsible
                        {
                            Curp = spd.Curp,
                            FirstSurname = spd.FirstSurname,
                            JobTitle = spd.JobTitleLong,
                            JobTitleShortDesc = spd.JobTitleShort,
                            Name = spd.Name,
                            SecondSurname = spd.SecondSurname
                        },
                        StudiesProgramRvoe = new StudiesProgramRvoe
                        {
                            IssuingDate = spd.RvoeIssuingDate,
                            Number = spd.RvoeNumber
                        }
                    }).ToList();
                }

                return studiesProgramDetails != null && studiesProgramDetails.Count > 0 ? studiesProgramDetails[0] : null;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - ECGenerationDA - GetStudiesProgramDetail", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the studies programs.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public List<StudiesProgram> GetStudiesPrograms(string peopleId, string operatorId)
        {
            try
            {
                List<StudiesProgram> studiesPrograms = null;
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    studiesPrograms = context.spSelAcademicPlanByStudent(peopleId, operatorId).Select(sp => new StudiesProgram()
                    {
                        Campus = sp.CampusName,
                        HasCampusCode = sp.MissingCampusSepCode != 1,
                        HasCoursesMapping = sp.IncompleteMapping != 1,
                        HasInstitutionCode = sp.MissingInstitutionSepId != 1,
                        HasOperatorCampus = sp.CampusNotAssignedToOperator != 1,
                        HasResponsibleCampus = sp.MissingCampusResponsible != 1,
                        HasRvoeInformation = sp.IncompleteRvoe != 1,
                        HasSigningInstitution = sp.MissingSigningInstitutionId != 1,
                        Id = sp.RvoeId,
                        Program = sp.PDC,
                        Term = sp.MatricYearTerm
                    }).ToList();
                }

                return studiesPrograms;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - ECGenerationDA - GetStudiesPrograms", ex.Message);
                throw;
            }
        }

        #region Courses List

        /// <summary>
        /// Creates the transcript detail certificate.
        /// </summary>
        /// <param name="academicPlanCourseDetails">The academic plan course details.</param>
        public void CreateTranscriptDetailCertificate(List<AcademicPlanCourseDetail> academicPlanCourseDetails)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    foreach (AcademicPlanCourseDetail academicPlanCourseDetail in academicPlanCourseDetails)
                    {
                        int? transcriptDetailCertificateId = 0;
                        transcriptDetailCertificateId = context.spInsTranscriptDetailCertificate(
                        academicPlanCourseDetail.TranscriptDetailId,
                        academicPlanCourseDetail.GradeRemarkId,
                        academicPlanCourseDetail.SubjectTypeId,
                        academicPlanCourseDetail.IsInclude,
                        academicPlanCourseDetail.OperatorId,
                        ref transcriptDetailCertificateId);
                    }
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - ECGenerationDA - CreateTranscriptDetailCertificate", exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the academic plan course by student.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="rvoeId">The rvoe identifier.</param>
        /// <returns></returns>
        public AcademicPlanCourse GetAcademicPlanCourseByStudent(string peopleId, int rvoeId)
        {
            try
            {
                DataSet academicPlanCourseDataSet = new DataSet();
                Database database = _factory.CreateDefault();
                AcademicPlanCourse academiPlanCourse = null;
                LoggerHelper.LogWebError("ElectronicCertificateInformation", "PowerCampus.DataAccess - ECGenerationDA - GetAcademicPlanCourseByStudent", peopleId);
                LoggerHelper.LogWebError("ElectronicCertificateInformation", "PowerCampus.DataAccess - ECGenerationDA - GetAcademicPlanCourseByStudent", rvoeId.ToString());
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelAcademicPlanCourseByStudent");
                    database.AddInParameter(command, "@peopleId", DbType.String, peopleId);
                    database.AddInParameter(command, "@rvoeId", DbType.Int32, rvoeId);
                    database.LoadDataSet(command, academicPlanCourseDataSet, "webAdmin");
                    if (academicPlanCourseDataSet.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = academicPlanCourseDataSet.Tables[0].Rows[0];
                        academiPlanCourse = new AcademicPlanCourse
                        {
                            TotalCourses = row.Field<int>("TotalCourses"),
                            TotalCredits = row.Field<decimal>("TotalCredits")
                        };

                        academiPlanCourse.CourseDetails = academicPlanCourseDataSet.Tables[1].AsEnumerable().Select(m => new AcademicPlanCourseDetail()
                        {
                            TranscriptDetailId = m.Field<int>("TranscriptDetailId"),
                            CourseCycle = m.Field<string>("CourseCycle"),
                            SepId = m.Field<string>("SepId"),
                            CourseCode = m.Field<string>("CourseCode"),
                            CourseName = m.Field<string>("CourseName"),
                            FinalGrade = m.Field<decimal>("FinalGrade"),
                            GradeRemarkId = m.Field<int>("GradeRemarkId"),
                            SubjectTypeId = m.Field<int>("SubjectTypeId"),
                            Credit = m.Field<decimal>("Credit"),
                            Section = m.Field<string>("Section"),
                            EventType = m.Field<string>("EventType"),
                            CreditType = m.Field<string>("CreditType"),
                            IsInclude = m.Field<bool?>("IncludeInCertificate") == true,
                            TranscriptDetailCertificateId = m.Field<int?>("TranscriptDetailCertificateId"),
                            CourseCycleId = m.Field<int>("CourseCycleId")
                        }).ToList();

                        academiPlanCourse.GradeRemarks = academicPlanCourseDataSet.Tables[2].AsEnumerable().Select(m => new DropDownSource()
                        {
                            Id = m.Field<int>("GradeRemarkId"),
                            Code = m.Field<string>("GradeRemarkCode"),
                            Desc = m.Field<string>("GradeRemarkDesc")
                        }).ToList();

                        academiPlanCourse.Subjects = academicPlanCourseDataSet.Tables[3].AsEnumerable().Select(m => new DropDownSource()
                        {
                            Id = m.Field<int>("SubjectTypeId"),
                            Code = m.Field<string>("SubjectTypeCode"),
                            Desc = m.Field<string>("SubjectTypeDesc")
                        }).ToList();
                    }
                }
                return academiPlanCourse;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - ECGenerationDA - GetAcademicPlanCourseByStudent", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the courses outside academic plan.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="rvoeId">The rvoe identifier.</param>
        /// <returns></returns>
        public AcademicPlanCourse GetCoursesOutsideAcademicPlan(string peopleId, int rvoeId)
        {
            try
            {
                DataSet academicPlanCourseDataSet = new DataSet();
                Database database = _factory.CreateDefault();
                AcademicPlanCourse academiPlanCourse = new AcademicPlanCourse();
                academiPlanCourse.CourseDetails = new List<AcademicPlanCourseDetail>();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelStudentCourseOutsideAcademicPlan");
                    database.AddInParameter(command, "@peopleId", DbType.String, peopleId);
                    database.AddInParameter(command, "@rvoeId", DbType.Int32, rvoeId);
                    database.LoadDataSet(command, academicPlanCourseDataSet, "webAdmin");
                    if (academicPlanCourseDataSet.Tables[0].Rows.Count > 0)
                    {
                        academiPlanCourse.CourseDetails = academicPlanCourseDataSet.Tables[0].AsEnumerable().Select(m => new AcademicPlanCourseDetail()
                        {
                            TranscriptDetailId = m.Field<int>("TranscriptDetailId"),
                            CourseCycle = m.Field<string>("CourseCycle"),
                            CourseCode = m.Field<string>("CourseCode"),
                            CourseName = m.Field<string>("CourseName"),
                            FinalGrade = m.Field<decimal>("FinalGrade"),
                            Credit = m.Field<decimal>("Credit"),
                            Section = m.Field<string>("Section"),
                            EventType = m.Field<string>("EventType"),
                            CreditType = m.Field<string>("CreditType"),
                            GradeRemarkId = m.Field<int>("GradeRemarkId"),
                            SubjectTypeId = m.Field<int>("SubjectTypeId"),
                            CourseCycleId = m.Field<int>("CourseCycleId")
                        }).ToList();
                    }
                    return academiPlanCourse;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - ECGenerationDA - GetCoursesOutsideAcademicPlan", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the transcript detail certificate.
        /// </summary>
        /// <param name="academicPlanCourseDetails">The academic plan course details.</param>
        public void UpdateTranscriptDetailCertificate(List<AcademicPlanCourseDetail> academicPlanCourseDetails)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    foreach (AcademicPlanCourseDetail academicPlanCourseDetail in academicPlanCourseDetails)
                    {
                        context.spUpdTranscriptDetailCertificate(
                        academicPlanCourseDetail.TranscriptDetailCertificateId,
                        academicPlanCourseDetail.GradeRemarkId,
                        academicPlanCourseDetail.SubjectTypeId,
                        academicPlanCourseDetail.IsInclude,
                        academicPlanCourseDetail.OperatorId);
                    }
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - ECGenerationDA - UpdateTranscriptDetailCertificate", exception.Message);
                throw;
            }
        }

        #endregion Courses List
    }
}