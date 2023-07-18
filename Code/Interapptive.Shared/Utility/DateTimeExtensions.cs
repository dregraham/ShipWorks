using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Extension methods on DateTime objects
    /// </summary>
    public static class DateTimeExtensions
    {
        private static readonly DateTime safeMinDateTime = new DateTime(1970, 1, 1, 0, 0, 0);

        /// <summary>
        /// Yesterdays date
        /// </summary>
        public static DateTime Yesterday => DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0));
        
        /// <summary>
        /// Get a value that is at least the minimum value specified
        /// </summary>
        /// <returns>The value if greater than the minimum, otherwise the minimum</returns>
        public static DateTime AtLeast(this DateTime value, DateTime minimum) =>
            value.Clamp(minimum, DateTime.MaxValue);

        /// <summary>
        /// Clamp a value to an allowable range
        /// </summary>
        public static DateTime Clamp(this DateTime value, DateTime left, DateTime right)
        {
            var (min, max) = left > right ?
                (right, left) :
                (left, right);

            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }

        /// <summary>
        /// Gets the next day specified (if current day requested, returns the next one)
        /// </summary>
        public static DateTime Next(this DateTime value, DayOfWeek dayOfWeek)
        {
            return value.AddDays(1).TodayOrNext(dayOfWeek);
        }

        /// <summary>
        /// Gets the next day specified (if the day requested is today, returns today)
        /// </summary>
        public static DateTime TodayOrNext(this DateTime value, DayOfWeek dayOfWeek)
        {
            int start = (int) value.DayOfWeek;
            int target = (int) dayOfWeek;
            if (target < start)
            {
                target += 7;
            }
            return value.AddDays(target - start);
        }

        /// <summary>
        /// Gets the next day specified (if current day requested, returns the next one)
        /// </summary>
        public static DateTime ToSqlSafeDateTime(this DateTime value)
        {
            return ToSqlSafeDateTime(value, safeMinDateTime);
        }

        /// <summary>
        /// Gets the next day specified (if current day requested, returns the next one)
        /// </summary>
        public static DateTime ToSqlSafeDateTime(this DateTime value, DateTime valueIfNotSafe)
        {
            return value >= safeMinDateTime ?
                value :
                valueIfNotSafe >= safeMinDateTime ? valueIfNotSafe : safeMinDateTime;
        }

        public static DateTime ToUniversalTime(this DateTime value, TimeZoneInfo timeZoneInfo)
        {
            if (value.Kind == DateTimeKind.Utc)
            {
                return value;
            }
            var valueWithoutKind = new DateTime(value.Ticks, DateTimeKind.Unspecified);
            if (timeZoneInfo.IsAmbiguousTime(valueWithoutKind))
            {
                return value;
            }
            return TimeZoneInfo.ConvertTimeToUtc(valueWithoutKind, timeZoneInfo);
        }

        public static DateTime ToLocalTime(this DateTime value, TimeZoneInfo timeZoneInfo)
        {
            if (value.Kind == DateTimeKind.Local)
            {
                return value;
            }
            return TimeZoneInfo.ConvertTimeFromUtc(value, timeZoneInfo);
        }
    }
}
