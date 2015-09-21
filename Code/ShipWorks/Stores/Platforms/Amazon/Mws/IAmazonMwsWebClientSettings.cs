using System.Xml.Linq;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Class for handeling Amazon MWS API Related Settings
    /// </summary>
    public interface IAmazonMwsWebClientSettings
    {
        /// <summary>
        /// Gets the api version for each call
        /// </summary>
        string GetApiVersion(AmazonMwsApiCall amazonMwsApiCall);

        /// <summary>
        /// Gets the API endpoint path
        /// </summary>
        string GetApiEndpointPath(AmazonMwsApiCall amazonMwsApiCall);

        /// <summary>
        /// Gets the Action parameter value for an API call 
        /// </summary>
        string GetActionName(AmazonMwsApiCall amazonMwsApiCall);

        /// <summary>
        /// Gets the XNamespace value for a particular API
        /// </summary>
        XNamespace GetApiNamespace(AmazonMwsApiCall api);
    }
}
