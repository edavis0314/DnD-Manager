using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCSOTicketManager.Classes
{
	public class PageIDLooper
	{
		public static List<string> GetIDsList(Page myPage, List<string> myIDs)
		{
			List<string> tempList = new List<string>();
			var mySuperContent = myPage.Master.Controls;
			var myContent = (myPage.Master.FindControl("MainContent") as ContentPlaceHolder).Controls;
			var mySubContent = myPage.Controls;

			Debug.WriteLine("Processing: " + myPage + " super content");
			FrontendIDLooper(myIDs, mySuperContent);

			Debug.WriteLine("Processing: " + myPage + " content");
			FrontendIDLooper(myIDs, myContent);

			Debug.WriteLine("Processing: " + myPage + " sub content");
			FrontendIDLooper(myIDs, mySubContent);

			Debug.WriteLine("Processing: " + myPage + " ID List");
			foreach (string temp in myIDs)
			{
				if (!temp.Contains("|"))
				{
					if (temp.StartsWith("~"))
						tempList.Add("site.master|" + temp);
					else
						tempList.Add(myPage.ToString().Substring(4, myPage.ToString().IndexOf("_") - 4) + "|" + temp);
				}
				else
				{
					tempList.Add(temp);
				}
			}
			tempList = tempList.OrderBy(o => o.ToString()).ToList();
			return tempList;
		}

		public static void StoreList (List<string> myIDs)
		{
			Debug.WriteLine("Storing to DB");
			DBStoredProcedure.ResetFrontEndIDValues();

			foreach(string id in myIDs)
			{
				Debug.WriteLine("Processing: " + id);
				DBStoredProcedure.InsertData_FrontEndIDValues(id);
			}

			Debug.WriteLine("Complete");
		}

		public static void FrontendIDLooper(List<string> myIDs, ControlCollection myContent)
		{
			for (int i = 0; i < myContent.Count; i++)
			{
				var myUpdatePanel = myContent[i].Controls;
				for (int j = 0; j < myUpdatePanel.Count; j++)
				{
					var myUpperControl = myUpdatePanel[j].Controls;
					for (int k = 0; k < myUpperControl.Count; k++)
					{
						ProcessIDLooper(myIDs, myUpperControl[k].ClientID, myUpperControl[k], true);
					}
					if (!myUpdatePanel[j].ToString().ToLower().Contains("literalcontrol"))
					{
						ProcessIDLooper(myIDs, myUpdatePanel[j].ClientID, myUpdatePanel[j]);
					}
				}
			}
		}

		public static void ProcessIDLooper(List<string> myIDs, string myContent, Control myControl, bool keepGoing = false)
		{
			string tempType = "";
			string tempID = "";
			string tempPageName = "";

			if(!myControl.GetType().ToString().Equals(""))
				tempType = myControl.GetType().ToString().Substring(myControl.GetType().ToString().LastIndexOf(".") + 1);

			if (!(myContent is null))
			{
				if(myContent.Contains("_"))
					tempID = myContent.Substring(myContent.IndexOf("_") + 1);

				tempPageName = myContent.ToString();
			}



			//This gets the ID and the Page Name in the format that we want
			while (tempID.Contains("_") && !tempID.StartsWith("UP_"))
			{
				tempPageName = tempPageName.Substring(tempPageName.IndexOf("_") + 1);
				tempID = tempID.Substring(tempID.IndexOf("_") + 1);
			}
			if(tempPageName.Contains("_"))
				tempPageName = tempPageName.Substring(0, tempPageName.IndexOf("_"));

			if (tempID.Equals(""))
			{
				tempID = tempPageName;
				tempPageName = "";
			}



			//Stores the ID if it is a valid ID for the application
			if (!(tempType.ToLower().Contains("table") || tempType.ToLower().Contains("panel") || tempType.ToLower().Contains("control") || tempType.ToLower().Contains("hiddenfield")
				|| tempType.ToLower().Equals("") || tempType.ToLower().Contains("html") || tempType.ToLower().Contains("placeholder") || tempID.ToLower().StartsWith("ctl")
				|| tempID.ToLower().Contains("rq") || tempType.ToLower().Contains("menu")) 
				&& !(myIDs.Contains(tempPageName + "~" + tempID + "~" + tempType) || myIDs.Contains("site.master|" + tempPageName + "~" + tempID + "~" + tempType)))
			{
				myIDs.Add(tempPageName + "~" + tempID + "~" + tempType);
			}

			//if the application has a sub page, aka controller, this will allow the checker to process the controller
			if (keepGoing)
			{
				var myController = myControl.Controls;
				for (int l = 0; l < myController.Count; l++)
				{
					FrontendIDLooper(myIDs, myController);
				}
			}
		}
	}
}
