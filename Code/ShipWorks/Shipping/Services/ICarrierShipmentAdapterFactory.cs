using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Factory for creating carrier shipment adapters
    /// </summary>
    public interface ICarrierShipmentAdapterFactory
    {
        /// <summary>
        /// Get a carrier shipment adapter for the specified shipment, using the shipment type of the shipment
        /// </summary>
        ICarrierShipmentAdapter Get(ShipmentEntity shipment);
    }
}