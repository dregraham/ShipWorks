using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

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
        void Sign(IHttpRequestSubmitter requestSubmitter, WalmartStoreEntity store);
    }
}