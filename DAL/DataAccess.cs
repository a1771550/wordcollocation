using System;
using System.Data;
using System.Transactions;
using DAL.Properties;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace DAL
{
	public enum DbType
	{
		MsSql,
		MySql,
		Oracle,
		Postgres
	}
	public class DataAccess
	{
		//private ProductsDataSet dsProducts;
		readonly DatabaseProviderFactory factory = new DatabaseProviderFactory();
		private readonly Database database;
		public DataAccess(string dbName = null)
		{
			//InitDataSet();
			DbType dbType;
			Enum.TryParse(Settings.Default.DbType, out dbType);

			switch (dbType)
			{
				case DbType.MsSql:
					database = string.IsNullOrEmpty(dbName) ? factory.CreateDefault() : factory.Create(dbName);
					break;
				case DbType.MySql:

					break;
				case DbType.Oracle:

					break;
				case DbType.Postgres:

					break;
			}
		}

		//private void InitDataSet()
		//{
		//	dsProducts = new ProductsDataSet();
		//	this.dsProducts.DataSetName = "ProductsDataSet";
		//	this.dsProducts.Locale = new System.Globalization.CultureInfo("en-US");
		//	this.dsProducts.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
		//}

		public object ExecuteScalar(string sql, CommandType commandType = CommandType.Text)
		{
			return database.ExecuteScalar(CommandType.Text, sql);
		}

		public int ExecuteNonQuery(string sql, CommandType commandType = CommandType.Text, object[] parameters = null)
		{
			int iRet = 0;
			switch (commandType)
			{
				case CommandType.StoredProcedure:
					try
					{
						using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
						{
							if (parameters != null)
							{
								iRet = database.ExecuteNonQuery(sql, parameters);
								scope.Complete();
							}
						}
					}
					catch (TransactionException exception)
					{
						throw new Exception(exception.Message, exception.InnerException);
					}

					break;
				case CommandType.Text:
					try
					{
						using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
						{
							iRet = database.ExecuteNonQuery(commandType, sql);
							scope.Complete();
						}
					}
					catch (TransactionException exception)
					{
						throw new Exception(exception.Message, exception.InnerException);
					}
					
					break;
			}
			return iRet;
		}

		public DataSet ExecuteDataSet(string sql, CommandType commandType = CommandType.Text, object[] parameters = null)
		{
			switch (commandType)
			{
				case CommandType.StoredProcedure:
					return database.ExecuteDataSet(commandType, sql);
				case CommandType.Text:
					return database.ExecuteDataSet(commandType, sql);
			}
			return null;
		}
		//public DataSet LoadDataSet(string sql, string[] tableNames, CommandType commandType = CommandType.Text, params object[] objects)
		//{
		//	switch (commandType)
		//	{
		//		case CommandType.StoredProcedure:
		//			dsProducts.Clear();
		//			database.LoadDataSet(sql, dsProducts, tableNames, objects);
		//			return dsProducts;
		//		case CommandType.Text:
		//			dsProducts.Clear();
		//			database.LoadDataSet(commandType, sql, dsProducts, tableNames);
		//			return dsProducts;
		//	}
		//	return null;
		//}

		//public int UpdateDataSet(string[] sqls, DbParameter[] inParameters, DbParameter[] deParameters, DbParameter[] upParameters, string tableName, UpdateBehavior behavior = UpdateBehavior.Transactional, CommandType commandType = CommandType.StoredProcedure)
		//{
		//	DbCommand insertCommand = database.GetStoredProcCommand(sqls[0]);
		//	DbCommand deleteCommand = database.GetStoredProcCommand(sqls[2]);
		//	DbCommand updateCommad = database.GetStoredProcCommand(sqls[1]);

		//	foreach (DbParameter obj in inParameters)
		//	{
		//		SetParameters(obj, insertCommand);
		//	}

		//	foreach (DbParameter obj in deParameters)
		//	{
		//		SetParameters(obj, deleteCommand);
		//	}

		//	foreach (DbParameter obj in upParameters)
		//	{
		//		SetParameters(obj, updateCommad);
		//	}

		//	return database.UpdateDataSet(dsProducts, tableName, insertCommand, updateCommad, deleteCommand, behavior);
		//}

		//private void SetParameters(DbParameter obj, DbCommand command)
		//{
		//	string name = obj.ParameterName;
		//	System.Data.DbType type = obj.DbType;
		//	string sourceColumn = obj.SourceColumn;
		//	DataRowVersion version = obj.SourceVersion;
		//	database.AddInParameter(command, name, type, sourceColumn, version);
		//}
	}
}
