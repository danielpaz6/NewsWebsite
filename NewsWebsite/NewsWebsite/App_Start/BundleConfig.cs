using System.Web;
using System.Web.Optimization;

namespace NewsWebsite
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                        "~/Scripts/script.js"));

            bundles.Add(new ScriptBundle("~/bundles/d3").Include(
                        "~/Scripts/d3.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css",
                      "~/Content/style.css")); // was site.css

            bundles.Add(new StyleBundle("~/Content/css2").Include(
                      //"~/Content/bootstrap.css",
                      "~/Content/app.css")); // was site.css

            bundles.Add(new StyleBundle("~/Content/css3").Include(
                      "~/Content/style3.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      //"~/Content/bootstrap.css",
                      "~/Content/bootstrap.css")); // was site.css
        }
    }
}
