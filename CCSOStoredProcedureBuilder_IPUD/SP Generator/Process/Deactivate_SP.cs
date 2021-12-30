using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Generator
{
	class Deactivate_SP
	{
		//might expand later to auto add to database
		public static StringBuilder Invoke (string connectionString, DataTable Index, DataTable column)
		{
			Console.WriteLine("");
			Console.WriteLine("Beginning to Create deactivate Statements");
			Console.WriteLine("");
			Console.WriteLine("");
			Console.WriteLine("Press Enter to Begin.");
			Console.WriteLine("");
			Console.ReadLine();
			StringBuilder output = GenerateList(connectionString, Index, column);

			return output;
		}

		private static StringBuilder GenerateList(string connectionString, DataTable index, DataTable column)
		{
			StringBuilder myList = new StringBuilder();

			foreach(DataRow table in index.Rows)
			{
				foreach (DataRow columnName in column.Rows)
				{
					if (table["tableName"].ToString().Equals(columnName["tableName"].ToString()) && columnName["columnName"].ToString().ToLower().Equals("active"))
					{
						StringBuilder tempBuild = GenerateDeactivateStatement(connectionString, table);
						myList.Append(tempBuild);
						Console.Write(tempBuild);
						Console.WriteLine("");
						Console.WriteLine("");

						tempBuild.Clear();
						tempBuild = GenerateActivateStatement(connectionString, table);
						myList.Append(tempBuild);
						Console.Write(tempBuild);
						Console.WriteLine("");
						Console.WriteLine("");
					}
				}
			}

			return myList;
		}

		private static StringBuilder GenerateDeactivateStatement(string connectionString, DataRow index)
		{
			StringBuilder deactivateStatement = new StringBuilder();

			//This is the Identifier Section of the Stored Procedure
			deactivateStatement.AppendLine("USE [" + connectionString.Substring(connectionString.IndexOf(";") + 17).Substring(0, connectionString.Substring(connectionString.IndexOf(";") + 17).IndexOf(";")) + "]");
			deactivateStatement.AppendLine("GO");
			deactivateStatement.AppendLine("SET ANSI_NULLS ON");
			deactivateStatement.AppendLine("GO");
			deactivateStatement.AppendLine("SET QUOTED_IDENTIFIER ON");
			deactivateStatement.AppendLine("GO");
			deactivateStatement.AppendLine("-- =============================================");
			deactivateStatement.AppendLine("-- Author: SP_Generator");
			deactivateStatement.AppendLine("-- Create date: " + DateTime.Now.ToShortDateString());
			deactivateStatement.AppendLine("-- =============================================");

			deactivateStatement.AppendLine("CREATE PROCEDURE gensp_Deactivate_" + index["TableName"].ToString());

			deactivateStatement.AppendLine("	 @" + index["tableIndex"].ToString() + " int = 0");
			deactivateStatement.AppendLine("AS");
			deactivateStatement.AppendLine("BEGIN");
			deactivateStatement.AppendLine("");
			deactivateStatement.AppendLine("	SET NOCOUNT ON;");
			deactivateStatement.AppendLine("");

			deactivateStatement.AppendLine("");
			deactivateStatement.AppendLine("	UPDATE	[dbo].[" + index["TableName"].ToString() + "] ");
			deactivateStatement.AppendLine("	SET 	active = 0 ");
			deactivateStatement.AppendLine("	WHERE 	" + index["tableIndex"].ToString() + " = @" + index["tableIndex"].ToString());
			deactivateStatement.AppendLine("");

			deactivateStatement.AppendLine("END");
			deactivateStatement.AppendLine("GO");
			deactivateStatement.AppendLine("");

			return deactivateStatement;
		}

		private static StringBuilder GenerateActivateStatement(string connectionString, DataRow index)
		{
			StringBuilder activateStatement = new StringBuilder();

			//This is the Identifier Section of the Stored Procedure
			activateStatement.AppendLine("USE [" + connectionString.Substring(connectionString.IndexOf(";") + 17).Substring(0, connectionString.Substring(connectionString.IndexOf(";") + 17).IndexOf(";")) + "]");
			activateStatement.AppendLine("GO");
			activateStatement.AppendLine("SET ANSI_NULLS ON");
			activateStatement.AppendLine("GO");
			activateStatement.AppendLine("SET QUOTED_IDENTIFIER ON");
			activateStatement.AppendLine("GO");
			activateStatement.AppendLine("-- =============================================");
			activateStatement.AppendLine("-- Author: SP_Generator");
			activateStatement.AppendLine("-- Create date: " + DateTime.Now.ToShortDateString());
			activateStatement.AppendLine("-- =============================================");

			activateStatement.AppendLine("CREATE PROCEDURE gensp_Activate_" + index["TableName"].ToString());

			activateStatement.AppendLine("	 @" + index["tableIndex"].ToString() + " int = 0");
			activateStatement.AppendLine("AS");
			activateStatement.AppendLine("BEGIN");
			activateStatement.AppendLine("");
			activateStatement.AppendLine("	SET NOCOUNT ON;");
			activateStatement.AppendLine("");

			activateStatement.AppendLine("");
			activateStatement.AppendLine("	UPDATE	[dbo].[" + index["TableName"].ToString() + "] ");
			activateStatement.AppendLine("	SET 	active = 1 ");
			activateStatement.AppendLine("	WHERE 	" + index["tableIndex"].ToString() + " = @" + index["tableIndex"].ToString());
			activateStatement.AppendLine("");

			activateStatement.AppendLine("END");
			activateStatement.AppendLine("GO");
			activateStatement.AppendLine("");

			return activateStatement;
		}
	}
}
