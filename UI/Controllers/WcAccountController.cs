using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BLL;
using BLL.Abstract;
using THResources;
using UI.Controllers.Abstract;
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
	public class WcAccountController : WcControllerBase<WcBase>
	{
		private readonly WcUserRepository repo = new WcUserRepository();
		private List<WcUser> Users;
		//
		// GET: /Account/Login
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			//if (TempData["IsAdmin"] == null) TempData["IsAdmin"] = false;
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		// POST
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				List<ModelError> Errors = new List<ModelError>();
				foreach (var m in ModelState.Values)
				{
					Errors.AddRange(m.Errors);
				}
				ViewBag.ErrorList = Errors;
				return View(model);
			}

			SignInStatus result = PasswordSignIn(model.UserNameEmail);

			switch (result)
			{
				case SignInStatus.Success:
					if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") &&
						!returnUrl.StartsWith("/\\"))
					{
						return Redirect(returnUrl);
					}
					return RedirectToAction("Index", "Home");
				case SignInStatus.Failure:
					ModelState.AddModelError("", Resources.UserNamePasswordNotMatch);
					return View("Login");
			}
			return null;
		}

		SignInStatus PasswordSignIn(string userEmail)
		{
			//login uses email as username
			WcUser user = FindByEmail(userEmail);
			if (user == null)
			{
				//login uses username
				user = FindByName(userEmail);
				if (user == null) return SignInStatus.Failure;
			}

			if (CheckPassword(user))
			{
				return SignIn(user);
			}

			return SignInStatus.Failure;
		}

		private SignInStatus SignIn(WcUser user)
		{
			string userInfo = "name:" + user.Name + ";role:" + user.RoleName;
			FormsAuthentication.SetAuthCookie(user.Name, false);
			var cookie = new HttpCookie("UserInfo", userInfo);
			Response.SetCookie(cookie);
			return SignInStatus.Success;
		}

		private bool CheckPassword(WcUser user)
		{
			//List<WcUser> users = (List<WcUser>)Session["UserRepo"];
			return Users.Any(x => x.Name == user.Name && x.Password == user.Password);
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

		[AllowAnonymous]
		public ViewResult Register()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new WcUser { Name = model.UserName, Email = model.Email, Password = model.Password, RoleId = (int)RoleType.Members };

				bool[] result = repo.Add(user);

				if (result[1])
					return View("SuccessfullRegister");
				return View(model);
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		public ActionResult LogOff()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Index", "Home");
		}
	}
}