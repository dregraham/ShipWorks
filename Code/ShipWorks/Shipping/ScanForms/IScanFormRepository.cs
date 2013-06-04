using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.ScanForms
{
    /// <summary>
    /// An interface for persisting a scan form entity to the database.
    /// </summary>
    public interface IScanFormRepository
    {       
        /// <summary>
        /// Saves the specified scan form batch.
        /// </summary>
        /// <param name="scanFormBatch">The scan form batch.</param>
        /// <returns>The ID value that is generated as a result saving the scan form.</returns>
        long Save(ScanFormBatch scanFormBatch);

        /// <summary>
        /// Gets the existing scan form batches for a carrier account.
        /// </summary>
        /// <param name="carrierAccount">The carrier account.</param>
        /// <returns>A collection of ScanFormBatch objects.</returns>
        IEnumerable<ScanFormBatch> GetExistingScanFormBatches(IScanFormCarrierAccount carrierAccount);

        /// <summary>
        /// Gets existing scan forms for a carrier.
        /// </summary>
        /// <param name="carrierAccount">The carrier account.</param>
        /// <returns>A collection of ScanForm objects.</returns>
        IEnumerable<ScanForm> GetExistingScanForms(IScanFormCarrierAccount carrierAccount);

        /// <summary>
        /// Gets the IDs of the shipments based on the given query predicate.
        /// </summary>
        /// <param name="bucket">The bucket containing the predicate for the query.</param>
        /// <returns>A collection of shipment ID values.</returns>
        IEnumerable<long> GetShipmentIDs(RelationPredicateBucket bucket);
    }
}
