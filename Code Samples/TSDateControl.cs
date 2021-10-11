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

namespace CCSOTimeSheet
{
	//can we please rename this to something that accurately describes what it is for? All this code is used for CalendarView
	public class TSDateControl
	{ 
		//This section is used to handle all of the functions that are used in Calendar view to add in date creation. In addition to DayView so that it can display holidays
		//This is used to create the HashTable for Haliddays used in Calendar View
		public static Hashtable GetHolidays()
		{
			DateTime projectOpenDate = Convert.ToDateTime("1/1/" + DateTime.Now.Year);
			DateTime projectEndDate = Convert.ToDateTime("12/31/" + DateTime.Now.AddYears(30).Year);
			Hashtable holiday = new Hashtable();

			while (projectEndDate.Subtract(projectOpenDate).TotalDays > 0)
			{
				if (!IsFederalHoliday(projectOpenDate).Equals(""))
				{
					//holiday lands on Sunday
					if (IsFederalHoliday(projectOpenDate).Substring(0, 1).Equals("+"))
					{
						//Check if observed day falls on a holiday or 
						if (!holiday.ContainsKey((projectOpenDate).ToShortDateString()) || (holiday.ContainsKey((projectOpenDate).ToShortDateString())
							&& holiday[(projectOpenDate).ToShortDateString()].ToString().Contains("Observed")))
						{
							if (!holiday.ContainsKey((projectOpenDate).ToShortDateString()))
							{
								holiday.Add(projectOpenDate.ToShortDateString(), IsFederalHoliday(projectOpenDate).Substring(1));
							}

							else if ((holiday.ContainsKey((projectOpenDate).ToShortDateString()) && holiday[(projectOpenDate).ToShortDateString()].ToString().Contains("Observed")))
							{
								holiday[projectOpenDate.ToShortDateString()] = IsFederalHoliday(projectOpenDate).Substring(1);
							}
							if (IsFederalHoliday(projectOpenDate.AddDays(+1)).Equals(""))
							{
								holiday.Add((projectOpenDate).AddDays(+1).ToShortDateString(), String.Concat(IsFederalHoliday(projectOpenDate).Substring(1), " Observed"));
							}
							else
							{
								string text = IsFederalHoliday(projectOpenDate.AddDays(+1)).Substring(1) + " Observed";
								holiday.Add((projectOpenDate).AddDays(+1).ToShortDateString(), IsFederalHoliday(projectOpenDate.AddDays(+1)).Substring(1));
								holiday[projectOpenDate.AddDays(+1).ToShortDateString()] = holiday[projectOpenDate.AddDays(+1).ToShortDateString()] + " / " + String.Concat(IsFederalHoliday(projectOpenDate).Substring(1), " Observed");

								bool observedDayFound = false;
								int counter = 1;

								while (!observedDayFound)
								{
									if (!holiday.ContainsKey((projectOpenDate).AddDays(+counter).ToShortDateString()) && IsFederalHoliday(projectOpenDate.AddDays(+counter)).Equals(""))
									{
										holiday.Add((projectOpenDate).AddDays(+counter).ToShortDateString(), text);
										observedDayFound = true;
									}
									counter++;

								}

							}
						}
					}
					//Holiday lands on Saturday
					else if (IsFederalHoliday(projectOpenDate).Substring(0, 1).Trim().Equals("-"))
					{
						if (!holiday.ContainsKey((projectOpenDate).ToShortDateString()))
						{
							holiday.Add(projectOpenDate.ToShortDateString(), IsFederalHoliday(projectOpenDate).Substring(1));
							if (!holiday.ContainsKey((projectOpenDate).AddDays(-1).ToShortDateString()))
							{
								holiday[projectOpenDate.AddDays(-1).ToShortDateString()] = String.Concat(IsFederalHoliday(projectOpenDate).Substring(1), " Observed");
							}
							else if (!holiday[(projectOpenDate).AddDays(-1).ToShortDateString()].ToString().Contains("Observed"))
							{
								string text = holiday[projectOpenDate.AddDays(-1).ToShortDateString()].ToString() + " Observed";
								holiday[projectOpenDate.AddDays(-1).ToShortDateString()] = holiday[projectOpenDate.AddDays(-1).ToShortDateString()] + " / " + String.Concat(IsFederalHoliday(projectOpenDate).Substring(1), " Observed");

								bool observedDayFound = false;
								int counter = 1;

								while (!observedDayFound)
								{
									if (!holiday.ContainsKey((projectOpenDate).AddDays(-counter).ToShortDateString()))
									{
										holiday.Add((projectOpenDate).AddDays(-counter).ToShortDateString(), text);
										observedDayFound = true;
									}
									counter++;

								}
							}
						}
					}
					/*else if (holiday.ContainsKey(projectOpenDate.ToShortDateString()))
						holiday[projectOpenDate] = IsFederalHoliday(projectOpenDate).Substring(1);*/
					else if (IsFederalHoliday(projectOpenDate).Substring(0, 1).Trim().Equals("="))
					{
						if (!holiday.ContainsKey((projectOpenDate).ToShortDateString()))
						{
							holiday.Add(projectOpenDate.ToShortDateString(), IsFederalHoliday(projectOpenDate).Substring(1));
						}
					}


				}
				projectOpenDate = projectOpenDate.AddDays(+1);
			}

			return holiday;
		}
		//Sets Reb/Blue Days
		public static Hashtable GetworkDays()
		{
			DateTime projectOpenDate = Convert.ToDateTime("1/1/" + DateTime.Now.Year);
			DateTime projectEndDate = Convert.ToDateTime("12/31/" + DateTime.Now.AddYears(30).Year);
			Hashtable redAndBlueDays = new Hashtable();
			//Debug.WriteLine(projectEndDate.Subtract(projectOpenDate).TotalDays);

			while (projectEndDate.Subtract(projectOpenDate).TotalDays > 0)
			{
				redAndBlueDays.Add(projectOpenDate.ToShortDateString(), IsRedorBlue(projectOpenDate));
				projectOpenDate = projectOpenDate.AddDays(1);
			}

			return redAndBlueDays;
		}

		//These will be used to generate the days and input into the db or csv type file or access (Stored in Session Varible)
		public static string IsFederalHoliday(DateTime date)
		{
			// to ease typing
			int nthWeekDay = (int)(Math.Ceiling((double)date.Day / 7.0d));
			DayOfWeek dayName = date.DayOfWeek;
			bool isThursday = dayName == DayOfWeek.Thursday;
			bool isFriday = dayName == DayOfWeek.Friday;
			bool isMonday = dayName == DayOfWeek.Monday;
			bool isSaterday = dayName == DayOfWeek.Saturday;
			bool isSunday = dayName == DayOfWeek.Sunday;
			// New Years Day 
			if (date.Month == 1 && date.Day == 1)
			{
				if (isSunday) return "+New Year's Day";
				if (isSaterday) return "-New Year's Day";
				return "=New Year's Day";
			}
			// MLK day (3rd monday in January)
			else if (date.Month == 1 && isMonday && nthWeekDay == 3)
			{
				if (isSunday) return "+Martin Luther King Jr. Day";
				if (isSaterday) return "-Martin Luther King Jr. Day";
				return "=Martin Luther King Jr. Day";
			}
			// President’s Day (3rd Monday in February)
			else if (date.Month == 2 && isMonday && nthWeekDay == 3)
			{
				if (isSunday) return "+President's Day";
				if (isSaterday) return "-President's Day";
				return "=President's Day";
			}
			// Memorial Day (Last Monday in May)
			else if (date.Month == 5 && isMonday && date.AddDays(7).Month == 6)
			{
				if (isSunday) return "+Memorial Day";
				if (isSaterday) return "-Memorial Day";
				return "=Memorial Day";
			}
			// Independence Day (July 4, or preceding Friday/following Monday if weekend)
			else if (date.Month == 7 && date.Day == 4)
			{
				if (isSunday) return "+Independence Day";
				if (isSaterday) return "-Independence Day";
				return "=Independence Day";
			}
			// Labor Day (1st Monday in September)
			else if (date.Month == 9 && isMonday && nthWeekDay == 1)
			{
				if (isSunday) return "+Labor Day";
				if (isSaterday) return "-Labor Day";
				return "=Labor Day";
			}
			// Veteran’s Day (November 11)
			else if (date.Month == 11 && date.Day == 11)
			{
				if (isSunday) return "+Veteran’s Day";
				if (isSaterday) return "-Veteran’s Day";
				return "=Veteran’s Day";
			}
			// Thanksgiving Day (4th Thursday in November)
			else if (date.Month == 11 && isThursday && nthWeekDay == 4) return "=Thanksgiving";
			// Thanksgiving Day dat after (4th Friday in November)
			else if (date.Month == 11 && isFriday && nthWeekDay == 4) return "=Black Friday";
			// Christmas Eve (December 24)
			else if (date.Month == 12 && date.Day == 24)
			{
				if (isSunday) return "+Christmas Eve";
				if (isSaterday) return "-Christmas Eve";
				return "=Christmas Eve";
			}
			// Christmas Day (December 25)
			else if (date.Month == 12 && date.Day == 25)
			{
				if (isSunday) return "+Christmas Day";
				if (isSaterday) return "-Christmas Day";
				return "=Christmas Day";
			}
			else if (date.Month == 6 && date.Day == 19)
			{
				if (isSunday) return "+Juneteenth";
				if (isSaterday) return "-Juneteenth";
				return "=Juneteenth";
			}

			else return "";
		}
		public static string IsRedorBlue(DateTime date)
		{
			int dayIn14Period = date.Subtract(Convert.ToDateTime("08/05/2019")).Days % 14;
			if (dayIn14Period == 0 || dayIn14Period == 1 || dayIn14Period == 4 || dayIn14Period == 5 || dayIn14Period == 6 || dayIn14Period == 9 || dayIn14Period == 10)
			{
				//Blue Day is true
				return "Blue";
			}
			else
			{
				//Red Day is false
				return "Red";
			}
		}

		//this is used to calculate the total number of hours each segment type has
		//To pull Regular hours from the decimal list it is .ElementAt(0)
		//To pull Leave hours from the decimal list it is .ElementAt(1)
		//To pull Overtime hours from the decimal list it is .ElementAt(2)
		//To pull not approved Leave hours from the decimal list it is .ElementAt(3)
		//To pull not approved Overtime hours from the decimal list it is .ElementAt(4)
		static public List<decimal> TotalHoursCalculate_CalendarView(List<TSSegment> myList)
		{
			List<decimal> myResults = new List<decimal>();
			decimal regular = 0m;
			decimal dual = 0m;
			decimal leave = 0m;
			decimal overtime = 0m;
			decimal notApprovedLeave = 0m;
			decimal notApprovedOvertime = 0m;

			for (int i = 0; i < myList.Count; i++)
			{
				if (myList.ElementAt(i).HourType.Equals("Reg") || myList.ElementAt(i).HourType.Equals("exReg"))
				{
					regular += myList.ElementAt(i).SegmentHours;
				}
				else if (myList.ElementAt(i).HourType.Equals("Dual"))
				{
					dual += myList.ElementAt(i).SegmentHours;
				}
				else if (myList.ElementAt(i).HourType.Equals("OT"))
				{
					if (myList.ElementAt(i).PayoutCode.Equals("Premium Pay"))
					{
						if (TSUtilities.ConvertToBinary(myList.ElementAt(i).StateCode).Substring(1, 1).Equals("1") && TSUtilities.ConvertToBinary(myList.ElementAt(i).StateCode).Substring(8, 1).Equals("1"))
						{
							overtime += myList.ElementAt(i).SegmentHours;
						}
						else
						{
							notApprovedOvertime += myList.ElementAt(i).SegmentHours;
						}
					}
					else
					{
						if (TSUtilities.ConvertToBinary(myList.ElementAt(i).StateCode).Substring(1, 1).Equals("1"))
						{
							overtime += myList.ElementAt(i).SegmentHours;
						}
						else
						{
							notApprovedOvertime += myList.ElementAt(i).SegmentHours;
						}
					}

				}
				else if (myList.ElementAt(i).HourType.Equals("Leave"))
				{
					if (TSUtilities.ConvertToBinary(myList.ElementAt(i).StateCode).Substring(1, 1).Equals("1"))
					{
						leave += myList.ElementAt(i).SegmentHours;
					}
					else
					{
						notApprovedLeave += myList.ElementAt(i).SegmentHours;
					}
				}
			}
			if (regular < 0)
				regular += 24;
			if (leave < 0)
				leave += 24;
			if (overtime < 0)
				overtime += 24;
			if (notApprovedLeave < 0)
				notApprovedLeave += 24;
			if (notApprovedOvertime < 0)
				notApprovedOvertime += 24;

			myResults.Add(regular);
			myResults.Add(leave);
			myResults.Add(overtime);
			myResults.Add(notApprovedLeave);
			myResults.Add(notApprovedOvertime);
			myResults.Add(dual);
			return myResults;
		}
	}
}