// --------------------------------------------------------------------
// <copyright file="FederalEntitiesServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.DataAccess.ElectronicDegree;
using PowerCampus.Entities.ElectronicDegree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerCampus.Business.ElectronicDegree
{
    /// <summary>
    /// Provides all the services required for handling all the operations over Federal Entities Catalog
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.ElectronicDegree.IFederalEntitiesServices" />
    public class FederalEntitiesServices : IFederalEntitiesServices
    {
        /// <summary>
        /// The electronic degree table
        /// </summary>
        private const string ElectronicDegreeTable = "WebAdmin.CODE_FEDERALENTITY";

        /// <summary>
        /// The power campus table
        /// </summary>
        private const string PowerCampusTable = "dbo.CODE_STATE";

        /// <summary>
        /// The entities data access
        /// </summary>
        private readonly FederalEntitiesDA entitiesDa;

        /// <summary>
        /// Initializes a new instance of the <see cref="FederalEntitiesServices"/> class.
        /// </summary>
        public FederalEntitiesServices() => entitiesDa = new FederalEntitiesDA();

        /// <summary>
        /// Gets the code catalog.
        /// </summary>
        /// <returns></returns>
        public List<Entities.ElectronicCertificate.CodeTable> GetCodeCatalog()
        {
            CodeFederalEntity[] entitiesCatalog = entitiesDa.GetCodeFederalEntityCatalog();
            List<Entities.ElectronicCertificate.CodeTable> catalog = new List<Entities.ElectronicCertificate.CodeTable>();
            if (entitiesCatalog != null)
            {
                for (int i = 0; i < entitiesCatalog.Length; i++)
                {
                    catalog.Add(new Entities.ElectronicCertificate.CodeTable
                    {
                        CodeValueKey = entitiesCatalog[i].CodeValueKey,
                        Description = entitiesCatalog[i].LongDescription,
                        ShortDescription = entitiesCatalog[i].Id
                    });
                }
            }
            return catalog;
        }

        /// <summary>
        /// Gets the code catalog without foreign.
        /// </summary>
        /// <returns></returns>
        public List<Entities.ElectronicCertificate.CodeTable> GetCodeCatalogWithoutForeign()
        {
            CodeFederalEntity[] entitiesCatalog = entitiesDa.GetCodeFederalEntityCatalogWithoutForeign();
            List<Entities.ElectronicCertificate.CodeTable> catalog = new List<Entities.ElectronicCertificate.CodeTable>();
            if (entitiesCatalog != null)
            {
                for (int i = 0; i < entitiesCatalog.Length; i++)
                {
                    catalog.Add(new Entities.ElectronicCertificate.CodeTable
                    {
                        CodeValueKey = entitiesCatalog[i].CodeValueKey,
                        Description = entitiesCatalog[i].LongDescription,
                        ShortDescription = entitiesCatalog[i].Id
                    });
                }
            }
            return catalog;
        }

        /// <summary>
        /// Gets the current catalog.
        /// </summary>
        /// <returns></returns>
        public CodeFederalEntitityMapping[] GetCurrentCatalog()
        {
            CodeFederalEntitityMapping[] catalog =
                entitiesDa.GetCodeFederalEntityCatalog()
                .Select(c => new CodeFederalEntitityMapping
                {
                    ShortDescription = c.Id,
                    MediumDescription = c.MediumDescription,
                    LongDescription = c.LongDescription
                }).ToArray();

            IQueryable<ElectronicDegreeMapping> mappings = ElectronicDegreeMappingDA.GetElectronicDegreeMapping(ElectronicDegreeTable);

            foreach (CodeFederalEntitityMapping item in catalog)
                item.MappedStates = mappings.Where(m => m.ElectronicDegreeValue == item.ShortDescription)
                    .Select(m => new PowerCampusCodeState
                    {
                        ElectronicDegreeMappingId = m.ElectronicDegreeMappingId,
                        CodeStateId = m.State,
                        ShortDescription = m.CODE_STATE.SHORT_DESC,
                        MediumDescription = m.CODE_STATE.MEDIUM_DESC,
                        LongDescription = m.CODE_STATE.LONG_DESC
                    }).ToArray();

            return catalog.ToArray();
        }

        /// <summary>
        /// Gets the states catalog.
        /// </summary>
        /// <returns></returns>
        public CodeState[] GetStatesCatalog() => entitiesDa.GetCodeStates();

        /// <summary>
        /// Saves the federal entity mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        public void SaveFederalEntityMapping(CodeFederalEntitityMapping mapping)
        {
            ElectronicDegreeMappingDA.DeleteElectronicDegreeMapping(mapping.ShortDescription, ElectronicDegreeTable);

            foreach (PowerCampusCodeState state in mapping.MappedStates)
            {
                ElectronicDegreeMapping edm = new ElectronicDegreeMapping();
                edm.ElectronicDegreeTable = ElectronicDegreeTable;
                edm.PowerCampusTable = PowerCampusTable;
                edm.ElectronicDegreeValue = mapping.ShortDescription;
                edm.State = state.CodeStateId;
                edm.CreateDatetime = DateTime.Now;
                edm.CreateUserName = mapping.UserName;
                edm.RevisionDatetime = DateTime.Now;
                edm.RevisionUserName = mapping.UserName;

                ElectronicDegreeMappingDA.SaveElectronicDegreeMapping(edm);
            }
        }
    }
}