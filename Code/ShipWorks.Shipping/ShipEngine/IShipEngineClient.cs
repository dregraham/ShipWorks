using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Web client for communicating with ShipEngine
    /// </summary>
    public interface IShipEngineClient
    {
        /// <summary>
        /// Connects the given DHL account to the users ShipEngine account
        /// </summary>
        Task<GenericResult<string>> ConnectDhlAccount(string accountNumber);
    }
}