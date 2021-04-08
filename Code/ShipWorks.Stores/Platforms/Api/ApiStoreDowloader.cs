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
    /// <summary>
    /// Basicly a no-op. API stores should only download from hub.
    /// </summary>
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

        /// <summary>
        /// Download should only be done via the hub.
        /// </summary>
        protected override Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            throw new NotImplementedException("API should only download via hub");
        }
    }
}
