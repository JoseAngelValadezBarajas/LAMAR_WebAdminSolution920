// --------------------------------------------------------------------
// <copyright file="IIssuerServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IIssuerServices
    {
        /// <summary>
        /// Creates the issuer set up.
        /// </summary>
        /// <param name="issuerDefault">The issuer default.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        int CreateIssuerSetUp(IssuerDefault issuerDefault, string userName);

        /// <summary>
        /// Delete issuer Address setup
        /// </summary>
        /// <param name="id">Expedition Id</param>
        /// <returns>Sucess or failure</returns>
        int DeleteIssuingAddress(int id);

        /// <summary>
        /// Gets the tax payers.
        /// </summary>
        /// <param name="prefer">The prefer.</param>
        /// <returns></returns>
        List<Issuer> GetAllIssuers(string prefer);

        /// <summary>
        /// Gets the issuer.
        /// </summary>
        /// <param name="InvoiceTaxpayerId">The taxpayer identifier.</param>
        /// <returns></returns>
        Issuer GetIssuer(int InvoiceTaxpayerId);

        /// <summary>
        /// Gets the issuer by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        InvoiceOrganization GetIssuerById(int id);

        /// <summary>
        /// Gets the tax payers.
        /// </summary>
        /// <param name="prefer">The prefer.</param>
        /// <returns></returns>
        List<InvoiceOrganization> GetIssuers(string prefer);

        /// <summary>
        /// Gets the issuer serial number.
        /// </summary>
        /// <param name="InvoiceTaxpayerId">The invoice taxpayer identifier.</param>
        /// <param name="InvoiceExpeditionId"></param>
        /// <returns></returns>
        Issuer GetIssuerSerialNumber(int InvoiceTaxpayerId, int? InvoiceExpeditionId);

        /// <summary>
        /// Get Issuing Address
        /// </summary>
        /// <param name="id"> TaxPayer Id</param>
        /// <param name="invoiceExpeditionId">invoiceExpeditionId is optional</param>
        /// <returns></returns>
        List<InvoiceExpedition> GetIssuingAddress(int id, int? invoiceExpeditionId);

        /// <summary>
        /// Get List of Doucument Setup
        /// </summary>
        /// <param name="id"></param>
        /// <param name="invoiceReceiptId"></param>
        /// <returns></returns>
        List<InvoiceReceipt> GetIssuingReceipt(int id, int? invoiceReceiptId);

        /// <summary>
        /// Create New Issuing Address For Fiscal Information.
        /// </summary>
        /// <param name="issuerAddress"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        int SaveIssuingAddress(InvoiceExpedition issuerAddress, string userName);

        /// <summary>
        /// updates doucumnet setup
        /// </summary>
        /// <param name="issuer"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        int SaveIssuingReceipt(InvoiceReceipt issuer, string userName);

        /// <summary>
        /// Create New Issuer Fiscal Information.
        /// </summary>
        /// <param name="issuer"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        int SaveTaxPayers(InvoiceOrganization issuer, string userName);

        /// <summary>
        /// Selects the issuer set up.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        IssuerDefault SelectIssuerSetUp(string userName);
    }
}