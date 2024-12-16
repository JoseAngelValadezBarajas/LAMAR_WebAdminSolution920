// --------------------------------------------------------------------
// <copyright file="IPeopleServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;

namespace PowerCampus.BusinessInterfaces
{
    /// <summary>
    /// IPeopleService Service
    /// </summary>
    public interface IPeopleServices
    {
        /// <summary>
        /// Gets the charges.
        /// </summary>
        /// <param name="peopleOrgCodeId">The people org code identifier.</param>
        /// <param name="YearTermSession">The yts.</param>
        /// <returns></returns>
        People GetCharges(string peopleOrgCodeId, string YearTermSession);

        /// <summary>
        /// Gets the taxpayer information.
        /// </summary>
        /// <param name="peopleOrgCodeId">The people org code identifier.</param>
        /// <returns></returns>
        People GetTaxpayerInfo(string peopleOrgCodeId);

        /// <summary>
        /// Saves the taxpayer identifier.
        /// </summary>
        /// <param name="people">The people.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        int SaveTaxpayerId(People people, string userName);

        /// <summary>
        /// Years the term session.
        /// </summary>
        /// <param name="PeopleCodeId">The organization identifier.</param>
        /// <returns></returns>
        People YearTermSession(string PeopleCodeId);
    }
}