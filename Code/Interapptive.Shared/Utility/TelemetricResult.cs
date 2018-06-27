﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Interapptive.Shared.Collections;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Generic class that can be used to return telemetry data along with a result
    /// </summary>
    public class TelemetricResult<T>
    {
        private readonly string baseTelemetryName;
        private readonly Stopwatch stopwatch;
        private long totalElapsed;
        private readonly Dictionary<string, string> telemetry;
        private string currentEventName;

        /// <summary>
        /// Contsructor
        /// </summary>
        public TelemetricResult(string baseTelemetryName)
        {
            this.baseTelemetryName = baseTelemetryName;
            totalElapsed = 0;
            currentEventName = string.Empty;
            stopwatch = new Stopwatch();
            telemetry = new Dictionary<string, string>();
        }

        /// <summary>
        /// Sets the result value to be returned
        /// </summary>
        public void SetValue(T value)
        {
            Value = value;
        }

        /// <summary>
        /// The actual result
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// The telemetry data associated with the result
        /// </summary>
        public Dictionary<string, string> Telemetry 
        {
            get
            {
                AddTotalTime();
                return telemetry;
            }
        }

        /// <summary>
        /// Start timing an event
        /// </summary>
        public void StartTimedEvent(string eventName)
        {
            Debug.Assert(stopwatch.IsRunning == false);

            currentEventName = eventName;
            stopwatch.Start();
        }

        /// <summary>
        /// End timing of an event and store the result
        /// </summary>
        public void StopTimedEvent(string eventName)
        {
            Debug.Assert(stopwatch.IsRunning);

            if (eventName != currentEventName)
            {
                throw new ArgumentException($"Invalid call to StopTimedEvent. EventName '{eventName}' must match CurrentEventName '{currentEventName}'.");
            }

            stopwatch.Stop();
            
            long elapsed = stopwatch.ElapsedMilliseconds;
            telemetry.Add($"{baseTelemetryName}.{eventName}", elapsed.ToString());
            totalElapsed += elapsed;
            
            stopwatch.Reset();
        }

        /// <summary>
        /// Add property to telemetry items
        /// </summary>
        public void AddProperty(string propertyName, string value) => telemetry.Add(propertyName, value);

        /// <summary>
        /// Add another telemetric result's properties and totalElapsedTime to this one
        /// </summary>
        public void Combine(TelemetricResult<T> resultToAdd, bool useNewResultsValue)
        {
            resultToAdd.Telemetry.ForEach(t => AddProperty(t.Key, t.Value));
            totalElapsed += resultToAdd.totalElapsed;

            if (useNewResultsValue)
            {
                Value = resultToAdd.Value;
            }
        }

        /// <summary>
        /// Add up the total time and store the result
        /// </summary>
        private void AddTotalTime()
        {
            Debug.Assert(stopwatch.IsRunning == false);
            
            telemetry[baseTelemetryName] = totalElapsed.ToString();
        }
    }
}