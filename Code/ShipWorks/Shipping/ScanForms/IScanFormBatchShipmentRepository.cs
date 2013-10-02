namespace ShipWorks.Shipping.ScanForms
{
    public interface IScanFormBatchShipmentRepository
    {
        /// <summary>
        /// Gets the shipment count from a data source that a batch was been persisted to.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <returns>The total number of shipments associated with the batch.</returns>
        int GetShipmentCount(ScanFormBatch batch);
    }
}
