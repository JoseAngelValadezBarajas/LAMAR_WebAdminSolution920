// --------------------------------------------------------------------
// <copyright file="ECInstitutionCampusDA.cs" company="Ellucian">
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
using System.Linq;
using Institution = PowerCampus.Entities.ElectronicCertificate.InstitutionCampus;
using InstitutionCampus = PowerCampus.DataAccess.DataAccess.InstitutionCampus;

namespace PowerCampus.DataAccess.ElectronicCertificate
{
    /// <summary>
    /// ECInstitutionCampus data access
    /// </summary>
    public class ECInstitutionCampusDA
    {
        /// <summary>
        /// The factory
        /// </summary>
        private readonly DatabaseProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ECInstitutionCampusDA"/> class.
        /// </summary>
        public ECInstitutionCampusDA() => _factory = new DatabaseProviderFactory();

        /// <summary>
        /// Creates the ec institution campus.
        /// </summary>
        /// <param name="institutionCampus">The institution campus.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public bool CreateECInstitutionCampus(Institution institutionCampus, string operatorId)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    InstitutionCampus institution = new InstitutionCampus()
                    {
                        CampusCodeId = institutionCampus.CampusCodeId,
                        CampusSepCode = institutionCampus.CampusSepCode,
                        CreateDatetime = DateTime.Now,
                        CreateOpId = operatorId,
                        FederalEntityId = institutionCampus.FederalEntityId,
                        Folio = "0",
                        FolioFormat = institutionCampus.FolioFormat ?? "#InstitutionCampus.Folio#",
                        InstitutionCampusId = institutionCampus.InstitutionCampusId ?? 0,
                        InstitutionCodeId = institutionCampus.InstitutionCodeId,
                        InstitutionSepId = institutionCampus.InstitutionSepId,
                        ResponsibleId = institutionCampus.ResponsibleId,
                        RevisionDatetime = DateTime.Now,
                        RevisionOpId = operatorId,
                        SigningInstitutionId = institutionCampus.SigningInstitutionId
                    };

                    context.InstitutionCampus.InsertOnSubmit(institution);
                    context.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ECInstitutionCampusDA - CreateECInstitutionCampus", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Gets the ec institution campus.
        /// </summary>
        /// <returns></returns>
        public InstitutionCampuses GetECInstitutionCampus()
        {
            try
            {
                DataSet institutionCampusDataSet = new DataSet();
                InstitutionCampuses institutionCampus = new InstitutionCampuses();
                List<Institution> institutionCampuses = null;
                Database database = _factory.CreateDefault();
                using (SqlConnection connection = database.CreateConnection() as SqlConnection)
                {
                    DbCommand command = database.GetStoredProcCommand("Webadmin.spSelInstitutionCampus");
                    database.LoadDataSet(command, institutionCampusDataSet, "InstitutionCampus");

                    if (institutionCampusDataSet.Tables[0].Rows.Count > 0)
                    {
                        institutionCampuses = institutionCampusDataSet.Tables[0].AsEnumerable().Select(m => new Institution()
                        {
                            CampusName = m.Field<string>("CampusName"),
                            CampusCodeId = m.Field<string>("CampusCodeId"),
                            CampusSepCode = m.Field<string>("CampusSepCode"),
                            FederalEntityId = m.Field<int>("FederalEntityId"),
                            InstitutionName = m.Field<string>("InstitutionName"),
                            InstitutionCodeId = m.Field<string>("InstitutionCodeId"),
                            InstitutionSepId = m.Field<string>("InstitutionSepId"),
                            ResponsibleId = m.Field<int?>("ResponsibleId"),
                            FolioFormat = m.Field<string>("FolioFormat"),
                            InstitutionCampusId = m.Field<int?>("InstitutionCampusId"),
                            SigningInstitutionId = m.Field<string>("SigningInstitutionId")
                        }).ToList();
                        institutionCampus.InstitutionCampus = institutionCampuses;
                    }
                    if (institutionCampusDataSet.Tables[1].Rows.Count > 0)
                    {
                        institutionCampus.IssuingPlace = institutionCampusDataSet.Tables[1].AsEnumerable().Select(m => new CodeTable()
                        {
                            Id = m.Field<int>("FederalEntityId"),
                            Description = m.Field<string>("FederalEntityDesc")
                        }).ToList();
                    }
                    return institutionCampus;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ECInstitutionCampusDA - GetECInstitutionCampus", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the responsible.
        /// </summary>
        /// <param name="campusSepCode">The campus sep code.</param>
        /// <returns></returns>
        public int GetResponsible(string campusSepCode)
        {
            try
            {
                int responsibleId = 0;
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    try
                    {
                        responsibleId = context.InstitutionCampus.Single(x => x.CampusSepCode == campusSepCode).ResponsibleId.Value;
                    }
                    catch (Exception ex)
                    {
                        responsibleId = context.InstitutionCampus.FirstOrDefault().ResponsibleId.Value;
                    }
                }
                return responsibleId;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ECInstitutionCampusDA - GetResponsible", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the ec institution campus.
        /// </summary>
        /// <param name="institutionCampus">The institution campus.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public bool UpdateECInstitutionCampus(Institution institutionCampus, string operatorId)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    InstitutionCampus institution = context.InstitutionCampus.Single(r => r.InstitutionCampusId == institutionCampus.InstitutionCampusId);
                    institution.CampusCodeId = institutionCampus.CampusCodeId;
                    institution.CampusSepCode = institutionCampus.CampusSepCode;
                    institution.FederalEntityId = institutionCampus.FederalEntityId;
                    institution.FolioFormat = institutionCampus.FolioFormat ?? "#InstitutionCampus.Folio#";
                    institution.InstitutionCampusId = institutionCampus.InstitutionCampusId ?? 0;
                    institution.InstitutionCodeId = institutionCampus.InstitutionCodeId;
                    institution.InstitutionSepId = institutionCampus.InstitutionSepId;
                    institution.ResponsibleId = institutionCampus.ResponsibleId;
                    institution.RevisionDatetime = DateTime.Now;
                    institution.RevisionOpId = operatorId;
                    institution.SigningInstitutionId = institutionCampus.SigningInstitutionId;

                    context.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ECInstitutionCampusDA - UpdateECInstitutionCampus", ex.Message);
                return false;
            }
        }
    }
}