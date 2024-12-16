// --------------------------------------------------------------------
// <copyright file="SignerInstitutionsMappers.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using WebAdminUI.Areas.ElectronicDegree.Models.SignerInstitution;

namespace WebAdminUI.Areas.ElectronicDegree.Mappers
{
    /// <summary>
    /// Signer-Institutions Mappers
    /// </summary>
    internal static class SignerInstitutionsMappers
    {
        internal static List<SignerInstitutionViewModel> ToViewModel(this List<InstitutionList> Institutions, List<SignerList> Signers, List<InstitutionSignerList> InstitutionSignerList)
        {
            List<SignerInstitutionViewModel> signerInstitutionViewModels = new List<SignerInstitutionViewModel>();
            List<LaborPosition> signerList = new List<LaborPosition>();
            foreach (SignerList signer in Signers)
            {
                signerList.Add(new LaborPosition
                {
                    CodeValue = signer.SignerId,
                    Description = signer.AbreviationTitle + " " + signer.Name + " " + signer.FirstSurname + " " + signer.SecondSurname
                });
            }

            foreach (InstitutionList institution in Institutions)
            {
                List<InstitutionSignerViewModel> institutionSignerViewModels = new List<InstitutionSignerViewModel>();
                foreach (InstitutionSignerList institutionSigner in InstitutionSignerList.FindAll(m => m.EdInstitutionId == institution.InstitutionId))
                {
                    institutionSignerViewModels.Add(new InstitutionSignerViewModel
                    {
                        AbreviationTitle = institutionSigner.EdAbreviationTitle,
                        InstitutionSignerId = institutionSigner.EdInstitutionSignerId,
                        InstitutionId = institutionSigner.EdInstitutionId,
                        SignerId = institutionSigner.EdSignerId,
                        SignerName = institutionSigner.EdAbreviationTitle + " " + institutionSigner.EdSignerName + " " + institutionSigner.EdSignerFirstSurname + " " + institutionSigner.EdSignerSecondSurname
                    });
                }

                List<LaborPosition> signerInstitutionList = new List<LaborPosition>();
                foreach (SignerList signer in Signers)
                {
                    signerInstitutionList.Add(new LaborPosition
                    {
                        CodeValue = signer.SignerId,
                        Description = signer.AbreviationTitle + " " + signer.Name + " " + signer.FirstSurname + " " + signer.SecondSurname
                    });
                }

                bool IsAssigned = false;
                int index = 0;
                foreach (InstitutionSignerList signer in InstitutionSignerList.FindAll(m => m.EdInstitutionId == institution.InstitutionId))
                {
                    index = signerInstitutionList.FindIndex(m => m.CodeValue == signer.EdSignerId);
                    IsAssigned = institution.InstitutionId == signer.EdInstitutionId;
                    signerInstitutionList[index].IsAssigned = IsAssigned;
                }

                signerInstitutionViewModels.Add(new SignerInstitutionViewModel
                {
                    InstitutionName = institution.InstitutionName,
                    InstitutionId = institution.InstitutionId,
                    InstitutionCode = institution.InstitutionCode,
                    SignersList = signerList,
                    SignerInstitutionList = signerInstitutionList,
                    InstitutionSignerViewModel = institutionSignerViewModels
                });
            }

            return signerInstitutionViewModels;
        }
    }
}