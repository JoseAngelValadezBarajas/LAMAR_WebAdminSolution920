// --------------------------------------------------------------------
// <copyright file="RepositoryActionResult.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System;

namespace PowerCampus.BussinessInterfaces
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryActionResult<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryActionResult{T}"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="status">The status.</param>
        public RepositoryActionResult(T entity, RepositoryActionStatus status)
        {
            Entity = entity;
            Status = status;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryActionResult{T}"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="status">The status.</param>
        /// <param name="exception">The exception.</param>
        public RepositoryActionResult(T entity, RepositoryActionStatus status, Exception exception) : this(entity, status)
        {
            Exception = exception;
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        public T Entity { get; private set; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public RepositoryActionStatus Status { get; private set; }
    }
}