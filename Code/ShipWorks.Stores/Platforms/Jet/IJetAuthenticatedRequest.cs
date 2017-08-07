using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// When processing request, adds authentication info
    /// </summary>
    public interface IJetAuthenticatedRequest
    {
        /// <summary>
        /// Process the request. If an error is thrown, refresh the token and try again.
        /// </summary>
        GenericResult<T> ProcessRequest<T>(string action, IHttpRequestSubmitter request, JetStoreEntity store);
    }
}