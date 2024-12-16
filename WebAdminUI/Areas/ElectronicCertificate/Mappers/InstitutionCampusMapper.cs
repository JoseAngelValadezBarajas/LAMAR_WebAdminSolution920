// --------------------------------------------------------------------
// <copyright file="InstitutionCampusMapper.cs" company="Ellucian">
//     Copyright 2020-2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;
using WebAdminUI.Areas.ElectronicCertificate.Models.InstitutionCampus;

namespace WebAdminUI.Areas.ElectronicCertificate.Mappers
{
    internal static class InstitutionCampusMapper
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="institutionCampuses">The institution campuses.</param>
        /// <returns></returns>
        internal static List<InstitutionCampusViewModel> ToViewModel(this List<InstitutionCampus> institutionCampuses)
        {
            List<InstitutionCampusViewModel> institutionCampusViewModels = new List<InstitutionCampusViewModel>();
            foreach (InstitutionCampus institutionCampus in institutionCampuses)
            {
                institutionCampusViewModels.Add(new InstitutionCampusViewModel
                {
                    CampusCodeId = institutionCampus.CampusCodeId,
                    CampusName = institutionCampus.CampusName,
                    CampusSepCode = institutionCampus.CampusSepCode,
                    FederalEntityId = institutionCampus.FederalEntityId,
                    InstitutionCampusId = institutionCampus.InstitutionCampusId,
                    InstitutionCodeId = institutionCampus.InstitutionCodeId,
                    FolioFormat = institutionCampus.FolioFormat,
                    InstitutionName = institutionCampus.InstitutionName,
                    InstitutionSepId = institutionCampus.InstitutionSepId,
                    ResponsibleId = institutionCampus.ResponsibleId,
                    SigningInstitutionId = institutionCampus.SigningInstitutionId
                });
            }

            return institutionCampusViewModels;
        }

        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="responsibleNames">The responsible names.</param>
        /// <returns></returns>
        internal static List<ResponsibleNameViewModel> ToViewModel(this List<ResponsibleName> responsibleNames)
        {
            List<ResponsibleNameViewModel> responsibleNameViewModels = new List<ResponsibleNameViewModel>();
            foreach (ResponsibleName responsibleName in responsibleNames)
            {
                responsibleNameViewModels.Add(new ResponsibleNameViewModel
                {
                    Id = responsibleName.CodeValue,
                    Description = responsibleName.Description
                });
            }

            return responsibleNameViewModels;
        }
    }
}