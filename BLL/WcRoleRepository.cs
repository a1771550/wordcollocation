﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using BLL.Abstract;
using BLL.Helpers;
using DAL;
using DAL.UsersRolesDSTableAdapters;

namespace BLL
{
	public class WcRoleRepository : RepositoryBase<WcRole>, IRepository<WcRole>
	{
		private WcRolesTableAdapter adapter;
		private const string CacheName = "WcRoleCache";

		protected WcRolesTableAdapter Adapter
		{
			get { return adapter ?? (adapter = new WcRolesTableAdapter()); }
		}

		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
		public UsersRolesDS.WcRolesDataTable GetRoles()
		{
			UsersRolesDS.WcRolesDataTable roles;
			if (CacheHelper.Exists(GetCacheName))
			{
				CacheHelper.Get(GetCacheName, out roles);
			}
			else
			{
				roles = Adapter.GetList();
				CacheHelper.Add(roles, GetCacheName, ModelAppSettings.CacheExpiration_Minutes);
			}
			return roles;
		}

		public override List<WcRole> GetList()
		{
			var role = GetRoles();
			return (from UsersRolesDS.WcRolesRow row in role
					select new WcRole
					{
						Id = row.Id.ToString(),
						Name = row.Name,
						RowVersion = row.RowVersion
					}).ToList();

		}
		internal override string GetCacheName
		{
			get { return CacheName; }
		}

		public override WcRole GetObjectById(string id)
		{
			return GetList().SingleOrDefault(x => x.Id.ToString() == id);
		}

		public bool[] Add(WcRole role)
		{
			bool[] bRet = new bool[2];
			bRet[0] = CheckIfDuplicatedEntry(role.Name);

			if (bRet[0])
			{
				bRet[1] = false;
				return bRet;
			}

			try
			{
				using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
				{
					var affectedRow = Convert.ToInt32(Adapter.Insert(role.Name));
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

		public bool Update(WcRole role)
		{
			try
			{
				using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
				{
					var affectedRow = Convert.ToInt32(Adapter.Update(role.Name, int.Parse(role.Id), role.RowVersion));
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
				UsersRolesDS.WcRolesDataTable currentRole = Adapter.GetObjectById(Id);
				if (currentRole.Count == 0) return false;
				UsersRolesDS.WcRolesRow row = currentRole[0];
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
