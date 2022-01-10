using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SP_Generator
{
	static class Program
	{
		static void Main()
		{
			bool isNotComplete = true;
			
			while (isNotComplete)
			{
				DataTable tableIndex;
				DataTable tableColumn;
				string connectionString = EstablishConnectionString();

				tableIndex = DefineTable.Index(connectionString);
				tableColumn = DefineTable.Column(connectionString);

				if (tableColumn == new DataTable() || tableIndex == new DataTable())
				{
					Console.WriteLine("");
					Console.WriteLine("Tables did not find information needed. Please confirm that the Tables have Index Values and all columns are filled out before continuing. ");
				}
				else
				{
					StringBuilder storedProcedures = new StringBuilder();
					storedProcedures.Append(Insert_SP.Invoke(connectionString, tableIndex, tableColumn));
					storedProcedures.Append(Pull_SP.Invoke(connectionString, tableIndex));
					storedProcedures.Append(Deactivate_SP.Invoke(connectionString, tableIndex, tableColumn));
					storedProcedures.Append(Update_SP.Invoke(connectionString, tableIndex, tableColumn));

					Console.WriteLine("");
					Console.WriteLine("");
					Console.WriteLine("Please Enter the location you want the file stored.");
					string filePath = Console.ReadLine() + @"\SP_Output" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
					File.WriteAllText(@filePath, storedProcedures.ToString());
					Console.WriteLine("");
					Console.WriteLine("Printed to file location: " + @filePath);
					Console.WriteLine("");
					Console.WriteLine("");

				}

				//This is for closing or restarting the process
				Console.WriteLine("All process have been completed.");
				Console.WriteLine("Press 'Y' to run again or anything else to close the application. ");
				string exitCode = Console.ReadLine();
				if (!(exitCode.Equals("Y") || exitCode.Equals("y")))
				{
					Console.WriteLine("Exiting");
					isNotComplete = false;
				}

				Thread.Sleep(2500);
			}
		}

		private static string EstablishConnectionString()
		{
			string serverName;
			string dbName;
			string username;
			string password;
			string connectionString;
			bool isAbleToConnect;

			do
			{
				Console.WriteLine("Username: ");
				username = Console.ReadLine();
				Console.WriteLine("");

				Console.WriteLine("Password: ");
				password = "";
				while (true)
				{
					var key = Console.ReadKey(true);
					if (key.Key == ConsoleKey.Enter)
						break;

					if (key.Key == ConsoleKey.Backspace)
						password = password.Substring(0,password.Length-1);
					else
						password += key.KeyChar;
				}
				Console.WriteLine("");
				Console.WriteLine("");

				Console.WriteLine("Which Server? ");
				serverName = Console.ReadLine();
				Console.WriteLine("");

				Console.WriteLine("Which Database? ");
				dbName = Console.ReadLine();
				Console.WriteLine("");

				connectionString = "Data Source=" + serverName + ";Initial Catalog=" + dbName + ";User id=" + username + ";password=" + password + ";MultipleActiveResultSets=True";
				isAbleToConnect = CheckServer(connectionString);

			} while (!isAbleToConnect);

			Console.WriteLine("Connection Established. Starting to create Stored Procedures Now. ");

			return connectionString;
		}

		private static bool CheckServer(string dbConnection)
		{
			bool isAbleToConnect = false;

			try
			{
				SqlConnection connection = new SqlConnection(dbConnection);
				Console.WriteLine("Press Enter to Continue. ");
				Console.ReadLine();
				Console.WriteLine("");
				connection.Open();

				isAbleToConnect = true;
				
			} 
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			return isAbleToConnect;
		}
	}
}
