using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;

namespace ShipWorks.Stores.Platforms.Yahoo
{
    class YahooDownloader : StoreDownloader
    {
        public YahooDownloader(StoreEntity store) : base(store)
        {
        }

        public YahooDownloader(StoreEntity store, StoreType storeType) : base(store, storeType)
        {
        }

        protected override void Download()
        {
            throw new NotImplementedException();
        }
    }
}
