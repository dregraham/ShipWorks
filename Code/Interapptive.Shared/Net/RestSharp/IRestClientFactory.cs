using RestSharp;

namespace Interapptive.Shared.Net.RestSharp
{
    /// <summary>
    /// Factory for creating IRestClients
    /// </summary>
    public interface IRestClientFactory
    {
        /// <summary>
        /// Create an IRestClient
        /// </summary>
        IRestClient Create();

        /// <summary>
        /// Create an IRestClient with the given base url
        /// </summary>
        IRestClient Create(string baseUrl);
    }
}
