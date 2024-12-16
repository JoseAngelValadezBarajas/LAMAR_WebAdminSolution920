// --------------------------------------------------------------------
// <copyright file="ILegalBaseServices.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicDegree;

namespace PowerCampus.BusinessInterfaces.ElectronicDegree
{
    /// <summary>
    /// Defines the contract for the Servuces of Legal Base Entity
    /// </summary>
    public interface ILegalBaseServices
    {
        /// <summary>
        /// Gets the legal base catalog.
        /// </summary>
        /// <returns></returns>
        CodeLegalBase[] GetLegalBaseCatalog();
    }
}