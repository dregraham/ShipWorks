using System;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Anytime an action happens somewhere in ShipWorks it should tell the dispatcher about it,
    /// and the rest will be taken care of.
    /// </summary>
    [Component]
    public class ActionDispatcherWrapper : IActionDispatcher
    {
        /// <summary>
        /// Called when a batch of shipments has finished processing
        /// </summary>
        public void DispatchProcessingBatchFinished(ISqlAdapter adapter, DateTime startingTime, int shipmentCount, int errorCount, string workflowName) =>
            ActionDispatcher.DispatchProcessingBatchFinished(adapter, startingTime, shipmentCount, errorCount, workflowName);

        /// <summary>
        /// Called each time a shipment has been successfully processed
        /// </summary>
        public void DispatchShipmentProcessed(IShipmentEntity shipment, ISqlAdapter adapter) =>
            ActionDispatcher.DispatchShipmentProcessed(shipment, adapter);
    }
}
