// --------------------------------------------------------------------
// <copyright file="IssuerServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.BusinessInterfaces;
using PowerCampus.DataAccess;
using PowerCampus.Entities;
using System.Collections.Generic;

namespace PowerCampus.Business
{
    /// <summary>
    /// Issuer Services class
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.IIssuerServices" />
    public class IssuerServices : IIssuerServices
    {
        private readonly IssuerDA _issuerDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="IssuerServices"/> class.
        /// </summary>
        public IssuerServices()
        {
            _issuerDA = new IssuerDA();
        }

        /// <summary>
        /// Creates the issuer set up.
        /// </summary>
        /// <param name="issuerDefault">The issuer default.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public int CreateIssuerSetUp(IssuerDefault issuerDefault, string userName)
        {
            // _logService.Debug("CreateIssuerSetUp starts");
            int issuerSetup = _issuerDA.CreateIssuerSetUp(issuerDefault, userName);
            // _logService.Debug("CreateIssuerSetUp ends");
            return issuerSetup;
        }

        /// <summary>
        /// Delete issuer Address setup
        /// </summary>
        /// <param name="id">Expedition Id</param>
        /// <returns>
        /// Sucess or failure
        /// </returns>
        public int DeleteIssuingAddress(int id)
        {
            // _logService.Debug("DeleteIssuingAddress starts");
            int issuerAddress = _issuerDA.DeleteIssuingAddress(id);
            // _logService.Debug("DeleteIssuingAddress ends");
            return issuerAddress;
        }

        /// <summary>
        /// Gets the tax payers.
        /// </summary>
        /// <param name="prefer">The prefer.</param>
        /// <returns></returns>
        public List<Issuer> GetAllIssuers(string prefer)
        {
            // _logService.Debug("GetAllIssuers starts");
            List<Issuer> lstIssuer = _issuerDA.GetAllIssuers(prefer);
            // _logService.Debug("GetAllIssuers ends");
            return lstIssuer;
        }

        /// <summary>
        /// Gets the issuer.
        /// </summary>
        /// <param name="InvoiceTaxpayerId">The tax payer identifier.</param>
        /// <returns></returns>
        public Issuer GetIssuer(int InvoiceTaxpayerId)
        {
            // _logService.Debug("GetIssuer starts");
            Issuer issuer = _issuerDA.GetIssuerTaxRegimen(InvoiceTaxpayerId);
            // _logService.Debug("GetIssuer ends");
            return issuer;
        }

        /// <summary>
        /// Gets the issuer by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public InvoiceOrganization GetIssuerById(int id)
        {
            // _logService.Debug("GetIssuerById starts");
            InvoiceOrganization invoiceOrganization = _issuerDA.GetIssuersById(id);
            // _logService.Debug("GetIssuerById ends");
            return invoiceOrganization;
        }

        /// <summary>
        /// Gets the tax payers.
        /// </summary>
        /// <param name="prefer">The prefer.</param>
        /// <returns></returns>
        public List<InvoiceOrganization> GetIssuers(string prefer)
        {
            // _logService.Debug("GetIssuers starts");
            List<InvoiceOrganization> lstInvoiceOrganization = _issuerDA.GetIssuers(prefer);
            // _logService.Debug("GetIssuers ends");
            return lstInvoiceOrganization;
        }

        /// <summary>
        /// Gets the issuer serial number.
        /// </summary>
        /// <param name="InvoiceTaxpayerId">The invoice taxpayer identifier.</param>
        /// <param name="InvoiceExpeditionId"></param>
        /// <returns></returns>
        public Issuer GetIssuerSerialNumber(int InvoiceTaxpayerId, int? InvoiceExpeditionId)
        {
            // _logService.Debug("GetIssuerSerialNumber starts");
            Issuer issuer = _issuerDA.GetIssuerSerialNumber(InvoiceTaxpayerId, InvoiceExpeditionId);
            // _logService.Debug("GetIssuerSerialNumber ends");
            return issuer;
        }

        /// <summary>
        /// Get Issuing Address
        /// </summary>
        /// <param name="id"> TaxPayer Id</param>
        /// <param name="invoiceExpeditionId">invoiceExpeditionId is optional</param>
        /// <returns></returns>
        public List<InvoiceExpedition> GetIssuingAddress(int id, int? invoiceExpeditionId)
        {
            // _logService.Debug("GetIssuingAddress starts");
            List<InvoiceExpedition> lstInvoiceExpedition = _issuerDA.GetIssuingAddress(id, invoiceExpeditionId);
            // _logService.Debug("GetIssuingAddress ends");
            return lstInvoiceExpedition;
        }

        /// <summary>
        /// get list of documentsetups
        /// </summary>
        /// <param name="id"></param>
        /// <param name="invoiceReceiptId"></param>
        /// <returns></returns>
        public List<InvoiceReceipt> GetIssuingReceipt(int id, int? invoiceReceiptId)
        {
            // _logService.Debug("GetIssuingReceipt starts");
            List<InvoiceReceipt> lstInvoiceReceipt = _issuerDA.GetIssuingReceipt(id, invoiceReceiptId);
            // _logService.Debug("GetIssuingReceipt ends");
            return lstInvoiceReceipt;
        }

        /// <summary>
        /// Create New Issuing Address For Fiscal Information.
        /// </summary>
        /// <param name="issuerAddress"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int SaveIssuingAddress(InvoiceExpedition issuerAddress, string userName)
        {
            // _logService.Debug("SaveIssuingAddress starts");
            int expeditionAddressId = _issuerDA.SaveIssuingAddress(issuerAddress, userName);
            // _logService.Debug("SaveIssuingAddress ends");
            return expeditionAddressId;
        }

        /// <summary>
        /// Update documnet setup
        /// </summary>
        /// <param name="issuer"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int SaveIssuingReceipt(InvoiceReceipt issuer, string userName)
        {
            // _logService.Debug("SaveIssuingReceipt starts");
            int issuerReceipt = _issuerDA.SaveIssuingReceipt(issuer, userName);
            // _logService.Debug("SaveIssuingReceipt ends");
            return issuerReceipt;
        }

        /// <summary>
        /// Create new Issuer Fiscal Information.
        /// </summary>
        /// <param name="issuer"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int SaveTaxPayers(InvoiceOrganization issuer, string userName)
        {
            // _logService.Debug("SaveTaxPayers starts");
            int taxpayerId = _issuerDA.SaveTaxPayers(issuer, userName);
            // _logService.Debug("SaveTaxPayers ends");
            return taxpayerId;
        }

        /// <summary>
        /// Selects the issuer set up.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public IssuerDefault SelectIssuerSetUp(string userName)
        {
            // _logService.Debug("SelectIssuerSetUp starts");
            IssuerDefault issuerDefault = _issuerDA.SelectIssuerSetUpData(userName);
            // _logService.Debug("SelectIssuerSetUp ends");
            return issuerDefault;
        }
    }
}