// --------------------------------------------------------------------
// <copyright file="TaxProfileValidity.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace PowerCampus.Entities
{
    /// <summary>
    /// TaxProfileValidity
    /// </summary>
    public class TaxProfileValidity
    {
        /// <summary>
        /// Gets or sets the end date time.
        /// </summary>
        /// <value>
        /// The end date time.
        /// </value>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the start date time.
        /// </summary>
        /// <value>
        /// The start date time.
        /// </value>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the tax profile detail list.
        /// </summary>
        /// <value>
        /// The tax profile detail list.
        /// </value>
        public List<TaxProfileDetail> TaxProfileDetailList { get; set; }

        public int TaxProfileId { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int TaxProfileValidityId { get; set; }
    }
}