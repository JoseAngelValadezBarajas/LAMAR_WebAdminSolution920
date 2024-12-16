// --------------------------------------------------------------------
// <copyright file="OperatorsDA.cs" company="Ellucian">
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

namespace PowerCampus.DataAccess.ElectronicDegree
{
    /// <summary>
    /// OperatorsDA
    /// </summary>
    public class OperatorsDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorsDA"/> class.
        /// </summary>
        public OperatorsDA() => _factory = new DatabaseProviderFactory();

        /// <summary>
        /// Creates the operators.
        /// </summary>
        /// <param name="operators">The operators.</param>
        public void CreateOperators(OperatorsList operators)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    foreach (int operatorList in operators.InstitutionId)
                    {
                        ElectronicDegreeOperator electronicDegreeOperator = new ElectronicDegreeOperator
                        {
                            OperatorId = operators.OperatorId,
                            ElectronicDegreeInstitutionId = operatorList,
                            CreateDatetime = DateTime.Now,
                            CreateUserName = operators.UserName,
                            RevisionDatetime = DateTime.Now,
                            RevisionUserName = operators.UserName
                        };
                        context.ElectronicDegreeOperators.InsertOnSubmit(electronicDegreeOperator);
                    }
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - OperatorDA - CreateOperators", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Creates the permissions.
        /// </summary>
        /// <param name="operators">The operators.</param>
        /// <returns></returns>
        public void CreatePermissions(OperatorsList operators)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    foreach (string operatorList in operators.GrantedOperatorId)
                    {
                        ElectronicDegreePermission electronicDegreePermissions = new ElectronicDegreePermission
                        {
                            OperatorId = operators.OperatorId,
                            GrantedOperatorId = operatorList,
                            CreateDatetime = DateTime.Now,
                            CreateUserName = operators.UserName,
                            RevisionDatetime = DateTime.Now,
                            RevisionUserName = operators.UserName
                        };
                        context.ElectronicDegreePermissions.InsertOnSubmit(electronicDegreePermissions);
                    }
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - OperatorDA - CreatePermissions", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes the specified operator identifier from ElectronicDegreePermission.
        /// And
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        public void Delete(string operatorId)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    IQueryable<ElectronicDegreePermission> queryPer = from perm in context.ElectronicDegreePermissions
                                                                      where perm.OperatorId == operatorId
                                                                      select perm;

                    IQueryable<ElectronicDegreeOperator> queryOpe = from ope in context.ElectronicDegreeOperators
                                                                    where ope.OperatorId == operatorId
                                                                    select ope;

                    context.ElectronicDegreePermissions.DeleteAllOnSubmit(queryPer);
                    context.ElectronicDegreeOperators.DeleteAllOnSubmit(queryOpe);
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - OperatorDA - Delete", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes the institution.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        public void DeleteInstitution(int institutionId, string operatorId)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    ElectronicDegreeOperator query = (from ope in context.ElectronicDegreeOperators
                                                      where ope.ElectronicDegreeInstitutionId == institutionId
                                                      && ope.OperatorId == operatorId
                                                      select ope).FirstOrDefault();

                    context.ElectronicDegreeOperators.DeleteOnSubmit(query);
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - OperatorDA - DeleteInstitution", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes the permission.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="grantedOperatorId">The granted operator identifier.</param>
        public void DeletePermission(string operatorId, string grantedOperatorId)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    ElectronicDegreePermission query = (from ope in context.ElectronicDegreePermissions
                                                        where ope.GrantedOperatorId == grantedOperatorId && ope.OperatorId == operatorId
                                                        select ope).FirstOrDefault();

                    context.ElectronicDegreePermissions.DeleteOnSubmit(query);
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - OperatorDA - DeletePermission", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the institutions.
        /// </summary>
        /// <returns></returns>
        public List<InstitutionOptions> GetInstitutions()
        {
            try
            {
                List<InstitutionOptions> institutionOptions = new List<InstitutionOptions>();
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                return (from ins in context.ElectronicDegreeInstitutions
                        select new InstitutionOptions()
                        {
                            InstitutionId = ins.ElectronicDegreeInstitutionId,
                            Code = ins.Code,
                            Description = ins.Name
                        }).ToList();
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - OperatorDA - GetInstitutions", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get Institution-Signers relation
        /// </summary>
        /// <param name="institutionId"></param>
        /// <returns></returns>
        public List<InstitutionSignerList> GetInstitutionSigners(int institutionId)
        {
            try
            {
                List<InstitutionSignerList> signersList = new List<InstitutionSignerList>();
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    return (from sngIns in context.ElectronicDegreeInstSigners
                            join sng in context.Signers
                            on sngIns.SignerId
                            equals sng.SignerId
                            join sp in context.CODE_SIGNERPOSITIONs
                            on sng.SignerPositionId
                            equals sp.SignerPositionId
                            join abv in context.CODE_ABREVIATIONTITLEs
                            on sng.AbreviationTitleId
                            equals abv.AbreviationTitleId
                            where sngIns.ElectronicDegreeInstitutionId == institutionId
                            select new InstitutionSignerList()
                            {
                                EdAbreviationTitle = sp.LONG_DESC,
                                EdSignerName = abv.LONG_DESC + " " + sng.Name + " " + sng.FirstSurname + " " + sng.SecondSurname
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - OperatorDA - GetInstitutionSigners", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the operator information.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public List<OperatorsList> GetOperator(string operatorId)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    return (from eo in context.ElectronicDegreeOperators
                            join usr in context.ABT_USERs
                            on eo.OperatorId
                            equals usr.OPERATOR_ID
                            join peo in context.PEOPLEs
                            on usr.PEOPLE_CODE_ID
                            equals peo.PEOPLE_CODE_ID
                            where eo.OperatorId == operatorId
                            select new OperatorsList()
                            {
                                OperatorId = eo.OperatorId,
                                ElectronicDegreeInstitutionId = eo.ElectronicDegreeInstitutionId,
                                PeopleCodeId = peo.PEOPLE_CODE_ID,
                                Name = peo.FIRST_NAME + " " + peo.LAST_NAME
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - OperatorDA - GetOperator", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the operator permissions.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public List<OperatorsList> GetOperatorPermissions(string operatorId)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    return (from ep in context.ElectronicDegreePermissions
                            join usr in context.ABT_USERs
                            on ep.OperatorId
                            equals usr.OPERATOR_ID
                            join peo in context.PEOPLEs
                            on usr.PEOPLE_CODE_ID
                            equals peo.PEOPLE_CODE_ID
                            where ep.OperatorId == operatorId
                            select new OperatorsList()
                            {
                                OperatorId = ep.OperatorId,
                                GrantedOperatorsId = ep.GrantedOperatorId,
                                PeopleCodeId = peo.PEOPLE_CODE_ID,
                                Name = peo.FIRST_NAME + " " + peo.LAST_NAME
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - OperatorDA - GetOperatorPermissions", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the operators.
        /// </summary>
        /// <returns></returns>
        public List<OperatorsList> GetOperators()
        {
            try
            {
                DataSet operatorsDataSet = new DataSet();
                List<OperatorsList> operatorsList = new List<OperatorsList>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelOperatorsInformation");
                    database.LoadDataSet(command, operatorsDataSet, "webAdmin");

                    if (operatorsDataSet.Tables[0].Rows.Count > 0)
                    {
                        operatorsList = operatorsDataSet.Tables[0].AsEnumerable().Select(m => new OperatorsList()
                        {
                            OperatorId = m.Field<string>("OperatorId"),
                            Institutions = m.Field<int>("Institutions"),
                            Permissions = m.Field<int>("Permissions"),
                            PeopleCodeId = m.Field<string>("PEOPLE_CODE_ID"),
                            Name = m.Field<string>("FIRST_NAME") + " " + m.Field<string>("MIDDLE_NAME") + " " + m.Field<string>("LAST_NAME")
                        }).ToList();
                    }
                    return operatorsList;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - OperatorDA - GetOperators", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public OperatorsList GetPermissions(string operatorId)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    return (from usr in context.ABT_USERs
                            join peo in context.PEOPLEs
                            on usr.PEOPLE_CODE_ID
                            equals peo.PEOPLE_CODE_ID
                            join ep in context.ElectronicDegreePermissions
                            on usr.OPERATOR_ID
                            equals ep.OperatorId into userEp
                            from per in userEp.DefaultIfEmpty()
                            join eo in context.ElectronicDegreeOperators
                            on usr.OPERATOR_ID
                            equals eo.OperatorId into userEo
                            from ope in userEo.DefaultIfEmpty()
                            where usr.OPERATOR_ID == operatorId
                            group new { usr, peo, ope }
                            by new { usr.OPERATOR_ID, usr.PEOPLE_CODE_ID, peo.FIRST_NAME, peo.LAST_NAME }
                                           into perm
                            select new OperatorsList()
                            {
                                OperatorId = perm.Key.OPERATOR_ID,
                                Institutions = perm.Count(m => m.ope.ElectronicDegreeInstitutionId > 0),
                                PeopleCodeId = perm.Key.PEOPLE_CODE_ID,
                                Name = perm.Key.FIRST_NAME + " " + perm.Key.LAST_NAME
                            }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - OperatorDA - GetPermissions", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Search Operators
        /// </summary>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public List<OperatorsList> SearchOperators(string operatorId)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    return (from usr in context.ABT_USERs
                            join peo in context.PEOPLEs
                            on usr.PEOPLE_CODE_ID
                            equals peo.PEOPLE_CODE_ID
                            join ep in context.ElectronicDegreePermissions
                            on usr.OPERATOR_ID
                            equals ep.OperatorId into userEp
                            from per in userEp.DefaultIfEmpty()
                            join eo in context.ElectronicDegreeOperators
                            on usr.OPERATOR_ID
                            equals eo.OperatorId into userEo
                            from ope in userEo.DefaultIfEmpty()
                            where usr.OPERATOR_ID.Contains(operatorId)
                            group new { usr, peo, ope }
                            by new { usr.OPERATOR_ID, usr.PEOPLE_CODE_ID, peo.FIRST_NAME, peo.LAST_NAME }
                            into perm
                            select new OperatorsList()
                            {
                                OperatorId = perm.Key.OPERATOR_ID,
                                Institutions = perm.Count(m => m.ope.ElectronicDegreeInstitutionId > 0),
                                PeopleCodeId = perm.Key.PEOPLE_CODE_ID,
                                Name = perm.Key.FIRST_NAME + " " + perm.Key.LAST_NAME
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - OperatorDA - GetOperator", ex.Message);
                throw;
            }
        }
    }
}