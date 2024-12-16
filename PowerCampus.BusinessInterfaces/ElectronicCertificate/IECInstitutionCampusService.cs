// --------------------------------------------------------------------
// <copyright file="IInstitutionCampusService.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicCertificate;

namespace PowerCampus.BusinessInterfaces.ElectronicCertificate
{
    /// <summary>
    /// IInstitutionCampus Service
    /// </summary>
    public interface IECInstitutionCampusService
    {
        /// <summary>
        /// Creates the ec institution campus.
        /// </summary>
        /// <param name="institutionCampus">The institution campus.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        bool CreateECInstitutionCampus(InstitutionCampus institutionCampus, string operatorId);

        /// <summary>
        /// Gets the ec institution campus.
        /// </summary>
        /// <returns></returns>
        InstitutionCampuses GetECInstitutionCampus();

        /// <summary>
        /// Gets the responsible.
        /// </summary>
        /// <param name="campusSepCode">The campus sep code.</param>
        /// <returns></returns>
        int GetResponsible(string campusSepCode);

        /// <summary>
        /// Updates the ec institution campus.
        /// </summary>
        /// <param name="institutionCampus">The institution campus.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        bool UpdateECInstitutionCampus(InstitutionCampus institutionCampus, string operatorId);
    }
}