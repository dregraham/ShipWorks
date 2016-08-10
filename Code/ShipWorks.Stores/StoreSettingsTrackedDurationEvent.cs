using Interapptive.Shared.Metrics;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Utility class for tracking the duration of a Store Settings event, along with other metric info
    /// </summary>
    public class StoreSettingsTrackedDurationEvent : TrackedDurationEvent
    {
        public StoreSettingsTrackedDurationEvent() : base(string.Empty)
        {
        }
    }
}