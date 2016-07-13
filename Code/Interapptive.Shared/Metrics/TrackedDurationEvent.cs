﻿using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ApplicationInsights.DataContracts;

namespace Interapptive.Shared.Metrics
{
    /// <summary>
    /// Utility class for tracking the duration of an event, along with other metric info
    /// </summary>
    public class TrackedDurationEvent : ITrackedDurationEvent
    {
        readonly Stopwatch stopwatch;
        private readonly EventTelemetry eventTelemetry;

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
        /// Stop the stopwatch
        /// </summary>
        private void Stop()
        {
            try
            {
                stopwatch.Stop();

                AddMetric("DurationIn(ms)", stopwatch.ElapsedMilliseconds);

                Telemetry.TrackEvent(eventTelemetry);
            }
            catch
            {
                // If for some reason the code throws, we don't want to stop the user from
                // doing their work, so igoring all exceptions here.
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
    }
}
