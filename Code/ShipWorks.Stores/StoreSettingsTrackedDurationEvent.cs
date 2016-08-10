using Autofac;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores
{
    /// <summary>
    /// An event for obtaining telemetry on store configuration.
    /// </summary>
    public class StoreSettingsTrackedDurationEvent : TrackedDurationEvent, IStoreSettingsTrackedDurationEvent
    {
        private readonly string formattedName;
        private string storeTypeCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreSettingsTrackedDurationEvent"/> class.
        /// </summary>
        /// <param name="formattedName">Name of the formatted.</param>
        public StoreSettingsTrackedDurationEvent(string formattedName) 
            : base(string.Empty)
        {
            this.formattedName = formattedName;
        }

        /// <summary>
        /// Records the given store configuration/settings and attaches it to this event.
        /// </summary>
        /// <param name="store">The store.</param>
        public void RecordStoreConfiguration(StoreEntity store)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                IStoreSettingsTelemetryCollector telemetryController = scope.ResolveKeyed<IStoreSettingsTelemetryCollector>(store.StoreTypeCode);
                telemetryController.CollectTelemetry(store, this);
            }

            storeTypeCode = EnumHelper.GetDescription(store.StoreTypeCode);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            string storeType = string.IsNullOrWhiteSpace(storeTypeCode) ? "Unknown" : storeTypeCode;
            string eventName = string.Format(formattedName, storeType);

            ChangeName(eventName);
            base.Dispose();
        }
    }
}