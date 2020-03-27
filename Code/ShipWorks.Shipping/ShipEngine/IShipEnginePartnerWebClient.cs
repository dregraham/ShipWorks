using System.Threading.Tasks;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Client to the ShipEngine Partner API
    /// </summary>
    public interface IShipEnginePartnerWebClient
    {
        /// <summary>
        /// Creates a new ShipEngine account and returns the api key
        /// </summary>
        Task<string> CreateNewAccount();
    }
}
