using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
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
        private readonly Dictionary<string, List<long>> telemetry;
        private readonly Dictionary<string, string> properties = new Dictionary<string, string>();
        private readonly string currentEventName;

        /// <summary>
        /// Constructor
        /// </summary>
        public TelemetricResult(string baseTelemetryName)
        {
            this.baseTelemetryName = baseTelemetryName;
            currentEventName = string.Empty;
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
        public K RunTimedEvent<K>(TelemetricEventType eventType, Func<K> eventToTime) =>
            RunTimedEvent(EnumHelper.GetDescription(eventType), eventToTime);

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
        public K RunTimedEvent<K>(string eventName, Func<K> eventToTime) =>
            Functional.Using(StartTimedEvent(eventName), _ => eventToTime());

        /// <summary>
        /// Run and record a time entry for specified action
        /// </summary>
        /// <remarks>
        /// Using the TelemetricEventType is useful when the same event
        /// type is being used in multiple places and you want to
        /// guarantee consistent event names.
        /// </remarks>
        public Task RunTimedEventAsync(TelemetricEventType eventType, Func<Task> eventToTime) =>
            RunTimedEventAsync(EnumHelper.GetDescription(eventType), () => eventToTime().ContinueWith((t) => Unit.Default));

        /// <summary>
        /// Run and record a time entry for specified action
        /// </summary>
        /// <remarks>
        /// Using the TelemetricEventType is useful when the same event
        /// type is being used in multiple places and you want to
        /// guarantee consistent event names.
        /// </remarks>
        public Task RunTimedEventAsync(string eventName, Func<Task> eventToTime) =>
            RunTimedEventAsync(eventName, () => eventToTime().ContinueWith((t) => Unit.Default));

        /// <summary>
        /// Run and record a time entry for specified action
        /// </summary>
        /// <remarks>
        /// Using the TelemetricEventType is useful when the same event
        /// type is being used in multiple places and you want to
        /// guarantee consistent event names.
        /// </remarks>
        public Task<K> RunTimedEventAsync<K>(TelemetricEventType eventType, Func<Task<K>> eventToTime) =>
            RunTimedEventAsync(EnumHelper.GetDescription(eventType), eventToTime);

        /// <summary>
        /// Run and record a time entry for specified action
        /// </summary>
        /// <remarks>
        /// Specifying the eventName as a string gives you more control
        /// over the naming of the timed event or for cases where you're not
        /// concerned about consistent naming for the same type of event that
        /// can be invoked from multiple places.
        /// </remarks>
        public Task<K> RunTimedEventAsync<K>(string eventName, Func<Task<K>> eventToTime) =>
            Functional.UsingAsync(StartTimedEvent(eventName), _ => eventToTime());

        /// <summary>
        /// Start timing an event
        /// </summary>
        private IDisposable StartTimedEvent(string eventName)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            return Disposable.Create(() =>
            {
                stopwatch.Stop();

                long elapsed = stopwatch.ElapsedMilliseconds;
                AddEntry($"{baseTelemetryName}.{eventName}", elapsed);
            });
        }

        /// <summary>
        /// Copy another telemetric result's properties and totalElapsedTime and add them to this one
        /// </summary>
        public void CopyFrom(TelemetricResult<T> resultToAdd, bool useNewResultsValue)
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
        public void WriteTo(ITrackedEvent trackedDurationEvent)
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

            properties.ForEach(x => trackedDurationEvent.AddProperty(x.Key, x.Value));
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

        /// <summary>
        /// Add a custom property
        /// </summary>
        public void AddProperty(string key, string value) => properties[key] = value;
    }
}