// --------------------------------------------------------------------
// <copyright file="CodeRvoe.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities.ElectronicDegree
{
    public class CodeRvoe
    {
        /// <summary>
        /// Gets or sets the agreement number.
        /// </summary>
        /// <value>
        /// The agreement number.
        /// </value>
        public string AgreementNumber { get; set; }

        /// <summary>
        /// Gets or sets the department.
        /// </summary>
        /// <value>
        /// The department.
        /// </value>
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets the department identifier.
        /// </summary>
        /// <value>
        /// The department identifier.
        /// </value>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description => $"{AgreementNumber} - {Program}";

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        /// <value>
        /// The organization.
        /// </value>
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets the org code identifier.
        /// </summary>
        /// <value>
        /// The org code identifier.
        /// </value>
        public string OrgCodeId { get; set; }

        /// <summary>
        /// Gets or sets the program.
        /// </summary>
        /// <value>
        /// The program.
        /// </value>
        public string Program { get; set; }

        /// <summary>
        /// Gets or sets the rvoe identifier.
        /// </summary>
        /// <value>
        /// The rvoe identifier.
        /// </value>
        public int RvoeId { get; set; }
    }
}