// --------------------------------------------------------------------
// <copyright file="SignerInstitutionModel.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Collections.Generic;

namespace PowerCampus.Entities.ElectronicDegree
{
    /// <summary>
    /// Institution List
    /// </summary>
    public class InstitutionList
    {
        /// <summary>
        /// Gets or sets the institution code.
        /// </summary>
        /// <value>
        /// The institution code.
        /// </value>
        public string InstitutionCode { get; set; }

        /// <summary>
        /// Gets or sets the institution identifier.
        /// </summary>
        /// <value>
        /// The institution identifier.
        /// </value>
        public int InstitutionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the institution.
        /// </summary>
        /// <value>
        /// The name of the institution.
        /// </value>
        public string InstitutionName { get; set; }

        /// <summary>
        /// Gets or sets the institution signer identifier.
        /// </summary>
        /// <value>
        /// The institution signer identifier.
        /// </value>
        public int InstitutionSignerId { get; set; }

        /// <summary>
        /// Gets or sets the signer identifier.
        /// </summary>
        /// <value>
        /// The signer identifier.
        /// </value>
        public List<int> SignerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }
    }

    /// <summary>
    /// Institution-Signer List
    /// </summary>
    public class InstitutionSignerList
    {
        /// <summary>
        /// Gets or sets the ed abreviation identifier.
        /// </summary>
        /// <value>
        /// The ed abreviation identifier.
        /// </value>
        public string EdAbreviationTitle { get; set; }

        /// <summary>
        /// Gets or sets the ed institution identifier.
        /// </summary>
        /// <value>
        /// The ed institution identifier.
        /// </value>
        public int? EdInstitutionId { get; set; }

        /// <summary>
        /// Gets or sets the ed institution signer identifier.
        /// </summary>
        /// <value>
        /// The ed institution signer identifier.
        /// </value>
        public int EdInstitutionSignerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the ed.
        /// </summary>
        /// <value>
        /// The name of the ed.
        /// </value>
        public string EdName { get; set; }

        /// <summary>
        /// Gets or sets the ed signer abreviation code.
        /// </summary>
        /// <value>
        /// The ed signer abreviation code.
        /// </value>
        public int EdSignerAbreviationCode { get; set; }

        /// <summary>
        /// Gets or sets the ed signer curp.
        /// </summary>
        /// <value>
        /// The ed signer curp.
        /// </value>
        public string EdSignerCurp { get; set; }

        /// <summary>
        /// Gets or sets the ed signer first surname.
        /// </summary>
        /// <value>
        /// The ed signer first surname.
        /// </value>
        public string EdSignerFirstSurname { get; set; }

        /// <summary>
        /// Gets or sets the ed signer identifier.
        /// </summary>
        /// <value>
        /// The ed signer identifier.
        /// </value>
        public int EdSignerId { get; set; }

        /// <summary>
        /// Gets or sets the ed signer labor position.
        /// </summary>
        /// <value>
        /// The ed signer labor position.
        /// </value>
        public string EdSignerLaborPosition { get; set; }

        /// <summary>
        /// Gets or sets the ed signer labor position code.
        /// </summary>
        /// <value>
        /// The ed signer labor position code.
        /// </value>
        public int EdSignerLaborPositionCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the ed signer.
        /// </summary>
        /// <value>
        /// The name of the ed signer.
        /// </value>
        public string EdSignerName { get; set; }

        /// <summary>
        /// Gets or sets the ed signer second surname.
        /// </summary>
        /// <value>
        /// The ed signer second surname.
        /// </value>
        public string EdSignerSecondSurname { get; set; }

        /// <summary>
        /// Gets or sets the ed signer thumprint.
        /// </summary>
        /// <value>
        /// The ed signer thumprint.
        /// </value>
        public string EdSignerThumprint { get; set; }

        /// <summary>
        /// Gets or sets the institution signer list.
        /// </summary>
        /// <value>
        /// The institution signer list.
        /// </value>
        public List<InstitutionSignerList> InstitutionSigners { get; set; }
    }
}