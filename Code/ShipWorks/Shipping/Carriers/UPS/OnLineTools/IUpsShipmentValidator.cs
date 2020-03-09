using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// Validates UPS shipments
    /// </summary>
    public interface IUpsShipmentValidator
    {
        /// <summary>
        /// Validates the shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        Result ValidateShipment(ShipmentEntity shipment);
    }
}
