using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;

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
        
        /// <summary>
        /// Gets rates from ShipEngine using the given request
        /// </summary>
        /// <param name="request">The rate shipment request</param>
        /// <returns>The rate shipment response</returns>
        Task<RateShipmentResponse> RateShipment(RateShipmentRequest request, ApiLogSource apiLogSource);

        /// <summary>
        /// Purchases a label from ShipEngine using the given request
        /// </summary>
        Task<Label> PurchaseLabel(PurchaseLabelRequest request, ApiLogSource apiLogSource);
    }
}