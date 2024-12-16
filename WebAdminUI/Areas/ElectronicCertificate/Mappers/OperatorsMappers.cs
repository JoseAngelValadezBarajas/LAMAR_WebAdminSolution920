// --------------------------------------------------------------------
// <copyright file="OperatorsMappers.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;
using WebAdminUI.Areas.ElectronicCertificate.Models.Operators;

namespace WebAdminUI.Areas.ElectronicCertificate.Mappers
{
    /// <summary>
    /// Operators Mappers
    /// </summary>
    internal static class OperatorsMappers
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="Operators">The operators.</param>
        /// <returns></returns>
        internal static List<OperatorsViewModel> ToViewModel(this List<OperatorsList> Operators)
        {
            List<OperatorsViewModel> operatorsViewModel = new List<OperatorsViewModel>();
            foreach (OperatorsList operators in Operators)
            {
                operatorsViewModel.Add(new OperatorsViewModel
                {
                    OperatorID = operators.OperatorId,
                    NumberCampuses = operators.Institutions,
                    NumberPermissions = operators.Permissions,
                    NumberID = operators.PeopleCodeId,
                    Name = operators.Name
                });
            }

            return operatorsViewModel;
        }
    }
}