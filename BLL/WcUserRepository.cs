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
	public class WcUserRepository : UserRoleRepositoryBase<WcUser>, IUserRoleRepository<WcUser>
	{
		private const string CacheName = "UserCache";

		private WcUsersTableAdapter adapter;

		protected WcUsersTableAdapter Adapter
		{
			get { return adapter ?? (adapter = new WcUsersTableAdapter()); }
		}

		public string GetCacheName
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
			var table = GetUserRoles();
			return table.Rows.Count > 0 ? ConvertTableToList(table) : null;
		}
		public override WcUser GetObjectById(int id)
		{
			UsersRolesDS.WcUsersDataTable table = Adapter.GetObjectById(id);
			return table.Rows.Count > 0 ? ConvertTableToList(table)[0] : null;
		}
		public WcUser GetObjectByEmail(string email)
		{
			UsersRolesDS.WcUsersDataTable table = Adapter.GetObjectByEmail(email);
			return table.Rows.Count > 0 ? ConvertTableToList(table)[0] : null;
		}
		public WcUser GetObjectByName(string name)
		{
			UsersRolesDS.WcUsersDataTable table = Adapter.GetObjectByName(name);
			return table.Rows.Count > 0 ? ConvertTableToList(table)[0] : null;
		}
		public WcUser GetObjectByNamePassword(string name, string password)
		{
			var table = Adapter.GetObjectByNamePassword(name, password);
			return table.Rows.Count > 0 ? ConvertTableToList(table)[0] : null;
		}
		public UsersRolesDS.WcUsersDataTable GetUserRoles()
		{
			return Adapter.GetUsersWithRole();
		}

		public bool ValidateUserByIdPwd(int id, string password)
		{
			return (Adapter.GetUserByIdPwd(id, password)) == 1;
		}

		public bool ValidateUserByNamePwd(string name, string password)
		{
			return Convert.ToBoolean(Adapter.ValidateByNamePwd(name, password));
		}
		public List<WcUser> GetUserRolesList()
		{
			var table = GetUserRoles();
			return ConvertTableToList(table);
		}

		public bool CheckIfDuplicatedEmail(string email)
		{
			return Convert.ToBoolean(Adapter.CheckIfDuplicatedEmail(email));
		}

		public bool CheckIfDuplicatedUserName(string name)
		{
			return Convert.ToBoolean(Adapter.CheckIfDuplicatedUserName(name));
		}

		private List<WcUser> ConvertTableToList(UsersRolesDS.WcUsersDataTable table)
		{
			return (from UsersRolesDS.WcUsersRow row in table.Rows
					select new WcUser
					{
						Id = row.Id,
						Name = row.Name,
						Password = row.Password,
						Email = row.Email,
						RowVersion = row.RowVersion,
						RoleId = row.RoleId,
						RoleName = row.RoleName
					}).ToList();
		}
		public int ResetPasswordByEmail(string email, string password)
		{
			var table = Adapter.GetObjectByEmail(email);
			var user = ConvertTableToList(table)[0];
			return Adapter.ResetPassword(password, user.Id, user.RowVersion);
		}

		public int ResetPasswordByName(string name, string password)
		{
			var table = Adapter.GetObjectByName(name);
			var user = ConvertTableToList(table)[0];
			return Adapter.ResetPassword(password, user.Id, user.RowVersion);
		}
		public bool[] Add(WcUser user)
		{
			bool[] bRet = new bool[2];

			bRet[0] = CheckIfDuplicatedEmail(user.Name);

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
					var affectedRow = Convert.ToInt32(Adapter.Update(user.Name, user.Password, user.Email, user.RoleId, user.Id, user.RowVersion));
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

		public bool Delete(int id)
		{
			try
			{
				var Id = id;
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
