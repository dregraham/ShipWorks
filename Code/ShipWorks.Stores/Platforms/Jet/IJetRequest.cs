using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// An interface for submitting requests to the Jet.com API.
    /// </summary>
    public interface IJetRequest
    {
        /// <summary>
        /// Get Token
        /// </summary>
        GenericResult<string> GetToken(string username, string password);

        /// <summary>
        /// Acknowledges the order
        /// </summary>
        void Acknowledge(JetOrderEntity order, JetStoreEntity store, string path);

        /// <summary>
        /// Processes the request.
        /// </summary>
        GenericResult<T> ProcessRequest<T>(string action, string path, HttpVerb method, JetStoreEntity store);

        /// <summary>
        /// Process the request
        /// </summary>
        GenericResult<string> ProcessRequest(string action, JetStoreEntity store, IHttpRequestSubmitter request);
    }
}