using ShipWorks.Warehouse.DTO.Configuration.ShippingSettings;

namespace ShipWorks.Shipping.CarrierSetup
{
    /// <summary>
    /// Setup carriers configurations downloaded from the Hub
    /// </summary>
    public interface ICarrierSetup
    {
        /// <summary>
        /// Setup the carrier with the given config
        /// </summary>
        void Setup(CarrierConfigurationPayload config);
    }
}
