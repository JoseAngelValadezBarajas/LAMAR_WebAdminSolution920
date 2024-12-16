// --------------------------------------------------------------------
// <copyright file="Account.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace PowerCampus.Entities
{
    public enum enumAccess
    {
        Authorized = 0,
        Unauthorized = 1,
        Expired = 2,
        Error = 3
    }

    /// <summary>
    /// enumFiscalRecordAction
    /// </summary>
    public enum EnumFiscalRecordAction
    {
        ViewAll = 1,
        CashReceipts = 2,
        Menu = 3,
        Settings = 4
    }

    public class Account
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public EnumFiscalRecordAction Action { get; set; }

        /// <summary>
        /// Gets or sets the charge credit number.
        /// </summary>
        /// <value>
        /// The charge credit number.
        /// </value>
        public int ChargeCreditNumber { get; set; }

        /// <summary>
        /// Gets or sets the create date time.
        /// </summary>
        /// <value>
        /// The create date time.
        /// </value>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Gets or sets the email regular expression.
        /// </summary>
        /// <value>
        /// The email regular expression.
        /// </value>
        public string EmailRegularExpression { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public Guid GUID { get; set; }

        /// <summary>
        /// Gets or sets the institution identifier format.
        /// </summary>
        /// <value>
        /// The institution identifier format.
        /// </value>
        public string InstitutionIdFormat { get; set; }

        /// <summary>
        /// Gets or sets the invoice header identifier.
        /// </summary>
        /// <value>
        /// The invoice header identifier.
        /// </value>
        public int InvoiceHeaderId { get; set; }

        /// <summary>
        /// Gets or sets the major identifier.
        /// </summary>
        /// <value>
        /// The major identifier.
        /// </value>
        public int MajorId { get; set; }

        /// <summary>
        /// Gets or sets the operator identifier.
        /// </summary>
        /// <value>
        /// The operator identifier.
        /// </value>
        public string OperatorId { get; set; }

        /// <summary>
        /// Gets or sets the name of the operator.
        /// </summary>
        /// <value>
        /// The name of the operator.
        /// </value>
        public string OperatorName { get; set; }

        /// <summary>
        /// Gets or sets the people org code identifier.
        /// </summary>
        /// <value>
        /// The people org code identifier.
        /// </value>
        public string PeopleOrgCodeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the people org.
        /// </summary>
        /// <value>
        /// The name of the people org.
        /// </value>
        public string PeopleOrgName { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public List<Permissions> permissions { get; set; }

        /// <summary>
        /// Gets or sets the receipt number.
        /// </summary>
        /// <value>
        /// The receipt number.
        /// </value>
        public string ReceiptNumber { get; set; }

        /// <summary>
        /// Gets or sets the report formats.
        /// </summary>
        /// <value>
        /// The report formats.
        /// </value>
        public ReportFormat reportFormats { get; set; }

        /// <summary>
        /// Gets or sets the responsible identifier.
        /// </summary>
        /// <value>
        /// The responsible identifier.
        /// </value>
        public int ResponsibleId { get; set; }

        /// <summary>
        /// Gets or sets the search people org code identifier.
        /// </summary>
        /// <value>
        /// The search people org code identifier.
        /// </value>
        public string SearchPeopleOrgCodeId { get; set; }

        /// <summary>
        /// Gets or sets the signer identifier.
        /// </summary>
        /// <value>
        /// The signer identifier.
        /// </value>
        public int SignerId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public enumAccess status { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the type of the token.
        /// </summary>
        /// <value>
        /// The type of the token.
        /// </value>
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }
    }

    /// <summary>
    /// Permissions Class
    /// </summary>
    public class Permissions
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Permissions"/> is value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if value; otherwise, <c>false</c>.
        /// </value>
        public bool Value { get; set; }
    }

    /// <summary>
    /// ReportFormat Class
    /// </summary>
    public class ReportFormat
    {
        /// <summary>
        /// Gets or sets the currency format.
        /// </summary>
        /// <value>
        /// The currency format.
        /// </value>
        public string CurrencyFormat { get; set; }

        /// <summary>
        /// Gets or sets the date format.
        /// </summary>
        /// <value>
        /// The date format.
        /// </value>
        public string DateFormat { get; set; }

        /// <summary>
        /// Gets or sets the time format.
        /// </summary>
        /// <value>
        /// The time format.
        /// </value>
        public string TimeFormat { get; set; }
    }
}