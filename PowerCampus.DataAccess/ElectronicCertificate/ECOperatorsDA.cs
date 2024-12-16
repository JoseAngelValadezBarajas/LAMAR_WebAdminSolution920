// --------------------------------------------------------------------
// <copyright file="OperatorsDA.cs" company="Ellucian">
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
    /// OperatorsDA
    /// </summary>
    public class ECOperatorsDA
    {
        /// <summary>
        /// The database provider factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ECOperatorsDA"/> class.
        /// </summary>
        public ECOperatorsDA() => _factory = new DatabaseProviderFactory();

        /// <summary>
        /// Creates the operators.
        /// </summary>
        /// <param name="operators">The operators.</param>
        public void CreateOperators(OperatorsList operators)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    foreach (string operatorList in operators.CampusCodeId)
                    {
                        ElectronicCertificateOperator electronicDegreeOperator = new ElectronicCertificateOperator
                        {
                            OperatorId = operators.OperatorId,
                            CampusCodeId = operatorList.ToString(),
                            CreateDatetime = DateTime.Now,
                            CreateOpId = operators.UserName,
                            RevisionDatetime = DateTime.Now,
                            RevisionOpId = operators.UserName
                        };
                        context.ElectronicCertificateOperators.InsertOnSubmit(electronicDegreeOperator);
                    }
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - OperatorDA - CreateOperators", ex.Message);
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
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    foreach (string operatorList in operators.GrantedOperatorId)
                    {
                        ElectronicCertPermission electronicCertPermissions = new ElectronicCertPermission
                        {
                            OperatorId = operators.OperatorId,
                            GrantedOperatorId = operatorList,
                            CreateDatetime = DateTime.Now,
                            CreateOpId = operators.UserName,
                            RevisionDatetime = DateTime.Now,
                            RevisionOpId = operators.UserName
                        };
                        context.ElectronicCertPermissions.InsertOnSubmit(electronicCertPermissions);
                    }
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - OperatorDA - CreatePermissions", ex.Message);
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
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    IQueryable<ElectronicCertificateOperator> queryOpe = from perm in context.ElectronicCertificateOperators
                                                                         where perm.OperatorId == operatorId
                                                                         select perm;

                    IQueryable<ElectronicCertPermission> queryPer = from ope in context.ElectronicCertPermissions
                                                                    where ope.OperatorId == operatorId
                                                                    select ope;

                    context.ElectronicCertPermissions.DeleteAllOnSubmit(queryPer);
                    context.ElectronicCertificateOperators.DeleteAllOnSubmit(queryOpe);
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - OperatorDA - Delete", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Delete Institution
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        public void DeleteInstitution(string institutionId, string operatorId)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    ElectronicCertificateOperator query = (from ope in context.ElectronicCertificateOperators
                                                           where ope.CampusCodeId == institutionId
                                                           && ope.OperatorId == operatorId
                                                           select ope).FirstOrDefault();

                    context.ElectronicCertificateOperators.DeleteOnSubmit(query);
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - OperatorDA - DeleteInstitution", ex.Message);
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
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    ElectronicCertPermission query = (from ope in context.ElectronicCertPermissions
                                                      where ope.GrantedOperatorId == grantedOperatorId && ope.OperatorId == operatorId
                                                      select ope).FirstOrDefault();

                    context.ElectronicCertPermissions.DeleteOnSubmit(query);
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - OperatorDA - DeletePermission", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the institutions.
        /// </summary>
        /// <returns></returns>
        public List<InstitutionCampusOperator> GetInstitutions()
        {
            try
            {
                DataSet institutionsDataSet = new DataSet();
                List<InstitutionCampusOperator> institutionsList = new List<InstitutionCampusOperator>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("WebAdmin.spSelCampus");
                    database.LoadDataSet(command, institutionsDataSet, "webAdmin");
                    if (institutionsDataSet.Tables[0].Rows.Count > 0)
                    {
                        institutionsList = institutionsDataSet.Tables[0].AsEnumerable().Select(m => new InstitutionCampusOperator()
                        {
                            CampusCodeId = m.Field<string>("CampusCodeId"),
                            CampusName = m.Field<string>("CampusName"),
                        }).ToList();
                    }
                    return institutionsList;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - OperatorDA - GetInstitutions", ex.Message);
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
                DataSet operatorDataSet = new DataSet();
                List<OperatorsList> operatorList = new List<OperatorsList>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("WebAdmin.spSelElectronicCertificateOperatorByOperator");
                    database.AddInParameter(command, "@OperatorId", DbType.String, operatorId);
                    database.LoadDataSet(command, operatorDataSet, "webAdmin");
                    if (operatorDataSet.Tables[0].Rows.Count > 0)
                    {
                        operatorList = operatorDataSet.Tables[0].AsEnumerable().Select(m => new OperatorsList()
                        {
                            OperatorId = m.Field<string>("OperatorId"),
                            CampusId = m.Field<string>("CampusCodeId"),
                            PeopleCodeId = m.Field<string>("PeopleCodeId"),
                            Name = m.Field<string>("FirstName") + " " + m.Field<string>("LastName")
                        }).ToList();
                    }
                    return operatorList;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - OperatorDA - GetOperator", ex.Message);
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
                DataSet operatorDataSet = new DataSet();
                List<OperatorsList> operatorList = new List<OperatorsList>();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("WebAdmin.spSelElectronicCertificatePermissionsByOperator");
                    database.AddInParameter(command, "@OperatorId", DbType.String, operatorId);
                    database.LoadDataSet(command, operatorDataSet, "webAdmin");
                    if (operatorDataSet.Tables[0].Rows.Count > 0)
                    {
                        operatorList = operatorDataSet.Tables[0].AsEnumerable().Select(m => new OperatorsList()
                        {
                            OperatorId = m.Field<string>("OperatorId"),
                            GrantedOperatorsId = m.Field<string>("GrantedOperatorId"),
                            PeopleCodeId = m.Field<string>("PeopleCodeId"),
                            Name = m.Field<string>("FirstName") + " " + m.Field<string>("LastName")
                        }).ToList();
                    }
                    return operatorList;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - OperatorDA - GetOperatorPermissions", ex.Message);
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
                    DbCommand command = database.GetStoredProcCommand("WebAdmin.spSelElectronicCertificateOperators");
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
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - OperatorDA - GetOperators", ex.Message);
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
                DataSet operatorsDataSet = new DataSet();
                OperatorsList operatorsList = new OperatorsList();
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("WebAdmin.spSelElectronicCertificatePermissionsByGrantedOperator");
                    database.AddInParameter(command, "@OperatorId", DbType.String, operatorId);
                    database.LoadDataSet(command, operatorsDataSet, "webAdmin");

                    if (operatorsDataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in operatorsDataSet.Tables[0].Rows)
                        {
                            OperatorsList operators = new OperatorsList()
                            {
                                OperatorId = row["OperatorId"].ToString(),
                                Institutions = (int)row["Institutions"],
                                PeopleCodeId = row["PeopleCodeId"].ToString(),
                                Name = row["FirstName"].ToString() + " " + row["LastName"].ToString()
                            };

                            operatorsList = operators;
                        }
                    }
                    return operatorsList;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - OperatorDA - GetPermissions", ex.Message);
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
                List<OperatorsList> users = new List<OperatorsList>();
                string operatorCert = string.Empty;
                string permissionCert = string.Empty;

                using (ElectronicCertificateContext ecContext = new ElectronicCertificateContext())
                {
                    operatorCert = (from ec in ecContext.ElectronicCertificateOperators
                                    where ec.OperatorId.Contains(operatorId)
                                    select ec.OperatorId).FirstOrDefault();

                    permissionCert = (from ecp in ecContext.ElectronicCertPermissions
                                      where ecp.OperatorId.Contains(operatorId)
                                      select ecp.OperatorId).FirstOrDefault();
                }

                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    users = (from usr in context.ABT_USERs
                             join peo in context.PEOPLEs
                             on usr.PEOPLE_CODE_ID
                             equals peo.PEOPLE_CODE_ID
                             where usr.OPERATOR_ID.Contains(operatorId)
                             select new OperatorsList()
                             {
                                 OperatorId = usr.OPERATOR_ID,
                                 PeopleCodeId = peo.PEOPLE_CODE_ID,
                                 Name = peo.FIRST_NAME + " " + peo.LAST_NAME
                             }).ToList();
                }

                if (!string.IsNullOrEmpty(operatorCert) || !string.IsNullOrEmpty(permissionCert))
                {
                    OperatorsList user = users.Single(i => i.OperatorId == operatorId.ToUpper());
                    users.Remove(user);
                }

                return users;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - OperatorDA - GetOperator", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Searches the operators permissions.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public List<OperatorsList> SearchOperatorsPerm(string operatorId)
        {
            try
            {
                int campuses = 0;
                using (ElectronicCertificateContext ecContext = new ElectronicCertificateContext())
                {
                    campuses = (from ec in ecContext.ElectronicCertificateOperators
                                where ec.OperatorId.Contains(operatorId)
                                select ec.CampusCodeId).Count();
                }

                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    return (from usr in context.ABT_USERs
                            join peo in context.PEOPLEs
                            on usr.PEOPLE_CODE_ID
                            equals peo.PEOPLE_CODE_ID
                            where usr.OPERATOR_ID.Contains(operatorId)
                            select new OperatorsList()
                            {
                                Institutions = campuses,
                                OperatorId = usr.OPERATOR_ID,
                                PeopleCodeId = peo.PEOPLE_CODE_ID,
                                Name = peo.FIRST_NAME + " " + peo.LAST_NAME
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampus.DataAccess - OperatorDA - GetOperator", ex.Message);
                throw;
            }
        }
    }
}