using DAL;

namespace BLL.Helpers
{
	public static class DbHelper
	{
		public static byte GetDbNumber()
		{
			return DAL.Properties.Settings.Default.CheckDbNumber;
		}
		public static bool[] CheckDbConnection()
		{
			return DataAccess.CheckDbConnections();
		}
	}
}
