// --------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System.Web.Optimization;

namespace WebAdminUI
{
    /// <summary>
    /// BundleConfig Class
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region General

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/cssED").Include(
                      "~/Content/SiteED.css"));

            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
                        "~/Content/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/General/Layout").Include(
                        "~/Scripts/General/Layout.js"));

            bundles.Add(new ScriptBundle("~/bundles/General/Pagination").Include(
                                    "~/Scripts/General/Pagination.js"));

            #endregion General

            #region Specific(externals)

            bundles.Add(new ScriptBundle("~/bundles/datetime").Include(
                                  "~/Scripts/moment-with-locales.min.js",
                                  "~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new StyleBundle("~/Content/datetime").Include(
                        "~/Content/bootstrap-datetimepicker.css"));

            #endregion Specific(externals)

            #region Fiscal Records

            bundles.Add(new ScriptBundle("~/bundles/FR/Layout").Include(
                        "~/Scripts/FiscalRecords/Layout.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/Pagination").Include(
                        "~/Scripts/FiscalRecords/Pagination.js"));

            bundles.Add(new ScriptBundle("~/bundles/FR/IssuerSetup").Include(
                        "~/Scripts/FiscalRecords/IssuerSetup.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/ViewAll").Include(
                        "~/Scripts/FiscalRecords/ViewAll.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/Edit").Include(
                        "~/Scripts/FiscalRecords/Edit.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/CreditNotes").Include(
                        "~/Scripts/FiscalRecords/CreditNotes.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/TaxProfiles").Include(
                        "~/Scripts/FiscalRecords/TaxProfiles.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/ChargeCredits").Include(
                        "~/Scripts/FiscalRecords/ChargeCredits.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/PaymentReceipts").Include(
                        "~/Scripts/FiscalRecords/PaymentReceipts.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/FiscalRecords").Include(
                       "~/Scripts/FiscalRecords/FiscalRecords.js",
                       "~/Scripts/FiscalRecords/Receiver.js",
                       "~/Scripts/FiscalRecords/ReceiverModal.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/GlobalFiscalRecords").Include(
                       "~/Scripts/FiscalRecords/GlobalFiscalRecords.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/ReceiptPayment").Include(
                       "~/Scripts/FiscalRecords/ReceiptPayment.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/Receiver").Include(
                       "~/Scripts/FiscalRecords/Receiver.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/Organization").Include(
                       "~/Scripts/FiscalRecords/Organization.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/People").Include(
                       "~/Scripts/FiscalRecords/People.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/Search").Include(
                       "~/Scripts/FiscalRecords/Search.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/ChargeCredit").Include(
                       "~/Scripts/FiscalRecords/ChargeCredit.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/Issuer").Include(
                       "~/Scripts/FiscalRecords/Issuer.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/Menu").Include(
                       "~/Scripts/FiscalRecords/Menu.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/CancelGlobal").Include(
                       "~/Scripts/FiscalRecords/CancelGlobal.js"));
            bundles.Add(new ScriptBundle("~/bundles/FR/ReceiversList").Include(
                       "~/Scripts/FiscalRecords/ReceiversList.js"));

            #endregion Fiscal Records

            #region Areas

            #region Electronic Degree

            bundles.Add(new ScriptBundle("~/bundles/ED/Generate").Include(
                        "~/Scripts/ElectronicDegree/Generate.js"));
            bundles.Add(new ScriptBundle("~/bundles/ED/Signers").Include(
                        "~/Scripts/ElectronicDegree/Signers.js"));
            bundles.Add(new ScriptBundle("~/bundles/ED/Majors").Include(
                        "~/Scripts/ElectronicDegree/Majors.js"));
            bundles.Add(new ScriptBundle("~/bundles/ED/Operators").Include(
                        "~/Scripts/ElectronicDegree/Operators.js"));
            bundles.Add(new ScriptBundle("~/bundles/ED/EditOperators").Include(
                       "~/Scripts/ElectronicDegree/EditOperators.js"));
            bundles.Add(new ScriptBundle("~/bundles/ED/SignerInstitution").Include(
                        "~/Scripts/ElectronicDegree/SignerInstitution.js"));
            bundles.Add(new ScriptBundle("~/bundles/ED/FederalEntity").Include(
                        "~/Scripts/ElectronicDegree/FederalEntity.js"));
            bundles.Add(new ScriptBundle("~/bundles/ED/BackgroundStudyType").Include(
                        "~/Scripts/ElectronicDegree/BackgroundStudyType.js"));
            bundles.Add(new ScriptBundle("~/bundles/ED/AuthorizationType").Include(
                        "~/Scripts/ElectronicDegree/AuthorizationType.js"));
            bundles.Add(new ScriptBundle("~/bundles/ED/Institutions").Include(
                        "~/Scripts/ElectronicDegree/Institutions.js"));
            bundles.Add(new ScriptBundle("~/bundles/ED/InstitutionsMapping").Include(
                        "~/Scripts/ElectronicDegree/InstitutionsMapping.js"));
            bundles.Add(new ScriptBundle("~/bundles/ED/Operations").Include(
                        "~/Scripts/ElectronicDegree/Operations.js"));

            #endregion Electronic Degree

            #region Electronic Certificate

            bundles.Add(new ScriptBundle("~/bundles/EC/Generations").Include(
                        "~/Scripts/ElectronicCertificate/Generations.js"));
            bundles.Add(new ScriptBundle("~/bundles/EC/Operations").Include(
                        "~/Scripts/ElectronicCertificate/Operations.js"));
            bundles.Add(new ScriptBundle("~/bundles/EC/CommonOperations").Include(
                        "~/Scripts/ElectronicCertificate/CommonOperations.js"));
            bundles.Add(new ScriptBundle("~/bundles/EC/AcademicPlan").Include(
                        "~/Scripts/ElectronicCertificate/AcademicPlan.js"));
            bundles.Add(new ScriptBundle("~/bundles/EC/Responsibles").Include(
                        "~/Scripts/ElectronicCertificate/Responsibles.js"));
            bundles.Add(new ScriptBundle("~/bundles/EC/InstitutionCampuses").Include(
                       "~/Scripts/ElectronicCertificate/InstitutionCampuses.js"));

            bundles.Add(new ScriptBundle("~/bundles/EC/Courses").Include(
                      "~/Scripts/ElectronicCertificate/Courses.js"));

            bundles.Add(new ScriptBundle("~/bundles/EC/FederalEntity").Include(
                       "~/Scripts/ElectronicCertificate/FederalEntity.js"));
            bundles.Add(new ScriptBundle("~/bundles/EC/Operators").Include(
                       "~/Scripts/ElectronicCertificate/Operators.js"));
            bundles.Add(new ScriptBundle("~/bundles/EC/EditOperators").Include(
                      "~/Scripts/ElectronicCertificate/EditOperators.js"));

            #endregion Electronic Certificate

            #endregion Areas

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}