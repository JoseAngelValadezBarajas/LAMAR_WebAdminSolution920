// --------------------------------------------------------------------
// <copyright file="CashReceiptsMapper.cs" company="Ellucian">
//     Copyright 2018 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebAdminUI.Helpers;
using WebAdminUI.Models.CashReceipts;
using WebAdminUI.Models.FiscalRecords;
using WebAdminUI.Models.ReceiptPaymentMapping;
using WebAdminUI.Models.Resources;

namespace WebAdminUI.CashReceiptsMappers
{
    /// <summary>
    /// Mapper for cash receipts view models
    /// </summary>
    internal static class CashReceiptsMapper
    {
        /// <summary>
        /// Converts to PPD viewmodel.
        /// </summary>
        /// <param name="cashReceipt">The cash receipt.</param>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        internal static PPDCreateViewModel ToPPDViewModel(this CashReceipt cashReceipt, Account account)
        {
            var ppdCreateViewModel = new PPDCreateViewModel
            {
                CancelReasonName = cashReceipt.CancelReasonName,
                CFDIRelated = cashReceipt.UUID,
                CFDIRelatedId = cashReceipt.CFDIRelatedId,
                ChargeCredit = new ChargeCreditViewModel(),
                IssuerInvoiceExpeditionId = cashReceipt.InvoiceExpeditionId,
                IssuerSerial = cashReceipt.SerialNumber,
                IssuerTaxpayerId = cashReceipt.IssuerTaxpayerId,
                PaymentCondition = cashReceipt.PaymentCondition,
                PaymentMethod = cashReceipt.ReceiverPaymentMethodDefault,
                PaymentType = cashReceipt.PaymentType.Description,
                PeopleOrgCodeId = cashReceipt.peopleOrgId,
                ReceiverCFDIList = cashReceipt.CFDIList,
                ReceiverCFDIUsageCode = cashReceipt.ReceiverList[0].PreferredCFDIUsageCode,
                ReceiverEmail = cashReceipt.preferredReceiverEmail,
                ReceiverTaxpayerId = cashReceipt.ReceiverList[0].TaxPayerId,
                RelationTypeDesc = string.IsNullOrEmpty(cashReceipt.RelationTypeCode) ? string.Empty : cashReceipt.RelationTypeCode + " - " + cashReceipt.RelationTypeDesc
            };

            if (cashReceipt.chargesToInvoiceList != null)
            {
                ppdCreateViewModel.ChargeCredit.ChargeCreditCode = cashReceipt.chargesToInvoiceList[0].ChargeCreditCode;
                ppdCreateViewModel.ChargeCredit.ChargeCreditCodeId = cashReceipt.chargesToInvoiceList[0].ChargeCreditCodeId;
                ppdCreateViewModel.ChargeCredit.ChargeCreditDesc = cashReceipt.chargesToInvoiceList[0].ChargeCreditDesc;
                ppdCreateViewModel.ChargeCredit.ProductServiceDesc = cashReceipt.chargesToInvoiceList[0].ProductServiceDesc;
                ppdCreateViewModel.ChargeCredit.ProductServiceKey = cashReceipt.chargesToInvoiceList[0].ProductServiceKey;
                ppdCreateViewModel.ChargeCredit.SubjectToTax = cashReceipt.chargesToInvoiceList[0].SubjectToTax;
                ppdCreateViewModel.ChargeCredit.TotalTaxes = ((decimal)cashReceipt.chargesToInvoiceList[0].TotalTaxes).ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
                ppdCreateViewModel.ChargeCredit.TotalUnit = (cashReceipt.chargesToInvoiceList[0].Quantity * cashReceipt.chargesToInvoiceList[0].UnitAmount).ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
                ppdCreateViewModel.ChargeCredit.UnitAmount = cashReceipt.chargesToInvoiceList[0].UnitAmount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
                ppdCreateViewModel.ChargeCredit.UnityKey = cashReceipt.chargesToInvoiceList[0].UnitKey;
                ppdCreateViewModel.ChargeCredit.UnityName = cashReceipt.chargesToInvoiceList[0].UnityName;
                ppdCreateViewModel.Subtotal = cashReceipt.chargesToInvoiceList[0].UnitAmount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
                ppdCreateViewModel.Total = cashReceipt.chargesToInvoiceList[0].TotalUnit.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            }

            ppdCreateViewModel.Subtotal = cashReceipt.SubTotal.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            ppdCreateViewModel.TotalTaxes = cashReceipt.TotalTT.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            ppdCreateViewModel.Total = cashReceipt.Total.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);

            return ppdCreateViewModel;
        }

        /// <summary>
        /// Map the cash receipt application from Business to View Model.
        /// </summary>
        /// <param name="cashReceiptDetail">The cash receipt application.</param>
        /// <returns>The cash receipt application ina  view model.</returns>
        internal static ChargeCreditApplicationViewModel ToViewModel(this ChargeCreditApplication cashReceiptDetail)
        {
            ChargeCreditApplicationViewModel cashReceiptDetailViewModel = null;
            ChargeCreditApplicationDetailsViewModel tempChargeCredit = null;
            decimal totalAmount = 0;
            decimal amountApplied = 0;
            try
            {
                if (cashReceiptDetail != null)
                {
                    cashReceiptDetailViewModel = new ChargeCreditApplicationViewModel
                    {
                        ReceiptNumber = cashReceiptDetail.ReceiptNumber,
                        EntryDate = cashReceiptDetail.EntryDate,
                        TotalAmount = cashReceiptDetail.TotalAmount,
                        BalanceAmount = cashReceiptDetail.BalanceAmount,
                        CanCreateInvoice = cashReceiptDetail.CanCreateInvoice,
                        IsVoid = cashReceiptDetail.IsVoid,
                        IsReversed = cashReceiptDetail.IsReversed,
                        PeopleOrgId = PowerCampusSystemFormat.FormatPeopleCodeId(cashReceiptDetail.PeopleOrgId.Substring(1, cashReceiptDetail.PeopleOrgId.Length - 1)),
                        PeopleOrgFullName = cashReceiptDetail.PeopleOrgFullName,
                        Charges = null,
                        ChargesWithPPD = null
                    };

                    if (cashReceiptDetail.Charges?.Count > 0)
                    {
                        #region Charges Without PPD

                        Func<ChargeCreditApplicationDetails, bool> predicateWithoutPPD = c => (c.PaymentMethod == null || !c.PaymentMethod.Equals(Constants.PPDPaymentMethod));
                        IQueryable<ChargeCreditApplicationDetails> charges = cashReceiptDetail.Charges.Where(predicateWithoutPPD).AsQueryable();
                        cashReceiptDetailViewModel.Charges = new List<ChargeCreditApplicationDetailsViewModel>();

                        foreach (ChargeCreditApplicationDetails chargeCredit in charges)
                        {
                            totalAmount = chargeCredit.TotalAmount;
                            amountApplied = chargeCredit.AmountApplied;

                            tempChargeCredit = new ChargeCreditApplicationDetailsViewModel
                            {
                                ApplicationDate = chargeCredit.ApplicationDate,
                                AmountApplied = amountApplied,
                                TotalAmount = totalAmount,
                                ChargeCreditDesc = $"{chargeCredit.ChargeCreditCode} - {chargeCredit.ChargeCreditDesc}",
                                InvoiceHeaderId = chargeCredit.InvoiceHeaderId,
                                InvoiceHeaderIdRelated = chargeCredit.InvoiceHeaderIdRelated
                            };

                            cashReceiptDetailViewModel.Charges.Add(tempChargeCredit);
                        }

                        if (cashReceiptDetailViewModel.Charges.Count > 0)
                            cashReceiptDetailViewModel.Charges = cashReceiptDetailViewModel.Charges.OrderBy(c => c.InvoiceHeaderId).ThenBy(c => c.ApplicationDate).ToList();
                        else
                            cashReceiptDetailViewModel.Charges = null;

                        #endregion Charges Without PPD

                        #region Charges with PPD

                        Func<ChargeCreditApplicationDetails, bool> predicateWithPPD = c => c.PaymentMethod?.Equals(Constants.PPDPaymentMethod) == true;
                        IQueryable<ChargeCreditApplicationDetails> chargesWithPPD = cashReceiptDetail.Charges.Where(predicateWithPPD).AsQueryable();
                        cashReceiptDetailViewModel.ChargesWithPPD = new List<ChargeCreditApplicationDetailsViewModel>();
                        foreach (ChargeCreditApplicationDetails chargeCredit in chargesWithPPD)
                        {
                            totalAmount = chargeCredit.TotalAmount;
                            amountApplied = chargeCredit.AmountApplied;

                            tempChargeCredit = new ChargeCreditApplicationDetailsViewModel
                            {
                                ApplicationDate = chargeCredit.ApplicationDate,
                                AmountApplied = amountApplied,
                                TotalAmount = totalAmount,
                                ChargeCreditDesc = $"{chargeCredit.ChargeCreditCode} - {chargeCredit.ChargeCreditDesc}",
                                InvoiceHeaderId = chargeCredit.InvoiceHeaderId,
                                InvoiceHeaderIdRelated = chargeCredit.InvoiceHeaderIdRelated,
                                CanCreateSupplement = chargeCredit.CanCreateSupplement
                            };

                            cashReceiptDetailViewModel.ChargesWithPPD.Add(tempChargeCredit);
                        }

                        if (cashReceiptDetailViewModel.ChargesWithPPD.Count > 0)
                            cashReceiptDetailViewModel.ChargesWithPPD = cashReceiptDetailViewModel.ChargesWithPPD.OrderBy(c => c.InvoiceHeaderId).ThenBy(c => c.ApplicationDate).ToList();
                        else
                            cashReceiptDetailViewModel.ChargesWithPPD = null;

                        #endregion Charges with PPD
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return cashReceiptDetailViewModel;
        }

        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="cashReceipt">The cash receipt.</param>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        internal static CashReceiptViewModel ToViewModel(this CashReceipt cashReceipt, Account account)
        {
            var cashReceiptViewModel = new CashReceiptViewModel
            {
                IssSerial = cashReceipt.SerialNumber,
                IssPaymentCondition = cashReceipt.PaymentCondition,
                IssTaxpayerId = cashReceipt.IssuerTaxpayerId,
                CancelReasonName = cashReceipt.CancelReasonName,
                CFDIList = cashReceipt.CFDIList,
                CFDIRelated = cashReceipt.UUID,
                CFDIRelatedId = cashReceipt.CFDIRelatedId,
                chargesToInvoiceList = new List<ChargeCreditViewModel>(),
                Comments = cashReceipt.Comments,
                CorporateName = cashReceipt.ReceiverList[0].CorporateName,
                FiscalIdentityNumber = cashReceipt.ReceiverList[0].FiscalIdentityNumber,
                FiscalResidency = cashReceipt.ReceiverList[0].FiscalResidencyDesc,
                FiscalResidencyCode = cashReceipt.ReceiverList[0].FiscalResidency,
                InvoiceTaxPayerId = cashReceipt.ReceiverList[0].InvoiceTaxpayerId,
                InvoiceExpeditionId = cashReceipt.InvoiceExpeditionId,
                PaymentType = cashReceipt.PaymentType,
                PaymentTypeList = cashReceipt.PaymentTypeList,
                peopleOrgId = cashReceipt.peopleOrgId,
                PostalCode = cashReceipt.PostalCode,
                PreferredCFDIUsageCode = cashReceipt.ReceiverList[0].PreferredCFDIUsageCode,
                preferredReceiverEmail = cashReceipt.preferredReceiverEmail,
                receiptNumber = cashReceipt.receiptNumber,
                ReceiverPaymentMethodDefault = cashReceipt.ReceiverPaymentMethodDefault,
                ReceiverPaymentMethodList = new List<FiscalRecordCatalog>(),
                RecTaxRegimen = cashReceipt.RecTaxRegimen,
                RelationTypeDesc = string.IsNullOrEmpty(cashReceipt.RelationTypeCode) ? string.Empty : cashReceipt.RelationTypeCode + " - " + cashReceipt.RelationTypeDesc,
                TaxPayerId = String.IsNullOrEmpty(cashReceipt.ReceiverList[0].FiscalIdentityNumber) ? cashReceipt.ReceiverList[0].TaxPayerId : cashReceipt.ReceiverList[0].TaxPayerId + "-" + cashReceipt.ReceiverList[0].FiscalIdentityNumber
            };

            if (cashReceipt.chargesToInvoiceList != null)
            {
                decimal total;
                foreach (ChargeCredit item in cashReceipt.chargesToInvoiceList)
                {
                    total = item.Quantity * item.UnitAmount;
                    cashReceiptViewModel.chargesToInvoiceList.Add(new ChargeCreditViewModel
                    {
                        Quantity = item.Quantity,
                        ChargeCreditCode = item.ChargeCreditCode,
                        ChargeCreditCodeId = item.ChargeCreditCodeId,
                        ChargeCreditDesc = item.ChargeCreditDesc,
                        UnitAmount = item.UnitAmount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        ProductServiceKey = item.ProductServiceKey,
                        ProductServiceDesc = item.ProductServiceDesc,
                        UnityKey = item.UnitKey,
                        UnityName = item.UnityName,
                        TotalTaxes = ((decimal)(item.TotalTaxes)).ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        TotalUnit = total.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        isATax = item.IsATax,
                        IsEmptyProductServiceKey = item.IsEmptyProductServiceKey,
                        SubjectToTax = item.SubjectToTax
                    });
                }
            }

            cashReceiptViewModel.SubTotal = cashReceipt.SubTotal.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            cashReceiptViewModel.TotalTT = cashReceipt.TotalTT.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            cashReceiptViewModel.Total = cashReceipt.Total.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);

            return cashReceiptViewModel;
        }

        /// <summary>
        /// Converts to InvoiceCashReceipt view model.
        /// </summary>
        /// <param name="cashReceipt">The cash receipt.</param>
        /// <returns></returns>
        internal static InvoiceCashReceiptViewModel ToViewModel(this InvoiceCashReceipt cashReceipt, Account account)
        {
            var cashReceiptViewModel = new InvoiceCashReceiptViewModel
            {
                Amount = cashReceipt.Amount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                EntryDate = cashReceipt.EntryDate.ToString(account.reportFormats.DateFormat),
                PeopleOrgFullName = cashReceipt.PeopleOrgFullName,
                PeopleOrgId = cashReceipt.PeopleOrgId,
                ReceiptNumber = cashReceipt.ReceiptNumber,
                TaxAmount = cashReceipt.TaxAmount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
            };

            return cashReceiptViewModel;
        }

        /// <summary>
        /// Receivers to receiver view.
        /// </summary>
        /// <param name="receiptPaymentMethodMapping">The receipt payment method mapping.</param>
        /// <returns></returns>
        internal static ReceiptPaymentMethodMappingViewModel ToViewModel(this ReceiptPaymentMethodMapping receiptPaymentMethodMapping)
        {
            var receiptPaymentMethodMappingViewModel = new ReceiptPaymentMethodMappingViewModel
            {
                Id = receiptPaymentMethodMapping.ReceiptPaymentMethodMappingId,
                ChargeCreditCodeId = receiptPaymentMethodMapping.ChargeCreditCodeId,
                ChargeCreditDesc = receiptPaymentMethodMapping.ChargeCreditDesc,
                PaymentMethodCode = receiptPaymentMethodMapping.PaymentMethodCode,
                PaymentMethodDesc = receiptPaymentMethodMapping.PaymentMethodDesc
            };
            return receiptPaymentMethodMappingViewModel;
        }

        /// <summary>
        /// To the receiver entity.
        /// </summary>
        /// <param name="receiptPaymentMethodMappingViewModel">The receipt payment method mapping view model.</param>
        /// <returns></returns>
        internal static ReceiptPaymentMethodMapping ToViewModel(this ReceiptPaymentMethodMappingViewModel receiptPaymentMethodMappingViewModel)
        {
            var receiptPaymentMethodMapping = new ReceiptPaymentMethodMapping
            {
                ReceiptPaymentMethodMappingId = receiptPaymentMethodMappingViewModel.Id,
                ChargeCreditCodeId = receiptPaymentMethodMappingViewModel.ChargeCreditCodeId,
                ChargeCreditDesc = receiptPaymentMethodMappingViewModel.ChargeCreditDesc,
                PaymentMethodCode = receiptPaymentMethodMappingViewModel.PaymentMethodCode,
                PaymentMethodDesc = receiptPaymentMethodMappingViewModel.PaymentMethodDesc
            };
            return receiptPaymentMethodMapping;
        }

        /// <summary>
        /// Converts to ChargeCredit view model.
        /// </summary>
        /// <param name="chargeCredits">The cash charge credits.</param>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        internal static List<ChargeCreditViewModel> ToViewModel(this List<ChargeCredit> chargeCredits, Account account)
        {
            List<ChargeCreditViewModel> chargeCreditViewModels = new List<ChargeCreditViewModel>();

            if (chargeCredits != null)
            {
                foreach (ChargeCredit chargeCredit in chargeCredits)
                {
                    chargeCreditViewModels.Add(new ChargeCreditViewModel
                    {
                        Quantity = chargeCredit.Quantity,
                        ReceiptNumber = chargeCredit.ReceiptNumber,
                        ProductServiceKey = chargeCredit.ProductServiceKey,
                        UnityKey = chargeCredit.UnitKey,
                        UnityName = chargeCredit.UnityName,
                        ChargeCreditCode = chargeCredit.ChargeCreditCode,
                        ChargeCreditDesc = chargeCredit.ChargeCreditDesc,
                        UnitAmount = chargeCredit.UnitAmount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        TotalTaxes = ((decimal)(chargeCredit.TotalTaxes)).ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        TotalUnit = chargeCredit.UnitAmount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        isATax = chargeCredit.IsATax,
                        SubjectToTax = chargeCredit.SubjectToTax
                    });
                }
            }
            return chargeCreditViewModels;
        }

        /// <summary>
        /// Converts to viewmodelforglobal.
        /// </summary>
        /// <param name="globalFiscal">The global fiscal.</param>
        /// <param name="cashReceiptViewModel">The cash receipt view model.</param>
        /// <returns></returns>
        internal static CashReceiptViewModel ToViewModelForGlobal(this CashReceipt globalFiscal, CashReceiptViewModel cashReceiptViewModel = null, FiscalRecord fiscalRecord = null, Account account = null)
        {
            if (cashReceiptViewModel == null)
                cashReceiptViewModel = new CashReceiptViewModel();
            cashReceiptViewModel.ReceiverPaymentMethodDefault = globalFiscal.ReceiverPaymentMethodDefault;
            cashReceiptViewModel.PaymentTypeList = globalFiscal.PaymentTypeList;
            cashReceiptViewModel.CFDIList = globalFiscal.CFDIList;
            cashReceiptViewModel.FrequencyList = globalFiscal.FrequencyList;
            cashReceiptViewModel.MonthList = GetMonthsCatalog();
            cashReceiptViewModel.YearList = GetYearsCatalog();
            cashReceiptViewModel.RecTaxAddress = globalFiscal.ReceiverList[0].PostalCode;
            cashReceiptViewModel.RecTaxRegimen = globalFiscal.ReceiverList[0].TaxRegimenCode + " " + FiscalRecordModelResource.Dash + " " + globalFiscal.ReceiverList[0].TaxRegimenDesc;
            cashReceiptViewModel.CorporateName = globalFiscal.ReceiverList[0].CorporateName;
            if (fiscalRecord != null)
            {
                cashReceiptViewModel.Frequency = new FiscalRecordCatalog() { Code = fiscalRecord.Frequency };
                cashReceiptViewModel.Month = new FiscalRecordCatalog() { Code = fiscalRecord.MonthCode };
                cashReceiptViewModel.Year = new FiscalRecordCatalog() { Code = fiscalRecord.Year };
                cashReceiptViewModel.StartDate = fiscalRecord.StartDate != null ? fiscalRecord.StartDate.Value.ToString(account.reportFormats.DateFormat) : string.Empty;
                cashReceiptViewModel.EndDate = fiscalRecord.EndDate != null ? fiscalRecord.EndDate.Value.ToString(account.reportFormats.DateFormat) : string.Empty;
            }
            return cashReceiptViewModel;
        }

        /// <summary>
        /// Gets the global cash receipts.
        /// </summary>
        /// <param name="GlobalCashReceipts">The global cash receipts.</param>
        /// <param name="account">The user account.</param>
        /// <returns></returns>
        internal static CashReceiptViewModel ToViewModelForGlobalWithCharges(this CashReceipt GlobalCashReceipts, Account account)
        {
            var globalCashReceipts = new CashReceiptViewModel()
            {
                IssSerial = GlobalCashReceipts.SerialNumber,
                IssPaymentCondition = GlobalCashReceipts.PaymentCondition,
                IssTaxpayerId = GlobalCashReceipts.IssuerTaxpayerId,
                CancelReasonName = GlobalCashReceipts.CancelReasonName,
                CFDIList = GlobalCashReceipts.CFDIList,
                CFDIRelated = GlobalCashReceipts.UUID,
                CFDIRelatedId = GlobalCashReceipts.CFDIRelatedId,
                Comments = GlobalCashReceipts.Comments,
                InvoiceExpeditionId = GlobalCashReceipts.InvoiceExpeditionId,
                PaymentType = GlobalCashReceipts.PaymentType,
                PaymentTypeList = GlobalCashReceipts.PaymentTypeList,
                peopleOrgId = GlobalCashReceipts.peopleOrgId,
                PostalCode = GlobalCashReceipts.PostalCode,
                preferredReceiverEmail = GlobalCashReceipts.preferredReceiverEmail,
                receiptNumber = GlobalCashReceipts.receiptNumber,
                ReceiverPaymentMethodDefault = GlobalCashReceipts.ReceiverPaymentMethodDefault,
                RecTaxRegimen = GlobalCashReceipts.RecTaxRegimen,
                RelationTypeDesc = string.IsNullOrEmpty(GlobalCashReceipts.RelationTypeCode) ? string.Empty : GlobalCashReceipts.RelationTypeCode + " - " + GlobalCashReceipts.RelationTypeDesc,
                StartDate = GlobalCashReceipts.StartDate.HasValue ? GlobalCashReceipts.StartDate.Value.ToString("dd/MM/yyyy") : null,
                EndDate = GlobalCashReceipts.EndDate.HasValue ? GlobalCashReceipts.EndDate.Value.ToString("dd/MM/yyyy") : null,
                Frequency = string.IsNullOrEmpty(GlobalCashReceipts.FrequencyCode) ? null : new FiscalRecordCatalog() { Code = GlobalCashReceipts.FrequencyCode },
                Year = string.IsNullOrEmpty(GlobalCashReceipts.Year) ? null : new FiscalRecordCatalog() { Id = int.Parse(GlobalCashReceipts.Year) },
                Month = string.IsNullOrEmpty(GlobalCashReceipts.MonthCode) ? null : new FiscalRecordCatalog() { Code = GlobalCashReceipts.MonthCode }
            };

            if (GlobalCashReceipts.chargesToInvoiceList != null)
            {
                globalCashReceipts.chargesToInvoiceList = new List<ChargeCreditViewModel>();
                foreach (ChargeCredit item in GlobalCashReceipts.chargesToInvoiceList)
                {
                    globalCashReceipts.chargesToInvoiceList.Add(new ChargeCreditViewModel
                    {
                        Quantity = item.Quantity,
                        ReceiptNumber = item.ReceiptNumber,
                        ProductServiceKey = item.ProductServiceKey,
                        UnityKey = item.UnitKey,
                        ChargeCreditDesc = item.ChargeCreditDesc,
                        UnitAmount = item.UnitAmount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        TotalTaxes = ((decimal)(item.TotalTaxes)).ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        TotalUnit = item.UnitAmount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        isATax = item.IsATax,
                        SubjectToTax = item.SubjectToTax
                    });
                }
            }

            globalCashReceipts.SubTotal = GlobalCashReceipts.SubTotal.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            globalCashReceipts.TotalTT = GlobalCashReceipts.TotalTT.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            globalCashReceipts.Total = GlobalCashReceipts.Total.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);

            return globalCashReceipts;
        }

        private static List<FiscalRecordCatalog> GetMonthsCatalog()
        {
            List<FiscalRecordCatalog> months = new List<FiscalRecordCatalog>()
            {
                new FiscalRecordCatalog()
                {
                    Code = "01",
                    Description = "Enero"
                },
                new FiscalRecordCatalog()
                {
                    Code = "02",
                    Description = "Febrero"
                },
                new FiscalRecordCatalog()
                {
                    Code = "03",
                    Description = "Marzo"
                },
                new FiscalRecordCatalog()
                {
                    Code = "04",
                    Description = "Abril"
                },
                new FiscalRecordCatalog()
                {
                    Code = "05",
                    Description = "Mayo"
                },
                new FiscalRecordCatalog()
                {
                    Code = "06",
                    Description = "Junio"
                },
                new FiscalRecordCatalog()
                {
                    Code = "07",
                    Description = "Julio"
                },
                new FiscalRecordCatalog()
                {
                    Code = "08",
                    Description = "Agosto"
                },
                new FiscalRecordCatalog()
                {
                    Code = "09",
                    Description = "Septiembre"
                },
                new FiscalRecordCatalog()
                {
                    Code = "10",
                    Description = "Octubre"
                },
                new FiscalRecordCatalog()
                {
                    Code = "11",
                    Description = "Noviembre"
                },
                new FiscalRecordCatalog()
                {
                    Code = "12",
                    Description = "Diciembre"
                }
            };

            return months;
        }

        private static List<FiscalRecordCatalog> GetYearsCatalog()
        {
            int currentYear = DateTime.Now.Year;
            int lastYear = currentYear - 1;

            List<FiscalRecordCatalog> years = new List<FiscalRecordCatalog>()
            {
                new FiscalRecordCatalog()
                {
                    Code = lastYear.ToString(),
                    Description = lastYear.ToString()
                },
                new FiscalRecordCatalog()
                {
                    Code = currentYear.ToString(),
                    Description = currentYear.ToString()
                }
            };

            return years;
        }
    }
}