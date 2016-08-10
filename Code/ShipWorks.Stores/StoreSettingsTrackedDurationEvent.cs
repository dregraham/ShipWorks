using System;
using Autofac;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Utility class for tracking the duration of a Store Settings event, along with other metric info
    /// </summary>
    public class StoreSettingsTrackedDurationEvent : TrackedDurationEvent, IDisposable
    {
        private readonly string formattedName;
        private string storeTypeCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreSettingsTrackedDurationEvent"/> class.
        /// </summary>
        /// <param name="formattedName">Name of the formatted.</param>
        public StoreSettingsTrackedDurationEvent(string formattedName) : base(formattedName)
        {
            this.formattedName = formattedName;
        }

        /// <summary>
        /// Adds the metric.
        /// </summary>
        /// <param name="store">The store.</param>
        public void AddMetric(StoreEntity store)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                IStoreSettingsTelemetryCollector telemetryController = scope.ResolveKeyed<IStoreSettingsTelemetryCollector>(store.StoreTypeCode);
                telemetryController.CollectTelemetry(store, this);
            }

            storeTypeCode = EnumHelper.GetDescription(store.StoreTypeCode);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public new void Dispose()
        {
            string storeType = string.IsNullOrWhiteSpace(storeTypeCode) ? "Unknown" : storeTypeCode;
            string eventName = string.Format(formattedName, storeType);

            UpdateEventName(eventName);
            base.Dispose();
        }
    }
}