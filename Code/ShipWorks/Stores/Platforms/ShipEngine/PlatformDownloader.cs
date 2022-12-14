using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Etsy;

namespace ShipWorks.Stores.Platforms.ShipEngine
{
    public abstract class PlatformDownloader : StoreDownloader
    {
        protected readonly ILog log;

        protected PlatformDownloader(StoreEntity store, StoreType storeType) : base(store, storeType)
        {
            log = LogManager.GetLogger(this.GetType());
        }
    }
}
