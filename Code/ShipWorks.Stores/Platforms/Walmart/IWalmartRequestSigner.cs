using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Walmart
{
    public interface IWalmartRequestSigner
    {
        /// <summary>
        /// Signs the given request with the timestamp and generated signature
        /// </summary>
        void Sign(HttpRequestSubmitter requestSubmitter, WalmartStoreEntity store, string epoch);
    }
}