// --------------------------------------------------------------------
// <copyright file="CustomDateFormat.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace WebAdminUI.Helpers
{
    /// <summary>
    /// CustomEmailRegularExpression
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.RegularExpressionAttribute" />
    public class CustomEmailRegularExpression : RegularExpressionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomEmailRegularExpression"/> class.
        /// </summary>
        public CustomEmailRegularExpression() : base(PowerCampusSystemFormat.GetMailValidation()) => this.ErrorMessage = Views.FiscalRecords.App_LocalResources.CreateResource.ErrorEmail;
    }
}