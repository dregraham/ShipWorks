using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
    public class DefaultPlatformOnlineUpdaterBehavior : IPlatformOnlineUpdaterBehavior
    {
        public virtual bool UseSwatId => false;

        public virtual bool IncludeSalesOrderItems => false;   
    
    }
}
