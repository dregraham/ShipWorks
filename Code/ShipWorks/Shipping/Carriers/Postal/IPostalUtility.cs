namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Utility functions for dealing with USPS in general, not a specific USPS integration.
    /// </summary>
    public interface IPostalUtility
    {
        /// <summary>
        /// Indicates if the shipment type is a postal shipment
        /// </summary>
        bool IsPostalShipmentType(ShipmentTypeCode shipmentTypeCode);
    }
}