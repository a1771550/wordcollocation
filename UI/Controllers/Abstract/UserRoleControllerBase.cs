using System.Web.Mvc;
using BLL.Abstract;

namespace UI.Controllers.Abstract
{
	public abstract class UserRoleControllerBase<T> : Controller where T: UserRoleBase
	{

	}
}