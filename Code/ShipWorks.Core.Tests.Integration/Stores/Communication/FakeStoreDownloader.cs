using System;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;

namespace ShipWorks.Core.Tests.Integration.Stores.Communication
{
    class FakeStoreDownloader : StoreDownloader
    {
        private readonly string orderCountryCode;

        public FakeStoreDownloader(string orderCountryCode, StoreEntity store, StoreType storeType, IConfigurationData configurationData, ISqlAdapterFactory sqlAdapterFactory) 
            : base(store, storeType, configurationData, sqlAdapterFactory)
        {
            this.orderCountryCode = orderCountryCode;
        }

        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            
            var orderResult = await InstantiateOrder(new OrderNumberIdentifier(42));
            orderResult.Value.OrderTotal = 0;
            orderResult.Value.OrderDate = DateTime.Now;
            orderResult.Value.OnlineLastModified = DateTime.Now;
            orderResult.Value.ShipStreet1 = "123 woop st";
            orderResult.Value.ShipCountryCode = orderCountryCode;

            await SaveDownloadedOrder(orderResult.Value);
        }
    }
}