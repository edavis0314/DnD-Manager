using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Generator
{
	class CSharpObjects
	{
		//might expand later to auto add to database
		public static StringBuilder Invoke(DataTable Index, DataTable Column)
		{
			Console.WriteLine("");
			Console.WriteLine("Beginning to Create Object Files");
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
				StringBuilder tempBuild = GenerateObjects(table, column);
				myList.Append(tempBuild);
				Console.Write(tempBuild);
				Console.WriteLine("");
				Console.WriteLine("");
			}

			return myList;
		}

		private static StringBuilder GenerateObjects(DataRow index, DataTable column)
		{
			StringBuilder objects = new StringBuilder();

			objects.AppendLine("using System;");
			objects.AppendLine("using System.Data;");
			objects.AppendLine("");
			objects.AppendLine("namespace CCSOTicketManager.Objects");
			objects.AppendLine("{");
			objects.AppendLine("	public class " + CapitalFirstLetter(index["TableName"].ToString()));
			objects.AppendLine("	{");
			objects.AppendLine("		//Variable Declarations");

			foreach (DataRow columnName in column.Rows)
			{
				string argumentLine = "private ";
				if (index["TableName"].ToString().Equals(columnName["TableName"].ToString()))
				{
					if (columnName["dataType"].ToString().Contains("varchar"))
						argumentLine += "string ";
					else if (columnName["dataType"].ToString().Contains("int"))
						argumentLine += "int ";
					else if (columnName["dataType"].ToString().Contains("decimal"))
						argumentLine += "decimal ";
					else if (columnName["dataType"].ToString().Contains("bit"))
						argumentLine += "bool ";
					else if (columnName["dataType"].ToString().Contains("datetime"))
						argumentLine += "DateTime ";
					else if (columnName["dataType"].ToString().Contains("time"))
						argumentLine += "DateTime";
					else
					{
						Console.WriteLine("dataType not found in application. Please check build and add if needed. Skipping SP Build.");
						Console.WriteLine(columnName["dataType"].ToString());
						Console.WriteLine("");
						return objects.Clear();
					}

					argumentLine += columnName["ColumnName"].ToString();
					argumentLine += ";";

					objects.AppendLine("			" + argumentLine);
				}
			}

			objects.AppendLine("");
			objects.AppendLine("		//Default Constructor");
			objects.AppendLine("		public " + CapitalFirstLetter(index["TableName"].ToString()) + "()");
			objects.AppendLine("		{");

			foreach (DataRow columnName in column.Rows)
			{
				string argumentLine = "this.";
				if (index["TableName"].ToString().Equals(columnName["TableName"].ToString()))
				{
					argumentLine += columnName["ColumnName"].ToString();
					argumentLine += " = ";

					if (columnName["dataType"].ToString().Contains("varchar"))
						argumentLine += @"""";
					else if (columnName["dataType"].ToString().Contains("int"))
						argumentLine += "0";
					else if (columnName["dataType"].ToString().Contains("decimal"))
						argumentLine += "0m";
					else if (columnName["dataType"].ToString().Contains("bit"))
						argumentLine += "false";
					else if (columnName["dataType"].ToString().Contains("datetime"))
						argumentLine += "new DateTime()";
					else if (columnName["dataType"].ToString().Contains("time"))
						argumentLine += "new DateTime()";
					else
					{
						Console.WriteLine("dataType not found in application. Please check build and add if needed. Skipping SP Build.");
						Console.WriteLine(columnName["dataType"].ToString());
						Console.WriteLine("");
						return objects.Clear();
					}


					argumentLine += ";";

					objects.AppendLine("			" + argumentLine);
				}
			}

			objects.AppendLine("		}");
			objects.AppendLine("");
			objects.AppendLine("		public " + CapitalFirstLetter(index["TableName"].ToString()) + "(" + CapitalFirstLetter(index["TableName"].ToString()) + " Copy)");
			objects.AppendLine("		{");

			foreach (DataRow columnName in column.Rows)
			{
				string argumentLine = "this.";
				if (index["TableName"].ToString().Equals(columnName["TableName"].ToString()))
				{
					argumentLine += columnName["ColumnName"].ToString();
					argumentLine += " = Copy.";
					argumentLine += CapitalFirstLetter(columnName["ColumnName"].ToString());
					argumentLine += ";";

					objects.AppendLine("		" + argumentLine);
				}
			}

			objects.AppendLine("		}");
			objects.AppendLine("");

			objects.AppendLine("		//Variable Public Declarations");
			objects.AppendLine("		#region Public Declarations");

			foreach (DataRow columnName in column.Rows)
			{
				string argumentLine = "public ";
				if (index["TableName"].ToString().Equals(columnName["TableName"].ToString()))
				{
					if (columnName["dataType"].ToString().Contains("varchar"))
						argumentLine += "string ";
					else if (columnName["dataType"].ToString().Contains("int"))
						argumentLine += "int ";
					else if (columnName["dataType"].ToString().Contains("decimal"))
						argumentLine += "decimal ";
					else if (columnName["dataType"].ToString().Contains("bit"))
						argumentLine += "bool ";
					else if (columnName["dataType"].ToString().Contains("datetime"))
						argumentLine += "DateTime ";
					else if (columnName["dataType"].ToString().Contains("time"))
						argumentLine += "DateTime";
					else
					{
						Console.WriteLine("dataType not found in application. Please check build and add if needed. Skipping SP Build.");
						Console.WriteLine(columnName["dataType"].ToString());
						Console.WriteLine("");
						return objects.Clear();
					}

					argumentLine += CapitalFirstLetter(columnName["ColumnName"].ToString());
					objects.AppendLine("		" + argumentLine);

					objects.AppendLine("		{");
					objects.AppendLine("			get");
					objects.AppendLine("			{");
					objects.AppendLine("			return this." + columnName["ColumnName"].ToString() + ";");
					objects.AppendLine("			}");
					objects.AppendLine("			Set");
					objects.AppendLine("			{");
					objects.AppendLine("			this." + columnName["ColumnName"].ToString() + " = value;");
					objects.AppendLine("			}");
					objects.AppendLine("		}");
					objects.AppendLine("");
				}
			}

			objects.AppendLine("");
			objects.AppendLine("		#endregion");



			objects.AppendLine("");
			objects.AppendLine("		#region Public Declarations");
			objects.AppendLine("		public void LoadObject (DataRow myRow)");
			objects.AppendLine("		{");

			foreach (DataRow columnName in column.Rows)
			{
				string argumentLine = "this." + columnName["columnName"].ToString() + " = ";
				if (index["TableName"].ToString().Equals(columnName["TableName"].ToString()))
				{
					if (columnName["dataType"].ToString().Contains("varchar"))
						argumentLine += "myRow[\"" + columnName["columnName"].ToString() + "\"].ToString();";
					else if (columnName["dataType"].ToString().Contains("int"))
						argumentLine += "Convert.ToInt32(myRow[\"" + columnName["columnName"].ToString() + "\"].ToString());";
					else if (columnName["dataType"].ToString().Contains("decimal"))
						argumentLine += "Convert.ToDecimal(myRow[\"" + columnName["columnName"].ToString() + "\"].ToString());";
					else if (columnName["dataType"].ToString().Contains("bit"))
						argumentLine += "Convert.ToBoolean(myRow[\"" + columnName["columnName"].ToString() + "\"].ToString());";
					else if (columnName["dataType"].ToString().Contains("datetime"))
						argumentLine += "Convert.ToDateTime(myRow[\"" + columnName["columnName"].ToString() + "\"].ToString());";
					else if (columnName["dataType"].ToString().Contains("time"))
						argumentLine += "Convert.ToDateTime(myRow[\"" + columnName["columnName"].ToString() + "\"].ToString());";
					else
					{
						Console.WriteLine("dataType not found in application. Please check build and add if needed. Skipping SP Build.");
						Console.WriteLine(columnName["dataType"].ToString());
						Console.WriteLine("");
						return objects.Clear();
					}

					objects.AppendLine("		" + argumentLine);
				}
			}

			objects.AppendLine("		}");
			objects.AppendLine("		#endregion");



			objects.AppendLine("	}");
			objects.AppendLine("}");

			return objects;
		}

		private static string CapitalFirstLetter(string temp)
		{
			string newTemp = temp.Substring(0, 1).ToUpper() + temp.Substring(1);
			return newTemp;
		}
	}
}
