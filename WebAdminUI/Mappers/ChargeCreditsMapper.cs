// --------------------------------------------------------------------
// <copyright file="ChargeCreditsMapper.cs" company="Ellucian">
//     Copyright 2018 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebAdministration.Models.Shared;
using WebAdminUI.Models.ChargeCreditMappings;
using WebAdminUI.Models.FiscalRecords;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.ChargeCreditsMappers
{
    /// <summary>
    /// Mapper for charge credits view models
    /// </summary>
    internal static class ChargeCreditsMapper
    {
        /// <summary>
        /// To the PPD create view model.
        /// </summary>
        /// <param name="chargeCredit">The charge credit.</param>
        /// <param name="people">The people.</param>
        /// <param name="issuerDefault">The issuer default.</param>
        /// <param name="invoiceOrganization">The invoice organization.</param>
        /// <param name="account">The user account.</param>
        /// <returns></returns>
        internal static PPDCreateViewModel ToViewModel(this ChargeCredit chargeCredit, People people, IssuerDefault issuerDefault, InvoiceOrganization invoiceOrganization, Account account)
        {
            var ppdCreateViewModel = new PPDCreateViewModel
            {
                ChargeCredit = new ChargeCreditViewModel(),
                PaymentMethod = chargeCredit.PaymentMethod,
                PaymentType = chargeCredit.PaymentType,
                PeopleOrgCodeId = people.PeopleCodeId,
                ChargeCreditNumber = chargeCredit.ChargeNumberSource
            };

            #region ChargeCredit

            ppdCreateViewModel.ChargeCredit.ChargeCreditCode = chargeCredit.ChargeCreditCode;
            ppdCreateViewModel.ChargeCredit.ChargeCreditCodeId = chargeCredit.ChargeCreditCodeId;
            ppdCreateViewModel.ChargeCredit.ChargeCreditDesc = chargeCredit.ChargeCreditDesc;
            ppdCreateViewModel.ChargeCredit.ProductServiceDesc = chargeCredit.ProductServiceDesc;
            ppdCreateViewModel.ChargeCredit.ProductServiceKey = chargeCredit.ProductServiceKey;
            ppdCreateViewModel.ChargeCredit.SubjectToTax = chargeCredit.SubjectToTax;
            ppdCreateViewModel.ChargeCredit.TotalTaxes = ((decimal)chargeCredit.TotalTaxes).ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            ppdCreateViewModel.ChargeCredit.TotalUnit = chargeCredit.TotalUnit.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            ppdCreateViewModel.ChargeCredit.UnitAmount = chargeCredit.UnitAmount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            ppdCreateViewModel.ChargeCredit.UnityKey = chargeCredit.UnitKey;
            ppdCreateViewModel.ChargeCredit.UnityName = chargeCredit.UnityName;
            ppdCreateViewModel.Subtotal = chargeCredit.UnitAmount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            ppdCreateViewModel.Total = chargeCredit.TotalUnit.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);

            #endregion ChargeCredit

            #region IssuerDefault

            ppdCreateViewModel.IssuerTaxpayerId = issuerDefault.IssInvoiceTaxpayerId;
            ppdCreateViewModel.IssuerCorporateName = issuerDefault.IssCorporateName;
            ppdCreateViewModel.IssuerAddress = issuerDefault.IssIssuingAddress;
            ppdCreateViewModel.IssuerSerial = issuerDefault.IssSerialNumber;
            ppdCreateViewModel.IssuerInvoiceOrganizationId = issuerDefault.IssInvoiceOrganizationId;
            ppdCreateViewModel.IssuerInvoiceExpeditionId = issuerDefault.IssInvoiceOrganizationId;
            ppdCreateViewModel.IssuerTaxRegimenId = invoiceOrganization.TaxRegimenId;

            #endregion IssuerDefault

            #region ReceiverDefault

            if (people.FiscalRecordsDefault != null)
            {
                ppdCreateViewModel.ReceiverTaxpayerId = people.FiscalRecordsDefault.TaxpayerId;
                ppdCreateViewModel.ReceiverEmail = people.FiscalRecordsDefault.Email;
                ppdCreateViewModel.ReceiverCorporateName = people.FiscalRecordsDefault.CorporateName;
                ppdCreateViewModel.ReceiverTaxAddress = people.FiscalRecordsDefault.PostalCode;
                if (!string.IsNullOrEmpty(people.FiscalRecordsDefault.TaxRegimen))
                {
                    ppdCreateViewModel.ReceiverTaxRegimen = String.Format("{0} - {1}", people.FiscalRecordsDefault.TaxRegimen, people.FiscalRecordsDefault.TaxRegimenDesc);
                }

                if (people.FiscalRecordsDefault.TaxpayerId.Equals("XEXX010101000"))
                {
                    ppdCreateViewModel.ReceiverCFDIUsageCode = "P01";
                    ppdCreateViewModel.ReceiverCFDIUsageDesc = "P01-Por definir";
                }
                else
                {
                    ppdCreateViewModel.ReceiverCFDIUsageCode = people.FiscalRecordsDefault.CFDIUsageCode;
                    ppdCreateViewModel.ReceiverCFDIUsageDesc = people.FiscalRecordsDefault.CFDIUsageDesc;
                    ppdCreateViewModel.ReceiverCFDIList = people.FiscalRecordsDefault.CFDICatalog;
                }
            }
            else
            {
                people.FiscalRecordsDefault = new FiscalRecordDefaults();
                ppdCreateViewModel.ReceiverCFDIUsageCode = string.Empty;
                ppdCreateViewModel.ReceiverCFDIUsageDesc = string.Empty;
                ppdCreateViewModel.ReceiverCFDIList = new List<CFDIUsageCatalog>();
            }

            #endregion ReceiverDefault

            return ppdCreateViewModel;
        }

        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="chargeCreditWithTaxesList">The charge credit with taxes list.</param>
        /// <returns></returns>
        internal static List<ChargeCreditWithTaxesViewModel> ToViewModel(this List<ChargeCreditWithTaxes> chargeCreditWithTaxesList)
        {
            List<ChargeCreditWithTaxesViewModel> chargeCreditWithTaxesViewModels = new List<ChargeCreditWithTaxesViewModel>();
            if (chargeCreditWithTaxesList != null)
            {
                foreach (ChargeCreditWithTaxes chargeCreditWithTaxes in chargeCreditWithTaxesList)
                {
                    List<ChargeCreditTaxes> chargeCreditWithTaxesFiltered = chargeCreditWithTaxes.ChargeCreditTaxes != null
                        ? chargeCreditWithTaxes.ChargeCreditTaxes
                               .Where(tax => !string.IsNullOrEmpty(tax.Description)
                                   || !string.IsNullOrEmpty(tax.TaxCode)
                                   || !string.IsNullOrEmpty(tax.ChargeCreditTaxCode)).ToList()
                                   : new List<ChargeCreditTaxes>();
                    chargeCreditWithTaxesViewModels.Add(new ChargeCreditWithTaxesViewModel()
                    {
                        Code = $"{chargeCreditWithTaxes.Code} - {chargeCreditWithTaxes.Description}",
                        Id = chargeCreditWithTaxes.Id,
                        Taxes = chargeCreditWithTaxesFiltered.Select(tax =>
                        {
                            if (string.IsNullOrEmpty(tax.FactorType))
                                return $" - {tax.ChargeCreditTaxCode} - ";
                            if (tax.TaxRate.HasValue)
                                return $"{tax.TaxCode} - {tax.ChargeCreditTaxCode} - {Math.Round(tax.TaxRate.Value, 2)}";
                            return $"{tax.TaxCode} - {tax.ChargeCreditTaxCode}";
                        }).ToList(),
                        TaxProfiles = chargeCreditWithTaxesFiltered.Select(tax =>
                        {
                            if (string.IsNullOrEmpty(tax.FactorType))
                                return ChargeCreditMappingModelResource.MappingMissing;
                            if (tax.TaxRate.HasValue)
                                return $"{tax.Description} - {Math.Round(tax.TaxRate.Value * 100, 0)}%";
                            return $"{tax.Description}";
                        }).ToList()
                    });
                }
            }
            return chargeCreditWithTaxesViewModels;
        }

        /// <summary>
        /// Receivers to receiver view.
        /// </summary>
        /// <param name="chargeCreditMappings">The charge credit mapping list.</param>
        /// <returns></returns>
        internal static List<ChargeCreditMappingViewModel> ToViewModel(this List<ChargeCreditMapping> chargeCreditMappings)
        {
            List<ChargeCreditMappingViewModel> chargeCreditMappingViewModels = new List<ChargeCreditMappingViewModel>();
            if (chargeCreditMappings != null)
            {
                foreach (ChargeCreditMapping chargeCreditMapping in chargeCreditMappings)
                {
                    int index = chargeCreditMappingViewModels.FindIndex(ccm => ccm.ChargeCreditCodeId == chargeCreditMapping.ChargeCreditCodeId
                        && ccm.ProductServiceKey == chargeCreditMapping.ProductServiceKey && ccm.UnityKey == chargeCreditMapping.UnityKey);

                    string taxesDesc = string.IsNullOrEmpty(chargeCreditMapping.TaxRate) ? chargeCreditMapping.TaxDescription + " - " + chargeCreditMapping.FactorType : chargeCreditMapping.TaxDescription + " - " + float.Parse(chargeCreditMapping.TaxRate) + "%";
                    if (index < 0)
                    {
                        ChargeCreditMappingViewModel chargeCreditMappingViewModel = new ChargeCreditMappingViewModel
                        {
                            Id = chargeCreditMapping.Id,
                            ChargeCreditCodeId = chargeCreditMapping.ChargeCreditCodeId,
                            ChargeCreditCode = chargeCreditMapping.ChargeCreditDesc,
                            ProductServiceKey = chargeCreditMapping.ProductServiceKey,
                            ProductServiceDesc = chargeCreditMapping.ProductServiceDesc,
                            UnityKey = chargeCreditMapping.UnityKey,
                            UnityDesc = chargeCreditMapping.UnityDesc,
                            TaxesDescList = new List<string>() { taxesDesc },
                        };
                        chargeCreditMappingViewModels.Add(chargeCreditMappingViewModel);
                    }
                    else
                    {
                        ChargeCreditMappingViewModel tmpChargeCreditMapping = chargeCreditMappingViewModels[index];
                        tmpChargeCreditMapping.TaxesDescList.Add(taxesDesc);
                    }
                }
            }
            return chargeCreditMappingViewModels;
        }

        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="chargeCreditSpecialTaxes">The charge credit special taxes.</param>
        /// <returns></returns>
        internal static List<ListOptionViewModel> ToViewModel(this List<ChargeCreditTaxes> chargeCreditSpecialTaxes)
        {
            List<ListOptionViewModel> options = new List<ListOptionViewModel>();
            if (chargeCreditSpecialTaxes != null)
            {
                for (int i = 0; i < chargeCreditSpecialTaxes.Count; i++)
                {
                    string taxFactorTypeDesc = chargeCreditSpecialTaxes[i].FactorType == FactorType.Exento.ToString()
                                           ? chargeCreditSpecialTaxes[i].FactorType
                                           : $"{Math.Round(chargeCreditSpecialTaxes[i].TaxRate.Value * 100, 0)}%";
                    string taxFactorTypeValue = chargeCreditSpecialTaxes[i].FactorType == FactorType.Exento.ToString()
                                           ? chargeCreditSpecialTaxes[i].FactorType
                                           : $"{chargeCreditSpecialTaxes[i].FactorType} - {Math.Round(chargeCreditSpecialTaxes[i].TaxRate.Value, 2)}";
                    options.Add(new ListOptionViewModel()
                    {
                        Description = $"{chargeCreditSpecialTaxes[i].Description} - {taxFactorTypeDesc}",
                        Value = $"{chargeCreditSpecialTaxes[i].TaxCode} - {chargeCreditSpecialTaxes[i].Description} - {taxFactorTypeValue}"
                    });
                }
            }
            return options;
        }

        /// <summary>
        /// To the receiver entity.
        /// </summary>
        /// <param name="chargeCreditMappingViewModel">The charge credit mapping view model.</param>
        /// <returns></returns>
        internal static ChargeCreditMapping ToViewModel(this ChargeCreditMappingViewModel chargeCreditMappingViewModel)
        {
            var chargeCreditMapping = new ChargeCreditMapping
            {
                ChargeCreditCodeId = chargeCreditMappingViewModel.ChargeCreditCodeId,
                ProductServiceKey = chargeCreditMappingViewModel.ProductServiceKey,
                UnityKey = chargeCreditMappingViewModel.UnityKey
            };
            return chargeCreditMapping;
        }
    }
}