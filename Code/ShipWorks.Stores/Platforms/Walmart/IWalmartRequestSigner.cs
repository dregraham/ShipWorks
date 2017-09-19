using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Signer for Walmart web requests
    /// </summary>
    [Service]
    public interface IWalmartRequestSigner
    {
        /// <summary>
        /// Signs the given request with the timestamp and generated signature
        /// </summary>
        void Sign(IHttpRequestSubmitter requestSubmitter, IWalmartStoreEntity store);
    }
}