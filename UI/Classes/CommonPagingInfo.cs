using System;
using UI.Classes.Abstract;

namespace UI.Classes
{
	public class CommonPagingInfo:PagingInfoBase
	{
		public int TotalWords { get; set; }
		public int EntitiesPerPage { get; set; }
		public override int TotalPages { get { return (int)Math.Ceiling((decimal)TotalWords / EntitiesPerPage); } }
	}
}