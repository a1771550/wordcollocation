﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using BLL.Helpers;

namespace UI.Controllers.Abstract
{
	public abstract class CommonControllerBase : Controller
	{
		public const string UserCookie = "UserName";
		public const string GreetingsCookie = "Greetings";
		
		protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
		{
			string cultureName;

			// attempt to read the culture cookie from request
			HttpCookie cultureCookie = Request.Cookies["_culture"];
			if (cultureCookie != null) cultureName = cultureCookie.Value;
			else
				cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null;

			// validate culture name
			cultureName = CultureHelper.GetImplementedCulture(cultureName);

			// modify current thread's cultures
			Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
			Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

			return base.BeginExecuteCore(callback, state);
		}

		public virtual void RenderModelErrorList()
		{
			List<ModelError> Errors = new List<ModelError>();
			foreach (var m in ModelState.Values)
			{
				Errors.AddRange(m.Errors);
			}
			ViewBag.ErrorList = Errors;
		}

	}
}