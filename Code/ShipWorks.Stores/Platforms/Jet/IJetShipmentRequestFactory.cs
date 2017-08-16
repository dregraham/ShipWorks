using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet.DTO.Requests;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// represents a factory for creating jet shipment requests
    /// </summary>
    public interface IJetShipmentRequestFactory
    {
        /// <summary>
        /// Create a jet shipment request from a shipment
        /// </summary>
        JetShipmentRequest Create(ShipmentEntity shipment);
    }
}