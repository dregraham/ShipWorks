using System.Threading.Tasks;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Client to the ShipEngine Partner API
    /// </summary>
    public interface IShipEnginePartnerWebClient
    {
        /// <summary>
        /// Creates a new ShipEngine account and returns the account ID
        /// </summary>
        Task<string> CreateNewAccount(string partnerApiKey);

        /// <summary>
        /// Gets an ApiKey from the ShipEngine API
        /// </summary>
        Task<string> GetApiKey(string partnerApiKey, string shipEngineAccountId);
    }
}