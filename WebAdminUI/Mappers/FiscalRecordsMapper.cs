// --------------------------------------------------------------------
// <copyright file="FiscalRecordsMapper.cs" company="Ellucian">
//     Copyright 2018 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebAdminUI.CashReceiptsMappers;
using WebAdminUI.Helpers;
using WebAdminUI.Mappers;
using WebAdminUI.Models.FiscalRecords;
using WebAdminUI.Models.Issuers;
using WebAdminUI.Models.Resources;
using WebAdminUI.Views.FiscalRecords.App_LocalResources;

namespace WebAdminUI.FiscalRecordsMappers
{
    /// <summary>
    /// Mapper for fiscal records view models
    /// </summary>
    internal static class FiscalRecordsMapper
    {
        /// <summary>
        /// Gets the relationship type description.
        /// </summary>
        /// <param name="cFDIRelationType">Type of the cfdi relation.</param>
        /// <param name="cFDIRelationTypeDesc">The cfdi relation type desc.</param>
        /// <param name="cFDIRelated">The cfdi related.</param>
        /// <returns></returns>
        internal static string GetRelationshipTypeDescription(string cFDIRelationType, string cFDIRelationTypeDesc, string cFDIRelated)
        {
            try
            {
                if (!string.IsNullOrEmpty(cFDIRelationType) && !string.IsNullOrEmpty(cFDIRelationTypeDesc) && !string.IsNullOrEmpty(cFDIRelated))
                {
                    return string.Format("{0} - {1} {2}", cFDIRelationType, cFDIRelationTypeDesc, cFDIRelated);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the description of the request state
        /// </summary>
        /// <param name="requestState"></param>
        /// <returns></returns>
        internal static string GetRequestStateDescription(enumFiscalRecordStatus requestState)
        {
            try
            {
                switch (requestState)
                {
                    case enumFiscalRecordStatus.Active:
                        return FiscalRecordModelResource.Active;

                    case enumFiscalRecordStatus.Canceled:
                        return FiscalRecordModelResource.Canceled;

                    case enumFiscalRecordStatus.RequestedFiscalRecord:
                        return FiscalRecordModelResource.RequestedFiscalRecord;

                    case enumFiscalRecordStatus.ProviderCannotCreate:
                        return FiscalRecordModelResource.ProviderCannotCreate;

                    case enumFiscalRecordStatus.ProviderIsCreating:
                        return FiscalRecordModelResource.ProviderIsCreating;

                    case enumFiscalRecordStatus.ProviderCannotCancel:
                        return FiscalRecordModelResource.ProviderCannotCancel;

                    case enumFiscalRecordStatus.RequestedCancellation:
                        return FiscalRecordModelResource.RequestedCancellation;

                    case enumFiscalRecordStatus.ProviderIsCanceling:
                        return FiscalRecordModelResource.ProviderIsCanceling;

                    case enumFiscalRecordStatus.Null:
                        return string.Empty;

                    default:
                        return string.Empty;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// To the receiver entity.
        /// </summary>
        /// <param name="invoiceExpeditionViewModel">The invoice expedition view model.</param>
        /// <returns></returns>
        internal static InvoiceExpedition ToDataEntity(this InvoiceExpeditionViewModel invoiceExpeditionViewModel)
        {
            var invoiceExpedition = new InvoiceExpedition
            {
                InvoiceExpeditionId = invoiceExpeditionViewModel.InvoiceExpeditionId,
                InvoiceOrganizationId = invoiceExpeditionViewModel.InvoiceOrganizationId,
                PostalCode = invoiceExpeditionViewModel.PostalCode,
                State = invoiceExpeditionViewModel.State,
                Description = invoiceExpeditionViewModel.Description,
                CanDelete = invoiceExpeditionViewModel.CanDelete
            };
            return invoiceExpedition;
        }

        /// <summary>
        /// To the invoice receipt entity.
        /// </summary>
        /// <param name="invoiceReceiptViewModel">The invoice receipt view model.</param>
        /// <returns></returns>
        internal static InvoiceReceipt ToDataEntity(this InvoiceReceiptViewModel invoiceReceiptViewModel)
        {
            var invoiceReceipt = new InvoiceReceipt
            {
                InvoiceExpeditionId = invoiceReceiptViewModel.InvoiceExpeditionId,
                InvoiceExpeditionDescription = invoiceReceiptViewModel.InvoiceExpeditionDescription,
                InvoiceOrganizationId = invoiceReceiptViewModel.InvoiceOrganizationId,
                InvoiceReceiptId = invoiceReceiptViewModel.InvoiceReceiptId,
                SerialNumber = invoiceReceiptViewModel.SerialNumber,
                Folio = invoiceReceiptViewModel.Folio
            };
            return invoiceReceipt;
        }

        /// <summary>
        /// To the receiver entity.
        /// </summary>
        /// <param name="invoiceOrganizationViewModel">The invoice organization view model.</param>
        /// <returns></returns>
        internal static InvoiceOrganization ToDataEntity(this InvoiceOrganizationViewModel invoiceOrganizationViewModel)
        {
            var invoiceOrganization = new InvoiceOrganization
            {
                CorporateName = invoiceOrganizationViewModel.CorporateName,
                InvoiceOrganizationId = invoiceOrganizationViewModel.InvoiceOrganizationId,
                InvoiceOrgTaxRegimenId = invoiceOrganizationViewModel.InvoiceOrgTaxRegimenId,
                TaxpayerId = invoiceOrganizationViewModel.TaxpayerId,
                TaxRegimenId = invoiceOrganizationViewModel.TaxRegimenId
            };
            return invoiceOrganization;
        }

        /// <summary>
        /// Map the fiscal records related entity to fiscal record origin view model
        /// </summary>
        /// <param name="fiscalRecodsRelated"></param>
        /// <returns></returns>
        internal static FiscalRecordOriginViewModel ToViewModel(this List<FiscalRecord>[] fiscalRecodsRelated)
        {
            FiscalRecordOriginViewModel fiscalRecordOriginViewModel = null;

            try
            {
                if (fiscalRecodsRelated?[0]?.Count > 0)
                {
                    fiscalRecordOriginViewModel = new FiscalRecordOriginViewModel();
                    List<FiscalRecordRelatedViewModel> tempList = null;
                    for (int i = 0; i < 2; i++)
                    {
                        if (fiscalRecodsRelated[i]?.Count > 0)
                        {
                            tempList = fiscalRecodsRelated[i].Select(fr => new FiscalRecordRelatedViewModel
                            {
                                InvoiceHeaderId = fr.InvoiceHeaderId,
                                ExpeditionDateTime = fr.ExpeditionDateTime.Value,
                                VoucherType = fr.VoucherType,
                                FiscalRecordType = fr.VoucherType,
                                Folio = fr.Folio,
                                SerialNumber = fr.SerialNumber,
                                Total = fr.Total,
                                RequestState = GetRequestStateDescription(fr.RequestState),
                                RelationshipType = i == 1 ? GetRelationshipTypeDescription(fr.CFDIRelationType, fr.CFDIRelationTypeDesc, fr.CFDIRelated) : string.Empty,
                                RelationshipType2 = i == 1 ? GetRelationshipTypeDescription(fr.CFDIRelationType2, fr.CFDIRelationTypeDesc2, fr.CFDIRelated2) : string.Empty,
                                PaymentMethod = fr.PaymentMethod,
                                UUID = fr.UUID,
                            }).OrderBy(fr => fr.ExpeditionDateTime).ToList();
                        }
                        switch (i)
                        {
                            case 0:
                                fiscalRecordOriginViewModel.Origin = tempList;
                                break;

                            case 1:
                                fiscalRecordOriginViewModel.Parent = tempList;
                                break;
                        }

                        tempList = null;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return fiscalRecordOriginViewModel;
        }

        internal static FiscalRecordOriginViewModel ToViewModel(this List<PaymentReceipt>[] fiscalRecodsRelated)
        {
            FiscalRecordOriginViewModel fiscalRecordOriginViewModel = null;
            if (fiscalRecodsRelated?[0]?.Count > 0)
            {
                fiscalRecordOriginViewModel = new FiscalRecordOriginViewModel();
                List<FiscalRecordRelatedViewModel> tempList = null;
                for (int i = 0; i < 4; i++)
                {
                    if (fiscalRecodsRelated[i]?.Count > 0)
                    {
                        tempList = fiscalRecodsRelated[i].Select(fr => new FiscalRecordRelatedViewModel
                        {
                            InvoiceHeaderId = fr.InvoiceHeaderId,
                            ExpeditionDateTime = fr.ExpeditionDateTime.Value,
                            VoucherType = fr.VoucherType,
                            FiscalRecordType = fr.FiscalRecordType,
                            Folio = fr.Folio,
                            SerialNumber = fr.SerialNumber,
                            Total = fr.FiscalRecordType == Constants.FiscalRecordTypePago ? fr.Amount ?? 0m : fr.Total,
                            RequestState = GetRequestStateDescription(fr.RequestState),
                            RelationshipType = i == 1 ? GetRelationshipTypeDescription(fr.CFDIRelationType, fr.CFDIRelationTypeDesc, fr.CFDIRelated) : string.Empty,
                            RelationshipType2 = i == 1 ? GetRelationshipTypeDescription(fr.CFDIRelationType2, fr.CFDIRelationTypeDesc2, fr.CFDIRelated2) : string.Empty,
                        }).OrderBy(fr => fr.ExpeditionDateTime).ToList();
                    }
                    switch (i)
                    {
                        case 0:
                            fiscalRecordOriginViewModel.Origin = tempList;
                            break;

                        case 1:
                            fiscalRecordOriginViewModel.Parent = tempList;
                            break;
                    }

                    tempList = null;
                }
            }

            return fiscalRecordOriginViewModel;
        }

        /// <summary>
        /// To the fiscal record view model.
        /// </summary>
        /// <param name="fiscalRecord">The fiscal record entity.</param>
        /// <param name="account">The user account.</param>
        /// <returns></returns>
        internal static FiscalRecordViewModel ToViewModel(this FiscalRecord fiscalRecord, Account account)
        {
            string letterPO = string.IsNullOrEmpty(fiscalRecord.PeopleOrgCodeId) ? string.Empty : fiscalRecord.PeopleOrgCodeId.Substring(0, 1);
            string formatPW = string.IsNullOrEmpty(fiscalRecord.PeopleOrgCodeId) ? string.Empty : PowerCampusSystemFormat.FormatPeopleCodeId(fiscalRecord.PeopleOrgCodeId.Substring(1));

            var fiscalRecordViewModel = new FiscalRecordViewModel
            {
                CancelReasonName = fiscalRecord.CancelReasonName,
                CancelReasonKey = fiscalRecord.CancelReasonKey,
                InvoiceHeaderId = fiscalRecord.InvoiceHeaderId,
                PeopleOrgCodeId = letterPO + formatPW,
                IsCancellationInProgress = !fiscalRecord.IsCancellationComplete,
                expeditionDateTime = fiscalRecord.ExpeditionDateTime.HasValue ?
                    fiscalRecord.ExpeditionDateTime.Value.ToString(account.reportFormats.DateFormat)
                    + " "
                    + fiscalRecord.ExpeditionDateTime.Value.ToString("HH:mm:ss")
                    : string.Empty,
                serialNumber = fiscalRecord.SerialNumber,
                folio = fiscalRecord.Folio,
                receiverTaxPayerId = fiscalRecord.Receiver.TaxPayerId,
                fiscalRecordType = fiscalRecord.FiscalRecordType,
                requestState = GetRequestStateDescription(fiscalRecord.RequestState),
                FiscalRecordStatusEnum = fiscalRecord.RequestState
            };

            return fiscalRecordViewModel;
        }

        /// <summary>
        /// To the new invoice view model.
        /// </summary>
        /// <param name="globalInvoiceCancellationDetails">The global invoice cancellation details.</param>
        /// <param name="account">The user account.</param>
        /// <returns></returns>
        internal static GlobalInvoiceDetailViewModel ToViewModel(this GlobalInvoiceCancellationDetails globalInvoiceCancellationDetails, Account account)
        {
            List<InvoiceCashReceiptViewModel> invoiceCashReceipts = new List<InvoiceCashReceiptViewModel>();
            List<FiscalRecordViewModel> newInvoices = new List<FiscalRecordViewModel>();

            if (globalInvoiceCancellationDetails != null)
            {
                if (globalInvoiceCancellationDetails.CashReceipts != null)
                {
                    foreach (InvoiceCashReceipt invoiceCashReceipt in globalInvoiceCancellationDetails.CashReceipts)
                    {
                        invoiceCashReceipts.Add(invoiceCashReceipt.ToViewModel(account));
                    }
                }

                if (globalInvoiceCancellationDetails.NewInvoices != null)
                {
                    foreach (FiscalRecord fiscalRecord in globalInvoiceCancellationDetails.NewInvoices)
                    {
                        newInvoices.Add(fiscalRecord.ToViewModel(account));
                    }
                }
            }

            return new GlobalInvoiceDetailViewModel
            {
                CashReceipts = invoiceCashReceipts,
                NewInvoices = newInvoices
            };
        }

        /// <summary>
        /// Receivers to receiver view.
        /// </summary>
        /// <param name="invoiceExpedition">The invoice expedition.</param>
        /// <returns></returns>
        internal static InvoiceExpeditionViewModel ToViewModel(this InvoiceExpedition invoiceExpedition)
        {
            var invoiceExpeditionViewModel = new InvoiceExpeditionViewModel
            {
                InvoiceExpeditionId = invoiceExpedition.InvoiceExpeditionId,
                InvoiceOrganizationId = invoiceExpedition.InvoiceOrganizationId,
                PostalCode = invoiceExpedition.PostalCode,
                State = invoiceExpedition.State,
                Description = invoiceExpedition.Description,
                CanDelete = invoiceExpedition.CanDelete
            };
            return invoiceExpeditionViewModel;
        }

        /// <summary>
        /// To the invoice receipt view model.
        /// </summary>
        /// <param name="invoiceReceipt">The invoice receipt.</param>
        /// <param name="account">The user account.</param>
        /// <returns></returns>
        internal static InvoiceReceiptViewModel ToViewModel(this InvoiceReceipt invoiceReceipt, Account account)
        {
            var invoiceReceiptViewModel = new InvoiceReceiptViewModel
            {
                InvoiceExpeditionId = invoiceReceipt.InvoiceExpeditionId,
                InvoiceExpeditionDescription = invoiceReceipt.InvoiceExpeditionDescription,
                InvoiceOrganizationId = invoiceReceipt.InvoiceOrganizationId,
                InvoiceReceiptId = invoiceReceipt.InvoiceReceiptId,
                SerialNumber = invoiceReceipt.SerialNumber,
                Folio = invoiceReceipt.Folio,
                LastFolioAssigned = invoiceReceipt.LastFolioAssigned,
                StartDate = invoiceReceipt.StartDate.ToString(account.reportFormats.DateFormat),
                EndDate = invoiceReceipt.EndDate.HasValue ? invoiceReceipt.EndDate.Value.ToString(account.reportFormats.DateFormat) : string.Empty,
                Version = invoiceReceipt.Version
            };
            return invoiceReceiptViewModel;
        }

        /// <summary>
        /// To the invoice organization view model.
        /// </summary>
        /// <param name="invoiceOrganization">The invoice organization.</param>
        /// <returns></returns>
        internal static InvoiceOrganizationViewModel ToViewModel(this InvoiceOrganization invoiceOrganization)
        {
            var invoiceOrganizationViewModel = new InvoiceOrganizationViewModel
            {
                CorporateName = invoiceOrganization.CorporateName,
                InvoiceOrganizationId = invoiceOrganization.InvoiceOrganizationId,
                InvoiceOrgTaxRegimenId = invoiceOrganization.InvoiceOrgTaxRegimenId,
                TaxpayerId = invoiceOrganization.TaxpayerId,
                TaxRegimenId = invoiceOrganization.TaxRegimenId
            };
            return invoiceOrganizationViewModel;
        }

        /// <summary>
        /// To the fiscal record credit notes model.
        /// </summary>
        /// <param name="fiscalRecord">The fiscal record.</param>
        /// <param name="account">The user account.</param>
        /// <returns></returns>
        internal static FiscalRecordViewModel ToViewModelCreditNote(this FiscalRecord fiscalRecord, Account account)
        {
            var fiscalRecordViewModel = new FiscalRecordViewModel
            {
                InvoiceHeaderId = fiscalRecord.InvoiceHeaderId,
                InvoiceExpeditionId = fiscalRecord.InvoiceExpeditionId,
                PeopleOrgCodeId = fiscalRecord.PeopleOrgCodeId,
                folio = fiscalRecord.Folio,
                receiverTaxPayerId = fiscalRecord.Receiver.TaxPayerId,
                receiverCorporateName = fiscalRecord.Receiver.CorporateName,
                receiverFiscalResidency = !string.IsNullOrEmpty(fiscalRecord.Receiver.FiscalResidency) ? fiscalRecord.Receiver.FiscalResidency + FiscalRecordModelResource.Dash + fiscalRecord.Receiver.FiscalResidencyDesc : string.Empty,
                receiverFiscalIdentityNumber = fiscalRecord.Receiver.FiscalIdentityNumber,
                receiverEmail = fiscalRecord.Receiver.Email,
                issuerIssTaxpayerId = fiscalRecord.Issuer.IssTaxpayerId,
                IssIssuerInvoiceOrganizationId = fiscalRecord.Issuer.IssInvoiceOrganizationId,
                IssuerCorporateName = fiscalRecord.Issuer.IssCorporateName,
                IssuerTaxRegimen = fiscalRecord.Issuer.TaxRegimenCode + FiscalRecordModelResource.Dash + fiscalRecord.Issuer.TaxRegimenDesc,
                IssSerial = fiscalRecord.Issuer.IssSerial,
                serialNumber = fiscalRecord.SerialNumber,
                paymentType = fiscalRecord.PaymentType + FiscalRecordModelResource.Dash + fiscalRecord.PaymentTypeDesc,
                paymentCondition = fiscalRecord.PaymentCondition,
                currency = fiscalRecord.Currency,
                paymentMethod = fiscalRecord.PaymentMethod + FiscalRecordModelResource.Dash + fiscalRecord.PaymentMethodDesc,
                subtotal = fiscalRecord.SubTotal.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                totalTransferTaxed = fiscalRecord.TotalTransferTaxes.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                total = fiscalRecord.Total.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                invoiceStatus = fiscalRecord.InvoiceStatus,
                requestStateId = fiscalRecord.RequestStateId,
                comments = fiscalRecord.Comments,
                uuid = fiscalRecord.UUID,
                fiscalRecordType = fiscalRecord.FiscalRecordType,
                CFDIUsageCatalogList = fiscalRecord.CFDIUsageCatalogList,
                CFDIUsageCode = fiscalRecord.CFDIUsageCode,
                CFDIUsageDesc = fiscalRecord.CFDIUsageDesc,
                PaymentTypeList = fiscalRecord.PaymentTypeList,
                ChargeCreditCodeId = fiscalRecord.ChargeCreditCodeId,
                ChargeCreditCodeTaxId = fiscalRecord.ChargeCreditCodeTaxId,
                IsAGlobalCreditNote = fiscalRecord.IsAGlobalCreditNote,
                IsAPPDTaxCreditNote = fiscalRecord.IsAPPDTaxCreditNote,
                ReceiverTaxRegimen = fiscalRecord.Receiver.TaxRegimenCode + FiscalRecordModelResource.Dash + fiscalRecord.Receiver.TaxRegimenDesc,
                RecTaxAddress = fiscalRecord.Receiver.PostalCode,
                CFDIRelated = fiscalRecord.CFDIRelated,
                CFDIRelatedId = fiscalRecord.CFDIRelatedId,
                CancelReasonName = fiscalRecord.CancelReasonName,
                RelationType = string.IsNullOrEmpty(fiscalRecord.RelationTypeCode) ? String.Empty : fiscalRecord.RelationTypeCode + " - " + fiscalRecord.RelationTypeDesc
            };

            if (fiscalRecord.Issuer.IssIssuingAdd.Count > 0)
            {
                fiscalRecordViewModel.issIssuingAddIssIssuingAddress = fiscalRecord.Issuer.IssIssuingAdd[0].IssIssuingAddress;
                fiscalRecordViewModel.issIssuingAddIssPlaceIssue = fiscalRecord.Issuer.IssIssuingAdd[0].IssPlaceIssue;
            }

            fiscalRecordViewModel.InvoiceDetails = new List<ChargeCreditViewModel>();

            List<ChargeCredit> OnlyCharges = fiscalRecord.InvoiceDetails.Where(m => m.IsATax.Equals(false) && Math.Truncate(100 * m.UnitAmount) / 100 > 0.00M).ToList();

            decimal Subtotal = 0;
            decimal TotalTaxes = 0;
            decimal Total = 0;

            foreach (ChargeCredit details in OnlyCharges)
            {
                if (fiscalRecord.IsAPPDTaxCreditNote) //PPD with Tax
                {
                    Subtotal = fiscalRecord.InvoiceDetails[0].UnitAmount;
                    TotalTaxes = (decimal)fiscalRecord.InvoiceDetails[0].TotalTaxes;
                    Total = Subtotal + TotalTaxes;

                    fiscalRecordViewModel.InvoiceDetails.Add(new ChargeCreditViewModel
                    {
                        Quantity = fiscalRecord.InvoiceDetails[0].Quantity,
                        ChargeCreditCode = fiscalRecord.InvoiceDetails[0].ChargeCreditCode,
                        ChargeCreditCodeId = fiscalRecord.InvoiceDetails[0].ChargeCreditCodeId,
                        ChargeCreditDesc = fiscalRecord.InvoiceDetails[0].ChargeCreditDesc,
                        UnitAmount = Subtotal.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        ProductServiceKey = fiscalRecord.InvoiceDetails[0].ProductServiceKey,
                        ProductServiceDesc = fiscalRecord.InvoiceDetails[0].ProductServiceDesc,
                        UnityKey = fiscalRecord.InvoiceDetails[0].UnitKey,
                        UnityName = fiscalRecord.InvoiceDetails[0].UnityName,
                        TotalTaxes = TotalTaxes.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        TotalUnit = Total.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        InvoiceDetailId = fiscalRecord.InvoiceDetails[0].InvoiceDetailId,
                        isATax = fiscalRecord.InvoiceDetails[0].IsATax,
                        ChargeNumberSource = fiscalRecord.InvoiceDetails[0].ChargeNumberSource,
                        TaxChargeNumber = fiscalRecord.InvoiceDetails[0].TaxChargeNumber,
                        SubjectToTax = fiscalRecord.InvoiceDetails[0].SubjectToTax
                    });
                }
                else if (!string.IsNullOrEmpty(details.PeopleOrgCodeIdComplete)) //Global
                {
                    Subtotal = fiscalRecord.InvoiceDetails.Where(m => m.ChargeNumberSource.Equals(details.ChargeNumberSource) && m.ReceiptNumber.Equals(details.ReceiptNumber)).Sum(m => m.UnitAmount);
                    TotalTaxes = (decimal)fiscalRecord.InvoiceDetails.Where(m => m.ChargeNumberSource.Equals(details.ChargeNumberSource) && m.IsATax.Equals(true) && m.ReceiptNumber.Equals(details.ReceiptNumber)).Sum(m => m.TotalTaxes);
                    Total = Subtotal + TotalTaxes;

                    foreach (ChargeCredit row in fiscalRecord.InvoiceDetails.Where(m => m.IsATax.Equals(false) && m.ChargeNumberSource.Equals(details.ChargeNumberSource) && m.ReceiptNumber.Equals(details.ReceiptNumber)))
                    {
                        fiscalRecordViewModel.InvoiceDetails.Add(new ChargeCreditViewModel
                        {
                            Quantity = row.Quantity,
                            ChargeCreditCode = row.ChargeCreditCode,
                            ChargeCreditCodeId = row.ChargeCreditCodeId,
                            ChargeCreditDesc = row.ChargeCreditDesc,
                            UnitAmount = Subtotal.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                            ProductServiceKey = row.ProductServiceKey,
                            ProductServiceDesc = row.ProductServiceDesc,
                            UnityKey = row.UnitKey,
                            UnityName = (fiscalRecord.IsAGlobalCreditNote || string.IsNullOrEmpty(row.UnityName)) ? row.UnityName : Constants.UnitDefaultForCreditNote,
                            ReceiptNumber = row.ReceiptNumber,
                            TotalTaxes = TotalTaxes.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                            TotalUnit = Total.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                            InvoiceDetailId = row.InvoiceDetailId,
                            isATax = row.IsATax,
                            ChargeNumberSource = row.ChargeNumberSource,
                            TaxChargeNumber = row.TaxChargeNumber,
                            SubjectToTax = row.SubjectToTax
                        });
                    }
                }
                else
                {
                    Subtotal = fiscalRecord.InvoiceDetails.Where(m => m.ChargeNumberSource.Equals(details.ChargeNumberSource)).Sum(m => m.UnitAmount);
                    TotalTaxes = (decimal)fiscalRecord.InvoiceDetails.Where(m => m.ChargeNumberSource.Equals(details.ChargeNumberSource) && m.IsATax.Equals(true)).Sum(m => m.TotalTaxes);
                    Total = Subtotal + TotalTaxes;

                    foreach (ChargeCredit row in fiscalRecord.InvoiceDetails.Where(m => m.IsATax.Equals(false) && m.ChargeNumberSource.Equals(details.ChargeNumberSource)))
                    {
                        fiscalRecordViewModel.InvoiceDetails.Add(new ChargeCreditViewModel
                        {
                            Quantity = row.Quantity,
                            ChargeCreditCode = row.ChargeCreditCode,
                            ChargeCreditCodeId = row.ChargeCreditCodeId,
                            ChargeCreditDesc = row.ChargeCreditDesc,
                            UnitAmount = Subtotal.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                            ProductServiceKey = row.ProductServiceKey,
                            ProductServiceDesc = row.ProductServiceDesc,
                            UnityKey = row.UnitKey,
                            UnityName = (fiscalRecord.IsAGlobalCreditNote || string.IsNullOrEmpty(row.UnityName)) ? row.UnityName : Constants.UnitDefaultForCreditNote,
                            TotalTaxes = TotalTaxes.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                            TotalUnit = Total.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                            InvoiceDetailId = row.InvoiceDetailId,
                            isATax = row.IsATax,
                            ChargeNumberSource = row.ChargeNumberSource,
                            TaxChargeNumber = row.TaxChargeNumber,
                            SubjectToTax = row.SubjectToTax
                        });
                    }
                }
            }

            fiscalRecordViewModel.ChargeCredit = new List<ChargeCreditViewModel>();

            foreach (ChargeCredit row in fiscalRecord.ChargeCredit)
            {
                var chargeCreditViewModelRowCredit = new ChargeCreditViewModel
                {
                    ChargeCreditCodeId = row.ChargeCreditCodeId,
                    DistributionOrder = row.DistributionOrder,
                    ChargeCreditCode = row.ChargeCreditCode,
                    ChargeCreditDesc = row.DistributionOrder + " - " + row.ChargeCreditCode + " - " + row.ChargeCreditDesc
                };

                fiscalRecordViewModel.ChargeCredit.Add(chargeCreditViewModelRowCredit);
            }

            return fiscalRecordViewModel;
        }

        /// <summary>
        /// To the fiscal record edit view model.
        /// </summary>
        /// <param name="fiscalRecord">The fiscal record.</param>
        /// <param name="account">The user account.</param>
        /// <param name="isGlobal">if set to <c>true</c> [is global].</param>
        /// <returns></returns>
        internal static FiscalRecordViewModel ToViewModelForEdit(this FiscalRecord fiscalRecord, Account account, bool isGlobal = false)
        {
            string letterPO = string.Empty;
            string formatPW = string.Empty;

            #region FiscalRecord

            var fiscalRecordViewModel = new FiscalRecordViewModel
            {
                CancelReasonName = fiscalRecord.CancelReasonName,
                CancelReason = string.Format("{0} - {1}", fiscalRecord.CancelReasonKey, fiscalRecord.CancelReasonDesc),
                CancelReasonKey = fiscalRecord.CancelReasonKey,
                CFDIRelatedSubstitution = fiscalRecord.CFDIRelatedSubstitution,
                InvoiceHeaderId = fiscalRecord.InvoiceHeaderId,
                IsCancellationInProgress = !fiscalRecord.IsCancellationComplete,
                PeopleOrgCodeId = fiscalRecord.PeopleOrgCodeId,
                folio = fiscalRecord.Folio,
                Frequency = fiscalRecord.Frequency,
                Month = GetMonthDescription(fiscalRecord.MonthCode),
                Year = fiscalRecord.Year
            };
            var chargeCreditViewModelList = new List<ChargeCreditViewModel>();
            if (fiscalRecord.FiscalRecordDetailList != null)
            {
                foreach (FiscalRecordDetail row in fiscalRecord.FiscalRecordDetailList)
                {
                    letterPO = string.IsNullOrEmpty(row.PeopleOrgCodeId) ? string.Empty : row.PeopleOrgCodeId.Substring(0, 1);
                    formatPW = string.IsNullOrEmpty(row.PeopleOrgCodeId) ? string.Empty : PowerCampusSystemFormat.FormatPeopleCodeId(row.PeopleOrgCodeId.Substring(1));

                    chargeCreditViewModelList.Add(new ChargeCreditViewModel
                    {
                        Quantity = row.Quantity,
                        PeopleOrgCodeId = letterPO + formatPW,
                        PeopleOrgCodeIdComplete = row.PeopleOrgCodeId,
                        PeopleOrgFullName = row.PeopleOrgFullName,
                        ChargeCreditCode = row.ChargeCreditCode,
                        ReceiptNumber = row.ReceiptNumber,
                        ChargeCreditDesc = row.ChargeCreditDesc,
                        UnitAmount = row.UnitAmount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        ProductServiceKey = row.ProductServiceKey,
                        ProductServiceDesc = row.ProductServiceDesc,
                        UnityKey = row.UnityKey,
                        UnityName = row.UnityName,
                        TotalTaxes = ((decimal)(row.TotalTaxes)).ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        TotalUnit = row.Amount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                        CanCreateCreditNote = row.CanCreateCreditNote,
                        SubjectToTax = row.SubjectToTax
                    });
                }
            }
            if (string.IsNullOrEmpty(fiscalRecord.PeopleOrgCodeId))
                fiscalRecordViewModel.InvoiceDetails = chargeCreditViewModelList.OrderBy(m => m.ReceiptNumber).ToList();
            else
                fiscalRecordViewModel.InvoiceDetails = chargeCreditViewModelList;

            fiscalRecordViewModel.receiverTaxPayerId = fiscalRecord.Receiver.TaxPayerId;
            fiscalRecordViewModel.receiverCorporateName = fiscalRecord.Receiver.CorporateName;
            fiscalRecordViewModel.receiverFiscalResidency = fiscalRecord.Receiver.FiscalResidency + FiscalRecordModelResource.Dash + fiscalRecord.Receiver.FiscalResidencyDesc;
            fiscalRecordViewModel.receiverFiscalIdentityNumber = fiscalRecord.Receiver.FiscalIdentityNumber;
            fiscalRecordViewModel.receiverEmail = fiscalRecord.Receiver.Email;
            fiscalRecordViewModel.ReceiverTaxRegimen = fiscalRecord.Receiver.TaxRegimenCode + FiscalRecordModelResource.Dash + fiscalRecord.Receiver.TaxRegimenDesc;
            fiscalRecordViewModel.RecTaxAddress = fiscalRecord.Receiver.PostalCode;
            fiscalRecordViewModel.issuerIssTaxpayerId = fiscalRecord.Issuer.IssTaxpayerId;
            fiscalRecordViewModel.IssuerCorporateName = fiscalRecord.Issuer.IssCorporateName;
            fiscalRecordViewModel.IssuerTaxRegimen = fiscalRecord.Issuer.TaxRegimenCode + FiscalRecordModelResource.Dash + fiscalRecord.Issuer.TaxRegimenDesc;
            if (fiscalRecord.Issuer.IssIssuingAdd.Count > 0)
            {
                fiscalRecordViewModel.issIssuingAddIssIssuingAddress = fiscalRecord.Issuer.IssIssuingAdd[0].IssIssuingAddress;
                fiscalRecordViewModel.issIssuingAddIssPlaceIssue = fiscalRecord.Issuer.IssIssuingAdd[0].IssPostalCode;
            }
            fiscalRecordViewModel.IssSerial = fiscalRecord.Issuer.IssSerial;
            fiscalRecordViewModel.paymentType = fiscalRecord.PaymentType + FiscalRecordModelResource.Dash + fiscalRecord.PaymentTypeDesc;
            fiscalRecordViewModel.paymentCondition = fiscalRecord.PaymentCondition;
            fiscalRecordViewModel.currency = fiscalRecord.Currency;
            fiscalRecordViewModel.paymentMethod = fiscalRecord.PaymentMethod + FiscalRecordModelResource.Dash + fiscalRecord.PaymentMethodDesc;
            fiscalRecordViewModel.subtotal = fiscalRecord.SubTotal.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            fiscalRecordViewModel.totalTransferTaxed = fiscalRecord.TotalTransferTaxes.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            fiscalRecordViewModel.total = fiscalRecord.Total.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            fiscalRecordViewModel.invoiceStatus = fiscalRecord.InvoiceStatus;
            fiscalRecordViewModel.requestStateId = fiscalRecord.RequestStateId;
            fiscalRecordViewModel.comments = fiscalRecord.Comments;
            fiscalRecordViewModel.uuid = fiscalRecord.UUID;
            fiscalRecordViewModel.CFDIUsageCode = fiscalRecord.CFDIUsageCode + FiscalRecordModelResource.Dash + fiscalRecord.CFDIUsageDesc;
            fiscalRecordViewModel.fiscalRecordType = fiscalRecord.FiscalRecordType;
            fiscalRecordViewModel.VoucherType = fiscalRecord.VoucherType;
            fiscalRecordViewModel.serialNumber = fiscalRecord.SerialNumber;
            if (fiscalRecord.ExpeditionDateTime.HasValue)
            {
                fiscalRecordViewModel.expeditionDateTime = fiscalRecord.ExpeditionDateTime.Value.ToString(account.reportFormats.DateFormat)
                    + " "
                    + fiscalRecord.ExpeditionDateTime.Value.ToShortTimeString();
            }
            else
            {
                fiscalRecordViewModel.expeditionDateTime = string.Empty;
            }

            if (fiscalRecord.ApprovedDatetime.HasValue)
            {
                fiscalRecordViewModel.approvedDateTime = fiscalRecord.ApprovedDatetime.Value.ToString(account.reportFormats.DateFormat)
                    + " "
                    + fiscalRecord.ApprovedDatetime.Value.ToShortTimeString();
            }
            else
            {
                fiscalRecordViewModel.approvedDateTime = string.Empty;
            }

            if (fiscalRecord.CancellationDatetime.HasValue)
            {
                fiscalRecordViewModel.CancellationDatetime = fiscalRecord.CancellationDatetime.Value.ToString(account.reportFormats.DateFormat)
                    + " "
                    + fiscalRecord.CancellationDatetime.Value.ToShortTimeString();
            }
            else
            {
                fiscalRecordViewModel.CancellationDatetime = string.Empty;
            }

            fiscalRecordViewModel.errorText = fiscalRecord.ErrorText;
            if (isGlobal)
            {
                fiscalRecordViewModel.startDate = fiscalRecord.StartDate != null ? fiscalRecord.StartDate.Value.ToString("dd/MM/yyyy") : string.Empty;
                fiscalRecordViewModel.endDate = fiscalRecord.EndDate != null ? fiscalRecord.EndDate.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
            else
            {
                fiscalRecordViewModel.startDate = fiscalRecord.StartDate != null ? fiscalRecord.StartDate.Value.ToString(account.reportFormats.DateFormat) : string.Empty;
                fiscalRecordViewModel.endDate = fiscalRecord.EndDate != null ? fiscalRecord.EndDate.Value.ToString(account.reportFormats.DateFormat) : string.Empty;
            }
            fiscalRecordViewModel.CFDIRelated = fiscalRecord.CFDIRelated;
            fiscalRecordViewModel.CFDIRelated2 = fiscalRecord.CFDIRelated2;
            fiscalRecordViewModel.RelationType = string.IsNullOrEmpty(fiscalRecord.RelationTypeCode) ? String.Empty : fiscalRecord.RelationTypeCode + " - " + fiscalRecord.RelationTypeDesc;
            fiscalRecordViewModel.RelationType2 = string.IsNullOrEmpty(fiscalRecord.RelationTypeCode2) ? String.Empty : fiscalRecord.RelationTypeCode2 + " - " + fiscalRecord.RelationTypeDesc2;
            fiscalRecordViewModel.CanCreateCreditNote = fiscalRecord.CanCreateCreditNote;
            fiscalRecordViewModel.CanCancelInvoice = fiscalRecord.CanCancelInvoice;
            fiscalRecordViewModel.HasInvoiceRelated = fiscalRecord.HasInvoiceRelated;
            fiscalRecordViewModel.Version = string.IsNullOrEmpty(fiscalRecord.Version) ? 0 : double.Parse(fiscalRecord.Version);
            fiscalRecordViewModel.FiscalRecordStatusEnum = fiscalRecord.RequestState;
            fiscalRecordViewModel.requestState = GetRequestStateDescription(fiscalRecord.RequestState);

            #endregion FiscalRecord

            #region CerficateFile

            fiscalRecordViewModel.FiscalRecordCertificateList = new List<FiscalRecordCertificateViewModel>();
            if (fiscalRecord.FiscalRecordCertificateList != null)
            {
                foreach (FiscalRecordCertificate row in fiscalRecord.FiscalRecordCertificateList)
                {
                    fiscalRecordViewModel.FiscalRecordCertificateList.Add(new FiscalRecordCertificateViewModel
                    {
                        ApprovedDateTime = row.ApprovedDateTime,
                        PdfFile = row.PdfFile,
                        XmlFile = row.XmlFile,
                        FiscalRecordCertificateId = row.FiscalRecordCertificateId
                    });
                }
            }

            #endregion CerficateFile

            fiscalRecordViewModel.CancelReasons = fiscalRecord.CancelReasonCatalogList.ToViewModel();

            return fiscalRecordViewModel;
        }

        /// <summary>
        /// Converts to viewmodelpaymentreceipt.
        /// </summary>
        /// <param name="fiscalRecord">The fiscal record.</param>
        /// <param name="paymentComplement">The payment complement.</param>
        /// <param name="paymentMappings">The payment mappings.</param>
        /// <param name="cfdiUsageList">The cfdi usage list.</param>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        internal static PaymentReceiptViewModel ToViewModelPaymentReceipt(this FiscalRecord fiscalRecord, PaymentComplement paymentComplement,
            List<ReceiptPaymentMethodMapping> paymentMappings, List<CFDIUsageCatalog> cfdiUsageList, Account account)
        {
            string zero = decimal.Zero.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            CFDIUsageCatalog cfdiUsage = cfdiUsageList.Find(c => c.Code == Constants.PaymentReceiptCFDIUsage);
            PaymentReceiptViewModel paymentReceiptViewModel = new PaymentReceiptViewModel
            {
                Amount = paymentComplement.AmountPaid.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                AmountPaid = paymentComplement.AmountPaid,
                CFDIRelated = fiscalRecord.CFDIRelated,
                FiscalRecordStatusEnum = fiscalRecord.RequestState,
                folio = fiscalRecord.Folio,
                InstallmentNumber = paymentComplement.InstallmentNumber + 1,
                IssSerial = fiscalRecord.Issuer.IssSerial,
                IssuerCorporateName = fiscalRecord.Issuer.IssCorporateName,
                issuerIssTaxpayerId = fiscalRecord.Issuer.IssTaxpayerId,
                IssuerTaxRegimen = fiscalRecord.Issuer.TaxRegimenCode + FiscalRecordModelResource.Dash + fiscalRecord.Issuer.TaxRegimenDesc,
                OutstandingBalanceAmount = paymentComplement.OutstandingBalanceAmount,
                // SAT: 12 hours because we do not have the exact time
                PaymentDate = paymentComplement.PaymentDate.AddHours(12).ToString("yyyy-MM-ddTHH:mm:ss"),
                paymentMethodDesc = fiscalRecord.PaymentMethod + FiscalRecordModelResource.Dash + fiscalRecord.PaymentMethodDesc,
                paymentTypeDesc = fiscalRecord.PaymentTypeDesc,
                PaymentTypeList = fiscalRecord.PaymentTypeList.Where(p => p.Code != Constants.PPDPaymentMethod).ToList(),
                PeopleOrgCodeId = fiscalRecord.PeopleOrgCodeId,
                PreviousBalanceAmount = paymentComplement.PreviousBalanceAmount,
                receiverCorporateName = fiscalRecord.Receiver.CorporateName,
                receiverEmail = fiscalRecord.Receiver.Email,
                receiverFiscalIdentityNumber = fiscalRecord.Receiver.FiscalIdentityNumber,
                receiverFiscalResidency = fiscalRecord.Receiver.FiscalResidency,
                receiverFiscalResidencyDesc = string.IsNullOrEmpty(fiscalRecord.Receiver.FiscalResidency) ? string.Empty : fiscalRecord.Receiver.FiscalResidency + FiscalRecordModelResource.Dash + fiscalRecord.Receiver.FiscalResidencyDesc,
                receiverTaxPayerId = fiscalRecord.Receiver.TaxPayerId,
                ReceiverTaxRegimen = fiscalRecord.Receiver.TaxRegimenCode + FiscalRecordModelResource.Dash + fiscalRecord.Receiver.TaxRegimenDesc,
                RecTaxAddress = fiscalRecord.Receiver.PostalCode,
                serialNumber = fiscalRecord.SerialNumber,
                SubjectToTax = fiscalRecord.FiscalRecordDetailList[0].SubjectToTax,
                uuid = fiscalRecord.UUID,

                #region Fixed values

                CFDIUsageCode = cfdiUsage.Code,
                CFDIUsageDesc = cfdiUsage.Description,
                comments = string.Empty,
                currency = Constants.PaymentReceiptCurrency,
                CurrencyComplement = fiscalRecord.Currency,
                Detail = new ChargeCreditViewModel
                {
                    ChargeCreditCode = paymentComplement.ChargeCreditCode,
                    ChargeCreditDesc = paymentComplement.ChargeCreditDesc,
                    ProductServiceDesc = paymentComplement.ProductServiceDesc,
                    ProductServiceKey = paymentComplement.ProductServiceKey,
                    Quantity = 1,
                    SubjectToTax = paymentComplement.SubjectToTax,
                    TotalTaxes = paymentComplement.TotalTaxes.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                    TotalUnit = paymentComplement.UnitAmount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                    UnitAmount = paymentComplement.UnitAmount.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture),
                    UnityKey = paymentComplement.UnityKey,
                    UnityName = paymentComplement.UnityName
                },
                fiscalRecordType = Constants.FiscalRecordTypePago,
                paymentCondition = string.Empty,
                paymentMethod = string.Empty,
                paymentType = string.Empty,
                subtotal = zero,
                total = zero,
                totalTransferTaxed = zero,
                Version = Constants.CurrentCfdiVersion,
                VoucherType = Constants.FiscalRecordTypePagoDesc,

                #endregion Fixed values
            };

            if (fiscalRecord.Issuer?.IssIssuingAdd?.Count > 0)
            {
                paymentReceiptViewModel.issIssuingAddIssIssuingAddress = fiscalRecord.Issuer.IssIssuingAdd[0].IssIssuingAddress;
                paymentReceiptViewModel.issIssuingAddIssPlaceIssue = fiscalRecord.Issuer.IssIssuingAdd[0].IssPostalCode;
                paymentReceiptViewModel.InvoiceExpeditionId = fiscalRecord.Issuer.IssIssuingAdd[0].IssInoviceExpeditionId;
            }

            if (fiscalRecord.ExpeditionDateTime.HasValue)
            {
                string time = fiscalRecord.ExpeditionDateTime.Value.ToShortTimeString();
                string date = fiscalRecord.ExpeditionDateTime.Value.ToString(account.reportFormats.DateFormat);
                paymentReceiptViewModel.expeditionDateTime = $"{date} {time}";
            }

            if (paymentMappings.Count > 0)
                paymentReceiptViewModel.PaymentTypeComplement = paymentMappings.FirstOrDefault(m => m.ChargeCreditCodeId == paymentComplement.ReceiptChargeCreditCodeId)?.PaymentMethodCode.ToString();
            paymentReceiptViewModel.PaymentTypeComplement = paymentReceiptViewModel.PaymentTypeComplement ?? string.Empty;

            return paymentReceiptViewModel;
        }

        /// <summary>
        /// Gets the month description.
        /// </summary>
        /// <param name="monthCode">The month code.</param>
        /// <returns></returns>
        private static string GetMonthDescription(string monthCode)
        {
            switch (monthCode)
            {
                case "01":
                    return "Enero";

                case "02":
                    return "Febrero";

                case "03":
                    return "Marzo";

                case "04":
                    return "Abril";

                case "05":
                    return "Mayo";

                case "06":
                    return "Junio";

                case "07":
                    return "Julio";

                case "08":
                    return "Agosto";

                case "09":
                    return "Septiembre";

                case "10":
                    return "Octubre";

                case "11":
                    return "Noviembre";

                case "12":
                    return "Diciembre";

                default:
                    return "";
            }
        }
    }
}