
using System;
using System.Collections.Generic;
using System.Linq;

namespace CCSOTimeSheet
{

	public class TSValidation
	{
		/// <summary>
		/// Validate user 
		/// </summary>
		/// <returns></returns>
		public static bool UserValidation(string command, string accessLevel)
		{
			if (command.Equals("permissionsUpdater"))
			{
				if (accessLevel.Equals("Developer"))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			return false;

		}
		#region split Time      
		/// <summary>
		/// Used to auto split time within a timesheet, For multi leave segments and Copy timesheet function.
		/// </summary>
		/// <param name="timeSheet"> Input timesheet to auto split segments of timesheet</param>
		/// <param name="isLeaveAutoAdd"> Input bool for running the method for leave</param>
		/// <returns>Timesheet after segments have been split</returns>
		public static TimeSheet AutoSplitTimeSheet(TimeSheet timeSheet, bool isLeaveAutoAdd = false)
		{
			List<TSSegment> TimeSheetSegmentReturnArray = new List<TSSegment>();
			List<TSSegment> NewTimeSheetSegmentArray = new List<TSSegment>();
			List<TSSegment> NextTimeSheetSegmentArray = new List<TSSegment>();
			List<TSSegment> PriorTimeSheetSegmentArray = new List<TSSegment>();
			bool ClearOriginalTimeSheetLeave = true;
			TimeSheet splitSheet = new TimeSheet();
			TimeSheet priorSheet = new TimeSheet();

			//TimeSheet splitSheet = TSUtilities.GetTimesheetInformation_V2(timeSheet.TSMain.StartDate.AddDays(14).ToShortDateString(), timeSheet.MemberInforamtion.MemberUsername,
			//	timeSheet.MemberInforamtion, false, true, timeSheet.MemberInforamtion.WorkCodes);
			//splitSheet.TSMain.TimesheetID = DBAssist.GetTSID_LeaveReq(timeSheet.MemberInforamtion.MemberOSN, timeSheet.TSMain.StartDate.AddDays(14).ToShortDateString()).ToString();
			//splitSheet.TSMain.StartDate = timeSheet.TSMain.StartDate.AddDays(14);

			splitSheet.SetTimesheet(timeSheet.TSMain.StartDate.AddDays(14).ToShortDateString(), timeSheet.MemberInforamtion.MemberUsername);
			//Prior Timesheet
			//TimeSheet priorSheet = TSUtilities.GetTimesheetInformation_V2(timeSheet.TSMain.StartDate.AddDays(-14).ToShortDateString(), timeSheet.MemberInforamtion.MemberUsername,
			//    timeSheet.MemberInforamtion, false, true, timeSheet.MemberInforamtion.WorkCodes);
			//priorSheet.TSMain.TimesheetID = DBAssist.GetTSID_LeaveReq(timeSheet.MemberInforamtion.MemberOSN, timeSheet.TSMain.StartDate.AddDays(-14).ToShortDateString()).ToString();
			//priorSheet.TSMain.StartDate = timeSheet.TSMain.StartDate.AddDays(-14);

			priorSheet.SetTimesheet(timeSheet.TSMain.StartDate.AddDays(-14).ToShortDateString(), timeSheet.MemberInforamtion.MemberUsername);

			if (!Int32.TryParse(timeSheet.TSMain.ApprovalID, out _))
			{
				splitSheet.TSMain.ApprovalID = DBAssist.GetChiefID(timeSheet.MemberInforamtion.Permissions.Substring(1, 2), false);
			}
			else
			{
				splitSheet.TSMain.ApprovalID = timeSheet.TSMain.ApprovalID;
			}

			if (isLeaveAutoAdd && splitSheet.TSMain.TimesheetID.Equals("-1"))
			{
				splitSheet.TSMain.FinApprovalID = "";
				splitSheet.TSMain.HourRegMax = timeSheet.MemberInforamtion.HourRegMax;
				splitSheet.TSMain.MemberName = timeSheet.TSMain.MemberName;
				splitSheet.TSMain.MemberOSN = timeSheet.MemberInforamtion.MemberOSN;
				splitSheet.TSMain.Notes = timeSheet.TSMain.Notes;
				splitSheet.TSMain.OvertimeThreshold = timeSheet.MemberInforamtion.OTThreshold;
				splitSheet.TSMain.ShiftCode = timeSheet.TSMain.ShiftCode;
				splitSheet.TSMain.ShiftID = timeSheet.MemberInforamtion.ShiftCode;
			}

			foreach (TSSegment item in timeSheet.TimeSegmentList)
			{
				//look at adding prior split here
				DateTime splitInner = GetSplitDate(/*item,*/ timeSheet, true);
				DateTime splitOuter = GetSplitDate(/*item,*/ timeSheet, false);
				// DateTime splitPrior = GetSplitDateTime(item, timeSheet);
				//if(item == timeSheet.TimeSegmentList[0]) // prior week check only on first segment
				//{
				if (SplitToPreviousWeek(item, timeSheet)&& RequiresTimeSplit(item, splitOuter, timeSheet))//issue is here for some of the splitting issues
				{
					TimeSheetSegmentReturnArray = GetSplitTimeSegment(item, timeSheet);
					foreach (TSSegment segment in TimeSheetSegmentReturnArray)
					{
						if (segment == TimeSheetSegmentReturnArray[0])
						{
							if (TSValidation.TimeValidationV2(segment, priorSheet) || isLeaveAutoAdd)
							{
								PriorTimeSheetSegmentArray.Add(segment);
							}
						}
						else if (segment.StartTime >= splitOuter) //FLSA Outer split
						{
							if (TSValidation.TimeValidationV2(segment, splitSheet) || isLeaveAutoAdd)
							{
								NextTimeSheetSegmentArray.Add(segment);
							}
						}
						else//Place the time on the original timesheet of the split
						{
							if (TSValidation.TimeValidationV2(segment, timeSheet) || isLeaveAutoAdd)
							{
								NewTimeSheetSegmentArray.Add(segment);
								ClearOriginalTimeSheetLeave = false; //Do NOT Clear the original time sheet's Leave
							}

						}
					}

				}

				//}
				else if (RequiresTimeSplit(item, splitOuter, timeSheet))//Read into the requires time split method
				{
					TimeSheetSegmentReturnArray = GetSplitTimeSegment(item, timeSheet);

					if (TimeSheetSegmentReturnArray.Count() > 0)
					{
						foreach (TSSegment segment in TimeSheetSegmentReturnArray)
						{

							if (segment.StartTime >= splitOuter) //FLSA Outer split
							{
								if (TSValidation.TimeValidationV2(segment, splitSheet) || isLeaveAutoAdd)
								{
									NextTimeSheetSegmentArray.Add(segment);
								}
							}
							else//Place the time on the original timesheet of the split
							{
								if (TSValidation.TimeValidationV2(segment, timeSheet) || isLeaveAutoAdd)
								{
									NewTimeSheetSegmentArray.Add(segment);
									ClearOriginalTimeSheetLeave = false; //Do NOT Clear the original time sheet's Leave
								}

							}
						}
					}
				}
				else //does not require time split
				{
					if (isLeaveAutoAdd)//holidays for admin would be the main case here
					{
						//item.EndTime = TimeSheetSegmentReturnArray[1].EndTime;
						if (item.Date >= timeSheet.TSMain.StartDate.AddDays(14))
						{
							if (TSValidation.TimeValidationV2(item, splitSheet))
							{
								NextTimeSheetSegmentArray.Add(item);
							}
						}
						else
						{
							//TimeSheet current = TSUtilities.GetTimesheetInformation_V2(timeSheet.TSMain.StartDate.ToShortDateString(), timeSheet.MemberInforamtion.MemberUsername,
							//	timeSheet.MemberInforamtion, false, true, timeSheet.MemberInforamtion.WorkCodes);
							TimeSheet current = new TimeSheet();
							current.SetTimesheet(timeSheet.TSMain.StartDate.ToShortDateString(), timeSheet.MemberInforamtion.MemberUsername);
							if (TSValidation.TimeValidationV2(item, current))
							{
								NewTimeSheetSegmentArray.Add(item);
							}
						}
					}
					//For copy function
					else
					{
						//item.EndTime = TimeSheetSegmentReturnArray[1].EndTime;
						if (item.StartTime >= splitOuter)//timeSheet.TSMain.StartDate.AddDays(14)) //+++ Changed on 4/27/2021, if still commented out after 5/18/2021 please remove -Drew +++
						{
							if (TSValidation.TimeValidationV2(item, splitSheet))
							{
								NextTimeSheetSegmentArray.Add(item);
							}
						}
						else
						{
							NewTimeSheetSegmentArray.Add(item);
						}
					}

				}
			}
			//If no time was placed BACK on the original timesheet of the split clear the entries
			if (ClearOriginalTimeSheetLeave)
			{
				timeSheet.TimeSegmentList.Clear();

			}

			if (NewTimeSheetSegmentArray.Count > 0)
			{
				// add time to current timesheet to be returned
				timeSheet.TimeSegmentList.Clear();
				timeSheet.TimeSegmentList.AddRange(NewTimeSheetSegmentArray);
			}

			if (NextTimeSheetSegmentArray.Count > 0)
			{
				//timeSheet.TimeSegmentList.Clear();
				//add time to next week
				splitSheet.TimeSegmentList.Clear();
				splitSheet.TimeSegmentList.AddRange(NextTimeSheetSegmentArray);

				splitSheet.TSMain.HourRegMax = timeSheet.MemberInforamtion.HourRegMax;
				splitSheet.TSMain.MemberName = timeSheet.TSMain.MemberName;
				splitSheet.TSMain.MemberOSN = timeSheet.MemberInforamtion.MemberOSN;
				splitSheet.TSMain.Notes = timeSheet.TSMain.Notes;
				splitSheet.TSMain.OvertimeThreshold = timeSheet.MemberInforamtion.OTThreshold;
				splitSheet.TSMain.ShiftCode = timeSheet.TSMain.ShiftCode;
				splitSheet.TSMain.ShiftID = timeSheet.TSMain.ShiftID;
				splitSheet.TSMain.StateCode = TSUtilities.ChangeStateIdentity(7, timeSheet.TSMain.StateCode, "1");

				//if this is a new timesheet 
				if (splitSheet.TSMain.TimesheetID.Equals("-1"))
				{
					splitSheet.TSMain.StateCode = TSUtilities.ChangeStateIdentity(6, splitSheet.TSMain.StateCode, "0");
					splitSheet.TSMain.StateCode = TSUtilities.ChangeStateIdentity(7, splitSheet.TSMain.StateCode, "1");
				}

				InsertSplitTimeIntoDB(splitSheet, timeSheet);
			}

			if (PriorTimeSheetSegmentArray.Count > 0)
			{
				//timeSheet.TimeSegmentList.Clear();
				//add time to next week

				priorSheet.TimeSegmentList.AddRange(PriorTimeSheetSegmentArray);

				//if this is a new timesheet 
				if (splitSheet.TSMain.TimesheetID.Equals("-1"))
				{
					splitSheet.TSMain.StateCode = TSUtilities.ChangeStateIdentity(6, splitSheet.TSMain.StateCode, "0");
					splitSheet.TSMain.StateCode = TSUtilities.ChangeStateIdentity(7, splitSheet.TSMain.StateCode, "1");
				}

				InsertSplitTimeIntoDB(priorSheet, timeSheet);
			}

			return timeSheet;
		}


		/// <summary>
		/// Gets the split time segment of a segment
		/// </summary>
		/// <param name="emptyObject">Splits Timesegment if needed</param>
		/// <param name="myTimesheet">Used for variable checks</param>
		/// <returns>
		/// -> Original if no split
		/// -> Updated Original and split time
		/// </returns>
		public static List<TSSegment> GetSplitTimeSegment(TSSegment emptyObject, TimeSheet myTimesheet)
		{
			List<TSSegment> TimeSegmentArray = new List<TSSegment>();
			/*DateTime ShiftSplit = GetSplitDateTime(emptyObject, myTimesheet);*/
			DateTime splitTime = new DateTime();
			//will likely need the prior split here
			DateTime innerSplitTimeDate = GetSplitDate(/*emptyObject,*/ myTimesheet, true);
			DateTime outerSplitTimeDate = GetSplitDate(/*emptyObject,*/ myTimesheet, false);
			DateTime priorSplitTime = GetSplitDate(/*emptyObject,*/ myTimesheet, true, true);
			bool trippleSplit = false;
			bool innerWeekSplit = false;
			//prior week split
			if (RequiresTimeSplit(emptyObject, innerSplitTimeDate, myTimesheet))
			{
				//string[] startTime = txtModalStart.Text.ToString().Split(':');
				string[] endTime = emptyObject.EndTime.ToString("HH:mm").Split(':');

				//Midnight
				string[] temp = emptyObject.Date.ToShortDateString().Split('/');
				DateTime midnight = new DateTime(Convert.ToInt32(temp[2]), Convert.ToInt32(temp[0]), Convert.ToInt32(temp[1]), Convert.ToInt32(0), Convert.ToInt32(0), 0);

				DateTime day = emptyObject.Date;

				//check if midnight split
				if (TimeValidationMidnightCheck(emptyObject, emptyObject))
				{
					if (emptyObject.Date == innerSplitTimeDate.Date || emptyObject.Date == innerSplitTimeDate.AddDays(-1).Date)
					{
						if (emptyObject.StartTime < innerSplitTimeDate && emptyObject.EndTime.AddDays(1) > innerSplitTimeDate)
						{

							if (midnight.TimeOfDay == innerSplitTimeDate.TimeOfDay)//In the event that the users FLSA splitTime is midnight, triple split can not occur 
							{
								innerWeekSplit = true;
							}
							else
							{
								//need to triple split
								trippleSplit = true;
								innerWeekSplit = true;
							}


						}

					}
					if (emptyObject.Date == outerSplitTimeDate.Date || emptyObject.Date == outerSplitTimeDate.AddDays(-1).Date)
					{
						if (emptyObject.StartTime < outerSplitTimeDate && emptyObject.EndTime.AddDays(1) > outerSplitTimeDate)
						{
							if (midnight.TimeOfDay == outerSplitTimeDate.TimeOfDay)//In the event that the users FLSA splitTime is midnight, triple split can not occur 
							{
								innerWeekSplit = false;
							}
							else
							{
								//need to triple split
								trippleSplit = true;
								innerWeekSplit = false;
							}

						}
					}
				}

				if (trippleSplit)
				{

					DateTime tempDay = emptyObject.Date;
					DateTime ShiftStart = new DateTime();
					DateTime ShiftEnd = Convert.ToDateTime("00:00");
					bool midnightFirst = false;

					if (innerWeekSplit)
					{
						//ShiftStart = innerSplitTimeDate;
						if (emptyObject.StartTime.TimeOfDay < innerSplitTimeDate.TimeOfDay)
						{//do FLSA split first
							ShiftStart = innerSplitTimeDate;
							ShiftEnd = Convert.ToDateTime("00:00");
						}
						else
						{ //Midnight split first
							ShiftStart = Convert.ToDateTime("00:00");
							ShiftEnd = innerSplitTimeDate;
							tempDay = tempDay.AddDays(1);
							midnightFirst = true;
						}
					}
					else
					{
						//ShiftStart = outerSplitTimeDate;
						if (emptyObject.StartTime.TimeOfDay < outerSplitTimeDate.TimeOfDay)
						{//do FLSA split first
							ShiftStart = outerSplitTimeDate;
							ShiftEnd = Convert.ToDateTime("00:00");
						}
						else
						{ //Midnight split first
							ShiftStart = Convert.ToDateTime("00:00");
							ShiftEnd = outerSplitTimeDate;
							tempDay = tempDay.AddDays(1);
							midnightFirst = true;
						}
					}

					if (ShiftEnd.Date != tempDay.Date || tempDay.Date != ShiftStart.Date)
					{
						string[] CleanDate = tempDay.Date.ToShortDateString().Split('/');
						string[] CleanStartTime = ShiftStart.ToString("HH:mm").Split(':');
						string[] CleanEnd = ShiftEnd.ToString("HH:mm").Split(':');
						//string[] CleanNoon = FLSASundaySplitTime.ToString("HH:mm").Split(':');

						ShiftStart = new DateTime(Convert.ToInt32(CleanDate[2]), Convert.ToInt32(CleanDate[0]), Convert.ToInt32(CleanDate[1]),
						Convert.ToInt32(CleanStartTime[0]), Convert.ToInt32(CleanStartTime[1]), 0);
						ShiftEnd = new DateTime(Convert.ToInt32(CleanDate[2]), Convert.ToInt32(CleanDate[0]), Convert.ToInt32(CleanDate[1]),
						Convert.ToInt32(CleanEnd[0]), Convert.ToInt32(CleanEnd[1]), 0);
						/*FLSASundaySplitTime = new DateTime(Convert.ToInt32(CleanDate[2]), Convert.ToInt32(CleanDate[0]), Convert.ToInt32(CleanDate[1]),
                              Convert.ToInt32(CleanNoon[0]), Convert.ToInt32(CleanNoon[1]), 0);*/
					}

					//create new split time segment split 1 split time to midnight
					//------New Split Segment 01----
					string[] TempDate = day.ToShortDateString().Split('/');

					TSSegment Temp = new TSSegment
					{
						TimesheetID = emptyObject.TimesheetID,
						HourType = emptyObject.HourType,
						Date = tempDay,
						StartTime = ShiftStart,
						EndTime = ShiftEnd,
						StateCode = emptyObject.StateCode,
						Identity = myTimesheet.TimeSegmentList.Count
					};

					Temp.CalculateSegmentHours();

					if (emptyObject.HourType.Equals("OT"))
					{
						Temp.PayoutCode = emptyObject.PayoutCode;
						Temp.ApprovalID = emptyObject.ApprovalID;
						Temp.Nature = emptyObject.Nature;
						Temp.CaseNum = emptyObject.CaseNum;
						Temp.OTGUID = Guid.NewGuid().ToString();
					}

					if (emptyObject.HourType.Equals("FTO"))
					{
						Temp.FTONotes = emptyObject.FTONotes;
						Temp.TraineeOSN = emptyObject.TraineeOSN;

					}

					if (emptyObject.HourType.Equals("ExReg"))
					{
						Temp.ExFundCode = emptyObject.ExFundCode;
						Temp.ExFundNotes = emptyObject.ExFundNotes;
					}

					if (emptyObject.HourType.Equals("Leave"))
					{
						Temp.ApprovalID = emptyObject.ApprovalID;
						Temp.LeaveCode = emptyObject.LeaveCode;
					}

					//-----ADD Orginal TIME--------------
					ShiftEnd = emptyObject.EndTime;
					emptyObject.EndTime = ShiftStart;
					ShiftStart = Temp.EndTime;

					emptyObject.CalculateSegmentHours();
					TimeSegmentArray.Add(emptyObject);

					//ADD SPLIT ONE TIME
					TimeSegmentArray.Add(Temp);

					//PREP FOR NEXT DAY TIME
					if (!midnightFirst)
					{
						tempDay = tempDay.AddDays(1);
					}

					if (ShiftEnd.Date != tempDay.Date || tempDay.Date != ShiftStart.Date)
					{
						string[] CleanDate = tempDay.Date.ToShortDateString().Split('/');
						string[] CleanStartTime = ShiftStart.ToString("HH:mm").Split(':');
						string[] CleanEnd = ShiftEnd.ToString("HH:mm").Split(':');
						ShiftStart = new DateTime(Convert.ToInt32(CleanDate[2]), Convert.ToInt32(CleanDate[0]), Convert.ToInt32(CleanDate[1]),
						Convert.ToInt32(CleanStartTime[0]), Convert.ToInt32(CleanStartTime[1]), 0);
						ShiftEnd = new DateTime(Convert.ToInt32(CleanDate[2]), Convert.ToInt32(CleanDate[0]), Convert.ToInt32(CleanDate[1]),
						Convert.ToInt32(CleanEnd[0]), Convert.ToInt32(CleanEnd[1]), 0);
					}

					//------New Split Segment 02----
					Temp = new TSSegment
					{
						TimesheetID = emptyObject.TimesheetID,
						HourType = emptyObject.HourType,
						Date = tempDay,
						StartTime = ShiftStart,
						EndTime = ShiftEnd,
						StateCode = emptyObject.StateCode,
						Identity = myTimesheet.TimeSegmentList.Count
					};

					Temp.CalculateSegmentHours();

					if (emptyObject.HourType.Equals("OT"))
					{
						Temp.PayoutCode = emptyObject.PayoutCode;
						Temp.ApprovalID = emptyObject.ApprovalID;
						Temp.Nature = emptyObject.Nature;
						Temp.CaseNum = emptyObject.CaseNum;
						Temp.OTGUID = Guid.NewGuid().ToString();
					}
					if (emptyObject.HourType.Equals("FTO"))
					{
						Temp.FTONotes = emptyObject.FTONotes;
						Temp.TraineeOSN = emptyObject.TraineeOSN;

					}

					if (emptyObject.HourType.Equals("ExReg"))
					{
						Temp.ExFundCode = emptyObject.ExFundCode;
						Temp.ExFundNotes = emptyObject.ExFundNotes;
					}

					if (emptyObject.HourType.Equals("Leave"))
					{
						Temp.ApprovalID = emptyObject.ApprovalID;
						Temp.LeaveCode = emptyObject.LeaveCode;
					}

					TimeSegmentArray.Add(Temp);

					return TimeSegmentArray;
				}
				else
				{ //Not tripple split
				  //check if splitting across midnight
				  //check if splitting across flsa
					bool midnightSplit = false;
					bool outerWeekSplit = false;
					innerWeekSplit = false;
					bool priorWeekSplit = false;

					if (TimeValidationMidnightCheck(emptyObject, emptyObject))
					{
						midnightSplit = true;
					}
					if (emptyObject.StartTime < innerSplitTimeDate && emptyObject.EndTime > innerSplitTimeDate)
					{
						innerWeekSplit = true;
					}
					if (emptyObject.StartTime < outerSplitTimeDate && emptyObject.EndTime > outerSplitTimeDate)
					{
						outerWeekSplit = true; //This was false, I'm willing to bet in error -Drew 3/29/2021 +++ Remove if not changed by 5/29/2021 +++
					}
					if ((emptyObject.StartTime.Date <= myTimesheet.TSMain.StartDate) && (emptyObject.StartTime < priorSplitTime && emptyObject.EndTime > priorSplitTime))
					{
						priorWeekSplit = true;
					}

					//process

					if (midnightSplit)
					{
						day = day.AddDays(1);
						splitTime = midnight;
					}
					else if (outerWeekSplit || innerWeekSplit || priorWeekSplit)
					{
						temp = emptyObject.Date.ToShortDateString().Split('/');
						splitTime = new DateTime(Convert.ToInt32(temp[2]), Convert.ToInt32(temp[0]), Convert.ToInt32(temp[1]), Convert.ToInt32(innerSplitTimeDate.Hour), Convert.ToInt32(innerSplitTimeDate.Minute), 0);
					}
				}


				
				string[] date = day.ToShortDateString().Split('/');

				//create new split time segment
				TSSegment splitSegment = new TSSegment
				{
					TimesheetID = emptyObject.TimesheetID,
					HourType = emptyObject.HourType,
					Date = day,
					StartTime = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[0]), Convert.ToInt32(date[1]),
					Convert.ToInt32(splitTime.Hour), Convert.ToInt32(splitTime.Minute), 0),
					EndTime = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[0]), Convert.ToInt32(date[1]),
					Convert.ToInt32(endTime[0]), Convert.ToInt32(endTime[1]), 0),
					StateCode = emptyObject.StateCode,
					Identity = myTimesheet.TimeSegmentList.Count
				};

				splitSegment.CalculateSegmentHours();

				if (emptyObject.HourType.Equals("OT"))
				{
					splitSegment.PayoutCode = emptyObject.PayoutCode;
					splitSegment.ApprovalID = emptyObject.ApprovalID;
					splitSegment.Nature = emptyObject.Nature;
					splitSegment.CaseNum = emptyObject.CaseNum;
					splitSegment.OTGUID = Guid.NewGuid().ToString();
				}

				if (emptyObject.HourType.Equals("FTO"))
				{
					splitSegment.FTONotes = emptyObject.FTONotes;
					splitSegment.TraineeOSN = emptyObject.TraineeOSN;

				}

				if (emptyObject.HourType.Equals("ExReg"))
				{
					splitSegment.ExFundCode = emptyObject.ExFundCode;
					splitSegment.ExFundNotes = emptyObject.ExFundNotes;
				}


				if (emptyObject.HourType.Equals("Leave"))
				{
					splitSegment.ApprovalID = emptyObject.ApprovalID;
					splitSegment.LeaveCode = emptyObject.LeaveCode;
				}
				emptyObject.EndTime = splitTime;
				emptyObject.CalculateSegmentHours();
				TimeSegmentArray.Add(emptyObject);
				TimeSegmentArray.Add(splitSegment);
			}

			//TimeSegmentArray.Add(emptyObject);
			return TimeSegmentArray;
		}


		/// <summary>
		/// Inserts the split time into the DB, only called when time splits into the nexr payperiod.
		/// </summary>
		/// <param name="SplitTimeSheet"> used</param>
		/// <param name="originalTimeSheet"> used to copy data to SplitTimeSheet</param>
		/// <returns> true if the procees worked</returns>
		public static bool InsertSplitTimeIntoDB(TimeSheet SplitTimeSheet, TimeSheet originalTimeSheet)
		{
			try
			{
				List<string> attachments = new List<string>();
				if (SplitTimeSheet.TSMain.TimesheetID.Equals("-1"))
				{
					SplitTimeSheet.TSMain.StateCode = TSUtilities.ChangeStateIdentity(7, SplitTimeSheet.TSMain.StateCode, "1");
					SplitTimeSheet.TSMain.Notes = "Auto Split time to next week";

					SplitTimeSheet.TSMain.HourRegMax = originalTimeSheet.MemberInforamtion.HourRegMax;
					SplitTimeSheet.TSMain.MemberName = originalTimeSheet.TSMain.MemberName;
					SplitTimeSheet.TSMain.MemberOSN = originalTimeSheet.MemberInforamtion.MemberOSN;
					//SplitTimeSheet.TSMain.Notes = originalTimeSheet.TSMain.Notes;
					SplitTimeSheet.TSMain.OvertimeThreshold = originalTimeSheet.MemberInforamtion.OTThreshold;
					SplitTimeSheet.TSMain.ShiftCode = originalTimeSheet.TSMain.ShiftCode;
					SplitTimeSheet.TSMain.ShiftID = originalTimeSheet.TSMain.ShiftID;

					DBAssist.InsertToDatabase(SplitTimeSheet.TSMain, attachments, SplitTimeSheet.TimeSegmentList, SplitTimeSheet.MemberInforamtion.MemberOSN, DateTime.Now.ToString());
				}
				else
				{
					SplitTimeSheet.TSMain.StateCode = TSUtilities.ChangeStateIdentity(6, originalTimeSheet.TSMain.StateCode, "1");
					DBAssist.InsertToDatabase(SplitTimeSheet.TSMain, attachments, SplitTimeSheet.TimeSegmentList, SplitTimeSheet.MemberInforamtion.MemberOSN, DateTime.Now.ToString());
				}

				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Checks to see if the time should splt to the next payperiod
		/// </summary>
		/// <param name="splitSegment"> segment to check </param>
		/// <param name="myTimesheet"></param>
		/// <returns> true if should split to next payperiod </returns>
		public static bool SplitToNextWeek(TSSegment splitSegment, TimeSheet myTimesheet)
		{
			DateTime OuterSplit = GetSplitDate(/*splitSegment,*/ myTimesheet, false);

			if (splitSegment.Date == OuterSplit.Date || splitSegment.Date == OuterSplit.AddDays(-1).Date)
			{
				if (splitSegment.StartTime >= OuterSplit || splitSegment.EndTime > OuterSplit || splitSegment.EndTime > OuterSplit.AddDays(-1))
				{
					return true;
				}
			}

			return false;


		}

		/// <summary>
		/// Checks to see if the time should splt to the next payperiod
		/// </summary>
		/// <param name="splitSegment"> segment to check </param>
		/// <param name="myTimesheet"></param>
		/// <returns> true if should split to next payperiod </returns>
		public static bool SplitToPreviousWeek(TSSegment splitSegment, TimeSheet myTimesheet)
		{
			DateTime priorSplit = GetSplitDate(myTimesheet, true, true);
			TimeSheet priorTimeSheet = new TimeSheet();
			priorTimeSheet.SetTimesheet(myTimesheet.TSMain.StartDate.AddDays(-14).ToShortDateString(), myTimesheet.MemberInforamtion.MemberUsername);

			if (splitSegment.Date <= priorSplit.Date)
			{
				if (splitSegment.StartTime < priorSplit)
				{
					if(!TSUtilities.ConvertToHex(priorTimeSheet.TSMain.StateCode).Equals("E0") && !TSUtilities.ConvertToHex(priorTimeSheet.TSMain.StateCode).Equals("C0") && !TSUtilities.ConvertToHex(priorTimeSheet.TSMain.StateCode).Equals("DELETED") && !TSUtilities.ConvertToHex(priorTimeSheet.TSMain.StateCode).Equals("LOCKED") && !TSUtilities.ConvertToHex(priorTimeSheet.TSMain.StateCode).Equals("80"))
					{
						return true;
					}
					
				}
			}

			return false;


		}

		/// <summary>
		/// Checks to se if the time requires a split
		/// </summary>
		/// <param name="emptyObject"></param>
		/// <param name="ShiftSplit"></param>
		/// <param name="myTimesheet"></param>
		/// <returns>true if it is required </returns>
		public static bool RequiresTimeSplit(TSSegment emptyObject, DateTime ShiftSplit, TimeSheet myTimesheet)
		{
			/*string test = ShiftSplit.ToString("HHmm");
               string test01 = emptyObject.Date.DayOfWeek.ToString();*/
			DateTime innerSplit = TSValidation.GetSplitDate(/*emptyObject,*/ myTimesheet, true);
			DateTime outerSplit = TSValidation.GetSplitDate(/*emptyObject,*/ myTimesheet, false);
			DateTime priorSplit = TSValidation.GetSplitDate(/*emptyObject,*/ myTimesheet, true, true);
			bool SundaySplits = false;
			//split time is not midnight
			string[] date = emptyObject.Date.ToShortDateString().Split('/');
			DateTime midnight = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(0), Convert.ToInt32(0), 0);
			/*if (ShiftSplit.TimeOfDay <= Convert.ToDateTime("07:00").TimeOfDay) //!@!@!@!@ CHANGE HERE
               {
                    SundaySplits = true;
               }*/
			if (emptyObject.StartTime < innerSplit && emptyObject.EndTime > innerSplit) // FLSA SPLIT !!HAD && myTimesheet.MemberInforamtion.CommStatus!! 3/12/2021
			{
				return true;
			}
			else if (emptyObject.StartTime < outerSplit && emptyObject.EndTime > outerSplit)
			{
				return true;
			}
			else if ((emptyObject.StartTime.Date <= myTimesheet.TSMain.StartDate) && (emptyObject.StartTime < priorSplit && emptyObject.EndTime > priorSplit))
			{
				return true;
			}
			else //Midnight Split
			{
				if ((emptyObject.StartTime != midnight && emptyObject.EndTime != midnight) && (emptyObject.EndTime < emptyObject.StartTime))
				{
					return true;
				}

			}


			return false;

		}

		/// <summary>
		/// Retrieve split time
		/// </summary>
		/// <param name="emptyObject"></param>
		/// <param name="myTimesheet"></param>
		/// <returns> returns the date time of split time</returns>
		public static DateTime GetSplitDateTime(TSSegment emptyObject, TimeSheet myTimesheet)
		{
			string[] splitInput = DBAssist.GetSplitTime(myTimesheet.MemberInforamtion.ShiftCode).Split('_');
			string[] splitTime = splitInput[1].Split(':');
			string splitDay = splitInput[0];

			string[] date = emptyObject.Date.ToShortDateString().Split('/');
			DateTime ShiftSplit = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[0]), Convert.ToInt32(date[1]),
			  Convert.ToInt32(splitTime[0]), Convert.ToInt32(splitTime[1]), 0);
			return ShiftSplit;
		}

		public static DateTime GetSplitDate(/*TSSegment emptyObject,*/ TimeSheet myTimesheet, bool innerWeekSplit, bool priorWeekSplit = false)
		{
			string[] splitInput = DBAssist.GetSplitTime(myTimesheet.MemberInforamtion.ShiftCode).Split('_');
			string[] splitTime = splitInput[1].Split(':');
			string splitDay = splitInput[0];
			int daysTillSplit = 0;
			if (innerWeekSplit)
			{
				if (myTimesheet.MemberInforamtion.CommStatus)// Here
				{
					switch (splitDay)
					{
						case "MO":
							daysTillSplit = 1;
							break;
						case "TU":
							daysTillSplit = 2;
							break;
						case "WE":
							daysTillSplit = 3;
							break;
						case "TH":
							daysTillSplit = 4;
							break;
						case "FR":
							daysTillSplit = 5;
							break;
						case "SA":
							daysTillSplit = 6;
							break;
						case "SU":
							daysTillSplit = 7;
							break;
					}
				}
				else
				{
					switch (splitDay)
					{
						case "MO":
							daysTillSplit = 3;
							break;
						case "TU":
							daysTillSplit = 4;
							break;
						case "WE":
							daysTillSplit = 5;
							break;
						case "TH":
							daysTillSplit = 6;
							break;
						case "FR":
							daysTillSplit = 7;
							break;
						case "SA":
							daysTillSplit = 8;
							break;
						case "SU":
							daysTillSplit = 9;
							break;
					}
				}

			}
			else
			{
				if (myTimesheet.MemberInforamtion.CommStatus)// and here
				{
					switch (splitDay)
					{
						case "MO":
							daysTillSplit = 8;
							break;
						case "TU":
							daysTillSplit = 9;
							break;
						case "WE":
							daysTillSplit = 10;
							break;
						case "TH":
							daysTillSplit = 11;
							break;
						case "FR":
							daysTillSplit = 12;
							break;
						case "SA":
							daysTillSplit = 13;
							break;
						case "SU":
							daysTillSplit = 14;
							break;
					}
				}
				else
				{
					switch (splitDay)
					{
						case "MO":
							daysTillSplit = 10;
							break;
						case "TU":
							daysTillSplit = 11;
							break;
						case "WE":
							daysTillSplit = 12;
							break;
						case "TH":
							daysTillSplit = 13;
							break;
						case "FR":
							daysTillSplit = 14;
							break;
						case "SA":
							daysTillSplit = 15;
							break;
						case "SU":
							daysTillSplit = 16;
							break;
					}
				}
			}
			if (/*myTimesheet.MemberInforamtion.CommStatus &&*/ priorWeekSplit)
			{
				if (innerWeekSplit)
				{
					daysTillSplit -= 7;
				}
				else
				{
					daysTillSplit -= 14;
				}
			}
			string[] date = myTimesheet.TSMain.StartDate.AddDays(daysTillSplit).ToShortDateString().Split('/');

			DateTime ShiftSplit = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[0]), Convert.ToInt32(date[1]),
			  Convert.ToInt32(splitTime[0]), Convert.ToInt32(splitTime[1]), 0);
			return ShiftSplit;
		}



		#endregion

		#region Time Validation



		/// <summary>
		/// Checks timesheet to see if anything has changed on save and then correcty any incorrect timesegments
		/// </summary>
		/// <param name="myTimeSheet"></param>
		/// <remarks> !!!THIS IS NOT IN USE, to process heavy at the moment, needs to be refined!!!</remarks>
		/// <returns> true if their is a time overlap conflict</returns>
		public static bool ValidateModifications(TimeSheet myTimeSheet)//Users can no longer modify time segments 
		{
			bool conflict = false;
			List<TSSegment> newItemList = new List<TSSegment>();
			List<TSSegment> noModifications = new List<TSSegment>();
			List<TSSegment> newSplits = new List<TSSegment>();
			DateTime splitTime = TSValidation.GetSplitDateTime(myTimeSheet.TimeSegmentList[0], myTimeSheet);
			//checkList.AddRange(myTimeSheet.TimeSegmentList);
			TimeSheet checkTimeSheet = new TimeSheet();

			checkTimeSheet.MemberInforamtion = myTimeSheet.MemberInforamtion;
			checkTimeSheet.TSMain = myTimeSheet.TSMain;
			checkTimeSheet.MemberInforamtion = myTimeSheet.MemberInforamtion;
			checkTimeSheet.TotalList = myTimeSheet.TotalList;

			TSSegment test = new TSSegment();

			try
			{
				foreach (TSSegment item in myTimeSheet.TimeSegmentList)
				{
					if (item.StateCode.Substring(7, 1).Equals("1") || item.StateCode.Substring(6, 1).Equals("1"))
					{
						newItemList.Add(item);
					}
					else
					{
						noModifications.Add(item);
					}
				}


				foreach (TSSegment newItem in newItemList)
				{

					if (TSValidation.RequiresTimeSplit(newItem, splitTime, myTimeSheet)) // Add && !emptyObject.HourType.Equals("OT")
					{
						// get split time and updated original time
						List<TSSegment> TimeSegmentList = TSValidation.GetSplitTimeSegment(newItem, myTimeSheet);
						// empty object OG in [0] , Split one [1], Split two [2]

						newItem.StartTime = TimeSegmentList[0].StartTime;
						newItem.EndTime = TimeSegmentList[0].EndTime;
						newItem.Date = TimeSegmentList[0].Date;

						/*TSSegment splitSegmentA = new TSSegment();
                              TSSegment splitSegmentB = null;*/
						if (TimeSegmentList.Count <= 2)
						{
							newSplits.Add(TimeSegmentList[1]);


						}
						else
						{
							newSplits.Add(TimeSegmentList[1]);
							newSplits.Add(TimeSegmentList[2]);
						}

						if (TSValidation.ValidateFTO(newItem, myTimeSheet))
						{
							//lblAlertText.Text = "The Information provided is missing the required Reg/OT needed for the entered FTO";
							return true;
						}
						if (TSValidation.ValidateFTO(TimeSegmentList[1], myTimeSheet))
						{
							//lblAlertText.Text = "The Information provided is missing the required Reg/OT needed for the entered FTO";
							return true;
						}
						if (TimeSegmentList.Count > 2)
						{
							if (TSValidation.ValidateFTO(TimeSegmentList[2], myTimeSheet))
							{
								//lblAlertText.Text = "The Information provided is missing the required Reg/OT needed for the entered FTO";
								return true;
							}
						}

						//split time is in current time period

						// myTimeSheet.TimeSegmentList.Add(emptyObject);
					}
				}
				if (newSplits.Count > 0)
				{
					newItemList.AddRange(newSplits);
				}

				checkTimeSheet.TimeSegmentList = noModifications;

				foreach (TSSegment NewItem in newItemList)
				{
					conflict = TimeValidationV2(NewItem, checkTimeSheet);
				}

			}
			catch (Exception e)
			{

			}
			if (!conflict && newSplits.Count > 0)
			{
				myTimeSheet.TimeSegmentList.AddRange(newSplits);
			}

			return conflict;
		}

		/// <summary>
		/// Time Validation Main Method
		/// Input new time segment and timehseet to check
		/// Returns true if their is an error
		/// Runs The Following Validation methods:
		/// --TimeValidationFullCheck
		/// --GetTimesheetInformation_V2
		/// --TimeValidationInnerCheck
		/// --TimeValidationOuterCheck
		/// --TimeValidationStartCheck
		/// --TimeValidationEndCheck
		/// </summary>
		/// <param name="emptyObject"></param>
		/// <param name="myTimesheet"></param>
		/// <returns></returns>
		public static bool TimeValidationV2(TSSegment emptyObject, TimeSheet myTimesheet)
		{
			if (emptyObject.StateCode.Substring(5, 1).Equals("0") && !emptyObject.HourType.Equals("FTO")) //make sure its not a deleted segment and not FTO, FTO has its own validation
			{
				if (!TimeValidationFullCheck(myTimesheet, emptyObject))
				{
					return false;
				}

				TimeSheet previousTimeSheet = new TimeSheet();
				previousTimeSheet.SetTimesheet(myTimesheet.TSMain.StartDate.AddDays(-14).ToShortDateString(), myTimesheet.MemberInforamtion.MemberUsername);
				foreach (TSSegment previousDate in previousTimeSheet.TimeSegmentList)
				{
					if (emptyObject.Date == previousDate.Date)
					{
						if (!TimeValidationInnerCheck(previousDate, emptyObject) || !TimeValidationOuterCheck(previousDate, emptyObject)
						  || !TimeValidationStartCheck(previousDate, emptyObject) || !TimeValidationEndCheck(previousDate, emptyObject))
						{
							return false;
						}
					}
				}

				TimeSheet futureTimeSheet = new TimeSheet();
				futureTimeSheet.SetTimesheet(myTimesheet.TSMain.StartDate.AddDays(14).ToShortDateString(), myTimesheet.MemberInforamtion.MemberUsername);
				foreach (TSSegment futureDate in futureTimeSheet.TimeSegmentList)
				{
					if (emptyObject.Date == futureDate.Date)
					{
						if (!TimeValidationInnerCheck(futureDate, emptyObject) || !TimeValidationOuterCheck(futureDate, emptyObject)
						  || !TimeValidationStartCheck(futureDate, emptyObject) || !TimeValidationEndCheck(futureDate, emptyObject))
						{
							return false;
						}
					}
				}
			}
			else
			{
				foreach (TSSegment Item in myTimesheet.TimeSegmentList)
				{
					if (emptyObject.StateCode.Substring(5, 1).Equals("0"))
					{
						if (Item.HourType.Equals("FTO") && emptyObject.HourType.Equals("FTO"))
						{
							if (!TimeValidationInnerCheck(Item, emptyObject) || !TimeValidationOuterCheck(Item, emptyObject)
							   || !TimeValidationStartCheck(Item, emptyObject) || !TimeValidationEndCheck(Item, emptyObject))
							{
								return false;
							}
						}
					}
				}
			}

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="myTimeSheet"></param>
		/// <param name="newObject"></param>
		/// <returns></returns>
		public static bool TimeValidationFullCheck(TimeSheet myTimeSheet, TSSegment newObject = null)
		{
			//check normal cases
			foreach (TSSegment item in myTimeSheet.TimeSegmentList)
			{
				TSSegment checkObject = item;
				newObject.SegmentCleaner();
				checkObject.SegmentCleaner();

				//Skip Cases
				if (!checkObject.StateCode.Substring(5, 1).Equals("0"))
				{
					continue;
				}
				if (checkObject.HourType.Equals("FTO") || newObject.HourType.Equals("FTO"))
				{
					if (newObject.HourType.Equals("FTO"))
					{
						if (!TimeValidationInnerCheck(checkObject, newObject) || !TimeValidationOuterCheck(checkObject, newObject)
					   || !TimeValidationStartCheck(checkObject, newObject) || !TimeValidationEndCheck(checkObject, newObject))
						{

							return false;
						}
					}
					else
					{
						continue;
					}
				}
				// a new segment is spread through 2 days. ignore checkObject end time
				if (TimeValidationMidnightCheck(checkObject, newObject))
				{
					if ((newObject.StartTime < checkObject.StartTime || newObject.StartTime < checkObject.EndTime)
					  && (newObject.Date == checkObject.Date) || newObject.StartTime == newObject.EndTime
					  || (newObject.StartTime == checkObject.StartTime && newObject.EndTime == checkObject.EndTime)
					  || !TimeValidationInnerCheck(checkObject, newObject, 1)) //GOOD
					{
						return false;
					}
					foreach (TSSegment subcheckObject in myTimeSheet.TimeSegmentList)
					{
						if (subcheckObject.Date == newObject.Date.AddDays(1) && newObject.EndTime > subcheckObject.StartTime
						   && (newObject.HourType == subcheckObject.HourType || subcheckObject.HourType.Equals("OT") || subcheckObject.HourType.Equals("Reg")))
						{
							return false;
						}
					}
				}
				else if (!TimeValidationInnerCheck(checkObject, newObject) || !TimeValidationOuterCheck(checkObject, newObject)
				   || !TimeValidationStartCheck(checkObject, newObject) || !TimeValidationEndCheck(checkObject, newObject))
				{

					return false;
				}
			}
			return true;
		}

		//Time Validation pending leave check 
		public static bool PendingLeaveCheck(TimeSheet myTimeSheet, TSSegment newObject, List<TSLeaveReq> myPendingLeave)
		{

			foreach (TSLeaveReq leave in myPendingLeave)
			{
				if (leave.DateRange.Contains("-"))//date range
				{
					DateTime tempStartLeave = Convert.ToDateTime(leave.DateRange.Substring(0, leave.DateRange.IndexOf('-')));
					DateTime tempEndLeave = Convert.ToDateTime(leave.DateRange.Split('-').Last());
					if (newObject.Date > tempStartLeave && newObject.Date < tempEndLeave && (leave.Status.Equals("APPROVED") || leave.Status.Equals("SUBMITTED"))) // new time with range
					{
						return true;
					}
					else if (newObject.Date == tempStartLeave || newObject.Date == tempEndLeave && (leave.Status.Equals("APPROVED") || leave.Status.Equals("SUBMITTED"))) // new time on start or end days
					{
						if ((newObject.StartTime >= leave.StartTime) || (newObject.EndTime < leave.EndTime))
						{
							return true;
						}
					}
				}
				else if ((Convert.ToDateTime(leave.DateRange) == newObject.Date) && (leave.Status.Equals("APPROVED") || leave.Status.Equals("SUBMITTED")))// single date
				{
					TSSegment tempDate = new TSSegment
					{
						Date = newObject.Date,
						StartTime = leave.StartTime,
						EndTime = leave.EndTime
					};

					tempDate.SegmentCleaner();

					if (!TimeValidationInnerCheck(tempDate, newObject) || !TimeValidationOuterCheck(tempDate, newObject)
					|| !TimeValidationStartCheck(tempDate, newObject) || !TimeValidationEndCheck(tempDate, newObject))
					{
						return true;
					}
				}
			}
			return false;
		}

		//Time Validation Midnight Check
		public static bool TimeValidationMidnightCheck(TSSegment Item, TSSegment checkItem = null)
		{
			if (checkItem.StartTime > checkItem.EndTime || checkItem.EndTime == Convert.ToDateTime("00:00")
			  && (checkItem.Date == Item.Date) || checkItem.StartTime == checkItem.EndTime)
			{
				return true;
			}
			return false;
		}

		//Time Validation Inner Check
		public static bool TimeValidationInnerCheck(TSSegment Item, TSSegment newObject = null, int NextDay = 0)
		{
			// new segment start is within old segment
			if (newObject.StartTime >= Item.StartTime && newObject.StartTime < Item.EndTime && !Item.EndTime.ToString("HH:mm").Equals("00:00") && ((newObject.Date == Item.Date) || (newObject.Date.AddDays(NextDay) == Item.Date)))
			{
				return false;
			}
			// new segment encapsulates old segment
			else if (newObject.StartTime <= Item.StartTime && newObject.EndTime.AddDays(NextDay) >= Item.EndTime && !Item.EndTime.ToString("HH:mm").Equals("00:00") && ((newObject.Date == Item.Date) || (newObject.Date.AddDays(NextDay) == Item.Date)))
			{
				return false;
			}
			// start time outside but end time is within
			else if (newObject.StartTime <= Item.StartTime && newObject.EndTime.AddDays(NextDay) <= Item.EndTime && newObject.EndTime.AddDays(NextDay) > Item.StartTime && ((newObject.Date == Item.Date) || (newObject.Date.AddDays(NextDay) == Item.Date)))
			{
				return false;
			}
			// stuff and things
			else if (newObject.EndTime > Item.StartTime && newObject.EndTime.AddDays(NextDay) < Item.EndTime && ((newObject.Date == Item.Date) || (newObject.Date.AddDays(NextDay) == Item.Date)))
			{
				return false;
			}

			return true;
		}

		//Check, return true if failed //Drew: If the new segment completly overlaps the old
		public static bool TimeValidationOuterCheck(TSSegment Item, TSSegment newObject = null)
		{
			if (((newObject.StartTime < Item.StartTime && newObject.EndTime > Item.EndTime)
			  && (newObject.Date == Item.Date) || newObject.StartTime == newObject.EndTime) && !Item.EndTime.ToString("HH:mm").Equals("00:00")) //
			{
				return false;
			}

			return true;
		}

		//Check, return true if failed , checks if start time is within Item's time frame 
		public static bool TimeValidationStartCheck(TSSegment Item, TSSegment newObject = null)
		{
			if ((newObject.StartTime > Item.StartTime && newObject.EndTime > Item.EndTime)
			  && (Item.EndTime > newObject.StartTime)
			  && (newObject.Date == Item.Date) || newObject.StartTime == newObject.EndTime) //GOOD
			{
				return false;
			}
			return true;
		}

		//Check, return true if failed, checks if end time is within Items's time frame
		public static bool TimeValidationEndCheck(TSSegment Item, TSSegment newObject = null)
		{
			if ((newObject.StartTime < Item.StartTime && newObject.EndTime < Item.EndTime && Item.StartTime < newObject.EndTime)
			  && (newObject.Date == Item.Date) || newObject.StartTime == newObject.EndTime) //
			{
				return false;
			}

			return true;
		}

		public static decimal TotalHoursFTO(TimeSheet myTimesheet)
		{
			decimal total = 0;

			foreach (TSSegment item in myTimesheet.TimeSegmentList)
			{
				if (item.HourType.Equals("FTO"))
				{
					total += item.SegmentHours;
				}
			}

			return total;
		}

		public static bool ValidateFTO(TSSegment newObject, TimeSheet myTimesheet)
		{
			//FTO can only go over Reg and Overtime
			// FTO hours then overtime 14 hours of fto over 12reg and 2ot 

			if (!newObject.HourType.Equals("FTO"))
			{
				return false;
			}
			if (myTimesheet.TimeSegmentList.Count == 0)
			{
				return true;
			}
			//if(TSValidation.RequiresTimeSplit(newObject,TSV))
			else
			{
				int matches = 0;
				foreach (TSSegment item in myTimesheet.TimeSegmentList)
				{
					if ((item.HourType.Equals("OT") || item.HourType.Equals("Reg")) && item.StateCode.Substring(5, 1).Equals("0") && item.Date == newObject.Date)
					{//add that item to a list for future checks
						matches++;
					}
				}
				if (matches == 0)
				{
					return true;
				}
			}

			TSSegment testCase = newObject;
			List<TSSegment> dateSeg = new List<TSSegment>();

			bool conflict = false;

			foreach (TSSegment item in myTimesheet.TimeSegmentList)
			{
				if (!item.StateCode.Substring(5, 1).Equals("0"))
				{
					continue;
				}
				if (item.Date == newObject.Date)//midnight split?
				{
					if (newObject.StartTime >= item.StartTime && newObject.EndTime <= item.EndTime && newObject.StartTime < item.EndTime && newObject.EndTime > item.StartTime)
					{//checked within and found a good match 
						if (item.HourType.Equals("Leave"))
						{
							conflict = true;
							continue;
						}
						conflict = false;
						return conflict;
					}
					if (item.HourType.Equals("OT") || item.HourType.Equals("Reg"))
					{//add that item to a list for future checks
						dateSeg.Add(item);
					}
					if (newObject.StartTime < item.StartTime || newObject.EndTime > item.EndTime)
					{//checks for overhang
						conflict = true;
					}
				}
			}

			if (conflict && dateSeg.Count > 0)
			{// make sure that the conflict is not Reg segments or OT segments right next to eachother

				bool startWithinCheck = false;
				bool endWithinCheck = false;

				// resort segment list by times
				dateSeg = TSUtilities.SortDayByTime(dateSeg);

				// check if FTO can cover continues time 
				// 05:00-06:00 then 06:00-07:00 add FTO of 05:00-07:00 should be correct
				for (int i = 0; i < dateSeg.Count; i++)
				{
					if (dateSeg[i].StartTime > dateSeg[i].EndTime)
					{//midnight time
						if (newObject.StartTime >= dateSeg[i].StartTime && (newObject.EndTime >= dateSeg[i].StartTime || newObject.EndTime <= dateSeg[i].EndTime))
						{
							conflict = false;
							break;
						}
					}
					//make sure the start time is within the object
					if (newObject.StartTime >= dateSeg[i].StartTime && newObject.StartTime <= dateSeg[i].EndTime)
					{
						startWithinCheck = true;
					}
					else
					{
						startWithinCheck = false;
					}

					if (newObject.EndTime >= dateSeg[i].StartTime && newObject.EndTime <= dateSeg[i].EndTime)
					{

						endWithinCheck = true;
					}
					else
					{
						if (i != dateSeg.Count - 1 && startWithinCheck)
						{
							int k = i;
							for (int j = i + 1; j < dateSeg.Count; j++, k++)
							{
								//Add Check for midnight split / split time split

								if (dateSeg[k].EndTime == dateSeg[j].StartTime)
								{
									if (newObject.EndTime >= dateSeg[j].StartTime && newObject.EndTime <= dateSeg[j].EndTime)
									{
										endWithinCheck = true;
									}
								}

							}

						}
						else
						{
							endWithinCheck = false;
						}
					}
					if (startWithinCheck && endWithinCheck)
					{
						conflict = false;
						break;
					}
					else
					{
						startWithinCheck = false;
						endWithinCheck = false;
					}
				}
			}

			return conflict;
		}
	}

	#endregion
}