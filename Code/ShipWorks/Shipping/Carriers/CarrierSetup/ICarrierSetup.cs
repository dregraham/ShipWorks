using ShipWorks.Warehouse.DTO.Configuration.ShippingSettings;

namespace ShipWorks.Shipping.CarrierSetup
{
    interface ICarrierSetup
    {
        void Setup(CarrierConfigurationPayload config);
    }
}
