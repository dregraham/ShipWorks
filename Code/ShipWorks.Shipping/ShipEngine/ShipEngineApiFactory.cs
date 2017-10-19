using Interapptive.Shared.ComponentRegistration;
using ShipEngine.ApiClient.Api;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Factory for creating ShipEngine CarrierAccountsApi
    /// </summary>
    [Component]
    public class ShipEngineApiFactory : IShipEngineApiFactory
    {
        /// <summary>
        /// Create the CarrierAccountsApi
        /// </summary>
        public ICarrierAccountsApi CreateCarrierAccountsApi() => new CarrierAccountsApi();

        /// <summary>
        /// Create the CarrierApi
        /// </summary>
        public ICarriersApi CreateCarrierApi() => new CarriersApi();

        /// <summary>
        /// Create the RatesApi
        /// </summary>
        public IRatesApi CreateRatesApi() => new RatesApi();

        /// <summary>
        /// Create the Labels Api
        /// </summary>
        public ILabelsApi CreateLabelsApi() => new LabelsApi();
    }
}
