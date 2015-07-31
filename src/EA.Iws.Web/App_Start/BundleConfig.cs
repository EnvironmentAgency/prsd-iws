﻿namespace EA.Iws.Web
{
    using System.Web.Optimization;
    using LibSassNet.Web;

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                      "~/Scripts/vendor/jquery-1.11.0.min.js",
                      "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery.select-to-autocomplete.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/custom-validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/govuk_toolkit").Include(
                      "~/Scripts/govuk_toolkit/vendor/polyfills/bind.js",
                      "~/Scripts/govuk_toolkit/govuk/selection-buttons.js"));

            bundles.Add(new ScriptBundle("~/bundles/govuk_iws").Include(
                      "~/Scripts/vendor/modernizr.custom.77028.js",
                      "~/Scripts/vendor/details.polyfill.js",
                      "~/Scripts/application.js",
                      "~/Scripts/business-type-radio-buttons.js"));

            bundles.Add(new ScriptBundle("~/bundles/prism").Include(
                      "~/Scripts/vendor/prism.js"));

            bundles.Add(new SassBundle("~/Content/iws-page-ie6", "~/Content/govuk_toolkit/stylesheets").Include(
                      "~/Content/iws-page-ie6.scss"));

            bundles.Add(new SassBundle("~/Content/iws-page-ie7", "~/Content/govuk_toolkit/stylesheets").Include(
                      "~/Content/iws-page-ie7.scss"));

            bundles.Add(new SassBundle("~/Content/iws-page-ie8", "~/Content/govuk_toolkit/stylesheets").Include(
                      "~/Content/iws-page-ie8.scss"));

            bundles.Add(new SassBundle("~/Content/iws-page", "~/Content/govuk_toolkit/stylesheets").Include(
                      "~/Content/iws-page.scss"));

            bundles.Add(new SassBundle("~/Content/prism", "~/Content/govuk_toolkit/stylesheets").Include(
                      "~/Content/prism.scss"));

            bundles.Add(new StyleBundle("~/Content/css/font-awesome")
                .Include("~/Content/css/font-awesome.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}