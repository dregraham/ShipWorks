using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Manages Jet Tokens
    /// </summary>
    public interface IJetTokenManager
    {
        /// <summary>
        /// Add the jet token to the request
        /// </summary>
        void AddTokenToRequest(IHttpRequestSubmitter request, JetStoreEntity store);

        /// <summary>
        /// Get a token for the username/password
        /// </summary>
        string GetToken(string username, string password);
    }
}