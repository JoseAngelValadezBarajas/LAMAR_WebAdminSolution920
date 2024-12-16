// --------------------------------------------------------------------
// <copyright file="ECAcademicPlanDA.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
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
    /// ECAcademicPlanDA
    /// </summary>
    public class ECAcademicPlanDA
    {
        /// <summary>
        /// The factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ECAcademicPlanDA"/> class.
        /// </summary>
        public ECAcademicPlanDA() => _factory = new DatabaseProviderFactory();

        /// <summary>
        /// Creates the ec academic plan course.
        /// </summary>
        /// <param name="pdcCourse">The PDC course.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public bool CreateECAcademicPlanCourse(PdcCourse pdcCourse, string operatorId)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    AcademicPlanCourseCatalog academicPlanCourseCatalog = new AcademicPlanCourseCatalog()
                    {
                        Classification = pdcCourse.Classification,
                        CreateDatetime = DateTime.Now,
                        CreateOpId = operatorId,
                        Credits = (decimal)pdcCourse.Credits,
                        Discipline = pdcCourse.Discipline,
                        EventId = pdcCourse.EventId,
                        EventSubType = pdcCourse.EventSubType,
                        RevisionDatetime = DateTime.Now,
                        RevisionOpId = operatorId,
                        RvoeId = pdcCourse.RvoeId,
                        SepId = pdcCourse.SepId,
                        SubjectTypeId = pdcCourse.SubjectTypeId
                    };

                    context.AcademicPlanCourseCatalogs.InsertOnSubmit(academicPlanCourseCatalog);
                    context.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate",
                    "PowerCampus.DataAccess - ECAcademicPlanDA - CreateECAcademicPlanCourse",
                    ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Gets the ec campus from institution campus.
        /// </summary>
        /// <returns></returns>
        public List<CodeTable> GetECCampusFromInstitutionCampus()
        {
            try
            {
                DataSet institutionCampusDataSet = new DataSet();
                List<CodeTable> campuses = new List<CodeTable>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelCampusFromInstitutionCampus");
                    database.LoadDataSet(command, institutionCampusDataSet, "Campus");

                    if (institutionCampusDataSet.Tables[0].Rows.Count > 0)
                    {
                        campuses = institutionCampusDataSet.Tables[0].AsEnumerable().Select(m => new CodeTable()
                        {
                            CodeValueKey = m.Field<string>("CampusCodeId"),
                            Description = m.Field<string>("CampusName")
                        }).ToList();
                    }
                    return campuses;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate",
                    "PowerCampus.DataAccess - ECAcademicPlanDA - GetECCampusFromInstitutionCampus",
                    ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the ec deg req by campus year term.
        /// </summary>
        /// <param name="campusCodeId">The campus code identifier.</param>
        /// <param name="matricYear">The matric year.</param>
        /// <param name="matricTerm">The matric term.</param>
        /// <returns></returns>
        public List<PdcRvoe> GetECDegReqByCampusYearTerm(string campusCodeId, string matricYear, string matricTerm)
        {
            try
            {
                DataSet institutionCampusDataSet = new DataSet();
                List<PdcRvoe> pdcs = new List<PdcRvoe>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelDegReqByCampusYearTerm");
                    database.AddInParameter(command, "@CampusCodeId", DbType.String, campusCodeId);
                    database.AddInParameter(command, "@MatricYear", DbType.String, matricYear);
                    database.AddInParameter(command, "@MatricTerm", DbType.String, matricTerm);
                    database.LoadDataSet(command, institutionCampusDataSet, "DegReqs");

                    if (institutionCampusDataSet.Tables[0].Rows.Count > 0)
                    {
                        pdcs = institutionCampusDataSet.Tables[0].AsEnumerable().Select(m => new PdcRvoe()
                        {
                            CourseMappingPercent = m.Field<decimal>("Percentage"),
                            IssuingDate = m.Field<DateTime>("AgreementDate"),
                            MajorId = m.Field<int?>("MajorId"),
                            MaximumGrade = m.Field<byte>("MaximumGrade"),
                            MinimumGrade = m.Field<byte>("MinimumGrade"),
                            MinimumPassingGrade = m.Field<decimal>("MinimumPassingGrade"),
                            PDCName = m.Field<string>("PDC"),
                            RvoeAgreement = m.Field<string>("RvoeAgreement"),
                            RvoeId = m.Field<int>("RvoeId"),
                        }).ToList();
                    }
                    return pdcs;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate",
                    "PowerCampus.DataAccess - ECAcademicPlanDA - GetECDegReqByCampusYearTerm",
                    ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the ec deg req courses by rvoe.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public PdcRvoe GetECDegReqCoursesByRvoe(int id)
        {
            try
            {
                DataSet institutionCampusDataSet = new DataSet();
                PdcRvoe pdc = new PdcRvoe();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelDegReqEventByRvoe");
                    database.AddInParameter(command, "@RvoeId", DbType.String, id);
                    database.LoadDataSet(command, institutionCampusDataSet, "DegReqEvent");

                    if (institutionCampusDataSet.Tables[0].Rows.Count > 0)
                    {
                        pdc = institutionCampusDataSet.Tables[0].AsEnumerable().Select(m => new PdcRvoe()
                        {
                            CampusCodeId = m.Field<string>("CampusCodeId"),
                            CampusName = m.Field<string>("CampusName"),
                            CourseMappingPercent = m.Field<decimal>("Percentage"),
                            IssuingDate = m.Field<DateTime>("AgreementDate"),
                            MajorId = m.Field<int?>("MajorId"),
                            MatricTerm = m.Field<string>("MatricTerm"),
                            MatricTermDesc = m.Field<string>("MatricTermDesc"),
                            MatricYear = m.Field<string>("MatricYear"),
                            MaximumGrade = m.Field<byte>("MaximumGrade"),
                            MinimumGrade = m.Field<byte>("MinimumGrade"),
                            MinimumPassingGrade = m.Field<decimal>("MinimumPassingGrade"),
                            PDCName = m.Field<string>("PDC"),
                            RvoeAgreement = m.Field<string>("RvoeAgreement"),
                            RvoeId = id
                        }).FirstOrDefault();
                        if (institutionCampusDataSet.Tables[1].Rows.Count > 0)
                        {
                            pdc.Courses = institutionCampusDataSet.Tables[1].AsEnumerable().Select(m => new PdcCourse()
                            {
                                AcademicPlanCourseCatalogId = m.Field<int?>("AcademicPlanCourseCatalogId"),
                                Classification = m.Field<string>("Classification"),
                                ClassificationDesc = m.Field<string>("ClassificationDesc"),
                                Credits = m.Field<decimal?>("Credits"),
                                Discipline = m.Field<string>("Discipline"),
                                DisciplineDesc = m.Field<string>("DisciplineDesc"),
                                EventId = m.Field<string>("EventId"),
                                EventName = m.Field<string>("EventName"),
                                EventSubType = m.Field<string>("EventSubType"),
                                EventSubTypeDesc = m.Field<string>("EventSubTypeDesc"),
                                SubjectTypeId = m.Field<int?>("SubjectTypeId") ?? 1,
                                SepId = m.Field<string>("SepId"),
                            }).ToList();
                        }
                        if (institutionCampusDataSet.Tables[2].Rows.Count > 0)
                        {
                            pdc.SubjectType = institutionCampusDataSet.Tables[2].AsEnumerable().Select(m => new CodeTable()
                            {
                                Id = m.Field<int>("SubjectTypeId"),
                                Description = $"{m.Field<string>("SubjectTypeCode")} - {m.Field<string>("SubjectTypeDesc")}"
                            }).ToList();
                        }
                    }
                    return pdc;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate",
                    "PowerCampus.DataAccess - ECAcademicPlanDA - GetECDegReqCoursesByRvoeId",
                    ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the ec matriculation term by campus year.
        /// </summary>
        /// <param name="campusCodeId">The campus code identifier.</param>
        /// <param name="matricYear">The matric year.</param>
        /// <returns></returns>
        public List<CodeTable> GetECMatriculationTermByCampusYear(string campusCodeId, string matricYear)
        {
            try
            {
                DataSet institutionCampusDataSet = new DataSet();
                List<CodeTable> campuses = new List<CodeTable>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelMatriculationTermByCampusYear");
                    database.AddInParameter(command, "@CampusCodeId", DbType.String, campusCodeId);
                    database.AddInParameter(command, "@MatricYear", DbType.String, matricYear);
                    database.LoadDataSet(command, institutionCampusDataSet, "MatricTerms");

                    if (institutionCampusDataSet.Tables[0].Rows.Count > 0)
                    {
                        campuses = institutionCampusDataSet.Tables[0].AsEnumerable().Select(m => new CodeTable()
                        {
                            CodeValueKey = m.Field<string>("MatricTerm"),
                            Description = m.Field<string>("MatricTermDesc")
                        }).ToList();
                    }
                    return campuses;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate",
                    "PowerCampus.DataAccess - ECAcademicPlanDA - GetECMatriculationTermByCampusYear",
                    ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the ec matriculation year by campus.
        /// </summary>
        /// <param name="campusCodeId">The campus code identifier.</param>
        /// <returns></returns>
        public List<CodeTable> GetECMatriculationYearByCampus(string campusCodeId)
        {
            try
            {
                DataSet institutionCampusDataSet = new DataSet();
                List<CodeTable> campuses = new List<CodeTable>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelMatriculationYearByCampus");
                    database.AddInParameter(command, "@CampusCodeId", DbType.String, campusCodeId);
                    database.LoadDataSet(command, institutionCampusDataSet, "MatricYears");

                    if (institutionCampusDataSet.Tables[0].Rows.Count > 0)
                    {
                        campuses = institutionCampusDataSet.Tables[0].AsEnumerable().Select(m => new CodeTable()
                        {
                            CodeValueKey = m.Field<string>("MatricYear"),
                            Description = m.Field<string>("MatricYear")
                        }).ToList();
                    }
                    return campuses;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate",
                    "PowerCampus.DataAccess - ECAcademicPlanDA - GetECMatriculationYearByCampus",
                    ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the ec academic plan course.
        /// </summary>
        /// <param name="pdcCourse">The PDC course.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public bool UpdateECAcademicPlanCourse(PdcCourse pdcCourse, string operatorId)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    AcademicPlanCourseCatalog academicPlanCourse = context.AcademicPlanCourseCatalogs.Single(r => r.AcademicPlanCourseCatalogId == pdcCourse.AcademicPlanCourseCatalogId);
                    academicPlanCourse.AcademicPlanCourseCatalogId = (int)pdcCourse.AcademicPlanCourseCatalogId;
                    academicPlanCourse.Classification = pdcCourse.Classification;
                    academicPlanCourse.CreateDatetime = DateTime.Now;
                    academicPlanCourse.CreateOpId = operatorId;
                    academicPlanCourse.Credits = (decimal)pdcCourse.Credits;
                    academicPlanCourse.Discipline = pdcCourse.Discipline;
                    academicPlanCourse.EventId = pdcCourse.EventId;
                    academicPlanCourse.EventSubType = pdcCourse.EventSubType;
                    academicPlanCourse.RevisionDatetime = DateTime.Now;
                    academicPlanCourse.RevisionOpId = operatorId;
                    academicPlanCourse.RvoeId = pdcCourse.RvoeId;
                    academicPlanCourse.SepId = pdcCourse.SepId;
                    academicPlanCourse.SubjectTypeId = pdcCourse.SubjectTypeId;
                    context.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate",
                    "PowerCampus.DataAccess - ECAcademicPlanDA - UpdateECAcademicPlanCourse",
                    ex.Message);
                return false;
            }
        }
    }
}