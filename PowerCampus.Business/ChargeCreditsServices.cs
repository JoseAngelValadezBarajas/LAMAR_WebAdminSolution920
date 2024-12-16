// --------------------------------------------------------------------
// <copyright file="ChargeCreditsServices.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.BusinessInterfaces;
using PowerCampus.DataAccess;
using PowerCampus.Entities;

namespace PowerCampus.Business
{
    /// <summary>
    /// ChargeCredits
    /// </summary>
    public class ChargeCreditsServices : IChargeCreditsServices
    {
        private readonly ChargeCreditsDA _chargeCreditsDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChargeCreditsServices"/> class.
        /// </summary>
        public ChargeCreditsServices()
        {
            _chargeCreditsDA = new ChargeCreditsDA();
        }

        /// <summary>
        /// Charges the credits.
        /// </summary>
        /// <param name="chargeCreditNumber">The charge credit number.</param>
        /// <returns></returns>
        public ChargeCredit ChargeCredits(int chargeCreditNumber)
        {
            // LoggerHelper.LogWebError("FiscalRecords", "ChargeCredits", "ChargeCredits starts");
            ChargeCredit ChargeCredit = _chargeCreditsDA.ChargeCredits(chargeCreditNumber);
            // LoggerHelper.LogWebError("FiscalRecords", "ChargeCredits", "ChargeCredits ends");
            return ChargeCredit;
        }
    }
}