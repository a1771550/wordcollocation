using System.Web.Optimization;

namespace UI
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/layout").Include("~/Scripts/jquery-{version}.js").Include("~/Scripts/bootstrap.js").Include("~/Scripts/tinynav.js").Include("~/Scripts/template.js").Include("~/Scripts/socialLinks.js").Include("~/Scripts/jquery-cookie-plugin.js").Include("~/Scripts/timezone.js").Include("~/Scripts/jquery-ui.custom/jquery-ui.js").Include("~/Scripts/suggestion.js").Include("~/Scripts/searchbox_submit.js"));

			//bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr.js"));

			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr.js"));

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
			bundles.Add(new StyleBundle("~/bundles/css").Include("~/Content/wisewords/magic-bootstrap.css").Include("~/Content/wisewords/bootstrap.css").Include("~/Content/wisewords/bootstrap-responsive.css").Include("~/Content/wisewords/socialicons.css").Include("~/Content/wisewords/glyphicons.css").Include("~/Content/wisewords/halflings.css").Include("~/Content/wisewords/template.css").Include("~/Content/wisewords/colors/color-classic.css").Include("~/Content/site.css").Include("~/Content/adminLinks.css").Include("~/Content/jquery-ui.css").Include("~/Content/jquery-ui.structure.css").Include("~/Content/jquery-ui.theme.css").Include("~/Content/suggestion.css").Include("~/Content/progress.css"));

			// Set EnableOptimizations to false for debugging. For more information,
			// visit http://go.microsoft.com/fwlink/?LinkId=301862
			BundleTable.EnableOptimizations = true;
		}
	}
}
