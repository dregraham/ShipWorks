using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.ShipSense
{
    /// <summary>
    /// Interface for ShipSense Synchronizer
    /// </summary>
    public interface IShipSenseSynchronizer
    {
        /// <summary>
        /// Gets all of the shipments being monitored by the synchronizer.
        /// </summary>
        IEnumerable<ShipmentEntity> MonitoredShipments { get; }

        /// <summary>
        /// Gets all of the knowledge base entries being used to compare against the
        /// monitored shipments.
        /// </summary>
        IEnumerable<KnowledgebaseEntry> KnowledgebaseEntries { get; }

        /// <summary>
        /// Adds a collection of shipments to the list of shipments being synchronized.
        /// </summary>
        /// <param name="shipments">The shipments.  Shipment.Order, OrderItems, and OrderItemAttributes MUST be populated for this to work correctly.</param>
        void Add(IEnumerable<ShipmentEntity> shipments);

        /// <summary>
        /// Adds the specified shipment to the list of shipments being synchronized.
        /// </summary>
        /// <param name="shipment">The shipment.  Shipment.Order, OrderItems, and OrderItemAttributes MUST be populated for this to work correctly.</param>
        void Add(ShipmentEntity shipment);

        /// <summary>
        /// Refreshes the data in the synchronizer's knowledge base entries.
        /// </summary>
        void RefreshKnowledgebaseEntries();

        /// <summary>
        /// Removes the specified shipment from being synchronized.
        /// </summary>
        /// <param name="shipment">The shipment.  Shipment.Order, OrderItems, and OrderItemAttributes MUST be populated for this to work correctly.</param>
        void Remove(ShipmentEntity shipment);

        /// <summary>
        /// Synchronizes the ShipSense tracked values of any shipments matching the same 
        /// knowledge base entry as the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.  Shipment.Order, OrderItems, and OrderItemAttributes MUST be populated for this to work correctly.</param>
        void SynchronizeWith(ShipmentEntity shipment);
    }
}
