using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.Common
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
        /// <remarks>The method version can be used to make testing easier when passing a method reference
        /// to another method.</remarks>
        public DateTime UtcNow => DateTime.UtcNow;

        /// <summary>
        /// Gets the current Epoc in seconds
        /// </summary>
        public double Epoc => DateTimeUtility.ToUnixTimestamp(DateTime.UtcNow);

        /// <summary>
        /// Gets the current UTC date and time
        /// </summary>
        public DateTime GetUtcNow() => DateTime.UtcNow;

        /// <summary>
        /// Gets the current date, with time set to 00:00:00
        /// </summary>
        public DateTime Today => DateTime.Today;
    }
}
