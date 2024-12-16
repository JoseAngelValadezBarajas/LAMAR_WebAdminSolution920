// --------------------------------------------------------------------
// <copyright file="FiscalRecordRequest.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities
{
    public class FiscalRecordRequest
    {
        /// <summary>
        /// Gets or sets the cancel reason key.
        /// </summary>
        /// <value>
        /// The cancel reason key.
        /// </value>
        public string CancelReasonKey { get; set; }

        /// <summary>
        /// Gets or sets the invoice header identifier.
        /// </summary>
        /// <value>
        /// The invoice header identifier.
        /// </value>
        public int InvoiceHeaderId { get; set; }

        /// <summary>
        /// Gets or sets the invoice payment receipt identifier.
        /// </summary>
        /// <value>
        /// The invoice payment receipt identifier
        /// </value>
        public int? InvoicePaymentReceiptId { get; set; }

        /// <summary>
        /// Gets or sets the invoice request identifier.
        /// </summary>
        /// <value>
        /// The invoice request identifier.
        /// </value>
        public int InvoiceRequestId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status { get; set; }
    }
}