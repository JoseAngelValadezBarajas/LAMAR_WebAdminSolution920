// --------------------------------------------------------------------
// <copyright file="PowerCampusAccess.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using Hedtech.PowerCampus.Logger;
using Newtonsoft.Json;
using PowerCampus.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebAdminUI.Helpers
{
    /// <summary>
    /// PowerCampusAccess Class
    /// </summary>
    public class PowerCampusAccess
    {
        /// <summary>
        /// Deletes the token client.
        /// </summary>
        /// <param name="Account">The Account model.</param>
        /// <param name="apiController">The API controller.</param>
        /// <returns></returns>
        public static async Task<bool> DeleteTokenClient(Account Account, string apiController)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            string json = JsonConvert.SerializeObject(Account);
            Uri uri = new Uri(client.BaseAddress + apiController);
            HttpResponseMessage response = await client.DeleteAsync(uri + "/" + Account.GUID);
            if (response.IsSuccessStatusCode)
                return true;
            LoggerHelper.LogWebError("FiscalRecords", "PowerCampusAccess", "Account was not deleted: " + Account.GUID + " " + DateTime.Now + " - " + response.ToString());

            return false;
        }

        /// <summary>
        /// Deletes the token client electronic degree.
        /// </summary>
        /// <param name="Account">The account.</param>
        /// <param name="apiController">The API controller.</param>
        /// <returns></returns>
        public static async Task<bool> DeleteTokenClientElectronicDegree(Account Account, string apiController)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            string json = JsonConvert.SerializeObject(Account);
            Uri uri = new Uri(client.BaseAddress + apiController);
            HttpResponseMessage response = await client.DeleteAsync(uri + "/" + Account.GUID);
            if (response.IsSuccessStatusCode)
                return true;
            LoggerHelper.LogWebError("ElectronicDegree", "PowerCampusAccess", "Account was not deleted: " + Account.GUID + " " + DateTime.Now + " - " + response.ToString());
            return false;
        }

        /// <summary>
        /// Gets the token client.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="apiController">The API controller.</param>
        /// <returns></returns>
        public static async Task<Token> GetTokenClient(string id, string apiController)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(null);
            Dictionary<string, string> values = new Dictionary<string, string>
                {
                    {"username", id},
                    {"grant_type", "password"}
                };
            FormUrlEncodedContent body = new FormUrlEncodedContent(values);
            Uri uri = new Uri(client.BaseAddress + apiController);
            HttpResponseMessage response = await client.PostAsync(uri, body);
            Token Token = new Token();
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Token = JsonConvert.DeserializeObject<Token>(content);
            }
            return Token;
        }

        /// <summary>
        /// Determines whether the specified unique identifier is authorized.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static async Task<Account> IsAuthorized(string id)
        {
            Account Account = new Account();
            HttpClient client = PowerCampusHttpClient.GetClient();
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "/api/accounts/" + id);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Account = JsonConvert.DeserializeObject<Account>(content);
                if (Account.status == enumAccess.Authorized)
                {
                    Token Token = await GetTokenClient(id, "/api/token");
                    if (Token != null)
                    {
                        Account.Token = Token.access_token;
                        Account.TokenType = Token.token_type;
                        Account.reportFormats = await GetReportFormats(Account);
                        Account.EmailRegularExpression = await GetEmailRegularExpression(Account);
                        Account.InstitutionIdFormat = await GetInstitutionIdFormat(Account);
                        Account.permissions = await GetFiscalRecordsPermissions(Account, "/api/accounts/GetPermissions");

                        bool wasDeleted = await DeleteTokenClient(Account, "/api/accounts");
                        if (Account != null && wasDeleted)
                        {
                            HttpContext context = HttpContext.Current;
                            context.Session["Account"] = Account;
                            LoggerHelper.LogWebError("FiscalRecords", "PowerCampusAccess", "Account Created: " + Account.UserName + " " + DateTime.Now);
                            return Account;
                        }
                        Account.status = enumAccess.Unauthorized;

                        return Account;
                    }
                    Account.status = enumAccess.Error;

                    return Account;
                }

                return Account;
            }
            Account.status = enumAccess.Unauthorized;

            return Account;
        }

        /// <summary>
        /// Determines whether [is authorized electronic certificate] [the specified identifier].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static async Task<Account> IsAuthorizedElectronicCertificate(string id)
        {
            Account Account = new Account();
            HttpClient client = PowerCampusHttpClient.GetClient();
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + ApiRoute.EcAccountGetAccountByGuid + id);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Account = JsonConvert.DeserializeObject<Account>(content);
                if (Account.status == enumAccess.Authorized)
                {
                    Token Token = await GetTokenClient(id, "/api/token/electronicCertificate");
                    if (Token != null)
                    {
                        Account.Token = Token.access_token;
                        Account.TokenType = Token.token_type;
                        Account.reportFormats = await GetReportFormats(Account);
                        Account.EmailRegularExpression = await GetEmailRegularExpression(Account);
                        Account.InstitutionIdFormat = await GetInstitutionIdFormat(Account);
                        // Permissions for fiscal records, electronic degree and electronig certificate are in the same end point
                        Account.permissions = await GetElectronicDegreePermissions(Account, "/api/accountsed/GetPermissions");

                        bool wasDeleted = await DeleteTokenClientElectronicDegree(Account, ApiRoute.EcAccountDelete);
                        if (Account != null && wasDeleted)
                        {
                            HttpContext context = HttpContext.Current;
                            context.Session["Account"] = Account;
                            LoggerHelper.LogWebError("ElectronicCertificate", "PowerCampusAccess", "Account Created: " + Account.UserName + " " + DateTime.Now);
                            return Account;
                        }
                        Account.status = enumAccess.Unauthorized;

                        return Account;
                    }
                    Account.status = enumAccess.Error;

                    return Account;
                }

                return Account;
            }
            Account.status = enumAccess.Unauthorized;

            return Account;
        }

        /// <summary>
        /// Determines whether [is authorized electronic degree] [the specified identifier].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static async Task<Account> IsAuthorizedElectronicDegree(string id)
        {
            Account Account = new Account();
            HttpClient client = PowerCampusHttpClient.GetClient();
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "/api/accountsed/" + id);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Account = JsonConvert.DeserializeObject<Account>(content);
                if (Account.status == enumAccess.Authorized)
                {
                    Token Token = await GetTokenClient(id, "/api/token/electronicDegree");
                    if (Token != null)
                    {
                        Account.Token = Token.access_token;
                        Account.TokenType = Token.token_type;
                        Account.reportFormats = await GetReportFormats(Account);
                        Account.EmailRegularExpression = await GetEmailRegularExpression(Account);
                        Account.InstitutionIdFormat = await GetInstitutionIdFormat(Account);
                        Account.permissions = await GetElectronicDegreePermissions(Account, "/api/accountsed/GetPermissions");

                        bool wasDeleted = await DeleteTokenClientElectronicDegree(Account, "/api/accountsed");
                        if (Account != null && wasDeleted)
                        {
                            HttpContext context = HttpContext.Current;
                            context.Session["Account"] = Account;
                            LoggerHelper.LogWebError("ElectronicDegree", "PowerCampusAccess", "Account Created: " + Account.UserName + " " + DateTime.Now);
                            return Account;
                        }
                        Account.status = enumAccess.Unauthorized;

                        return Account;
                    }
                    Account.status = enumAccess.Error;

                    return Account;
                }

                return Account;
            }
            Account.status = enumAccess.Unauthorized;

            return Account;
        }

        /// <summary>
        /// Updates the token client.
        /// </summary>
        /// <param name="Account">The Account model.</param>
        /// <param name="apiController">The API controller.</param>
        /// <returns></returns>
        public static async Task<Account> UpdateTokenClient(Account Account, string apiController)
        {
            HttpClient client = PowerCampusHttpClient.GetClient(Account);
            string json = JsonConvert.SerializeObject(Account);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            Uri uri = new Uri(client.BaseAddress + apiController);
            HttpResponseMessage response = await client.PutAsync(uri, httpContent);
            if (!response.IsSuccessStatusCode)
                return null;

            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Account>(content);
        }

        /// <summary>
        /// Gets the electronic degree permissions.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="apiController">The API controller.</param>
        /// <returns></returns>
        private static async Task<List<Permissions>> GetElectronicDegreePermissions(Account account, string apiController)
        {
            Account accountPermissions = new Account();
            HttpClient client = PowerCampusHttpClient.GetClient(account);
            Uri uri = new Uri(client.BaseAddress + apiController);
            string userName = account.UserName;
            HttpResponseMessage response = await client.GetAsync(uri + "/" + userName);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                accountPermissions = JsonConvert.DeserializeObject<Account>(content);
            }

            return accountPermissions.permissions;
        }

        private static async Task<string> GetEmailRegularExpression(Account account)
        {
            string RegularExpression = string.Empty;
            HttpClient client = PowerCampusHttpClient.GetClient(account);
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "/api/settings?area=SYSADMIN&section=EMAIL_ADDRESS &label=REGULAR_EXPRESSION");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                RegularExpression = JsonConvert.DeserializeObject<string>(content);
            }

            return RegularExpression;
        }

        /// <summary>
        /// Gets the fiscal records permissions.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="apiController">The API controller.</param>
        /// <returns></returns>
        private static async Task<List<Permissions>> GetFiscalRecordsPermissions(Account account, string apiController)
        {
            Account accountPermissions = new Account();
            HttpClient client = PowerCampusHttpClient.GetClient(account);
            Uri uri = new Uri(client.BaseAddress + apiController);
            HttpResponseMessage response = await client.GetAsync(uri + "/" + account.UserName);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                accountPermissions = JsonConvert.DeserializeObject<Account>(content);
            }

            return accountPermissions.permissions;
        }

        /// <summary>
        /// Gets the institution identifier format.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        private static async Task<string> GetInstitutionIdFormat(Account account)
        {
            string idFormat = string.Empty;
            HttpClient client = PowerCampusHttpClient.GetClient(account);
            HttpResponseMessage response =
                await client.GetAsync(client.BaseAddress + "/api/settings?Area=SYSADMIN&Section=ID_FORMAT&Label=INSTITUTION");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                idFormat = JsonConvert.DeserializeObject<string>(content);
            }
            return idFormat;
        }

        /// <summary>
        /// Gets the system formats.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        private static async Task<ReportFormat> GetReportFormats(Account account)
        {
            /*
             * DATE_FORMAT
             * CURRENCY_FORMAT
             * TIME_FORMAT
            */
            ReportFormat reportFormats = new ReportFormat();
            HttpClient client = PowerCampusHttpClient.GetClient(account);
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "/api/settings?area=SYSADMIN&section=DATE_FORMAT&label=REPORT_FORMAT");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                string dateformat = JsonConvert.DeserializeObject<string>(content);
                reportFormats.DateFormat = dateformat.Replace("D", "d").Replace("m", "M").Replace("Y", "y");
            }

            response = await client.GetAsync(client.BaseAddress + "/api/settings?area=SYSADMIN&section=CURRENCY_FORMAT&label=REPORT_FORMAT");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                string currencyFormat = JsonConvert.DeserializeObject<string>(content);
                reportFormats.CurrencyFormat = currencyFormat;
            }

            response = await client.GetAsync(client.BaseAddress + "/api/settings?area=SYSADMIN&section=TIME_FORMAT&label=REPORT_FORMAT");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                string timeFormat = JsonConvert.DeserializeObject<string>(content);
                reportFormats.TimeFormat = timeFormat.Replace("M", "m").Replace("S", "s").Replace("am/pm", "tt").Replace("AM/PM", "tt").Replace("a/p", "t").Replace("A/P", "t");
                if (reportFormats.TimeFormat.Contains("H") && reportFormats.TimeFormat.Contains("tt"))
                    reportFormats.TimeFormat = reportFormats.TimeFormat.Replace("H", "h");
                else if (reportFormats.TimeFormat.Contains("h") && !reportFormats.TimeFormat.Contains("tt"))
                    reportFormats.TimeFormat = reportFormats.TimeFormat.Replace("h", "H");
            }

            return reportFormats;
        }
    }
}