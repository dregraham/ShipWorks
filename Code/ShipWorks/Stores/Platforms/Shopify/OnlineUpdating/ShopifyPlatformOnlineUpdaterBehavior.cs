using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Stores.Platforms.Platform.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Shopify.OnlineUpdating
{
    [KeyedComponent(typeof(IPlatformOnlineUpdaterBehavior), StoreTypeCode.Shopify)]
    public class ShopifyPlatformDownloaderBehavior : DefaultPlatformOnlineUpdaterBehavior
    {
        public override bool UseSwatId => true;

        public override bool IncludeSalesOrderItems => true;
    }
}
