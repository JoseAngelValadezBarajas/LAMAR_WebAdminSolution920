// --------------------------------------------------------------------
// <copyright file="ReceiptPaymentMapping.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.Models.ReceiptPaymentMapping
{
    /// <summary>
    /// ReceiptPaymentMethodMappingViewModel Class
    /// </summary>
    public class ReceiptPaymentMethodMappingViewModel
    {
        /// <summary>
        /// Gets or sets the charge credit code identifier.
        /// </summary>
        /// <value>
        /// The charge credit code identifier.
        /// </value>
        [ScaffoldColumn(false)]
        [Required(ErrorMessageResourceType = typeof(CashReceiptMappingModelResource), ErrorMessageResourceName = "ReceiptCodeRequired")]
        public int ChargeCreditCodeId { get; set; }

        [Display(Name = nameof(CashReceiptMappingModelResource.ReceiptCode), ResourceType = typeof(CashReceiptMappingModelResource))]
        public string ChargeCreditDesc { get; set; }

        /// <summary>
        /// Gets or sets the charge credit number.
        /// </summary>
        /// <value>
        /// The charge credit number.
        /// </value>
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the charge credit desc.
        /// </summary>
        /// <value>
        /// The charge credit desc.
        /// </value>
        [ScaffoldColumn(false)]
        [Required(ErrorMessageResourceType = typeof(CashReceiptMappingModelResource), ErrorMessageResourceName = "PaymentTypeRequired")]
        public string PaymentMethodCode { get; set; }

        /// <summary>
        /// Gets or sets the payment method desc.
        /// </summary>
        /// <value>
        /// The payment method desc.
        /// </value>
        [Display(Name = nameof(CashReceiptMappingModelResource.PaymentType), ResourceType = typeof(CashReceiptMappingModelResource))]
        public string PaymentMethodDesc { get; set; }

        /// <summary>
        /// Gets or sets the payment method list.
        /// </summary>
        /// <value>
        /// The payment method list.
        /// </value>
        public List<FiscalRecordCatalog> PaymentTypeList { get; set; }

        /// <summary>
        /// Gets or sets the receipt code list.
        /// </summary>
        /// <value>
        /// The receipt code list.
        /// </value>
        public List<FiscalRecordCatalog> ReceiptCodeList { get; set; }

        /// <summary>
        /// Gets or sets the product service desc.
        /// </summary>
        /// <value>
        /// The product service desc.
        /// </value>
        /// <summary>
        /// Gets or sets the charge credit list.
        /// </summary>
        /// <value>
        /// The charge credit list.
        /// </value>
        public List<ReceiptPaymentMethodMappingViewModel> ReceiptPaymentMethodList { get; set; }
    }
}