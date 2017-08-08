using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Common.Net
{
    /// <summary>
    /// Represents a Json Request
    /// </summary>
    public interface IJsonRequest
    {
        /// <summary>
        /// Process the request, log the result and deserialize 
        /// </summary>
        T ProcessRequest<T>(string action, ApiLogSource logSource, IHttpRequestSubmitter request);
    }
}