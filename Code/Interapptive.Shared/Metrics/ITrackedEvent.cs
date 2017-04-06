using System;

namespace Interapptive.Shared.Metrics
{
    /// <summary>
    /// Track telemetry events
    /// </summary>
    public interface ITrackedEvent : IDisposable
    {
        /// <summary>
        /// Add a metric value to the event
        /// </summary>
        void AddMetric(string metricName, double metricValue);

        /// <summary>
        /// Adds a property value to the event.
        /// </summary>
        void AddProperty(string propertyName, string propertyValue);
    }
}