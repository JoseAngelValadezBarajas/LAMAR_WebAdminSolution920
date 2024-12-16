// --------------------------------------------------------------------
// <copyright file="SignerServices.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.BusinessInterfaces.ElectronicDegree;
using PowerCampus.DataAccess.ElectronicDegree;
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace PowerCampus.Business.ElectronicDegree
{
    /// <summary>
    /// Signer Services
    /// </summary>
    public class SignerServices : ISignerServices
    {
        private readonly SignerDA _signerDA;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignerServices"/> class.
        /// </summary>
        public SignerServices() => _signerDA = new SignerDA();

        /// <summary>
        /// Creates the signer.
        /// </summary>
        /// <param name="signerList">The signer list.</param>
        public void CreateSigner(SignerList signerList) => _signerDA.CreateSigner(signerList);

        /// <summary>
        /// Exists the thumprint.
        /// </summary>
        /// <param name="Thumprint">The thumprint.</param>
        /// <returns></returns>
        public bool ExistThumprint(string Thumprint)
        {
            bool exist = true;
            X509Store myStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            myStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificates = myStore.Certificates.Find(X509FindType.FindByThumbprint, Thumprint, false);

            if (certificates.Count == 0)
                exist = false;
            myStore.Close();
            return exist;
        }

        /// <summary>
        /// Gets the edit signer.
        /// </summary>
        /// <param name="signerId">The signer identifier.</param>
        /// <returns></returns>
        public SignerList GetEditSigner(int signerId) => _signerDA.GetEditSigner(signerId);

        /// <summary>
        /// Gets the labor catalog.
        /// </summary>
        /// <returns></returns>
        public SignersModel GetLaborCatalog() => _signerDA.GetLaborCatalog();

        /// <summary>
        /// Gets the signers.
        /// </summary>
        /// <returns></returns>
        public List<SignerList> GetSigners() => _signerDA.GetSigners();

        /// <summary>
        /// Gets the title abbreviation.
        /// </summary>
        /// <returns></returns>
        public SignersModel GetTitleAbbreviation() => _signerDA.GetTitleAbbreviation();

        /// <summary>
        /// Updates the signer.
        /// </summary>
        /// <param name="signerList">The signer list.</param>
        public void UpdateSigner(SignerList signerList) => _signerDA.UpdateSigner(signerList);

        /// <summary>
        /// Validates the curp.
        /// </summary>
        /// <param name="Curp">The curp.</param>
        /// <returns></returns>
        public bool ValidateCurp(string Curp) => _signerDA.ValidateCurp(Curp);

        /// <summary>
        /// Validates the thumprint.
        /// </summary>
        /// <param name="Thumprint">The thumprint.</param>
        /// <returns></returns>
        public bool ValidateThumprint(string Thumprint) => _signerDA.ValidateThumprint(Thumprint);
    }
}