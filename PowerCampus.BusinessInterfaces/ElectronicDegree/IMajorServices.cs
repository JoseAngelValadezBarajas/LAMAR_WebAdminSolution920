// --------------------------------------------------------------------
// <copyright file="IMajorServices.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces.ElectronicDegree
{
    /// <summary>
    /// IMajor Services
    /// </summary>
    public interface IMajorServices
    {
        /// <summary>
        /// Creates the major.
        /// </summary>
        /// <param name="majorList">The major list.</param>
        void CreateMajor(MajorList majorList);

        /// <summary>
        /// Deletes the major.
        /// </summary>
        /// <param name="majorId">The major identifier.</param>
        void DeleteMajor(int majorId);

        /// <summary>
        /// Gets the edit major.
        /// </summary>
        /// <param name="MajorId">The major identifier.</param>
        /// <returns></returns>
        MajorList GetMajor(int MajorId);

        /// <summary>
        /// Gets the majors.
        /// </summary>
        /// <returns></returns>
        List<MajorList> GetMajors();

        /// <summary>
        /// Updates the major.
        /// </summary>
        /// <param name="majorList">The major list.</param>
        void UpdateMajor(MajorList majorList);

        /// <summary>
        /// Validates the code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        bool ValidateCode(string code);
    }
}