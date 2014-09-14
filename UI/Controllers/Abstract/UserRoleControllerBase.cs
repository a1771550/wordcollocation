using System.Linq;
using System.Web.Mvc;

namespace UI.Controllers.Abstract
{
	public abstract class UserRoleControllerBase : CommonControllerBase
	{
		/// <summary>
		/// Read the timezone offset value from cookie and store in session.
		/// </summary>
		/// <param name="filterContext"></param>
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (HttpContext.Request.Cookies.AllKeys.Contains("timezoneoffset"))
			{
				var httpCookie = HttpContext.Request.Cookies["timezoneoffset"];
				if (httpCookie != null)
					Session["timezoneoffset"] = httpCookie.Value;
			}
			base.OnActionExecuting(filterContext);
		}
	}
}