using System;
using UI.Models.Abstract;

namespace UI.Models
{
	public class CommonPagingInfo:PagingInfoBase
	{
		public int TotalWords { get; set; }
		public int EntitiesPerPage { get; set; }
		public override int TotalPages { get { return (int)Math.Ceiling((decimal)TotalWords / EntitiesPerPage); } }
	}
}