using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
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
		private List<WcUser> Users;
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
			WcUser user = FindByEmail(model.UserNameEmail);
			if (user == null)
			{
				// login uses username
				user = FindByName(model.UserNameEmail);
				if (user == null) return false;
				Session["UserName"] = model.UserNameEmail;
				CookieHelper.SetCookie(UserCookie, model.UserNameEmail, DateTime.Now.AddDays(1));
			}

			if (CheckPassword(user))
			{
				FormsAuthentication.SetAuthCookie(model.UserNameEmail, model.RememberMe);
				// login uses email as username
				string name = repo.GetList().Single(x => x.Email == model.UserNameEmail).Name;
				Session["UserName"] = name;
				CookieHelper.SetCookie(UserCookie, name, DateTime.Now.AddDays(1));
				GreetingsHelper.SetGreetings(name, GreetingsCookie);
				return true;
			}

			return false;
		}


		private bool CheckPassword(WcUser user)
		{
			var pwd = TextHelper.Encrypt(user.Password, true);
			bool pass= Users.Any(x => x.Name == user.Name && x.Password == pwd);
			if (!pass)
				pass = Users.Any(x => x.Name == user.Name && x.Password == user.Password);
			return pass;
		}

		private WcUser FindByName(string name)
		{
			//List<WcUser> users = (List<WcUser>) Session["UserRepo"];
			return Users.SingleOrDefault(x => x.Name == name);
		}

		private WcUser FindByEmail(string email)
		{
			//List<WcUser> users;
			//Session["UserRepo"] = users =  repo.GetUserRolesList();
			Users = repo.GetUserRolesList();
			return Users.SingleOrDefault(x => x.Email == email);
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

		/// <summary>
		/// specially for elmah use only
		/// </summary>
		public void elmah()
		{
			Response.Redirect("~/elmah");
		}
	}
}