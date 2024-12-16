// --------------------------------------------------------------------
// <copyright file="CodeAuthorizationTypeMapping.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities.ElectronicDegree
{
    public class CodeAuthorizationTypeMapping
    {
        public string LongDescription { get; set; }
        public PowerCampusRvoe[] MappedRvoes { get; set; }
        public string MediumDescription { get; set; }
        public string ShortDescription { get; set; }
        public string UserName { get; set; }
    }

    public class PowerCampusRvoe
    {
        public string AgreementNumber { get; set; }
        public string Department { get; set; }
        public int? DepartmentId { get; set; }
        public string Description => $"{AgreementNumber} - {Organization}";
        public int ElectronicDegreeMappingId { get; set; }
        public string Organization { get; set; }
        public string OrgCodeId { get; set; }
        public int RvoeId { get; set; }
    }
}