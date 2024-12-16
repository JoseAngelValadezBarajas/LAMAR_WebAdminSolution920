// --------------------------------------------------------------------
// <copyright file="SignerInstitutionServices.cs" company="Ellucian">
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
    /// Signer-Institution Services
    /// </summary>
    public class SignerInstitutionServices : ISignerInstitutionServices
    {
        private readonly SignerInstitutionDA _signerInstitutionDa;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignerInstitutionServices"/> class.
        /// </summary>
        public SignerInstitutionServices() => _signerInstitutionDa = new SignerInstitutionDA();

        /// <summary>
        /// Creates the signer institution.
        /// </summary>
        /// <param name="signerInstitution">The signer institution.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void CreateSignerInstitution(InstitutionList signerInstitution) => _signerInstitutionDa.CreateSignerInstitution(signerInstitution);

        /// <summary>
        /// Signers the institution lists.
        /// </summary>
        /// <returns></returns>
        public List<InstitutionList> GetInstitutions() => _signerInstitutionDa.GetInstitutions();

        /// <summary>
        /// Gets the institution signer list.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<InstitutionSignerList> GetInstitutionSignerList() => _signerInstitutionDa.GetInstitutionSigner();

        /// <summary>
        /// Signers the lists.
        /// </summary>
        /// <returns></returns>
        public List<SignerList> GetSigners() => _signerInstitutionDa.GetSigners();
    }
}