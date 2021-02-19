using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;

namespace ShipWorks.Stores.Platforms.Api
{
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Api)]
    public class ApiStoreDowloader : StoreDownloader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApiStoreDowloader(StoreEntity store, IStoreTypeManager storeTypeManager)
            : base(store, storeTypeManager.GetType(store))
        {
        }

        protected override Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            throw new NotImplementedException("Api should only download via hub");
        }
    }
}
