using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Downloader for Overstock stores
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Overstock)]
    public class OverstockDownloader : StoreDownloader
    {
        private IOverstockWebClient webClient;
        private readonly OverstockStoreEntity overstockStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockDownloader(StoreEntity store, IStoreTypeManager storeTypeManager, IOverstockWebClient webClient)
            : base(store, storeTypeManager.GetType(store))
        {
            this.webClient = webClient;
            overstockStore = (OverstockStoreEntity) store;
        }

        /// <summary>
        /// Download
        /// </summary>
        protected override Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            // TODO: Implement in later story.
            return Task.CompletedTask;
        }
    }
}
