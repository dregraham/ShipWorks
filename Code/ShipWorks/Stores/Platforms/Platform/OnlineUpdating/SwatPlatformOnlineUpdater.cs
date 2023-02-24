using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Stores.Content;
using ShipWorks.Warehouse.Orders;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
    /// <summary>
    /// Inhertis from PlatformOnlineUpdater. This uses the SwatID when uploading
    /// shipment notifications to Platform
    /// </summary>
    [KeyedComponent(typeof(IPlatformOnlineUpdater), StoreTypeCode.Etsy)]
    public class SwatPlatformOnlineUpdater : PlatformOnlineUpdater
    {
        public SwatPlatformOnlineUpdater(
            IOrderManager orderManager, 
            IShippingManager shippingManager, 
            ISqlAdapterFactory sqlAdapterFactory, 
            Func<IWarehouseOrderClient> createWarehouseOrderClient, 
            IIndex<StoreTypeCode, IOnlineUpdater> storeSpecificOnlineUpdaterFactory) : 
            base(orderManager, shippingManager, sqlAdapterFactory, createWarehouseOrderClient, storeSpecificOnlineUpdaterFactory)
        {
        }

        protected override bool UseSwatId => true;
    }
}
