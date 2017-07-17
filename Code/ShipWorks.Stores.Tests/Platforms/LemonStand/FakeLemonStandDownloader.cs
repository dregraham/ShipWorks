using System.Threading.Tasks;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.LemonStand;

namespace ShipWorks.Tests.Stores.LemonStand
{
    /// <summary>
    /// Used for unit testing only
    /// Overrides the InstantiateOrder and SaveDownloadedOrder to get rid of the dependencies on the webClient and sqlAdapter
    /// </summary>
    class FakeLemonStandDownloader : LemonStandDownloader
    {
        public LemonStandOrderEntity Order { get; set; }

        public LemonStandOrderEntity SavedOrder { get; set; }

        public FakeLemonStandDownloader(StoreEntity store) : base(store)
        {
        }

        public FakeLemonStandDownloader(StoreEntity store, ILemonStandWebClient webClient, ISqlAdapterRetry sqlAdapter) : base(store, webClient, sqlAdapter)
        {
        }

        public FakeLemonStandDownloader(StoreEntity store, ILemonStandWebClient webClient, ISqlAdapterRetry sqlAdapter, StoreType storeType) : base(store, webClient, sqlAdapter, storeType)
        {
        }

        protected override Task<OrderEntity> InstantiateOrder(OrderIdentifier orderIdentifier)
        {
            Order = new LemonStandOrderEntity();
            return Task.FromResult(Order as OrderEntity);
        }

        protected override Task SaveDownloadedOrder(OrderEntity order)
        {
            SavedOrder = (LemonStandOrderEntity) order;
            return Task.CompletedTask;
        }
    }
}
