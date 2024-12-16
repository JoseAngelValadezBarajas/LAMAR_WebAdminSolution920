// --------------------------------------------------------------------
// <copyright file="SharedMapper.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities.ElectronicCertificate;
using System.Collections.Generic;
using WebAdminUI.Areas.ElectronicCertificate.Models.Shared;

namespace WebAdminUI.Areas.ElectronicCertificate.Mappers
{
    /// <summary>
    /// SharedMapper
    /// </summary>
    internal static class SharedMapper
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="codeTablesDto">The code tables dto.</param>
        /// <returns></returns>
        internal static List<DropdownListViewModel> ToViewModel(this List<CodeTable> codeTablesDto)
        {
            List<DropdownListViewModel> codetables = new List<DropdownListViewModel>();
            if (codeTablesDto != null)
            {
                foreach (CodeTable codeTable in codeTablesDto)
                {
                    codetables.Add(new DropdownListViewModel
                    {
                        CodeValueKey = codeTable.CodeValueKey,
                        Description = codeTable.Description,
                        ShortDescription = codeTable.ShortDescription,
                        Id = codeTable.Id
                    });
                }
            }

            return codetables;
        }

        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="statusDto">The status dto.</param>
        /// <returns></returns>
        internal static List<DropdownListViewModel> ToViewModel(this List<string> statusDto)
        {
            List<DropdownListViewModel> statuses = new List<DropdownListViewModel>();
            if (statusDto != null)
            {
                foreach (string status in statusDto)
                {
                    statuses.Add(new DropdownListViewModel
                    {
                        CodeValueKey = status,
                        Description = status == "G" ? Resources.ElectronicCertificate.Parameters.lblGenerated :
                         status == "S" ? Resources.ElectronicCertificate.Parameters.lblStamped :
                         status == "E" ? Resources.ElectronicCertificate.Parameters.lblError :
                         Resources.ElectronicCertificate.Parameters.lblCanceled
                    });
                }
            }
            return statuses;
        }
    }
}