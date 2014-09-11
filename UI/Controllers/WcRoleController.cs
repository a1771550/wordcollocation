using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using UI.Controllers.Abstract;

namespace UI.Controllers
{
    public class WcRoleController : ControllerBase<WcRole>
    {
        // GET: WcRole
        public ActionResult Index()
        {
            return View();
        }
    }
}