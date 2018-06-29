using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Generic class that can be used to return telemetry data along with a result
    /// </summary>
    public class TelemetricResult<T> : ITelemetryCollection
    {
        private readonly string baseTelemetryName;
        private readonly Stopwatch stopwatch;
        private readonly Dictionary<string, List<long>> telemetry;
        private string currentEventName;

        /// <summary>
        /// Constructor
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
            if (Value != null)
            {
                Value = value;
            }
        }

        /// <summary>
        /// The actual result
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Run and record a time entry for specified action
        /// </summary>
        public void RunTimedEvent(TelemetricEventType eventType, Action eventToTime)
        {
            StartTimedEvent(EnumHelper.GetDescription(eventType));
            try
            {
                eventToTime();
                StopTimedEvent(EnumHelper.GetDescription(eventType));
            }
            catch (Exception)
            {
                StopTimedEvent(EnumHelper.GetDescription(eventType));
                throw;
            }
        }
        
        /// <summary>
        /// Run and record a time entry for specified action
        /// </summary>
        public async Task RunTimedEventAsync(TelemetricEventType eventType, Func<Task> eventToTime)
        {
            StartTimedEvent(EnumHelper.GetDescription(eventType));
            try
            {
                await eventToTime();
                StopTimedEvent(EnumHelper.GetDescription(eventType));
            }
            catch (Exception)
            {
                StopTimedEvent(EnumHelper.GetDescription(eventType));
                throw;
            }
        }
        
        /// <summary>
        /// Start timing an event
        /// </summary>
        private void StartTimedEvent(string eventName)
        {
            Debug.Assert(stopwatch.IsRunning == false);

            currentEventName = eventName;
            stopwatch.Start();
        }

        /// <summary>
        /// End timing of an event and store the result
        /// </summary>
        private void StopTimedEvent(string eventName)
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
        /// Copy another telemetric result's properties and totalElapsedTime and add them to this one
        /// </summary>
        public void CopyFrom<A>(TelemetricResult<A> resultToAdd, bool useNewResultsValue)
        {
            resultToAdd.telemetry.ForEach(entries => entries.Value.ForEach(time => AddEntry(entries.Key, time)));

            if (useNewResultsValue && GetType() == resultToAdd.Value.GetType())
            {
                Value = (T) Convert.ChangeType(resultToAdd.Value, typeof(T));
            }
        }

        /// <summary>
        /// Copy all of the telemetry to the destination
        /// </summary>
        public void CopyTo(ITelemetryCollection destination)
        {
            foreach (KeyValuePair<string, List<long>> item in telemetry)
            {
                foreach (long metric in item.Value)
                {
                    destination.AddEntry(item.Key, metric);
                }
            }
        }

        /// <summary>
        /// Write telemetric events to the passed in TrackedDurationEvent
        /// </summary>
        public void WriteTo(ITrackedDurationEvent trackedDurationEvent)
        {
            long overallTime = 0;
            foreach (KeyValuePair<string, List<long>> telemetryEventType in telemetry)
            {
                if (telemetryEventType.Value.Count > 1)
                {
                    for (int i = 0; i < telemetryEventType.Value.Count; i++)
                    {
                        trackedDurationEvent.AddMetric($"{telemetryEventType.Key}.{i + 1}", telemetryEventType.Value[i]);
                    }
                }

                long totalTimeForEventType = telemetryEventType.Value.Sum(v => v);
                overallTime += totalTimeForEventType;
                trackedDurationEvent.AddMetric($"{telemetryEventType.Key}", totalTimeForEventType);
            }
            
            trackedDurationEvent.AddMetric(baseTelemetryName, overallTime);
        }

        /// <summary>
        /// Adds an entry to telemetry
        /// </summary>
        public void AddEntry(string name, long time)
        {
            if (!telemetry.ContainsKey(name))
            {
                telemetry[name] = new List<long>();
            }
            
            telemetry[name].Add(time);
        }
    }
}