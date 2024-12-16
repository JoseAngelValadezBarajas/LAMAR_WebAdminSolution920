// --------------------------------------------------------------------
// <copyright file="IssuersController.cs" company="Ellucian">
//     Copyright 2017 - 2022 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAdminUI.Filter;
using WebAdminUI.FiscalRecordsMappers;
using WebAdminUI.Helpers;
using WebAdminUI.Models.Issuers;

namespace WebAdminUI.Controllers
{
    /// <summary>
    /// IssuersController Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [PermissionRequired(PermissionName = PermissionsConstants.FiscalRecords)]
    [PermissionRequired(PermissionName = PermissionsConstants.IssuerFiscalInformation)]
    public class IssuersController : BaseController
    {
        /// <summary>
        /// Create view for a new expedition or issuer address
        /// </summary>
        /// <param name="id"> Invoice organization id</param>
        /// <returns></returns>
        public ActionResult AddIssuerAddress(int? id)
        {
            InvoiceExpeditionViewModel invoiceExpeditionViewModel = new InvoiceExpeditionViewModel();
            invoiceExpeditionViewModel.InvoiceOrganizationId = id;
            invoiceExpeditionViewModel.InvoiceExpeditionId = null;
            return View(invoiceExpeditionViewModel);
        }

        /// <summary>
        /// Create expedition or issuer address
        /// </summary>
        /// <param name="Model">The invoice expedition view model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> AddIssuerAddress(string Model)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);

            InvoiceExpeditionViewModel invoiceExpeditionViewModel = JsonConvert.DeserializeObject<InvoiceExpeditionViewModel>(Model);

            InvoiceExpedition invoiceExpedition;
            invoiceExpedition = invoiceExpeditionViewModel.ToDataEntity();

            string json = JsonConvert.SerializeObject(invoiceExpedition);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Issuers/Address/", httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                int InvoiceExpeditionId = await resp.Content.ReadAsAsync<int>();
                return (int)invoiceExpedition.InvoiceOrganizationId;
            }
            return -1;
        }

        /// <summary>
        /// Adds the issuer document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="Option">The option.</param>
        /// <returns></returns>
        public async Task<ActionResult> AddIssuerDocument(int? id, string Option)
        {
            InvoiceReceiptViewModel invoiceReceiptViewModel = new InvoiceReceiptViewModel();
            int OptionBy;

            if (Option.Equals(DoucumnetSetpType.ByExpedition.ToString()))
            {
                OptionBy = 0;

                IEnumerable<InvoiceExpedition> IEtypeInvoiceExpedition = await
                      GetListOfClassOfTypeT<InvoiceExpedition>(Account, "api/Issuers/Address/", id);
                List<InvoiceExpedition> invoiceExpeditions = IEtypeInvoiceExpedition == null ? null : IEtypeInvoiceExpedition.ToList<InvoiceExpedition>();
                List<InvoiceExpeditionViewModel> invoiceExpeditionsViewModel = new List<InvoiceExpeditionViewModel>();
                if (invoiceExpeditions != null)
                {
                    foreach (var invoiceExpedition in invoiceExpeditions)
                    {
                        InvoiceExpeditionViewModel invoiceExpeditionViewModel;
                        invoiceExpeditionViewModel = invoiceExpedition.ToViewModel();
                        invoiceExpeditionsViewModel.Add(invoiceExpeditionViewModel);
                    }
                }
                invoiceReceiptViewModel.ListInvoiceExpedition = invoiceExpeditionsViewModel;
                invoiceReceiptViewModel.InvoiceOrganizationId = id;
                invoiceReceiptViewModel.OptionBy = OptionBy;
            }
            else if (Option.Equals(DoucumnetSetpType.ByTaxPayerId.ToString()))
            {
                OptionBy = 1;
                invoiceReceiptViewModel.InvoiceOrganizationId = id;
                invoiceReceiptViewModel.InvoiceExpeditionId = null;
                invoiceReceiptViewModel.OptionBy = OptionBy;
            }
            else
            {
                return RedirectToAction(enumAccess.Error.ToString(), "Home");
            }

            return Json(invoiceReceiptViewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create Document for issuer
        /// </summary>
        /// <param name="invoiceReceiptViewModel">The invoice receipt view model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> AddIssuerDocument(string invoiceReceiptViewModel)
        {
            InvoiceReceiptViewModel InvoiceModel = JsonConvert.DeserializeObject<InvoiceReceiptViewModel>(invoiceReceiptViewModel);

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            InvoiceReceipt invoiceReceipt;
            invoiceReceipt = InvoiceModel.ToDataEntity();
            string json = JsonConvert.SerializeObject(invoiceReceipt);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Issuers/Receipt/", httpContent);
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                return 0;
            }

            int InvoiceExpeditionId = await resp.Content.ReadAsAsync<int>();

            switch (InvoiceExpeditionId)
            {
                case -1:
                    InvoiceModel.InvoiceOrganizationId = -1;
                    break;

                case -2:
                    InvoiceModel.InvoiceOrganizationId = -2;
                    break;

                case -3:
                    InvoiceModel.InvoiceOrganizationId = -3;
                    break;

                default:
                    InvoiceModel.InvoiceOrganizationId = InvoiceExpeditionId;
                    break;
            }

            return (int)InvoiceModel.InvoiceOrganizationId;
        }

        /// <summary>
        /// Create Issuer View Populated with taxregimen
        /// </summary>
        /// <returns></returns>
        // GET: Issuers/Create
        public async Task<ActionResult> Create()
        {
            IssuerViewModel issuerViewModel = new IssuerViewModel();
            issuerViewModel.InvoiceOrganization = new InvoiceOrganizationViewModel();

            issuerViewModel.InvoiceOrganization.ListTaxRegimenCatalogs = await GetTaxRegimenCatalogs(13);
            return View(issuerViewModel);
        }

        /// <summary>
        /// Create New issuer
        /// </summary>
        /// <param name="issuerViewModel"></param>
        /// <returns></returns>
        // POST: Issuers/Create
        [HttpPost]
        public async Task<int> Create(IssuerViewModel issuerViewModel)
        {
            if (!ModelState.IsValid)
            {
                issuerViewModel.InvoiceOrganization.ListTaxRegimenCatalogs = await GetTaxRegimenCatalogs(13);
                return 0;
            }

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            string json = JsonConvert.SerializeObject(issuerViewModel.InvoiceOrganization);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Issuers/", httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                int invoiceOrganizationId = await resp.Content.ReadAsAsync<int>();
                issuerViewModel.InvoiceOrganization.InvoiceOrganizationId = invoiceOrganizationId;
                issuerViewModel.InvoiceExpedition.InvoiceOrganizationId = invoiceOrganizationId;

                json = JsonConvert.SerializeObject(issuerViewModel.InvoiceExpedition);
                httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                resp = await client.PostAsync(client.BaseAddress + "/api/Issuers/Address/", httpContent);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    int InvoiceExpeditionId = await resp.Content.ReadAsAsync<int>();
                }
                return invoiceOrganizationId;
            }
            return 0;
        }

        /// <summary>
        /// Creates the issuer set up.
        /// </summary>
        /// <param name="issuerDefault">The issuer default.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int?> CreateIssuerSetUp(IssuerDefault issuerDefault)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            string json = JsonConvert.SerializeObject(issuerDefault);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Issuers/CreateIssuerSetUp/", httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                int InvoiceExpeditionId = await resp.Content.ReadAsAsync<int>();
                return InvoiceExpeditionId;
            }
            return 0;
        }

        /// <summary>
        /// Delete view for a expedition or issuer address
        /// </summary>
        /// <param name="id"> Invoice organization id</param>
        /// <param name="pkid"> Invoice Expidition id</param>
        /// <param name="taxpayerId"> taxpayer id of Invoice organization id</param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteIssuerAddress(int? id, int? pkid, string taxpayerId)
        {
            InvoiceExpedition invoiceExpedition;
            IEnumerable<InvoiceExpedition> IEtypeInvoiceExpedition = await
                   GetListOfClassOfTypeT<InvoiceExpedition>(Account, "api/Issuers/Address/", id);
            List<InvoiceExpedition> invoiceExpeditions = IEtypeInvoiceExpedition == null ? null : IEtypeInvoiceExpedition.ToList<InvoiceExpedition>();
            invoiceExpedition = invoiceExpeditions.Single(add => add.InvoiceExpeditionId == pkid);
            InvoiceExpeditionViewModel invoiceExpeditionViewModel = new InvoiceExpeditionViewModel();
            invoiceExpeditionViewModel = invoiceExpedition.ToViewModel();
            return View(invoiceExpeditionViewModel);
        }

        /// <summary>
        /// Delete Issuer Address
        /// </summary>
        /// <param name="pkid"> Invoice expidition address id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> DeleteIssuerAddressId(int pkid)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.DeleteAsync(client.BaseAddress + "/api/Issuers/Edit/DeleteIssuerAddressId/?id=" + pkid);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                //resp.EnsureSuccessStatusCode();
                return 1;
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuersController - DeleteIssuerAddressId", resp.ReasonPhrase);
                return 0;
            }
        }

        /// <summary>
        /// Edit View of Issuer
        /// </summary>
        /// <param name="id"> tax payer Id</param>
        /// <returns></returns>
        // GET: Issuers/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            IssuerViewModel issuerViewModel = new IssuerViewModel();
            issuerViewModel.ReceiptType = DoucumnetSetpType.None;

            #region For getting Invoice Organization based of Tax payer Id

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "api/IssuersList/" + id);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                var Model = await resp.Content.ReadAsAsync<InvoiceOrganization>();
                issuerViewModel.InvoiceOrganization = Model.ToViewModel();

                #endregion For getting Invoice Organization based of Tax payer Id

                #region For getting list of Tax regiam for code table and add for dropdowns for tax regimen

                issuerViewModel.InvoiceOrganization.ListTaxRegimenCatalogs = await GetTaxRegimenCatalogs(issuerViewModel.InvoiceOrganization.TaxpayerId.Length);

                #endregion For getting list of Tax regiam for code table and add for dropdowns for tax regimen

                #region For getting list of address and adding to model

                IEnumerable<InvoiceExpedition> IEtypeInvoiceExpedition = await
                            GetListOfClassOfTypeT<InvoiceExpedition>(Account, "api/Issuers/Address/", issuerViewModel.InvoiceOrganization.InvoiceOrganizationId);
                List<InvoiceExpedition> invoiceExpeditions = IEtypeInvoiceExpedition == null ? null : IEtypeInvoiceExpedition.ToList<InvoiceExpedition>();
                List<InvoiceExpeditionViewModel> invoiceExpeditionsViewModel = new List<InvoiceExpeditionViewModel>();
                if (invoiceExpeditions != null)
                {
                    foreach (var invoiceExpedition in invoiceExpeditions)
                    {
                        InvoiceExpeditionViewModel invoiceExpeditionViewModel = new InvoiceExpeditionViewModel();
                        invoiceExpeditionViewModel = invoiceExpedition.ToViewModel();
                        invoiceExpeditionsViewModel.Add(invoiceExpeditionViewModel);
                    }
                }

                issuerViewModel.InvoiceExpeditions = invoiceExpeditionsViewModel;

                #endregion For getting list of address and adding to model

                #region For getting list of Doucument and adding to model

                IEnumerable<InvoiceReceipt> IEtypeInvoiceReceipt = await
                            GetListOfClassOfTypeT<InvoiceReceipt>(Account, "api/Issuers/Receipt/", issuerViewModel.InvoiceOrganization.InvoiceOrganizationId);
                List<InvoiceReceipt> invoiceReceipts = IEtypeInvoiceReceipt == null ? null : IEtypeInvoiceReceipt.ToList<InvoiceReceipt>();
                List<InvoiceReceiptViewModel> invoiceReceiptsViewModel = new List<InvoiceReceiptViewModel>();
                if (invoiceReceipts != null)
                {
                    foreach (var item in invoiceReceipts)
                    {
                        InvoiceReceiptViewModel invoiceReceiptViewModel = new InvoiceReceiptViewModel();
                        invoiceReceiptViewModel = item.ToViewModel(Account);
                        invoiceReceiptsViewModel.Add(invoiceReceiptViewModel);
                    }
                    issuerViewModel.InvoiceReceipts = invoiceReceiptsViewModel;
                    if (invoiceReceipts.FirstOrDefault().InvoiceExpeditionId != null)
                    {
                        foreach (InvoiceReceiptViewModel item in issuerViewModel.InvoiceReceipts)
                        {
                            if (item.InvoiceExpeditionId != null)
                                item.InvoiceExpeditionDescription = issuerViewModel.InvoiceExpeditions.FirstOrDefault(add => add.InvoiceExpeditionId == item.InvoiceExpeditionId).Description;
                        }
                        issuerViewModel.ReceiptType = DoucumnetSetpType.ByExpedition;
                    }
                    else if (invoiceReceipts.FirstOrDefault().InvoiceReceiptId != null)
                    {
                        issuerViewModel.ReceiptType = DoucumnetSetpType.ByTaxPayerId;
                    }
                    else
                    {
                        issuerViewModel.ReceiptType = DoucumnetSetpType.None;
                    }
                }

                #endregion For getting list of Doucument and adding to model

                return View(issuerViewModel);
            }
            else
            {
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Update Edit of Issuer information
        /// </summary>
        /// <param name="issuerViewModel"></param>
        /// <returns></returns>
        // POST: Issuers/Edit/5
        [HttpPost]
        public async Task<int> Edit(IssuerViewModel issuerViewModel)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            string json = JsonConvert.SerializeObject(issuerViewModel.InvoiceOrganization);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Issuers/", httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                int invoiceOrganizationId = await resp.Content.ReadAsAsync<int>();
                return invoiceOrganizationId;
            }
            return 0;
        }

        /// <summary>
        /// Edit view for a new expedition or issuer address
        /// </summary>
        /// <param name="id"> Invoice Organization id</param>
        /// <param name="pkid"> Invoice Expedition id</param>
        /// <param name="taxpayerId"> Invoice TaxPayer id</param>
        /// <returns></returns>
        public async Task<ActionResult> EditIssuerAddress(int? id, int? pkid, string taxpayerId)
        {
            InvoiceExpedition invoiceExpedition;
            IEnumerable<InvoiceExpedition> IEtypeInvoiceExpedition = await
                      GetListOfClassOfTypeT<InvoiceExpedition>(Account, "api/Issuers/Address/", id);
            List<InvoiceExpedition> invoiceExpeditions = IEtypeInvoiceExpedition == null ? null : IEtypeInvoiceExpedition.ToList<InvoiceExpedition>();
            invoiceExpedition = invoiceExpeditions.Single(add => add.InvoiceExpeditionId == pkid);
            InvoiceExpeditionViewModel invoiceExpeditionViewModel = invoiceExpedition.ToViewModel();
            return View(invoiceExpeditionViewModel);
        }

        /// <summary>
        /// Save Edit expedition or issuer address
        /// </summary>
        /// <param name="Model">The invoice expedition view model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> EditIssuerAddress(string Model)
        {
            InvoiceExpeditionViewModel invoiceExpeditionViewModel = JsonConvert.DeserializeObject<InvoiceExpeditionViewModel>(Model);

            HttpClient client = PowerCampusHttpClient.GetClient(Account);

            InvoiceExpedition invoiceExpedition = invoiceExpeditionViewModel.ToDataEntity();

            string json = JsonConvert.SerializeObject(invoiceExpedition);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Issuers/AddressId", httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                int InvoiceExpeditionId = await resp.Content.ReadAsAsync<int>();
                return (int)invoiceExpeditionViewModel.InvoiceOrganizationId;
            }
            return 0;
        }

        /// <summary>
        /// Edit view for a new expedition or issuer address
        /// </summary>
        /// <param name="id"> Invoice Organization id</param>
        /// <param name="pkid"> Invoice Receipt id</param>
        /// <param name="taxpayerId"> Invoice TaxPayer id</param>
        /// <returns></returns>
        public async Task<ActionResult> EditIssuerDocument(int? id, int? pkid, string taxpayerId)
        {
            InvoiceReceipt invoiceReceipt;
            IEnumerable<InvoiceReceipt> IEtypeInvoiceReceipt = await
                         GetListOfClassOfTypeT<InvoiceReceipt>(Account, "api/Issuers/Receipt/", id);
            List<InvoiceReceipt> invoiceReceipts = IEtypeInvoiceReceipt == null ? null : IEtypeInvoiceReceipt.ToList<InvoiceReceipt>();
            invoiceReceipt = invoiceReceipts.Single(document => document.InvoiceReceiptId == pkid);
            InvoiceReceiptViewModel invoiceReceiptViewModel;
            invoiceReceiptViewModel = invoiceReceipt.ToViewModel(Account);
            return View(invoiceReceiptViewModel);
        }

        /// <summary>
        /// GET: Issuers
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Get(string id)
        {
            List<Issuer> LstReceiverModel;

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/issuers/" + id);
            //resp.EnsureSuccessStatusCode();
            LstReceiverModel = await resp.Content.ReadAsAsync<List<Issuer>>();

            return Json(LstReceiverModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the list of charge credit codes
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetChargeCreditCode()
        {
            List<ChargeCredit> chargeCreditCodes = null;

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Catalogs?name=Credit");
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                //resp.EnsureSuccessStatusCode();
                chargeCreditCodes = await resp.Content.ReadAsAsync<List<ChargeCredit>>();
                return Json(chargeCreditCodes, JsonRequestBehavior.AllowGet);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuersController - GetChargeCreditCode", resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Gets the countries.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetPostalCode(string id)
        {
            List<FiscalRecordCatalog> LstCountryCatalog = new List<FiscalRecordCatalog>();
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/catalogs?name=" + Catalog.PostalCode + "&code=" + id);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                LstCountryCatalog = await resp.Content.ReadAsAsync<List<FiscalRecordCatalog>>(); ;
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuersController - GetPostalCode", resp.ReasonPhrase);
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuersController - GetPostalCode", resp.RequestMessage.ToString());
            }

            return Json(LstCountryCatalog, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the serial number.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="expId"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetSerialNumber(int id, int? expId)
        {
            Issuer LstIssuerModel;

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Issuers/GetSerialNumber/?id=" + id + "&expId=" + expId);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                //resp.EnsureSuccessStatusCode();
                LstIssuerModel = await resp.Content.ReadAsAsync<Issuer>();
                return Json(LstIssuerModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuersController - GetSerialNumber", resp.ReasonPhrase);
                return RedirectToRoute("ErrorException");
            }
        }

        /// <summary>
        /// Gets the taxpayer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> GetTaxpayer(string id)
        {
            List<IssuerViewModel> LstIssuerViewModel;

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            var values = new Dictionary<string, string>
                {
                    {"IssTaxpayerId", id},
                };
            var body = new FormUrlEncodedContent(values);
            HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Issuers/GetIssuer", body);
            //resp.EnsureSuccessStatusCode();
            LstIssuerViewModel = await resp.Content.ReadAsAsync<List<IssuerViewModel>>();

            return Json(LstIssuerViewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the tax regimen.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> GetTaxRegimen(int id)
        {
            Issuer LstIssuerModel;

            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Issuers/GetTaxRegimen/?id=" + id);
            LstIssuerModel = await resp.Content.ReadAsAsync<Issuer>();

            return Json(LstIssuerModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the tax regimen catalog.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ActionResult> GetTaxRegimenCatalog(int? id)
        {
            IEnumerable<TaxRegimenCatalog> iEtypeTaxRegimenCatalog = await
                       GetListOfClassOfTypeT<TaxRegimenCatalog>(Account, "api/catalogs?name=TaxRegimen", null);
            List<TaxRegimenCatalog> taxRegimenCatalogs = null;
            if (id == 13)
                taxRegimenCatalogs = iEtypeTaxRegimenCatalog == null ? null : iEtypeTaxRegimenCatalog.Where(cat => cat.PhysicalPerson == true).ToList<TaxRegimenCatalog>();

            if (id == 12)
                taxRegimenCatalogs = iEtypeTaxRegimenCatalog == null ? null : iEtypeTaxRegimenCatalog.Where(cat => cat.MoralPerson == true).ToList<TaxRegimenCatalog>();

            List<TaxRegimenCatalog> catalog = new List<TaxRegimenCatalog>();
            TaxRegimenCatalog item = new TaxRegimenCatalog
            {
                Code = "",
                Description = Models.Resources.IssuerModelResource.lblSelect
            };
            catalog.Add(item);
            catalog.AddRange(taxRegimenCatalogs);

            return Json(catalog, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<List<TaxRegimenCatalog>> GetTaxRegimenCatalogs(int? id)
        {
            IEnumerable<TaxRegimenCatalog> IEtypeTaxRegimenCatalog = await
                       GetListOfClassOfTypeT<TaxRegimenCatalog>(Account, "api/catalogs?name=TaxRegimen", null);
            List<TaxRegimenCatalog> TaxRegimenCatalogs = null;
            if (id == 13)
                TaxRegimenCatalogs = IEtypeTaxRegimenCatalog == null ? null : IEtypeTaxRegimenCatalog.Where(cat => cat.PhysicalPerson == true).ToList<TaxRegimenCatalog>();

            if (id == 12)
                TaxRegimenCatalogs = IEtypeTaxRegimenCatalog == null ? null : IEtypeTaxRegimenCatalog.Where(cat => cat.MoralPerson == true).ToList<TaxRegimenCatalog>();

            return TaxRegimenCatalogs;
        }

        /// <summary>
        /// Setups this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult IssuerSetup()
        {
            return View();
        }

        /// <summary>
        /// Get:  Issuers List
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> List()
        {
            IEnumerable<InvoiceOrganization> IEtypeInvoiceOrganization = await
                     GetListOfClassOfTypeT<InvoiceOrganization>(Account, "api/IssuersList/", null);
            List<InvoiceOrganization> invoiceOrganizations = IEtypeInvoiceOrganization == null ? null : IEtypeInvoiceOrganization.ToList<InvoiceOrganization>();
            List<IssuerViewModel> LstReceiverModel = new List<IssuerViewModel>();
            foreach (InvoiceOrganization item in invoiceOrganizations)
            {
                InvoiceOrganizationViewModel invoiceOrganizationViewModel = new InvoiceOrganizationViewModel();
                invoiceOrganizationViewModel = item.ToViewModel();
                LstReceiverModel.Add(new IssuerViewModel() { InvoiceOrganization = invoiceOrganizationViewModel });
            }
            return View(LstReceiverModel);
        }

        /// <summary>
        /// Selects the issuer set up.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> SelectIssuerSetUp()
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            IssuerDefault issuerDefault;

            HttpResponseMessage resp = await client.GetAsync(client.BaseAddress + "/api/Issuers/SelectIssuerSetUp/");
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                issuerDefault = await resp.Content.ReadAsAsync<IssuerDefault>();
                return Json(issuerDefault, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        /// <summary>
        /// Updates the issuer set up.
        /// </summary>
        /// <param name="issuerDefault">The issuer view model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int?> UpdateIssuerSetUp(IssuerDefault issuerDefault)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);

            string json = JsonConvert.SerializeObject(issuerDefault);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await client.PostAsync(client.BaseAddress + "/api/Issuers/UpdateIssuerSetUp/", httpContent);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                int InvoiceExpeditionId = await resp.Content.ReadAsAsync<int>();
                return issuerDefault.IssInvoiceOrganizationId;
            }
            return 0;
        }

        /// <summary>
        /// Get List of target Class from call of web api values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Account"></param>
        /// <param name="WebApiLink"></param>
        /// <param name="id"></param>
        /// <param name="method">Verbs</param>
        /// /// <param name="json">Verbs</param>
        /// <returns></returns>
        private async Task<IEnumerable<T>> GetListOfClassOfTypeT<T>(Account Account, string WebApiLink, int? id, string method = "GET", string json = "")
        {
            List<T> ListOfClassOfTypeT;
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            try
            {
                HttpResponseMessage resp = null;
                switch (method)
                {
                    case "DELETE":
                        resp = await client.DeleteAsync(client.BaseAddress + WebApiLink + id);
                        break;

                    case "POST":
                        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                        resp = await client.PostAsync(client.BaseAddress + WebApiLink, httpContent);
                        break;

                    default:
                        resp = await client.GetAsync(client.BaseAddress + "/" + WebApiLink + id);
                        break;
                }

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    //resp.EnsureSuccessStatusCode();
                    ListOfClassOfTypeT = await resp.Content.ReadAsAsync<List<T>>();
                }
                else
                {
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuersController - GetListOfClassOfTypeT", resp.ReasonPhrase);
                    LoggerHelper.LogWebError("FiscalRecords", "PowerCampus.DataAccess - IssuersController - GetListOfClassOfTypeT", resp.RequestMessage.ToString());
                    throw new System.Exception("Error in getting request from WebAPI: " + WebApiLink + id);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
            return ListOfClassOfTypeT;
        }
    }
}