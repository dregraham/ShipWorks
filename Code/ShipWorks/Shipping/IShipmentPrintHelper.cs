namespace ShipWorks.Shipping
{
    /// <summary>
    /// Utility class for resolving what templates to print a shipment with
    /// </summary>
    public interface IShipmentPrintHelper
    {
        /// <summary>
        /// Installs the default set of printing rules for the given shipment type.
        /// </summary>
        void InstallDefaultRules(ShipmentTypeCode shipmentType);
    }
}