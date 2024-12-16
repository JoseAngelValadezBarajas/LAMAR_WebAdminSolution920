// --------------------------------------------------------------------
// <copyright file="PeopleMapper.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using WebAdminUI.Areas.General.Models;

namespace WebAdminUI.Areas.General.Mappers
{
    /// <summary>
    /// PeopleMapper
    /// </summary>
    internal static class PeopleMapper
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="people">The people.</param>
        /// <returns></returns>
        internal static List<PeopleViewModel> ToViewModel(this List<PeopleModel> people)
        {
            List<PeopleViewModel> peopleViewModel = new List<PeopleViewModel>();
            foreach (PeopleModel person in people)
            {
                peopleViewModel.Add(new PeopleViewModel
                {
                    BirthDate = person.BirthDate?.ToString("yyyy/MM/dd"),
                    Curp = person.Curp,
                    FirstSurname = person.FirstSurname,
                    GenderIdentity = person.GenderIdentity,
                    SecondSurname = person.SecondSurname,
                    Email = person.Email,
                    PeopleCodeId = person.PeopleCodeId,
                    Name = person.Name
                });
            }
            return peopleViewModel;
        }
    }
}