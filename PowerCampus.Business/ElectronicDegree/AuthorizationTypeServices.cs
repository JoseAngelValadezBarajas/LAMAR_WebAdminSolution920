// --------------------------------------------------------------------
// <copyright file="AuthorizationTypeServices.cs" company="Ellucian">
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
    /// AuthorizationTypeServices
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.ElectronicDegree.IAuthorizationTypeServices" />
    public class AuthorizationTypeServices : IAuthorizationTypeServices
    {
        /// <summary>
        /// The electronic degree table
        /// </summary>
        private const string ElectronicDegreeTable = "WebAdmin.CODE_AuthorizationType";

        /// <summary>
        /// The power campus table
        /// </summary>
        private const string PowerCampusTable = "dbo.Rvoe";

        /// <summary>
        /// The BST da
        /// </summary>
        private readonly AuthorizationTypeDA atDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationTypeServices"/> class.
        /// </summary>
        public AuthorizationTypeServices() => this.atDA = new AuthorizationTypeDA();

        /// <summary>
        /// Gets the current catalog.
        /// </summary>
        /// <returns></returns>
        public CodeAuthorizationTypeMapping[] GetCurrentCatalog()
        {
            CodeAuthorizationTypeMapping[] catalog =
                atDA.GetCodeAuthorizationTypeCatalog()
                .Select(c => new CodeAuthorizationTypeMapping
                {
                    ShortDescription = c.Id,
                    MediumDescription = c.MediumDescription,
                    LongDescription = c.LongDescription
                }).ToArray();

            IQueryable<ElectronicDegreeMapping> mappings = ElectronicDegreeMappingDA.GetElectronicDegreeMapping(ElectronicDegreeTable);

            foreach (CodeAuthorizationTypeMapping item in catalog)
                item.MappedRvoes = mappings.Where(m => m.ElectronicDegreeValue == item.ShortDescription)
                    .Select(m => new PowerCampusRvoe
                    {
                        ElectronicDegreeMappingId = m.ElectronicDegreeMappingId,
                        RvoeId = m.RvoeId.Value,
                        AgreementNumber = m.Rvoe.AgreementNumber,
                        OrgCodeId = m.Rvoe.OrgCodeId,
                        Organization = m.Rvoe.ORGANIZATION.ORG_NAME_1,
                        Department = m.Rvoe.CODE_DEPARTMENT == null ? "" : m.Rvoe.CODE_DEPARTMENT.LONG_DESC,
                        DepartmentId = m.Rvoe.DepartmentId
                    }).ToArray();

            return catalog.ToArray();
        }

        /// <summary>
        /// Gets the rvoe catalog.
        /// </summary>
        /// <returns></returns>
        public CodeRvoe[] GetRvoeCatalog() => atDA.GetRvoeCatalog();

        /// <summary>
        /// Saves the authorization type mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        public void SaveAuthorizationTypeMapping(CodeAuthorizationTypeMapping mapping)
        {
            ElectronicDegreeMappingDA.DeleteElectronicDegreeMapping(mapping.ShortDescription, ElectronicDegreeTable);

            foreach (PowerCampusRvoe level in mapping.MappedRvoes)
            {
                ElectronicDegreeMapping edm = new ElectronicDegreeMapping();
                edm.ElectronicDegreeTable = ElectronicDegreeTable;
                edm.PowerCampusTable = PowerCampusTable;
                edm.ElectronicDegreeValue = mapping.ShortDescription;
                edm.RvoeId = level.RvoeId;
                edm.CreateDatetime = DateTime.Now;
                edm.CreateUserName = mapping.UserName;
                edm.RevisionDatetime = DateTime.Now;
                edm.RevisionUserName = mapping.UserName;

                ElectronicDegreeMappingDA.SaveElectronicDegreeMapping(edm);
            }
        }
    }
}