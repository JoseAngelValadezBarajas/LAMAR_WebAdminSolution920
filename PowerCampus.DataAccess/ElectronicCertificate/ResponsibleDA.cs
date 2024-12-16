// --------------------------------------------------------------------
// <copyright file="ResponsibleDA.cs" company="Ellucian">
//     Copyright 2020 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using PowerCampus.DataAccess.DataAccess;
using PowerCampus.Entities;
using PowerCampus.Entities.ElectronicCertificate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerCampus.DataAccess.ElectronicCertificate
{
    /// <summary>
    /// SignerDA
    /// </summary>
    public class ResponsibleDA
    {
        /// <summary>
        /// Creates the responsible.
        /// </summary>
        /// <param name="responsibleList">The responsible list.</param>
        public void CreateResponsible(ResponsibleList responsibleList)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    Responsible responsible = new Responsible
                    {
                        CreateDatetime = DateTime.Now,
                        Name = responsibleList.Name,
                        IsActive = responsibleList.IsActive,
                        FirstSurname = responsibleList.FirstSurname,
                        SecondSurname = responsibleList.SecondSurname,
                        Curp = responsibleList.Curp,
                        RevisionDatetime = DateTime.Now,
                        ResponsiblePositionId = responsibleList.ResponsiblePositionId,
                        Thumbprint = responsibleList.Thumbprint,
                        CreateOpId = responsibleList.UserName,
                        RevisionOpId = responsibleList.UserName
                    };
                    context.Responsibles.InsertOnSubmit(responsible);
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ResponsibleDA - CreateResponsible", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the edit responsible.
        /// </summary>
        /// <param name="responsibleId">The responsible identifier.</param>
        /// <returns></returns>
        public ResponsibleList GetEditResponsible(int responsibleId)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    return (from resp in context.Responsibles
                            where resp.ResponsibleId == responsibleId
                            orderby resp.FirstSurname, resp.SecondSurname, resp.Name
                            select new ResponsibleList()
                            {
                                ResponsibleId = resp.ResponsibleId,
                                Name = resp.Name,
                                FirstSurname = resp.FirstSurname,
                                SecondSurname = resp.SecondSurname,
                                Curp = resp.Curp,
                                ResponsiblePositionId = resp.ResponsiblePositionId,
                                IsActive = resp.IsActive,
                                Thumbprint = resp.Thumbprint
                            }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ResponsibleDA - GetEditResponsible", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the position catalog.
        /// </summary>
        /// <returns></returns>
        public ResponsibleModel GetPositionCatalog()
        {
            try
            {
                ResponsibleModel responsible = new ResponsibleModel();
                ElectronicCertificateContext context = new ElectronicCertificateContext();
                IQueryable<CODE_RESPONSIBLEPOSITION> query = from crp in context.CODE_RESPONSIBLEPOSITIONs
                                                             select crp;

                if (query != null)
                {
                    responsible.Position = new List<Position>();
                    responsible.Position = query.Select(m => new Position()
                    {
                        Description = m.LONG_DESC,
                        CodeValue = m.CODE_VALUE,
                        Status = m.STATUS
                    }).ToList();

                    return responsible;
                }
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ResponsibleDA - GetPositionCatalog", "The catalog CODE_RESPONSIBLEPOSITION does not exist");
                return null;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ResponsibleDA - GetPositionCatalog", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the responsibles.
        /// </summary>
        /// <returns></returns>
        public List<ResponsibleList> GetResponsibles()
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    return (from resp in context.Responsibles
                            join cr in context.CODE_RESPONSIBLEPOSITIONs
                            on resp.ResponsiblePositionId equals cr.ResponsiblePositionId
                            select new ResponsibleList()
                            {
                                Name = resp.Name,
                                FirstSurname = resp.FirstSurname,
                                SecondSurname = resp.SecondSurname,
                                ResponsibleId = resp.ResponsibleId,
                                IsActive = resp.IsActive,
                                ResponsiblePosition = cr.LONG_DESC,
                            }).OrderBy(y => y.FirstSurname)
                              .ThenBy(y => y.SecondSurname)
                              .ThenBy(y => y.Name)
                              .GroupBy(x => new { x.ResponsibleId })
                              .Select(g => g.First()).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ResponsibleDA - GetResponsibles", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the responsibles names.
        /// </summary>
        /// <returns></returns>
        public List<ResponsibleName> GetResponsiblesNames()
        {
            try
            {
                List<ResponsibleName> responsibleName = new List<ResponsibleName>();
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    List<ResponsibleList> responsibles = (from resp in context.Responsibles
                                                          select new ResponsibleList()
                                                          {
                                                              Name = resp.Name,
                                                              FirstSurname = resp.FirstSurname,
                                                              SecondSurname = resp.SecondSurname,
                                                              ResponsibleId = resp.ResponsibleId,
                                                          }).OrderBy(y => y.FirstSurname)
                                  .ThenBy(y => y.SecondSurname)
                                  .ThenBy(y => y.Name)
                                  .GroupBy(x => new { x.ResponsibleId })
                                  .Select(g => g.First()).ToList();

                    foreach (ResponsibleList responsible in responsibles)
                    {
                        responsibleName.Add(new ResponsibleName
                        {
                            CodeValue = responsible.ResponsibleId,
                            Description = responsible.Name + " " + responsible.FirstSurname + " " + responsible.SecondSurname
                        });
                    }
                }

                return responsibleName;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ResponsibleDA - GetResponsiblesNames", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Determines whether [is thumbprint assigned] [the specified responsible identifier].
        /// </summary>
        /// <param name="responsibleId">The responsible identifier.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <returns>
        ///   <c>true</c> if [is thumbprint assigned] [the specified responsible identifier]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsThumbprintAssigned(int responsibleId, string thumbprint)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    int count = (from row in context.Responsibles
                                 where row.Thumbprint == thumbprint && row.ResponsibleId != responsibleId
                                 select row).Count();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ResponsibleDA - IsThumbprintAssigned", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the responsible.
        /// </summary>
        /// <param name="responsibleList">The responsible list.</param>
        public void UpdateResponsible(ResponsibleList responsibleList)
        {
            try
            {
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    Responsible responsible = context.Responsibles.Single(r => r.ResponsibleId == responsibleList.ResponsibleId);
                    responsible.Name = responsibleList.Name;
                    responsible.IsActive = responsibleList.IsActive;
                    responsible.FirstSurname = responsibleList.FirstSurname;
                    responsible.SecondSurname = responsibleList.SecondSurname;
                    responsible.RevisionDatetime = DateTime.Now;
                    responsible.ResponsiblePositionId = responsibleList.ResponsiblePositionId;
                    responsible.Thumbprint = responsibleList.Thumbprint;
                    responsible.RevisionOpId = responsibleList.UserName;
                    context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ResponsibleDA - UpdateResponsible", ex.Message);
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
                using (ElectronicCertificateContext context = new ElectronicCertificateContext())
                {
                    Responsible query = (from resp in context.Responsibles
                                         where resp.Curp == Curp
                                         select resp).FirstOrDefault();

                    return query != null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogWebError(Constants.EcArea, $"{Constants.DataAccess} - ResponsibleDA - ValidateCurp", ex.Message);
                throw;
            }
        }
    }
}