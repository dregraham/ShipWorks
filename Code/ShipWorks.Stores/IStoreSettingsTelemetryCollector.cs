using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Collects telemetry data for a store
    /// </summary>
    public interface IStoreSettingsTelemetryCollector
    {
        /// <summary>
        /// Collects telemetry for a store
        /// </summary>
        void CollectTelemetry(StoreEntity store, StoreSettingsTrackedDurationEvent storeSettingsTrackedDurationEvent);
    }
}