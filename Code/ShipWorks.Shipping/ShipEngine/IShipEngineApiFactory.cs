using ShipEngine.ApiClient.Api;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// interface for creating ShipEngine API Calls
    /// </summary>
    public interface IShipEngineApiFactory
    {
        /// <summary>
        /// Create the CarrierAccountsApi
        /// </summary>
        /// <returns></returns>
        ICarrierAccountsApi CreateCarrierAccountsApi();

        /// <summary>
        /// Create the CarrierApi
        /// </summary>
        /// <returns></returns>
        ICarriersApi CreateCarrierApi();

        /// <summary>
        /// Creates the Rate API
        /// </summary>
        IRatesApi CreateRatesApi();
    }
}