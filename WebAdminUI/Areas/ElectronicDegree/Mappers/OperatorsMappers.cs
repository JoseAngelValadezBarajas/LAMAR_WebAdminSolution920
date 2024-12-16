// --------------------------------------------------------------------
// <copyright file="OperatorsMappers.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicDegree;
using System.Collections.Generic;
using WebAdminUI.Areas.ElectronicDegree.Models.Operators;

namespace WebAdminUI.Areas.ElectronicDegree.Mappers
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
                    Institution = operators.Institutions,
                    Permissions = operators.Permissions,
                    NumberID = operators.PeopleCodeId,
                    Name = operators.Name
                });
            }

            return operatorsViewModel;
        }

        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="SignerList">The signer list.</param>
        /// <returns></returns>
        internal static AddOperatorsViewModel ToViewModel(this List<InstitutionSignerList> SignerList)
        {
            AddOperatorsViewModel institutionSignersViewModel = new AddOperatorsViewModel();
            institutionSignersViewModel.InstitutionSignerLists = new List<InstitutionSignerList>();
            foreach (InstitutionSignerList institutionSignerList in SignerList)
            {
                institutionSignersViewModel.InstitutionSignerLists.Add(new InstitutionSignerList
                {
                    EdAbreviationTitle = institutionSignerList.EdAbreviationTitle,
                    EdSignerName = institutionSignerList.EdSignerName
                });
            }

            return institutionSignersViewModel;
        }
    }
}