using System;
using UI.Classes.Abstract;

namespace UI.Models
{
	public class WcExamplePagingInfo:PagingInfoBase
	{
		public int TotalWcExamples { get; set; }
		public int WcExamplesPerPage { get; set; }

		public override int TotalPages { get { return (int)Math.Ceiling((decimal)TotalWcExamples / WcExamplesPerPage); } }
	}
}