using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Generator
{
	class DefineTable
	{
		public static DataTable Index (string dataBaseConnection)
		{
			DataTable myTable;
			SqlCommand cmd;
			string sqlString =	"SELECT * FROM tableIndex ";

			using (SqlConnection connection = new SqlConnection(dataBaseConnection))
			{
				cmd = new SqlCommand(sqlString, connection);
				cmd.Connection.Open();

				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				myTable = new DataTable();

				try
				{
					sda.Fill(myTable);
				}
				catch (Exception ex)
				{
					cmd.Connection.Close();
					Console.WriteLine(ex.Message + ". Database does not contain tableIndex View. Attempting to use sys Tables to complete action.");
					sqlString =	"SELECT REPLACE(sys.indexes.name, 'PK_', '') AS TableName, sys.identity_columns.name AS TableIndex, sys.indexes.index_id AS currentIndex " +
								"FROM sys.indexes INNER JOIN sys.identity_columns ON sys.identity_columns.object_id = sys.indexes.object_id " +
								"WHERE(sys.indexes.name LIKE 'PK_%') AND (NOT(sys.identity_columns.name = 'diagram_id'))";

					cmd = new SqlCommand(sqlString, connection);
					cmd.Connection.Open();

					sda = new SqlDataAdapter(cmd);
					myTable = new DataTable();

					try
					{
						sda.Fill(myTable);
					}
					catch (Exception exTwo)
					{
						Console.WriteLine(exTwo.Message + ". Failed Again. Please attempt again after review of Database.");
					}
				}
			}
			
			return myTable;
		}

		public static DataTable Column (string dataBaseConnection)
		{
			DataTable myTable;
			SqlCommand cmd;
			string sqlString = "SELECT * FROM tableColumn ";

			using (SqlConnection connection = new SqlConnection(dataBaseConnection))
			{
				cmd = new SqlCommand(sqlString, connection);
				cmd.Connection.Open();

				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				myTable = new DataTable();

				try
				{
					sda.Fill(myTable);
				}
				catch (Exception ex)
				{
					cmd.Connection.Close();
					Console.WriteLine(ex.Message + ". Database does not contain tableColumn View. Attempting to use sys Tables to complete action.");
					sqlString =	"SELECT INFORMATION_SCHEMA.COLUMNS.TABLE_NAME AS TableName, INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME AS ColumnName, INFORMATION_SCHEMA.COLUMNS.IS_NULLABLE AS isNull," +
									" INFORMATION_SCHEMA.COLUMNS.DATA_TYPE AS dataType, (CASE WHEN dbo.tableIndex.TableIndex = [CCSOTicketingManager].[INFORMATION_SCHEMA].[COLUMNS].[COLUMN_NAME] " +
									"THEN 1 ELSE 0 END) AS isIndex " +
								"FROM INFORMATION_SCHEMA.COLUMNS INNER JOIN dbo.tableIndex ON INFORMATION_SCHEMA.COLUMNS.TABLE_NAME = dbo.tableIndex.TableName " +
								"WHERE (NOT(INFORMATION_SCHEMA.COLUMNS.TABLE_NAME = 'tableIndex' OR INFORMATION_SCHEMA.COLUMNS.TABLE_NAME = 'sysdiagrams'))";

					cmd = new SqlCommand(sqlString, connection);
					cmd.Connection.Open();

					sda = new SqlDataAdapter(cmd);
					myTable = new DataTable();

					try
					{
						sda.Fill(myTable);
					}
					catch (Exception exTwo)
					{
						Console.WriteLine(exTwo.Message + ". Failed Again. Please attempt again after review of Database.");
					}
				}
			}

			return myTable;
		}
	}
}
