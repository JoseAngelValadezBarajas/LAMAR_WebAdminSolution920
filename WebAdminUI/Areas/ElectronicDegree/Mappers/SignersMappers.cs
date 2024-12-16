// --------------------------------------------------------------------
// <copyright file="SignersMappers.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using WebAdminUI.Areas.ElectronicDegree.Models.Signers;

namespace WebAdminUI.Areas.ElectronicDegree.Mappers
{
    /// <summary>
    /// Mapper for Signers view models
    /// </summary>
    internal static class SignersMappers
    {
        internal static AddSignersViewModel ToAddViewModel(this LaborPosition signersModel)
        {
            AddSignersViewModel signersCatalogViewModel = new AddSignersViewModel();
            signersCatalogViewModel.LaborPosition.LaborPosition.Add(new LaborPosition
            {
                CodeValue = signersModel.CodeValue,
                Description = signersModel.Description,
                Status = signersModel.Status
            });

            return signersCatalogViewModel;
        }

        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="signers">The signers.</param>
        /// <returns></returns>
        internal static List<SignersIndexList> ToViewModel(this List<SignerList> signers)
        {
            List<SignersIndexList> signersViewModel = new List<SignersIndexList>();
            foreach (SignerList signer in signers)
            {
                signersViewModel.Add(new SignersIndexList
                {
                    SignerId = signer.SignerId,
                    Signer = signer.AbreviationTitle + " " + signer.Name + " " + signer.FirstSurname + " " + signer.SecondSurname,
                    LaborPosition = signer.SignerPosition,
                    Institutions = signer.Institutions,
                    IsActive = signer.IsActive,
                });
            }
            return signersViewModel;
        }
    }
}