// --------------------------------------------------------------------
// <copyright file="IChargeCredits.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities;

namespace PowerCampus.BusinessInterfaces
{
    /// <summary>
    /// IChargeCredits Interface class
    /// </summary>
    public interface IChargeCreditsServices
    {
        /// <summary>
        /// Charges the credits.
        /// </summary>
        /// <param name="chargeCreditNumber">The charge credit number.</param>
        /// <returns></returns>
        ChargeCredit ChargeCredits(int chargeCreditNumber);
    }
}