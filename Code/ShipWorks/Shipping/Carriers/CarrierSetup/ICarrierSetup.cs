using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;

namespace ShipWorks.Shipping.CarrierSetup
{
    /// <summary>
    /// Setup carrier configurations downloaded from the Hub
    /// </summary>
    public interface ICarrierSetup
    {
        /// <summary>
        /// Setup the carrier with the given config
        /// </summary>
        void Setup(CarrierConfiguration config);
    }
}
