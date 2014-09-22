using System;
using System.Data;
using System.Data.SqlClient;
using DAL.Properties;
using DALC4NET;
using MySql.Data.MySqlClient;

namespace DAL
{
	public enum DbEngine
	{
		MsSql,
		MySql,
		Oracle,
	}
	public class DataAccess
	{
		private readonly DBHelper helper;
		//readonly DatabaseProviderFactory factory = new DatabaseProviderFactory();
		//private readonly Database database;
		public DataAccess()
		{
			//helper = new DBHelper();
			DbEngine engine = (DbEngine)Enum.Parse(typeof(DbEngine), Settings.Default.DbEngine, true);

			switch (engine)
			{
				case DbEngine.MsSql:
					helper = new DBHelper(DbEngine.MsSql.ToString());
					//database = string.IsNullOrEmpty(dbName) ? factory.CreateDefault() : factory.Create(dbName);
					break;
				case DbEngine.MySql:
					helper = new DBHelper(DbEngine.MySql.ToString());
					break;
				case DbEngine.Oracle:

					break;
			}
		}

		public object ExecuteScalar(string sql, CommandType commandType = CommandType.Text, bool useTrans = true, DBParameterCollection parameters = null)
		{
			if (useTrans)
			{
				IDbTransaction transaction = helper.BeginTransaction();
				try
				{
					object oRet = helper.ExecuteScalar(sql, parameters, transaction, commandType);
					helper.CommitTransaction(transaction);
					return oRet;
				}
				catch (Exception ex)
				{
					helper.RollbackTransaction(transaction);
					throw new Exception(ex.Message, ex.InnerException);
				}
			}
			return helper.ExecuteScalar(sql, parameters, commandType);
		}

		public object ExecuteScalar(string sql, CommandType commandType = CommandType.Text, bool useTrans = true, params DBParameter[] parameters)
		{
			DBParameterCollection parameterCollection = new DBParameterCollection();
			foreach (DBParameter parameter in parameters)
				parameterCollection.Add(parameter);

			return ExecuteScalar(sql, commandType, useTrans, parameterCollection);
		}

		public int ExecuteNonQuery(string sql, CommandType commandType = CommandType.Text, bool useTrans = true, DBParameterCollection parameters = null)
		{
			int iRet;

			if (useTrans)
			{
				IDbTransaction transaction = helper.BeginTransaction();

				try
				{
					iRet = helper.ExecuteNonQuery(sql, parameters ?? new DBParameterCollection(), transaction, commandType);
					helper.CommitTransaction(transaction);
					return iRet;
				}
				catch (Exception ex)
				{
					helper.RollbackTransaction(transaction);
					throw new Exception(ex.Message, ex.InnerException);
				}

			}
			iRet = helper.ExecuteNonQuery(sql, parameters, commandType);

			return iRet;
		}

		public DataSet ExecuteDataSet(string sql, CommandType commandType = CommandType.Text, DBParameterCollection parameters = null)
		{
			return helper.ExecuteDataSet(sql, parameters ?? new DBParameterCollection(), commandType);
		}

		public static bool[] CheckDbConnections()
		{
			byte dbNum = Settings.Default.CheckDbNumber;
			bool[] bEngines = new bool[dbNum];
			
			var msConnString = Settings.Default.DbConnectionString;
			var myConnString = Settings.Default.MySqlConnectionString;
			try
			{
				SqlConnection msConnection = new SqlConnection(msConnString);
				using (msConnection)
				{
					msConnection.Open();
					bEngines[0] = msConnection.State == ConnectionState.Open;
				}
				//if (dbNum == 1) bEngines[1] = true;

				if (dbNum == 2)
				{
					MySqlConnection myConnection = new MySqlConnection(myConnString);
					using (myConnection)
					{
						myConnection.Open();
						bEngines[1] = myConnection.State == ConnectionState.Open;
					}
				}
				
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex.InnerException);
			}

			return bEngines;

		}
	}
}
