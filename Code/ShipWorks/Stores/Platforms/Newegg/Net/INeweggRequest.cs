using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Newegg.Net
{
    /// <summary>
    /// Interface for submitting requests to the Newegg API.
    /// </summary>
    public interface INeweggRequest
    {
        /// <summary>
        /// Submits the request with the given credentials and request configuration.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="requestConfiguration">The request configuration.</param>
        /// <returns>
        /// A NeweggResponse containing the response from the Newegg API.
        /// </returns>
        Task<string> SubmitRequest(Credentials credentials, RequestConfiguration requestConfiguration);
    }
}
