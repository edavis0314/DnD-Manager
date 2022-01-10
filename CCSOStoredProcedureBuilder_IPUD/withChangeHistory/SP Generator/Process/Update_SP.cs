using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Generator
{
	class Update_SP
	{
		//might expand later to auto add to database
		public static StringBuilder Invoke (string connectionString, DataTable Index, DataTable Column)
		{
			Console.WriteLine("");
			Console.WriteLine("Beginning to Create Update Statements");
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
				StringBuilder tempBuild = GenerateUpdateStatement(connectionString, table, column);
				myList.Append(tempBuild);
				Console.Write(tempBuild);
				Console.WriteLine("");
				Console.WriteLine("");
			}

			return myList;
		}

		private static StringBuilder GenerateUpdateStatement(string connectionString, DataRow index, DataTable column)
		{
			StringBuilder updateStatement = new StringBuilder();
			bool isFirstColumn = true;

			//This is the Identifier Section of the Stored Procedure
			updateStatement.AppendLine("USE [" + connectionString.Substring(connectionString.IndexOf(";") + 17).Substring(0, connectionString.Substring(connectionString.IndexOf(";") + 17).IndexOf(";")) + "]");
			updateStatement.AppendLine("GO");
			updateStatement.AppendLine("SET ANSI_NULLS ON");
			updateStatement.AppendLine("GO");
			updateStatement.AppendLine("SET QUOTED_IDENTIFIER ON");
			updateStatement.AppendLine("GO");
			updateStatement.AppendLine("-- =============================================");
			updateStatement.AppendLine("-- Author: SP_Generator");
			updateStatement.AppendLine("-- Create date: " + DateTime.Now.ToShortDateString());
			updateStatement.AppendLine("-- =============================================");

			updateStatement.AppendLine("CREATE PROCEDURE gensp_Update_" + index["TableName"].ToString());

			foreach (DataRow columnName in column.Rows)
			{
				string argumentLine = "";
				if (index["TableName"].ToString().Equals(columnName["TableName"].ToString()))
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
						return updateStatement.Clear();
					}

					if (isFirstColumn)
					{
						isFirstColumn = false;
						updateStatement.AppendLine("	 " + argumentLine + "");
					}
					else
						updateStatement.AppendLine("	," + argumentLine + "");
				}
			}

			updateStatement.AppendLine("	,@dateChanged datetime = '01-01-0001 00:00:00'");
			updateStatement.AppendLine("	,@changeUser int = 0");

			updateStatement.AppendLine("AS");
			updateStatement.AppendLine("BEGIN");
			updateStatement.AppendLine("");
			updateStatement.AppendLine("	SET NOCOUNT ON;");
			updateStatement.AppendLine("");

			updateStatement.AppendLine("	IF	(@" + index["TableIndex"].ToString() + " in (SELECT [" + index["TableIndex"].ToString()
					+ "] FROM [dbo].[" + index["TableName"].ToString() + "]))");
			updateStatement.AppendLine("		BEGIN");

			foreach (DataRow columnName in column.Rows)
			{
				if (index["TableName"].ToString().Equals(columnName["TableName"].ToString()))
				{
					if (index["TableName"].ToString().Equals(columnName["TableName"].ToString()) && !(index["TableName"].ToString().Equals("dateAdded") || index["TableName"].ToString().Equals("createdUser")))
					{
						updateStatement.AppendLine("			IF	(NOT @" + columnName["ColumnName"].ToString() + " = (SELECT [" + columnName["ColumnName"].ToString()
							+ "] FROM [dbo].[" + index["TableName"].ToString() + "] WHERE [" + index["TableIndex"].ToString() + "] = @" + index["TableIndex"].ToString()
							+ ") AND NOT @" + columnName["ColumnName"].ToString() + " = '' )");
						updateStatement.AppendLine("				BEGIN");

						updateStatement.AppendLine("					DECLARE @output" + columnName["ColumnName"].ToString() + " as nvarchar (MAX) = (SELECT [" + columnName["ColumnName"].ToString()
							+ "] FROM [dbo].[" + index["TableName"].ToString() + "] WHERE [" + index["TableIndex"].ToString() + "] = @" + index["TableIndex"].ToString()
							+ ")");
						updateStatement.AppendLine("					EXEC	[dbo].[gensp_Insert_changeHistory]");
						updateStatement.AppendLine("							@tableChanged = N'" + index["TableName"].ToString() + "',");
						updateStatement.AppendLine("							@fieldChanged = N'" + columnName["ColumnName"].ToString() + "', ");
						updateStatement.AppendLine("							@tableIndexID = @" + index["TableIndex"].ToString() + ", ");
						updateStatement.AppendLine("							@originalInfo = @output" + columnName["ColumnName"].ToString() + ", ");
						updateStatement.AppendLine("							@newInfo = @" + columnName["ColumnName"].ToString() + ", ");
						updateStatement.AppendLine("							@userID = @changeUser, ");
						updateStatement.AppendLine("							@dateTimeChanged = @dateChanged");

						updateStatement.AppendLine("				END");
						updateStatement.AppendLine("");
					}
				}
			}
			



			updateStatement.AppendLine("");
			updateStatement.AppendLine("			UPDATE	 [dbo].[" + index["TableName"].ToString() + "] ");

			isFirstColumn = true;
			foreach (DataRow columnName in column.Rows)
			{
				string argumentLine = "";
				if (index["TableName"].ToString().Equals(columnName["TableName"].ToString()) && !(index["TableName"].ToString().Equals("dateAdded") || index["TableName"].ToString().Equals("createdUser")))
				{
					if (!Convert.ToBoolean(columnName["isIndex"]) && index["TableName"].ToString().Equals(columnName["TableName"].ToString()))
					{
						argumentLine += "[" + columnName["ColumnName"].ToString() + "] = CASE WHEN NOT @" + columnName["ColumnName"].ToString() + " = " +
							"[" + columnName["ColumnName"].ToString() + "] AND NOT @" + columnName["ColumnName"].ToString() + " = ";


						if (columnName["dataType"].ToString().Contains("varchar"))
							argumentLine += @"''";
						else if (columnName["dataType"].ToString().Contains("int"))
							argumentLine += "0";
						else if (columnName["dataType"].ToString().Contains("decimal"))
							argumentLine += "0m";
						else if (columnName["dataType"].ToString().Contains("bit"))
							argumentLine = "[" + columnName["ColumnName"].ToString() + "] = CASE WHEN NOT @" + columnName["ColumnName"].ToString() + " = [" + columnName["ColumnName"].ToString() + "]";
						else if (columnName["dataType"].ToString().Contains("datetime"))
							argumentLine += "'01-01-0001 00:00:00'";
						else if (columnName["dataType"].ToString().Contains("time"))
							argumentLine += "'00:00:00'";
						else
						{
							Console.WriteLine("dataType not found in application. Please check build and add if needed. Skipping SP Build.");
							Console.WriteLine(columnName["dataType"].ToString());
							Console.WriteLine("");
							return updateStatement.Clear();
						}

						argumentLine += " THEN @" + columnName["ColumnName"].ToString() + " ELSE [" + columnName["ColumnName"].ToString() + "] END";

						if (isFirstColumn)
						{
							isFirstColumn = false;
							updateStatement.AppendLine("			SET		 " + argumentLine + "");
						}
						else
							updateStatement.AppendLine("					," + argumentLine + "");
					}
				}
			}

			updateStatement.AppendLine("			WHERE	 [" + index["TableIndex"].ToString() + "] = @" + index["TableIndex"].ToString());

			updateStatement.AppendLine("		END");
			updateStatement.AppendLine("END");
			updateStatement.AppendLine("GO");
			updateStatement.AppendLine("");

			return updateStatement;
		}
	}
}
