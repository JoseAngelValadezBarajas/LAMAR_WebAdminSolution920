// --------------------------------------------------------------------
// <copyright file="Constants.cs" company="Ellucian">
//     Copyright 2020 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
namespace PowerCampus.Entities
{
    /// <summary>
    /// Constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The current CFDI version
        /// </summary>
        public const double CurrentCfdiVersion = 4.0d;

        /// <summary>
        /// The data access
        /// </summary>
        public const string DataAccess = "PowerCampus.DataAccess";

        /// <summary>
        /// The default currency
        /// </summary>
        public const string DefaultCurrency = "MXN";

        /// <summary>
        /// The ec area
        /// </summary>
        public const string EcArea = "ElectronicCertificate";

        /// <summary>
        /// The ec business
        /// </summary>
        public const string EcBusiness = "PowerCampus.Business";

        /// <summary>
        /// The ed area
        /// </summary>
        public const string EdArea = "ElectronicDegree";

        /// <summary>
        /// The fiscal record type egreso
        /// </summary>
        public const string FiscalRecordTypeEgreso = "E";

        /// <summary>
        /// The fiscal record type egreso desc
        /// </summary>
        public const string FiscalRecordTypeEgresoDesc = "Egreso";

        /// <summary>
        /// The fiscal record type ingreso
        /// </summary>
        public const string FiscalRecordTypeIngreso = "I";

        /// <summary>
        /// The fiscal record type ingreso desc
        /// </summary>
        public const string FiscalRecordTypeIngresoDesc = "Ingreso";

        /// <summary>
        /// The unit default value for credit note
        /// </summary>
        public const string UnitDefaultForCreditNote = "Actividad";

        /// <summary>
        /// The fiscal record type pago
        /// </summary>
        public const string FiscalRecordTypePago = "P";

        /// <summary>
        /// The fiscal record type pago desc
        /// </summary>
        public const string FiscalRecordTypePagoDesc = "Pago";

        /// <summary>
        /// The foreign tax payer identifier
        /// </summary>
        public const string ForeignTaxPayerId = "XEXX010101000";

        /// <summary>
        /// The global receiver tax payer identifier
        /// </summary>
        public const string GlobalReceiverTaxPayerId = "XAXX010101000";

        /// <summary>
        /// The iva 16
        /// </summary>
        public const decimal IVA16 = 0.16m;

        /// <summary>
        /// The iva 8
        /// </summary>
        public const decimal IVA8 = 0.08m;

        /// <summary>
        /// Gets the payment method.
        /// </summary>
        /// <value>
        /// The payment method.
        /// </value>
        public const string PaymentMethod = "PUE";

        /// <summary>
        /// The payment receipt cfdi usage
        /// </summary>
        public const string PaymentReceiptCFDIUsage = "CP01";

        /// <summary>
        /// The payment receipt charge credit c desc
        /// </summary>
        public const string PaymentReceiptChargeCreditCDesc = "Pago";

        /// <summary>
        /// The payment receipt charge credit code
        /// </summary>
        public const string PaymentReceiptChargeCreditCode = "84111506";

        /// <summary>
        /// The payment receipt currency
        /// </summary>
        public const string PaymentReceiptCurrency = "XXX";

        /// <summary>
        /// The payment receipt unity key
        /// </summary>
        public const string PaymentReceiptUnityKey = "ACT";

        /// <summary>
        /// The PPD payment method
        /// </summary>
        public const string PPDPaymentMethod = "PPD";

        /// <summary>
        /// The PPD payment type
        /// </summary>
        public const string PPDPaymentType = "99";

        /// <summary>
        /// The previous CFDI version
        /// </summary>
        public const double PreviousCfdiVersion = 3.3d;

        /// <summary>
        /// The web admin services
        /// </summary>
        public const string WebAdminServices = "WebAdminServices";

        /// <summary>
        /// The web admin UI
        /// </summary>
        public const string WebAdminUi = "WebAdminUI";
    }
}