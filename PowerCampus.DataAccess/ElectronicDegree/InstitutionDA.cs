// --------------------------------------------------------------------
// <copyright file="InstitutionDA.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.Entities.ElectronicDegree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerCampus.DataAccess.ElectronicDegree
{
    /// <summary>
    /// Institution DA
    /// </summary>
    public class InstitutionDA
    {
        /// <summary>
        /// Creates the institution.
        /// </summary>
        /// <param name="institutionDAModel">
        /// The institution da model.
        /// </param>
        public void CreateInstitution(InstitutionDAModel institutionDAModel)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    ElectronicDegreeInstitution institution = new ElectronicDegreeInstitution
                    {
                        Code = institutionDAModel.Code,
                        Name = institutionDAModel.Name,
                        Folio = institutionDAModel.Folio,
                        FolioFormat = institutionDAModel.FolioFormat,
                        CreateUserName = institutionDAModel.CreateUserName,
                        RevisionUserName = institutionDAModel.RevisionUserName,
                        CreateDatetime = DateTime.Now,
                        RevisionDatetime = DateTime.Now
                    };
                    context.ElectronicDegreeInstitutions.InsertOnSubmit(institution);
                    context.SubmitChanges();

                    int id = -1;
                    IEnumerable<int> query = from inst in context.ElectronicDegreeInstitutions
                                             where inst.Code == institutionDAModel.Code
                                             select inst.ElectronicDegreeInstitutionId;
                    id = query.First();

                    if (id > -1)
                    {
                        ElectronicDegreeMapping mapping = new ElectronicDegreeMapping()
                        {
                            ElectronicDegreeTable = "WebAdmin.ElectronicDegreeInstitutions",
                            PowerCampusTable = "dbo.INSTITUTION",
                            ElectronicDegreeValue = id.ToString(),
                            OrgCodeId = institutionDAModel.EquivalentId,
                            CreateDatetime = DateTime.Now,
                            CreateUserName = institutionDAModel.CreateUserName,
                            RevisionDatetime = DateTime.Now,
                            RevisionUserName = institutionDAModel.CreateUserName,
                        };
                        context.ElectronicDegreeMappings.InsertOnSubmit(mapping);
                        context.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA - CreateInstitution", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes the mapping.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void DeleteMapping(int id)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    ElectronicDegreeInstMajor query = (from edim in context.ElectronicDegreeInstMajors
                                                       where edim.ElectronicDegreeInstMajorId == id
                                                       select edim).FirstOrDefault();
                    if (query != null)
                    {
                        ElectronicDegreeMapping edmQuery = (from edm in context.ElectronicDegreeMappings
                                                            where edm.ElectronicDegreeValue == query.ElectronicDegreeInstMajorId.ToString() &&
                                                            edm.ElectronicDegreeTable == "WebAdmin.ElectronicDegreeInstMajor"
                                                            select edm).FirstOrDefault();
                        if (edmQuery != null)
                        {
                            context.ElectronicDegreeMappings.DeleteOnSubmit(edmQuery);
                            context.SubmitChanges();
                        }

                        context.ElectronicDegreeInstMajors.DeleteOnSubmit(query);
                        context.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA - RemoveMapping", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the institution.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        public InstitutionDAModel GetInstitution(int institutionId)
        {
            try
            {
                InstitutionDAModel institutionDAModel = null;
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                IQueryable<InstitutionDAModel> instQuery = from institution in context.ElectronicDegreeInstitutions
                                                           where institution.ElectronicDegreeInstitutionId == institutionId
                                                           orderby institution.Name
                                                           select new InstitutionDAModel()
                                                           {
                                                               Id = institution.ElectronicDegreeInstitutionId,
                                                               Code = institution.Code,
                                                               Name = institution.Name,
                                                               Folio = institution.Folio,
                                                               FolioFormat = institution.FolioFormat,
                                                           };
                if (instQuery != null)
                {
                    institutionDAModel = new InstitutionDAModel();
                    institutionDAModel = instQuery.Select(x => new InstitutionDAModel()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Folio = x.Folio,
                        FolioFormat = x.FolioFormat,
                    }).FirstOrDefault();

                    IEnumerable<string> query = from mapper in context.ElectronicDegreeMappings
                                                where mapper.ElectronicDegreeValue == institutionId.ToString()
                                                select mapper.OrgCodeId;

                    if (query.Any())
                        institutionDAModel.EquivalentId = query.First();

                    return institutionDAModel;
                }
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA", "GetInstitution");

                return null;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the institution by identifier.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        public InstitutionModel GetInstitutionById(int institutionId)
        {
            try
            {
                InstitutionModel institution = null;
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                IQueryable<InstitutionModel> query = from edi in context.ElectronicDegreeInstitutions
                                                     where edi.ElectronicDegreeInstitutionId == institutionId
                                                     select new InstitutionModel()
                                                     {
                                                         Name = edi.Name,
                                                         Code = edi.Code,
                                                     };
                if (query != null)
                {
                    institution = new InstitutionModel();
                    institution = query.Select(x => new InstitutionModel()
                    {
                        Name = x.Name,
                        Code = x.Code
                    }).FirstOrDefault();

                    institution.Majors = GetInstitutionsMajors(institutionId);
                }
                return institution;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the institutions.
        /// </summary>
        /// <returns></returns>
        public List<InstitutionModel> GetInstitutions()
        {
            try
            {
                List<InstitutionModel> institutions = null;
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                IQueryable<InstitutionModel> instQuery = from institution in context.ElectronicDegreeInstitutions
                                                         orderby institution.Name
                                                         select new InstitutionModel()
                                                         {
                                                             Id = institution.ElectronicDegreeInstitutionId,
                                                             Code = institution.Code,
                                                             Name = institution.Name,
                                                             Folio = institution.Folio,
                                                         };

                if (instQuery != null)
                {
                    institutions = new List<InstitutionModel>();
                    institutions = instQuery.Select(x => new InstitutionModel()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Folio = x.Folio,
                    }).ToList();

                    foreach (InstitutionModel institution in institutions)
                    {
                        IQueryable<string> mapping = from edInst in context.ElectronicDegreeInstitutions
                                                     join mapper in context.ElectronicDegreeMappings
                                                     on edInst.ElectronicDegreeInstitutionId.ToString() equals mapper.ElectronicDegreeValue
                                                     where edInst.ElectronicDegreeInstitutionId == institution.Id && mapper.ElectronicDegreeTable == "WebAdmin.ElectronicDegreeInstitutions" && mapper.PowerCampusTable == "dbo.INSTITUTION"
                                                     select mapper.OrgCodeId;
                        if (mapping.Any())
                        {
                            IQueryable<string> mappingValue = from pcInst in context.INSTITUTIONs
                                                              join pcOrg in context.ORGANIZATIONs
                                                              on pcInst.ORG_CODE_ID equals pcOrg.ORG_CODE_ID
                                                              where pcInst.ORG_CODE_ID == mapping.First()
                                                              select pcOrg.ORG_NAME_1;
                            if (mappingValue.Any())
                                institution.Equivalent = mappingValue.First();
                        }
                        institution.Majors = GetInstitutionsMajors(institution.Id);
                    }

                    return institutions;
                }
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA", "GetInstitutions");

                return null;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the institutions.
        /// </summary>
        /// <returns></returns>
        public List<InstitutionModel> GetInstitutionsDetail()
        {
            try
            {
                List<InstitutionModel> institutions = null;
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                IQueryable<InstitutionModel> instQuery = from institution in context.ElectronicDegreeInstitutions
                                                         orderby institution.Name
                                                         select new InstitutionModel()
                                                         {
                                                             Id = institution.ElectronicDegreeInstitutionId,
                                                             Code = institution.Code,
                                                             Name = institution.Name,
                                                             Folio = institution.Folio,
                                                         };

                if (instQuery != null)
                {
                    institutions = new List<InstitutionModel>();
                    institutions = instQuery.Select(x => new InstitutionModel()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        Folio = x.Folio,
                    }).ToList();

                    foreach (InstitutionModel institution in institutions)
                    {
                        IQueryable<string> mapping = from edInst in context.ElectronicDegreeInstitutions
                                                     join mapper in context.ElectronicDegreeMappings
                                                     on edInst.ElectronicDegreeInstitutionId.ToString() equals mapper.ElectronicDegreeValue
                                                     where edInst.ElectronicDegreeInstitutionId == institution.Id
                                                     select mapper.OrgCodeId;
                        if (mapping.Any())
                        {
                            IQueryable<string> mappingValue = from pcInst in context.INSTITUTIONs
                                                              join pcOrg in context.ORGANIZATIONs
                                                              on pcInst.ORG_CODE_ID equals pcOrg.ORG_CODE_ID
                                                              where pcInst.ORG_CODE_ID == mapping.First()
                                                              select pcOrg.ORG_NAME_1;
                            if (mappingValue.Any())
                                institution.Equivalent = mappingValue.First();
                        }
                        institution.Majors = GetInstitutionsMajors(institution.Id);

                        IQueryable<MajorList> majors = from majorList in context.ElectronicDegreeMajors
                                                       select new MajorList()
                                                       {
                                                           Code = majorList.Code,
                                                           ElectronicDegreeMajorId = majorList.ElectronicDegreeMajorId,
                                                           Name = majorList.Name,
                                                           StudyLevel = majorList.StudyLevel,
                                                           Selected = false
                                                       };
                        if (majors.Any())
                            institution.MajorList = majors.ToList();

                        IQueryable<int> mappingMajors = from instMajor in context.ElectronicDegreeInstMajors
                                                        where instMajor.ElectronicDegreeInstitutionId == institution.Id
                                                        select instMajor.ElectronicDegreeMajorId;
                        if (mappingMajors.Any())
                        {
                            foreach (MajorList majorList in institution.MajorList)
                            {
                                foreach (int majorId in mappingMajors.ToList())
                                {
                                    if (majorList.ElectronicDegreeMajorId == majorId)
                                        majorList.Selected = true;
                                }
                            }
                        }
                    }

                    return institutions;
                }
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA", "GetInstitutionsDetail");

                return null;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the institutions majors.
        /// </summary>
        /// <returns></returns>
        public int GetInstitutionsMajors(int institutionId)
        {
            try
            {
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                IQueryable<object> majorsQuery = from instMajors in context.ElectronicDegreeInstMajors
                                                 where instMajors.ElectronicDegreeInstitutionId == institutionId
                                                 select new
                                                 {
                                                     instMajors.ElectronicDegreeMajorId
                                                 };

                if (majorsQuery != null)
                    return majorsQuery.Count();

                return 0;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the institutions signers.
        /// </summary>
        /// <returns></returns>
        public int GetInstitutionsSigners(int institutionId)
        {
            try
            {
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                IQueryable<object> signersQuery = (from instSigners in context.ElectronicDegreeInstSigners
                                                   where instSigners.ElectronicDegreeInstitutionId == institutionId
                                                   orderby instSigners.ElectronicDegreeInstSignerId
                                                   select new
                                                   {
                                                       instSigners.SignerId
                                                   });

                if (signersQuery != null)
                    return signersQuery.Count();
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA", "GetInstitutionsSigners");

                return -1;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the majors by institution identifier.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        public List<MajorList> GetMajorsByInstitutionId(int institutionId)
        {
            try
            {
                List<MajorList> majors = null;
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                IQueryable<MajorList> majorsQuery = from edim in context.ElectronicDegreeInstMajors.Where(x => x.ElectronicDegreeInstitutionId == institutionId)
                                                    join edm in context.ElectronicDegreeMajors
                                                    on edim.ElectronicDegreeMajorId equals edm.ElectronicDegreeMajorId
                                                    select new MajorList()
                                                    {
                                                        ElectronicDegreeMajorId = edm.ElectronicDegreeMajorId,
                                                        Name = edm.Name,
                                                        Code = edm.Code,
                                                        StudyLevel = edm.StudyLevel,
                                                        ElectronicDegreeInstMajorId = edim.ElectronicDegreeInstMajorId
                                                    };
                if (majorsQuery != null)
                {
                    majors = new List<MajorList>();
                    majors = majorsQuery.Select(x => new MajorList()
                    {
                        ElectronicDegreeMajorId = x.ElectronicDegreeMajorId,
                        Name = x.Name,
                        Code = x.Code,
                        StudyLevel = x.StudyLevel,
                        AgreementNumber = x.AgreementNumber,
                        ElectronicDegreeInstMajorId = x.ElectronicDegreeInstMajorId
                    }).ToList();

                    foreach (MajorList major in majors)
                    {
                        IQueryable<RvoeList> rvoesQuery = from edm in context.ElectronicDegreeMappings.Where(x => x.ElectronicDegreeTable == "WebAdmin.ElectronicDegreeInstMajor" &&
                                                   x.PowerCampusTable == "dbo.Rvoe" && x.ElectronicDegreeValue == major.ElectronicDegreeInstMajorId.ToString())
                                                          join r in context.Rvoes on edm.RvoeId equals r.RvoeId
                                                          join rd in context.DEGREQs on r.DegreeRequirementId equals rd.DegreeRequirementId
                                                          join cc in context.CODE_CURRICULUMs on rd.CURRICULUM equals cc.CODE_VALUE
                                                          join ca in context.CODE_ACATERMs on rd.MATRIC_TERM equals ca.CODE_VALUE_KEY
                                                          select new RvoeList()
                                                          {
                                                              Description = string.Format("{0} - {1} ({2}/{3})", r.AgreementNumber, cc.LONG_DESC, rd.MATRIC_YEAR, ca.MEDIUM_DESC),
                                                              Rvoe = edm.RvoeId
                                                          };
                        if (rvoesQuery != null)
                        {
                            List<RvoeList> rvoeIds = new List<RvoeList>();
                            rvoeIds = rvoesQuery.Select(x => new RvoeList()
                            {
                                Description = x.Description,
                                Rvoe = x.Rvoe
                            }).ToList();
                            major.RvoeIds = rvoeIds;
                        }
                    }
                }
                return majors;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the power campus institutions.
        /// </summary>
        /// <returns></returns>
        public List<DropDownListModel> GetPowerCampusInstitutions()
        {
            try
            {
                List<DropDownListModel> listModel = null;
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                IQueryable<DropDownListModel> query = from powerCampusInstitution in context.INSTITUTIONs
                                                      join organization in context.ORGANIZATIONs
                                                      on powerCampusInstitution.ORG_CODE_ID equals organization.ORG_CODE_ID
                                                      orderby organization.ORG_NAME_1
                                                      select new DropDownListModel()
                                                      {
                                                          Description = organization.ORG_NAME_1 + "-" + organization.ORG_ID,
                                                          Value = powerCampusInstitution.ORG_CODE_ID,
                                                      };

                if (query != null)
                {
                    listModel = new List<DropDownListModel>();
                    listModel = query.Select(x => new DropDownListModel()
                    {
                        Description = x.Description,
                        Value = x.Value
                    }).ToList();

                    return listModel;
                }
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA", "GetPowerCampusInstitutions");

                return null;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the rvoe options.
        /// </summary>
        /// <returns></returns>
        public List<DropDownListModel> GetRvoeOptions()
        {
            try
            {
                List<DropDownListModel> options = null;
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                IQueryable<DropDownListModel> query = from r in context.Rvoes
                                                      join rd in context.DEGREQs on r.DegreeRequirementId equals rd.DegreeRequirementId
                                                      join cc in context.CODE_CURRICULUMs on rd.CURRICULUM equals cc.CODE_VALUE
                                                      join ca in context.CODE_ACATERMs on rd.MATRIC_TERM equals ca.CODE_VALUE_KEY
                                                      orderby r.AgreementNumber, cc.LONG_DESC
                                                      select new DropDownListModel()
                                                      {
                                                          Value = (int)r.RvoeId,
                                                          Description = string.Format("{0} - {1} ({2}/{3})", r.AgreementNumber, cc.LONG_DESC,
                                                          rd.MATRIC_YEAR, ca.MEDIUM_DESC)
                                                      };
                if (query != null)
                {
                    options = new List<DropDownListModel>();
                    options = query.Select(x => new DropDownListModel()
                    {
                        Value = x.Value,
                        Description = x.Description
                    }).ToList();
                }
                return options;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Maps the institution major rvoe.
        /// </summary>
        /// <param name="mapInstitutionMajorRvoe">The map institution major rvoe.</param>
        public void MapInstitutionMajorRvoe(MapInstitutionMajorRvoe mapInstitutionMajorRvoe)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    // Now insert new mappings
                    foreach (InstitutionMajorRvoe mapping in mapInstitutionMajorRvoe.institutionMajorRvoe)
                    {
                        if (mapping.Checked)
                        {
                            IQueryable<ElectronicDegreeMapping> selectQuery = from edm2 in context.ElectronicDegreeMappings
                                                                              where edm2.ElectronicDegreeTable == "WebAdmin.ElectronicDegreeInstMajor" &&
                                                                                edm2.PowerCampusTable == "dbo.Rvoe" &&
                                                                                edm2.ElectronicDegreeValue == mapping.ElectronicDegreeInstMajorId.ToString() &&
                                                                                edm2.RvoeId == mapping.RvoeId
                                                                              select edm2;
                            if (!selectQuery.Any())
                            {
                                ElectronicDegreeMapping edm = new ElectronicDegreeMapping
                                {
                                    ElectronicDegreeTable = "WebAdmin.ElectronicDegreeInstMajor",
                                    PowerCampusTable = "dbo.Rvoe",
                                    ElectronicDegreeValue = mapping.ElectronicDegreeInstMajorId.ToString(),
                                    RvoeId = mapping.RvoeId,
                                    CreateUserName = mapInstitutionMajorRvoe.UserName,
                                    CreateDatetime = DateTime.Now,
                                    RevisionUserName = mapInstitutionMajorRvoe.UserName,
                                    RevisionDatetime = DateTime.Now,
                                };
                                context.ElectronicDegreeMappings.InsertOnSubmit(edm);
                                context.SubmitChanges();
                            }
                        }
                        else
                        {
                            IQueryable<ElectronicDegreeMapping> query = from edm in context.ElectronicDegreeMappings
                                                                        select edm;

                            ElectronicDegreeMapping electronicDegreeMapping = query.FirstOrDefault();
                            if (electronicDegreeMapping != null)
                                context.ElectronicDegreeMappings.Where(x => x.ElectronicDegreeValue == mapping.ElectronicDegreeInstMajorId.ToString() &&
                                x.RvoeId == mapping.RvoeId &&
                                x.ElectronicDegreeTable == "WebAdmin.ElectronicDegreeInstMajor" &&
                                x.PowerCampusTable == "dbo.Rvoe").ToList().ForEach(context.ElectronicDegreeMappings.DeleteOnSubmit);

                            context.SubmitChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA - MapInstitutionMajorRvoe", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Maps the major.
        /// </summary>
        /// <param name="majorMappingModel">The major mapping model.</param>
        public void MapMajor(MajorMappingModel majorMappingModel)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    if (majorMappingModel.MajorIdList != null)
                    {
                        // Insert new mappings
                        foreach (int majorId in majorMappingModel.MajorIdList)
                        {
                            ElectronicDegreeInstMajor instMajor = new ElectronicDegreeInstMajor
                            {
                                ElectronicDegreeInstitutionId = majorMappingModel.InstitutionId,
                                ElectronicDegreeMajorId = majorId,
                                CreateDatetime = DateTime.Now,
                                CreateUserName = majorMappingModel.UserName,
                                RevisionDatetime = DateTime.Now,
                                RevisionUserName = majorMappingModel.UserName,
                            };
                            context.ElectronicDegreeInstMajors.InsertOnSubmit(instMajor);
                            context.SubmitChanges();
                        }
                    }
                    if (majorMappingModel.MajorDeleteIdList != null)
                    {
                        // Remove unselected mappings
                        foreach (int majorId in majorMappingModel.MajorDeleteIdList)
                        {
                            ElectronicDegreeInstMajor query = (from edim in context.ElectronicDegreeInstMajors
                                                               where edim.ElectronicDegreeMajorId == majorId && edim.ElectronicDegreeInstitutionId == majorMappingModel.InstitutionId
                                                               select edim).FirstOrDefault();
                            if (query != null)
                            {
                                ElectronicDegreeMapping edmQuery = (from edm in context.ElectronicDegreeMappings
                                                                    where edm.ElectronicDegreeValue == query.ElectronicDegreeInstMajorId.ToString() &&
                                                                    edm.ElectronicDegreeTable == "WebAdmin.ElectronicDegreeInstMajor"
                                                                    select edm).FirstOrDefault();
                                if (edmQuery != null)
                                {
                                    context.ElectronicDegreeMappings.DeleteOnSubmit(edmQuery);
                                    context.SubmitChanges();
                                }

                                context.ElectronicDegreeInstMajors.DeleteOnSubmit(query);
                                context.SubmitChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA - MapMajor", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the institution.
        /// </summary>
        /// <param name="institutionDAModel">The institution da model.</param>
        public void UpdateInstitution(InstitutionDAModel institutionDAModel)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    ElectronicDegreeInstitution institution =
                        context.ElectronicDegreeInstitutions.Single(m => m.ElectronicDegreeInstitutionId ==
                        institutionDAModel.Id);

                    institution.Code = institutionDAModel.Code;
                    institution.Name = institutionDAModel.Name;
                    institution.Folio = institutionDAModel.Folio;
                    institution.FolioFormat = institutionDAModel.FolioFormat;
                    institution.RevisionUserName = institutionDAModel.RevisionUserName;
                    institution.RevisionDatetime = DateTime.Now;

                    context.SubmitChanges();

                    if (institutionDAModel.EquivalentId != string.Empty)
                    {
                        int id = -1;
                        IEnumerable<int> query = from mapper in context.ElectronicDegreeMappings
                                                 where mapper.ElectronicDegreeValue == institutionDAModel.Id.ToString()
                                                 select mapper.ElectronicDegreeMappingId;

                        if (query.Any())
                            id = query.First();

                        if (id > -1)
                        {
                            ElectronicDegreeMapping mapping =
                                context.ElectronicDegreeMappings.Single(m => m.ElectronicDegreeMappingId == id);

                            mapping.OrgCodeId = institutionDAModel.EquivalentId;

                            context.SubmitChanges();
                        }
                    }
                    else
                    {
                        ElectronicDegreeMapping mapping = new ElectronicDegreeMapping
                        {
                            ElectronicDegreeTable = "WebAdmin.ElectronicDegreeInstitution",
                            PowerCampusTable = "dbo.INSTITUTION",
                            ElectronicDegreeValue = institutionDAModel.Id.ToString(),
                            OrgCodeId = institutionDAModel.EquivalentId,
                            CreateDatetime = DateTime.Now,
                            CreateUserName = institutionDAModel.CreateUserName
                        };
                        context.ElectronicDegreeMappings.InsertOnSubmit(mapping);
                        context.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA - UpdateInstitution", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the code.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns></returns>
        public bool ValidateCode(string code)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    ElectronicDegreeContext context = new ElectronicDegreeContext();
                    IQueryable<InstitutionModel> query = from institution in context.ElectronicDegreeInstitutions
                                                         where institution.Code == code
                                                         select new InstitutionModel()
                                                         {
                                                             Id = institution.ElectronicDegreeInstitutionId,
                                                             Code = institution.Code,
                                                             Name = institution.Name,
                                                             Folio = institution.Folio,
                                                         };
                    return query.Count() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA - ValidateCode", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the folio.
        /// </summary>
        /// <param name="folio">The folio.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public bool ValidateFolio(string folio, string code)
        {
            try
            {
                if (!string.IsNullOrEmpty(folio) && !string.IsNullOrEmpty(code))
                {
                    ElectronicDegreeContext context = new ElectronicDegreeContext();
                    IQueryable<InstitutionModel> query = from institution in context.ElectronicDegreeInstitutions
                                                         where institution.Folio == folio && institution.Code == code
                                                         select new InstitutionModel()
                                                         {
                                                             Id = institution.ElectronicDegreeInstitutionId,
                                                             Code = institution.Code,
                                                             Name = institution.Name,
                                                             Folio = institution.Folio,
                                                         };
                    return query.Count() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA - ValidateFolio", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns></returns>
        public bool ValidateName(string name)
        {
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    ElectronicDegreeContext context = new ElectronicDegreeContext();
                    IQueryable<InstitutionModel> query = from institution in context.ElectronicDegreeInstitutions
                                                         where institution.Name == name
                                                         select new InstitutionModel()
                                                         {
                                                             Id = institution.ElectronicDegreeInstitutionId,
                                                             Code = institution.Code,
                                                             Name = institution.Name,
                                                             Folio = institution.Folio,
                                                         };
                    return query.Count() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - InstitutionDA - ValidateName", ex.Message);
                throw;
            }
        }
    }
}