using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Extension methods on DateTime objects
    /// </summary>
    public static class DateTimeExtensions
    {
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
            return value.AddDays(target - start);
        }
    }
}
