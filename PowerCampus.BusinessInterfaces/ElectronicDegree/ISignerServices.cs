// --------------------------------------------------------------------
// <copyright file="ISignerServices.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;

namespace PowerCampus.BusinessInterfaces.ElectronicDegree
{
    /// <summary>
    /// ISigner Services
    /// </summary>
    public interface ISignerServices
    {
        /// <summary>
        /// Creates the signer.
        /// </summary>
        /// <param name="signerList">The signer list.</param>
        void CreateSigner(SignerList signerList);

        /// <summary>
        /// Exists the thumprint.
        /// </summary>
        /// <param name="Thumprint">The thumprint.</param>
        /// <returns></returns>
        bool ExistThumprint(string Thumprint);

        /// <summary>
        /// Gets the edit signer.
        /// </summary>
        /// <param name="signerId">The signer identifier.</param>
        /// <returns></returns>
        SignerList GetEditSigner(int signerId);

        /// <summary>
        /// Gets the labor catalog.
        /// </summary>
        /// <returns></returns>
        SignersModel GetLaborCatalog();

        /// <summary>
        /// Gets the signers.
        /// </summary>
        /// <returns></returns>
        List<SignerList> GetSigners();

        /// <summary>
        /// Gets the title abbreviation.
        /// </summary>
        /// <returns></returns>
        SignersModel GetTitleAbbreviation();

        /// <summary>
        /// Updates the signer.
        /// </summary>
        /// <param name="signerList">The signer list.</param>
        void UpdateSigner(SignerList signerList);

        /// <summary>
        /// Validates the curp.
        /// </summary>
        /// <param name="Curp">The curp.</param>
        /// <returns></returns>
        bool ValidateCurp(string Curp);

        /// <summary>
        /// Validates the thumprint.
        /// </summary>
        /// <param name="Thumprint">The thumprint.</param>
        /// <returns></returns>
        bool ValidateThumprint(string Thumprint);
    }
}