using System.Collections.Generic;

namespace BLL.Abstract
{
	public interface IUserRoleRepository<in T> where T:UserRoleBase
	{
		string GetCacheName { get; }

		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, true)]
		bool[] Add(T t);

		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, true)]
		bool Update(T t);

		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, true)]
		bool Delete(int id);

		
	}
}
