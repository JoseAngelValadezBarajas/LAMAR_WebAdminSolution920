// --------------------------------------------------------------------
// <copyright file="ICreditNotes.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;

namespace PowerCampus.BusinessInterfaces
{
    /// <summary>
    /// ICreditNotesServices class
    /// </summary>
    public interface ICreditNotesServices
    {
        /// <summary>
        /// Creates the credit note.
        /// </summary>
        /// <param name="fiscalRecordModel">The fiscal record model.</param>
        /// <returns></returns>
        int CreateCreditNote(CreateFiscalRecord fiscalRecordModel);

        /// <summary>
        /// Gets the fiscal record.
        /// </summary>
        /// <param name="InvoiceHeaderId">The invoice header identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="PeopleOrgCodeId">The people org code identifier.</param>
        /// <returns></returns>
        FiscalRecord GetFiscalRecord(int InvoiceHeaderId, string userName, string PeopleOrgCodeId);

        /// <summary>
        /// Gets for substitution.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="peopleOrgcodeId">The people orgcode identifier.</param>
        /// <returns></returns>
        FiscalRecord GetForSubstitution(int id, string userName, string peopleOrgcodeId);
    }
}