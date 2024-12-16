// --------------------------------------------------------------------
// <copyright file="PowerCampusSystemFormat.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using PowerCampus.Entities;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace WebAdminUI.Helpers
{
    /// <summary>
    /// PowerCampusSystemFormat Class
    /// </summary>
    public static class PowerCampusSystemFormat
    {
        /// <summary>
        /// Formats the people code identifier.
        /// </summary>
        /// <param name="peopleId">The people identifier.</param>
        /// <returns></returns>
        public static string FormatPeopleCodeId(string peopleId)
        {
            Account account = (Account)HttpContext.Current.Session["Account"];
            string format = string.Empty;
            string peopleIdFormatted = string.Empty;
            if (account != null)
            {
                int count = 0;
                for (int i = 0; i < account.InstitutionIdFormat.Length; i++)
                {
                    if (account.InstitutionIdFormat.Substring(i, 1).Equals("#"))
                    {
                        format += "{" + count.ToString() + "}";
                        count++;
                    }
                    else
                    {
                        char[] character = account.InstitutionIdFormat.Substring(i, 1).ToCharArray();
                        int ascii = Encoding.ASCII.GetBytes(character)[0];

                        format += account.InstitutionIdFormat.Substring(i, 1);
                        if ((ascii > 64 && ascii < 91) || (ascii > 96 && ascii < 123))
                            count++;
                    }
                }
                peopleIdFormatted = string.Format(format, peopleId.Select(x => x.ToString()).ToArray());
            }
            return peopleIdFormatted;
        }

        /// <summary>
        /// Gets the mail validation.
        /// </summary>
        /// <returns></returns>
        public static string GetMailValidation()
        {
            Account account = (Account)HttpContext.Current.Session["Account"];
            if (account != null)
                return account.EmailRegularExpression;
            else
                return string.Empty;
        }

        /// <summary>
        ///  Gets the value with the report format
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToReportCurrency(this decimal value)
        {
            var account = (Account)HttpContext.Current.Session["Account"];
            if (account.reportFormats.CurrencyFormat != string.Empty)
                return value.ToString(account.reportFormats.CurrencyFormat, CultureInfo.InvariantCulture);
            throw new Exception("No currency format found.");
        }

        /// <summary>
        /// Gets the value with the report format
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToReportDate(this DateTime date)
        {
            var account = (Account)HttpContext.Current.Session["Account"];
            if (account.reportFormats.DateFormat != string.Empty)
                return date.ToString(account.reportFormats.DateFormat, CultureInfo.InvariantCulture);
            throw new Exception("No date format found.");
        }

        /// <summary>
        ///  Gets the value with the report format
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToReportDateTime(this DateTime dateTime)
        {
            var account = (Account)HttpContext.Current.Session["Account"];
            if (account.reportFormats.TimeFormat != string.Empty && account.reportFormats.DateFormat != string.Empty)
                return dateTime.ToString(string.Concat(account.reportFormats.DateFormat, " ", account.reportFormats.TimeFormat), CultureInfo.InvariantCulture);
            throw new Exception("No date/time format found.");
        }

        /// <summary>
        ///  Gets the value with the report format
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToReportTime(this DateTime time)
        {
            var account = (Account)HttpContext.Current.Session["Account"];
            if (account.reportFormats.TimeFormat != string.Empty)
                return time.ToString(account.reportFormats.TimeFormat, CultureInfo.InvariantCulture);
            throw new Exception("No time format found.");
        }
    }
}