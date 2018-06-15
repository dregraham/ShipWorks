using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.ShipSense
{
    /// <summary>
    /// No-op implementation of IShipSenseSynchronizer
    /// </summary>
    [Component(RegistrationType.Self)]
    public class NullShipSenseSynchronizer : IShipSenseSynchronizer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NullShipSenseSynchronizer()
        {
            MonitoredShipments = Enumerable.Empty<ShipmentEntity>();
            KnowledgebaseEntries = Enumerable.Empty<KnowledgebaseEntry>();
        }

        /// <summary>
        /// Gets all of the shipments being monitored by the synchronizer.
        /// </summary>
        public IEnumerable<ShipmentEntity> MonitoredShipments { get; }

        /// <summary>
        /// Gets all of the knowledge base entries being used to compare against the
        /// monitored shipments.
        /// </summary>
        public IEnumerable<KnowledgebaseEntry> KnowledgebaseEntries { get; }

        /// <summary>
        /// Adds a collection of shipments to the list of shipments being synchronized.
        /// </summary>
        /// <param name="shipments">The shipments.  Shipment.Order, OrderItems, and OrderItemAttributes MUST be populated for this to work correctly.</param>
        public void Add(IEnumerable<ShipmentEntity> shipments)
        {
        }

        /// <summary>
        /// Adds the specified shipment to the list of shipments being synchronized.
        /// </summary>
        /// <param name="shipment">The shipment.  Shipment.Order, OrderItems, and OrderItemAttributes MUST be populated for this to work correctly.</param>
        public void Add(ShipmentEntity shipment)
        {
        }

        /// <summary>
        /// Refreshes the data in the synchronizer's knowledge base entries.
        /// </summary>
        public void RefreshKnowledgebaseEntries()
        {
        }

        /// <summary>
        /// Removes the specified shipment from being synchronized.
        /// </summary>
        /// <param name="shipment">The shipment.  Shipment.Order, OrderItems, and OrderItemAttributes MUST be populated for this to work correctly.</param>
        public void Remove(ShipmentEntity shipment)
        {
        }

        /// <summary>
        /// Synchronizes the ShipSense tracked values of any shipments matching the same 
        /// knowledge base entry as the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.  Shipment.Order, OrderItems, and OrderItemAttributes MUST be populated for this to work correctly.</param>
        public void SynchronizeWith(ShipmentEntity shipment)
        {
        }
    }
}
