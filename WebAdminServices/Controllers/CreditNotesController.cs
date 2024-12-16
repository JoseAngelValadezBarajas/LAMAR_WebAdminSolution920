// --------------------------------------------------------------------
// <copyright file="CreditNotesController.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using PowerCampus.Business;
using PowerCampus.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WebAdminServices.Filters;

namespace WebAdminServices.Controllers
{
    /// <summary>
    /// CreditNotesController class
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [LoggingActionFilterAttribute]
    public class CreditNotesController : ApiController
    {
        private readonly CreditNotesServices creditNotesServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditNotesController"/> class.
        /// </summary>
        public CreditNotesController()
        {
            creditNotesServices = new CreditNotesServices();
        }

        /// <summary>
        /// Calculates the tax totals by charge.
        /// </summary>
        /// <param name="fiscalRecordDetail">The fiscal record detail.</param>
        /// <returns></returns>
        [Route("api/CreditNotes/CalculateTaxTotalsByCharge")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult CalculateTaxTotalsByCharge(FiscalRecordDetail fiscalRecordDetail)
        {
            return Ok(DeductAmountsByCharge(fiscalRecordDetail));
        }

        /// <summary>
        /// Calculates the totals.
        /// </summary>
        /// <param name="fiscalRecordDetails">The fiscal record details.</param>
        /// <returns></returns>
        [Route("api/CreditNotes/CalculateTotals")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult CalculateTotals(List<FiscalRecordDetail> fiscalRecordDetails)
        {
            int invoiceHeaderId = 0;
            decimal? subTotal = 0.0m;
            decimal? totalTaxes = 0.0m;
            string PeopleCodeId = fiscalRecordDetails[0].PeopleOrgCodeId;

            if (fiscalRecordDetails != null)
            {
                invoiceHeaderId = fiscalRecordDetails[0].InvoiceHeaderId;
                ClaimsPrincipal claim = User as ClaimsPrincipal;
                string userName = claim.FindFirst("userName").Value;
                FiscalRecord currentCreditNote = null;

                if (fiscalRecordDetails[0].IsAGlobalCreditNote)
                    currentCreditNote = creditNotesServices.GetFiscalRecord(invoiceHeaderId, userName, PeopleCodeId);
                else
                    currentCreditNote = creditNotesServices.GetFiscalRecord(invoiceHeaderId, userName, null);

                if (currentCreditNote != null)
                {
                    if (currentCreditNote.IsAPPDTaxCreditNote) //PPD Credit Note
                    {
                        foreach (ChargeCredit item in currentCreditNote.InvoiceDetails)
                        {
                            foreach (FiscalRecordDetail creditNoteDetail in fiscalRecordDetails)
                            {
                                if (creditNoteDetail.ChargeCreditNumber == item.ChargeCreditCodeId)
                                {
                                    //Get percentage of refund
                                    decimal refundPercentage;
                                    if (item.UnitAmount > 0)
                                    {
                                        refundPercentage = (creditNoteDetail.Amount * 100) / item.UnitAmount;

                                        List<ChargeCredit> taxes = currentCreditNote.InvoiceDetails
                                            .Where(x => x.ChargeNumberSource == creditNoteDetail.ChargeCreditNumber && x.IsATax).ToList();

                                        foreach (ChargeCredit itemTax in taxes)
                                        {
                                            totalTaxes += decimal.Round((decimal)(refundPercentage * itemTax.TotalTaxes) / 100, 2);
                                        }
                                        subTotal += creditNoteDetail.Amount;
                                    }
                                }
                            }
                        }
                    }
                    else if (currentCreditNote.IsAGlobalCreditNote) //Global Credit Note
                    {
                        foreach (ChargeCredit item in currentCreditNote.InvoiceDetails)
                        {
                            foreach (FiscalRecordDetail creditNoteDetail in fiscalRecordDetails)
                            {
                                if (creditNoteDetail.ChargeCreditNumber.Equals(item.ChargeCreditCodeId) && creditNoteDetail.IsATax.Equals(0) && creditNoteDetail.ReceiptNumber.Equals(item.ReceiptNumber))
                                {
                                    //Get percentage of refund
                                    decimal refundPercentage;
                                    if (item.UnitAmount > 0)
                                    {
                                        refundPercentage = (creditNoteDetail.Amount * 100) / item.UnitAmount;

                                        List<ChargeCredit> taxes = currentCreditNote.InvoiceDetails
                                            .Where(x => x.ChargeNumberSource == creditNoteDetail.ChargeCreditNumber
                                            && x.ReceiptNumber.Equals(creditNoteDetail.ReceiptNumber)
                                            && x.IsATax).ToList();

                                        foreach (ChargeCredit itemTax in taxes)
                                        {
                                            totalTaxes += decimal.Round((decimal)(refundPercentage * itemTax.TotalTaxes) / 100, 2);
                                        }
                                        subTotal += creditNoteDetail.Amount;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (ChargeCredit item in currentCreditNote.InvoiceDetails)
                        {
                            foreach (FiscalRecordDetail creditNoteDetail in fiscalRecordDetails)
                            {
                                if (creditNoteDetail.ChargeCreditNumber == item.ChargeCreditCodeId && creditNoteDetail.IsATax == 0)
                                {
                                    //Get percentage of refund
                                    decimal refundPercentage;
                                    if (item.UnitAmount > 0)
                                    {
                                        refundPercentage = (creditNoteDetail.Amount * 100) / item.UnitAmount;

                                        List<ChargeCredit> taxes = currentCreditNote.InvoiceDetails
                                            .Where(x => x.ChargeNumberSource == creditNoteDetail.ChargeCreditNumber
                                            && x.IsATax).ToList();

                                        foreach (ChargeCredit itemTax in taxes)
                                        {
                                            totalTaxes += decimal.Round((decimal)(refundPercentage * itemTax.TotalTaxes) / 100, 2);
                                        }
                                        subTotal += creditNoteDetail.Amount;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            var fiscalRecord = new FiscalRecord
            {
                InvoiceHeaderId = invoiceHeaderId,
                SubTotal = (decimal)subTotal,
                TotalTransferTaxes = (decimal)totalTaxes,
                Total = decimal.Round((decimal)subTotal, 2) + decimal.Round((decimal)totalTaxes, 2)
            };
            return Ok(fiscalRecord);
        }

        /// <summary>
        /// Creates the fiscal record.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <returns></returns>
        [Route("api/CreditNote/CreateCreditNote")]
        [Authorize]
        [HttpPost]
        public IHttpActionResult CreateCreditNote(CreateFiscalRecord Model)
        {
            decimal? totalTaxes = 0;
            //Before Creating Credit Note, Calculate Taxes Amount for all details
            //Get All InvoiceDetails (included taxes) and deduct taxes for each amount.
            ClaimsPrincipal claim = User as ClaimsPrincipal;
            string userName = claim.FindFirst("userName").Value;
            FiscalRecord currentCreditNote = null;
            bool IsAGlobalCreditNote = Model.IsAGlobalCreditNote;
            bool IsAPPDTaxCreditNote = Model.IsAPPDTaxCreditNote;

            if (IsAGlobalCreditNote)  //Create credit note from global fiscal record
                currentCreditNote = creditNotesServices.GetFiscalRecord(Model.InvoiceHeaderId, userName, Model.PeopleOrgCodeId);
            else
                currentCreditNote = creditNotesServices.GetFiscalRecord(Model.InvoiceHeaderId, userName, null);

            if (Model.Detail.Count <= currentCreditNote.InvoiceDetails.Select(m => !m.IsATax).Count())
            {
                var creditNoteCalculatedList = new List<FiscalRecordDetail>();

                decimal refundPercentage = 0;
                foreach (FiscalRecordDetail creditNoteDetail in Model.Detail)
                {
                    if (IsAPPDTaxCreditNote)
                    {
                        foreach (ChargeCredit fiscalRecordDetail in currentCreditNote.InvoiceDetails)
                        {
                            if (creditNoteDetail.ChargeCreditNumber == fiscalRecordDetail.ChargeNumberSource && !fiscalRecordDetail.IsATax && creditNoteDetail.Amount > 0.0m && fiscalRecordDetail.UnitAmount > 0)
                            {
                                refundPercentage = (creditNoteDetail.Amount * 100) / fiscalRecordDetail.UnitAmount;

                                //Search for the taxes in the fiscalRecordDetails
                                List<ChargeCredit> taxes = currentCreditNote.InvoiceDetails
                                                        .Where(x => x.ChargeNumberSource == creditNoteDetail.ChargeCreditNumber && x.IsATax).ToList();
                                foreach (ChargeCredit itemTax in taxes)
                                {
                                    totalTaxes = (refundPercentage * itemTax.TotalTaxes) / 100;
                                    creditNoteCalculatedList.Add(new FiscalRecordDetail
                                    {
                                        InvoiceDetailId = itemTax.InvoiceDetailId,
                                        InvoiceHeaderId = Model.InvoiceHeaderId,
                                        IsATax = 1,
                                        Description = itemTax.ChargeCreditDesc,
                                        UnitDescription = itemTax.UnityName,
                                        Amount = decimal.Round((decimal)(totalTaxes), 2)
                                    });
                                }
                            }
                        }
                    }
                    else if (IsAGlobalCreditNote)
                    {
                        foreach (ChargeCredit fiscalRecordDetail in currentCreditNote.InvoiceDetails)
                        {
                            if (creditNoteDetail.ChargeCreditNumber.Equals(fiscalRecordDetail.ChargeNumberSource)
                                && creditNoteDetail.ReceiptNumber.Equals(fiscalRecordDetail.ReceiptNumber)
                                && creditNoteDetail.IsATax.Equals(0) && creditNoteDetail.Amount > 0.0m
                                && fiscalRecordDetail.UnitAmount > 0)
                            {
                                refundPercentage = (creditNoteDetail.Amount * 100) / fiscalRecordDetail.UnitAmount;

                                //Search for the taxes in the fiscalRecordDetails
                                List<ChargeCredit> taxes = currentCreditNote.InvoiceDetails
                                                        .Where(x => x.ChargeNumberSource == creditNoteDetail.ChargeCreditNumber
                                                        && x.ReceiptNumber.Equals(creditNoteDetail.ReceiptNumber)
                                                        && x.IsATax).ToList();
                                foreach (ChargeCredit itemTax in taxes)
                                {
                                    totalTaxes = (refundPercentage * itemTax.TotalTaxes) / 100;
                                    creditNoteCalculatedList.Add(new FiscalRecordDetail
                                    {
                                        InvoiceDetailId = itemTax.InvoiceDetailId,
                                        InvoiceHeaderId = Model.InvoiceHeaderId,
                                        IsATax = 1,
                                        Description = itemTax.ChargeCreditDesc,
                                        UnitDescription = itemTax.UnityName,
                                        Amount = decimal.Round((decimal)(totalTaxes), 2)
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (ChargeCredit fiscalRecordDetail in currentCreditNote.InvoiceDetails)
                        {
                            if (creditNoteDetail.ChargeCreditNumber == fiscalRecordDetail.ChargeNumberSource && creditNoteDetail.IsATax == 0 && creditNoteDetail.Amount > 0.0m && fiscalRecordDetail.UnitAmount > 0)
                            {
                                refundPercentage = (creditNoteDetail.Amount * 100) / fiscalRecordDetail.UnitAmount;

                                //Search for the taxes in the fiscalRecordDetails
                                List<ChargeCredit> taxes = currentCreditNote.InvoiceDetails
                                                        .Where(x => x.ChargeNumberSource == creditNoteDetail.ChargeCreditNumber
                                                        && x.IsATax).ToList();
                                foreach (ChargeCredit itemTax in taxes)
                                {
                                    totalTaxes = (refundPercentage * itemTax.TotalTaxes) / 100;
                                    creditNoteCalculatedList.Add(new FiscalRecordDetail
                                    {
                                        InvoiceDetailId = itemTax.InvoiceDetailId,
                                        InvoiceHeaderId = Model.InvoiceHeaderId,
                                        IsATax = 1,
                                        Description = itemTax.ChargeCreditDesc,
                                        UnitDescription = itemTax.UnityName,
                                        Amount = decimal.Round((decimal)(totalTaxes), 2)
                                    });
                                }
                            }
                        }
                    }
                }

                Model.TotalTransferTaxes = decimal.Round(creditNoteCalculatedList.Sum(m => m.Amount), 2).ToString();
                foreach (FiscalRecordDetail item in creditNoteCalculatedList)
                    Model.Detail.Add(item);

                int invoiceHeaderId = creditNotesServices.CreateCreditNote(Model); //Create credit note
                if (invoiceHeaderId > 0)
                    return Ok(invoiceHeaderId);
                return NotFound();
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - TaxProfileDA - CreateCreditNote", "Invalid number of Credit Note Details");
                return InternalServerError();
            }
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// /// <param name="peopleOrgcodeId">The identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/creditnotes/Get")]
        public IHttpActionResult Get(int Id, string peopleOrgcodeId)
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal claim = User as ClaimsPrincipal;
                string userName = claim.FindFirst("userName").Value;
                FiscalRecord fiscalRecordResult = creditNotesServices.GetFiscalRecord(Id, userName, peopleOrgcodeId);
                return Ok(fiscalRecordResult);
            }
            else
                return Unauthorized();
        }

        /// <summary>
        /// Gets for substitution.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="peopleOrgcodeId">The people orgcode identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/CreditNotes/ForSubstitution/{id}/{peopleOrgcodeId}")]
        public IHttpActionResult GetForSubstitution(int id, string peopleOrgcodeId)
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal claim = User as ClaimsPrincipal;
                string userName = claim.FindFirst("userName").Value;
                // TO DO: Review why when the people code id is not empty, flag IsAGlobalCreditNote returns True
                FiscalRecord fiscalRecordResult = creditNotesServices.GetForSubstitution(id, userName, string.Empty);
                return Ok(fiscalRecordResult);
            }
            else
                return Unauthorized();
        }

        /// <summary>
        /// Deducts the amounts by charge.
        /// </summary>
        /// <param name="fiscalRecordDetail">The fiscal record detail.</param>
        /// <returns></returns>
        private FiscalRecord DeductAmountsByCharge(FiscalRecordDetail fiscalRecordDetail)
        {
            decimal? totalDeductedTaxes = 0.0m;

            if (fiscalRecordDetail != null)
            {
                ClaimsPrincipal claim = User as ClaimsPrincipal;
                string userName = claim.FindFirst("userName").Value;
                FiscalRecord currentCreditNote = creditNotesServices.GetFiscalRecord(fiscalRecordDetail.InvoiceHeaderId, userName, fiscalRecordDetail.PeopleOrgCodeId);

                if (currentCreditNote != null)
                {
                    if (fiscalRecordDetail.IsAPPDTaxCreditNote)
                    {
                        foreach (ChargeCredit item in currentCreditNote.InvoiceDetails)
                        {
                            if (fiscalRecordDetail.ChargeCreditNumber == item.ChargeCreditCodeId)
                            {
                                //Get percentage of refund
                                decimal refundPercentage;
                                if (item.UnitAmount > 0)
                                {
                                    refundPercentage = (fiscalRecordDetail.Amount * 100) / item.UnitAmount;
                                    List<ChargeCredit> taxes = currentCreditNote.InvoiceDetails
                                        .Where(x => x.ChargeNumberSource == fiscalRecordDetail.ChargeCreditNumber && x.IsATax).ToList();

                                    foreach (ChargeCredit itemTax in taxes)
                                        totalDeductedTaxes += (refundPercentage * itemTax.TotalTaxes) / 100;
                                }
                            }
                        }
                    }
                    else if (fiscalRecordDetail.IsAGlobalCreditNote) //Global Credit Note
                    {
                        foreach (ChargeCredit item in currentCreditNote.InvoiceDetails)
                        {
                            if (fiscalRecordDetail.ChargeCreditNumber.Equals(item.ChargeCreditCodeId) && fiscalRecordDetail.ReceiptNumber.Equals(item.ReceiptNumber))
                            {
                                //Get percentage of refund
                                decimal refundPercentage;
                                if (item.UnitAmount > 0)
                                {
                                    refundPercentage = (fiscalRecordDetail.Amount * 100) / item.UnitAmount;
                                    List<ChargeCredit> taxes = currentCreditNote.InvoiceDetails
                                        .Where(x => x.ChargeNumberSource == fiscalRecordDetail.ChargeCreditNumber
                                        && x.ReceiptNumber.Equals(fiscalRecordDetail.ReceiptNumber)
                                        && x.IsATax).ToList();

                                    foreach (ChargeCredit itemTax in taxes)
                                        totalDeductedTaxes += (refundPercentage * itemTax.TotalTaxes) / 100;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (ChargeCredit item in currentCreditNote.InvoiceDetails)
                        {
                            if (fiscalRecordDetail.ChargeCreditNumber == item.ChargeCreditCodeId)
                            {
                                //Get percentage of refund
                                decimal refundPercentage;
                                if (item.UnitAmount > 0)
                                {
                                    refundPercentage = (fiscalRecordDetail.Amount * 100) / item.UnitAmount;
                                    List<ChargeCredit> taxes = currentCreditNote.InvoiceDetails
                                        .Where(x => x.ChargeNumberSource == fiscalRecordDetail.ChargeCreditNumber
                                        && x.IsATax).ToList();

                                    foreach (ChargeCredit itemTax in taxes)
                                        totalDeductedTaxes += (refundPercentage * itemTax.TotalTaxes) / 100;
                                }
                            }
                        }
                    }
                }
            }
            var fiscalRecord = new FiscalRecord
            {
                InvoiceHeaderId = fiscalRecordDetail.InvoiceHeaderId,
                SubTotal = fiscalRecordDetail.Amount,
                TotalTransferTaxes = decimal.Round((decimal)totalDeductedTaxes, 2),
                Total = (decimal.Round(fiscalRecordDetail.Amount, 2)) + decimal.Round((decimal)totalDeductedTaxes, 2)
            };

            return fiscalRecord;
        }
    }
}