using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Generator
{
	class RunSPCSharp
	{
		//might expand later to auto add to database
		public static StringBuilder Invoke(DataTable Index, DataTable Column)
		{
			Console.WriteLine("");
			Console.WriteLine("Beginning to Create Run SP Files");
			Console.WriteLine("");
			Console.WriteLine("");
			Console.WriteLine("Press Enter to Begin.");
			Console.WriteLine("Type 'Skip' to Skip Process");
			string skip = Console.ReadLine().ToLower();
			StringBuilder output = new StringBuilder();
			if (!skip.Equals("skip"))
				output = GenerateList(Index, Column);

			return output;
		}

		private static StringBuilder GenerateList(DataTable index, DataTable column)
		{
			StringBuilder myList = new StringBuilder();

			foreach (DataRow table in index.Rows)
			{
				StringBuilder tempBuild = GenerateInsertObjects(table, column);
				myList.Append(tempBuild);
				Console.Write(myList);
				Console.WriteLine("");
				Console.WriteLine("");
			}

			foreach (DataRow table in index.Rows)
			{
				StringBuilder tempBuild = GeneratePullObjects(table, column);
				myList.Append(tempBuild);
				Console.Write(myList);
				Console.WriteLine("");
				Console.WriteLine("");
			}

			foreach (DataRow table in index.Rows)
			{
				StringBuilder tempBuild = GenerateUpdateObjects(table, column);
				myList.Append(tempBuild);
				Console.Write(myList);
				Console.WriteLine("");
				Console.WriteLine("");
			}

			return myList;
		}

		private static StringBuilder GenerateInsertObjects(DataRow index, DataTable column)
		{
			StringBuilder objects = new StringBuilder();

			objects.AppendLine("		public static void InsertData_ " + CapitalFirstLetter(index["TableName"].ToString()) + "(" + CapitalFirstLetter(index["TableName"].ToString()) + " myObj)");
			objects.AppendLine("		{");
			objects.AppendLine("			using (SqlConnection connection = new SqlConnection(CreateDBConns(\"ticket\")))");
			objects.AppendLine("			{");
			objects.AppendLine("				SqlCommand cmd = new SqlCommand(\"[dbo].[gensp_Insert_" + index["TableName"].ToString() + "]\", connection);");
			objects.AppendLine("				cmd.CommandType = System.Data.CommandType.StoredProcedure;");


			foreach (DataRow columnName in column.Rows)
			{
				if (!Convert.ToBoolean(columnName["isIndex"]) && index["TableName"].ToString().Equals(columnName["TableName"].ToString()))
				{
					objects.AppendLine("				cmd.Parameters.AddWithValue(\"@" + columnName["ColumnName"].ToString() + "\", myObj." + CapitalFirstLetter(columnName["ColumnName"].ToString()) + ");");
				}
			}
			objects.AppendLine("				cmd.Connection.Open();");
			objects.AppendLine("				cmd.ExecuteNonQuery();");
			objects.AppendLine("			}");
			objects.AppendLine("		}");

			objects.AppendLine("");

			return objects;
		}

		private static StringBuilder GeneratePullObjects(DataRow index, DataTable column)
		{
			StringBuilder objects = new StringBuilder();

			objects.AppendLine("		public static DataTable PullData_ " + CapitalFirstLetter(index["TableName"].ToString()) + "(" + CapitalFirstLetter(index["TableName"].ToString()) + " myObj, bool recordPull, int userID = 1)");
			objects.AppendLine("		{");
			objects.AppendLine("			DataTable myTable = new DataTable();");
			objects.AppendLine("			using (SqlConnection connection = new SqlConnection(CreateDBConns(\"ticket\")))");
			objects.AppendLine("			{");
			objects.AppendLine("				SqlCommand cmd = new SqlCommand(\"[dbo].[gensp_Pull_" + index["TableName"].ToString() + "]\", connection);");
			objects.AppendLine("");
			objects.AppendLine("				cmd.CommandType = System.Data.CommandType.StoredProcedure;");


			foreach (DataRow columnName in column.Rows)
			{
				if (index["TableName"].ToString().Equals(columnName["TableName"].ToString())
					&& (columnName["ColumnName"].ToString().Contains("active") || Convert.ToBoolean(columnName["isIndex"])))
				{
					objects.AppendLine("				cmd.Parameters.AddWithValue(\"@" + columnName["ColumnName"].ToString() + "\", myObj." + CapitalFirstLetter(columnName["ColumnName"].ToString()) + ");");
				}
			}
			objects.AppendLine("				cmd.Parameters.AddWithValue(\"@dateChanged\", DateTime.Now);");
			objects.AppendLine("				cmd.Parameters.AddWithValue(\"@changeUser\", userID);");
			objects.AppendLine("				cmd.Parameters.AddWithValue(\"@recordPull\", recordPull);");
			objects.AppendLine("");

			objects.AppendLine("				using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))");
			objects.AppendLine("				{");
			objects.AppendLine("					cmd.Connection.Open();");
			objects.AppendLine("					dataAdapter.Fill(myTable);");
			objects.AppendLine("				}");
			objects.AppendLine("			}");
			objects.AppendLine("		}");

			objects.AppendLine("");

			return objects;
		}

		private static StringBuilder GenerateUpdateObjects(DataRow index, DataTable column)
		{
			StringBuilder objects = new StringBuilder();

			objects.AppendLine("		public static void UpdateData_ " + CapitalFirstLetter(index["TableName"].ToString()) + "(" + CapitalFirstLetter(index["TableName"].ToString()) + " myObj, int userID = 1)");
			objects.AppendLine("		{");
			objects.AppendLine("			using (SqlConnection connection = new SqlConnection(CreateDBConns(\"ticket\")))");
			objects.AppendLine("			{");
			objects.AppendLine("				SqlCommand cmd = new SqlCommand(\"[dbo].[gensp_Update_" + index["TableName"].ToString() + "]\", connection);");
			objects.AppendLine("				cmd.CommandType = System.Data.CommandType.StoredProcedure;");


			foreach (DataRow columnName in column.Rows)
			{
				if (index["TableName"].ToString().Equals(columnName["TableName"].ToString()) && !(index["TableName"].ToString().Equals("dateAdded") || index["TableName"].ToString().Equals("createdUser")))
				{
					objects.AppendLine("				cmd.Parameters.AddWithValue(\"@" + columnName["ColumnName"].ToString() + "\", myObj." + CapitalFirstLetter(columnName["ColumnName"].ToString()) + ");");
				}
			}
			objects.AppendLine("				cmd.Parameters.AddWithValue(\"@dateChanged\", DateTime.Now);");
			objects.AppendLine("				cmd.Parameters.AddWithValue(\"@changeUser\", userID);");

			objects.AppendLine("				cmd.Connection.Open();");
			objects.AppendLine("				cmd.ExecuteNonQuery();");
			objects.AppendLine("			}");
			objects.AppendLine("		}");

			objects.AppendLine("");

			return objects;
		}


		private static string CapitalFirstLetter(string temp)
		{
			string newTemp = temp.Substring(0, 1).ToUpper() + temp.Substring(1);
			return newTemp;
		}
	}
}
