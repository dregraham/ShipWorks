using ShipWorks.Data.Model.EntityClasses;

namespace Interapptive.Shared.Metrics
{
    public interface IStoreSettingsTrackedDurationEvent : ITrackedDurationEvent
    {
        void RecordStoreConfiguration(StoreEntity store);
    }
}