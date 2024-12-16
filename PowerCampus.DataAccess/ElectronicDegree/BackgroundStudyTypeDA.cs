// --------------------------------------------------------------------
// <copyright file="BackgroundStudyTypeDA.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.Entities.ElectronicDegree;
using System.Linq;

namespace PowerCampus.DataAccess.ElectronicDegree
{
    /// <summary>
    /// Provides all the data access operations for BackgroundStudyType
    /// </summary>
    public class BackgroundStudyTypeDA
    {
        /// <summary>
        /// Gets the code background study catalog.
        /// </summary>
        /// <returns></returns>
        public CodeBackgroundStudyType[] GetCodeBackgroundStudyCatalog()
        {
            using (ElectronicDegreeContext context = new ElectronicDegreeContext())
            {
                IQueryable<CodeBackgroundStudyType> query =
                    from cbs in context.CODE_BACKGROUNDSTUDYTYPEs
                    where cbs.STATUS == "A"
                    select new CodeBackgroundStudyType()
                    {
                        Id = $"{cbs.CODE_VALUE_KEY}",
                        MediumDescription = cbs.MEDIUM_DESC,
                        LongDescription = cbs.LONG_DESC
                    };

                return query.ToArray();
            }
        }

        /// <summary>
        /// Gets the scholarship level catalog.
        /// </summary>
        /// <returns></returns>
        public CodeScholarshipLevel[] GetScholarshipLevelCatalog()
        {
            using (ElectronicDegreeContext context = new ElectronicDegreeContext())
            {
                IQueryable<CodeScholarshipLevel> query =
                    from cbs in context.CODE_SCHOLARSHIPLEVELs
                    where cbs.STATUS == "A"
                    select new CodeScholarshipLevel
                    {
                        CodeValueKey = cbs.CODE_VALUE_KEY,
                        ShortDescription = cbs.SHORT_DESC,
                        MediumDescription = cbs.MEDIUM_DESC,
                        LongDescription = cbs.LONG_DESC
                    };

                return query.ToArray();
            }
        }
    }
}