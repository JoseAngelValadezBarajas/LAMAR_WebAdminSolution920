// --------------------------------------------------------------------
// <copyright file="CatalogsMapper.cs" company="Ellucian">
//     Copyright 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using System.Collections.Generic;
using System.Linq;
using WebAdministration.Models.Shared;

namespace WebAdminUI.Mappers
{
    /// <summary>
    /// CatalogsMapper class.
    /// </summary>
    internal static class CatalogsMapper
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="fiscalRecordCancelReasons">The fiscal record cancel reasons.</param>
        /// <returns></returns>
        internal static List<ListOptionViewModel> ToViewModel(this List<FiscalRecordCancelReason> fiscalRecordCancelReasons)
        {
            return fiscalRecordCancelReasons.Select(c => new ListOptionViewModel
            {
                Description = $"{c.Code} {c.Description}",
                Value = c.Code
            }).ToList();
        }
    }
}