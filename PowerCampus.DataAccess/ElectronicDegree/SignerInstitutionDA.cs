// --------------------------------------------------------------------
// <copyright file="SingerInstitutionDA.cs" company="Ellucian">
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
    /// Singer-InstitutionDA
    /// </summary>
    public class SignerInstitutionDA
    {
        /// <summary>
        /// Creates the signer institution.
        /// </summary>
        /// <param name="signerInstitution">The signer institution.</param>
        public void CreateSignerInstitution(InstitutionList signerInstitution)
        {
            try
            {
                ElectronicDegreeContext context = new ElectronicDegreeContext();

                IQueryable<ElectronicDegreeInstSigner> query = from insSig in context.ElectronicDegreeInstSigners
                                                               select insSig;

                ElectronicDegreeInstSigner electronicDegreeInstSigner = query.FirstOrDefault();
                if (electronicDegreeInstSigner != null)
                    context.ElectronicDegreeInstSigners.Where(x => x.ElectronicDegreeInstitutionId == signerInstitution.InstitutionId).ToList().ForEach(context.ElectronicDegreeInstSigners.DeleteOnSubmit);

                context.SubmitChanges();

                foreach (int signerId in signerInstitution.SignerId)
                {
                    ElectronicDegreeInstSigner elecDegreeInstSigner = new ElectronicDegreeInstSigner
                    {
                        ElectronicDegreeInstitutionId = signerInstitution.InstitutionId,
                        SignerId = signerId,
                        CreateUserName = signerInstitution.UserName,
                        CreateDatetime = DateTime.Now,
                        RevisionDatetime = DateTime.Now,
                        RevisionUserName = signerInstitution.UserName
                    };
                    context.ElectronicDegreeInstSigners.InsertOnSubmit(elecDegreeInstSigner);
                }
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - SignerInstitutionDA - CreateSignerInstitution", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the signer institution.
        /// </summary>
        /// <returns></returns>
        public List<InstitutionList> GetInstitutions()
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    return (from ins in context.ElectronicDegreeInstitutions
                            orderby ins.Name
                            select new InstitutionList()
                            {
                                InstitutionId = ins.ElectronicDegreeInstitutionId,
                                InstitutionCode = ins.Code,
                                InstitutionName = ins.Name
                            }).ToList();
                }
            }
            catch (Exception)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - SingerInstitutionDA - GetInstitutions", "The table are has no records");
                throw;
            }
        }

        /// <summary>
        /// Gets the institution signer.
        /// </summary>
        /// <returns></returns>
        public List<InstitutionSignerList> GetInstitutionSigner()
        {
            try
            {
                using (ElectronicDegreeContext context = new ElectronicDegreeContext())
                {
                    return (
                        from ins in context.ElectronicDegreeInstSigners
                        join ei in context.ElectronicDegreeInstitutions
                        on ins.ElectronicDegreeInstitutionId
                        equals ei.ElectronicDegreeInstitutionId
                        join sng in context.Signers
                        on ins.SignerId
                        equals sng.SignerId
                        join abv in context.CODE_ABREVIATIONTITLEs
                        on sng.AbreviationTitleId
                        equals abv.AbreviationTitleId
                        orderby sng.FirstSurname, sng.SecondSurname, sng.Name
                        select new InstitutionSignerList()
                        {
                            EdAbreviationTitle = abv.LONG_DESC,
                            EdInstitutionId = ins.ElectronicDegreeInstitutionId,
                            EdInstitutionSignerId = ins.ElectronicDegreeInstSignerId,
                            EdSignerId = ins.SignerId,
                            EdSignerName = sng.Name,
                            EdSignerFirstSurname = sng.FirstSurname,
                            EdSignerSecondSurname = sng.SecondSurname
                        }).ToList();
                }
            }
            catch (Exception)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - SingerInstitutionDA - GetInstitutionSigner", "The table are has no records");
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
                            join abv in context.CODE_ABREVIATIONTITLEs
                            on sng.AbreviationTitleId
                            equals abv.AbreviationTitleId
                            orderby sng.FirstSurname, sng.SecondSurname, sng.Name
                            select new SignerList()
                            {
                                AbreviationTitle = abv.LONG_DESC,
                                SignerId = sng.SignerId,
                                Name = sng.Name,
                                FirstSurname = sng.FirstSurname,
                                SecondSurname = sng.SecondSurname
                            }).ToList();
                }
            }
            catch (Exception)
            {
                LoggerHelper.LogWebError("ElectronicDegree", "PowerCampus.DataAccess - SingerInstitutionDA - GetSigners", "The table are has no records");
                throw;
            }
        }
    }
}