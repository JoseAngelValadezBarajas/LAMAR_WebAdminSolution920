// --------------------------------------------------------------------
// <copyright file="InvoiceReceipt.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace PowerCampus.Entities
{
    public class InvoiceReceipt
    {
        /// <summary>
        /// Gets or sets the EndDate.
        /// </summary>
        /// <value>
        /// Receipt EndDate.
        /// </value>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the initial Folio.
        /// </summary>
        /// <value>
        /// Initial Folio.
        /// </value>
        public int? Folio { get; set; }

        /// <summary>
        /// Invoice Expedition id Description
        /// </summary>
        public string InvoiceExpeditionDescription { get; set; }

        /// <summary>
        /// Gets or sets the InvoiceExpeditionId.
        /// </summary>
        /// <value>
        /// The InvoiceExpeditionId number.
        /// </value>
        public int? InvoiceExpeditionId { get; set; }

        /// <summary>
        /// Gets or sets the InvoiceOrganizationId.
        /// </summary>
        /// <value>
        /// The InvoiceOrganizationId number.
        /// </value>
        public int? InvoiceOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the InvoiceReceiptId.
        /// </summary>
        /// <value>
        /// The InvoiceReceiptId number.
        /// </value>
        public int? InvoiceReceiptId { get; set; }

        /// <summary>
        /// Gets or sets the Last Folio Assigned.
        /// </summary>
        /// <value>
        /// Last Folio Assigned number.
        /// </value>
        public int? LastFolioAssigned { get; set; }

        /// <summary>
        /// All Expidition belongs to this Documnet.
        /// </summary>
        public List<InvoiceExpedition> ListInvoiceExpedition { get; set; }

        /// <summary>
        /// Gets or sets the Receipt Serial Number.
        /// </summary>
        /// <value>
        /// Receipt Serial Number.
        /// </value>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// Receipt Description.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }
    }
}