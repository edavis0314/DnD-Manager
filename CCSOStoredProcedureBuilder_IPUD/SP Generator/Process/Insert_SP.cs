using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Generator
{
	class Insert_SP
	{
		//might expand later to auto add to database
		public static StringBuilder Invoke (string connectionString, DataTable Index, DataTable Column)
		{
			Console.WriteLine("");
			Console.WriteLine("Beginning to Create insert Statements");
			Console.WriteLine("");
			Console.WriteLine("");
			Console.WriteLine("Press Enter to Begin.");
			Console.WriteLine("Type 'Skip' to Skip Process");
			string skip = Console.ReadLine().ToLower();
			StringBuilder output = new StringBuilder();
			if (!skip.Equals("skip"))
				output = GenerateList(connectionString, Index, Column);

			return output;
		}

		private static StringBuilder GenerateList(string connectionString, DataTable index, DataTable column)
		{
			StringBuilder myList = new StringBuilder();

			foreach(DataRow table in index.Rows)
			{
				StringBuilder tempBuild = GenerateInsertStatement(connectionString, table, column);
				myList.Append(tempBuild);
				Console.Write(tempBuild);
				Console.WriteLine("");
				Console.WriteLine("");
			}

			return myList;
		}

		private static StringBuilder GenerateInsertStatement(string connectionString, DataRow index, DataTable column)
		{
			StringBuilder insertStatement = new StringBuilder();
			bool isFirstColumn = true;

			//This is the Identifier Section of the Stored Procedure
			insertStatement.AppendLine("USE [" + connectionString.Substring(connectionString.IndexOf(";") + 17).Substring(0, connectionString.Substring(connectionString.IndexOf(";") + 17).IndexOf(";")) + "]");
			insertStatement.AppendLine("GO");
			insertStatement.AppendLine("SET ANSI_NULLS ON");
			insertStatement.AppendLine("GO");
			insertStatement.AppendLine("SET QUOTED_IDENTIFIER ON");
			insertStatement.AppendLine("GO");
			insertStatement.AppendLine("-- =============================================");
			insertStatement.AppendLine("-- Author: SP_Generator");
			insertStatement.AppendLine("-- Create date: " + DateTime.Now.ToShortDateString());
			insertStatement.AppendLine("-- =============================================");

			insertStatement.AppendLine("CREATE PROCEDURE gensp_Insert_" + index["TableName"].ToString());

			foreach (DataRow columnName in column.Rows)
			{
				string argumentLine = "";
				if (!Convert.ToBoolean(columnName["isIndex"]) && index["TableName"].ToString().Equals(columnName["TableName"].ToString()))
				{
					argumentLine += "@" + columnName["ColumnName"].ToString() + " " + columnName["dataType"].ToString();

					if (columnName["dataType"].ToString().Contains("varchar"))
						argumentLine += " (MAX)";

					argumentLine += " = ";

					if (columnName["dataType"].ToString().Contains("varchar"))
						argumentLine += @"''";
					else if (columnName["dataType"].ToString().Contains("int"))
						argumentLine += "0";
					else if (columnName["dataType"].ToString().Contains("decimal"))
						argumentLine += "0m";
					else if (columnName["dataType"].ToString().Contains("bit"))
						argumentLine += "0";
					else if (columnName["dataType"].ToString().Contains("datetime"))
						argumentLine += "'01-01-0001 00:00:00'";
					else if (columnName["dataType"].ToString().Contains("time"))
						argumentLine += "'00:00:00'";
					else
					{
						Console.WriteLine("dataType not found in application. Please check build and add if needed. Skipping SP Build.");
						Console.WriteLine(columnName["dataType"].ToString());
						Console.WriteLine("");
						return insertStatement.Clear();
					}

					if (isFirstColumn)
					{
						isFirstColumn = false;
						insertStatement.AppendLine("	 " + argumentLine + "");
					}
					else
						insertStatement.AppendLine("	," + argumentLine + "");
				}
			}

			insertStatement.AppendLine("AS");
			insertStatement.AppendLine("BEGIN");
			insertStatement.AppendLine("");
			insertStatement.AppendLine("	SET NOCOUNT ON;");
			insertStatement.AppendLine("");

			insertStatement.AppendLine("");
			insertStatement.AppendLine("INSERT INTO"); 
			insertStatement.AppendLine("	[dbo].[" + index["TableName"].ToString() + "] (");

			isFirstColumn = true;
			foreach (DataRow columnName in column.Rows)
			{
				if (!Convert.ToBoolean(columnName["isIndex"]) && index["TableName"].ToString().Equals(columnName["TableName"].ToString()))
				{
					if (isFirstColumn)
					{
						isFirstColumn = false;
						insertStatement.AppendLine("		 [" + columnName["ColumnName"].ToString() + "]");
					}
					else
						insertStatement.AppendLine("		,[" + columnName["ColumnName"].ToString() + "]");
				}
			}

			insertStatement.AppendLine("	)VALUES(");

			isFirstColumn = true;
			foreach (DataRow columnName in column.Rows)
			{
				if (!Convert.ToBoolean(columnName["isIndex"]) && index["TableName"].ToString().Equals(columnName["TableName"].ToString()))
				{
					if (isFirstColumn)
					{
						isFirstColumn = false;
						insertStatement.AppendLine("		 @" + columnName["ColumnName"].ToString());
					}
					else
						insertStatement.AppendLine("		,@" + columnName["ColumnName"].ToString());
				}
			}

			insertStatement.AppendLine("	)");

			insertStatement.AppendLine("END");
			insertStatement.AppendLine("GO");
			insertStatement.AppendLine("");

			return insertStatement;
		}
	}
}
