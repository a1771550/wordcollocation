using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using UI.Controllers.Abstract;
using UI.Models;
using UI.Models.Abstract;

namespace UI.Controllers
{
    public class WcUserController : ControllerBase<WcUser>
    {
		private readonly WcUserRepository repo = new WcUserRepository();
        // GET: WcUser
        public ActionResult Index()
        {
	        var model = new WcUserViewModel();
			return View("Index");
        }
    }
}