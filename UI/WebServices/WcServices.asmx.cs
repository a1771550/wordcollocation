using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using BLL;
using BLL.Abstract;
using UI.Classes;
using UI.Models.Abstract;

namespace UI.WebServices
{
	/// <summary>
	/// Summary description for WcServices
	/// </summary>
	[WebService(Namespace = "http://www.translationhall.com/WebServices")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	[System.Web.Script.Services.ScriptService]
	public class WcServices : WebService
	{
		private IEnumerable<WcBase> GetWcList(WcEntity entity)
		{
			switch (entity)
			{
				case WcEntity.Pos:


				case WcEntity.ColPos:


				case WcEntity.Word:


				case WcEntity.ColWord:

					return null;
			}
			return null;
		}
		const string CollocationListSessionName = "CollocationList";
		/*
				private IEnumerable<UserRoleBase> GetUrList(WcEntity entity)
				{
					switch (entity)
					{
						case WcEntity.User:
							var urepo = new WcUserRepository();
							return urepo.GetList();
						case WcEntity.Role:
							var rrepo = new WcRoleRepository();
							return rrepo.GetList();
					}
					return null;
				}
		*/

		[WebMethod]
		public bool CheckIfDuplicatedUserName(string name)
		{
			var repo = new WcUserRepository();
			return repo.CheckIfDuplicatedUserName(name);
		}

		[WebMethod]
		public bool CheckIfDuplicatedEmail(string email)
		{
			var repo = new WcUserRepository();
			return repo.CheckIfDuplicatedEmail(email);
		}

		[WebMethod(EnableSession = true)]
		public string SearchCollocation(string word, string id)
		{
			if (word != null && id != null)
			{
				short Id = Convert.ToInt16(id);
				var repo = new CollocationRepository();
				var collocationList = repo.GetCollocationListByWordColPosId(word, Id);
				string host = HttpContext.Current.Request.Headers["HOST"];
				string url;
				string protocol = SiteConfiguration.Protocol + @"://";
				if (collocationList.Count > 0)
				{
					HttpContext.Current.Session[CollocationListSessionName] = collocationList;
					int pageSize = SiteConfiguration.WcViewPageSize;
					int pageCount;
					int listSize = collocationList.Count;
					if (listSize > 10)
						pageCount = (int)Math.Ceiling((double)(collocationList.Count / pageSize));
					else pageCount = 1;
					url = string.Format("{0}{1}{2}{3}",protocol, host, @"/Home/SearchResult/", pageCount);
					return url;
				}
				url = string.Format("{0}{1}{2}",protocol, host, @"/Home/NoSearchResult");
				return url;
			}
			return null;
		}

		[WebMethod]
		public bool[] CheckEmail(string email)
		{
			bool[] bRet = new bool[2];
			var repo = new WcUserRepository();
			if (repo.CheckIfDuplicatedEmail(email)) bRet[0] = false;
			else bRet[0] = true;

			string pattern = SiteConfiguration.EmailPattern;
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
			if (!regex.IsMatch(email))
				bRet[1] = false;
			else bRet[1] = true;
			return bRet;
		}

		[WebMethod]
		public bool CheckIfDuplicatedEntry(WcEntity entity, params string[] entities)
		{
			bool bRet = false;

			if (entities.Length == 1) // for pos,colpos,word,colword, wcuser, wcrole
			{
				bRet = GetWcList(entity).Any(x => x.Entry.Equals(entities[0], StringComparison.OrdinalIgnoreCase));
			}

			return bRet;
		}

		[WebMethod]
		public bool CheckPasswordLength(string package)
		{
			var min = SiteConfiguration.MinPasswordLength;
			var max = SiteConfiguration.MaxPasswordLength;
			return package.Length >= min && package.Length <= max;
		}
	}
}
