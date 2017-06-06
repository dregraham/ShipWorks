using ShipWorks.Data.Model.EntityClasses;

namespace Interapptive.Shared.Metrics
{
    /// <summary>
    /// Tracked duration event
    /// </summary>
    public interface IStoreSettingsTrackedDurationEvent : ITrackedDurationEvent
    {
        /// <summary>
        /// Record configuration for a store
        /// </summary>
        void RecordStoreConfiguration(StoreEntity store);
    }
}