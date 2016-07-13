using System;

namespace Interapptive.Shared.Metrics
{
    /// <summary>
    /// Track the duration of an event, along with other metric info
    /// </summary>
    public interface ITrackedDurationEvent : IDisposable
    {
        /// <summary>
        /// Add a metric value to the event
        /// </summary>
        void AddMetric(string metricName, double metricValue);

        /// <summary>
        /// Dispose
        /// </summary>
        void Dispose();
    }
}