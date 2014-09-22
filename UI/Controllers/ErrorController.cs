using System.Web.Mvc;
using Elmah;
using UI.Classes;

namespace UI.Controllers
{
    public class ErrorController : Controller
    {
		public ActionResult Index()
		{
			return View("Error");
		}

		public ActionResult NotFound()
		{
			return View("Error");
		}

		public ActionResult AccessDenied()
		{
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