using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.ShipSense
{
    public class ShipSenseSynchronizer
    {
        //private Dictionary<string, List<ShipmentEntity>> shipmentDictionary;
        //private Dictionary<string, KnowledgebaseEntry> knowledgebaseEntyDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseSynchronizer"/> class.
        /// </summary>
        /// <param name="shipments">The shipments.</param>
        public ShipSenseSynchronizer(IEnumerable<ShipmentEntity> shipments)
        {
            
        }

        /// <summary>
        /// Adds the specified shipment to the list of shipments being synchronized.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public void Add(ShipmentEntity shipment)
        {
            
        }

        /// <summary>
        /// Removes the specified shipment from being synchronized.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public void Remove(ShipmentEntity shipment)
        {
            
        }

        /// <summary>
        /// Synchronizes the ShipSense tracked values of any shipments matching the same 
        /// knowledge base entry as the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public void SynchronizeWith(ShipmentEntity shipment)
        {
            
        }
    }
}
