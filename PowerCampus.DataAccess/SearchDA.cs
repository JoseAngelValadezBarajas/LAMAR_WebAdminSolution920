// --------------------------------------------------------------------
// <copyright file="SearchDA.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PowerCampus.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace PowerCampus.DataAccess
{
    /// <summary>
    /// SearchDA Class
    /// </summary>
    public class SearchDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchDA"/> class.
        /// </summary>
        public SearchDA()
        {
            _factory = new DatabaseProviderFactory();
        }

        /// <summary>
        /// Gets the advanced search.
        /// </summary>
        /// <param name="people">The people.</param>
        /// <returns></returns>
        public List<SearchResult> GetAdvancedSearch(People people)
        {
            List<SearchResult> response = new List<SearchResult>();
            try
            {
                // _logService.Debug("Method starts - GetAdvancedSearch");
                Database database = _factory.CreateDefault();
                DataSet searchResultDataset = new DataSet();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelPeopleAdvancedSearch");
                    database.AddInParameter(command, "@PeopleId", DbType.String, string.IsNullOrEmpty(people.PeopleCodeId) ? null : people.PeopleCodeId);
                    database.AddInParameter(command, "@FirstName", DbType.String, string.IsNullOrEmpty(people.FirstName) ? null : people.FirstName);
                    database.AddInParameter(command, "@MiddleName", DbType.String, string.IsNullOrEmpty(people.MiddleName) ? null : people.MiddleName);
                    database.AddInParameter(command, "@LastName", DbType.String, string.IsNullOrEmpty(people.LastName) ? null : people.LastName);
                    database.AddInParameter(command, "@Email", DbType.String, string.IsNullOrEmpty(people.PrimaryEmail) ? null : people.PrimaryEmail);
                    database.AddInParameter(command, "@RecordType", DbType.String, string.IsNullOrEmpty(people.RecordType) ? null : people.RecordType);
                    database.LoadDataSet(command, searchResultDataset, "sPeopleAdvancedSearch");
                }
                if (searchResultDataset.Tables[0].Rows.Count > 0)
                {
                    List<SearchResult> searchResultPeople = searchResultDataset.Tables[0].AsEnumerable().Select(m => new SearchResult()
                    {
                        PeopleOrgCodeId = m.Field<string>("PeopleId"),
                        FullName = m.Field<string>("Name"),
                        PrimaryEmail = m.Field<string>("Email"),
                        RecordType = m.Field<string>("RecordTypeDesc")
                    }).ToList();

                    string peoplecodeId = string.Empty;
                    var peopleResultSearch = new SearchResult();
                    foreach (var peopleResult in searchResultPeople)
                    {
                        if (!peoplecodeId.Equals(peopleResult.PeopleOrgCodeId))
                        {
                            List<string> recordtypeLstPerson = new List<string>();
                            foreach (var rtList in searchResultPeople.Where(p => p.PeopleOrgCodeId.Equals(peopleResult.PeopleOrgCodeId)).ToList())
                            {
                                recordtypeLstPerson.Add(rtList.RecordType);
                            }

                            peopleResultSearch = new SearchResult
                            {
                                PeopleOrgCodeId = peopleResult.PeopleOrgCodeId,
                                FullName = peopleResult.FullName,
                                PrimaryEmail = peopleResult.PrimaryEmail,
                                RecordTypeList = recordtypeLstPerson
                            };

                            response.Add(peopleResultSearch);
                        }
                        peoplecodeId = peopleResult.PeopleOrgCodeId;
                    }
                }
                else
                {
                    var searchResPeopleNull = new SearchResult
                    {
                        PeopleOrgCodeId = string.Empty,
                        FullName = string.Empty
                    };
                    response.Add(searchResPeopleNull);
                }
                // _logService.Debug("Method ends - GetAdvancedSearch");
                return response;
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - SearchDA - GetAdvancedSearch", e.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the advanced search.
        /// </summary>
        /// <param name="organization">The organization.</param>
        /// <returns></returns>
        public List<SearchResult> GetAdvancedSearch(Organization organization)
        {
            DataSet searchResultDataset = new DataSet();
            List<SearchResult> response = new List<SearchResult>();
            try
            {
                // _logService.Debug("Method starts - GetAdvancedSearch");
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelOrganizationAdvancedSearch");

                    database.AddInParameter(command, "@OrganizationId", DbType.String, string.IsNullOrEmpty(organization.OrganizationCodeId) ? null : organization.OrganizationCodeId);
                    database.AddInParameter(command, "@OrganizationName", DbType.String, string.IsNullOrEmpty(organization.OrganizationName) ? null : organization.OrganizationName);
                    database.AddInParameter(command, "@Email", DbType.String, string.IsNullOrEmpty(organization.Email) ? null : organization.Email);
                    database.AddInParameter(command, "@RecordType", DbType.String, string.IsNullOrEmpty(organization.RecordType) ? null : organization.RecordType);
                    database.LoadDataSet(command, searchResultDataset, "OrganizationAdvancedSearch");
                }
                if (searchResultDataset.Tables[0].Rows.Count > 0)
                {
                    List<SearchResult> searchOrganization = searchResultDataset.Tables[0].AsEnumerable().Select(m => new SearchResult()
                    {
                        PeopleOrgCodeId = m.Field<string>("OrganizationId"),
                        FullName = m.Field<string>("Name"),
                        PrimaryEmail = m.Field<string>("Email"),
                        RecordType = m.Field<string>("RecordTypeDesc")
                    }).ToList();

                    string peoplecodeId = string.Empty;
                    var organizationResultSearch = new SearchResult();

                    foreach (var prganizationResult in searchOrganization)
                    {
                        if (!peoplecodeId.Equals(prganizationResult.PeopleOrgCodeId))
                        {
                            List<string> recordtypeLstPerson = new List<string>();
                            var recordtypeLst = searchOrganization.Where(p => p.PeopleOrgCodeId.Equals(prganizationResult.PeopleOrgCodeId)).ToList();

                            foreach (var rtList in recordtypeLst)
                            {
                                recordtypeLstPerson.Add(rtList.RecordType);
                            }

                            organizationResultSearch = new SearchResult
                            {
                                PeopleOrgCodeId = prganizationResult.PeopleOrgCodeId,
                                FullName = prganizationResult.FullName,
                                PrimaryEmail = prganizationResult.PrimaryEmail,
                                RecordTypeList = recordtypeLstPerson
                            };

                            response.Add(organizationResultSearch);
                        }
                        peoplecodeId = prganizationResult.PeopleOrgCodeId;
                    }
                }
                else
                {
                    var searchOrgNull = new SearchResult
                    {
                        PeopleOrgCodeId = string.Empty,
                        FullName = string.Empty
                    };
                    response.Add(searchOrgNull);
                }
                // _logService.Debug("Method ends - GetAdvancedSearch");
                return response;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - SearchDA - GetAdvancedSearch", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Searches the result.
        /// </summary>
        /// <param name="Keyword">The keyword.</param>
        /// <returns></returns>
        public List<SearchResult> SearchResult(string Keyword)
        {
            List<SearchResult> response = new List<SearchResult>();
            try
            {
                // _logService.Debug("Method starts - SearchResult");
                DataSet searchResultDataset = new DataSet();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("spSelPeopleQuickSearch");

                    if (IsNumeric(Keyword))
                        database.AddInParameter(command, "@WildCard", DbType.String, "%" + Keyword + "%");
                    else
                        database.AddInParameter(command, "@WildCard", DbType.String, Keyword + "%");

                    database.LoadDataSet(command, searchResultDataset, "SearchResult");
                }
                if (searchResultDataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in searchResultDataset.Tables[0].Rows)
                    {
                        var searchResPeople = new SearchResult
                        {
                            PeopleOrgCodeId = dr["PeopleId"].ToString(),
                            FullName = dr["FirstName"].ToString() + " " + dr["MiddleName"].ToString() + " " + dr["LastName"].ToString()
                        };
                        response.Add(searchResPeople);
                    }
                }
                else
                {
                    var searchResPeopleNull = new SearchResult
                    {
                        PeopleOrgCodeId = string.Empty,
                        FullName = string.Empty
                    };
                    response.Add(searchResPeopleNull);
                }
                // _logService.Debug("Method ends - SearchResult");
                return response;
            }
            catch (Exception e)
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - SearchDA - SearchResult", e.Message);
                return null;
            }
        }

        private bool IsNumeric(string keyword)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(keyword, "[0-9]");
        }
    }
}