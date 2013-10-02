using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

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
        public int GetShipmentCount(ScanFormBatch batch)
        {
            if (batch == null)
            {
                throw new ArgumentNullException("batch");
            }

            ScanFormBatchEntity batchEntity = new ScanFormBatchEntity(batch.BatchId);
            SqlAdapter.Default.FetchEntity(batchEntity);

            return batchEntity.ShipmentCount;
        }
    }
}
