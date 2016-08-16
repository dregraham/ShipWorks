using System;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Get the date and time as provided by SQL server
    /// </summary>
    public interface ISqlDateTimeProvider
    {
        /// <summary>
        /// Get the latest time information from the server
        /// </summary>
        DateTime GetLocalDate();

        /// <summary>
        /// Get the latest Utc time information from the server
        /// </summary>
        DateTime GetUtcDate();

        /// <summary>
        /// Reset the cached time
        /// </summary>
        void ResetCache();
    }
}