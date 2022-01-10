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
		public static StringBuilder Invoke (string connectionString, DataTable Index)
		{
			Console.WriteLine("");
			Console.WriteLine("Beginning to Create Pull Statements");
			Console.WriteLine("");
			Console.WriteLine("");
			Console.WriteLine("Press Enter to Begin.");
			Console.WriteLine("");
			Console.ReadLine();
			StringBuilder output = GenerateList(connectionString, Index);

			return output;
		}

		private static StringBuilder GenerateList(string connectionString, DataTable index)
		{
			StringBuilder myList = new StringBuilder();

			foreach(DataRow table in index.Rows)
			{
				StringBuilder tempBuild = GeneratePullStatement(connectionString, table);
				myList.Append(tempBuild);
				Console.Write(tempBuild);
				Console.WriteLine("");
				Console.WriteLine("");
			}

			return myList;
		}

		private static StringBuilder GeneratePullStatement(string connectionString, DataRow index)
		{
			StringBuilder pullStatement = new StringBuilder();

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

			pullStatement.AppendLine("AS");
			pullStatement.AppendLine("BEGIN");
			pullStatement.AppendLine("");
			pullStatement.AppendLine("	SET NOCOUNT ON;");
			pullStatement.AppendLine("");

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
