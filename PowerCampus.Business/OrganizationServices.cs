// --------------------------------------------------------------------
// <copyright file="OrganizationServices.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.BusinessInterfaces;
using PowerCampus.DataAccess;
using PowerCampus.Entities;

namespace PowerCampus.Business
{
    /// <summary>
    /// OrganizationServices class
    /// </summary>
    public class OrganizationServices : IOrganizationServices
    {
        private readonly OrganizationDA _organizationDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationServices"/> class.
        /// </summary>
        public OrganizationServices()
        {
            _organizationDA = new OrganizationDA();
        }

        /// <summary>
        /// Chargeses the specified organization identifier.
        /// </summary>
        /// <param name="OrganizationId">The organization identifier.</param>
        /// <param name="YearTermSession">The yts.</param>
        /// <returns></returns>
        public Organization Charges(string OrganizationId, string YearTermSession)
        {
            // _logService.Debug("Charges starts");
            Organization organization = _organizationDA.GetCharges(OrganizationId, YearTermSession);
            // _logService.Debug("Charges ends");
            return organization;
        }

        /// <summary>
        /// Gets the taxpayer information.
        /// </summary>
        /// <param name="organizationCodeId">The organization code identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Organization GetTaxpayerInfo(string organizationCodeId)
        {
            // _logService.Debug("GetTaxpayerInfo starts");
            Organization organization = _organizationDA.GetTaxpayerInfo(organizationCodeId);
            // _logService.Debug("GetTaxpayerInfo ends");
            return organization;
        }

        /// <summary>
        /// Saves the taxpayer identifier.
        /// </summary>
        /// <param name="organization">The organization.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int SaveTaxpayerId(Organization organization, string userName)
        {
            // _logService.Debug("SaveTaxpayerId starts");
            int taxpayerId = _organizationDA.SaveTaxpayerId(organization, userName);
            // _logService.Debug("SaveTaxpayerId ends");
            return taxpayerId;
        }

        /// <summary>
        /// Years the term session.
        /// </summary>
        /// <param name="OrganziationId">The organziation identifier.</param>
        /// <returns></returns>
        public Organization YearTermSession(string OrganziationId)
        {
            // _logService.Debug("YearTermSession starts");
            Organization organization = _organizationDA.GetYearTermSession(OrganziationId);
            // _logService.Debug("YearTermSession ends");
            return organization;
        }
    }
}