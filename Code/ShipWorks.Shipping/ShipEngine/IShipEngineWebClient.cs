using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Web client for communicating with ShipEngine
    /// </summary>
    public interface IShipEngineWebClient
    {
        /// <summary>
        /// Connects the given DHL account to the users ShipEngine account
        /// </summary>
        /// <returns>The CarrierId</returns>
        Task<GenericResult<string>> ConnectDhlAccount(string accountNumber);
    }
}