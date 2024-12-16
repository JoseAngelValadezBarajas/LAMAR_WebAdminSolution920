// --------------------------------------------------------------------
// <copyright file="ElectronicDegreeInformationDA.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.Entities.ElectronicDegree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PowerCampus.DataAccess.ElectronicDegree
{
    /// <summary>
    /// Electronic Degree Information DA
    /// </summary>
    public class ElectronicDegreeInformationDA
    {
        /// <summary>
        /// Deletes the electronic degree.
        /// </summary>
        /// <param name="electronicDegreeId">The electronic degree identifier.</param>
        public void DeleteElectronicDegree(int electronicDegreeId)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    ElectronicDegreeInformation edi = context.ElectronicDegreeInformations.FirstOrDefault(e => e.ElectronicDegreeInformationId == electronicDegreeId);
                    if (edi != null)
                    {
                        edi.IsActive = false;
                        context.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - ElectronicDegreeInformationDA - DeleteElectronicDegree", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the cancelation catalog.
        /// </summary>
        /// <returns></returns>
        public List<CodeCancelationCatalog> GetCancelationCatalog()
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    return (from canCat in context.CODE_DEGREECANCELATIONs
                            select new CodeCancelationCatalog()
                            {
                                ShortDesc = canCat.SHORT_DESC,
                                LongDesc = canCat.LONG_DESC
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - ElectronicDegreeInformationDA - GetCancelationCatalog", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the electronic degree information request.
        /// </summary>
        /// <param name="electronicDegreeId">The electronic degree identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ElectronicDegreeInfoRequest GetElectronicDegreeInfoRequest(int electronicDegreeId)
        {
            using (ElectronicDegreeContext context = new ElectronicDegreeContext())
            {
                ElectronicDegreeInfoRequest query =
                    (from edr in context.ElectronicDegreeRequests
                     where edr.ElectronicDegreeInformationId == electronicDegreeId
                     select new ElectronicDegreeInfoRequest
                     {
                         BatchNumber = edr.BatchNumber,
                         CreateDatetime = edr.CreateDatetime,
                         CreateUserName = edr.CreateUserName,
                         ElectronicDegreeRequestId = edr.ElectronicDegreeRequestId,
                         ElectronicDegreeInformationId = edr.ElectronicDegreeInformationId,
                         RequestXML = edr.RequestXml.ToString(),
                         ResponseMessage = edr.ResponseMessage,
                         RevisionDatetime = edr.RevisionDatetime,
                         Status = edr.Status[0]
                     }).SingleOrDefault();

                return query;
            }
        }

        /// <summary>
        /// Gets the electronic degree information.
        /// </summary>
        /// <param name="folio">The folio.</param>
        /// <param name="student">The student.</param>
        /// <param name="degreeType">Type of the degree.</param>
        /// <param name="major">The major.</param>
        /// <returns></returns>
        public List<ElectronicDegreeInfo> GetElectronicDegreeInformation(string folio, string student, string degreeType, string major)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    List<ElectronicDegreeInfo> electronicDegreeInfoList = null;
                    IQueryable<ElectronicDegreeInfo> query;
                    if (folio == "*" && student == "*" && degreeType == "*" && major == "*")
                    {
                        query = from edi in context.ElectronicDegreeInformations
                                where edi.IsActive == true
                                orderby edi.CreateDatetime descending
                                select new ElectronicDegreeInfo()
                                {
                                    Folio = edi.Folio,
                                    Student = new ElectronicDegreeStudent()
                                    {
                                        DisplayName = edi.Name + " " + edi.FirstSurname + " " + (edi.SecondSurname ?? string.Empty),
                                        PeopleCodeId = edi.PeopleCodeId
                                    },
                                    Major = edi.Major,
                                    MajorCode = edi.MajorCode,
                                    EducationLevel = edi.StudyLevel,
                                    ElectronicDegreeInformationId = edi.ElectronicDegreeInformationId,
                                    Status = edi.Status,
                                    RequestXML = edi.XMLRequest != null ? edi.XMLRequest.ToString() : string.Empty
                                    
                                };
                    }
                    else
                    {
                        query = from edi in context.ElectronicDegreeInformations
                                where (edi.Folio.Contains(folio) || folio.Length == 0 || folio == null)
                                      && (edi.FirstSurname.Contains(student)
                                          || edi.Name.Contains(student)
                                          || edi.PEOPLE.DisplayName.Contains(student)
                                          || student.Length == 0
                                          || student == null)
                                      && (edi.Major == major
                                          || edi.MajorCode.Contains(major)
                                          || major.Length == 0
                                          || major == null)
                                     && (edi.StudyLevel.Contains(degreeType)
                                          || edi.StudyLevel.Contains(degreeType)
                                          || degreeType.Length == 0
                                          || degreeType == null)
                                           && edi.IsActive == true
                                orderby edi.CreateDatetime descending
                                select new ElectronicDegreeInfo()
                                {
                                    Folio = edi.Folio,
                                    Student = new ElectronicDegreeStudent()
                                    {
                                        DisplayName = edi.Name + " " + edi.FirstSurname + " " + edi.SecondSurname,
                                        PeopleCodeId = edi.PeopleCodeId
                                    },
                                    Major = edi.Major,
                                    MajorCode = edi.MajorCode,
                                    EducationLevel = edi.StudyLevel,
                                    ElectronicDegreeInformationId = edi.ElectronicDegreeInformationId,
                                    Status = edi.Status,
                                    RequestXML = edi.XMLRequest != null ? edi.XMLRequest.ToString() : string.Empty
                                 
                                };
                    }

                    if (query != null)
                    {
                        electronicDegreeInfoList = new List<ElectronicDegreeInfo>();
                        electronicDegreeInfoList = query.Select(x => new ElectronicDegreeInfo()
                        {
                            Folio = x.Folio,
                            Student = x.Student,
                            Major = x.Major,
                            MajorCode = x.MajorCode,
                            EducationLevel = x.EducationLevel,
                            ElectronicDegreeInformationId = x.ElectronicDegreeInformationId,
                            Status = x.Status,
                            RequestXML = x.RequestXML
                        
                        }).ToList();
                    }
                    return electronicDegreeInfoList;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - ElectronicDegreeInformationDA - GetElectronicDegreeInformation", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the electronic degree information.
        /// </summary>
        /// <param name="electronicDegreeId">The electronic degree identifier.</param>
        /// <returns></returns>
        public ElectronicDegreeInfo GetElectronicDegreeInformation(int electronicDegreeId)
        {
            using (ElectronicDegreeContext context = new ElectronicDegreeContext())
            {
                var query =
                    (from edi in context.ElectronicDegreeInformations
                     join edr in context.ElectronicDegreeRequests
                        on edi.ElectronicDegreeInformationId equals edr.ElectronicDegreeInformationId
                     into edir
                     from x in edir.DefaultIfEmpty()
                     where edi.ElectronicDegreeInformationId == electronicDegreeId
                     select new
                     {
                         edi,
                         x.RequestXml
                     
                     }).SingleOrDefault();
                if (query == null) return null;

                return MapObject(query.edi, query.RequestXml);
            }
        }

      

        /// <summary>
        /// Inserts the electronic degree information request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public int InsertElectronicDegreeInformationRequest(ElectronicDegreeInfoRequest request)
        {
            using (ElectronicDegreeContext context = new ElectronicDegreeContext())
            {
                int? id = 0;
                id = context.spInsElectronicDegreeRequest(
                    request.ElectronicDegreeInformationId,
                    XElement.Parse(request.RequestXML),                     
                    request.CreateUserName,
                    request.BatchNumber,
                    request.ResponseMessage,
                    ref id);

                return id.Value;
            }
        }

        /// <summary>
        /// Updates the electronic degree information request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public void UpdateElectronicDegreeInformationRequest(ElectronicDegreeInfoRequest request)
        {
            using (ElectronicDegreeContext context = new ElectronicDegreeContext())
            {
                context.spUpdElectronicDegreeRequest(
                    request.ElectronicDegreeInformationId,
                    XElement.Parse(request.RequestXML),
                    request.Status.ToString(),
                    request.BatchNumber,
                    request.ResponseMessage,
                    request.CancellationReason);
            }
        }

        /// <summary>
        /// Updates the electronic degree information status.
        /// </summary>
        /// <param name="electronicDegreeId">The electronic degree identifier.</param>
        /// <param name="status">The status.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void UpdateElectronicDegreeInfoStatus(int electronicDegreeId, char status)
        {
            using (ElectronicDegreeContext context = new ElectronicDegreeContext())
            {
                ElectronicDegreeInformation edi = context.ElectronicDegreeInformations.FirstOrDefault(e => e.ElectronicDegreeInformationId == electronicDegreeId);
                if (edi != null)
                {
                    edi.Status = status.ToString();
                    edi.RevisionDatetime = DateTime.Now;
                    context.SubmitChanges();
                }
            }
        }

        /// <summary>
        /// Maps the object for Electronic Degree Information
        /// </summary>
        /// <param name="edi">The edi.</param>
        /// <param name="xmlRequest">The XML request.</param>
        /// <returns></returns>
        private ElectronicDegreeInfo MapObject(ElectronicDegreeInformation edi, XElement xmlRequest)
        {
            return new ElectronicDegreeInfo()
            {
                Folio = edi.Folio,
                Student = new ElectronicDegreeStudent()
                {
                    DisplayName = edi.PEOPLE.DisplayName,
                    PeopleCodeId = edi.PeopleCodeId,
                    Curp = edi.Curp,
                    Email = edi.Email,
                    FirstSurname = edi.FirstSurname,
                    Name = edi.Name,
                    SecondSurname = edi.SecondSurname
                },
                AuthorizationType = edi.AuthorizationType,
                AuthorizationTypeCode = edi.AuthorizationTypeCode,
                BackgroundStudyEndDate = edi.BackgroundStudyEndDate.ToString("yyyy-MM-dd"),
                BackgroundStudyStartDate = edi.BackgroundStudyStartDate?.ToString("yyyy-MM-dd"),
                BackgroundStudyType = edi.BackgroundStudyType,
                BackgroundStudyTypeCode = edi.BackgroundStudyTypeCode,
                EducationLevel = "missing",
                ExaminationDate = edi.ExaminationDate?.ToString("yyyy-MM-dd"),
                ExaminationExemptionDate = edi.ExaminationExemptionDate?.ToString("yyyy-MM-dd"),
                ExpeditionDate = edi.ExpeditionDate.ToString("yyyy-MM-dd"),
                FederalEntity = edi.FederalEntity,
                FederalEntityCode = edi.FederalEntityCode,
                FulfilledSocialService = edi.FulfilledSocialService,
                GraduationRequirement = edi.GraduationRequirement,
                GraduationRequirementCode = edi.GraduationRequirementCode,
                InstitutionCode = edi.InstitutionCode,
                InstitutionName = edi.InstitutionName,
                LegalBase = edi.LegalBase,
                LegalBaseCode = edi.LegalBaseCode,
                LicenseNumber = edi.LicenseNumber,
                Major = edi.Major,
                MajorCode = edi.MajorCode,
                MajorEndDate = edi.MajorEndDate.ToString("yyyy-MM-dd"),
                MajorStartDate = edi.MajorStartDate?.ToString("yyyy-MM-dd"),
                OriginInstFederalEntity = edi.OriginInstFederalEntity,
                OriginInstFederalEntityCode = edi.OriginInstFederalEntityCode,
                OriginInstitution = edi.OriginInstitution,
                RvoeAgreementNumber = edi.RvoeAgreementNumber,
                RequestXML = xmlRequest != null ? xmlRequest.ToString() : edi.XMLRequest != null ? edi.XMLRequest.ToString() : string.Empty,
                OriginalString = edi.OriginalString,
                //RequestXML_Stamp = xmlRequest != null ? xmlRequest.ToString() : edi.XMLRequest_Stamp != null ? edi.XMLRequest_Stamp.ToString() : string.Empty,
                Signer = edi.ElectronicDegreeInfoSigners.Select(s => new InstitutionSignerList
                {
                    EdAbreviationTitle = s.AbbreviationTitle,
                    EdSignerAbreviationCode = s.AbbreviationTitleCode.Value,
                    EdSignerCurp = s.Curp,
                    EdSignerFirstSurname = s.FirstSurname,
                    EdName = s.Name,
                    EdSignerName = s.Name,
                    EdSignerSecondSurname = s.SecondSurname,
                    EdSignerLaborPosition = s.SignerPosition,
                    EdSignerLaborPositionCode = s.SignerPositionCode,
                    EdSignerThumprint = s.Thumbprint
                }).ToList()
            };
        }
    }
}