using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using BLL;

namespace UI
{
	public class MvcApplication : HttpApplication
	{
		private WcUserRepository repo;
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			//FilterConfig.RegisterHttpFilters(GlobalConfiguration.Configuration.Filters); => web api
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			//var httpContext = ((MvcApplication)sender).Context;
			//var currentController = " ";
			//var currentAction = " ";
			//var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

			//if (currentRouteData != null)
			//{
			//	if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
			//	{
			//		currentController = currentRouteData.Values["controller"].ToString();
			//	}

			//	if (currentRouteData.Values["action"] != null && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
			//	{
			//		currentAction = currentRouteData.Values["action"].ToString();
			//	}
			//}

			//var ex = Server.GetLastError();
			//var controller = new ErrorController();
			//var routeData = new RouteData();
			//var action = "Index";

			//if (ex is HttpException)
			//{
			//	var httpEx = ex as HttpException;

			//	switch (httpEx.GetHttpCode())
			//	{
			//		case 404:
			//			action = "NotFound";
			//			break;

			//		case 401:
			//			action = "AccessDenied";
			//			break;
			//	}
			//}

			//httpContext.ClearError();
			//httpContext.Response.Clear();
			//httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
			//httpContext.Response.TrySkipIisCustomErrors = true;

			//routeData.Values["controller"] = "Error";
			//routeData.Values["action"] = action;

			//controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
			//((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
		}


		protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
		{
			if (FormsAuthentication.CookiesSupported)
			{
				if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
				{
					try
					{
						//let us take out the username now                
						var formsAuthenticationTicket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);
						if (formsAuthenticationTicket != null)
						{
							string username = formsAuthenticationTicket.Name;
							repo = new WcUserRepository();
							List<WcUser> Users = repo.GetUserRolesList();

							WcUser user = Users.SingleOrDefault(u => u.Name == username) ?? Users.SingleOrDefault(u => u.Email == username);

							if (user == null) return;
							string role = user.RoleName;

							//let us extract the roles from our own custom cookie


							//Let us set the Pricipal with our user specific details
							HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
								new System.Security.Principal.GenericIdentity(username, "Forms"), role.Split(';'));
						}
					}
					catch (Exception exception)
					{
						//somehting went wrong
						throw new Exception(exception.Message, exception.InnerException);
					}
				}
			}
		}
	}
}
