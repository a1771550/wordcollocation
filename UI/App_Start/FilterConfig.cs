using System.Web.Mvc;
using UI.Classes;

namespace UI
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new ElmahHandleErrorAttribute());
		}
	}
}
