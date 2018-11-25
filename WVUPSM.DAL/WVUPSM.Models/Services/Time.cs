using System;
using System.Collections.Generic;
using System.Text;

namespace WVUPSM.Models.Services
{
    /// <summary>
    ///     Helper Methods dealing with Time
    /// </summary>
    public static class Time
    {
        /// <summary>
        ///     Returns a string representing how long it's been since the passed in datetime
        /// </summary>
        /// <param name="dateTime">the time to compare</param>
        public static string TimeSince(DateTime dateTime)
        {
            var now = DateTime.Now;
            var difference = now - dateTime;
            if (difference.Days == 1)
            {
                return $"{difference.Days} day ago";
            }
            else if (difference.Days > 0)
            {
                return $"{difference.Days} days ago";
            }
            else if (difference.Hours == 1)
            {
                return $"{difference.Hours} hour ago";
            }
            else if (difference.Hours > 0)
            {
                return $"{difference.Hours} hours ago";
            }
            else if (difference.Minutes == 1)
            {
                return $"{difference.Minutes} minute ago";
            }
            else if (difference.Minutes > 0)
            {
                return $"{difference.Minutes} minutes ago";
            }
            else if (difference.Seconds == 1)
            {
                return $"{difference.Seconds} second ago";
            }
            else if (difference.Seconds > 0)
            {
                return $"{difference.Seconds} seconds ago";
            }
            else if (difference.Seconds < 0)
            {
                return $"{difference.Hours} seconds in the future???";
            }
            else
            {
                return "Just now";
            }
        }
    }
}
