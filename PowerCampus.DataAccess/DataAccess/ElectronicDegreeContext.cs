// --------------------------------------------------------------------
// <copyright file="ElectronicDegreeContext.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Configuration;

namespace PowerCampus.DataAccess.DataAccess
{
    /// <summary>
    /// Electronic Degree Context
    /// </summary>
    /// <seealso cref="PowerCampus.DataAccess.DataAccess.ElectronicDegreeDataContext" />
    public sealed class ElectronicDegreeContext : ElectronicDegreeDataContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElectronicDegreeContext"/> class.
        /// </summary>
        public ElectronicDegreeContext() : base(ConfigurationManager.ConnectionStrings["PowerCampusDbContext"].ConnectionString)
        {
        }
    }
}