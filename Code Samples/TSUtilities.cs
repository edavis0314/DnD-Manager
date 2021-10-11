using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows;
using Microsoft.Exchange.WebServices.Data;
using System.Collections;
using System.Security.Cryptography;
using System.Globalization;
using System.Text.RegularExpressions;
using SimpleImpersonation;
using Microsoft.Ajax.Utilities;
using System.Net.Sockets;
using System.Net;
using System.Data.OleDb;
using CCSOUtils;

namespace CCSOTimeSheet
{
	public class TSUtilities
	{
		#region Sorting Tools
		//method that gets passed a list of Tssegments for a specific day to be resorted by times.  *****Used to sort for FTO validation
		static public List<TSSegment> SortDayByTime(List<TSSegment> day)
		{
			List<TSSegment> result = day.OrderBy(o => o.StartTime).ToList();

			return result;
		}
		//method that gets passed a list of objects of employee info. *****Used to sort the results for the ddl
		static public List<EmployeeInfo> SortMyList_ApprovalsDDL(DataTable inTable)
		{
			List<EmployeeInfo> inList = new List<EmployeeInfo>();
			List<string> tempNames = new List<string>();
			List<string> names = new List<string>();
			foreach (DataRow row in inTable.Rows)
			{
				EmployeeInfo myEntry = new EmployeeInfo
				{
					MemberName = row["Name"].ToString().Trim()
				};
				string tempName = myEntry.MemberName;
				tempNames.Add(tempName);

				myEntry.MemberOSN = row["OSN"].ToString().Trim();
				myEntry.PosNumber = row["Permission"].ToString().Trim(); // This position number only contains the Department and Division they are and thier rank

				if (tempNames.Contains(tempName) && !names.Contains(tempName))
				{
					names.Add(tempName);
					inList.Add(myEntry);
				}
			}

			inList = inList.OrderBy(o => o.MemberName).ToList();
			inList = inList.OrderBy(o => o.PosNumber).ToList();
			return inList;
		}
		#endregion


		#region Finders
		//This is used find the start date of a timesheet if none are found
		static public string FindTimesheetStartDate(string jobType, string currentDate)
		{
			bool isLeapYear = false;
			int difference = 0;
			string dateCheck = "12/06/2019";
			if (jobType.Equals("Comm"))
			{
				dateCheck = "12/08/2019";
			}
			isLeapYear = DateTime.IsLeapYear(DateTime.Now.Year);
			difference = Convert.ToDateTime(currentDate).Subtract(Convert.ToDateTime(dateCheck)).Days;
			if (isLeapYear && (difference / 14) % 1 != 0)
			{
				difference++;
			}

			difference /= 14;
			string myStart = Convert.ToDateTime(dateCheck).AddDays((difference * 14)).ToShortDateString();
			return myStart;
		}

		#endregion


		#region Checks
		//This is the section for general methods that handle any and all non defined functionality
		//The Following are a set of methods that handle statuses for visability on the page and Text that appears 
		//in special occuations.
		static public bool CheckLeave(string test)
		{
			if (test != null)
			{
				if (test.Equals("Leave"))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		//small function to see if the passed in string is equal to OT
		static public bool CheckOT(string test, string stateCode)
		{
			if (test != null && !stateCode.Equals(""))
			{
				if (stateCode.Substring(5, 1).Equals("0"))
				{
					if (test.Equals("OT"))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		static public bool CheckFTO(string test, string stateCode)
		{
			if (test != null && !stateCode.Equals(""))
			{
				if (stateCode.Substring(5, 1).Equals("0"))
				{
					if (test.Equals("FTO"))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		static public bool CheckexReg(string test, string stateCode)
		{
			if (test != null && !stateCode.Equals(""))
			{
				if (stateCode.Substring(5, 1).Equals("0"))
				{
					if (test.Equals("exReg"))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		//small method to verify if a segment is expanded or not
		static public bool CheckExpand(string test, string stateCode)
		{
			if (test != null)
			{
				if (test.Equals("OT"))
				{
					if (stateCode.Substring(4, 1).Equals("1") && stateCode.Substring(5, 1).Equals("0"))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		static public bool CheckExpandFTO(string test, string stateCode)
		{
			if (test != null)
			{
				if (test.Equals("FTO"))
				{
					if (stateCode.Substring(4, 1).Equals("1") && stateCode.Substring(5, 1).Equals("0"))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		static public bool CheckExpandexReg(string test, string stateCode)
		{
			if (test != null)
			{
				if (test.Equals("exReg"))
				{
					if (stateCode.Substring(4, 1).Equals("1") && stateCode.Substring(5, 1).Equals("0"))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		//method that works with the CheckExpand method for expanding and collapsing segment entries
		static public string CheckTextExpand(string test)
		{
			if (test != null)
			{
				if (test.Substring(4, 1).Equals("1"))
				{
					return "Collapse";
				}
				else
				{
					return "Expand";
				}
			}
			else
			{
				return "Expand";
			}
		}

		//method that checks if a entry needs to be shown or not
		static public bool CheckDeleted(string stateCode)
		{
			if (stateCode.Substring(5, 1).Equals("1"))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		//small function to check if something was approved
		//add to these comments to specify what might be approved
		static public string CheckApproved(string test, string payoutCode = "")
		{
			string temp = test.Substring(1, 1); // supervisor approval
			string tempPremium = test.Substring(8, 1); // director approval
			if (payoutCode != null)
			{
				if (payoutCode.Equals("Premium Pay"))
				{
					if (temp.Equals("1") && tempPremium.Equals("1"))
					{
						return "Approved";
					}
					else if (tempPremium.Equals("0") && temp.Equals("1"))
					{
						return "Pending Director Approval";
					}
					else if (tempPremium.Equals("1") && temp.Equals("0"))
					{
						return "Pending Supervisor Approval";
					}
					else
					{
						return "Not Approved";
					}
				}
			}

			if (temp != null)
			{
				if (temp.Equals("1"))
				{
					return "Approved";
				}
				else
				{
					return "Not Approved";
				}
			}
			else
			{
				return "Not Approved";
			}
		}

		static public string CheckStatus(Page currentPage)
		{
			using (WindowsIdentity.Impersonate(IntPtr.Zero))
			{
				//This code executes under app pool user
				string access = "";
				List<string> emailList = new List<string>();
				string username = currentPage.User.Identity.Name.Replace(@"CCSO\", "").ToUpper();
				string serverPath = "\\\\shares\\TimesheetAttach$\\TS_Config\\HREmails.txt";
				bool isMaintenanceMode = false;

				if (ConfigurationManager.AppSettings["MaintenanceMode"].ToString().Equals("On"))
				{
					isMaintenanceMode = true;
				}

				ArrayList groups = TSADGroups.Groups(username.Substring(username.IndexOf("\\") + 1));

				if (!isMaintenanceMode)
				{
					Impersonate.ImpersonateUser();
					var emails = File.ReadAllLines(serverPath).Where(line => line.Contains("@"));
					foreach (string emailAddress in emails)
					{
						emailList.Add(emailAddress);
					}
					Impersonate.impersonationContext.Undo();

					//lbluserName.Text = userName1;
					//Session["Access"] = "Denied";

					foreach (string users in emailList)
					{
						if (users.Contains(username))
						{
							access = "HRER";
						}
					}

					for (int y = 0; y < groups.Count; y++)
					{
						//if (groups[y].ToString().Contains("Financial Services"))//need group name to look for to be able to edit
						//groups[y].ToString().Contains("Information Technology Unit")

						if (groups[y].ToString().Contains("Payroll") /*|| groups[y].ToString().Contains("Information Technology Unit")*/)//need group name to look for to be able to edit
						{
							access = "Payroll";
						}
						if ((groups[y].ToString().Contains("Developer Unit") && !currentPage.Request.Url.ToString().Contains("iis2016")))
						{
							access = "Developer";
						}
						if ((groups[y].ToString().Contains("Developer Unit") || groups[y].ToString().Contains("ittechs")) && currentPage.Request.Url.ToString().Contains("iis2016"))
						{
							access = "ITTech";
						}
						if (groups[y].ToString().Contains("Command Staff Assistants"))
						{
							access = "StaffAssistant";
						}
					}
					return access;
				}
				else
				{
					for (int y = 0; y < groups.Count; y++)
					{
						if (groups[y].ToString().Contains("Developer Unit"))
						{
							access = "Developer";
							break;
						}
						else
						{
							access = "Denied";
						}
					}

					return access;
				}
			}
		}

		//This method is to check and see if the time entered into the modal is valid based on hire and term dates
		public static bool CheckEmploymentStatus(string start, string end, TimeSheet myTimesheet)
		{
			DateTime term = myTimesheet.MemberInforamtion.TermDate;
			DateTime hire = myTimesheet.MemberInforamtion.HireDate;
			bool checkStart = false;
			bool checkEnd = false;

			if (DateTime.TryParse(start, out DateTime startDate))
			{
				if (hire <= startDate && term >= startDate)
				{
					checkStart = true;
				}
			}
			else
			{
				checkStart = true;
			}

			if (DateTime.TryParse(end, out DateTime endDate))
			{
				if (hire <= endDate && term >= endDate)
				{
					checkEnd = true;
				}
			}
			else
			{
				checkEnd = true;
			}

			if (checkStart && checkEnd)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static string CheckTimeSheetStatus(string stateCode)
		{
			string timeSheetStatus = "";
			string hexStateCode = "";

			if (stateCode.Equals("LOCKED"))
			{
				timeSheetStatus = "Locked by Payroll";
			}
			else
			{
				hexStateCode = ConvertToHex(stateCode);

				if (hexStateCode.Equals("01"))
				{
					timeSheetStatus = " New Time Sheet";
				}
				else if (hexStateCode.Equals("00"))
				{
					timeSheetStatus = " Unsubmitted";
				}
				else if (hexStateCode.Equals("80"))
				{
					timeSheetStatus = " Submitted";
				}
				else if (hexStateCode.Equals("10"))
				{
					timeSheetStatus = " Rejected, please resubmit";
				}
				else if (hexStateCode.Equals("E0"))
				{
					timeSheetStatus = " Under review by Payroll";
				}
				else if (hexStateCode.Equals("C0"))
				{
					timeSheetStatus = " Approved by Supervisor";
				}
				else if (hexStateCode.Substring(1, 1).Equals("8"))
				{
					timeSheetStatus = " Approved by Payroll";
				}
			}

			return timeSheetStatus;
		}

		public static void CheckIfActiveUser(string user)
		{

		}
		#endregion


		#region Convert
		//This section is for all methods that manimulate data to conform to our standards
		//converts our state codes into binary.
		//takes a passed in string and if there is no exception it will return a binary value in string format.
		static public string ConvertToBinary(string stateCode)
		{
			try
			{
				StringBuilder result = new StringBuilder();
				foreach (char c in stateCode)
				{
					// This will crash for non-hex characters. You might want to handle that differently.
					result.Append(TSDictionary.HexCharacterToBinary[char.ToUpper(c)]);
				}
				return result.ToString();
			}
			catch (Exception E)
			{
				Debug.WriteLine(E);
				return "null";
			}
		}

		//converts our state codes into hex
		//takes a passed in string and if there is no exception it will return a hexidecimal value in string format
		static public string ConvertToHex(string stateCode)
		{
			try
			{
				StringBuilder result = new StringBuilder();
				for (int c = 0; c < stateCode.Length; c++)
				{
					// This will crash for non-hex characters. You might want to handle that differently.
					// I know that I had a comment noting that, must of deleted it.
					result.Append(TSDictionary.BinaryStringToHex[stateCode.Substring(c, 4)]);
					c += 3;
				}
				return result.ToString();
			}
			catch (Exception E)
			{
				Debug.WriteLine(E.Message + " ---------- " + E.StackTrace + " ------ " + stateCode);
				return "null";
			}
		}

		#endregion


		#region Other Operations
		//This is for binding the inforamtion from the DB into a list of TSMaster 
		static public List<TSMaster> LoadSiteMaster(string osn, bool limit)
		{
			List<TSMaster> myList = new List<TSMaster>();
			DataTable myTable = DBAssist.GetTsMain_SiteMaster(osn, limit);
			TSMaster myEntry;

			foreach (DataRow row in myTable.Rows)
			{
				myEntry = new TSMaster();
				myEntry.SiteMaster();
				myEntry.TimesheetID = Convert.ToString(row["timesheetID"]).Trim();
				myEntry.StartDate = Convert.ToDateTime(row["tsStartDate"]);

				myList.Add(myEntry);
			}

			return myList;
		}

		static public List<decimal> TotalHoursCalculate(List<TSSegment> myList)
		{
			List<decimal> myResults = new List<decimal>();
			decimal overtime = 0m;
			decimal leave = 0m;
			decimal regular = 0m;
			decimal dual = 0m;
			decimal FTO = 0m;

			for (int i = 0; i < myList.Count; i++)
			{
				if (!myList.ElementAt(i).StateCode.Substring(5, 1).Equals("1"))
				{
					if (myList.ElementAt(i).HourType.Equals("Reg") || myList.ElementAt(i).HourType.Equals("exReg"))
					{
						regular += myList.ElementAt(i).SegmentHours;
					}
					else if (myList.ElementAt(i).HourType.Equals("OT"))
					{
						overtime += myList.ElementAt(i).SegmentHours;
					}
					else if (myList.ElementAt(i).HourType.Equals("Leave"))
					{
						leave += myList.ElementAt(i).SegmentHours;
					}
					else if (myList.ElementAt(i).HourType.Equals("FTO"))
					{
						FTO += myList.ElementAt(i).SegmentHours;
					}
					else if (myList.ElementAt(i).HourType.Equals("Dual"))
					{
						dual += myList.ElementAt(i).SegmentHours;
					}
				}
			}

			myResults.Add(regular);
			myResults.Add(leave);
			myResults.Add(overtime);
			myResults.Add(FTO);
			myResults.Add(dual);
			return myResults;
		}

		//Cost conversion can be used to format decimal dollar amounts into the proper format.
		//It can also be used for any decimal type data in string format to shorten it to two decimal places.
		static public string CostConversion(string myCost)
		{
			if (!myCost.Contains("."))
			{
				string StringTemp = myCost + ".00";
				return string.Format("{0:0.00}", Convert.ToDecimal(StringTemp));
			}
			else
			{
				string StringTemp = myCost + "00";
				return string.Format("{0:0.00}", Convert.ToDecimal(StringTemp));
			}
		}

		//method to round time to a specified amount
		static public DateTime RoundTime(string inputTime)
		{
			if (DateTime.TryParse(inputTime, out DateTime e))
			{
				int hour = Convert.ToInt32(e.Hour.ToString());
				int minute = Convert.ToInt32(e.Minute.ToString());
				int div = minute / 15;
				int mod = minute % 15;

				if (mod < 8)
				{
					minute = div * 15;
				}
				if (mod >= 8)
				{
					minute = (div + 1) * 15;
					if (minute == 60)
					{
						hour++;
						minute = 0;
					}
				}

				return Convert.ToDateTime(hour.ToString() + ":" + minute.ToString());
			}
			else
			{
				return Convert.ToDateTime("00:00");
			}
		}

		//the following is used to change stateCodes in a more unified manner
		static public string ChangeStateIdentity(int position, string stateCode, string newValue)
		{
			string part1, part2, changedElement;
			try
			{
				part1 = stateCode.Substring(0, position);
				changedElement = newValue;
				part2 = stateCode.Substring(position + 1, stateCode.Length - position - 1);
				return (part1 + changedElement + part2);
			}
			catch (Exception e)
			{
				return "";
			}

		}

		//This is for making sure OSN information is 5 characters long
		static public string FormatOSNString(string osn)
		{
			StringBuilder result = new StringBuilder();
			for (int i = osn.Length; i < 5; i++)
			{
				result.Append("0");
			}
			result.Append(osn);
			return result.ToString();
		}

		public static string CleanInput(string strIn)
		{
			// Replace invalid characters with empty strings.
			try
			{
				return Regex.Replace(strIn, @"[^\s\w.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
			}
			// If we timeout when replacing invalid characters, 
			// we should return Empty.
			catch (RegexMatchTimeoutException)
			{
				return String.Empty;
			}
		}

		public static void WriteLogEntry(string message)
		{
			string loggingStatus = ConfigurationManager.AppSettings["LoggingStatus"];
			if (loggingStatus.Equals("On"))
			{
				try
				{
					Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

					IPAddress broadcast = IPAddress.Parse("10.16.8.5");

					byte[] sendbuf = Encoding.ASCII.GetBytes(message);
					IPEndPoint ep = new IPEndPoint(broadcast, 11000);

					s.SendTo(sendbuf, ep);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}

		//This is used to get the list of files names that we can revert the DB in the case of needing to undo a LOCK
		static public void GetFileNames(DropDownList myDDL)
		{
			Impersonate.ImpersonateUser();
			string serverPath = "\\\\shares\\TimesheetAttach$\\TS_Config\\";
			string[] fileNames = Directory.GetFiles(serverPath, "*.csv");
			List<string> reversionList = new List<string> { "---" };

			for (int i = 0; i < fileNames.Length; i++)
			{
				reversionList.Add(Path.GetFileName(fileNames[i]));
			}

			FillDropdown(myDDL, myStringList: reversionList);

		}

		//This is for when we are updating the Permissions Table and we want to have the ability to Email people errors and any changes that were made
		static public void HandlePermissionsTable(List<Permission> permissionsTable, string user)
		{
			List<string> sendTo = new List<string>
			{
				"Developer@claysheriff.com"
			};

			//This is so that we can send the edited update email to the following
			List<string> externalSendTo = new List<string>
			{
				"HR@claysheriff.com"//HR
			};

			//This is package this way so that we can call it easily anywhere in the code
			DBAssist.InsertToPermissions(permissionsTable);
			TSMailHandler.CreatePermissionEmail(permissionsTable, user, sendTo);
			TSMailHandler.CreatePermissionEmail(permissionsTable, user, externalSendTo);
		}

		//This is passed in a set a parameters that will determine how the drop down list is filled
		static public void FillDropdown(DropDownList ddlList, List<string> myStringList = null, List<EmployeeInfo> myEmployeeList = null, DataTable myWorkCodes = null, string workCodeType = "",
			DataTable supervisorData = null, string mySupID = "")
		{
			ddlList.Items.Clear();

			if (myStringList != null)
			{
				ddlList.Items.Add(new ListItem("", ""));
				foreach (string row in myStringList)
				{
					ddlList.Items.Add(new ListItem(row.Trim(), row.Trim()));
				}
			}

			if (myEmployeeList != null)
			{
				ddlList.Items.Add(new ListItem("As Me", ""));
				foreach (EmployeeInfo row in myEmployeeList)
				{
					ddlList.Items.Add(new ListItem(Convert.ToString(row.MemberName).Trim(), Convert.ToString(row.MemberOSN).Trim()));
				}
			}

			if (myWorkCodes != null)
			{
				ddlList.Items.Add(new ListItem("", ""));
				foreach (DataRow row in myWorkCodes.Rows)
				{
					if ((workCodeType.Equals(row["codeType"].ToString()) || ("Special".Equals(row["codeType"].ToString()) && workCodeType.Equals("OT"))) &&
						!(row["navName"].Equals("Regular Hours Worked") || row["navName"].Equals("FTO Hours Worked") || row["navName"].Equals("Dual Employment")))
					{
						ddlList.Items.Add(new ListItem(Convert.ToString(row["navName"]).Trim(), Convert.ToString(row["navName"]).Trim()));
					}
				}
			}

			//gets passed a string and a DropDownList object and the ID of a supervisor
			//creates a new datatable and fills it using the results from a method to access a DB
			//fills in the DropDownList with the list of supervisors
			if (supervisorData != null)
			{
				bool supInList = false;
				if (!mySupID.Equals(""))
					mySupID = FormatOSNString(mySupID);

				ddlList.Items.Add(new ListItem("REQUIRED: Choose Supervisor", ""));

				foreach (DataRow row in supervisorData.Rows)
				{
					ddlList.Items.Add(new ListItem(Convert.ToString(row["Name"]).Trim(), Convert.ToString(row["emuserid"]).Trim()));

					if (Convert.ToString(row["emuserid"]).Trim().Equals(mySupID))
						supInList = true;
				}
				
				if (!supInList)
				{
					DataTable newSup = DBAssist.FindMemberPermissions(mySupID);
					foreach (DataRow row in newSup.Rows)
					{
						ddlList.Items.Add(new ListItem(Convert.ToString(row["memberName"]).Trim(), mySupID));
					}
				}
			}

			ddlList.DataBind();
		}
		#endregion
	}
}