// --------------------------------------------------------------------
// <copyright file="SignerModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace PowerCampus.Entities.ElectronicDegree
{
    public class LaborPosition
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

    public class SignerList
    {
        /// <summary>
        /// Gets or sets the abreviation title.
        /// </summary>
        /// <value>
        /// The abreviation title.
        /// </value>
        public string AbreviationTitle { get; set; }

        /// <summary>
        /// Gets or sets the abreviation title identifier.
        /// </summary>
        /// <value>
        /// The abreviation title identifier.
        /// </value>
        public int AbreviationTitleId { get; set; }

        /// <summary>
        /// Gets or sets the create datetime.
        /// </summary>
        /// <value>
        /// The create datetime.
        /// </value>
        public DateTime? CreateDatetime { get; set; }

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
        /// Gets or sets the institutions.
        /// </summary>
        /// <value>
        /// The institutions.
        /// </value>
        public int? Institutions { get; set; }

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
        /// Gets or sets the revision datetime.
        /// </summary>
        /// <value>
        /// The revision datetime.
        /// </value>
        public DateTime? RevisionDatetime { get; set; }

        /// <summary>
        /// Gets or sets the second surname.
        /// </summary>
        /// <value>
        /// The second surname.
        /// </value>
        public string SecondSurname { get; set; }

        /// <summary>
        /// Gets or sets the signer identifier.
        /// </summary>
        /// <value>
        /// The signer identifier.
        /// </value>
        public int SignerId { get; set; }

        /// <summary>
        /// Gets or sets the signer position.
        /// </summary>
        /// <value>
        /// The signer position.
        /// </value>
        public string SignerPosition { get; set; }

        /// <summary>
        /// Gets or sets the signer position identifier.
        /// </summary>
        /// <value>
        /// The signer position identifier.
        /// </value>
        public int SignerPositionId { get; set; }

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

    public class SignersModel
    {
        /// <summary>
        /// Gets or sets the labor position.
        /// </summary>
        /// <value>
        /// The labor position.
        /// </value>
        public List<LaborPosition> LaborPosition { get; set; }

        /// <summary>
        /// Gets or sets the signers list.
        /// </summary>
        /// <value>
        /// The signers list.
        /// </value>
        public List<SignerList> SignersList { get; set; }

        /// <summary>
        /// Gets or sets the title abbreviation.
        /// </summary>
        /// <value>
        /// The title abbreviation.
        /// </value>
        public List<LaborPosition> TitleAbbreviation { get; set; }
    }
}