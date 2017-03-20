﻿using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ApplicationInsights.DataContracts;

namespace Interapptive.Shared.Metrics
{
    /// <summary>
    /// Utility class for tracking telemetry events
    /// </summary>
    public class TrackedEvent : ITrackedEvent
    {
        private readonly EventTelemetry eventTelemetry;
        private bool disposed = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public TrackedEvent(string name)
        {
            eventTelemetry = new EventTelemetry(name);
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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (disposed == false)
            {
                Telemetry.TrackEvent(eventTelemetry);
                disposed = true;
            }
        }
    }
}
