using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Stores.Platforms.Platform.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Etsy.OnlineUpdating
{
    [KeyedComponent(typeof(IPlatformOnlineUpdaterBehavior), StoreTypeCode.Etsy)]
    public class EtsyPlatformOnlineUpdaterBehavior : DefaultPlatformOnlineUpdaterBehavior
    {
        public override bool UseSwatId => true;
    }
}
