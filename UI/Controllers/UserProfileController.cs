using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THResources;
using UI.Controllers.Abstract;

namespace UI.Controllers
{
    public class UserProfileController : UserRoleControllerBase
    {
        // GET: UserProfile
        public ActionResult Index()
        {
			//TODO:
	       
            return View();
        }
    }
}