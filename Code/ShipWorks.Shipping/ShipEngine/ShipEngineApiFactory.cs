using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.ShipEngine.API;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Factory for creating ShipEngine CarrierAccountsApi
    /// </summary>
    [Component]
    public class ShipEngineApiFactory : IShipEngineApiFactory
    {
        private readonly string endpoint = "http://platform.shipengine.com/";

        /// <summary>
        /// Create the CarrierAccountsApi
        /// </summary>
        public ICarrierAccountsApi CreateCarrierAccountsApi() => new CarrierAccountsApi(endpoint);

        /// <summary>
        /// Create the CarrierApi
        /// </summary>
        public ICarriersApi CreateCarrierApi() => new CarriersApi(endpoint);

        /// <summary>
        /// Create the RatesApi
        /// </summary>
        public IRatesApi CreateRatesApi() => new RatesApi(endpoint);

        /// <summary>
        /// Create the Labels Api
        /// </summary>
        public ILabelsApi CreateLabelsApi() => new LabelsApi(endpoint);

        /// <summary>
        /// Create the Tracking API
        /// </summary>
        public ITrackingApi CreateTrackingApi() => new TrackingApi(endpoint);
    }
}
