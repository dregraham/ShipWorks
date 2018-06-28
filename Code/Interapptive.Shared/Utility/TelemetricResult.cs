using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Generic class that can be used to return telemetry data along with a result
    /// </summary>
    public class TelemetricResult<T>
    {
        private readonly string baseTelemetryName;
        private readonly Stopwatch stopwatch;
        private readonly Dictionary<string, long> telemetry;
        private string currentEventName;

        /// <summary>
        /// Contsructor
        /// </summary>
        public TelemetricResult(string baseTelemetryName)
        {
            this.baseTelemetryName = baseTelemetryName;
            currentEventName = string.Empty;
            stopwatch = new Stopwatch();
            telemetry = new Dictionary<string, long>();
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
            telemetry.Add($"{baseTelemetryName}.{eventName}", elapsed);
            
            stopwatch.Reset();
        }

        /// <summary>
        /// Add another telemetric result's properties and totalElapsedTime to this one
        /// </summary>
        public void Combine(TelemetricResult<T> resultToAdd, bool useNewResultsValue)
        {
            resultToAdd.telemetry.ForEach(t => telemetry.Add(t.Key, t.Value));

            if (useNewResultsValue)
            {
                Value = resultToAdd.Value;
            }
        }

        /// <summary>
        /// Add internal telemetric events to the passed in TrackedDurationEvent
        /// </summary>
        public void Populate(ITrackedDurationEvent trackedDurationEvent)
        {
            telemetry.ForEach(t => trackedDurationEvent.AddProperty(t.Key, t.Value.ToString()));
            trackedDurationEvent.AddProperty(baseTelemetryName, telemetry.Sum(x=>x.Value).ToString());
        }
    }
}