// --------------------------------------------------------------------
// <copyright file="ElectronicCertificateContext.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Configuration;

namespace PowerCampus.DataAccess.DataAccess
{
    /// <summary>
    /// Electronic Degree Context
    /// </summary>
    /// <seealso cref="PowerCampus.DataAccess.DataAccess.ElectronicCertificateDataContext" />
    public sealed class ElectronicCertificateContext : ElectronicCertificateDataContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElectronicDegreeContext"/> class.
        /// </summary>
        public ElectronicCertificateContext() : base(ConfigurationManager.ConnectionStrings["PowerCampusDbContext"].ConnectionString)
        {
        }
    }
}