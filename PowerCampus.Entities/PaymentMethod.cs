// --------------------------------------------------------------------
// <copyright file="PaymentMethod.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
namespace PowerCampus.Entities
{
    /// <summary>
    /// PaymentMethod Class
    /// </summary>
    public class PaymentMethod
    {
        /// <summary>
        /// Gets or sets the payment method code.
        /// </summary>
        /// <value>
        /// The payment method code.
        /// </value>
        public string PaymentMethodCode { get; set; }

        /// <summary>
        /// Gets or sets the payment method desc.
        /// </summary>
        /// <value>
        /// The payment method desc.
        /// </value>
        public string PaymentMethodDesc { get; set; }
    }
}