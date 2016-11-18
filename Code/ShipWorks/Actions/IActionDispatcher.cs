using System;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Anytime an action happens somewhere in ShipWorks it should tell the dispatcher about it,
    /// and the rest will be taken care of.
    /// </summary>
    public interface IActionDispatcher
    {
        /// <summary>
        /// Called when a batch of shipments has finished processing
        /// </summary>
        void DispatchProcessingBatchFinished(ISqlAdapter adapter, DateTime startingTime, int shipmentCount, int errorCount);

        /// <summary>
        /// Called each time a shipment has been successfully processed
        /// </summary>
        void DispatchShipmentProcessed(ShipmentEntity shipment, ISqlAdapter adapter);
    }
}
