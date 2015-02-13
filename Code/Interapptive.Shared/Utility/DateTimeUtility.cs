using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Utility methods for DateTime
    /// </summary>
    public static class DateTimeUtility
    {
        private static DateTime epoch = new System.DateTime(1970, 1, 1, 0, 0, 0);

        /// <summary>
        /// Parse the date\time string in en-US culture regardless of the culture of the OS
        /// </summary>
        public static DateTime ParseEnUS(string dateTime)
        {
            try
            {
                return DateTime.Parse(dateTime, CultureInfo.CreateSpecificCulture("en-US"));
            }
            catch (FormatException ex)
            {
                throw new FormatException(string.Format("DateTimeUtility.ParseEnUs cannot parse datetime '{0}'", dateTime), ex);
            }
        }

        /// <summary>
        /// Returns a Unix Epoch date from the passed in date.
        /// </summary>
        public static double ToUnixTimestamp(DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime - epoch;
            return Math.Floor(timeSpan.TotalSeconds);
        }

        /// <summary>
        /// Returns a datetime, given a unix epoch timestamp.
        /// </summary>
        public static DateTime FromUnixTimestamp(double timestamp)
        {
            return epoch.AddSeconds(timestamp);
        }

        /// <summary>
        /// Converts a date time to the specified time zone.  If the converted time falls during DST, 2 hours will be added and returned.
        /// </summary>
        public static DateTime ConvertTimeToUtcForTimeZone(DateTime sourceDateTime, TimeZoneInfo timeZoneInfo)
        {
            try
            {
                return TimeZoneInfo.ConvertTimeToUtc(sourceDateTime, timeZoneInfo);
            }
            catch (ArgumentException)
            {
                return TimeZoneInfo.ConvertTimeToUtc(sourceDateTime.AddHours(2), timeZoneInfo);
            }
        }

        /// <summary>
        /// Determines if a date is a business day
        /// </summary>
        public static bool IsBusinessDay(this DateTime value)
        {
            if (value.DayOfWeek == DayOfWeek.Sunday) return false;
            if (value.DayOfWeek == DayOfWeek.Saturday) return false;

            return true;
        }
    }
}
