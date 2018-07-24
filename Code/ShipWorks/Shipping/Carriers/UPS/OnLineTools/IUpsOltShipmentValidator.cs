using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// Interface for UpsOltShipmentValidator
    /// </summary>
    public interface IUpsOltShipmentValidator
    {
        /// <summary>
        /// Validates the shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        void ValidateShipment(ShipmentEntity shipment);
    }
}