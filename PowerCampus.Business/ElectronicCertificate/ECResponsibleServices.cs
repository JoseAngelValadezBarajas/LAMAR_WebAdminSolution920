// --------------------------------------------------------------------
// <copyright file="ECResponsibleServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.BusinessInterfaces.ElectronicCertificate;
using PowerCampus.DataAccess.ElectronicCertificate;
using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;

namespace PowerCampus.Business.ElectronicCertificate
{
    /// <summary>
    /// ECResponsibleServices
    /// </summary>
    /// <seealso cref="PowerCampus.BusinessInterfaces.ElectronicCertificate.IECResponsibleService" />
    public class ECResponsibleServices : IECResponsibleService
    {
        private readonly ResponsibleDA _responsibleDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="ECResponsibleServices"/> class.
        /// </summary>
        public ECResponsibleServices() => _responsibleDA = new ResponsibleDA();

        /// <summary>
        /// Creates the responsible.
        /// </summary>
        /// <param name="responsibleList">The responsible list.</param>
        public void CreateResponsible(ResponsibleList responsibleList) => _responsibleDA.CreateResponsible(responsibleList);

        /// <summary>
        /// Gets the edit responsible.
        /// </summary>
        /// <param name="responsibleId">The responsible identifier.</param>
        /// <returns></returns>
        public ResponsibleList GetEditResponsible(int responsibleId) => _responsibleDA.GetEditResponsible(responsibleId);

        /// <summary>
        /// Gets the labor catalog.
        /// </summary>
        /// <returns></returns>
        public ResponsibleModel GetPositionCatalog() => _responsibleDA.GetPositionCatalog();

        /// <summary>
        /// Gets the responsible names.
        /// </summary>
        /// <returns></returns>
        public List<ResponsibleName> GetResponsibleNames() => _responsibleDA.GetResponsiblesNames();

        /// <summary>
        /// Gets the signers.
        /// </summary>
        /// <returns></returns>
        public List<ResponsibleList> GetResponsibles() => _responsibleDA.GetResponsibles();

        /// <summary>
        /// Determines whether [is thumbprint assigned] [the specified responsible identifier].
        /// </summary>
        /// <param name="responsibleId">The responsible identifier.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <returns>
        /// <c>true</c> if [is thumbprint assigned] [the specified responsible identifier]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsThumbprintAssigned(int responsibleId, string thumbprint) => _responsibleDA.IsThumbprintAssigned(responsibleId, thumbprint);

        /// <summary>
        /// Updates the signer.
        /// </summary>
        /// <param name="responsibleList">The responsible list.</param>
        public void UpdateResponsible(ResponsibleList responsibleList) => _responsibleDA.UpdateResponsible(responsibleList);

        /// <summary>
        /// Validates the curp.
        /// </summary>
        /// <param name="Curp">The curp.</param>
        /// <returns></returns>
        public bool ValidateCurp(string Curp) => _responsibleDA.ValidateCurp(Curp);
    }
}