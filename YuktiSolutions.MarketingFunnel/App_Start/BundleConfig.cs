using System.Web;
using System.Web.Optimization;

namespace YuktiSolutions.MarketingFunnel
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.js", "~/Scripts/WebManager/drawer-menu.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/jquery3").Include(
                       "~/Scripts/jquery-3.3.1.js"));
            bundles.Add(new ScriptBundle("~/bundles/Admin/ajaxjqueryjs").Include(
            "~/Scripts/jquery-1.10.2.min.js",
            "~/Scripts/modernizr-2.8.3.js",
            "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/Admin/kendouijs").Include(
            "~/Scripts/kendo/2015.2.902/kendo.all.min.js",
            "~/Scripts/kendo/2015.2.902/kendo.aspnetmvc.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/Admin/drawermenujs").Include(
            "~/Scripts/WebManager/drawer-menu.js"));

            bundles.Add(new StyleBundle("~/Content/Admin/css").Include(
                     "~/Content/Admin_Theme/bootstrap.css",
                     "~/Content/Admin_Theme/site.css"));

            bundles.Add(new StyleBundle("~/Content/Admin/kendouicss").Include(
                     "~/Content/kendo/2015.2.902/kendo.common.min.css",
                     "~/Content/kendo/2015.2.902/kendo.default.min.css",
                     "~/Content/Admin_Theme/kendocustom.css"));
        }
    }
}
