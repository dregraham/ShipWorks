using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services
{
    public interface IAutoReturnShipmentService
    {
        /// <summary>
        /// Applies the given return profile ID to the shipment
        /// </summary>
        void ApplyReturnProfile(ShipmentEntity shipment, long returnProfileID);

        /// <summary>
        /// Creates a new auto return shipments
        /// </summary>
        ShipmentEntity CreateReturn(ShipmentEntity shipment);
    }
}
