using Interapptive.Shared.Net;

namespace ShipWorks.ApplicationCore.Logging
{
    public interface ILogEntryFactory
    {
        /// <summary>
        /// Gets the log entry.
        /// </summary>
        IApiLogEntry GetLogEntry(ApiLogSource source, string name, LogActionType logActionType);
    }
}