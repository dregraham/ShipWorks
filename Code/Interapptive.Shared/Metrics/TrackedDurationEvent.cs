using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ApplicationInsights.DataContracts;

namespace Interapptive.Shared.Metrics
{
    /// <summary>
    /// Utility class for tracking the duration of an event, along with other metric info
    /// </summary>
    public class TrackedDurationEvent : ITrackedDurationEvent
    {
        public const string DurationMetricKey = "DurationIn(ms)";
        private readonly Stopwatch stopwatch;
        private readonly EventTelemetry eventTelemetry;
        private bool disposed = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public TrackedDurationEvent(string name)
        {
            eventTelemetry = new EventTelemetry(name);

            stopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Add a metric value to the event
        /// </summary>
        public void AddMetric(string metricName, double metricValue)
        {
            if (eventTelemetry.Metrics.ContainsKey(metricName))
            {
                eventTelemetry.Metrics[metricName] = metricValue;
            }
            else
            {
                eventTelemetry.Metrics.Add(new KeyValuePair<string, double>(metricName, metricValue));
            }
        }

        /// <summary>
        /// Adds a property value to the event.
        /// </summary>
        public void AddProperty(string propertyName, string propertyValue)
        {
            if (eventTelemetry.Properties.ContainsKey(propertyName))
            {
                eventTelemetry.Properties[propertyName] = propertyValue;
            }
            else
            {
                eventTelemetry.Properties.Add(propertyName, propertyValue);
            }
        }

        /// <summary>
        /// Stop the stopwatch
        /// </summary>
        private void Stop()
        {
            try
            {
                stopwatch.Stop();

                AddMetric(DurationMetricKey, stopwatch.ElapsedMilliseconds);

                Telemetry.TrackEvent(eventTelemetry);
            }
            catch
            {
                // If for some reason the code throws, we don't want to stop the user from
                // doing their work, so igoring all exceptions here.
            }
        }

        /// <summary>
        /// Changes the name used to identify this specific event.
        /// </summary>
        protected void ChangeName(string name)
        {
            eventTelemetry.Name = name;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (disposed == false)
            {
                Stop();
                disposed = true;
            }
        }
    }
}
