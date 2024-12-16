// --------------------------------------------------------------------
// <copyright file="CurrencyExtensions.cs" company="Ellucian">
//     Copyright 2018 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using WebAdminUI.Helpers;

namespace WebAdminUI.HtmlHelpers
{
    /// <summary>
    /// CurrencyExtensions
    /// </summary>
    public static class CurrencyExtensions
    {
        private static readonly Regex _regexCurrency = new Regex(@"value=\""[\d\D.,]*\""");
        private static readonly Regex _regexDate = new Regex(@"value=\""[\d\D-:\w]*\""");

        /// <summary>
        /// CurrencyFor
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="format"></param>
        /// <param name="type"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString CurrencyFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, string format, string type = null, object htmlAttributes = null)
        {
            decimal value = (decimal)(ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model);
            string valueWithFormat = string.Empty;

            switch (format)
            {
                case "report":
                    valueWithFormat = value.ToReportCurrency();
                    break;

                default:
                    valueWithFormat = value.ToString();
                    break;
            }

            if (type == null)
                return new MvcHtmlString(valueWithFormat);

            MvcHtmlString result = htmlHelper.TextBoxFor(expression, htmlAttributes);

            string resultStr = result.ToString();
            Match match = _regexCurrency.Match(resultStr);

            return new MvcHtmlString(resultStr.Replace(match.Value, $"value=\"{valueWithFormat}\""));
        }

        /// <summary>
        /// DateFor
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="format"></param>
        /// <param name="type"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString DateFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, string format, string type = null, object htmlAttributes = null)
        {
            var value = (DateTime)(ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model);
            var valueWithFormat = string.Empty;

            switch (format)
            {
                case "report":
                    valueWithFormat = value.ToReportDate();
                    break;

                default:
                    valueWithFormat = value.ToString();
                    break;
            }

            if (type == null)
                return new MvcHtmlString(valueWithFormat);

            var result = htmlHelper.TextBoxFor(expression, htmlAttributes);

            var resultStr = result.ToString();
            var match = Regex.Match(resultStr, @"value=\""[\d\D-:\w]*\""");

            return new MvcHtmlString(resultStr.Replace(match.Value, "value=\"" + valueWithFormat + "\""));
        }

        /// <summary>
        /// DateTimeFor
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="format"></param>
        /// <param name="type"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString DateTimeFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, string format, string type = null, object htmlAttributes = null)
        {
            DateTime value = (DateTime)(ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model);
            string valueWithFormat = string.Empty;

            switch (format)
            {
                case "report":
                    valueWithFormat = value.ToReportDateTime();
                    break;

                default:
                    valueWithFormat = value.ToString();
                    break;
            }

            if (type == null)
                return new MvcHtmlString(valueWithFormat);

            MvcHtmlString result = htmlHelper.TextBoxFor(expression, htmlAttributes);

            string resultStr = result.ToString();
            Match match = _regexDate.Match(resultStr);

            return new MvcHtmlString(resultStr.Replace(match.Value, $"value=\"{valueWithFormat}\""));
        }

        /// <summary>
        /// TimeFor
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="format"></param>
        /// <param name="type"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString TimeFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, string format, string type = null, object htmlAttributes = null)
        {
            DateTime value = (DateTime)(ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model);
            string valueWithFormat = string.Empty;

            switch (format)
            {
                case "report":
                    valueWithFormat = value.ToReportTime();
                    break;

                default:
                    valueWithFormat = value.ToString();
                    break;
            }

            if (type == null)
                return new MvcHtmlString(valueWithFormat);

            MvcHtmlString result = htmlHelper.TextBoxFor(expression, htmlAttributes);

            string resultStr = result.ToString();
            Match match = _regexDate.Match(resultStr);

            return new MvcHtmlString(resultStr.Replace(match.Value, $"value=\"{valueWithFormat}\""));
        }
    }
}