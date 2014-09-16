using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using BLL;

namespace UI
{
	public class MvcApplication : System.Web.HttpApplication
	{
		private WcUserRepository repo;
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
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
