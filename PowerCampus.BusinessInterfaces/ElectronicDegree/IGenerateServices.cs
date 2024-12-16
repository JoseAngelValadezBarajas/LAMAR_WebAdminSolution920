// --------------------------------------------------------------------
// <copyright file="IGenerateServices.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces.ElectronicDegree
{
    /// <summary>
    /// IGenerate Services
    /// </summary>
    public interface IGenerateServices
    {
        /// <summary>
        /// Inserts the electronic degree information.
        /// </summary>
        /// <param name="electronicDegreeInfo">The electronic degree information.</param>
        /// <returns></returns>
        int Create(ElectronicDegreeInfo electronicDegreeInfo);

        /// <summary>
        /// Gets the background studies.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <returns></returns>
        List<BackgroundStudies> GetBackgroundStudies(string peopleId);

        /// <summary>
        /// Gets the institution folio.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="ElectronicDegreeInstMajorId">The electronic degree inst major identifier.</param>
        /// <returns></returns>
        string GetInstitutionFolio(string peopleId, int ElectronicDegreeInstMajorId);

        /// <summary>
        /// Gets the institution major.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns></returns>
        List<InstitutionMajor> GetInstitutionMajor(string peopleId, string operatorId);

        /// <summary>
        /// Gets the institution signers.
        /// </summary>
        /// <param name="institutionId">The institution identifier.</param>
        /// <returns></returns>
        List<InstitutionSignerList> GetInstitutionSigners(int institutionId);

        /// <summary>
        /// Gets the issuing degree.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <param name="instMajorId">The inst major identifier.</param>
        /// <param name="transcritDegree">The transcrit degree.</param>
        IssuingDegree GetIssuingDegree(string peopleId, int instMajorId, int transcritDegree);

        /// <summary>
        /// Gets the people.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <returns></returns>
        List<PeopleModel> GetPeople(string peopleId);

        /// <summary>
        /// Updates the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="originalString">The original string.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        bool UpdateXml(string xml, string originalString, int id);
    }
}