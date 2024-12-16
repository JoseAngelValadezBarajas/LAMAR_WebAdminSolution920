// --------------------------------------------------------------------
// <copyright file="ECInstitutionCampusService.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.BusinessInterfaces.ElectronicCertificate;
using PowerCampus.DataAccess.ElectronicCertificate;
using PowerCampus.Entities.ElectronicCertificate;

namespace PowerCampus.Business.ElectronicCertificate
{
    /// <summary>
    /// InstitutionCampus Service
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.ElectronicCertificate.IECInstitutionCampusService" />
    /// <seealso cref="PowerCampus.BusinessInterfaces.ElectronicDegree.IInstitutionService" />
    public class ECInstitutionCampusService : IECInstitutionCampusService
    {
        /// <summary>
        /// The institution campus da
        /// </summary>
        private readonly ECInstitutionCampusDA _institutionCampusDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="ECInstitutionCampusService" /> class.
        /// </summary>
        public ECInstitutionCampusService() => _institutionCampusDA = new ECInstitutionCampusDA();

        /// <summary>
        /// Creates the ec institution campus.
        /// </summary>
        /// <param name="institutionCampus">The institution campus.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public bool CreateECInstitutionCampus(InstitutionCampus institutionCampus, string operatorId) => _institutionCampusDA.CreateECInstitutionCampus(institutionCampus, operatorId);

        /// <summary>
        /// Gets the electronic certificate institution campus.
        /// </summary>
        /// <returns></returns>
        public InstitutionCampuses GetECInstitutionCampus() => _institutionCampusDA.GetECInstitutionCampus();

        /// <summary>
        /// Gets the responsible.
        /// </summary>
        /// <param name="campusSepCode">The campus sep code.</param>
        /// <returns></returns>
        public int GetResponsible(string campusSepCode)
        {
            if (string.IsNullOrEmpty(campusSepCode))
            {
                return 0;
            }
            return _institutionCampusDA.GetResponsible(campusSepCode);
        }

        /// <summary>
        /// Updates the ec institution campus.
        /// </summary>
        /// <param name="institutionCampus">The institution campus.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public bool UpdateECInstitutionCampus(InstitutionCampus institutionCampus, string operatorId) => _institutionCampusDA.UpdateECInstitutionCampus(institutionCampus, operatorId);
    }
}