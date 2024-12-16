// --------------------------------------------------------------------
// <copyright file="InstitutionService.cs" company="Ellucian">
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
    /// Institution Service
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.ElectronicDegree.IInstitutionService" />
    public class InstitutionService : IInstitutionService
    {
        private readonly InstitutionDA _institutionDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstitutionService"/> class.
        /// </summary>
        public InstitutionService() => _institutionDA = new InstitutionDA();

        /// <summary>
        /// Creates the institution.
        /// </summary>
        /// <param name="institutionDAModel">The major list.</param>
        public void Createinstitution(InstitutionDAModel institutionDAModel) => _institutionDA.CreateInstitution(institutionDAModel);

        /// <summary>
        /// Deletes the mapping.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void DeleteMapping(int id) => _institutionDA.DeleteMapping(id);

        /// <summary>
        /// Gets the institution.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        public InstitutionDAModel GetInstitution(int institutionId) => _institutionDA.GetInstitution(institutionId);

        /// <summary>
        /// Gets the institution by identifier.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        public InstitutionModel GetInstitutionById(int institutionId) => _institutionDA.GetInstitutionById(institutionId);

        /// <summary>
        /// Gets the institutions.
        /// </summary>
        /// <returns></returns>
        public List<InstitutionModel> GetInstitutions()
        {
            List<InstitutionModel> institutionModel = _institutionDA.GetInstitutions();

            foreach (InstitutionModel instModel in institutionModel)
            {
                int signers = _institutionDA.GetInstitutionsSigners(instModel.Id);
                instModel.Signers = signers;
                int majors = _institutionDA.GetInstitutionsMajors(instModel.Id);
                instModel.Majors = majors;
            }

            return institutionModel;
        }

        /// <summary>
        /// Gets the institutions detail.
        /// </summary>
        /// <returns></returns>
        public List<InstitutionModel> GetInstitutionsDetail() => _institutionDA.GetInstitutionsDetail();

        /// <summary>
        /// Gets the majors by institution identifier.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        public List<MajorList> GetMajorsByInstitutionId(int institutionId) => _institutionDA.GetMajorsByInstitutionId(institutionId);

        /// <summary>
        /// Gets the power campus institutions.
        /// </summary>
        /// <returns></returns>
        public List<DropDownListModel> GetPowerCampusInstitutions() => _institutionDA.GetPowerCampusInstitutions();

        /// <summary>
        /// Gets the rvoe options.
        /// </summary>
        /// <returns></returns>
        public List<DropDownListModel> GetRvoeOptions() => _institutionDA.GetRvoeOptions();

        /// <summary>
        /// Maps the institution major rvoe.
        /// </summary>
        /// <param name="mapInstitutionMajorRvoe">The map institution major rvoe.</param>
        public void MapInstitutionMajorRvoe(MapInstitutionMajorRvoe mapInstitutionMajorRvoe) => _institutionDA.MapInstitutionMajorRvoe(mapInstitutionMajorRvoe);

        /// <summary>
        /// Maps the major.
        /// </summary>
        /// <param name="majorMappingModel">The major mapping model.</param>
        public void MapMajor(MajorMappingModel majorMappingModel) => _institutionDA.MapMajor(majorMappingModel);

        /// <summary>
        /// Initializes a new instance of the <see cref="InstitutionService" /> class.
        /// </summary>
        /// <param name="institutionDAModel">The institution da model.</param>
        public void Update(InstitutionDAModel institutionDAModel) => _institutionDA.UpdateInstitution(institutionDAModel);

        /// <summary>
        /// Validates the code.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns></returns>
        public bool ValidateCode(string code) => _institutionDA.ValidateCode(code);

        /// <summary>
        /// Validates the folio.
        /// </summary>
        /// <param name="folio">
        /// The folio.
        /// </param>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns></returns>
        public bool ValidateFolio(string folio, string code) => _institutionDA.ValidateFolio(folio, code);

        /// <summary>
        /// Validates the code.
        /// </summary>
        /// <param name="name">
        /// The code.
        /// </param>
        /// <returns></returns>
        public bool ValidateName(string name) => _institutionDA.ValidateName(name);
    }
}