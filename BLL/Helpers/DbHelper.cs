using System.Data;
using DAL;

namespace BLL.Helpers
{
	public static class DbHelper
	{
		public static bool CheckDbConnection()
		{
			ConnectionState state= DataAccess.CheckSqlConnection();
			return state == ConnectionState.Open;
		}
	}
}
