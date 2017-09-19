using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

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
        GenericResult<T> Submit<T>(string action, IHttpRequestSubmitter request, IJetStoreEntity store);
    }
}