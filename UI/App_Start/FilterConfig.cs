using System.Web.Mvc;
using UI.Classes;

namespace UI
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new ElmahHandleErrorAttribute(),1);
			//filters.Add(new ElmahMVCErrorAttribute(),1);
			filters.Add(new HandleErrorAttribute(),2);
		}

		// the following method should belong to web api, so comment it
		//public static void RegisterHttpFilters(System.Web.Http.Filters.HttpFilterCollection filters)
		//{
		//	filters.Add(new ElmahHTTPErrorAttribute());
		//}
	}
}
