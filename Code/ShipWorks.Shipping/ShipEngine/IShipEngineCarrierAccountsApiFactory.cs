using ShipEngine.ApiClient.Api;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// interface for creating the ShipEngine CarrierAccountsApi
    /// </summary>
    public interface IShipEngineCarrierAccountsApiFactory
    {
        /// <summary>
        /// Create the CarrierAccountsApi
        /// </summary>
        /// <returns></returns>
        ICarrierAccountsApi CreateCarrierAccountsApi();
    }
}