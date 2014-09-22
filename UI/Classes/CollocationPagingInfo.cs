using System;
using UI.Classes.Abstract;

namespace UI.Classes
{
	public class CollocationPagingInfo:PagingInfoBase
	{
		public int TotalCollocations { get; set; }
		public int CollocationsPerPage { get; set; }

		public override int TotalPages { get { return (int) Math.Ceiling((decimal) TotalCollocations/CollocationsPerPage); } }
	}
}