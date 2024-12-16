// --------------------------------------------------------------------
// <copyright file="IBackgroundStudyTypeServices - Copy.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicDegree;

namespace PowerCampus.BusinessInterfaces.ElectronicDegree
{
    /// <summary>
    /// Interface for Authorization Type Services
    /// </summary>
    public interface IAuthorizationTypeServices
    {
        /// <summary>
        /// Gets the current catalog.
        /// </summary>
        /// <returns></returns>
        CodeAuthorizationTypeMapping[] GetCurrentCatalog();

        /// <summary>
        /// Gets the rvoe catalog.
        /// </summary>
        /// <returns></returns>
        CodeRvoe[] GetRvoeCatalog();

        /// <summary>
        /// Saves the authorization type mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        void SaveAuthorizationTypeMapping(CodeAuthorizationTypeMapping mapping);
    }
}