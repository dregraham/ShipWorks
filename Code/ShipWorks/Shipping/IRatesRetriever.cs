using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Retrieve rates for a shipment
    /// </summary>
    public interface IRatesRetriever
    {
        /// <summary>
        /// Get rates for the given shipment using the appropriate ShipmentType
        /// </summary>
        GenericResult<RateGroup> GetRates(ShipmentEntity shipment);

        /// <summary>
        /// Get rates for the given shipment using the appropriate ShipmentType
        /// </summary>
        GenericResult<RateGroup> GetRates(ShipmentEntity shipment, ShipmentType shipmentType);
    }
}
