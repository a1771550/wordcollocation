using DAL.WordCollocationDSTableAdapters;

namespace DAL
{
	public partial class WordCollocationDS
	{
		public partial class CollocationsRow
		{
			public WcExamplesDataTable GetWcExamples()
			{
				WcExamplesTableAdapter adapter = new WcExamplesTableAdapter();
				return adapter.GetObjectByCollocationId(Id);
			}
		}
	}
	
}
