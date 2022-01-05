using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Generator
{
	class Pull_SP
	{
		//might expand later to auto add to database
		public static StringBuilder Invoke (string connectionString, DataTable Index, DataTable Column)
		{
			Console.WriteLine("");
			Console.WriteLine("Beginning to Create Pull Statements");
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
				StringBuilder tempBuild = GeneratePullStatement(connectionString, table, column);
				myList.Append(tempBuild);
				Console.Write(tempBuild);
				Console.WriteLine("");
				Console.WriteLine("");
			}

			return myList;
		}

		private static StringBuilder GeneratePullStatement(string connectionString, DataRow index, DataTable column)
		{
			StringBuilder pullStatement = new StringBuilder();
			string ifActive = "";
			string declareLine = "";

			//This is the Identifier Section of the Stored Procedure
			pullStatement.AppendLine("USE [" + connectionString.Substring(connectionString.IndexOf(";") + 17).Substring(0, connectionString.Substring(connectionString.IndexOf(";") + 17).IndexOf(";")) + "]");
			pullStatement.AppendLine("GO");
			pullStatement.AppendLine("SET ANSI_NULLS ON");
			pullStatement.AppendLine("GO");
			pullStatement.AppendLine("SET QUOTED_IDENTIFIER ON");
			pullStatement.AppendLine("GO");
			pullStatement.AppendLine("-- =============================================");
			pullStatement.AppendLine("-- Author: SP_Generator");
			pullStatement.AppendLine("-- Create date: " + DateTime.Now.ToShortDateString());
			pullStatement.AppendLine("-- =============================================");

			pullStatement.AppendLine("CREATE PROCEDURE gensp_Pull_" + index["TableName"].ToString());
			pullStatement.AppendLine("	 @" + index["TableIndex"].ToString() + " int = 0");
			pullStatement.AppendLine("	,@changeUser int = 0");
			pullStatement.AppendLine("	,@recordPull bit = 0");
			pullStatement.AppendLine("	,@dateChanged nvarchar (MAX) = '01-01-0001 00:00:00'");

			foreach (DataRow columnName in column.Rows)
			{
				if (!Convert.ToBoolean(columnName["isIndex"]) && index["TableName"].ToString().Equals(columnName["TableName"].ToString()) 
					&& columnName["ColumnName"].ToString().Contains("active"))
				{
					pullStatement.AppendLine("	,@" + columnName["ColumnName"].ToString() + " bit = 0");
					ifActive = columnName["ColumnName"].ToString();
				}
			}

			pullStatement.AppendLine("AS");
			pullStatement.AppendLine("BEGIN");
			pullStatement.AppendLine("");
			pullStatement.AppendLine("	SET NOCOUNT ON;");
			pullStatement.AppendLine("");

			pullStatement.AppendLine("	IF (@recordPull = 1)");
			pullStatement.AppendLine("		BEGIN");


			declareLine = "			DECLARE	@ParametersUsed as nvarchar = '@" + index["TableIndex"].ToString() + " = ' + @" + index["TableIndex"].ToString() + " ";
			if (!ifActive.Equals(""))
				declareLine += "+ ' @" + ifActive + " = ' + @" + ifActive;

			pullStatement.AppendLine(declareLine);
			
			pullStatement.AppendLine("");
			
			pullStatement.AppendLine("			EXEC	[dbo].[gensp_Insert_changeHistory]");
			pullStatement.AppendLine("					@tableChanged = N'" + index["TableName"].ToString() + "',");
			pullStatement.AppendLine("					@fieldChanged = N'PullRequest', ");
			pullStatement.AppendLine("					@tableIndexID = 0, ");
			pullStatement.AppendLine("					@originalInfo = @ParametersUsed, ");
			pullStatement.AppendLine("					@newInfo = '', ");
			pullStatement.AppendLine("					@userID = @changeUser, ");
			pullStatement.AppendLine("					@dateTimeChanged = @dateChanged");
			pullStatement.AppendLine("		END");
			pullStatement.AppendLine("");

			pullStatement.AppendLine("	IF (@" + index["TableIndex"].ToString() + " IN (SELECT [" + index["TableIndex"].ToString() + "] FROM [dbo].[" 
				+ index["TableName"].ToString() + "]))");
			pullStatement.AppendLine("		BEGIN");
			pullStatement.AppendLine("			SELECT	*");
			pullStatement.AppendLine("			FROM 	[dbo].[" + index["TableName"].ToString() + "]");
			pullStatement.AppendLine("			WHERE	[" + index["TableIndex"].ToString() + "] = @" + index["TableIndex"].ToString() + "");
			pullStatement.AppendLine("");
			pullStatement.AppendLine("			RETURN;");
			pullStatement.AppendLine("		END");
			pullStatement.AppendLine("");

			if (!ifActive.Equals(""))
			{
				pullStatement.AppendLine("	IF (@" + ifActive + " = 1)");
				pullStatement.AppendLine("		BEGIN");
				pullStatement.AppendLine("			SELECT	*");
				pullStatement.AppendLine("			FROM 	[dbo].[" + index["TableName"].ToString() + "]");
				pullStatement.AppendLine("			WHERE	[" + ifActive + "] = 1");
				pullStatement.AppendLine("");
				pullStatement.AppendLine("			RETURN;");
				pullStatement.AppendLine("		END");
				pullStatement.AppendLine("");
			}

			pullStatement.AppendLine("");
			pullStatement.AppendLine("	SELECT	*");
			pullStatement.AppendLine("	FROM 	[dbo].[" + index["TableName"].ToString() + "]");
			pullStatement.AppendLine("");

			pullStatement.AppendLine("END");
			pullStatement.AppendLine("GO");
			pullStatement.AppendLine("");

			return pullStatement;
		}
	}
}
