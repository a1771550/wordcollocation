﻿using System.Collections.Generic;
using System.Web.Mvc;
using BLL;
using BLL.Abstract;
using THResources;

namespace UI.Models.Abstract
{
	public abstract class ViewModelBase
	{
		public virtual List<SelectListItem> CreateDropDownList(WcEntity wcEntity, string id = null)
		{
			var ddlEntity = new List<SelectListItem>();

			switch (wcEntity)
			{
				case WcEntity.Role:
					ddlEntity.Add(new SelectListItem { Selected = id == null, Text = string.Format("- {0} -", Resources.Pos), Value = "0" });
					var rrepo = new WcRoleRepository();
					List<WcRole> rList = rrepo.GetList();

					foreach (var entity in rList)
					{
						PopulateDropDownList_UsersRoles(id,ref ddlEntity, entity);
					}
					break;
				case WcEntity.User:
					ddlEntity.Add(new SelectListItem { Selected = id == null, Text = string.Format("- {0} -", Resources.Pos), Value = "0" });
					var urepo = new WcUserRepository();
					List<WcUser> uList = urepo.GetList();

					foreach (var entity in uList)
					{
						PopulateDropDownList_UsersRoles(id,ref ddlEntity, entity);
					}
					break;
				case WcEntity.Pos:
					ddlEntity.Add(new SelectListItem { Selected = id == null, Text = string.Format("- {0} -", Resources.Pos), Value = "0" });
					var prepo = new PosRepository();
					List<Pos> pList = prepo.GetList();

					foreach (var entity in pList)
					{
						PopulateDropDownList(entity, ref ddlEntity, id);
					}
					break;
				case WcEntity.ColPos:
					ddlEntity.Add(new SelectListItem { Selected = id == null, Text = string.Format("- {0} -", Resources.ColPos), Value = "0" });
					var cprepo = new ColPosRepository();
					List<ColPos> cpList = cprepo.GetList();
					foreach (var entity in cpList)
					{
						PopulateDropDownList(entity, ref ddlEntity, id);
					}
					break;
				case WcEntity.Word:
					ddlEntity.Add(new SelectListItem { Selected = id == null, Text = string.Format("- {0} -", Resources.Word), Value = "0" });
					var wrepo = new WordRepository();
					List<Word> wList = wrepo.GetList();
					foreach (var entity in wList)
					{
						PopulateDropDownList(entity, ref ddlEntity, id);
					}
					break;
				case WcEntity.ColWord:
					ddlEntity.Add(new SelectListItem { Selected = id == null, Text = string.Format("- {0} -", Resources.ColWord), Value = "0" });
					var cwrepo = new ColWordRepository();
					List<ColWord> cwList = cwrepo.GetList();
					foreach (var entity in cwList)
					{
						PopulateDropDownList(entity, ref ddlEntity, id);
					}
					break;
			}

			return ddlEntity;
		}

		private static void PopulateDropDownList_UsersRoles(string id,ref List<SelectListItem> ddlEntity, WcRole entity)
		{
			ddlEntity.Add(new SelectListItem
			{
				Text = entity.Name,
				Value = entity.Id,
				Selected = id == entity.Id
			});
		}

		private static void PopulateDropDownList(WcBase entity, ref List<SelectListItem> ddlEntity, string id)
		{
			ddlEntity.Add(new SelectListItem
				{
					Text = entity.Entry,
					Value = entity.Id,
					Selected = id == entity.Id
				});
		}
	}

	public enum WcEntity
	{
		Pos,
		ColPos,
		Word,
		ColWord,
		Collocation,
		WcExample,
		User,
		Role
	}
}