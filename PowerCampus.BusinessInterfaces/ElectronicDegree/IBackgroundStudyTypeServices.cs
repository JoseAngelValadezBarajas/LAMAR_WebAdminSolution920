// --------------------------------------------------------------------
// <copyright file="IBackgroundStudyTypeServices.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicDegree;

namespace PowerCampus.BusinessInterfaces.ElectronicDegree
{
    /// <summary>
    /// Interface for Background Study Type Services
    /// </summary>
    public interface IBackgroundStudyTypeServices
    {
        /// <summary>
        /// Gets the current catalog.
        /// </summary>
        /// <returns></returns>
        CodeBackgroundStudyTypeMapping[] GetCurrentCatalog();

        /// <summary>
        /// Gets the scholarship levels catalog.
        /// </summary>
        /// <returns></returns>
        CodeScholarshipLevel[] GetScholarshipLevelsCatalog();

        /// <summary>
        /// Saves the scholarship level mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        void SaveScholarshipLevelMapping(CodeBackgroundStudyTypeMapping mapping);
    }
}