using System;
using Interapptive.Shared.Metrics;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Interface that represents a collection of telemetry data
    /// </summary>
    public interface ITelemetryCollection
    {
        /// <summary>
        /// Add an entry to the collection
        /// </summary>
        void AddEntry(string name, long time);

        /// <summary>
        /// Copy to the destination 
        /// </summary>
        void CopyTo(ITelemetryCollection destination);
    }
}