﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using log4net;
using Microsoft.ApplicationInsights.DataContracts;

namespace Interapptive.Shared.Metrics
{
    /// <summary>
    /// Utility class for tracking telemetry events
    /// </summary>
    public class TrackedEvent : ITrackedEvent
    {
        /// <summary>
        /// Creates and sends a single event with the given name
        /// </summary>
        public static void SendSingleEvent(string name) => new TrackedEvent(name).Dispose();
        
        private readonly EventTelemetry eventTelemetry;
        protected bool disposed = false;

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
                try
                {
                    Telemetry.TrackEvent(eventTelemetry);
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger(typeof(TrackedEvent)).Debug("Failed to write telemetry", ex);
                }

                disposed = true;
            }
        }
    }
}
