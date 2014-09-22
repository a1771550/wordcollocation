using System.Web.Mvc;
using Elmah;
using UI.Classes;

namespace UI.Controllers
{
	public enum HttpStatusErrorCode
	{
		code404,
		code403,
		UrlErrors,
	}
    public class ErrorController : Controller
    {
		public ActionResult Index()
		{
			return View("Error");
		}

		/// <summary>
		/// 404 error
		/// </summary>
		/// <returns></returns>
		public ActionResult NotFound()
		{
			ViewBag.ErrorCode = HttpStatusErrorCode.code404;
			return View("Error");
		}

		/// <summary>
		/// 403 error
		/// </summary>
		/// <returns></returns>
		public ActionResult AccessDenied()
		{
			ViewBag.ErrorCode = HttpStatusErrorCode.code403;
			return View("Error");
		}

		/// <summary>
		/// Catch all route errors
		/// </summary>
		/// <returns></returns>
	    public ActionResult RouteErrors()
		{
			ViewBag.ErrorCode = HttpStatusErrorCode.UrlErrors;
			return View("Error");
		}

		public void LogJavaScriptError(string message)
		{
			ErrorSignal
				.FromCurrentContext()
				.Raise(new JavaScriptException(message));
		}
    }
}