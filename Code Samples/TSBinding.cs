using System;
using System.Data;
using System.Web.UI;
using System.Web;

namespace CCSOTimeSheet
{
	public class TSBinding
	{
		//This is a funtion that handles generating User Information
		static public TimeSheet GetUserData(string overrideUser, Page myPage)
		{
			TimeSheet myTimesheet = new TimeSheet();
			string user = myPage.User.Identity.Name.Replace(@"CCSO\", "").ToUpper();
			string oldUser = "";
			string overrideSessionContent;
			if (!overrideUser.Equals(""))
			{
				oldUser = user;
				user = overrideUser;
			}
			else if (HttpContext.Current.Session["override"] != null)
			{
				overrideSessionContent = HttpContext.Current.Session["override"].ToString();
				if (!overrideSessionContent.Equals(""))
				{
					user = HttpContext.Current.Session["override"].ToString();
				}
			}
			
			myTimesheet.SetTimesheet(DateTime.Now.ToShortDateString(), user);

			//if (!myTimesheet.MemberInforamtion.UserActive)
				//HttpContext.Current.Response.Redirect("Error_Page.aspx", true);

			HttpContext.Current.Session["currentMemberTimesheet"] = myTimesheet;
			HttpContext.Current.Session["currentMemberInformation"] = myTimesheet.MemberInforamtion;
			HttpContext.Current.Session["currentMemberWorkCode"] = myTimesheet.MemberInforamtion.WorkCodes;

			if (!myPage.Request.Url.AbsoluteUri.Contains("Permissions"))
			{
				HttpContext.Current.Session["currentMemberPosNum"] = myTimesheet.MemberInforamtion.PosNumber.Trim();
			}

			//myPage fills out user work codes on initial load. make sure to heck if we are overriding a user to get their supervisors
			if (Convert.ToString(HttpContext.Current.Session["supDTCheck"]).Equals("") || !user.Equals(oldUser))
			{

				HttpContext.Current.Session["supDTCheck"] = "TRUE";
			}

			HttpContext.Current.Session["supervisorDataTable"] = DBAssist.FillSupervisorDropdown(osn: myTimesheet.MemberInforamtion.MemberOSN.ToString());

			return myTimesheet;
		}
	}
}