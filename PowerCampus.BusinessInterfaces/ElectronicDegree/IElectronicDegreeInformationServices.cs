// --------------------------------------------------------------------
// <copyright file="IElectronicDegreeInformationServices.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces.ElectronicDegree
{
    /// <summary>
    /// IElectronic Degree Information Services
    /// </summary>
    public interface IElectronicDegreeInformationServices
    {
        /// <summary>
        /// Deletes the electronic degree information.
        /// </summary>
        /// <param name="electronicDegreeId">The electronic degree identifier.</param>
        void DeleteElectronicDegreeInfo(int electronicDegreeId);

        /// <summary>
        /// Gets the cancelation catalog.
        /// </summary>
        /// <returns></returns>
        List<CodeCancelationCatalog> GetCancelationCatalog();

        /// <summary>
        /// Gets the electronic degree information.
        /// </summary>
        /// <param name="electronicDegreeId">The electronic degree identifier.</param>
        /// <returns></returns>
        ElectronicDegreeInfo GetElectronicDegreeInfo(int electronicDegreeId);

        /// <summary>
        /// Gets the electronic degree information request.
        /// </summary>
        /// <param name="electronicDegreeId">The electronic degree identifier.</param>
        /// <returns></returns>
        ElectronicDegreeInfoRequest GetElectronicDegreeInfoRequest(int electronicDegreeId);

        /// <summary>
        /// Gets the electronic degree information.
        /// </summary>
        /// <param name="folio">The folio.</param>
        /// <param name="student">The student.</param>
        /// <param name="degreeType">Type of the degree.</param>
        /// <param name="major">The major.</param>
        /// <returns></returns>
        List<ElectronicDegreeInfo> GetElectronicDegreeInformation(string folio, string student, string degreeType, string major);

       
        /// <summary>
        /// Inserts the electronic degree information request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        int InsertElectronicDegreeInfoRequest(ElectronicDegreeInfoRequest request);

        /// <summary>
        /// Updates the electronic degree information request.
        /// </summary>
        /// <param name="request">The request.</param>
        void UpdateElectronicDegreeInfoRequest(ElectronicDegreeInfoRequest request);

        /// <summary>
        /// Updates the electronic degree information.
        /// </summary>
        /// <param name="electronicDegreeId">The electronic degree identifier.</param>
        /// <param name="value">The value.</param>
        void UpdateElectronicDegreeInfoStatus(int electronicDegreeId, char value);
    }
}