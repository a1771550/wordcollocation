using System.Collections.Generic;
using System.Web.Mvc;
using BLL;
using UI.Models.Abstract;

namespace UI.Models
{
	public class WcUserViewModel:ViewModelBase
	{
		private readonly string id;
		public WcUserViewModel(string id = null)
		{
			this.id = id;
		}
		public IEnumerable<WcUser> List
		{
			get { return new WcUserRepository().GetList(); }
		}
		public List<SelectListItem> DropDownList
		{
			get
			{
				return CreateDropDownList(WcEntity.User,id);
			}
		}
	}
}