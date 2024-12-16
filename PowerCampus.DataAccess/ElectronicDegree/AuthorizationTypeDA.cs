// --------------------------------------------------------------------
// <copyright file="AuhtorizationTypeDA.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.Entities.ElectronicDegree;
using System.Linq;

namespace PowerCampus.DataAccess.ElectronicDegree
{
    /// <summary>
    /// Provides all the data access operations for Authorization Type
    /// </summary>
    public class AuthorizationTypeDA
    {
        /// <summary>
        /// Gets the code authorization type catalog.
        /// </summary>
        /// <returns></returns>
        public CodeAuthorizationType[] GetCodeAuthorizationTypeCatalog()
        {
            using (ElectronicDegreeContext context = new ElectronicDegreeContext())
            {
                IQueryable<CodeAuthorizationType> query =
                    from cat in context.CODE_AUTHORIZATIONTYPEs
                    where cat.STATUS == "A"
                    orderby cat.LONG_DESC
                    select new CodeAuthorizationType()
                    {
                        Id = $"{cat.CODE_VALUE_KEY}",
                        MediumDescription = cat.MEDIUM_DESC,
                        LongDescription = cat.LONG_DESC
                    };

                return query.ToArray();
            }
        }

        /// <summary>
        /// Gets the rvoe catalog.
        /// </summary>
        /// <returns></returns>
        public CodeRvoe[] GetRvoeCatalog()
        {
            using (ElectronicDegreeContext context = new ElectronicDegreeContext())
            {
                IQueryable<CodeRvoe> query =
                    from cr in context.Rvoes
                    join rd in context.DEGREQs on cr.DegreeRequirementId equals rd.DegreeRequirementId
                    join cc in context.CODE_CURRICULUMs on rd.CURRICULUM equals cc.CODE_VALUE
                    orderby cr.AgreementNumber, cc.LONG_DESC
                    select new CodeRvoe
                    {
                        RvoeId = cr.RvoeId,
                        OrgCodeId = cr.OrgCodeId,
                        Organization = cr.ORGANIZATION.ORG_NAME_1,
                        Department = cr.CODE_DEPARTMENT == null ? "" : cr.CODE_DEPARTMENT.LONG_DESC,
                        DepartmentId = cr.DepartmentId,
                        AgreementNumber = cr.AgreementNumber,
                        Program = cc.LONG_DESC
                    };

                return query.ToArray();
            }
        }
    }
}