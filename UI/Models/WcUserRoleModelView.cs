using System.Collections.Generic;
using System.Linq;
using BLL;
using UI.Models.Abstract;

namespace UI.Models
{
	public class WcUserRoleModelView : ViewModelBase
	{
		private WcRoleRepository rrepo;
		private WcUserRepository urepo;

		public WcEntity Entity { get; set; }
		public List<WcRole> RoleList { get; set; }
		public List<WcUser> UserList { get; set; }
		public WcRole Role { get; set; }
		public WcUser User { get; set; }
		public WcUserRoleModelView(WcEntity entity)
		{
			Entity = entity;

			GetList(entity);
		}

		private void GetList(WcEntity entity)
		{
			switch (entity)
			{
				case WcEntity.Role:
					rrepo = new WcRoleRepository();
					RoleList = rrepo.GetList();
					break;
				case WcEntity.User:
					urepo = new WcUserRepository();
					UserList = urepo.GetList();
					break;
			}
		}

		public WcUserRoleModelView(WcEntity entity, int id)
			: this(entity)
		{
			switch (entity)
			{
				case WcEntity.Role:
					Role = RoleList.SingleOrDefault(x => x.Id == id);
					break;
				case WcEntity.User:
					User = UserList.SingleOrDefault(x => x.Id == id);
					break;
			}

		}
	}
}