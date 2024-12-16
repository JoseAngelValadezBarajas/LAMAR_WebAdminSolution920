// --------------------------------------------------------------------
// <copyright file="ElectronicDegreeInformationServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.DataAccess.ElectronicDegree;
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;

namespace PowerCampus.Business.ElectronicDegree
{
    /// <summary>
    /// Electronic Degree Information Services
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.ElectronicDegree.IElectronicDegreeInformationServices" />
    public class ElectronicDegreeInformationServices : IElectronicDegreeInformationServices
    {
        private readonly ElectronicDegreeInformationDA _electronicDegreeInformationDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElectronicDegreeInformationServices"/> class.
        /// </summary>
        public ElectronicDegreeInformationServices() => _electronicDegreeInformationDA = new ElectronicDegreeInformationDA();

        /// <summary>
        /// Deletes the electronic degree information.
        /// </summary>
        /// <param name="electronicDegreeId">The electronic degree identifier.</param>
        public void DeleteElectronicDegreeInfo(int electronicDegreeId) => _electronicDegreeInformationDA.DeleteElectronicDegree(electronicDegreeId);

        /// <summary>
        /// Gets the cancelation catalog.
        /// </summary>
        /// <returns></returns>
        public List<CodeCancelationCatalog> GetCancelationCatalog() => _electronicDegreeInformationDA.GetCancelationCatalog();

        /// <summary>
        /// Gets the electronic degree information.
        /// </summary>
        /// <param name="electronicDegreeId">The electronic degree identifier.</param>
        /// <returns></returns>
        public ElectronicDegreeInfo GetElectronicDegreeInfo(int electronicDegreeId) => _electronicDegreeInformationDA.GetElectronicDegreeInformation(electronicDegreeId);

      

        /// <summary>
        /// Gets the electronic degree information request.
        /// </summary>
        /// <param name="electronicDegreeId">The electronic degree identifier.</param>
        /// <returns></returns>
        public ElectronicDegreeInfoRequest GetElectronicDegreeInfoRequest(int electronicDegreeId) => _electronicDegreeInformationDA.GetElectronicDegreeInfoRequest(electronicDegreeId);

        /// <summary>
        /// Gets the electronic degree information.
        /// </summary>
        /// <param name="folio">The folio.</param>
        /// <param name="student">The student.</param>
        /// <param name="degreeType">Type of the degree.</param>
        /// <param name="major">The major.</param>
        /// <returns></returns>
        public List<ElectronicDegreeInfo> GetElectronicDegreeInformation(string folio, string student, string degreeType, string major) =>
            _electronicDegreeInformationDA.GetElectronicDegreeInformation(folio, student, degreeType, major);

        

        /// <summary>
        /// Inserts the electronic degree information request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public int InsertElectronicDegreeInfoRequest(ElectronicDegreeInfoRequest request) => _electronicDegreeInformationDA.InsertElectronicDegreeInformationRequest(request);

        /// <summary>
        /// Updates the electronic degree information request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public void UpdateElectronicDegreeInfoRequest(ElectronicDegreeInfoRequest request) => _electronicDegreeInformationDA.UpdateElectronicDegreeInformationRequest(request);

        /// <summary>
        /// Updates the electronic degree information status.
        /// </summary>
        /// <param name="electronicDegreeId">The electronic degree identifier.</param>
        /// <param name="status">The status.</param>
        public void UpdateElectronicDegreeInfoStatus(int electronicDegreeId, char status) => _electronicDegreeInformationDA.UpdateElectronicDegreeInfoStatus(electronicDegreeId, status);
    }
}