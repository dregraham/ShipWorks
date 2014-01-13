using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// Helper class that groups all WorldShipProcessed entries for a shipment
    /// </summary>
    public class WorldShipProcessedGrouping
    {
        readonly long? shipmentID;
        List<WorldShipProcessedEntity> worldShipProcessedEntries;
        bool haveProcessedEntriesBeenSorted = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentID">The shipment for processing</param>
        /// <param name="worldShipProcessedEntries">The WorldShipProcessed entries to process</param>
        public WorldShipProcessedGrouping(long? shipmentID, List<WorldShipProcessedEntity> worldShipProcessedEntries)
        {
            this.shipmentID = shipmentID;
            this.worldShipProcessedEntries = worldShipProcessedEntries;
        }

        /// <summary>
        /// The ShipmentID for processing
        /// </summary>
        public long? ShipmentID
        {
            get 
            {
                return shipmentID; 
            }
        }

        /// <summary>
        /// The WorldShipProcessEntity list for processing.  
        /// The list is sorted by UpsPackageID (with nulls at the end), then by VoidIndicator, then by WorldShipProcessedID.
        /// </summary>
        public List<WorldShipProcessedEntity> OrderedWorldShipProcessedEntries
        {
            get
            {
                if (!haveProcessedEntriesBeenSorted && worldShipProcessedEntries != null && worldShipProcessedEntries.Any())
                {
                    // So that we pick the correct entry later, we want null UpsPackageIDs to be at the end of the list.  To do that, we
                    // first order desc by UpsPackageID having a value, then by it's actual value.  
                    worldShipProcessedEntries = worldShipProcessedEntries.OrderByDescending(worldShipProcessedEntry => !string.IsNullOrWhiteSpace(worldShipProcessedEntry.UpsPackageID))
                                                                         .ThenBy(worldShipProcessedEntry => worldShipProcessedEntry.UpsPackageID)
                                                                         .ThenBy(worldShipProcessedEntry => worldShipProcessedEntry.VoidIndicator)
                                                                         .ThenBy(worldShipProcessedEntry => worldShipProcessedEntry.WorldShipProcessedID).ToList();

                    // Fix any blank shipmentIDs
                    WorldShipUtility.FixInvalidShipmentIDs(worldShipProcessedEntries);

                    // Set the flag stating we've already sorted the list so we don't do it again
                    haveProcessedEntriesBeenSorted = true;
                }

                return worldShipProcessedEntries;
            }
        }
    }
}
