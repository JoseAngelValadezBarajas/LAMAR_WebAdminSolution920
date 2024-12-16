// --------------------------------------------------------------------
// <copyright file="FederalEntitiesDA.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using Hedtech.PowerCampus.Logger;
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.Entities.ElectronicDegree;
using System;
using System.Linq;

namespace PowerCampus.DataAccess.ElectronicDegree
{
    /// <summary>
    /// Handles all the CRUD operations for Federal Entities
    /// </summary>
    public class FederalEntitiesDA
    {
        /// <summary>
        /// Gets the code federal entity catalog.
        /// </summary>
        /// <returns></returns>
        public CodeFederalEntity[] GetCodeFederalEntityCatalog()
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    IQueryable<CodeFederalEntity> query =
                        (from cfe in context.CODE_FEDERALENTITies
                         where cfe.STATUS == "A"
                         select new CodeFederalEntity
                         {
                             CodeValueKey = cfe.CODE_VALUE_KEY,
                             Id = cfe.SHORT_DESC,
                             MediumDescription = cfe.MEDIUM_DESC,
                             LongDescription = cfe.LONG_DESC
                         }).OrderBy(y => y.LongDescription);

                    return query.ToArray();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - FederalEntitiesDA - GetCodeFederalEntityCatalog", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the code federal entity catalog without foreign.
        /// </summary>
        /// <returns></returns>
        public CodeFederalEntity[] GetCodeFederalEntityCatalogWithoutForeign()
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    IQueryable<CodeFederalEntity> query =
                        (from cfe in context.CODE_FEDERALENTITies
                         where cfe.STATUS == "A" && cfe.CODE_VALUE_KEY != 33
                         select new CodeFederalEntity
                         {
                             CodeValueKey = cfe.CODE_VALUE_KEY,
                             Id = cfe.SHORT_DESC,
                             MediumDescription = cfe.MEDIUM_DESC,
                             LongDescription = cfe.LONG_DESC
                         });

                    return query.ToArray();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - FederalEntitiesDA - GetCodeFederalEntityCatalog", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the code states.
        /// </summary>
        /// <returns></returns>
        public CodeState[] GetCodeStates()
        {
            using (ElectronicDegreeContext context = new ElectronicDegreeContext())
            {
                IQueryable<CodeState> query =
                    (from cfe in context.CODE_STATEs
                     where cfe.STATUS == "A"
                     select new CodeState
                     {
                         CodeValueKey = cfe.CODE_VALUE_KEY,
                         ShortDescription = cfe.SHORT_DESC,
                         MediumDescription = cfe.MEDIUM_DESC,
                         LongDescription = cfe.LONG_DESC
                     }).OrderBy(y => y.LongDescription);

                return query.ToArray();
            }
        }
    }
}