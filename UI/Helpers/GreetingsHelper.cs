using System;
using System.Text.RegularExpressions;
using UI.Models;

namespace UI.Helpers
{
	public static class GreetingsHelper
	{
		public static void SetGreetings(string userName, string cookieName)
		{
			string greetings = SetupGreetings(userName);
			CookieHelper.SetCookie(cookieName, greetings, DateTime.Now.AddDays(1));
		}
		private static string SetupGreetings(string userName)
		{
			DateTime dt = DateTime.UtcNow;
			string client = dt.ToClientTime();
			// client = 14/9/2014 16:26:50
			const string pattern = @"(\s\d{2})";
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
			int hour = 0;
			if (regex.IsMatch(client))
			{
				MatchCollection matches = regex.Matches(client);
				foreach (Match match in matches)
				{
					hour = Convert.ToInt32(match.Groups[1].Value);
					break;
				}

				string greetings = null;
				string culture = CookieHelper.GetCookieValue("_culture");
				switch (culture)
				{
					case null: // => set default value
					case "en-us": // => set default value
					case "zh-hans":
					case "zh-hant":
						if (hour <= 12)
							greetings = SiteConfiguration.GoodMorning;
						else if (hour > 12 && hour <= 17)
							greetings = SiteConfiguration.GoodAfternoon;
						else if (hour > 17) greetings = SiteConfiguration.GoodEvening;
						break;
					case "ja-jp":
						if (hour <= 12)
							greetings = SiteConfiguration.GoodMorningJap;
						else if (hour > 12 && hour <= 17)
							greetings = SiteConfiguration.GoodAfternoonJap;
						else if (hour > 17) greetings = SiteConfiguration.GoodEveningJap;
						break;
				}
				return string.Format("{0} {1}", userName, greetings);
			}
			return null;
		}

	}
}