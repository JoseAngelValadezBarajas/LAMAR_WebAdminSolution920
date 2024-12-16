// --------------------------------------------------------------------
// <copyright file="LegalBaseServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.DataAccess.ElectronicDegree;
using PowerCampus.Entities.ElectronicDegree;

namespace PowerCampus.Business.ElectronicDegree
{
    /// <summary>
    /// Implements the services required for Legal Base Entity
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.ElectronicDegree.ILegalBaseServices" />
    public class LegalBaseServices : ILegalBaseServices
    {
        /// <summary>
        /// The lb da
        /// </summary>
        private readonly LegalBaseDA lbDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="LegalBaseServices"/> class.
        /// </summary>
        public LegalBaseServices() => this.lbDA = new LegalBaseDA();

        /// <summary>
        /// Gets the legal base catalog.
        /// </summary>
        /// <returns></returns>
        public CodeLegalBase[] GetLegalBaseCatalog() => lbDA.GetLegalBaseCatalog();
    }
}