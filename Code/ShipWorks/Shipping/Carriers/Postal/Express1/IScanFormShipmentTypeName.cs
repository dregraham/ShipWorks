namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Gets the dynamic name of the shipment type for ScanForms
    /// </summary>
    public interface IScanFormShipmentTypeName
    {
        /// <summary>
        /// Gets the name of the shipment type.
        /// </summary>
        string GetShipmentTypeName(ShipmentTypeCode shipmentTypeCode);
    }
}