using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.ScanForms
{
    /// <summary>
    /// An implementation of the IScanFormBatchShipmentRepository interface that abstracts the 
    /// underlying LLBLGen dependency from the ScanFormBatch domain object.
    /// </summary>
    public class DefaultScanFormBatchShipmentRepository : IScanFormBatchShipmentRepository
    {
        /// <summary>
        /// Gets the shipment count from a data source that a batch was been persisted to.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <returns>The total number of shipments associated with the batch.</returns>
        /// <exception cref="ShippingException">Thrown when the batch cannot be retrieved from the database.</exception>
        public int GetShipmentCount(ScanFormBatch batch)
        {
            if (batch == null)
            {
                throw new ArgumentNullException("batch");
            }

            try
            {
                ScanFormBatchEntity batchEntity = new ScanFormBatchEntity(batch.BatchId);
                SqlAdapter.Default.FetchEntity(batchEntity);

                return batchEntity.ShipmentCount;
            }
            catch (ORMEntityOutOfSyncException ex)
            {
                string message = string.Format("The scan form batch having batch ID {0} has been deleted.", batch.BatchId);
                throw new ShippingException(message, ex);
            }   
        }
    }
}
