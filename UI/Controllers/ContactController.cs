using System.Threading.Tasks;
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
        public async Task<ActionResult> Index(Contact contact)
        {
	        if (ModelState.IsValid)
	        {
				await EmailHelper.SendMailAsnyc(contact);
		        return View("Completed");
	        }
	        return View(contact);
        }
    }
}