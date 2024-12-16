// --------------------------------------------------------------------
// <copyright file="GenerateServices.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.DataAccess.ElectronicDegree;
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;

namespace PowerCampus.Business.ElectronicDegree
{
    /// <summary>
    /// GenerateServices
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.ElectronicDegree.IGenerateServices" />
    public class GenerateServices : IGenerateServices
    {
        private readonly GenerateDA _generateDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateServices"/> class.
        /// </summary>
        public GenerateServices() => _generateDA = new GenerateDA();

        /// <summary>
        /// Inserts the electronic degree information.
        /// </summary>
        /// <param name="electronicDegreeInfo">The electronic degree information.</param>
        /// <returns></returns>
        public int Create(ElectronicDegreeInfo electronicDegreeInfo) => _generateDA.Create(electronicDegreeInfo);

        /// <summary>
        /// Gets the background studies.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <returns></returns>
        public List<BackgroundStudies> GetBackgroundStudies(string peopleId) => _generateDA.GetBackgroundStudies(peopleId);

        /// <summary>
        /// Gets the institution folio.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="ElectronicDegreeInstMajorId">The electronic degree inst major identifier.</param>
        /// <returns></returns>
        public string GetInstitutionFolio(string peopleId, int ElectronicDegreeInstMajorId) => _generateDA.GetInstitutionFolio(peopleId, ElectronicDegreeInstMajorId);

        /// <summary>
        /// Gets the institution major.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        public List<InstitutionMajor> GetInstitutionMajor(string peopleId, string operatorId) => _generateDA.GetInstitutionMajor(peopleId, operatorId);

        /// <summary>
        /// Gets the institution signers.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        public List<InstitutionSignerList> GetInstitutionSigners(int institutionId) => _generateDA.GetInstitutionSigner(institutionId);

        /// <summary>
        /// Gets the issuing degree.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="instMajorId">The inst major identifier.</param>
        /// <param name="transcritDegree">The transcrit degree.</param>
        public IssuingDegree GetIssuingDegree(string peopleId, int instMajorId, int transcritDegree) => _generateDA.GetIssuingDegree(peopleId, instMajorId, transcritDegree);

        /// <summary>
        /// Gets the people.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <returns></returns>
        public List<PeopleModel> GetPeople(string peopleId) => _generateDA.GetPeople(peopleId);

        /// <summary>
        /// Updates the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="originalString"></param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public bool UpdateXml(string xml, string originalString, int id) => _generateDA.UpdateXml(xml, originalString, id);
    }
}