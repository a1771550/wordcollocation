using System.Web.Optimization;

namespace UI
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/layout").Include("~/js/jquery-{version}.js").Include("~/js/bootstrap.js").Include("~/js/tinynav.js").Include("~/js/template.js").Include("~/js/socialLinks.js").Include("~/js/jquery-cookie-plugin.js").Include("~/js/timezone.js").Include("~/js/jquery-ui.custom/jquery-ui.js").Include("~/js/searchbox_submit.js"));

			//bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/js/modernizr.js"));

			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/js/modernizr.js"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"));

			//bundles.UseCdn = true;
			//const string cdn = "http://fonts.googleapis.com/css?family=Poiret+One|PT+Serif|Open+Sans:400,300";
			//@import url("/css/wisewords/magic-bootstrap.css");
			//@import url("/css/wisewords/bootstrap.min.css");
			//@import url("/css/wisewords/bootstrap-responsive.min.css");
			//@import url("/css/wisewords/socialicons.css");
			//@import url("/css/wisewords/glyphicons.css");
			//@import url("/css/wisewords/halflings.css");
			//@import url("/css/wisewords/template.css");
			//@import url("/css/wisewords/colors/color-classic.css");
			bundles.Add(new StyleBundle("~/bundles/css").Include("~/css/wisewords/magic-bootstrap.css").Include("~/css/wisewords/bootstrap.css").Include("~/css/wisewords/bootstrap-responsive.css").Include("~/css/wisewords/socialicons.css").Include("~/css/wisewords/glyphicons.css").Include("~/css/wisewords/halflings.css").Include("~/css/wisewords/template.css").Include("~/css/wisewords/colors/color-classic.css").Include("~/css/site.css").Include("~/css/adminLinks.css").Include("~/js/jquery-ui.custom/jquery-ui.css").Include("~/js/jquery-ui.custom/jquery-ui.structure.css").Include("~/js/jquery-ui.custom/jquery-ui.theme.css").Include("~/css/progress.css"));

			// Set EnableOptimizations to false for debugging. For more information,
			// visit http://go.microsoft.com/fwlink/?LinkId=301862
			BundleTable.EnableOptimizations = true;
		}
	}
}
