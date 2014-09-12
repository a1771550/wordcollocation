using System;
using System.Web.Mvc;
using BLL;
using UI.Models;
using UI.Models.Abstract;

namespace UI.Controllers
{
	[Authorize(Roles = "Admin")]
	public class WcRoleController : Controller
	{
		readonly WcRoleRepository repo = new WcRoleRepository();
		private WcUserRoleModelView model;
		// GET: WcRole
		public ActionResult Index()
		{
			model = new WcUserRoleModelView(WcEntity.Role);
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
			try
			{
				if (ModelState.IsValid)
				{
					if (id == 0) // Create
					{
						var role = new WcRole();
						role.Name = name;
						role.CanDel = true;
						bool[] isOk = repo.Add(role);

						if (isOk[0]) // duplicated entry
						{
							ViewBag.IsDuplicatedEntry = true;
							return View("UserRoleEdit", role);
						}
						else if (!isOk[1]) // failed in inserting
						{

						}
						else if (!isOk[0] && isOk[1]) // add ok!
						{

						}
					}
					else // Edit
					{

					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex.InnerException);
			}

			return View("Index");
		}

		public ActionResult Delete(string id)
		{
			throw new NotImplementedException();
		}
	}
}