using Interapptive.Shared.Metrics;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Interface that represents a collection of telemetry data
    /// </summary>
    public interface ITelemetryCollection<T>
    {
        /// <summary>
        /// Add an entry to the collection
        /// </summary>
        void AddEntry(string name, long time);

        /// <summary>
        /// Copy another telemetric result's properties and totalElapsedTime and add them to this one
        /// </summary>
        void CopyFrom<A>(TelemetricResult<T> resultToAdd, bool useNewResultsValue);

        /// <summary>
        /// Copy to the destination 
        /// </summary>
        void CopyTo<A>(ITelemetryCollection<A> destination);

        /// <summary>
        /// Write telemetric events to the passed in TrackedDurationEvent
        /// </summary>
        void WriteTo(ITrackedDurationEvent trackedDurationEvent);
    }
}