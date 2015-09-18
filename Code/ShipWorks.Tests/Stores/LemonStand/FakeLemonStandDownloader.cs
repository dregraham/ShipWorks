using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
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

        protected override OrderEntity InstantiateOrder(OrderIdentifier orderIdentifier)
        {
            Order = new LemonStandOrderEntity();
            return Order;
        }

        protected override void SaveDownloadedOrder(OrderEntity orderEntity)
        {
            SavedOrder = (LemonStandOrderEntity)orderEntity;
        }
    }
}
