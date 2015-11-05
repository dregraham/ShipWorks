using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Enforce whether a label can be created through Amazon for a shipment
    /// </summary>
    public interface IAmazonLabelEnforcer
    {
        /// <summary>
        /// Is Amazon allowed for the given shipment
        /// </summary>
        EnforcementResult IsAllowed(ShipmentEntity shipment);

        /// <summary>
        /// Verify that the processed shipment is valid
        /// </summary>
        void VerifyShipment(ShipmentEntity shipment);
    }
}
