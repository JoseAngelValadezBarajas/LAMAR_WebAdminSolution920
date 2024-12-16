// --------------------------------------------------------------------
// <copyright file="ReceiversMapper.cs" company="Ellucian">
//     Copyright 2018 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using WebAdminUI.Models.Receivers;

namespace WebAdminUI.ReceiversMappers
{
    /// <summary>
    /// Mapper for receivers view models
    /// </summary>
    internal static class ReceiversMapper
    {
        /// <summary>
        /// To the receiver entity.
        /// </summary>
        /// <param name="receiverViewModel">The receiver view model.</param>
        /// <returns></returns>
        internal static Receiver ToDataEntity(this ReceiverViewModel receiverViewModel)
        {
            Receiver receiver = new Receiver
            {
                CorporateName = receiverViewModel.CorporateName,
                FiscalIdentityNumber = receiverViewModel.FiscalIdentityNumber,
                FiscalResidency = receiverViewModel.FiscalResidency,
                FiscalResidencyDesc = receiverViewModel.FiscalResidencyDesc,
                InvoiceForeignTaxpayerId = receiverViewModel.InvoiceForeignTaxpayerId,
                InvoiceTaxpayerId = receiverViewModel.InvoiceTaxpayerId,
                TaxPayerId = receiverViewModel.TaxPayerId,
                TaxRegimenId = receiverViewModel.TaxRegimenId,
                PostalCode = receiverViewModel.PostalCode,
                StreetName = receiverViewModel.StreetName,
                StreetNumber = receiverViewModel.StreetNumber,
                ApartmentNumber = receiverViewModel.ApartmentNumber,
                Neighborhood = receiverViewModel.Neighborhood,
                Location = receiverViewModel.Location,
                StateProvinceId = receiverViewModel.StateProvinceId,
                CountryId = receiverViewModel.CountryId,
            };
            return receiver;
        }

        /// <summary>
        /// Receivers to receiver view.
        /// </summary>
        /// <param name="receiver">The receiver.</param>
        /// <returns></returns>
        internal static ReceiverViewModel ToViewModel(this Receiver receiver)
        {
            var receiverViewModel = new ReceiverViewModel
            {
                CorporateName = receiver.CorporateName,
                FiscalIdentityNumber = receiver.FiscalIdentityNumber,
                FiscalResidency = receiver.FiscalResidency,
                FiscalResidencyDesc = receiver.FiscalResidencyDesc,
                HasInvoice = receiver.HasInvoice,
                InvoiceForeignTaxpayerId = receiver.InvoiceForeignTaxpayerId,
                InvoiceTaxpayerId = receiver.InvoiceTaxpayerId,
                PostalCode = receiver.PostalCode,
                TaxPayerId = receiver.TaxPayerId,
                TaxRegimenCode = receiver.TaxRegimenCode,
                TaxRegimenDesc = receiver.TaxRegimenDesc,
                TaxRegimenId = receiver.TaxRegimenId,
                StreetName = receiver.StreetName,
                StreetNumber = receiver.StreetNumber,
                ApartmentNumber = receiver.ApartmentNumber,
                Neighborhood = receiver.Neighborhood,
                Location = receiver.Location,
                StateProvinceId = receiver.StateProvinceId,
                CountryId = receiver.CountryId,
            };
            return receiverViewModel;
        }
    }
}