using System;
using System.Web;
using System.Web.Mvc;
using BLL;
using BLL.Helpers;
using UI.Controllers.Abstract;
using UI.Helpers;
using UI.Models;

namespace UI.Controllers
{
	//[HandleError]
	public class HomeController : WcControllerBase
	{
		public const string CollocationListSessionName = "CollocationList";
		//
		// GET: /Home/
		public ActionResult Index()
		{
			bool[] bCon = DbHelper.CheckDbConnection();
			if (bCon.Length == 1)
			{
				if (!bCon[0])
				{
					ViewBag.DbEngines = "MsSql";
					return View("DbConnectionFailed");
				}
			}
			if (bCon.Length == 2)
			{
				if (!bCon[0] && bCon[1])
				{
					ViewBag.DbEngines = "MsSql";
					return View("DbConnectionFailed");
				}
				if (!bCon[0] && !bCon[1])
				{
					ViewBag.DbEngines = "MsSql + MySql";
					return View("DbConnectionFailed");
				}
				if (bCon[0] && !bCon[1])
				{
					ViewBag.DbEngines = "MySql";
					return View("DbConnectionFailed");
				}
			}
			
			WcSearchViewModel model = new WcSearchViewModel(ViewMode.Home);
			return View(model);
		}

		public void SetCulture(string culture, string returnUrl)
		{
			culture = CultureHelper.GetImplementedCulture(culture);
			HttpCookie cookie = Request.Cookies["_culture"];
			if (cookie != null) cookie.Value = culture;
			else
			{
				cookie = new HttpCookie("_culture");
				cookie.Value = culture;
				cookie.Expires = DateTime.Now.AddYears(1);
			}
			Response.Cookies.Add(cookie);

			if (Request.IsAuthenticated)
			{
				GreetingsHelper.SetGreetings((string)Session["UserName"], GreetingsCookie);	
			}

			Response.Redirect(returnUrl);
		}

		[HttpPost]
		public ActionResult Search(WcSearchViewModel model)
		{
			if (ModelState.IsValid)
			{
				string word = model.Word;
				short colPosId = Convert.ToInt16(model.ColPosId);
				var repo = new CollocationRepository();
				var collocationList = repo.GetCollocationListByWordColPosId(word, colPosId);
				if (collocationList.Count > 0)
				{
					Session[CollocationListSessionName] = collocationList;
					//return View("SearchResult",model);
					return RedirectToAction("SearchResult");
				}
				return View("NoSearchResult", model);
			}
			return null;
		}

		/*
		 * public ActionResult Index(string letter = null, int page = 1)
		{
			if (string.IsNullOrEmpty(letter)) letter = "a";
			WcSearchViewModel model = new WcSearchViewModel(ViewMode.Admin, letter, page);
            return View(model);
        }
		 */
		[HttpGet]
		public ViewResult SearchResult(int page=1)
		{
			WcSearchViewModel model = new WcSearchViewModel(ViewMode.SearchResult, page);
			return View("SearchResult", model);
			//return null;
		}

		public ViewResult UnderConstruction()
		{
			return View("_SiteUnderConstruction");
		}
	}
}