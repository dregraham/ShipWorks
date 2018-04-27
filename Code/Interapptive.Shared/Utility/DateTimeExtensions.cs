using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Extension methods on DateTime objects
    /// </summary>
    public static class DateTimeExtensions
    {
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
    }
}
