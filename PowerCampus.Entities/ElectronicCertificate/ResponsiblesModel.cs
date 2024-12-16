// --------------------------------------------------------------------
// <copyright file="ResponsiblesModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace PowerCampus.Entities.ElectronicCertificate
{
    public class Position
    {
        /// <summary>
        /// Gets or sets the code value.
        /// </summary>
        /// <value>
        /// The code value.
        /// </value>
        public int CodeValue { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is asigned.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is asigned; otherwise, <c>false</c>.
        /// </value>
        public bool IsAssigned { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }
    }

    public class ResponsibleList
    {
        /// <summary>
        /// Gets or sets the create datetime.
        /// </summary>
        /// <value>
        /// The create datetime.
        /// </value>
        public DateTime? CreateDatetime { get; set; }

        /// <summary>
        /// Gets or sets the create op identifier.
        /// </summary>
        /// <value>
        /// The create op identifier.
        /// </value>
        public string CreateOpId { get; set; }

        /// <summary>
        /// Gets or sets the curp.
        /// </summary>
        /// <value>
        /// The curp.
        /// </value>
        public string Curp { get; set; }

        /// <summary>
        /// Gets or sets the first surname.
        /// </summary>
        /// <value>
        /// The first surname.
        /// </value>
        public string FirstSurname { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the signer identifier.
        /// </summary>
        /// <value>
        /// The signer identifier.
        /// </value>
        public int ResponsibleId { get; set; }

        /// <summary>
        /// Gets or sets the signer position.
        /// </summary>
        /// <value>
        /// The signer position.
        /// </value>
        public string ResponsiblePosition { get; set; }

        /// <summary>
        /// Gets or sets the signer position identifier.
        /// </summary>
        /// <value>
        /// The signer position identifier.
        /// </value>
        public int ResponsiblePositionId { get; set; }

        /// <summary>
        /// Gets or sets the revision datetime.
        /// </summary>
        /// <value>
        /// The revision datetime.
        /// </value>
        public DateTime? RevisionDatetime { get; set; }

        /// <summary>
        /// Gets or sets the revision op identifier.
        /// </summary>
        /// <value>
        /// The revision op identifier.
        /// </value>
        public string RevisionOpId { get; set; }

        /// <summary>
        /// Gets or sets the second surname.
        /// </summary>
        /// <value>
        /// The second surname.
        /// </value>
        public string SecondSurname { get; set; }

        /// <summary>
        /// Gets or sets the thumbprint.
        /// </summary>
        /// <value>
        /// The thumbprint.
        /// </value>
        public string Thumbprint { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }
    }

    public class ResponsibleModel
    {
        /// <summary>
        /// Gets or sets the labor position.
        /// </summary>
        /// <value>
        /// The labor position.
        /// </value>
        public List<Position> Position { get; set; }

        /// <summary>
        /// Gets or sets the signers list.
        /// </summary>
        /// <value>
        /// The signers list.
        /// </value>
        public List<ResponsibleList> ResponsibleList { get; set; }

        /// <summary>
        /// Gets or sets the responsible names.
        /// </summary>
        /// <value>
        /// The responsible names.
        /// </value>
        public List<ResponsibleName> ResponsibleNames { get; set; }
    }

    public class ResponsibleName
    {
        /// <summary>
        /// Gets or sets the code value.
        /// </summary>
        /// <value>
        /// The code value.
        /// </value>
        public int CodeValue { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
    }
}