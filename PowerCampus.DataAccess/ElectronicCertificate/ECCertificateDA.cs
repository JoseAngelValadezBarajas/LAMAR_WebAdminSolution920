// --------------------------------------------------------------------
// <copyright file="ECCertificateDA.cs" company="Ellucian">
//     Copyright 2020-2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.Entities;
using PowerCampus.Entities.ElectronicCertificate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Xml.Linq;

namespace PowerCampus.DataAccess.ElectronicCertificate
{
    /// <summary>
    /// ECCertificateDA
    /// </summary>
    public class ECCertificateDA
    {
        #region Private Fields

        /// <summary>
        /// The decimal format
        /// </summary>
        private const string decimalFormat = "{0:0.00##}";

        /// <summary>
        /// The factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        #endregion Private Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="ECCertificateDA" /> class.
        /// </summary>
        public ECCertificateDA() => _factory = new DatabaseProviderFactory();

        /// <summary>
        /// Creates the specified certificate information.
        /// </summary>
        /// <param name="c">The certificate information.</param>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public int? Create(CertificateInfo c)
        {
            try
            {
                int? electronicCertificateId = 0;
                int? electronicCertificateFileId = 0;
                if (c != null)
                {
                    using (TransactionScope scope = new TransactionScope())
                    using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                    {
                        #region Insert ElectronicCertificate

                        context.spInsElectronicCertificate(
                                               "3.0"
                                               , "5"
                                               , c.Folio
                                               , c.InstitutionSepId
                                               , c.InstitutionName
                                               , c.CampusSepCode
                                               , c.CampusName
                                               , c.FederalEntityShortDesc
                                               , c.FederalEntity
                                               , c.ResponsibleCurp
                                               , c.ResponsibleName
                                               , c.ResponsibleFirstSurname
                                               , c.ResponsibleSecondSurname
                                               , c.ResponsibleJobTitleId
                                               , c.ResponsibleJobTitle
                                               , c.RvoeAgreementNumber
                                               , c.ExpeditionDate
                                               , c.MajorId
                                               , c.MajorCode
                                               , c.MajorName
                                               , c.PeriodTypeId
                                               , c.PeriodType
                                               , c.PlanCode
                                               , c.StudyLevelId
                                               , c.StudyLevel
                                               , c.MinimumGrade
                                               , c.MaximumGrade
                                               , c.MinimumPassingGrade
                                               , $"P{c.PeopleId}"
                                               , c.Curp
                                               , c.Name
                                               , c.FirstSurname
                                               , c.SecondSurname
                                               , c.Gender.Equals("M") ? "251" : "250"
                                               , c.Gender
                                               , c.BirthDate
                                               , c.TotalCourse
                                               , c.CourseAssigned
                                               , c.GPA
                                               , c.TotalCredit
                                               , c.CreditEarned
                                               , c.CertificationTypeId
                                               , c.CertificationType
                                               , c.IssuingDate
                                               , c.IssuingFederalShortDesc
                                               , c.IssuingFederalEntity
                                               , CertificateStatus.Generated
                                               , string.Empty
                                               , string.Empty
                                               , c.OperatorId
                                               , c.SigningInstitutionId
                                               , c.TotalCycle
                                               , ref electronicCertificateId);

                        #endregion Insert ElectronicCertificate

                        if (electronicCertificateId > 0)
                        {
                            //Increment the folio
                            DataAccess.InstitutionCampus institutionCampus = null;

                            try
                            {
                                // Intentar encontrar el campus específico por CampusSepCode
                                institutionCampus = context.InstitutionCampus
                                    .SingleOrDefault(x => x.CampusSepCode == c.CampusSepCode);
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    LoggerHelper.LogWebError(Constants.EcArea, "Error al buscar CampusSepCode", ex.Message);
                                    institutionCampus = context.InstitutionCampus.FirstOrDefault();
                                }
                                catch (Exception ex2)
                                {
                                    LoggerHelper.LogWebError(Constants.EcArea, "Error al buscar CampusSepCode", ex2.Message);
                                }
                            }

                            if (institutionCampus == null)
                            {
                        
                                institutionCampus = context.InstitutionCampus.FirstOrDefault();
                            }

                            if (institutionCampus != null)
                            {
                                try
                                {
                                    int institutionFolio = int.Parse(institutionCampus.Folio);
                                    institutionCampus.Folio = (institutionFolio + 1).ToString();
                                }
                                catch (Exception ex)
                                {
                                    LoggerHelper.LogWebError(Constants.EcArea, "Error al actualizar el folio", ex.Message);
                                }
                            }
                            else
                            {
                               
                            }

                            int? electronicCertificateCourseId = 0;
                            foreach (CertificateCourseInfo course in c.Courses)
                            {
                                #region Insert ElectronicCertificateCourse

                                context.spInsElectronicCertificateCourse(
                                    electronicCertificateId
                                    , course.TranscriptDetailId
                                    , course.SepId
                                    , course.CourseCode
                                    , course.EventName
                                    , course.EventCycle
                                    , string.Format(decimalFormat, course.FinalGrade)
                                    , course.GradeRemarkId
                                    , course.GradeRemark
                                    , course.SubjectTypeId
                                    , course.SubjectType
                                    , course.Credits
                                    , ref electronicCertificateCourseId
                                    );
                                if (!electronicCertificateCourseId.HasValue)
                                    break;

                                #endregion Insert ElectronicCertificateCourse
                            }

                            if (electronicCertificateFileId.HasValue)
                            {
                                context.SubmitChanges();
                                scope.Complete();
                            }
                        }
                        else
                        {
                            LoggerHelper.LogWebError(Constants.EcArea, Constants.DataAccess + " - Create", "Electronic Certificiate was not created");
                        }
                    }
                }

                return electronicCertificateId;
            }
            catch (Exception exception)
            {
                LoggerHelper.LogWebError(Constants.EcArea, Constants.DataAccess + " - Create", exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Creates the XML.
        /// </summary>
        /// <param name="electronicCertificateId">The electronic certificate identifier.</param>
        /// <param name="certificateXml">The certificate XML.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public bool CreateXml(int electronicCertificateId, string certificateXml, string operatorId)
        {
            using (ElectronicCertificateContext context = new ElectronicCertificateContext())
            {
                int? electronicCertificateFileId = 0;
                XElement xElement = XElement.Parse(certificateXml);
                electronicCertificateFileId =
                    context.spInsElectronicCertificateFile(electronicCertificateId, xElement, null, operatorId, ref electronicCertificateFileId);
                context.SubmitChanges();
                return electronicCertificateFileId > 0 && electronicCertificateFileId != null;
            }
        }

        /// <summary>
        /// Deletes the specified electronic certificate identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    DataAccess.ElectronicCertificate electronicCertificate = context.ElectronicCertificates.Single(r => r.ElectronicCertificateId == id);

                    context.ElectronicCertificates.DeleteOnSubmit(electronicCertificate);
                    context.SubmitChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError(Constants.EcArea, Constants.DataAccess + " - ECCertificateDA - Delete", e.Message + e.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Gets the ec campus by status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public List<CodeTable> GetECCampusByStatus(string status)
        {
            try
            {
                DataSet institutionCampusDataSet = new DataSet();
                List<CodeTable> campuses = new List<CodeTable>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelElectronicCertificateCampusByStatus");
                    database.AddInParameter(command, "@Status", DbType.String, status == "All" ? null : status);
                    database.LoadDataSet(command, institutionCampusDataSet, "Campus");

                    if (institutionCampusDataSet.Tables[0].Rows.Count > 0)
                    {
                        campuses = institutionCampusDataSet.Tables[0].AsEnumerable().Select(m => new CodeTable()
                        {
                            CodeValueKey = m.Field<string>("CampusSepCode"),
                            Description = m.Field<string>("CampusName")
                        }).ToList();
                    }
                    return campuses;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, Constants.DataAccess + " - ECCertificateDA - GetECCampusByStatus", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the ec certificate detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public CertificateInfo GetECCertificateDetail(int id)
        {
            try
            {
                DataSet certificateDetailDataSet = new DataSet();
                CertificateInfo certificate = new CertificateInfo();
                Database database = _factory.CreateDefault();
                List<CertificateCourseInfo> courses = null;
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("WebAdmin.spSelElectronicCertificateDetail");

                    database.AddInParameter(command, "@ElectronicCertificateId", DbType.Int32, id);
                    database.LoadDataSet(command, certificateDetailDataSet, "CertificateDetail");

                    if (certificateDetailDataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in certificateDetailDataSet.Tables[0].Rows)
                        {
                            certificate = new CertificateInfo()
                            {
                                Average = row.Field<decimal>("Average"),
                                BirthDate = row.Field<DateTime>("BirthDate"),
                                CampusName = row.Field<string>("CampusName"),
                                CampusSepCode = row.Field<string>("CampusSepCode"),
                                CertificationType = row.Field<string>("CertificationType"),
                                CertificationTypeId = row.Field<string>("CertificationTypeId"),
                                CourseAssigned = row.Field<int>("CourseAssigned"),
                                CreditEarned = row.Field<decimal>("CreditEarned"),
                                Curp = row.Field<string>("Curp"),
                                ExpeditionDate = row.Field<DateTime>("ExpeditionDate"),
                                FederalEntity = row.Field<string>("CampusFederalEntity"),
                                FederalEntityCode = row.Field<string>("CampusFederalEntityCode"),
                                SigningInstitutionId = row.Field<string>("SigningInstitutionId"),
                                FirstSurname = row.Field<string>("FirstSurname"),
                                Folio = row.Field<string>("Folio"),
                                Gender = row.Field<string>("Gender"),
                                GenderId = row.Field<string>("GenderId"),
                                Id = row.Field<int>("ElectronicCertificateId"),
                                InstitutionName = row.Field<string>("InstitutionName"),
                                InstitutionSepId = row.Field<string>("InstitutionSepId"),
                                IssuingDate = row.Field<DateTime>("IssuingDate"),
                                IssuingFederalEntity = row.Field<string>("IssuingFederalEntity"),
                                IssuingFederalEntityCode = row.Field<string>("IssuingFederalEntityCode"),
                                MajorId = row.Field<int>("MajorId"),
                                MajorName = row.Field<string>("MajorName"),
                                MaximumGrade = row.Field<byte>("MaximumGrade"),
                                MinimumGrade = row.Field<byte>("MinimumGrade"),
                                MinimumPassingGrade = row.Field<decimal>("MinimumPassingGrade"),
                                Name = row.Field<string>("Name"),
                                PeopleId = row.Field<string>("PeopleCodeId"),
                                PeriodType = row.Field<string>("PeriodType"),
                                PeriodTypeId = row.Field<string>("PeriodTypeId"),
                                PlanCode = row.Field<string>("PlanCode"),
                                ResponsibleCurp = row.Field<string>("ResponsibleCurp"),
                                ResponsibleFirstSurname = row.Field<string>("ResponsibleFirstSurname"),
                                ResponsibleJobTitle = row.Field<string>("ResponsiblePosition"),
                                ResponsibleJobTitleId = row.Field<string>("ResponsiblePositionId"),
                                ResponsibleName = row.Field<string>("ResponsibleName"),
                                ResponsibleSecondSurname = row.Field<string>("ResponsibleSecondSurname"),
                                RvoeAgreementNumber = row.Field<string>("RvoeAgreementNumber"),
                                SecondSurname = row.Field<string>("SecondSurname"),
                                StudyLevel = row.Field<string>("StudyLevel"),
                                StudyLevelId = row.Field<string>("StudyLevelId"),
                                TotalCourse = row.Field<int>("TotalCourse"),
                                TotalCredit = row.Field<decimal>("TotalCredit"),
                                TotalCycle = row.Field<int>("TotalCycle")
                            };
                        }

                        if (certificateDetailDataSet.Tables[1].Rows.Count > 0)
                        {
                            courses = certificateDetailDataSet.Tables[1].AsEnumerable().Select(m => new CertificateCourseInfo()
                            {
                                Id = m.Field<int>("ElectronicCertificateCourseId"),
                                SepId = m.Field<string>("SepId"),
                                EventName = m.Field<string>("EventName"),
                                EventCycle = m.Field<string>("EventCycle"),
                                FinalGrade = m.Field<string>("FinalGrade"),
                                GradeRemarkId = m.Field<string>("GradeRemarkId"),
                                GradeRemark = m.Field<string>("GradeRemark"),
                                GradeRemarkShortDesc = m.Field<string>("GradeRemarkShortDesc"),
                                SubjectTypeId = m.Field<string>("SubjectTypeId"),
                                SubjectType = m.Field<string>("SubjectType"),
                                SubjectTypeShortDesc = m.Field<string>("SubjectTypeShortDesc"),
                                Credits = m.Field<decimal>("Credits"),
                            }).ToList();
                            certificate.Courses = courses;
                        }
                    }
                    return certificate;
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.LogWebError(Constants.EcArea, Constants.DataAccess + " - GetECCertificateDetail", exception.Message);
                throw;
            }
        }

        #region Search

        /// <summary>
        /// Gets the ec advanced search.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        public List<Certificate> GetECAdvancedSearch(Search search)
        {
            try
            {
                DataSet advancedSearchDataSet = new DataSet();
                List<Certificate> certificates = new List<Certificate>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("WebAdmin.spSelElectronicCertificateSearch");

                    database.AddInParameter(command, "@CampusSepCode", DbType.String, string.IsNullOrEmpty(search.CampusCodeId) ? null : search.CampusCodeId);
                    database.AddInParameter(command, "@CertificationTypeId", DbType.String, string.IsNullOrEmpty(search.CertificationTypeId) ? null : search.CertificationTypeId);
                    database.AddInParameter(command, "@Folio", DbType.String, string.IsNullOrEmpty(search.Folio) ? null : search.Folio);
                    database.AddInParameter(command, "@IssuingDate", DbType.DateTime, string.IsNullOrEmpty(search.IssuingDate) ?
                        (DateTime?)null : DateTime.ParseExact(search.IssuingDate, "yyyy/MM/dd", CultureInfo.InvariantCulture));
                    database.AddInParameter(command, "@MajorId", DbType.Int32, search.MajorId == null || search.MajorId <= 0 ? null : search.MajorId);
                    database.AddInParameter(command, "@Status", DbType.String, string.IsNullOrEmpty(search.Status) ? null : search.Status);
                    database.AddInParameter(command, "@StudentKeyword", DbType.String, string.IsNullOrEmpty(search.Student) ? null : search.Student);
                    database.LoadDataSet(command, advancedSearchDataSet, "AdvancedSearch");

                    if (advancedSearchDataSet.Tables[0].Rows.Count > 0)
                    {
                        certificates = advancedSearchDataSet.Tables[0].AsEnumerable().Select(m =>
                        {
                            return new Certificate()
                            {
                                Campus = m.Field<string>("Campus"),
                                CertificationType = m.Field<string>("CertificationType"),
                                ElectronicCertificateFileId = m.Field<int>("ElectronicCertificateFileId"),
                                Folio = m.Field<string>("ControlFolio"),
                                HasPdfFile = m.Field<int>("HasPdf") == 1,
                                HasXmlFile = m.Field<int>("HasXml") == 1,
                                Id = m.Field<int>("ElectronicCertificateId"),
                                IssuingDate = m.Field<DateTime>("IssuingDate"),
                                Notes = m.Field<string>("Note"),
                                PaymentFolio = m.Field<string>("PaymentFolio"),
                                PdfSize = m.Field<long?>("PDFSize") ?? 0,
                                PeopleCodeId = m.Field<string>("PeopleCodeId"),
                                Program = m.Field<string>("PDC"),
                                Status = m.Field<string>("Status"),
                                StudentName = m.Field<string>("StudentName"),
                                XmlSize = m.Field<int?>("XMLSize") ?? 0
                            };
                        }).ToList();
                    }
                    return certificates;
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.LogWebError(Constants.EcArea, Constants.DataAccess + " - GetECAdvancedSearch", exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the ec basic search.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public List<Certificate> GetECBasicSearch(string keyword, string status)
        {
            try
            {
                DataSet basicSearchDataSet = new DataSet();
                List<Certificate> certificates = new List<Certificate>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelElectronicCertificateByKeywordStatus");
                    database.AddInParameter(command, "@Keyword", DbType.String, keyword);
                    database.AddInParameter(command, "@Status", DbType.String, string.IsNullOrEmpty(status) ? null : status);
                    database.LoadDataSet(command, basicSearchDataSet, "BasicSearch");

                    if (basicSearchDataSet.Tables[0].Rows.Count > 0)
                    {
                        certificates = basicSearchDataSet.Tables[0].AsEnumerable().Select(m =>
                        {
                            return new Certificate()
                            {
                                Campus = m.Field<string>("Campus"),
                                CertificationType = m.Field<string>("CertificationType"),
                                ElectronicCertificateFileId = m.Field<int?>("ElectronicCertificateFileId") ?? 0,
                                Folio = m.Field<string>("ControlFolio"),
                                HasPdfFile = m.Field<int>("HasPdf") == 1,
                                HasXmlFile = m.Field<int>("HasXml") == 1,
                                Id = m.Field<int>("ElectronicCertificateId"),
                                IssuingDate = m.Field<DateTime>("IssuingDate"),
                                Notes = m.Field<string>("Note"),
                                PaymentFolio = m.Field<string>("PaymentFolio"),
                                PdfSize = m.Field<long?>("PDFSize") ?? 0,
                                PeopleCodeId = m.Field<string>("PeopleCodeId"),
                                Program = m.Field<string>("PDC"),
                                Status = m.Field<string>("Status"),
                                StudentName = m.Field<string>("StudentName"),
                                XmlSize = m.Field<int?>("XMLSize") ?? 0
                            };
                        }).ToList();
                    }
                    return certificates;
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.LogWebError(Constants.EcArea, Constants.DataAccess + " - GetECBasicSearch", exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the ec download files.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        public List<CertificateFile> GetECDownloadFiles(List<int> ids)
        {
            using (ElectronicCertificateContext context = new ElectronicCertificateContext())
            {
                return (from ec in context.ElectronicCertificates
                        join ecf in context.ElectronicCertificateFiles
                        on ec.ElectronicCertificateId equals ecf.ElectronicCertificateId
                        where ids.Contains(ecf.ElectronicCertificateFileId)
                        select new CertificateFile()
                        {
                            XmlFile = ecf.CertificateXml.ToString(),
                            Folio = ec.Folio,
                            PdfFile = ecf.CertificatePdf != null ? ecf.CertificatePdf.ToArray() : null
                        }).ToList();
            }
        }

        #endregion Search

        /// <summary>
        /// Gets the ec certificate status.
        /// </summary>
        /// <returns></returns>
        public List<string> GetECCertificateStatus()
        {
            try
            {
                DataSet majorDataSet = new DataSet();
                List<string> status = new List<string>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelElectronicCertificateStatus");
                    database.LoadDataSet(command, majorDataSet, "Status");

                    if (majorDataSet.Tables[0].Rows.Count > 0)
                        status = majorDataSet.Tables[0].AsEnumerable().Select(m => m.Field<string>("Status")).ToList();
                }
                return status;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, Constants.DataAccess + " - ECCertificateDA - GetECCertificateStatus", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the ec certification type by status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public List<CodeTable> GetECCertificationTypeByStatus(string status)
        {
            try
            {
                DataSet certificationTypeDataSet = new DataSet();
                List<CodeTable> campuses = new List<CodeTable>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelElectronicCertificateCertificationTypeByStatus");
                    database.AddInParameter(command, "@Status", DbType.String, status == "All" ? null : status);
                    database.LoadDataSet(command, certificationTypeDataSet, "CertificationType");

                    if (certificationTypeDataSet.Tables[0].Rows.Count > 0)
                    {
                        campuses = certificationTypeDataSet.Tables[0].AsEnumerable().Select(m => new CodeTable()
                        {
                            CodeValueKey = m.Field<string>("CertificationTypeId"),
                            Description = m.Field<string>("CertificationType")
                        }).ToList();
                    }
                    return campuses;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, Constants.DataAccess + " - ECCertificateDA - GetECCertificationTypeByStatus", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the ec major by status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public List<CodeTable> GetECMajorByStatus(string status)
        {
            try
            {
                DataSet majorDataSet = new DataSet();
                List<CodeTable> major = new List<CodeTable>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelElectronicCertificateMajorByStatus");
                    database.AddInParameter(command, "@Status", DbType.String, status == "All" ? null : status);
                    database.LoadDataSet(command, majorDataSet, "Major");

                    if (majorDataSet.Tables[0].Rows.Count > 0)
                    {
                        major = majorDataSet.Tables[0].AsEnumerable().Select(m => new CodeTable()
                        {
                            Id = m.Field<int>("MajorId"),
                            Description = m.Field<string>("MajorName")
                        }).ToList();
                    }
                    return major;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, Constants.DataAccess + " - ECCertificateDA - GetECMajorByStatus", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the ec PDF file.
        /// </summary>
        /// <param name="electronicCertificateFileId">The electronic certificate file identifier.</param>
        /// <returns></returns>
        /// <exception cref="ElectronicCertificateContext"></exception>
        public CertificateFile GetECPdfFile(int electronicCertificateFileId)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    return (from ec in context.ElectronicCertificates
                            join ecf in context.ElectronicCertificateFiles
                            on ec.ElectronicCertificateId equals ecf.ElectronicCertificateId
                            where ecf.ElectronicCertificateFileId == electronicCertificateFileId
                            select new CertificateFile()
                            {
                                PdfFile = ecf.CertificatePdf.ToArray(),
                                Folio = ec.Folio
                            }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError(Constants.EcArea, Constants.DataAccess + " - ECCertificateDA - GetECPdfFile", e.Message + e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets the ec XML file.
        /// </summary>
        /// <param name="electronicCertificateFileId">The electronic certificate file identifier.</param>
        /// <returns></returns>
        public CertificateFile GetECXmlFile(int electronicCertificateFileId)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    return (from ec in context.ElectronicCertificates
                            join ecf in context.ElectronicCertificateFiles
                            on ec.ElectronicCertificateId equals ecf.ElectronicCertificateId
                            where ecf.ElectronicCertificateFileId == electronicCertificateFileId
                            select new CertificateFile()
                            {
                                XmlFile = ecf.CertificateXml.ToString(),
                                Folio = ec.Folio
                            }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, Constants.DataAccess + " - ECCertificateDA - GetECXmlFile", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Stamps the specified certificate operation.
        /// </summary>
        /// <param name="certificateOperation">The certificate operation.</param>
        /// <returns></returns>
        public bool Stamp(CertificateOperation certificateOperation)
        {
            try
            {
                if (certificateOperation.CertificateId <= 0 || certificateOperation.CertificateFileId <= 0)
                {
                    return false;
                }
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                    {
                        if (certificateOperation.CertificateId > 0 && !string.IsNullOrEmpty(certificateOperation.XmlFile))
                        {
                            ElectronicCertificateFile file =
                                context.ElectronicCertificateFiles.Single(x => x.ElectronicCertificateFileId == certificateOperation.CertificateFileId);
                            XElement xElement = XElement.Parse(certificateOperation.XmlFile);
                            file.CertificateXml = xElement;
                            file.RevisionDatetime = DateTime.Now;
                            file.RevisionOpId = certificateOperation.OperatorId;
                            context.SubmitChanges();

                            DataAccess.ElectronicCertificate certificate =
                                context.ElectronicCertificates.Single(x => x.ElectronicCertificateId == certificateOperation.CertificateId);
                            certificate.Status = CertificateStatus.Stamped;
                            certificate.RevisionDatetime = DateTime.Now;
                            certificate.RevisionOpId = certificateOperation.OperatorId;
                            context.SubmitChanges();

                            transaction.Complete();
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ECCertificateDA - Stamp", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Updates the ec certificate status.
        /// </summary>
        /// <param name="certificateOperation">The certificate operation.</param>
        /// <returns></returns>
        public bool UpdateECCertificateStatus(CertificateOperation certificateOperation)
        {
            try
            {
                bool result = false;
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                    {
                        result = UpdateCertificateStatus(context, certificateOperation);
                        if (result)
                            transaction.Complete();
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ECCertificateDA - UpdateECCertificateStatus", ex.Message);
                return false;
            }
        }

        #region Private

        /// <summary>
        /// Updates the certificate status.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="certificateOperation">The certificate operation.</param>
        /// <returns></returns>
        private bool UpdateCertificateStatus(ElectronicCertificateContext context, CertificateOperation certificateOperation)
        {
            try
            {
                DataAccess.ElectronicCertificate certificate =
                    context.ElectronicCertificates.Single(x => x.ElectronicCertificateId == certificateOperation.CertificateId);
                if (certificateOperation.Status == CertificateStatus.Canceled
                           || certificateOperation.Status == CertificateStatus.Error
                           || certificateOperation.Status == CertificateStatus.Processed)
                {
                    certificate.PaymentFolio = certificateOperation.PaymentFolio;
                    certificate.Note = certificateOperation.Notes;
                }
                else if (certificateOperation.Status == CertificateStatus.Stamped)
                {
                    certificate.Note = certificateOperation.Notes;
                    ElectronicCertificateFile file =
                    context.ElectronicCertificateFiles.Single(x => x.ElectronicCertificateFileId == certificateOperation.CertificateFileId);
                    if (!certificateOperation.GenerateXml)
                    {
                        if (!string.IsNullOrEmpty(certificateOperation.XmlFile))
                        {
                            string xml = Encoding.UTF8.GetString(Convert.FromBase64String(certificateOperation.XmlFile));
                            XElement xElement = XElement.Parse(xml);
                            file.CertificateXml = xElement;
                            file.RevisionDatetime = DateTime.Now;
                            file.RevisionOpId = certificateOperation.OperatorId;
                        }
                        else if (file.CertificateXml == null)
                            return false;
                    }
                }
                else
                {
                    return false;
                }
                if (certificateOperation.Status == CertificateStatus.Processed)
                {
                    ElectronicCertificateFile file =
                        context.ElectronicCertificateFiles.Single(x => x.ElectronicCertificateFileId == certificateOperation.CertificateFileId);
                    if (!string.IsNullOrEmpty(certificateOperation.XmlFile))
                    {
                        string xml = Encoding.UTF8.GetString(Convert.FromBase64String(certificateOperation.XmlFile));
                        XElement xElement = XElement.Parse(xml);
                        file.CertificateXml = xElement;
                        file.RevisionDatetime = DateTime.Now;
                        file.RevisionOpId = certificateOperation.OperatorId;
                    }
                    else if (file.CertificateXml == null)
                        return false;
                    if (!string.IsNullOrEmpty(certificateOperation.PdfFile))
                    {
                        byte[] pdf = Convert.FromBase64String(certificateOperation.PdfFile);
                        file.CertificatePdf = new System.Data.Linq.Binary(pdf);
                        file.RevisionDatetime = DateTime.Now;
                        file.RevisionOpId = certificateOperation.OperatorId;
                    }
                }
                certificate.RevisionDatetime = DateTime.Now;
                certificate.RevisionOpId = certificateOperation.OperatorId;
                certificate.Status = certificateOperation.Status;
                certificate.RevisionDatetime = DateTime.Now;
                certificate.RevisionOpId = certificateOperation.OperatorId;

                context.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ECCertificateDA - UpdateCertificateStatus", ex.Message);
                return false;
            }
        }

        #endregion Private
    }
}