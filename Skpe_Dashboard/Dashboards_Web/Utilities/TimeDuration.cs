using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboards_Web.Utilities
{
    /// <summary>
    /// This class is utility class which helps for the convertion of time duration
    /// </summary>
    public static class TimeDuration
    {
        public static string SortingOrder = "asc";
        public static string AscSort = "asc";
        public static string DescSort = "desc";

        public static double ConvertTicketAgeDurationToMins(string duration)
        {
            double durationAge = Convert.ToInt32(duration.Substring(0, duration.IndexOf(' ')));
            if (duration.ToLower().Contains("days"))
            {
                return durationAge * 24 * 60;
            }
            else if (duration.ToLower().Contains("hours"))
            {
                return durationAge * 60;
            }
            else
            {
                return durationAge;
            }
        }

        public static string CovertTicketAgeMinsToDuration(double mins)
        {
            TimeSpan t = TimeSpan.FromMinutes(mins);
            if (t.Days != 0)
            {
                return t.Days == 1 ? t.Days + " Day" : t.Days + " Days";
            }
            else if (t.Hours != 0)
            {
                return t.Hours == 1 ? t.Hours + " Hour" : t.Hours + " Hours";
            }
            else
            {
                return t.Minutes == 1 ? t.Minutes + " Minute" : t.Minutes + " Minutes";
            }
        }

        public static double ConvertTTETTADurationToMinutes(string duration)
        {
            if (string.IsNullOrEmpty(duration))
            {
                return 0;
            }

            double tte = Convert.ToInt32(duration.Substring(0, duration.IndexOf(' ')));
            if (duration.ToLower().Contains("days"))
            {
                tte = tte * 24 * 60;
            }
            else if (duration.ToLower().Contains("hours"))
            {
                tte = tte * 60;
            }
            if (duration.ToLower().Contains("overdue"))
            {
                tte *= -1;
            }
            return tte;
        }

        public static string CovertTTETTAMinsToDuration(double min)
        {
            TimeSpan t = TimeSpan.FromMinutes(min);
            string tteDuration = string.Empty;
            if (t.Days != 0)
            {
                tteDuration += t.Days == 1 ? t.Days + " Day" : t.Days + " Days";
            }
            else if (t.Hours != 0)
            {
                tteDuration += t.Hours == 1 ? t.Hours + " Hour" : t.Hours + " Hours";
            }
            else
            {
                tteDuration += t.Minutes == 1 ? t.Minutes + " Minute" : t.Minutes + " Minutes";
            }
            if (min < 0)
            {
                tteDuration += " Overdue";
            }
            else
            {
                tteDuration += " Left";
            }
            return tteDuration.Contains("-") ? tteDuration.Replace("-", string.Empty) : tteDuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ToggleSortingOrder(string ttaDir,string tteDir)
        {
            if (string.IsNullOrEmpty(ttaDir) && string.IsNullOrEmpty(tteDir))
            {
                SortingOrder = DescSort;
            }
            else
            {
                if (SortingOrder.Contains(AscSort))
                {
                    SortingOrder = DescSort;
                }
                else
                {
                    SortingOrder = AscSort;
                }
            }
        }
    }
}