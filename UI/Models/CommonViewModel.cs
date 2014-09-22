using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL;
using UI.Classes;
using UI.Models.Abstract;

namespace UI.Models
{
	public class CommonViewModel : ViewModelBase
	{
		private readonly WcEntity entity;
		public WcEntity Entity { get { return entity; } }
		private readonly string id;
		private PosRepository prepo;
		private ColPosRepository cprepo;
		private WordRepository wrepo;
		private ColWordRepository cwrepo;
		private List<Pos> _PosList;
		private List<ColPos> _ColPosList;
		private List<Word> _WordList;
		private List<ColWord> _ColWordList;
		private readonly int page;
		public CommonPagingInfo PagingInfo { get; set; }
		public List<Pos> PosList { get { return _PosList; } }
		public List<ColPos> ColPosList { get { return _ColPosList; } }
		public List<ColWord> ColWordList { get { return _ColWordList; } }
		public List<Word> WordList { get { return _WordList; } }

		public int PageSize = SiteConfiguration.WcViewPageSize;

		public CommonViewModel(int page = 1)
		{
			this.page = page;
		}

		public CommonViewModel(string id = null)
		{
			this.id = id;
		}

		public CommonViewModel(WcEntity entity, string id = null)
			: this(id)
		{
			this.entity = entity;
		}

		public CommonViewModel(WcEntity entity, int page = 1)
			: this(page)
		{
			this.entity = entity;
			SetCommonList();
			if(entity != WcEntity.Pos && entity!= WcEntity.ColPos)
				SetPageInfo();
		}

		private void SetCommonList()
		{
			switch (entity)
			{
				case WcEntity.Pos:
					prepo = new PosRepository();
					_PosList = prepo.GetList();
					break;
				case WcEntity.ColPos:
					cprepo = new ColPosRepository();
					_ColPosList = cprepo.GetList();
					break;
				case WcEntity.Word:
					wrepo = new WordRepository();
					_WordList = wrepo.GetList();
					break;
				case WcEntity.ColWord:
					cwrepo = new ColWordRepository();
					_ColWordList = cwrepo.GetList();
					break;

			}
		}
		private void SetPageInfo()
		{
			CommonPagingInfo pagingInfo = new CommonPagingInfo();
			pagingInfo.CurrentPage = page;
			pagingInfo.EntitiesPerPage = PageSize;

			switch (entity)
			{
				case WcEntity.Word:
					_ColWordList = null;
					setPagingDetails(ref _WordList, ref _ColWordList, pagingInfo);
					break;
				case WcEntity.ColWord:
					_WordList = null;
					setPagingDetails(ref _WordList, ref _ColWordList, pagingInfo);
					break;
			}

		}

		private void setPagingDetails(ref List<Word> wlist, ref List<ColWord> cwlist, CommonPagingInfo pagingInfo)
		{
			if (wlist != null && wlist.Count > 0)
			{
				if (page < 1) return;
				pagingInfo.TotalWords = wlist.Count();
				PagingInfo = pagingInfo;
				wlist = wlist.Skip((page - 1) * PageSize).Take(PageSize).ToList();
			}
			else if (cwlist != null && cwlist.Count > 0)
			{
				if (page < 1) return;
				pagingInfo.TotalWords = cwlist.Count();
				PagingInfo = pagingInfo;
				cwlist = cwlist.Skip((page - 1) * PageSize).Take(PageSize).ToList();
			}
		}

		public List<SelectListItem> CommonDropDownList
		{
			get
			{
				return CreateDropDownList(entity, id);
			}
		}
	}
}