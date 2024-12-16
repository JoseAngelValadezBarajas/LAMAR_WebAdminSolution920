// --------------------------------------------------------------------
// <copyright file="CodeBackgroundStudyTypeMapping.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities.ElectronicDegree
{
    public class CodeBackgroundStudyTypeMapping
    {
        public string LongDescription { get; set; }
        public PowerCampusScholarshipLevel[] MappedLevels { get; set; }
        public string MediumDescription { get; set; }
        public string ShortDescription { get; set; }
        public string UserName { get; set; }
    }

    public class PowerCampusScholarshipLevel
    {
        public int ElectronicDegreeMappingId { get; set; }
        public string LongDescription { get; set; }
        public string MediumDescription { get; set; }
        public string ScholarshipLevelId { get; set; }
        public string ShortDescription { get; set; }
    }
}