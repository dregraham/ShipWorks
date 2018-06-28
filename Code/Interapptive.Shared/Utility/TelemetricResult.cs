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
        private readonly Dictionary<string, List<long>> telemetry;
        private string currentEventName;

        /// <summary>
        /// Contsructor
        /// </summary>
        public TelemetricResult(string baseTelemetryName)
        {
            this.baseTelemetryName = baseTelemetryName;
            currentEventName = string.Empty;
            stopwatch = new Stopwatch();
            telemetry = new Dictionary<string, List<long>>();
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
        /// Run and record a time entry for specified action
        /// </summary>
        public void TimedEvent(string eventName, Action eventToTime)
        {
            StartTimedEvent(eventName);
            try
            {
                eventToTime();
                StopTimedEvent(eventName);
            }
            catch (Exception)
            {
                StopTimedEvent(eventName);
                throw;
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
            AddEntry($"{baseTelemetryName}.{eventName}", elapsed);
            
            stopwatch.Reset();
        }

        /// <summary>
        /// Add another telemetric result's properties and totalElapsedTime to this one
        /// </summary>
        public void Combine(TelemetricResult<T> resultToAdd, bool useNewResultsValue)
        {
            resultToAdd.telemetry.ForEach(entries => entries.Value.ForEach(time => AddEntry(entries.Key, time)));

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
            long overallTime = 0;
            foreach (KeyValuePair<string, List<long>> telemetryEventType in telemetry)
            {
                if (telemetryEventType.Value.Count > 1)
                {
                    for (int i = 0; i < telemetryEventType.Value.Count; i++)
                    {
                        trackedDurationEvent.AddProperty($"{telemetryEventType.Key}.{i + 1}",
                            telemetryEventType.Value[i].ToString());
                    }
                }

                long totalTimeForEventType = telemetryEventType.Value.Sum(v => v);
                overallTime += totalTimeForEventType;
                trackedDurationEvent.AddProperty($"{telemetryEventType.Key}", totalTimeForEventType.ToString());
            }
            
            trackedDurationEvent.AddProperty(baseTelemetryName, overallTime.ToString());
        }

        /// <summary>
        /// Adds an entry to telemetry
        /// </summary>
        private void AddEntry(string name, long time)
        {
            if (!telemetry.ContainsKey(name))
            {
                telemetry[name] = new List<long>();
            }
            
            telemetry[name].Add(time);
        }
    }
}