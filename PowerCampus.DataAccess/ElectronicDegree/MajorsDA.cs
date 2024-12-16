// --------------------------------------------------------------------
// <copyright file="MajorsDA.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.Entities;
using PowerCampus.Entities.ElectronicDegree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerCampus.DataAccess.ElectronicDegree
{
    /// <summary>
    /// MajorsDA
    /// </summary>
    public class MajorsDA
    {
        /// <summary>
        /// Creates the major.
        /// </summary>
        /// <param name="majorList">The major list.</param>
        /// <returns></returns>
        public void CreateMajor(MajorList majorList)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    ElectronicDegreeMajor major = (from majorQuery in context.ElectronicDegreeMajors
                                                   where majorQuery.Name == majorList.Name
                                                   select majorQuery).FirstOrDefault();

                    if (major == null)
                    {
                        ElectronicDegreeMajor electronicDegreeMajor = new ElectronicDegreeMajor
                        {
                            Code = majorList.Code,
                            Name = majorList.Name,
                            StudyLevel = majorList.StudyLevel,
                            CreateUserName = majorList.UserName,
                            RevisionUserName = majorList.UserName,
                            CreateDatetime = DateTime.Now,
                            RevisionDatetime = DateTime.Now,
                            LegalBaseId = majorList.LegalBaseId
                        };
                        context.ElectronicDegreeMajors.InsertOnSubmit(electronicDegreeMajor);
                        context.SubmitChanges();
                    }
                    else
                    {
                        LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - MajorsDA - CreateMajor", $"Major Name: {majorList.Name} already exists.");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - MajorsDA - CreateMajor", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes the major.
        /// </summary>
        /// <param name="majorId">The major identifier.</param>
        public void DeleteMajor(int majorId)
        {
            try
            {
                MajorList majorList = new MajorList();
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                ElectronicDegreeMajor query = (from mjr in context.ElectronicDegreeMajors
                                               where mjr.ElectronicDegreeMajorId == majorId
                                               select mjr).FirstOrDefault();

                context.ElectronicDegreeMajors.DeleteOnSubmit(query);
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EdArea, $"{Constants.DataAccess} - MajorsDA - DeleteMajor", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the institutions.
        /// </summary>
        /// <param name="majorId">The major identifier.</param>
        /// <returns></returns>
        public int GetInstitutions(int majorId)
        {
            try
            {
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                IQueryable<object> majorsQuery = from instMajors in context.ElectronicDegreeInstMajors
                                                 where instMajors.ElectronicDegreeMajorId == majorId
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
                LoggerHelper.LogWebError(Constants.EdArea, $"{Constants.DataAccess} - MajorDA", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the edit major.
        /// </summary>
        /// <param name="electronicDegreeMajorId">The electronic degree major identifier.</param>
        /// <returns></returns>
        public MajorList GetMajor(int electronicDegreeMajorId)
        {
            try
            {
                MajorList majorList = new MajorList();
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                var query = (from mjr in context.ElectronicDegreeMajors
                             where mjr.ElectronicDegreeMajorId == electronicDegreeMajorId
                             orderby mjr.Name
                             select new
                             {
                                 mjr.ElectronicDegreeMajorId,
                                 mjr.Code,
                                 mjr.Name,
                                 mjr.StudyLevel,
                                 mjr.LegalBaseId
                             }).FirstOrDefault();

                if (query != null)
                {
                    majorList.ElectronicDegreeMajorId = query.ElectronicDegreeMajorId;
                    majorList.Code = query.Code;
                    majorList.Name = query.Name;
                    majorList.StudyLevel = query.StudyLevel;
                    majorList.LegalBaseId = query.LegalBaseId ?? 0;
                    return majorList;
                }
                LoggerHelper.LogWebError(Constants.EdArea, $"{Constants.DataAccess} - MajorsDA - GetMajor", "The person doesn´t exists");

                return null;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EdArea, $"{Constants.DataAccess} - MajorsDA - GetMajor", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get Majors
        /// </summary>
        /// <returns></returns>
        public List<MajorList> GetMajors()
        {
            try
            {
                List<MajorList> majorLists = new List<MajorList>();
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                IQueryable<ElectronicDegreeMajor> query = from mjrs in context.ElectronicDegreeMajors
                                                          orderby mjrs.Name
                                                          select mjrs;

                if (query != null)
                {
                    majorLists = query.Select(m => new MajorList()
                    {
                        ElectronicDegreeMajorId = m.ElectronicDegreeMajorId,
                        Code = m.Code,
                        Name = m.Name,
                        StudyLevel = m.StudyLevel,
                        LegalBaseId = m.LegalBaseId ?? 0
                    }).ToList();

                    foreach (MajorList major in majorLists)
                        major.Institutions = GetInstitutions(major.ElectronicDegreeMajorId);

                    return majorLists;
                }

                LoggerHelper.LogWebError(Constants.EdArea, $"{Constants.DataAccess} - MajorsDA - GetMajors", "The table are has no records");

                return null;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EdArea, $"{Constants.DataAccess} - MajorsDA - GetMajors", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the major.
        /// </summary>
        /// <param name="majorList">The major list.</param>
        public void UpdateMajor(MajorList majorList)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    ElectronicDegreeMajor major = context.ElectronicDegreeMajors.Single(m => m.ElectronicDegreeMajorId == majorList.ElectronicDegreeMajorId);
                    major.Code = majorList.Code;
                    major.Name = majorList.Name;
                    major.RevisionDatetime = DateTime.Now;
                    major.RevisionUserName = majorList.UserName;
                    major.StudyLevel = majorList.StudyLevel;
                    major.LegalBaseId = majorList.LegalBaseId;
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EdArea, $"{Constants.DataAccess} - MajorsDA - GetMajors", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public bool ValidateCode(string code)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    ElectronicDegreeMajor query = (from mjr in context.ElectronicDegreeMajors
                                                   where mjr.Code == code
                                                   select mjr).FirstOrDefault();

                    return query != null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EdArea, $"{Constants.DataAccess} - MajorsDA - ValidateCode", ex.Message);
                throw;
            }
        }
    }
}