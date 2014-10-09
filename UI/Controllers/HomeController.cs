using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using BLL;
using BLL.Helpers;
using UI.Classes;
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

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Search(string word, string colposid)
		{
			if (word!=null&&colposid!=null)
			{
				//string word = model.Word;
				short colPosId = Convert.ToInt16(colposid);
				var repo = new CollocationRepository();
				var collocationList = repo.GetCollocationListByWordColPosId(word, colPosId);
				if (collocationList.Count > 0)
				{
					Session[CollocationListSessionName] = collocationList;
					int pageSize = SiteConfiguration.WcViewPageSize;
					int pageCount;
					int listSize = collocationList.Count;
					if (listSize > 10)
						pageCount = (int) Math.Ceiling((double) (collocationList.Count/pageSize));
					else pageCount = 1;
					return RedirectToAction("SearchResult", pageCount);
				}
				WcSearchViewModel model=new WcSearchViewModel();
				model.Word = word;
				model.ColPosId = colposid;
				return View("NoSearchResult", model);
			}
			return null;
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ViewResult SearchResult(int page = 1)
		{
			WcSearchViewModel model = new WcSearchViewModel(ViewMode.SearchResult, page);
			return View("SearchResult", model);
			//return null;
		}

		public ViewResult UnderConstruction()
		{
			return View("_SiteUnderConstruction");
		}

		/// <summary>
		/// for debug only
		/// </summary>
		/// <returns></returns>
		public string Headers()
		{
			string host = System.Web.HttpContext.Current.Request.Headers["HOST"];
			return host;
		}
	}
}