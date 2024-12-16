// --------------------------------------------------------------------
// <copyright file="MajorsMappers.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using WebAdminUI.Areas.ElectronicDegree.Models.Majors;

namespace WebAdminUI.Areas.ElectronicDegree.Mappers
{
    /// <summary>
    /// Majors Mappers
    /// </summary>
    internal static class MajorsMappers
    {
        internal static List<MajorsViewModel> ToViewModel(this List<MajorList> majors, List<DropDownListModel> dropDownOptions = null)
        {
            List<MajorsViewModel> majorsViewModels = new List<MajorsViewModel>();
            foreach (MajorList major in majors)
            {
                majorsViewModels.Add(new MajorsViewModel
                {
                    MajorId = major.ElectronicDegreeMajorId,
                    Cve = major.Code,
                    MajorName = major.Name,
                    EducationLevel = major.StudyLevel,
                    Institutions = major.Institutions.ToString(),
                    Selected = major.Selected,
                    Rvoe = dropDownOptions,
                    SelectedRvoes = major.RvoeIds,
                    ElectronicDegreeInstMajorId = major.ElectronicDegreeInstMajorId
                });
            }
            return majorsViewModels;
        }
    }
}