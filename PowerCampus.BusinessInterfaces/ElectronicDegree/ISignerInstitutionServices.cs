// --------------------------------------------------------------------
// <copyright file="ISignerInstitutionServices.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces.ElectronicDegree
{
    /// <summary>
    /// ISigner-Institution Services
    /// </summary>
    public interface ISignerInstitutionServices
    {
        /// <summary>
        /// Creates the signer institution.
        /// </summary>
        /// <param name="signerInstitution">The signer institution.</param>
        void CreateSignerInstitution(InstitutionList signerInstitution);

        /// <summary>
        /// Gets the institutions.
        /// </summary>
        /// <returns></returns>
        List<InstitutionList> GetInstitutions();

        /// <summary>
        /// Gets the institution signer list.
        /// </summary>
        /// <returns></returns>
        List<InstitutionSignerList> GetInstitutionSignerList();

        /// <summary>
        /// Gets the signers.
        /// </summary>
        /// <returns></returns>
        List<SignerList> GetSigners();
    }
}