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
        /// Gets the current Epoc in seconds
        /// </summary>
        double Epoc { get; }

        /// <summary>
        /// Gets the current UTC date and time
        /// </summary>
        /// <remarks>The method version can be used to make testing easier when passing a method reference
        /// to another method</remarks>
        DateTime GetUtcNow();

        /// <summary>
        /// Gets the current date, with time set to 00:00:00
        /// </summary>
        DateTime Today { get; }
    }
}