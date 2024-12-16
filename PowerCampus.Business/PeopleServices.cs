// --------------------------------------------------------------------
// <copyright file="PeopleServices.cs" company="Ellucian">
//     Copyright 2017-2020 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.BusinessInterfaces;
using PowerCampus.DataAccess;
using PowerCampus.Entities;

namespace PowerCampus.Business
{
    /// <summary>
    /// PeopleServices Class
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.IPeopleServices" />
    public class PeopleServices : IPeopleServices
    {
        private readonly PeopleDA _peopleDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchServices"/> class.
        /// </summary>
        public PeopleServices()
        {
            _peopleDA = new PeopleDA();
        }

        /// <summary>
        /// Gets the charges.
        /// </summary>
        /// <param name="PeopleCodeId">The people code identifier.</param>
        /// <param name="YearTermSession">The yts.</param>
        /// <returns></returns>
        public People GetCharges(string PeopleCodeId, string YearTermSession)
        {
            // _logService.Debug("GetCharges starts");
            People people = _peopleDA.GetCharges(PeopleCodeId, YearTermSession);
            // _logService.Debug("GetCharges ends");
            return people;
        }

        /// <summary>
        /// Gets the taxpayer information.
        /// </summary>
        /// <param name="peopleOrgCodeId">The people org code identifier.</param>
        /// <returns></returns>
        public People GetTaxpayerInfo(string peopleOrgCodeId)
        {
            // _logService.Debug("GetTaxpayerInfo starts");
            People people = _peopleDA.GetTaxpayerInfo(peopleOrgCodeId);
            // _logService.Debug("GetTaxpayerInfo ends");
            return people;
        }

        /// <summary>
        /// Saves the taxpayer identifier.
        /// </summary>
        /// <param name="people">The people.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int SaveTaxpayerId(People people, string userName)
        {
            // _logService.Debug("SaveTaxpayerId starts");
            int taxpayerId = _peopleDA.SaveTaxpayerId(people, userName);
            // _logService.Debug("SaveTaxpayerId ends");
            return taxpayerId;
        }

        /// <summary>
        /// Years the term session.
        /// </summary>
        /// <param name="PeopleCodeId">The people code identifier.</param>
        /// <returns></returns>
        public People YearTermSession(string PeopleCodeId)
        {
            // _logService.Debug("YearTermSession starts");
            People people = _peopleDA.GetYearTermSession(PeopleCodeId);
            // _logService.Debug("YearTermSession ends");
            return people;
        }
    }
}