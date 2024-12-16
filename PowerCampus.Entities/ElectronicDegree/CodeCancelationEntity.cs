// --------------------------------------------------------------------
// <copyright file="CodeCancelationEntity.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.Collections.Generic;

namespace PowerCampus.Entities.ElectronicDegree
{
    /// <summary>
    /// CodeCancelationCatalog
    /// </summary>
    public class CodeCancelationCatalog
    {
        /// <summary>
        /// Gets or sets the long desc.
        /// </summary>
        /// <value>
        /// The long desc.
        /// </value>
        public string LongDesc { get; set; }

        /// <summary>
        /// Gets or sets the short desc.
        /// </summary>
        /// <value>
        /// The short desc.
        /// </value>
        public string ShortDesc { get; set; }
    }

    /// <summary>
    /// CodeCancelationEntity
    /// </summary>
    public class CodeCancelationEntity
    {
        /// <summary>
        /// Gets or sets the cancelation catalog list.
        /// </summary>
        /// <value>
        /// The cancelation catalog list.
        /// </value>
        public List<CodeCancelationCatalog> CancelationCatalogList { get; set; }

        /// <summary>
        /// Gets or sets the folio.
        /// </summary>
        /// <value>
        /// The folio.
        /// </value>
        public int Folio { get; set; }

        /// <summary>
        /// Gets or sets the long desc.
        /// </summary>
        /// <value>
        /// The long desc.
        /// </value>
        public string LongDesc { get; set; }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the short desc.
        /// </summary>
        /// <value>
        /// The short desc.
        /// </value>
        public string ShortDesc { get; set; }
    }
}