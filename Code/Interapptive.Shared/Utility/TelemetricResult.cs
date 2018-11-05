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
    public class TelemetricResult<T> : ITelemetryCollection<T>
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
            if (value != null)
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
        /// <remarks>
        /// Using the TelemetricEventType is useful when the same event
        /// type is being used in multiple places and you want to
        /// guarantee consistent event names.
        /// </remarks>
        public void RunTimedEvent(TelemetricEventType eventType, Action eventToTime) =>
            RunTimedEvent(eventType, eventToTime.ToFunc());

        /// <summary>
        /// Run and record a time entry for specified action
        /// </summary>
        /// <remarks>
        /// Using the TelemetricEventType is useful when the same event
        /// type is being used in multiple places and you want to
        /// guarantee consistent event names.
        /// </remarks>
        public K RunTimedEvent<K>(TelemetricEventType eventType, Func<K> eventToTime)
        {
            return RunTimedEvent(EnumHelper.GetDescription(eventType), eventToTime);
        }

        /// <summary>
        /// Run and record a time entry for specified action
        /// </summary>
        /// <remarks>
        /// Using the TelemetricEventType is useful when the same event
        /// type is being used in multiple places and you want to
        /// guarantee consistent event names.
        /// </remarks>
        public async Task RunTimedEventAsync(TelemetricEventType eventType, Func<Task> eventToTime)
        {
            await RunTimedEvent(EnumHelper.GetDescription(eventType), eventToTime);
        }

        /// <summary>
        /// Run and record a time entry for specified action
        /// </summary>
        /// <remarks>
        /// Specifying the eventName as a string gives you more control
        /// over the naming of the timed event or for cases where you're not
        /// concerned about consistent naming for the same type of event that
        /// can be invoked from multiple places.
        /// </remarks>
        public void RunTimedEvent(string eventName, Action eventToTime) =>
            RunTimedEvent(eventName, eventToTime.ToFunc());

        /// <summary>
        /// Run and record a time entry for specified action
        /// </summary>
        /// <remarks>
        /// Specifying the eventName as a string gives you more control
        /// over the naming of the timed event or for cases where you're not
        /// concerned about consistent naming for the same type of event that
        /// can be invoked from multiple places.
        /// </remarks>
        public K RunTimedEvent<K>(string eventName, Func<K> eventToTime)
        {
            StartTimedEvent(eventName);
            try
            {
                return eventToTime();
            }
            catch (Exception)
            {
                StopTimedEvent(eventName);
                throw;
            }
            finally
            {
                StopTimedEvent(eventName);
            }
        }

        /// <summary>
        /// Run and record a time entry for specified action
        /// </summary>
        /// <remarks>
        /// Using the TelemetricEventType is useful when the same event
        /// type is being used in multiple places and you want to
        /// guarantee consistent event names.
        /// </remarks>
        public async Task RunTimedEventAsync(string eventName, Func<Task> eventToTime)
        {
            StartTimedEvent(eventName);
            try
            {
                await eventToTime();
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
        public void CopyFrom<A>(TelemetricResult<T> resultToAdd, bool useNewResultsValue)
        {
            resultToAdd.telemetry.ForEach(entries => entries.Value.ForEach(time => AddEntry(entries.Key, time)));

            if (useNewResultsValue)
            {
                Value = resultToAdd.Value;
            }
        }

        /// <summary>
        /// Copy all of the telemetry to the destination
        /// </summary>
        public void CopyTo<A>(ITelemetryCollection<A> destination)
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
            if (trackedDurationEvent == null)
            {
                return;
            }

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