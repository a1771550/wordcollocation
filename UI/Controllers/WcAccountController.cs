using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using BLL;
using BLL.Helpers;
using RestSharp;
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
	public class WcAccountController : UserRoleControllerBase
	{
		private const string UserCookie = "UserName";
		private const string GreetingsCookie = "Greetings";
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
		public ActionResult Login(WcLoginViewModel model, string returnUrl)
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

			SignInStatus result = PasswordSignIn(model);

			switch (result)
			{
				case SignInStatus.Success:

					if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") &&
						!returnUrl.StartsWith("/\\"))
					{
						return RedirectToAction(returnUrl);
					}
					return RedirectToAction("Index", "Home");
				case SignInStatus.Failure:
					ModelState.AddModelError("", Resources.UserNamePasswordNotMatch);
					return View("Login");
			}
			return null;
		}


		SignInStatus PasswordSignIn(WcLoginViewModel model)
		{
			//login uses email as username
			WcUser user = FindByEmail(model.UserNameEmail);
			if (user == null)
			{
				//login uses username
				user = FindByName(model.UserNameEmail);
				if (user == null) return SignInStatus.Failure;
				CookieHelper.SetCookie(UserCookie, model.UserNameEmail, DateTime.Now.AddDays(1));
				//Session["UserName"] = model.UserNameEmail;
			}

			if (CheckPassword(user))
			{
				FormsAuthentication.SetAuthCookie(model.UserNameEmail, model.RememberMe);
				//Session["UserName"] = repo.GetList().Single(x => x.Email == model.UserNameEmail).Name;
				string name = repo.GetList().Single(x => x.Email == model.UserNameEmail).Name;
				CookieHelper.SetCookie(UserCookie, name, DateTime.Now.AddDays(1));
				string greetings = SetGreetings(name);
				CookieHelper.SetCookie(GreetingsCookie,greetings,DateTime.Now.AddDays(1));
				return SignInStatus.Success;
			}

			return SignInStatus.Failure;
		}

		private string SetGreetings(string userName)
		{
			DateTime dt = DateTime.UtcNow;
			string client = dt.ToClientTime();
			// client = 14/9/2014 16:26:50
			const string pattern = @"(\s\d{2})";
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
			int hour = 0;
			if (regex.IsMatch(client))
			{
				MatchCollection matches = regex.Matches(client);
				foreach (Match match in matches)
				{
					hour = Convert.ToInt32(match.Groups[1].Value);
					break;
				}

				string greetings = null;
				string culture = CookieHelper.GetCookieValue("_culture");
				switch (culture)
				{
					case null: // => set default value
					case "zh-hans":
					case "zh-hant":
						if (hour <= 12)
							greetings = SiteConfiguration.GoodMorning;
						else if (hour > 12 && hour <= 17)
							greetings = SiteConfiguration.GoodAfternoon;
						else if (hour > 17) greetings = SiteConfiguration.GoodEvening;
						break;
					case "ja-jp":
						if (hour <= 12)
							greetings = SiteConfiguration.GoodMorningJap;
						else if (hour > 12 && hour <= 17)
							greetings = SiteConfiguration.GoodAfternoonJap;
						else if (hour > 17) greetings = SiteConfiguration.GoodEveningJap;
						break;
				}
				return string.Format("{0} {1}", userName, greetings);
			}
			return null;
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
		public ActionResult Register(WcRegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new WcUser { Name = model.UserName, Email = model.Email, Password = model.Password, RoleId = (int)RoleType.Members };

				bool[] result = repo.Add(user);

				if (result[1])
				{
					return View("SuccessfullRegister");
				}

				return View(model);
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		public ActionResult LogOff()
		{
			FormsAuthentication.SignOut();
			CookieHelper.DeleteCookie("UserName");
			CookieHelper.DeleteCookie("Greetings");
			Session.Abandon();
			return RedirectToAction("Index", "Home");
		}
	}
}