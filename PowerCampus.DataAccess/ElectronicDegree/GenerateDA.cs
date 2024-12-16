// --------------------------------------------------------------------
// <copyright file="GenerateDA.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.Entities.ElectronicDegree;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace PowerCampus.DataAccess.ElectronicDegree
{
    /// <summary>
    /// GenerateDA
    /// </summary>
    public class GenerateDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        ///GenerateDA
        /// </summary>
        public GenerateDA() => _factory = new DatabaseProviderFactory();

        /// <summary>
        /// Inserts the electronic degree information.
        /// </summary>
        /// <param name="electronicDegreeInfo">The electronic degree information.</param>
        /// <returns></returns>
        public int Create(ElectronicDegreeInfo electronicDegreeInfo)
        {
            try
            {
                Database database = _factory.CreateDefault();
                int electronicDegreeInformationId = 0;
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    connection.Open();
                    DbTransaction trans = connection.BeginTransaction();
                    try
                    {
                        DbCommand insertElectronicDegree = database.GetStoredProcCommand("WebAdmin.spInsElectronicDegreeInformation");
                        database.AddOutParameter(insertElectronicDegree, "@ElectronicDegreeInformationId", DbType.Int32, 0);
                        database.AddInParameter(insertElectronicDegree, "@PeopleId", DbType.String, electronicDegreeInfo.PeopleCodeId);
                        database.AddInParameter(insertElectronicDegree, "@Version", DbType.String, "1.0");
                        database.AddInParameter(insertElectronicDegree, "@StudyLevel", DbType.String, electronicDegreeInfo.EducationLevel);
                        database.AddInParameter(insertElectronicDegree, "@Folio", DbType.String, electronicDegreeInfo.Folio);
                        database.AddInParameter(insertElectronicDegree, "@InstitutionCode", DbType.String, electronicDegreeInfo.InstitutionCode);
                        database.AddInParameter(insertElectronicDegree, "@InstitutionName", DbType.String, electronicDegreeInfo.InstitutionName);
                        database.AddInParameter(insertElectronicDegree, "@MajorCode", DbType.String, electronicDegreeInfo.MajorCode);
                        database.AddInParameter(insertElectronicDegree, "@Major", DbType.String, electronicDegreeInfo.Major);
                        database.AddInParameter(insertElectronicDegree, "@MajorStartDate", DbType.Date, !string.IsNullOrEmpty(electronicDegreeInfo.MajorStartDate) ? electronicDegreeInfo.MajorStartDate : null);
                        database.AddInParameter(insertElectronicDegree, "@MajorEndDate", DbType.Date, DateTime.Parse(electronicDegreeInfo.MajorEndDate, System.Globalization.CultureInfo.InvariantCulture));
                        database.AddInParameter(insertElectronicDegree, "@AuthorizationTypeCode", DbType.Int32, electronicDegreeInfo.AuthorizationTypeCode);
                        database.AddInParameter(insertElectronicDegree, "@AuthorizationType", DbType.String, electronicDegreeInfo.AuthorizationType);
                        database.AddInParameter(insertElectronicDegree, "@RvoeAgreementNumber", DbType.String, electronicDegreeInfo.RvoeAgreementNumber);
                        database.AddInParameter(insertElectronicDegree, "@Curp", DbType.String, electronicDegreeInfo.Curp);
                        database.AddInParameter(insertElectronicDegree, "@Name", DbType.String, electronicDegreeInfo.Name);
                        database.AddInParameter(insertElectronicDegree, "@FirstSurname", DbType.String, electronicDegreeInfo.FirstSurname);
                        database.AddInParameter(insertElectronicDegree, "@SecondSurname", DbType.String, electronicDegreeInfo.SecondSurname);
                        database.AddInParameter(insertElectronicDegree, "@Email", DbType.String, electronicDegreeInfo.Email);
                        database.AddInParameter(insertElectronicDegree, "@ExpeditionDate", DbType.Date, DateTime.Parse(electronicDegreeInfo.ExpeditionDate, System.Globalization.CultureInfo.InvariantCulture));
                        database.AddInParameter(insertElectronicDegree, "@GraduationRequirementCode", DbType.Int32, electronicDegreeInfo.GraduationRequirementCode);
                        database.AddInParameter(insertElectronicDegree, "@GraduationRequirement", DbType.String, electronicDegreeInfo.GraduationRequirement);
                        database.AddInParameter(insertElectronicDegree, "@GraduationRequirementDate ", DbType.Date, DateTime.Parse(electronicDegreeInfo.ExaminationExemptionDate, System.Globalization.CultureInfo.InvariantCulture));
                        database.AddInParameter(insertElectronicDegree, "@FulfilledSocialService", DbType.Boolean, electronicDegreeInfo.FulfilledSocialService);
                        database.AddInParameter(insertElectronicDegree, "@LegalBaseCode", DbType.Int32, electronicDegreeInfo.LegalBaseCode);
                        database.AddInParameter(insertElectronicDegree, "@LegalBase", DbType.String, electronicDegreeInfo.LegalBase);
                        database.AddInParameter(insertElectronicDegree, "@FederalEntityCode", DbType.String, electronicDegreeInfo.FederalEntityCode);
                        database.AddInParameter(insertElectronicDegree, "@FederalEntity", DbType.String, electronicDegreeInfo.FederalEntity);
                        database.AddInParameter(insertElectronicDegree, "@OriginInstitution", DbType.String, electronicDegreeInfo.OriginInstitution);
                        database.AddInParameter(insertElectronicDegree, "@BackgroundStudyTypeCode", DbType.Int32, electronicDegreeInfo.BackgroundStudyTypeCode);
                        database.AddInParameter(insertElectronicDegree, "@BackgroundStudyType", DbType.String, electronicDegreeInfo.BackgroundStudyType);
                        database.AddInParameter(insertElectronicDegree, "@OriginInstFederalEntityCode", DbType.String, electronicDegreeInfo.OriginInstFederalEntityCode);
                        database.AddInParameter(insertElectronicDegree, "@OriginInstFederalEntity", DbType.String, electronicDegreeInfo.OriginInstFederalEntity);
                        database.AddInParameter(insertElectronicDegree, "@BackgroundStudyStartDate", DbType.Date, !string.IsNullOrEmpty(electronicDegreeInfo.BackgroundStudyStartDate) ? electronicDegreeInfo.BackgroundStudyStartDate : null);
                        database.AddInParameter(insertElectronicDegree, "@BackgroundStudyEndDate", DbType.Date, !string.IsNullOrEmpty(electronicDegreeInfo.BackgroundStudyEndDate) ? electronicDegreeInfo.BackgroundStudyEndDate : null);
                        database.AddInParameter(insertElectronicDegree, "@LicenseNumber", DbType.String, electronicDegreeInfo.LicenseNumber);
                        database.AddInParameter(insertElectronicDegree, "@Status", DbType.String, "G");
                        database.AddInParameter(insertElectronicDegree, "@CreateUserName", DbType.String, electronicDegreeInfo.CreateUserName);
                        database.ExecuteNonQuery(insertElectronicDegree, trans);
                        electronicDegreeInformationId = (int)database.GetParameterValue(insertElectronicDegree, "@ElectronicDegreeInformationId");

                        if (electronicDegreeInformationId > 0)
                        {
                            foreach (InstitutionSignerList electronicDegreeInfoSigner in electronicDegreeInfo.Signer)
                            {
                                DbCommand insertSignerInformation = database.GetStoredProcCommand("WebAdmin.spInsElectronicDegreeInfoSigner");
                                database.AddInParameter(insertSignerInformation, "@ElectronicDegreeInformationId", DbType.Int32, electronicDegreeInformationId);
                                database.AddInParameter(insertSignerInformation, "@Name", DbType.String, electronicDegreeInfoSigner.EdName);
                                database.AddInParameter(insertSignerInformation, "@Curp", DbType.String, electronicDegreeInfoSigner.EdSignerCurp);
                                database.AddInParameter(insertSignerInformation, "@FirstSurname", DbType.String, electronicDegreeInfoSigner.EdSignerFirstSurname);
                                database.AddInParameter(insertSignerInformation, "@SecondSurname", DbType.String, electronicDegreeInfoSigner.EdSignerSecondSurname);
                                database.AddInParameter(insertSignerInformation, "@SignerPositionCode", DbType.Int32, electronicDegreeInfoSigner.EdSignerLaborPositionCode);
                                database.AddInParameter(insertSignerInformation, "@SignerPosition", DbType.String, electronicDegreeInfoSigner.EdSignerLaborPosition);
                                database.AddInParameter(insertSignerInformation, "@AbbreviationTitleCode", DbType.Int32, electronicDegreeInfoSigner.EdSignerAbreviationCode);
                                database.AddInParameter(insertSignerInformation, "@AbbreviationTitle", DbType.String, electronicDegreeInfoSigner.EdAbreviationTitle);
                                database.AddInParameter(insertSignerInformation, "@Thumbprint", DbType.String, electronicDegreeInfoSigner.EdSignerThumprint);
                                database.AddInParameter(insertSignerInformation, "@Certificate", DbType.String, string.Empty);
                                database.AddInParameter(insertSignerInformation, "@CertificateNumber", DbType.String, string.Empty);
                                database.ExecuteNonQuery(insertSignerInformation, trans);
                            }
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - GenerateDA - Create", ex.Message);
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                return electronicDegreeInformationId;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - GenerateDA - Create", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the background studies.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <returns></returns>
        public List<BackgroundStudies> GetBackgroundStudies(string peopleId)
        {
            try
            {
                DataSet backgroundStudiesDataSet = new DataSet();
                Database database = _factory.CreateDefault();
                List<BackgroundStudies> backgroundStudies = new List<BackgroundStudies>();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelBackgroundStudies");
                    database.AddInParameter(command, "@PeopleId", DbType.String, peopleId);
                    database.LoadDataSet(command, backgroundStudiesDataSet, "webAdmin");

                    if (backgroundStudiesDataSet.Tables[0].Rows.Count > 0)
                    {
                        backgroundStudies = backgroundStudiesDataSet.Tables[0].AsEnumerable().Select(m => new BackgroundStudies()
                        {
                            InstitutionNameOrigin = m.Field<string>("InstitutionNameOrigin"),
                            EndDate = !string.IsNullOrEmpty(m.Field<DateTime?>("EndDate").ToString()) ? m.Field<DateTime>("EndDate").ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) : string.Empty,
                            StartDate = !string.IsNullOrEmpty(m.Field<DateTime?>("StartDate").ToString()) ? m.Field<DateTime>("StartDate").ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) : string.Empty,
                            State = !string.IsNullOrEmpty(m.Field<string>("State")) ? m.Field<string>("State") : string.Empty,
                            StateCode = m.Field<int?>("StateCode"),
                            StateDesc = m.Field<string>("StateDesc"),
                            LicenceNumber = m.Field<string>("LicenceNumber"),
                            LevelStudy = !string.IsNullOrEmpty(m.Field<string>("LevelStudy")) ? m.Field<string>("LevelStudy") : string.Empty //mod 13082024
                        }).ToList();

                        backgroundStudies[0].BackgroundStudiesCatalog = backgroundStudiesDataSet.Tables[1].AsEnumerable().Select(m => new BackgroundStudyCatalog()
                        {
                            BackgroundStudyTypeCode = m.Field<int>("BackgroundStudyTypeCode"),
                            BackgroundStudyTypeDesc = m.Field<string>("BackgroundStudyTypeDesc")
                        }).ToList();
                    }

                    return backgroundStudies;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - GenerateDA - GetBackgroundStudies", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the institution folio.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="ElectronicDegreeInstMajorId">The electronic degree inst major identifier.</param>
        /// <returns></returns>
        public string GetInstitutionFolio(string peopleId, int ElectronicDegreeInstMajorId)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                    return context.spSelInstitutionFolio(peopleId, ElectronicDegreeInstMajorId).FirstOrDefault()?.Folio;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - GenerateDA - GetInstitutionFolio", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the institution major.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public List<InstitutionMajor> GetInstitutionMajor(string peopleId, string operatorId)
        {
            try
            {
                peopleId = Regex.Replace(peopleId, "[^0-9]", "");
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    return context.spSelInstitutionMajor(peopleId, operatorId).Select(m => new InstitutionMajor()
                    {
                        MajorCode = !string.IsNullOrEmpty(m.MajorCode) ? m.MajorCode : string.Empty,
                        MajorName = !string.IsNullOrEmpty(m.MajorName) ? m.MajorName : string.Empty,
                        NumberOfSigners = m.NumberOfSigners ?? 0,
                        InstitutionName = !string.IsNullOrEmpty(m.InstitutionName) ? m.InstitutionName : string.Empty,
                        InstitutionCode = !string.IsNullOrEmpty(m.InstitutionCode) ? m.InstitutionCode : string.Empty,
                        ProgramDesc = m.ProgramDesc,
                        AuthorizationCode = m.AuthorizationCode ?? 0,
                        AuthorizationType = m.AuthorizationType,
                        RvoeAgreementNumber = m.RvoeAgreementNumber,
                        IsOperatorOfInstitution = m.IsOperatorOfInstitution,
                        ElectronicDegreeInstitutionId = m.ElectronicDegreeInstitutionId ?? 0,
                        ProgramEndDate = m.ProgramEndDate?.ToString("yyyy-MM-dd"),
                        ProgramStartDate = m.ProgramStartDate?.ToString("yyyy-MM-dd"),
                        HasPCOrganization = m.HasPCOrganization,
                        TranscriptDegreeId = m.TranscriptDegreeId,
                        ElectronicDegreeInstMajorId = m.ElectronicDegreeInstMajorId ?? 0,
                        StudyLevel = m.StudyLevel,
                        MatricYear = m.MatricYear,
                        TermDesc = m.TermDesc
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - GenerateDA - GetInstitutionMajor", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the institution signer.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        public List<InstitutionSignerList> GetInstitutionSigner(int institutionId)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    return context.spSelInstitutionSigners(institutionId).Select(m => new InstitutionSignerList()
                    {
                        EdSignerAbreviationCode = m.AbreviationCode,
                        EdAbreviationTitle = m.Abreviation,
                        EdSignerName = m.Name,
                        EdName = m.Name,
                        EdSignerFirstSurname = m.FirstSurname,
                        EdSignerSecondSurname = m.SecondSurname,
                        EdSignerCurp = m.Curp,
                        EdSignerLaborPosition = m.LaborPosition,
                        EdSignerLaborPositionCode = m.LaborPositionCode,
                        EdSignerThumprint = m.Thumbprint
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - GenerateDA - GetInstitutionSigner", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get the Issuing Degree
        /// </summary>
        /// <param name="peopleId"></param>
        /// <param name="insMajorId"></param>
        /// <param name="transcriptDegreeId"></param>
        public IssuingDegree GetIssuingDegree(string peopleId, int insMajorId, int transcriptDegreeId)
        {
            try
            {
                DataSet issuingDegreeDataSet = new DataSet();
                Database database = _factory.CreateDefault();
                IssuingDegree issuingDegree = new IssuingDegree();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelIssuingDegree");
                    database.AddInParameter(command, "@PeopleId", DbType.String, peopleId);
                    database.AddInParameter(command, "@ElectronicDegreeInstMajorId", DbType.String, insMajorId);
                    database.AddInParameter(command, "@TranscriptDegreeId", DbType.String, transcriptDegreeId);
                    database.LoadDataSet(command, issuingDegreeDataSet, "webAdmin");

                    if (issuingDegreeDataSet.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = issuingDegreeDataSet.Tables[0].Rows[0];
                        issuingDegree = new IssuingDegree
                        {
                            IssuingDate = row.Field<DateTime>("IssuingDate").ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                            GraduationRequirementCode = row.Field<int>("GraduationRequirementCode"),
                            GraduationRequirementDesc = row.Field<string>("GraduationRequirementDesc"),
                            ProfExaminationDate = row.Field<DateTime>("ProfExaminationDate").ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                            ExemptionProfExaminationDate = row.Field<DateTime>("ExemptionProfExaminationDate").ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                            HasService = row.Field<bool>("HasService"),
                            ShowService = row.Field<bool>("ShowService"),
                            State = row.Field<string>("State"),
                            StateCode = row.Field<string>("StateCode"),
                            StateDesc = row.Field<string>("StateDesc"),
                            LegalBaseCode = row.Field<int?>("LegalBaseCode"),
                            LegalBaseDesc = row.Field<string>("LegalBaseDesc")
                        };

                        issuingDegree.GraduationCatalog = issuingDegreeDataSet.Tables[1].AsEnumerable().Select(m => new GraduationRequirementCatalog()
                        {
                            GraduationRequirementCode = m.Field<int>("GraduationRequirementCode"),
                            GraduationRequirementDesc = m.Field<string>("GraduationRequirementDesc")
                        }).ToList();

                        issuingDegree.StateCatalog = issuingDegreeDataSet.Tables[2].AsEnumerable().Where(m => m.Field<string>("StateCode") != "33").Select(m => new StateCatalog()
                        {
                            StateCode = m.Field<string>("StateCode"),
                            StateDesc = m.Field<string>("StateDesc")
                        }).ToList();

                        issuingDegree.StateCatalogBS = issuingDegreeDataSet.Tables[2].AsEnumerable().Select(m => new StateCatalog()
                        {
                            StateCode = m.Field<string>("StateCode"),
                            StateDesc = m.Field<string>("StateDesc")
                        }).ToList();
                    }

                    return issuingDegree;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - GenerateDA - GetIssuingDegree", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the people.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <returns></returns>
        public List<PeopleModel> GetPeople(string peopleId)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    return context.spSelStudentInformation(peopleId).Select(m => new PeopleModel()
                    {
                        BirthDate = m.BirthDate,
                        Curp = m.Curp,
                        FirstSurname = m.ApellidoPaterno,
                        GenderIdentity = m.Gender,
                        SecondSurname = m.ApellidoMaterno,
                        Email = m.Email,
                        PeopleCodeId = m.PeopleId,
                        Name = m.Nombre
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - GenerateDA - GetPeople", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="originalString">The original string.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public bool UpdateXml(string xml, string originalString, int id)
        {
            try
            {
                Database database = _factory.CreateDefault();
                bool success = false;
                var base64xml = Convert.FromBase64String(xml);
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spUpdElectronicDegreeInformation");
                    database.AddInParameter(command, "@XMLRequest", DbType.Binary, base64xml);
                    database.AddInParameter(command, "@OriginalString", DbType.String, originalString);
                    database.AddInParameter(command, "@ElectronicDegreeInformationId", DbType.Int32, id);
                    database.ExecuteNonQuery(command);
                    success = true;
                    return success;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - GenerateDA - UpdateXml", ex.Message);
                throw;
            }
        }
    }
}