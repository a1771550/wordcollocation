using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using BLL.Abstract;
using BLL.Helpers;
using DAL;
using DAL.UsersRolesDSTableAdapters;

namespace BLL
{
	public class WcUserRepository : RepositoryBase<WcUser>, IRepository<WcUser>
	{
		private const string CacheName = "UserCache";

		private WcUsersTableAdapter adapter;

		protected WcUsersTableAdapter Adapter
		{
			get { return adapter ?? (adapter = new WcUsersTableAdapter()); }
		}
		internal override string GetCacheName
		{
			get { return CacheName; }
		}

		public UsersRolesDS.WcUsersDataTable GetUsers()
		{
			UsersRolesDS.WcUsersDataTable users;
			if (CacheHelper.Exists(GetCacheName))
			{
				CacheHelper.Get(GetCacheName, out users);
			}
			else
			{
				users = Adapter.GetList();
				CacheHelper.Add(users, GetCacheName, ModelAppSettings.CacheExpiration_Minutes);
			}
			return users;
		}

		public override List<WcUser> GetList()
		{
			var user = GetUsers();
			return (from UsersRolesDS.WcUsersRow row in user
					select new WcUser
					{
						Id = row.Id.ToString(),
						Name = row.Name,
						Password = row.Password,
						Email = row.Email,
						RowVersion = row.RowVersion,
						RoleId = row.RoleId
					}
				).ToList();
		}
		public override WcUser GetObjectById(string id)
		{
			return GetUserRolesList().SingleOrDefault(x => x.Id.ToString() == id);
		}

		public UsersRolesDS.WcUsersDataTable GetUserRoles()
		{
			return Adapter.GetUsersWithRole();
		}

		public bool ValidateUser(int id, string password)
		{
			return (Adapter.GetUserByIdPwd(id, password))==1;
		}

		public List<WcUser> GetUserRolesList()
		{
			var user = GetUserRoles();
			return (from UsersRolesDS.WcUsersRow row in user
					select new WcUser
					{
						Id = row.Id.ToString(),
						Name = row.Name,
						Password = row.Password,
						Email = row.Email,
						RowVersion = row.RowVersion,
						RoleId = row.RoleId,
						RoleName = row.RoleName
					}
				).ToList();
		} 

		public bool[] Add(WcUser user)
		{
			bool[] bRet = new bool[2];
			bRet[0] = CheckIfDuplicatedEntry(user.Name);

			if (bRet[0])
			{
				bRet[1] = false;
				return bRet;
			}

			try
			{
				using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
				{
					var affectedRow = Convert.ToInt32(Adapter.Insert(user.Name, user.Password, user.Email, user.RoleId));
					scope.Complete();
					bRet[1] = affectedRow == 1;
					CacheHelper.Clear(GetCacheName);
					return bRet;
				}
			}
			catch (TransactionException ex)
			{
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public bool Update(WcUser user)
		{
			try
			{
				using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
				{
					var affectedRow = Convert.ToInt32(Adapter.Update(user.Name, user.Password, user.Email, user.RoleId, int.Parse(user.Id), user.RowVersion));
					scope.Complete();
					CacheHelper.Clear(GetCacheName);
					return affectedRow == 1;
				}
			}
			catch (TransactionException ex)
			{
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		public bool Delete(string id)
		{
			try
			{
				var Id = short.Parse(id);
				UsersRolesDS.WcUsersDataTable currentUser = Adapter.GetObjectById(Id);
				if (currentUser.Count == 0) return false;
				UsersRolesDS.WcUsersRow row = currentUser[0];
				using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
				{
					var affectedRow = Convert.ToInt32(Adapter.Delete(Id, row.RowVersion));
					scope.Complete();
					CacheHelper.Clear(GetCacheName);
					return affectedRow == 1;
				}
			}
			catch (TransactionException ex)
			{
				throw new Exception(ex.Message, ex.InnerException);
			}
		}
	}
}
