// --------------------------------------------------------------------
// <copyright file="CodeFederalEntitityMapping.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities.ElectronicCertificate
{
    public class CodeFederalEntitityMapping
    {
        public string LongDescription { get; set; }
        public PowerCampusCodeState[] MappedStates { get; set; }
        public string MediumDescription { get; set; }
        public string ShortDescription { get; set; }
        public string UserName { get; set; }
    }

    public class PowerCampusCodeState
    {
        public string CodeStateId { get; set; }

        public int ElectronicDegreeMappingId { get; set; }

        public string LongDescription { get; set; }

        public string MediumDescription { get; set; }

        public string ShortDescription { get; set; }
    }
}