namespace ShipWorks.Shipping
{
    /// <summary>
    /// Result of a shipment update operation
    /// </summary>
    internal enum ShipmentUpdateOperationResult
    {
        /// <summary>
        /// The shipment was loaded
        /// </summary>
        Loaded,

        /// <summary>
        /// The shipment was deleted
        /// </summary>
        Deleted
    }
}