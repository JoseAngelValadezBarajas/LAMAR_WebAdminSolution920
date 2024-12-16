// --------------------------------------------------------------------
// <copyright file="ReceiptPaymentMethodMapping.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

namespace PowerCampus.Entities
{
    public class ReceiptPaymentMethodMapping
    {
        /// <summary>
        /// Gets or sets the charge credit code identifier.
        /// </summary>
        /// <value>
        /// The charge credit code identifier.
        /// </value>
        public int ChargeCreditCodeId { get; set; }

        /// <summary>
        /// Gets or sets the charge credit desc.
        /// </summary>
        /// <value>
        /// The charge credit desc.
        /// </value>
        public string ChargeCreditDesc { get; set; }

        /// <summary>
        /// Gets or sets the payment method desc.
        /// </summary>
        /// <value>
        /// The payment method desc.
        /// </value>
        public string PaymentMethodDesc { get; set; }


        /// <summary>
        /// Gets or sets the payment method code.
        /// </summary>
        /// <value>
        /// The payment method code.
        /// </value>
        public string PaymentMethodCode { get; set; }

        /// <summary>
        /// Gets or sets the receipt payment method mapping identifier.
        /// </summary>
        /// <value>
        /// The receipt payment method mapping identifier.
        /// </value>
        public int ReceiptPaymentMethodMappingId { get; set; }
    }
}