using System.Web.Http.Filters;
using System.Web.Mvc;

namespace UI.Classes
{
	public class ElmahHTTPErrorAttribute : ExceptionFilterAttribute
	{

		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			if (actionExecutedContext.Exception != null)
			{
				Elmah.ErrorSignal.FromCurrentContext().Raise(actionExecutedContext.Exception);
			}
			base.OnException(actionExecutedContext);
		}
	}

	public class ElmahMVCErrorAttribute : System.Web.Mvc.IExceptionFilter
	{

		public void OnException(ExceptionContext filterContext)
		{
			if (filterContext.Exception != null)
			{
				Elmah.ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);
			}
		}
	}
}