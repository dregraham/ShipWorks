using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Provides an abstraction over the static DateTime methods to make testing time based code easier.
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Gets the current date and time
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Gets the current UTC date and time
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// Gets the current date, with time set to 00:00:00
        /// </summary>
        DateTime Today { get; }

        /// <summary>
        /// Gets the current SQL server date time.
        /// </summary>
        DateTime CurrentSqlServerDateTime { get; }
    }
}