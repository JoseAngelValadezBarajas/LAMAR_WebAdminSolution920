// --------------------------------------------------------------------
// <copyright file="ResponsiblesMapper.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;
using WebAdminUI.Areas.ElectronicCertificate.Models.Responsibles;

namespace WebAdminUI.Areas.ElectronicCertificate.Mapper
{
    /// <summary>
    /// Mapper for Responsibles view models
    /// </summary>
    internal static class ResponsiblesMapper
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="responsibles"></param>
        /// <returns></returns>
        internal static List<ResponsiblesIndexList> ToViewModel(this List<ResponsibleList> responsibles)
        {
            List<ResponsiblesIndexList> responsibleViewModel = new List<ResponsiblesIndexList>();
            foreach (ResponsibleList responsible in responsibles)
            {
                responsibleViewModel.Add(new ResponsiblesIndexList
                {
                    ResponsibleId = responsible.ResponsibleId,
                    Responsibles = responsible.Name + " " + responsible.FirstSurname + " " + responsible.SecondSurname,
                    Position = responsible.ResponsiblePosition,
                    IsActive = responsible.IsActive,
                });
            }
            return responsibleViewModel;
        }
    }
}