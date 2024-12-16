// --------------------------------------------------------------------
// <copyright file="CodeLegalBase.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities.ElectronicDegree
{
    /// <summary>
    /// Defines the entity for the Legal Base
    /// </summary>
    public class CodeLegalBase
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the long description.
        /// </summary>
        /// <value>
        /// The long description.
        /// </value>
        public string LongDescription { get; set; }

        /// <summary>
        /// Gets or sets the medium description.
        /// </summary>
        /// <value>
        /// The medium description.
        /// </value>
        public string MediumDescription { get; set; }
    }
}