using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BLL;
using THResources;
using UI.Controllers.Abstract;
using UI.Helpers;
using UI.Models;

namespace UI.Controllers
{
	public enum AccountResult
	{
		Succeeded,
		Failed,
	}

	public enum RoleType
	{
		Admin = 1,
		Editors = 2,
		Members = 3,
		Guests = 4
	}

	[Authorize]
	public class WcAccountController : UserRoleControllerBase
	{
		private readonly WcUserRepository repo = new WcUserRepository();
		//
		// GET: /Account/Login
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		// POST
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Login(WcLoginViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				RenderModelErrorList();
				return View(model);
			}

			bool result = PasswordSignIn(model);

			if (result)
			{
				if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") &&
					!returnUrl.StartsWith("/\\"))
				{
					return RedirectToAction(returnUrl);
				}
				return RedirectToAction("Index", "Home");

			}
			ModelState.AddModelError("", Resources.UserNamePasswordNotMatch);
			return View("Login");
		}


		[AllowAnonymous]
		public ViewResult Register()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Register(WcRegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new WcUser
				{
					Name = model.UserName,
					Email = model.Email,
					Password = TextHelper.Encrypt(model.Password,true),
					RoleId = (int)RoleType.Members
				};

				bool[] result = repo.Add(user);

				return result[1] ? View("SuccessfullRegister") : View(model);
			}
			RenderModelErrorList();
			return View(model);
		}

		bool PasswordSignIn(WcLoginViewModel model)
		{
			// login uses email as username
			WcUser user = repo.GetObjectByEmail(model.UserNameEmail);
			if (user == null)
			{
				// login uses username
				user = repo.GetObjectByName(model.UserNameEmail);
				if (user == null) return false;
				if (CheckPassword(user))
				{
					FormsAuthentication.SetAuthCookie(model.UserNameEmail, model.RememberMe);
					//Session["UserName"] = model.UserNameEmail;
					CookieHelper.SetCookie(UserCookie, model.UserNameEmail, DateTime.Now.AddDays(1));
					GreetingsHelper.SetGreetings(model.UserNameEmail, GreetingsCookie);
					return true;
				}
			}

			// login uses email as username
			if (CheckPassword(user))
			{
				FormsAuthentication.SetAuthCookie(model.UserNameEmail, model.RememberMe);
				//string name = repo.GetObjectByEmail(model.UserNameEmail).Name;
				//Session["UserName"] = name;
				CookieHelper.SetCookie(UserCookie, user.Name, DateTime.Now.AddDays(1));
				GreetingsHelper.SetGreetings(user.Name, GreetingsCookie);
				return true;
			}

			return false;
		}

		private bool CheckPassword(WcUser user)
		{
			var pwd = TextHelper.Encrypt(user.Password, true);
			bool pass = repo.ValidateUserByNamePwd(user.Name, pwd);
			if (!pass)
				pass = repo.ValidateUserByNamePwd(user.Name, user.Password);
			return pass;
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult ForgetPassword()
		{
			WcLoginViewModel model = new WcLoginViewModel();
			return View(model);
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult ForgetPassword(string userNameEmail)
		{
			var currentEmail = repo.CheckIfDuplicatedEmail(userNameEmail);
			WcRegisterViewModel model;
			if (currentEmail)
			{
				model = new WcRegisterViewModel();
				Session["UserEmail"] = userNameEmail;
				return View("ResetPassword", model);
			}
			var currentName = repo.CheckIfDuplicatedUserName(userNameEmail);
			if (currentName)
			{
				model = new WcRegisterViewModel();
				Session["UserName"] = userNameEmail;
				return View("ResetPassword", model);
			}
			return View("InvalidAccount");
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult ResetPassword(WcResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				RenderModelErrorList();
				return View(model);
			}
			var pwd = TextHelper.Encrypt(model.Password, true);
			if (Session["UserEmail"] != null){ 
				string email = (string) Session["UserEmail"];
				Session["UserEmail"] = null;
				repo.ResetPasswordByEmail(email, pwd);
			}
			if (Session["UserName"] != null)
			{
				string name = (string) Session["UserName"];
				Session["UserName"] = null;
				repo.ResetPasswordByName(name, pwd);
			}
			
			return View("ResetPasswordComplete");
		}

		public ActionResult LogOff()
		{
			FormsAuthentication.SignOut();
			CookieHelper.DeleteCookie("UserName");
			CookieHelper.DeleteCookie("Greetings");
			Session.Abandon();
			return RedirectToAction("Index", "Home");
		}

		[AllowAnonymous]
		[ChildActionOnly]
		public ActionResult ExternalLoginsList(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
		}

		[AllowAnonymous]
		public ActionResult ExternalLoginCallback(string returnUrl)
		{
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
			if (loginInfo == null)
			{
				return RedirectToAction("Login");
			}

			// Sign in the user with this external login provider if the user already has a login
			var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(returnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
				case SignInStatus.Failure:
				default:
					// If the user does not have an account, then prompt the user to create an account
					ViewBag.ReturnUrl = returnUrl;
					ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
					return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
			}
		}

		//[HttpPost]
		//[AllowAnonymous]
		//[ValidateAntiForgeryToken]
		//public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
		//{
		//	string provider = null;
		//	string providerUserId = null;

		//	if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
		//	{
		//		return RedirectToAction("Manage");
		//	}

		//	if (ModelState.IsValid)
		//	{
		//		// Insert a new user into the database
		//		//using (UsersContext db = new UsersContext())
		//		//{
		//		//	UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
		//		//	// Check if user already exists
		//		//	if (user == null)
		//		//	{
		//		//		// Insert name into the profile table
		//		//		db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
		//		//		db.SaveChanges();

		//		//		OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
		//		//		OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

		//		//		return RedirectToLocal(returnUrl);
		//		//	}
		//		//	else
		//		//	{
		//		//		ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
		//		//	}
		//		//}
		//	}

		//	ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
		//	ViewBag.ReturnUrl = returnUrl;
		//	return View(model);
		//}


		[AllowAnonymous]
		public ActionResult ExternalLoginFailure()
		{
			return View();
		}
		#region Helpers
		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		public enum ManageMessageId
		{
			ChangePasswordSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
		}

		internal class ExternalLoginResult : ActionResult
		{
			public ExternalLoginResult(string provider, string returnUrl)
			{
				Provider = provider;
				ReturnUrl = returnUrl;
			}

			public string Provider { get; private set; }
			public string ReturnUrl { get; private set; }

			public override void ExecuteResult(ControllerContext context)
			{
				OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
			}
		}

		private static string ErrorCodeToString(MembershipCreateStatus createStatus)
		{
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for
			// a full list of status codes.
			switch (createStatus)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return "User name already exists. Please enter a different user name.";

				case MembershipCreateStatus.DuplicateEmail:
					return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";

				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidUserName:
					return "The user name provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.ProviderError:
					return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				case MembershipCreateStatus.UserRejected:
					return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				default:
					return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
			}
		}
		#endregion

		/// <summary>
		/// specially for elmah use only
		/// </summary>
		public void elmah()
		{
			Response.Redirect("~/elmah");
		}
	}
}