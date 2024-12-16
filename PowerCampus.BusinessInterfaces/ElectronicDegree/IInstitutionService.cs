// --------------------------------------------------------------------
// <copyright file="IInstitutionService.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces.ElectronicDegree
{
    /// <summary>
    /// IInstitution Service
    /// </summary>
    public interface IInstitutionService
    {
        /// <summary>
        /// Createinstitutions the specified institution da model.
        /// </summary>
        /// <param name="institutionDAModel">
        /// The institution da model.
        /// </param>
        void Createinstitution(InstitutionDAModel institutionDAModel);

        /// <summary>
        /// Deletes the mapping.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void DeleteMapping(int id);

        /// <summary>
        /// Gets the institution.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        InstitutionDAModel GetInstitution(int institutionId);

        /// <summary>
        /// Gets the institution by identifier.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        InstitutionModel GetInstitutionById(int institutionId);

        /// <summary>
        /// Gets the institutions.
        /// </summary>
        /// <returns></returns>
        List<InstitutionModel> GetInstitutions();

        /// <summary>
        /// Gets the institutions detail.
        /// </summary>
        /// <returns></returns>
        List<InstitutionModel> GetInstitutionsDetail();

        /// <summary>
        /// Gets the majors by institution identifier.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        List<MajorList> GetMajorsByInstitutionId(int institutionId);

        /// <summary>
        /// Gets the power campus institutions.
        /// </summary>
        /// <returns></returns>
        List<DropDownListModel> GetPowerCampusInstitutions();

        /// <summary>
        /// Gets the rvoe options.
        /// </summary>
        /// <returns></returns>
        List<DropDownListModel> GetRvoeOptions();

        /// <summary>
        /// Maps the institution major rvoe.
        /// </summary>
        /// <param name="mapInstitutionMajorRvoe">The map institution major rvoe.</param>
        void MapInstitutionMajorRvoe(MapInstitutionMajorRvoe mapInstitutionMajorRvoe);

        /// <summary>
        /// Maps the major.
        /// </summary>
        /// <param name="majorMappingModel">The major mapping model.</param>
        void MapMajor(MajorMappingModel majorMappingModel);

        /// <summary>
        /// Updates the specified institution da model.
        /// </summary>
        /// <param name="institutionDAModel">The institution da model.</param>
        void Update(InstitutionDAModel institutionDAModel);

        /// <summary>
        /// Validates the code.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns></returns>
        bool ValidateCode(string code);

        /// <summary>
        /// Validates the folio.
        /// </summary>
        /// <param name="folio">The folio.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        bool ValidateFolio(string folio, string code);

        /// <summary>
        /// Validates the name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns></returns>
        bool ValidateName(string name);
    }
}