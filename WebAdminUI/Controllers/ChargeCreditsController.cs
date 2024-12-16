// --------------------------------------------------------------------
// <copyright file="ChargeCreditsController.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using PowerCampus.Entities;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.CashReceiptsMappers;
using WebAdminUI.ChargeCreditsMappers;
using WebAdminUI.Filter;
using WebAdminUI.Helpers;
using WebAdminUI.Models.FiscalRecords;

namespace WebAdminUI.Controllers
{
    /// <summary>
    /// ChargeCreditsController
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecordsProccesses)]
    public class ChargeCreditsController : BaseController
    {
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Create(int? id)
        {
            bool isSubstitution = id.HasValue && id > 0;

            PPDCreateViewModel PPDCreateViewModel;
            HttpClient client = PowerCampusHttpClient.GetClient(Account);

            if (isSubstitution)
            {
                HttpResponseMessage resp = await client.GetAsync($"{client.BaseAddress}/api/fiscalrecords/forsubstitution/{id.Value}");
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    CashReceipt CRModel = await resp.Content.ReadAsAsync<CashReceipt>();
                    if (!(CRModel.chargesToInvoiceList?.Count > 0))
                        return RedirectToAction("Index", "Home");
                    PPDCreateViewModel = CRModel.ToPPDViewModel(Account);
                }
                else if (resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoggerHelper.LogWebError("FiscalRecords", nameof(ChargeCreditsController), resp.ReasonPhrase);
                    return RedirectToRoute("ErrorExpired");
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", nameof(ChargeCreditsController), resp.ReasonPhrase);
                    return RedirectToRoute("ErrorException");
                }
            }
            else
            {
                ChargeCredit chargeCredit;
                IssuerDefault issuerDefault;
                InvoiceOrganization invoiceOrganization = new InvoiceOrganization();
                People peopleModel;

                #region GetChargeCredit

                HttpResponseMessage respChargeCredit = await client.GetAsync(client.BaseAddress + "/api/chargecredits/Get/?chargeCreditNumber=" + Account.ChargeCreditNumber);
                if (respChargeCredit.StatusCode.Equals(HttpStatusCode.OK))
                {
                    chargeCredit = await respChargeCredit.Content.ReadAsAsync<ChargeCredit>();
                    chargeCredit.ChargeNumberSource = Account.ChargeCreditNumber;
                    chargeCredit.PeopleOrgCodeId = Account.SearchPeopleOrgCodeId;
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditController", respChargeCredit.ReasonPhrase);
                    return RedirectToRoute("ErrorExpired");
                }

                #endregion GetChargeCredit

                #region GetIssuerDefault

                HttpResponseMessage respIssuerDefault = await client.GetAsync(client.BaseAddress + "/api/Issuers/SelectIssuerSetUp");
                if (respIssuerDefault.StatusCode.Equals(HttpStatusCode.OK))
                {
                    issuerDefault = await respIssuerDefault.Content.ReadAsAsync<IssuerDefault>();
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditController", respIssuerDefault.ReasonPhrase);
                    return RedirectToRoute("ErrorExpired");
                }

                #endregion GetIssuerDefault

                #region GetReceiverDefault

                if (Account.PeopleOrgCodeId.StartsWith("P"))
                {
                    HttpResponseMessage respReceiverPeopleDefault = await client.GetAsync(client.BaseAddress + "/api/People/Get/P" + Account.SearchPeopleOrgCodeId);

                    if (respReceiverPeopleDefault.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        peopleModel = await respReceiverPeopleDefault.Content.ReadAsAsync<People>();
                        peopleModel.PeopleCodeId = "P" + Account.SearchPeopleOrgCodeId;
                    }
                    else
                    {
                        LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditController", respReceiverPeopleDefault.ReasonPhrase);
                        return RedirectToRoute("ErrorExpired");
                    }
                }
                else
                {
                    HttpResponseMessage respReceiverOrgDefault = await client.GetAsync(client.BaseAddress + "/api/Organization/" + "O" + Account.SearchPeopleOrgCodeId);

                    if (respReceiverOrgDefault.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        peopleModel = await respReceiverOrgDefault.Content.ReadAsAsync<People>();
                        peopleModel.PeopleCodeId = "O" + Account.SearchPeopleOrgCodeId;
                    }
                    else
                    {
                        LoggerHelper.LogWebError("FiscalRecords", "ChargeCreditController", respReceiverOrgDefault.ReasonPhrase);
                        return RedirectToRoute("ErrorExpired");
                    }
                }

                #endregion GetReceiverDefault

                PPDCreateViewModel = chargeCredit.ToViewModel(peopleModel, issuerDefault, invoiceOrganization, Account);
            }

            return View("PPDView", PPDCreateViewModel);
        }

        /// <summary>
        /// Gets the charge credit number.
        /// </summary>
        /// <param name="ChargeCreditNumber">The charge credit number.</param>
        /// <returns></returns>
        public EmptyResult GetChargeCreditNumber(int ChargeCreditNumber)
        {
            if (Account != null)
                Account.ChargeCreditNumber = ChargeCreditNumber;

            return null;
        }
    }
}