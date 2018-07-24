using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Extension methods for the timespan object
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Multiplies a timespan
        /// </summary>
        public static TimeSpan MultiplyBy(this TimeSpan timespan, int multiplier) =>
            TimeSpan.FromTicks(timespan.Ticks * multiplier);
    }
}
