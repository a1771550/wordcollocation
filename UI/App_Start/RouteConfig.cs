using System.Web.Mvc;
using System.Web.Routing;

namespace UI
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute("UnderConstruction", "{controller}/{action}", new{controller="Home",action="UnderConstruction"} );

			routes.MapRoute("CommonList", "{controller}/{action}/{page}",
				new { controller = "Word", action = "Index", page = UrlParameter.Optional }, new { page = @"\d+" });

			routes.MapRoute("CommonListAdmin", "{controller}/{action}/{page}",
				new { controller = "Word", action = "IndexForAdmin", page = UrlParameter.Optional }, new { page = @"\d+" });

			routes.MapRoute("CommonListEditor", "{controller}/{action}/{page}",
				new {controller = "Word", action = "IndexForEditor", page = UrlParameter.Optional}, new {page = @"\d+"});

			routes.MapRoute("Login", "{controller}/{action}/{id}",
				new { controller = "WcAccount", action = "Login", id = UrlParameter.Optional }, new { id = @"\d+" });

			routes.MapRoute("CollocationEdit", "{controller}/{action}/{id}",
				new {controller = "Collocation", action = "Edit", id = UrlParameter.Optional},new {id=@"\d+"});

			routes.MapRoute("RedirectFromCollocation", "{controller}/{action}/{id}",
				new { controller = "WcExample", action = "Edit", id = UrlParameter.Optional }, new { id = @"\d+" });


			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

			

			routes.MapRoute(
				null,
				"{controller}/{action}/Page{page}",
				new { controller = "Home", action = "SearchResult", page = UrlParameter.Optional },
				new { page = @"\d+" }
			);
		}
	}
}
