using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;

namespace ShipWorks.Stores.Platforms.Platform
{
    /// <summary>
    /// Basically a no-op. API stores should only download from hub.
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Api)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.BrightpearlHub)]
    public class PlatformStoreDowloader : StoreDownloader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformStoreDowloader(StoreEntity store, IStoreTypeManager storeTypeManager)
            : base(store, storeTypeManager.GetType(store))
        {
        }

        /// <summary>
        /// Download should only be done via the hub.
        /// </summary>
        protected override Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            throw new DownloadException($"{StoreType} should only download via hub");
        }
    }
}
