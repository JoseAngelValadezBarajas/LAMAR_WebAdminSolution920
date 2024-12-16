// --------------------------------------------------------------------
// <copyright file="IOrganizationServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;

namespace PowerCampus.BusinessInterfaces
{
    /// <summary>
    /// IOrganizationServices class
    /// </summary>
    public interface IOrganizationServices
    {
        /// <summary>
        /// Chargeses the specified organization identifier.
        /// </summary>
        /// <param name="OrganizationId">The organization identifier.</param>
        /// <param name="YearTermSession">The yts.</param>
        /// <returns></returns>
        Organization Charges(string OrganizationId, string YearTermSession);

        /// <summary>
        /// Gets the taxpayer information.
        /// </summary>
        /// <param name="organizationCodeId">The organization code identifier.</param>
        /// <returns></returns>
        Organization GetTaxpayerInfo(string organizationCodeId);

        /// <summary>
        /// Saves the taxpayer identifier.
        /// </summary>
        /// <param name="organization">The organization.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        int SaveTaxpayerId(Organization organization, string userName);

        /// <summary>
        /// Years the term session.
        /// </summary>
        /// <param name="OrganizationId">The organization identifier.</param>
        /// <returns></returns>
        Organization YearTermSession(string OrganizationId);
    }
}