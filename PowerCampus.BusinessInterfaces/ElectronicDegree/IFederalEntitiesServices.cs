// --------------------------------------------------------------------
// <copyright file="IFederalEntitiesServices.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces.ElectronicDegree
{
    /// <summary>
    /// Interface for the Federal Entities Services
    /// </summary>
    public interface IFederalEntitiesServices
    {
        /// <summary>
        /// Gets the code catalog.
        /// </summary>
        /// <returns></returns>
        List<Entities.ElectronicCertificate.CodeTable> GetCodeCatalog();

        /// <summary>
        /// Gets the code catalog without foreign.
        /// </summary>
        /// <returns></returns>
        List<Entities.ElectronicCertificate.CodeTable> GetCodeCatalogWithoutForeign();

        /// <summary>
        /// Gets the current catalog.
        /// </summary>
        /// <returns></returns>
        CodeFederalEntitityMapping[] GetCurrentCatalog();

        /// <summary>
        /// Gets the states catalog.
        /// </summary>
        /// <returns></returns>
        CodeState[] GetStatesCatalog();

        /// <summary>
        /// Saves the federal entity mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        void SaveFederalEntityMapping(CodeFederalEntitityMapping mapping);
    }
}