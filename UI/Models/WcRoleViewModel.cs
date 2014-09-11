using System.Collections.Generic;
using System.Web.Mvc;
using BLL;
using UI.Models.Abstract;

namespace UI.Models
{
	public class WcRoleViewModel:ViewModelBase
	{
		private readonly string id;
		public WcRoleViewModel(string id = null)
		{
			this.id = id;
		}
		public IEnumerable<WcRole> List
		{
			get { return new WcRoleRepository().GetList(); }
		}
		public List<SelectListItem> DropDownList
		{
			get
			{
				return CreateDropDownList(WcEntity.Role,id);
			}
		}
	}
}