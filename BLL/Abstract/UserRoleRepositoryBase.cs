using System;
using System.Collections.Generic;
using System.Linq;

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

		public virtual bool CheckIfDuplicatedEntry(params string[] entities)
		{
			bool bRet = false;

			if (entities.Length == 1) 
			{
				bRet = GetList().Any(x => x.Name.Equals(entities[0], StringComparison.OrdinalIgnoreCase));
			}

			return bRet;
		}
	}
}