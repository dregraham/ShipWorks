using System;

namespace Interapptive.Shared.Metrics
{
    /// <summary>
    /// Dummy tracked duration event for situations when telemetry should conditionally be tracked
    /// </summary>
    internal class DummyTrackedDurationEvent : ITrackedDurationEvent
    {
        /// <summary>
        /// Add a metric
        /// </summary>
        public void AddMetric(string metricName, double metricValue)
        {
            // Do nothing, since this is a dummy
        }

        /// <summary>
        /// Add a property
        /// </summary>
        public void AddProperty(string propertyName, string propertyValue)
        {
            // Do nothing, since this is a dummy
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // Do nothing, since this is a dummy
        }
    }
}