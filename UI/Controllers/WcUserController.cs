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
    public class WcUserController : UserRoleControllerBase<WcUser>
    {
		//private readonly WcUserRepository repo = new WcUserRepository();
		private WcUserRoleModelView model;
        // GET: WcUser
        public ActionResult Index()
        {
	        model = new WcUserRoleModelView(WcEntity.User);
	        return View("UserRoleList", model);
        }

		// Get
		public ViewResult Edit(int id = 0)
		{
			ViewBag.Title = id == 0 ? "Create" : "Edit";
			model = new WcUserRoleModelView(WcEntity.Role, id);
			return View("Edit", model);
		}

		[HttpPost]
		public ActionResult Edit(int id, string name)
		{

			return View("Index");
		}

		public ActionResult Delete(string id)
		{
			throw new NotImplementedException();
		}

	   
    }
}