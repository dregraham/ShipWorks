using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Utility class for resolving what templates to print a shipment with
    /// </summary>
    [Component]
    public class ShipmentPrintHelperWrapper : IShipmentPrintHelper
    {
        /// <summary>
        /// Installs the default set of printing rules for the given shipment type.
        /// </summary>
        public void InstallDefaultRules(ShipmentTypeCode shipmentType) =>
            ShipmentPrintHelper.InstallDefaultRules(shipmentType, false, null, true);
    }
}
