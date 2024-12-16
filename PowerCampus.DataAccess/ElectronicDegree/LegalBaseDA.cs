// --------------------------------------------------------------------
// <copyright file="LegalBaseDA.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.Entities.ElectronicDegree;
using System.Linq;

namespace PowerCampus.DataAccess.ElectronicDegree
{
    /// <summary>
    /// LegalBaseDA
    /// </summary>
    public class LegalBaseDA
    {
        /// <summary>
        /// Gets the legal base catalog.
        /// </summary>
        /// <returns></returns>
        public CodeLegalBase[] GetLegalBaseCatalog()
        {
            using (ElectronicDegreeContext context = new ElectronicDegreeContext())
            {
                IQueryable<CodeLegalBase> query = from lb in context.CODE_LEGALBASEs
                                                  where lb.STATUS == "A"
                                                  select new CodeLegalBase
                                                  {
                                                      Id = lb.LegalBaseId,
                                                      MediumDescription = lb.MEDIUM_DESC,
                                                      LongDescription = lb.LONG_DESC
                                                  };

                return query.ToArray();
            }
        }
    }
}