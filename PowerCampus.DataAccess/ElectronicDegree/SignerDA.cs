// --------------------------------------------------------------------
// <copyright file="SignerDA.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.Entities.ElectronicDegree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerCampus.DataAccess.ElectronicDegree
{
    /// <summary>
    /// SignerDA
    /// </summary>
    public class SignerDA
    {
        /// <summary>
        /// Creates the signer.
        /// </summary>
        /// <param name="signerList">The signer list.</param>
        public void CreateSigner(SignerList signerList)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    Signer signer = new Signer
                    {
                        AbreviationTitleId = signerList.AbreviationTitleId,
                        CreateDatetime = DateTime.Now,
                        Name = signerList.Name,
                        IsActive = signerList.IsActive,
                        FirstSurname = signerList.FirstSurname,
                        SecondSurname = signerList.SecondSurname,
                        Curp = signerList.Curp,
                        RevisionDatetime = DateTime.Now,
                        SignerPositionId = signerList.SignerPositionId,
                        Thumbprint = signerList.Thumbprint,
                        CreateUserName = signerList.UserName,
                        RevisionUserName = signerList.UserName
                    };
                    context.Signers.InsertOnSubmit(signer);
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - SignerDA - CreateSigner", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the edit signer.
        /// </summary>
        /// <param name="signerId">The signer identifier.</param>
        /// <returns></returns>
        public SignerList GetEditSigner(int signerId)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    return (from sng in context.Signers
                            where sng.SignerId == signerId
                            orderby sng.FirstSurname, sng.SecondSurname, sng.Name
                            select new SignerList()
                            {
                                SignerId = sng.SignerId,
                                Name = sng.Name,
                                FirstSurname = sng.FirstSurname,
                                SecondSurname = sng.SecondSurname,
                                Curp = sng.Curp,
                                AbreviationTitleId = sng.AbreviationTitleId,
                                SignerPositionId = sng.SignerPositionId,
                                IsActive = sng.IsActive,
                                Thumbprint = sng.Thumbprint
                            }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - SignerDA - GetEditSigner", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the labor catalog.
        /// </summary>
        /// <returns></returns>
        public SignersModel GetLaborCatalog()
        {
            try
            {
                SignersModel signer = new SignersModel();
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                IQueryable<CODE_SIGNERPOSITION> query = from csp in context.CODE_SIGNERPOSITIONs
                                                        select csp;

                if (query != null)
                {
                    signer.LaborPosition = new List<LaborPosition>();
                    signer.LaborPosition = query.Select(m => new LaborPosition()
                    {
                        Description = m.LONG_DESC,
                        CodeValue = m.SignerPositionId,
                        Status = m.STATUS
                    }).ToList();

                    return signer;
                }
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - SignerDA - GetLaborCatalog", "The catalog doesn´t exists");

                return null;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - SignerDA - GetLaborCatalog", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the signers.
        /// </summary>
        /// <returns></returns>
        public List<SignerList> GetSigners()
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    return (from sng in context.Signers
                            join ca in context.CODE_ABREVIATIONTITLEs
                            on sng.AbreviationTitleId equals ca.AbreviationTitleId
                            join cs in context.CODE_SIGNERPOSITIONs
                            on sng.SignerPositionId equals cs.SignerPositionId
                            join edis in context.ElectronicDegreeInstSigners
                            on sng.SignerId equals edis.SignerId into tempEdis
                            from edis in tempEdis.DefaultIfEmpty()
                            select new SignerList()
                            {
                                Name = sng.Name,
                                FirstSurname = sng.FirstSurname,
                                SecondSurname = sng.SecondSurname,
                                SignerId = sng.SignerId,
                                IsActive = sng.IsActive,
                                AbreviationTitle = ca.MEDIUM_DESC,
                                SignerPosition = cs.LONG_DESC,
                                Institutions = tempEdis.Count()
                            }).OrderBy(y => y.FirstSurname).ThenBy(y => y.SecondSurname).ThenBy(y => y.Name).GroupBy(x => new { x.SignerId }).Select(g => g.First()).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - SignerDA - GetTitleAbbreviation", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the title abbreviation.
        /// </summary>
        /// <returns></returns>
        public SignersModel GetTitleAbbreviation()
        {
            try
            {
                SignersModel signer = new SignersModel();
                ElectronicDegreeContext context = new ElectronicDegreeContext();
                IQueryable<CODE_ABREVIATIONTITLE> query = from cat in context.CODE_ABREVIATIONTITLEs
                                                          select cat;

                if (query != null)
                {
                    signer.TitleAbbreviation = new List<LaborPosition>();
                    signer.TitleAbbreviation = query.Select(m => new LaborPosition()
                    {
                        Description = m.LONG_DESC,
                        CodeValue = m.CODE_VALUE,
                        Status = m.STATUS
                    }).ToList();

                    return signer;
                }
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - SignerDA - GetTitleAbbreviation", "The catalog doesn´t exists");

                return null;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - SignerDA - GetTitleAbbreviation", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the signer.
        /// </summary>
        /// <param name="signerList">The signer list.</param>
        public void UpdateSigner(SignerList signerList)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    Signer signer = context.Signers.Single(s => s.SignerId == signerList.SignerId);
                    signer.AbreviationTitleId = signerList.AbreviationTitleId;
                    signer.Name = signerList.Name;
                    signer.IsActive = signerList.IsActive;
                    signer.FirstSurname = signerList.FirstSurname;
                    signer.SecondSurname = signerList.SecondSurname;
                    signer.RevisionDatetime = DateTime.Now;
                    signer.SignerPositionId = signerList.SignerPositionId;
                    signer.Thumbprint = signerList.Thumbprint;
                    signer.RevisionUserName = signerList.UserName;
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - SignerDA - UpdateSigner", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the curp.
        /// </summary>
        /// <param name="Curp">The curp.</param>
        /// <returns></returns>
        public bool ValidateCurp(string Curp)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    Signer query = (from sng in context.Signers
                                    where sng.Curp == Curp
                                    select sng).FirstOrDefault();

                    return query != null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - SignerDA - ValidateCurp", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the thumprint.
        /// </summary>
        /// <param name="Thumprint">The thumprint.</param>
        /// <returns></returns>
        public bool ValidateThumprint(string Thumprint)
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    Signer query = (from sng in context.Signers
                                    where sng.Thumbprint == Thumprint
                                    select sng).FirstOrDefault();

                    return query != null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - SignerDA - ValidateThumprint", ex.Message);
                throw;
            }
        }
    }
}