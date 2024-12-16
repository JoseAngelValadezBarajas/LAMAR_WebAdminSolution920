// --------------------------------------------------------------------
// <copyright file="IECResponsible.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces.ElectronicCertificate
{
    /// <summary>
    /// IECResponsibleService
    /// </summary>
    public interface IECResponsibleService
    {
        /// <summary>
        /// Creates the responsible.
        /// </summary>
        /// <param name="responsibleList">The responsible list.</param>
        void CreateResponsible(ResponsibleList responsibleList);

        /// <summary>
        /// Gets the edit responsible.
        /// </summary>
        /// <param name="responsibleId">The responsible identifier.</param>
        /// <returns></returns>
        ResponsibleList GetEditResponsible(int responsibleId);

        /// <summary>
        /// Gets the position catalog.
        /// </summary>
        /// <returns></returns>
        ResponsibleModel GetPositionCatalog();

        /// <summary>
        /// Gets the responsible names.
        /// </summary>
        /// <returns></returns>
        List<ResponsibleName> GetResponsibleNames();

        /// <summary>
        /// Gets the responsibles.
        /// </summary>
        /// <returns></returns>
        List<ResponsibleList> GetResponsibles();

        /// <summary>
        /// Determines whether [is thumbprint assigned] [the specified responsible identifier].
        /// </summary>
        /// <param name="responsibleId">The responsible identifier.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <returns>
        ///   <c>true</c> if [is thumbprint assigned] [the specified responsible identifier]; otherwise, <c>false</c>.
        /// </returns>
        bool IsThumbprintAssigned(int responsibleId, string thumbprint);

        /// <summary>
        /// Updates the responsible.
        /// </summary>
        /// <param name="responsibleList">The responsible list.</param>
        void UpdateResponsible(ResponsibleList responsibleList);

        /// <summary>
        /// Validates the curp.
        /// </summary>
        /// <param name="Curp">The curp.</param>
        /// <returns></returns>
        bool ValidateCurp(string Curp);
    }
}