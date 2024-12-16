// --------------------------------------------------------------------
// <copyright file="RepositoryActionStatus.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
namespace PowerCampus.BussinessInterfaces
{
    /// <summary>
    /// Enumerator for Action Status
    /// </summary>
    public enum RepositoryActionStatus
    {
        /// <summary>
        /// The ok
        /// </summary>
        Ok,

        /// <summary>
        /// The created
        /// </summary>
        Created,

        /// <summary>
        /// The updated
        /// </summary>
        Updated,

        /// <summary>
        /// The not found
        /// </summary>
        NotFound,

        /// <summary>
        /// The deleted
        /// </summary>
        Deleted,

        /// <summary>
        /// The nothing modified
        /// </summary>
        NothingModified,

        /// <summary>
        /// The error
        /// </summary>
        Error
    }
}