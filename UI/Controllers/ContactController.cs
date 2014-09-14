using System.Web.Mvc;
using BLL;
using UI.Controllers.Abstract;
using UI.Helpers;

namespace UI.Controllers
{
    public class ContactController : WcControllerBase
    {
	    [HttpGet]
	    public ActionResult Index()
	    {
		    return View(new Contact());
	    }

        [HttpPost]
        public ActionResult Index(Contact contact)
        {
	        if (ModelState.IsValid)
	        {
				EmailHelper.SendMail_Contact(contact);
		        return View("Completed");
	        }
	        return View(contact);
        }
    }
}