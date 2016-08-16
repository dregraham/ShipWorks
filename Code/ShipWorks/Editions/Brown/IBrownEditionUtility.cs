using ShipWorks.Shipping;

namespace ShipWorks.Editions.Brown
{
    /// <summary>
    /// Utility functions for working with the brown-only edition
    /// </summary>
    public interface IBrownEditionUtility
    {
        /// <summary>
        /// Indicates if the shipment type is full allowed \ visible within brown-only edition
        /// </summary>
        bool IsShipmentTypeAllowed(ShipmentTypeCode shipmentType);
    }
}