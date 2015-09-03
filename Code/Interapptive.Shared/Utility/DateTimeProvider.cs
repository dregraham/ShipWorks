using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Provides a concrete implementation of IDateProvider that uses the DateTime object
    /// </summary>
    public class DateTimeProvider : IDateTimeProvider
    {
        /// <summary>
        /// Gets the current date and time
        /// </summary>
        public DateTime Now => DateTime.Now;

        /// <summary>
        /// Gets the current UTC date and time
        /// </summary>
        public DateTime UtcNow => DateTime.UtcNow;

        /// <summary>
        /// Gets the current date, with time set to 00:00:00
        /// </summary>
        public DateTime Today => DateTime.Today;
    }
}
