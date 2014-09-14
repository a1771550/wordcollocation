using System.Collections.Generic;

namespace BLL.Abstract
{
	public abstract class UserRoleRepositoryBase<T> where T:UserRoleBase
	{
		public abstract List<T> GetList();

		public abstract T GetObjectById(int id);

		public virtual bool UpdateCanDel(WcRole T)
		{
			return false;
		}
	}
}