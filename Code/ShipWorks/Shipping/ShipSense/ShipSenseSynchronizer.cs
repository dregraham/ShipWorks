using System.Collections.Generic;
using System.Linq;
using Quartz.Util;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipSense.Hashing;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Shipping.ShipSense
{
    public class ShipSenseSynchronizer
    {
        private readonly bool isShipSenseEnabled;
        private readonly string shipSenseUniquenessXml;

        private readonly IKnowledgebase knowledgebase;

        private readonly Dictionary<string, List<ShipmentEntity>> shipmentDictionary;
        private readonly Dictionary<string, KnowledgebaseEntry> knowledgebaseEntryDictionary;
        private readonly ShipSenseUniquenessXmlParser parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseSynchronizer" /> class.
        /// </summary>
        /// <param name="shipments">The shipments.  Shipment.Order, OrderItems, and OrderItemAttributes MUST be populated for this to work correctly.</param>
        public ShipSenseSynchronizer(IEnumerable<ShipmentEntity> shipments)
            : this(shipments, ShippingSettings.Fetch(), new Knowledgebase())
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseSynchronizer" /> class.
        /// </summary>
        /// <param name="shipments">The shipments.  Shipment.Order, OrderItems, and OrderItemAttributes MUST be populated for this to work correctly.</param>
        /// <param name="shippingSettings">The shipping settings.</param>
        /// <param name="knowledgebase">The knowledge base that entries should be retrieved from.</param>
        public ShipSenseSynchronizer(IEnumerable<ShipmentEntity> shipments, ShippingSettingsEntity shippingSettings, IKnowledgebase knowledgebase)
        {
            IEnumerable<ShipmentEntity> shipmentEntities = shipments as IList<ShipmentEntity> ?? shipments.ToList();

            this.knowledgebase = knowledgebase;
            isShipSenseEnabled = shippingSettings.ShipSenseEnabled;
            shipSenseUniquenessXml = shippingSettings.ShipSenseUniquenessXml;

            parser = new ShipSenseUniquenessXmlParser();

            shipmentDictionary = new Dictionary<string, List<ShipmentEntity>>();
            knowledgebaseEntryDictionary = new Dictionary<string, KnowledgebaseEntry>();

            // Add the shipments to the shipment dictionary; this will also create
            // the necessary KB entry dictionary items as well
            Add(shipmentEntities);
        }

        /// <summary>
        /// Gets all of the shipments being monitored by the synchronizer.
        /// </summary>
        public IEnumerable<ShipmentEntity> MonitoredShipments
        {
            get { return shipmentDictionary.SelectMany(d => d.Value); }
        }

        /// <summary>
        /// Gets all of the knowledge base entries being used to compare against the
        /// monitored shipments.
        /// </summary>
        public IEnumerable<KnowledgebaseEntry> KnowledgebaseEntries
        {
            get { return knowledgebaseEntryDictionary.Select(d => d.Value); }
        }

        /// <summary>
        /// Adds a collection of shipments to the list of shipments being synchronized.
        /// </summary>
        /// <param name="shipments">The shipments.  Shipment.Order, OrderItems, and OrderItemAttributes MUST be populated for this to work correctly.</param>
        public void Add(IEnumerable<ShipmentEntity> shipments)
        {
            foreach (ShipmentEntity shipment in shipments.Where(s => !s.DeletedFromDatabase))
            {
                Add(shipment);
            }
        }

        /// <summary>
        /// Adds the specified shipment to the list of shipments being synchronized.
        /// </summary>
        /// <param name="shipment">The shipment.  Shipment.Order, OrderItems, and OrderItemAttributes MUST be populated for this to work correctly.</param>
        public void Add(ShipmentEntity shipment)
        {
            if (shipment != null && !shipment.DeletedFromDatabase)
            {
                KnowledgebaseHashResult hashResult = GetHashResult(shipment);

                // Ignore shipments that have an invalid hash
                if (hashResult.IsValid)
                {
                    if (!shipmentDictionary.ContainsKey(hashResult.HashValue))
                    {
                        // We haven't seen this hash before, so we need to a new entry
                        shipmentDictionary[hashResult.HashValue] = new List<ShipmentEntity>();
                    }

                    // Remove any existing shipments that have the same shipment ID, so we don't have any duplicates
                    shipmentDictionary[hashResult.HashValue].RemoveAll(s => s.ShipmentID == shipment.ShipmentID);
                    shipmentDictionary[hashResult.HashValue].Add(shipment);

                    // We also need to create a corresponding entry in the KB entry dictionary
                    AddKnowledgebaseEntryToDictionary(hashResult.HashValue, shipment);
                }
            }
        }

        /// <summary>
        /// Refreshes the data in the synchronizer's knowledge base entries.
        /// </summary>
        public void RefreshKnowledgebaseEntries()
        {
            knowledgebaseEntryDictionary.Clear();
            foreach (string key in shipmentDictionary.Keys)
            {
                ShipmentEntity shipmentEntity = shipmentDictionary[key].FirstOrDefault();
                if (shipmentEntity != null && !shipmentEntity.DeletedFromDatabase)
                {
                    AddKnowledgebaseEntryToDictionary(key, shipmentEntity);
                }
            }
        }

        /// <summary>
        /// Removes the specified shipment from being synchronized.
        /// </summary>
        /// <param name="shipment">The shipment.  Shipment.Order, OrderItems, and OrderItemAttributes MUST be populated for this to work correctly.</param>
        public void Remove(ShipmentEntity shipment)
        {
            // Find the hash for the shipment and remove it from the corresponding 
            // dictionary entry's list
            KnowledgebaseHashResult hashResult = GetHashResult(shipment);
            if (hashResult.IsValid)
            {
                shipmentDictionary[hashResult.HashValue].Remove(shipment);
            }
        }

        /// <summary>
        /// Synchronizes the ShipSense tracked values of any shipments matching the same 
        /// knowledge base entry as the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.  Shipment.Order, OrderItems, and OrderItemAttributes MUST be populated for this to work correctly.</param>
        public void SynchronizeWith(ShipmentEntity shipment)
        {
            // Do some housekeeping and remove any shipments that have been processed, so we 
            // don't have to worry about excluding these later
            List<ShipmentEntity> processedOrDeletedShipments = MonitoredShipments.Where(s => s.Processed || s.DeletedFromDatabase).ToList();
            foreach (ShipmentEntity processedOrDeletedShipment in processedOrDeletedShipments)
            {
                Remove(processedOrDeletedShipment);
            }

            // This shipment has had ShipSense applied previously, so we need to update the status
            KnowledgebaseHashResult hashResult = GetHashResult(shipment);
            if (hashResult.IsValid)
            {
                if (shipment.ShipSenseStatus != (int)ShipSenseStatus.NotApplied)
                {
                    if (knowledgebaseEntryDictionary.ContainsKey(hashResult.HashValue))
                    {
                        // Consider a status of ShipSense applied if the shipment matches the corresponding KB entry
                        KnowledgebaseEntry entry = knowledgebaseEntryDictionary[hashResult.HashValue];
                        shipment.ShipSenseStatus = entry.Matches(shipment) ? (int)ShipSenseStatus.Applied : (int)ShipSenseStatus.Overwritten;
                    }
                }

                SynchronizeMatchingShipments(hashResult, shipment);
            }
        }

        /// <summary>
        /// Synchronizes the ShipSense data from the shipment provided to all of the shipments having 
        /// the same KB hash as the one provided (if ShipSense is enabled).
        /// </summary>
        /// <param name="hashResult">The hash result.</param>
        /// <param name="shipment">The shipment that the ShipSense data is being sourced from.</param>
        private void SynchronizeMatchingShipments(KnowledgebaseHashResult hashResult, ShipmentEntity shipment)
        {
            // Don't bother trying to synchronize if there are no shipments that match the specified hash.  This
            // scenario was causing a KeyNotFoundException
            if (isShipSenseEnabled && shipmentDictionary.ContainsKey(hashResult.HashValue))
            {
                // Create a knowledge base entry and populate it based on the shipment provided. We'll
                // use this to apply the ShipSense settings to any other shipments with the same hash
                ShipmentType sourceShipmentType = ShipmentTypeManager.GetType(shipment);
                KnowledgebaseEntry entry = new KnowledgebaseEntry();
                entry.ApplyFrom(sourceShipmentType.GetPackageAdapters(shipment), shipment.CustomsItems);

                foreach (ShipmentEntity matchedShipment in shipmentDictionary[hashResult.HashValue].Where(s => s.ShipmentID != shipment.ShipmentID))
                {
                    ShipmentType shipmentType = ShipmentTypeManager.GetType(matchedShipment);
                    IEnumerable<IPackageAdapter> packageAdapters = shipmentType.GetPackageAdapters(matchedShipment);

                    // We can't guarantee that package details will be the same if the counts do not match to 
                    // begin with for whatever reason
                    if (entry.Packages.Count() == packageAdapters.Count())
                    {
                        // Use the KB entry created above to simulate ShipSense being applied again
                        if (shipmentType.IsCustomsRequired(matchedShipment))
                        {
                            entry.ApplyTo(shipmentType.GetPackageAdapters(matchedShipment), matchedShipment.CustomsItems);
                        }
                        else
                        {
                            entry.ApplyTo(shipmentType.GetPackageAdapters(matchedShipment));
                        }

                        // Update the status of the matched shipment if needed
                        if (IsShipSenseApplied(matchedShipment))
                        {
                            // Consult the original entry to see if this shipment matches or not. We can't just use the status
                            // of the shipment that triggered the change since the triggering shipment may be an international
                            // shipment where the customs data makes it seen as overwritten but that data didn't impact the 
                            // configuration of a domestic shipment meaning the domestic shipment still matches the KB entry
                            // (e.g. the harmonized code of an international shipment changed which does not impact weight 
                            // or dimensions of a domestic shipment)
                            matchedShipment.ShipSenseStatus = MatchesOriginalEntry(matchedShipment, hashResult) ? 
                                (int)ShipSenseStatus.Applied : 
                                (int)ShipSenseStatus.Overwritten;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether the matched shipment is already in the knowledgebase
        /// </summary>
        private bool MatchesOriginalEntry(ShipmentEntity matchedShipment, KnowledgebaseHashResult hash)
        {
            return knowledgebaseEntryDictionary.ContainsKey(hash.HashValue) && 
                knowledgebaseEntryDictionary[hash.HashValue].Matches(matchedShipment);
        }

        /// <summary>
        /// Inspects the ShipSense status of the given shipment to determine whether ShipSense 
        /// has ever been applied.
        /// </summary>
        private bool IsShipSenseApplied(ShipmentEntity shipment)
        {
            return shipment.ShipSenseStatus != (int)ShipSenseStatus.NotApplied;
        }

        /// <summary>
        /// Gets the hash result based on the order items of the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A KnowledgebaseHashResult object.</returns>
        private KnowledgebaseHashResult GetHashResult(ShipmentEntity shipment)
        {
            IEnumerable<ShipSenseOrderItemKey> keys = knowledgebase.KeyFactory.GetKeys(shipment.Order.OrderItems, parser.GetItemProperties(shipSenseUniquenessXml), parser.GetItemAttributes(shipSenseUniquenessXml));
            KnowledgebaseHashResult hashResult = knowledgebase.HashingStrategy.ComputeHash(keys);

            return hashResult;
        }

        /// <summary>
        /// Adds a knowledge base entry to dictionary.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="shipment">The shipment.  Shipment.Order, OrderItems, and OrderItemAttributes MUST be populated for this to work correctly.</param>
        private void AddKnowledgebaseEntryToDictionary(string key, ShipmentEntity shipment)
        {
            // Don't add an entry if there is already a dictionary item for the given key
            if (shipment != null && !knowledgebaseEntryDictionary.ContainsKey(key))
            {
                // The knowledge base entries need to be fetched from the knowledge base; we can't be slick
                // and use the shipment entities to populate the KB entries here because the shipment 
                // entities may reflect different ShipSense values due to a shipping profile being applied
                OrderEntity order = shipment.Order;

                KnowledgebaseEntry entry = knowledgebase.GetEntry(order);
                if (entry != null && entry.Packages.Any())
                {
                    // Only add the entry if a valid ShipSense record was found
                    knowledgebaseEntryDictionary[key] = knowledgebase.GetEntry(order);
                }
            }
        }
    }
}
