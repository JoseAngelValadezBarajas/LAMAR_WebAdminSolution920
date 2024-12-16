// --------------------------------------------------------------------
// <copyright file="BackgroundStudyTypeServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.DataAccess.ElectronicDegree;
using PowerCampus.Entities.ElectronicDegree;
using System;
using System.Linq;

namespace PowerCampus.Business.ElectronicDegree
{
    /// <summary>
    /// BackgroundStudyTypeServices
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.ElectronicDegree.IBackgroundStudyTypeServices" />
    public class BackgroundStudyTypeServices : IBackgroundStudyTypeServices
    {
        /// <summary>
        /// The electronic degree table
        /// </summary>
        private const string ElectronicDegreeTable = "WebAdmin.CODE_BACKGROUNDSTUDYTYPE";

        /// <summary>
        /// The power campus table
        /// </summary>
        private const string PowerCampusTable = "dbo.CODE_SCHOLARSHIPLEVEL";

        /// <summary>
        /// The BST da
        /// </summary>
        private readonly BackgroundStudyTypeDA bstDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundStudyTypeServices"/> class.
        /// </summary>
        public BackgroundStudyTypeServices() => this.bstDA = new BackgroundStudyTypeDA();

        /// <summary>
        /// Gets the current catalog.
        /// </summary>
        /// <returns></returns>
        public CodeBackgroundStudyTypeMapping[] GetCurrentCatalog()
        {
            CodeBackgroundStudyTypeMapping[] catalog =
                bstDA.GetCodeBackgroundStudyCatalog()
                .Select(c => new CodeBackgroundStudyTypeMapping
                {
                    ShortDescription = c.Id,
                    MediumDescription = c.MediumDescription,
                    LongDescription = c.LongDescription
                }).ToArray();

            IQueryable<ElectronicDegreeMapping> mappings = ElectronicDegreeMappingDA.GetElectronicDegreeMapping(ElectronicDegreeTable);

            foreach (CodeBackgroundStudyTypeMapping item in catalog)
                item.MappedLevels = mappings.Where(m => m.ElectronicDegreeValue == item.ShortDescription)
                    .Select(m => new PowerCampusScholarshipLevel
                    {
                        ElectronicDegreeMappingId = m.ElectronicDegreeMappingId,
                        ScholarshipLevelId = m.ScholarshipLevel,
                        ShortDescription = m.CODE_SCHOLARSHIPLEVEL.SHORT_DESC,
                        MediumDescription = m.CODE_SCHOLARSHIPLEVEL.MEDIUM_DESC,
                        LongDescription = m.CODE_SCHOLARSHIPLEVEL.LONG_DESC
                    }).ToArray();

            return catalog.ToArray();
        }

        /// <summary>
        /// Gets the scholarship levels catalog.
        /// </summary>
        /// <returns></returns>
        public CodeScholarshipLevel[] GetScholarshipLevelsCatalog() => bstDA.GetScholarshipLevelCatalog();

        /// <summary>
        /// Saves the scholarship level mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        public void SaveScholarshipLevelMapping(CodeBackgroundStudyTypeMapping mapping)
        {
            ElectronicDegreeMappingDA.DeleteElectronicDegreeMapping(mapping.ShortDescription, ElectronicDegreeTable);

            foreach (PowerCampusScholarshipLevel level in mapping.MappedLevels)
            {
                ElectronicDegreeMapping edm = new ElectronicDegreeMapping();
                edm.ElectronicDegreeTable = ElectronicDegreeTable;
                edm.PowerCampusTable = PowerCampusTable;
                edm.ElectronicDegreeValue = mapping.ShortDescription;
                edm.ScholarshipLevel = level.ScholarshipLevelId;
                edm.CreateDatetime = DateTime.Now;
                edm.CreateUserName = mapping.UserName;
                edm.RevisionDatetime = DateTime.Now;
                edm.RevisionUserName = mapping.UserName;

                ElectronicDegreeMappingDA.SaveElectronicDegreeMapping(edm);
            }
        }
    }
}