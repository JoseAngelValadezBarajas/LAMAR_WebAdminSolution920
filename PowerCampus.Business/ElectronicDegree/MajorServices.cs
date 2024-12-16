// --------------------------------------------------------------------
// <copyright file="MajorServices.cs" company="Ellucian">
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
    /// Major Services
    /// </summary>
    public class MajorServices : IMajorServices
    {
        private readonly MajorsDA _majorsDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="MajorServices"/> class.
        /// </summary>
        public MajorServices() => _majorsDA = new MajorsDA();

        /// <summary>
        /// Creates the major.
        /// </summary>
        /// <param name="majorList">The major list.</param>
        public void CreateMajor(MajorList majorList) => _majorsDA.CreateMajor(majorList);

        /// <summary>
        /// Deletes the major.
        /// </summary>
        /// <param name="majorId">The major identifier.</param>
        public void DeleteMajor(int majorId) => _majorsDA.DeleteMajor(majorId);

        /// <summary>
        /// Gets the edit major.
        /// </summary>
        /// <param name="majorId">The major identifier.</param>
        /// <returns></returns>
        public MajorList GetMajor(int majorId) => _majorsDA.GetMajor(majorId);

        /// <summary>
        /// Gets the majors.
        /// </summary>
        /// <returns></returns>
        public List<MajorList> GetMajors() => _majorsDA.GetMajors();

        /// <summary>
        /// Updates the major.
        /// </summary>
        /// <param name="majorList">The major list.</param>
        public void UpdateMajor(MajorList majorList) => _majorsDA.UpdateMajor(majorList);

        /// <summary>
        /// Validates the code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public bool ValidateCode(string code) => _majorsDA.ValidateCode(code);
    }
}