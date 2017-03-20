using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ApplicationInsights.DataContracts;

namespace Interapptive.Shared.Metrics
{
    /// <summary>
    /// Utility class for tracking the duration of an event, along with other metric info
    /// </summary>
    public class TrackedDurationEvent : TrackedEvent, ITrackedDurationEvent
    {
        public const string DurationMetricKey = "DurationIn(ms)";
        private readonly Stopwatch stopwatch;

        /// <summary>
        /// Constructor
        /// </summary>
        public TrackedDurationEvent(string name)
            :base(name)
        {
            stopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Get a dummy tracked duration event for situations where an event should not be tracked
        /// </summary>
        public static ITrackedDurationEvent Dummy => new DummyTrackedDurationEvent();

        /// <summary>
        /// Stop the stopwatch
        /// </summary>
        private void Stop()
        {
            try
            {
                stopwatch.Stop();
                AddMetric(DurationMetricKey, stopwatch.ElapsedMilliseconds);
            }
            catch
            {
                // If for some reason the code throws, we don't want to stop the user from
                // doing their work, so igoring all exceptions here.
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (disposed == false)
            {
                Stop();
                base.Dispose();
            }
        }
    }
}
